using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarCardGameConsole.Models;

namespace WarCardGameConsole.Controllers
{
    public class DeckController
    {
        //properties
        public Queue<CardModel> Cards { get; set; }


        //constructors
        public DeckController()
        {
            Cards = new Queue<CardModel>();
        }

        //card methods

        //Given another DeckController, move all Cards to that controller
        //This is used to move Claims piles into a Deck
        public bool MoveCards(DeckController target)
        {
            try
            {
                //move cards over to new controller
                target.AddCards(Cards);

                //reinit cards in this deck
                Cards = new Queue<CardModel>();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        //The actual method that enqueues cards into a target deck
        public bool AddCards(Queue<CardModel> cards)
        {
            try
            {
                foreach(var c in cards)
                {
                    Cards.Enqueue(c);
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //A method to request a card from the deck
        //The method returns True if a card can be drawn, and outputs a CardModel of the drawn card
        //The method returns False if a card cannot be drawn (the deck and claims are empty), and outputs a null CardModel
        public bool TryDrawCard(out CardModel outCard, DeckController claims)
        {
            //Sense number of cards left in deck
            int numDeck = Cards.Count;
            if(numDeck < 1)
            {
                //no cards are left, so try to shuffle claims pile into the deck
                claims.MoveCards(this);
                //now check number of cards again
                numDeck = Cards.Count;
                if(numDeck < 1)
                {
                    //this actor has no more cards left
                    outCard = null;
                    return false;
                }
            }
            outCard = Cards.Dequeue();
            return true;
        }

        //A method to randomize the order of cards in a deck
        public void Shuffle()
        {
            var cardsToShuffle = new List<CardModel>(Cards);
            Cards = new Queue<CardModel>();
            int numberOfCards = cardsToShuffle.Count;
            Random rand = new Random();

            //may need a special case to remove the last card in the deck
            for(int i=numberOfCards-1; i>=0; i--)
            {
                //pick a random card
                int index = rand.Next(0, i + 1);

                //pull card out of "deck"
                CardModel card = cardsToShuffle[index];
                cardsToShuffle.RemoveAt(index);

                //put it into the new deck
                Cards.Enqueue(card);
            }

            //cards should now be shuffled
        }
    }
}
