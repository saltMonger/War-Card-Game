using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarCardGame.Models;

namespace WarCardGame.Controllers
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

        public CardModel DrawCard()
        {
            return Cards.Dequeue();
        }

        public void Shuffle()
        {
            var cardsToShuffle = new List<CardModel>(Cards);
            Cards = new Queue<CardModel>();
            int numberOfCards = cardsToShuffle.Count;
            Random rand = new Random();

            for(int i=0; i<numberOfCards; i++)
            {
               int index = rand.Next(0, numberOfCards + 1);
            }
        }
    }
}
