using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        [SerializeField]
        private Text progressLabelText;

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        bool isConnecting;
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";



        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            if(!PlayerPrefs.HasKey("RoomName"))
            {
                PlayerPrefs.SetString("RoomName", "RIKKEN");
            }
            if (!PlayerPrefs.HasKey("Mic"))
            {
                PlayerPrefs.SetString("Mic", Microphone.devices[0]);
                PlayerPrefs.SetInt("MicID", 0);
            }
        }

        private void Update()
        {
            if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            {
                progressLabelText.text  = PhotonNetwork.CurrentRoom.PlayerCount +  "/4 spelers verbonden";

                if(PhotonNetwork.CurrentRoom.PlayerCount == 4)
                {
                    SceneManager.LoadScene("Game");
                }
            }
        }



        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(PlayerPrefs.GetString("RoomName"), new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");
            }
        }

        public override void OnConnected()
        {
            base.OnConnected();
        }

        public void ShowSettings()
        {
            SceneManager.LoadScene("Settings");
        }
        public void Quit()
        {
            Application.Quit();
        }


    }
}