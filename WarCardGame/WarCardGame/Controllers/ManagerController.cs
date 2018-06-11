using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarCardGame.Models;

namespace WarCardGame.Controllers
{
    public class ManagerController
    {
        public DeckController PlayerDeck { get; set; }
        public DeckController ComputerDeck { get; set; }
        public DeckController PlayerClaims { get; set; }
        public DeckController ComputerClaims { get; set; }
        private static ManagerController _instance = null;


        public static ManagerController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ManagerController();
                }
                return _instance;
            }
        }

        public void Init()
        {
            PlayerDeck = new DeckController();
            PlayerClaims = new DeckController();
            ComputerDeck = new DeckController();
            ComputerClaims = new DeckController();
            DealerController dc = new DealerController();
            dc.FillDeck();
            dc.Shuffle();
            dc.Deal(PlayerDeck, ComputerDeck);
        }

        //the computer draws immediately and rounds are single turn only (all parties act simultaneously),
        //so wait for the player to press the DRAW button
        public void TakeTurn()
        {

            //update gfx here
            WarOutcome outcome = WarOutcome.NONE;
            do
            {
                //TODO: Extend the check of draw cards to pull claims pile, as well as return a tuple (or have an output type, like TryDrawCard();
                CardModel playerCard = null;
                CardModel computerCard = null;
                try
                {
                    
                    playerCard = PlayerDeck.DrawCard();
                }
                catch(InvalidOperationException ex)
                {

                }

                try
                {
                    computerCard = ComputerDeck.DrawCard();

                }
                catch (InvalidOperationException ex)
                {

                }



                outcome = CompareController.Instance.CompareCards(playerCard, computerCard);
            } while (outcome == WarOutcome.WAR);

            switch (outcome)
            {
                case WarOutcome.WIN:
                    {
                        //get the winnings
                        var claims = CompareController.Instance.HistoryToDeck();
                        //clear history on card comparer
                        CompareController.Instance.ClearHistory();
                        //award the winnings to the player
                        PlayerClaims.AddCards(claims);

                        break;
                    }
                case WarOutcome.LOSE:
                    {
                        //get the winnings
                        var claims = CompareController.Instance.HistoryToDeck();
                        //clear history on card comparer
                        CompareController.Instance.ClearHistory();
                        //award the winnings to the computer
                        ComputerClaims.AddCards(claims);

                        break;
                    }
            }
        }
    }
}
