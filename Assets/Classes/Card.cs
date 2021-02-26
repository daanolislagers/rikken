using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rikken
{
    public class Card : IComparable<Card>
    {
        private CardSuit suit;
        private CardFace face;

        public Card(CardSuit suit, CardFace face)
        {
            this.suit = suit;
            this.face = face;
        }

        public CardSuit Suit { get => suit; set => suit = value; }
        public CardFace Face { get => face; set => face = value; }

        public int CompareTo(Card other)
        {
            int i = suit.CompareTo(other.suit);
            if (i != 0) return i;

            return face.CompareTo(other.face);
        }

        public String PrintCard()
        {
            return suit.ToString().Substring(0, 1) +  face.ToString();
        }

        public static Card fromString(string cardString)
        {
            CardSuit suit;
            string suitString = cardString.Substring(0, 1);
            switch(suitString)
            {
                case "H":
                    suit = CardSuit.Hearts;
                    break;
                case "C":
                    suit = CardSuit.Cloves;
                    break;
                case "D":
                    suit = CardSuit.Diamonds;
                    break;
                default:
                    suit = CardSuit.Spades;
                    break;
            }

            
            CardFace face = (CardFace) Enum.Parse(typeof(CardFace), cardString.Substring(1));

            return new Card(suit, face);
        }


    }

    public enum CardSuit
    {
        Hearts, Cloves, Diamonds, Spades
    }

    public enum CardFace
    {
        Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack, Queen, King, Ace
    }


}
