using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rikken
{
    public class Trick
    {
        private List<Card> cards;
        private LocalPlayer[] players;
        private CardSuit trump;
        private bool usesTrump;

        internal List<Card> Cards { get => cards; set => cards = value; }
        public CardSuit Trump { get => trump; set => trump = value; }
        public bool UsesTrump { get => usesTrump; set => usesTrump = value; }

        public Trick()
        {
            usesTrump = false;
            Cards = new List<Card>();
            players = new LocalPlayer[4];
        }
        public Trick(CardSuit trump)
        {
            usesTrump = true;
            this.trump = trump;
            players = new LocalPlayer[4];
            Cards = new List<Card>();
        }

        public void AddCard(Card card, LocalPlayer player)
        {
            players[Cards.Count] = player;
            Cards.Add(card);
            player.Cards.Remove(card);

            String cardString = card.PrintCard();
            GameObject cardObject = GameObject.Find(card.PrintCard());
            cardObject.transform.parent = GameObject.Find("Player" + player.Index + "Card").transform;
        }

        public bool IsComplete()
        {
            return Cards.Count() == 4;
        }

        public bool HasPlayed(LocalPlayer player)
        {
            return players.Contains(player);
        }

        public LocalPlayer PickWinner()
        {
            Card[] temp = new Card[4];
            CardSuit start = Cards[0].Suit;
            Cards.CopyTo(temp, 0);
            Array.Sort(temp);
            //Array.Reverse(temp);
            Card winner = temp[0];

            for (int i = 0; i < 4; i++)
            {
                Card card = temp[i];
                if (card.Suit == start)
                {
                    winner = card;
                }
            }
            if (usesTrump)
            {
                //Array.Reverse(temp);
                for (int i = 0; i < 4; i++)
                {
                    Card card = temp[i];
                    if (card.Suit == trump)
                    {
                        winner = card;
                    }
                }
            }
            int index = Cards.IndexOf(winner);



            return players[index];
        }

    }
}
