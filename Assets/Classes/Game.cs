using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rikken
{
    public class Game
    {
        private GameManager manager;
        private LocalPlayer[] players;
        private Stack<Card> cardStack;
        private Stack<Trick> trickStack;
        private Card[] fullDeck;
        private Random rnd;
        private Trick currentTrick;
        private int dealTurn = 0;

        private CardSuit trump;
        private bool usesTrump = false;

        public Game(LocalPlayer[] players, GameManager manager)
        {
            this.Players = players;
            cardStack = new Stack<Card>();
            FullDeck = new Card[52];
            trickStack = new Stack<Trick>();

            //int seed = (int) Math.Ceiling( (double) new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() / 100000) * 100000;
            //rnd = new Random(seed);
            rnd = new Random();

            this.manager = manager;

            GenerateStack();
            //Deal();
        }

        public void SetRandomSeed(int seed)
        {
            this.rnd = new Random(seed);
        }

        internal LocalPlayer[] Players { get => players; set => players = value; }
        internal Trick CurrentTrick { get => currentTrick; set => currentTrick = value; }
        internal Card[] FullDeck { get => fullDeck; set => fullDeck = value; }
        public int DealTurn { get => dealTurn; set => dealTurn = value; }
        public CardSuit Trump { get => trump; set => trump = value; }
        public bool UsesTrump { get => usesTrump; set => usesTrump = value; }

        public void PlayCard(Card card, LocalPlayer player)
        {
            CurrentTrick.AddCard(card, player);
        }

        public void AddTrick()
        {
            if(UsesTrump)
                CurrentTrick = new Trick(trump);
            else
                CurrentTrick = new Trick();
            trickStack.Push(currentTrick);
        }

        public void Deal()
        {
            LocalPlayer[] temp = new LocalPlayer[4];
            players.CopyTo(temp,0);
            LeftShiftArray(temp, DealTurn);

            foreach (LocalPlayer player in Players)
            {
                for(int i = 1; i <= 6; i++)
                {
                    Card card = cardStack.Pop();
                    player.AddCard(card);
                    manager.photonView.RPC("givePlayerCard", Photon.Pun.RpcTarget.Others, player.Index-1, card.PrintCard());
                }
            }
            foreach (LocalPlayer player in Players)
            {
                for (int i = 1; i <= 7; i++)
                {
                    Card card = cardStack.Pop();
                    player.AddCard(card);
                    manager.photonView.RPC("givePlayerCard", Photon.Pun.RpcTarget.Others, player.Index-1, card.PrintCard());
                }
                player.SortCards();
            }
            DealTurn++;
            if(DealTurn == 4)
            {
                DealTurn = 0;
            }

            AddTrick();
        }

        public static void LeftShiftArray<T>(T[] arr, int shift)
        {
            shift = shift % arr.Length;
            T[] buffer = new T[shift];
            Array.Copy(arr, buffer, shift);
            Array.Copy(arr, shift, arr, 0, arr.Length - shift);
            Array.Copy(buffer, 0, arr, arr.Length - shift, shift);
        }

        private void GenerateStack()
        {
            int i = 0;
            foreach(CardSuit suit in Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>())
            {
                foreach (CardFace face in Enum.GetValues(typeof(CardFace)).Cast<CardFace>())
                {
                    Card card = new Card(suit, face);
                    cardStack.Push(card);
                    FullDeck[i] = card;
                    i++;
                }
            }

            Shuffle(cardStack);
        }

        private void Shuffle(Stack<Card> stack)
        {
            var values = stack.ToArray();
            stack.Clear();
            foreach (var value in values.OrderBy(x => rnd.Next()))
                stack.Push(value);
        }

        private void ShuffleStack()
        {
            int split = rnd.Next(15, 40);
            var values = cardStack.ToArray();
            var first = values.Take(split).ToArray();
            var second = values.Skip(split).ToArray();
            cardStack.Clear();
            foreach (var value in second)
                cardStack.Push(value);
            foreach (var value in first)
                cardStack.Push(value);
        }

        public void Reset()
        {
            foreach(Trick trick in trickStack)
            {
                foreach(Card card in trick.Cards)
                {
                    cardStack.Push(card);
                }
            }
            foreach(LocalPlayer player in Players)
            {
                foreach(Card card in player.Cards)
                {
                    cardStack.Push(card);
                }
                player.ResetCards();
            }
            trickStack = new Stack<Trick>();
            
            ShuffleStack();
            //Deal();
        }

        public bool IsOver()
        {
            return trickStack.Count == 13;
        }

    }
}
