using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rikken;
using System;

using Photon.Pun;
using Photon.Realtime;
using Random = System.Random;
using UnityEngine.UI;
using Photon.Voice.PUN;

public class GameManager : MonoBehaviourPunCallbacks
{

    private Game game;
    private LocalPlayer[] players;
    private LocalPlayer winner;
    private int turn = 1;

    public GameObject startButton;
    public GameObject trumpDown;
    public GameObject dealButton;

    private bool gameHasStarted = false;

    private int currentPlayer = 0;

    public CameraHandler cameraHandler;

    public PhotonVoiceNetwork voice;

    public GameObject playerPrefab;

    internal Game Game { get => game; set => game = value; }
    public int CurrentPlayer { get => currentPlayer; }
    public bool GameHasStarted { get => gameHasStarted; set => gameHasStarted = value; }

    // Start is called before the first frame update
    void Start()
    {
        /*dealButton = GameObject.Find("DealButton");
        startButton = GameObject.Find("StartRoundButton");
        trumpDown = GameObject.Find("DropdownTrump");*/


        Screen.sleepTimeout = SleepTimeout.NeverSleep;


        List<LocalPlayer> tempplayers = new List<LocalPlayer>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            tempplayers.Add(new LocalPlayer(player.NickName, player.ActorNumber, player));

            GameObject.Find("Player" + player.ActorNumber + "NameText").GetComponent<Text>().text = player.NickName;

            //if (PhotonNetwork.IsMasterClient)
            //{
            //PhotonView view = GameObject.Find("Player" + player.ActorNumber + "Chair").GetComponent<PhotonView>();
            //view.TransferOwnership(player);
            //}

            PhotonNetwork.Instantiate(playerPrefab.name, GameObject.Find("Player" + player.ActorNumber + "Chair").transform.position, Quaternion.identity, 0);

            if (player.IsLocal)
            {
                SetCurrentPlayer(player.ActorNumber);
                GameObject.Find("Player" + player.ActorNumber + "NameText").SetActive(false);
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
            trumpDown.SetActive(false);
        }

        players = tempplayers.ToArray();

        game = new Game(players, this);

        voice.PrimaryRecorder.UnityMicrophoneDevice = PlayerPrefs.GetString("Mic");
        voice.ConnectAndJoinRoom();
    }

    public void SetCurrentPlayer(int current)
    {
        this.currentPlayer = current;
        cameraHandler.SetPlayer(current);
    }


    [PunRPC]
    public void CheckGameAfterCardPlay()
    {
        turn++;
        if (turn == 4)
            turn = 0;

        ResetNamesForTurn();
        SetNameForTurn(turn + 1);

        if (game.CurrentTrick.IsComplete())
        {
            ResetNamesForTurn();
            winner = game.CurrentTrick.PickWinner();
            Invoke("SetWinner", 3.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                this.photonView.RPC("ForceQuit", RpcTarget.Others);
            }
            Application.Quit();
        }
    }

    [PunRPC]
    public void ForceQuit()
    {
        Application.Quit();
    }

    void SetWinner()
    {
        foreach (Card card in game.CurrentTrick.Cards)
        {
            String cardString = card.PrintCard();
            GameObject cardObject = GameObject.Find(card.PrintCard());
            cardObject.transform.parent = GameObject.Find("Player" + winner.Index + "TrickStack").transform;
        }
        if (game.IsOver())
        {
            ResetNamesForTurn();
            Invoke("ResetGame", 3f);
        }
        else
        {
            turn = winner.Index - 1;
            game.AddTrick();
            ResetNamesForTurn();
            SetNameForTurn(turn + 1);
        }
    }


    public void DoReset()
    {
        this.photonView.RPC("ResetGame", RpcTarget.All);
    }

    [PunRPC]
    public void ResetGame()
    {
        gameHasStarted = false;
        game.UsesTrump = false;
        if (PhotonNetwork.IsMasterClient)
        {
            trumpDown.SetActive(false);
            startButton.SetActive(false);
            dealButton.SetActive(false);
            ((Dropdown)trumpDown.GetComponent(typeof(Dropdown))).value = 0;
        }
        //all cards to center
        foreach (Card card in game.FullDeck)
        {
            String cardString = card.PrintCard();
            GameObject cardObject = GameObject.Find(card.PrintCard());
            cardObject.transform.parent = GameObject.Find("CardStack").transform;
        }

        ResetNamesForTurn();

        GameObject.Find("DealerChip").transform.parent = GameObject.Find("Player" + (game.DealTurn + 1) + "DealerChip").transform;
        Invoke("Reshuffle", 3f);

    }

    public void ResetNamesForTurn()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (i != currentPlayer)
            {
                GameObject.Find("Player" + i + "NameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
                GameObject.Find("Player" + i + "NameText").GetComponent<Text>().color = Color.white;
            }
        }
    }

    public void SetNameForTurn(int player)
    {
        if (player != currentPlayer)
        {
            GameObject.Find("Player" + player + "NameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
            GameObject.Find("Player" + player + "NameText").GetComponent<Text>().color = Color.red;
        }
    }


    private void Reshuffle()
    {
        game.Reset();
        this.turn = game.DealTurn + 1;
        if (turn == 4)
            turn = 0;
        if (PhotonNetwork.IsMasterClient)
        {
            trumpDown.SetActive(false);
            startButton.SetActive(false);
            dealButton.SetActive(true);
        }
    }

    public void StartDealing()
    {
        game.Deal();
        if (PhotonNetwork.IsMasterClient)
        {
            trumpDown.SetActive(true);
            startButton.SetActive(true);
            dealButton.SetActive(false);
        }
        this.photonView.RPC("DealGame", RpcTarget.Others, null);
        //DealGame();
    }

    [PunRPC]
    public void DealGame()
    {
        
        game.DealTurn++;
        if (game.DealTurn == 4)
        {
            game.DealTurn = 0;
        }

        game.AddTrick();
    }

    public void PlayerPlayCard(int index)
    {
        if (turn == currentPlayer - 1)
        {
            LocalPlayer player = game.Players[turn];
            this.photonView.RPC("PlayerPlaysCard", RpcTarget.Others, index, turn);
            PlayerPlaysCard(index, turn);
        }
    }

    [PunRPC]
    public void PlayerPlaysCard(int cardIndex, int playerIndex)
    {
        LocalPlayer player = game.Players[playerIndex];
        game.PlayCard(player.Cards[cardIndex], player);
        CheckGameAfterCardPlay();
    }

    public void StartGame()
    {
        this.photonView.RPC("StartRound", RpcTarget.Others);
        StartRound();
    }

    [PunRPC]
    public void StartRound()
    {
        gameHasStarted = true;
        if (PhotonNetwork.IsMasterClient)
        {
            trumpDown.SetActive(false);
            startButton.SetActive(false);
            dealButton.SetActive(false);
        }
        SetNameForTurn(turn + 1);
    }

    public void setTrump(CardSuit suit, bool usesTrump)
    {
        photonView.RPC("setTrumpAll", RpcTarget.Others, suit, usesTrump);

        setTrumpAll(suit, usesTrump);
    }



    [PunRPC]
    public void setTrumpAll(CardSuit suit, bool usesTrump)
    {
        game.CurrentTrick.Trump = suit;
        game.CurrentTrick.UsesTrump = usesTrump;
        game.Trump = suit;
        game.UsesTrump = usesTrump;
    }


    [PunRPC]
    public void givePlayerCard(int player, String card)
    {
        LocalPlayer playerObj = this.game.Players[player];
        playerObj.AddCard(Card.fromString(card));
        if(playerObj.Cards.Count == 13)
            playerObj.SortCards();
    }

}