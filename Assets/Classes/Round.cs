using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rikken
{
    abstract class Round
    {

        protected Stack<Trick> trickStack;
        protected CardSuit trump;
        protected LocalPlayer leader;
        protected LocalPlayer mate;

        protected int turn = 0;

        protected Trick currentTrick;

        protected int minRounds;
        protected int maxRounds;
        protected bool hasMate;
        protected bool usesTrump;

        protected int winScore;
        protected int lossScore;
        protected int overshootScore;

        public Round(LocalPlayer leader)
        {
            trickStack = new Stack<Trick>();
            this.leader = leader;
        }
        public Round(LocalPlayer leader, LocalPlayer mate)
        {
            trickStack = new Stack<Trick>();
            this.leader = leader;
            this.mate = mate;
        }

        public CardSuit Trump { get => trump; set => trump = value; }

        public void NextTrick()
        {
            Trick trick = new Trick();
            trickStack.Push(trick);
            currentTrick = trick;
        }

        public void PlayCard(Card card, LocalPlayer player)
        {
            currentTrick.AddCard(card, player);
        }

        public bool IsComplete()
        {
            return trickStack.Count == 13;
        }

        public bool IsSuccess()
        {
            int numTricks = 0;

            foreach(Trick trick in trickStack)
            {
                LocalPlayer winner = trick.PickWinner();
                if (winner == leader || (hasMate && winner == mate))
                    numTricks++;
            }

            if (numTricks >= minRounds && numTricks <= maxRounds)
                return true;
            else
                return false;
        }

    }
}
