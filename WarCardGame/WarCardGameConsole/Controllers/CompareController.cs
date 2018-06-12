using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarCardGameConsole.Models;

namespace WarCardGameConsole.Controllers
{
    public enum WarOutcome
    {
        WIN,
        LOSE,
        WAR,
        NONE
    }

    public class CompareController
    {
        public List<HistoryModel> History { get; set; }

        private static CompareController _instance = null;


        private CompareController()
        {
            History = new List<HistoryModel>();
        }

        public static CompareController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CompareController();
                }
                return _instance;
            }
        }



        public WarOutcome CompareCards(CardModel playerCard, CardModel computerCard)
        {
            History.Add(new HistoryModel
            {
                PlayerCard = playerCard,
                ComputerCard = computerCard
            });

            //compare the rank of the cards
            if (playerCard.CardRank > computerCard.CardRank)
            {
                return WarOutcome.WIN; //the war wins, in the player's favor
            }
            else if(playerCard.CardRank < computerCard.CardRank)
            {
                return WarOutcome.LOSE; //the player loses the war
            }
            else
            {
                return WarOutcome.WAR; //cards are equivalent, so start a war.
            }
        }

        public Queue<CardModel> HistoryToDeck()
        {
            Queue<CardModel> claims = new Queue<CardModel>();
            foreach(var h in History)
            {
                claims.Enqueue(h.PlayerCard);
                claims.Enqueue(h.ComputerCard);
            }

            return claims;
        }

        public void ClearHistory()
        {
            History = new List<HistoryModel>();
        }
    }
}
