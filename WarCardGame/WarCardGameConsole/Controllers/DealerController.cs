using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarCardGameConsole.Models;

namespace WarCardGameConsole.Controllers
{
    public class DealerController : DeckController
    {

        //this will fill the deck with all the cards in a 52 card deck
        //this information is packed in a text file stored in Assets
        public void FillDeck()
        {
            using (StreamReader sr = new StreamReader("Assets/cards.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string str = sr.ReadLine();
                    string[] cardData = str.Split(',');
                    Cards.Enqueue(new CardModel()
                    {
                        CardId = cardData[2].Replace("_"," "),
                        CardRank = Int32.Parse(cardData[1])
                    });
                }
            }
        }

        //this "deals" a deck of cards to a player and a computer, alternating the receipient of each new card
        public void Deal(DeckController playerDeck, DeckController computerDeck)
        {
            for(int i=0; i<52; i++)
            {
                if(i % 2 == 0)
                {
                    playerDeck.Cards.Enqueue(Cards.Dequeue());
                }
                else
                {
                    computerDeck.Cards.Enqueue(Cards.Dequeue());
                }
            }
        }

    }
}
