using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Rikken
{
    public class LocalPlayer
    {
        String name;
        List<Card> cards;
        Player photonPlayer;
        int score;
        int index;


        public LocalPlayer(String name, int index, Player photonPlayer)
        {
            this.name = name;
            this.index = index;
            this.cards = new List<Card>();
            this.photonPlayer = photonPlayer;
        }

        public void AddScore(int score) => this.score += score;

        public int Score { get => score; set => score = value; }
        public string Name { get => name; }
        internal List<Card> Cards { get => cards; }
        public int Index { get => index; }

        public void AddCard(Card card)
        {
            this.cards.Add(card);
        }

        public void SortCards()
        {
            cards.Sort();

            foreach(Card card in cards)
            {
                String cardString = card.PrintCard();
                GameObject cardObject = GameObject.Find(card.PrintCard());
                cardObject.transform.parent = GameObject.Find("Player" + index + "Hand").transform;
            }

        }

        public bool canCommit(CardSuit suit)
        {
            foreach (Card card in cards)
                if (card.Suit == suit)
                    return true;
            return false;
        }

        public void ResetCards()
        {
            this.cards = new List<Card>();
        }
    }
}
