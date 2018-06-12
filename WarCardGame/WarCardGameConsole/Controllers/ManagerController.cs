using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarCardGameConsole.Models;

namespace WarCardGameConsole.Controllers
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

        //Initialize method for setting up player and computer decks and claims piles
        //Also sets up a "Dealer Deck" with all 52 cards, and deal them to each player (alternate between adding a card to Player and to Computer)
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

        //Initialize method for setting up tests
        //Does not set up a Dealer Deck, so cards can be loaded manually during Unit Tests
        public void BareBonesInit()
        {
            PlayerDeck = new DeckController();
            PlayerClaims = new DeckController();
            ComputerDeck = new DeckController();
            ComputerClaims = new DeckController();
        }

        //Super fast mode to test your processor speed
        //This plays the game without user input, just advancing the game as fast as possible.
        public void PlayGameTurbo()
        {
            bool isGameContinuing = true;
            while (isGameContinuing)
            {
                isGameContinuing = TakeTurn(true);
            }
        }

        //Regular game play mode
        //Players can play the game by pressing "space" to draw a new card and enter the round.
        //Players can check their statistics (the cards in their claim pile, and the size of their playing deck) by pressing 's'
        //Players can forfeit the game by pressing 'x'
        //The game ends when the turn cannot continue (TakeTurn() returns false)
        public void PlayGame()
        {
            bool isGameContinuing = true;
            //player input loop
            while (isGameContinuing)
            {
                char controlChar = '0';
                while (controlChar != ' ')
                {
                    Console.WriteLine("Press space to draw, and 's' to check stats. Press 'x' to exit the game.");
                    controlChar = Console.ReadLine().ToLower()[0];
                    if (controlChar == 's')
                    {
                        PrintPlayerStats();
                    }
                    else if (controlChar == 'x')
                    {
                        Console.WriteLine("----------------------------");
                        Console.WriteLine("Player forfeits the game.");
                        Console.WriteLine("----------------------------");
                        //break out of the game loop
                        return;
                    }
                    else if (controlChar != ' ')
                    {
                        Console.WriteLine("I couldn't understand that command.");
                    }
                }
                isGameContinuing = TakeTurn(false);
            }
        }

        //Main action-method of the program
        //Both player and computer request a card
        //If they can both draw, they compare their cards
        //If either cannot draw, the other wins, and returns to Program.cs
        //If both cannot draw, the game is a draw, and returns to Program.cs
        //If turboMode is set to True, run TakeTurn without waiting for user input (as fast as possible)
        public bool TakeTurn(bool turboMode)
        {

            WarOutcome outcome = WarOutcome.NONE;
            do
            {
                //if the player and computer are currently in war, wait for enter to be pressed for each round of the war
                if(outcome == WarOutcome.WAR)
                {
                    Console.WriteLine("WAR! You're at war with the computer, whoever draws higher takes home all the cards in the war.");
                    if(!turboMode)
                    {
                        char controlChar = '0';
                        while (controlChar != ' ')
                        {
                            Console.WriteLine("Press space to draw, and 's' to check stats. Press 'x' to exit the game");
                            controlChar = Console.ReadLine().ToLower()[0];
                            if (controlChar == 's')
                            {
                                PrintPlayerStats();
                            }
                            else if (controlChar == 'x')
                            {
                                Console.WriteLine("----------------------------");
                                Console.WriteLine("Player forfeits the game.");
                                Console.WriteLine("----------------------------");
                                return false;
                            }
                            else if (controlChar != ' ')
                            {
                                Console.WriteLine("I couldn't understand that command.");
                            }
                            
                        }
                    }
                }
                CardModel playerCard = null;
                CardModel computerCard = null;

                //Request a card from Player and Computer decks
                bool canPlayerDraw = PlayerDeck.TryDrawCard(out playerCard, PlayerClaims);
                bool canComputerDraw = ComputerDeck.TryDrawCard(out computerCard, ComputerClaims);

                if(canPlayerDraw && !canComputerDraw)
                {
                    //player has cards, but computer does not, player wins!
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Player wins the game!");
                    Console.WriteLine("----------------------------");
                    return false;
                }
                else if(!canPlayerDraw && canComputerDraw)
                {
                    //player does't have cards, but computer does, so the computer wins
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Computer wins the game!");
                    Console.WriteLine("----------------------------");
                    return false;
                }
                else if(!canPlayerDraw && !canComputerDraw)
                {
                    //nobody has cards, so it's a draw.
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Nobody wins; it's a draw.");
                    Console.WriteLine("----------------------------");
                    return false;
                }

                Console.WriteLine("Computer Card: " + computerCard.CardId + " || Your Card: " + playerCard.CardId);

                outcome = CompareController.Instance.CompareCards(playerCard, computerCard);
            } while (outcome == WarOutcome.WAR);

            //Check the outcome of the round, and award the cards in the round history to the player or computer
            switch (outcome)
            {
                case WarOutcome.WIN:
                    {
                        Console.WriteLine("Player takes the cards home.");
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
                        Console.WriteLine("Computer takes the cards home.");
                        //get the winnings
                        var claims = CompareController.Instance.HistoryToDeck();
                        //clear history on card comparer
                        CompareController.Instance.ClearHistory();
                        //award the winnings to the computer
                        ComputerClaims.AddCards(claims);

                        break;
                    }
            }

            //no win condition has been reached, return true to continue playing the game
            return true;
        }

        //Print the number of cards in the player's claims pile, as well as the cards in the player's claims pile
        //Print the number of cards in the player's deck, but don't print the actual cards in them (that'd be cheating)
        public void PrintPlayerStats()
        {
            Console.WriteLine("Cards in claims pile: " + PlayerClaims.Cards.Count);
            foreach(var card in PlayerClaims.Cards)
            {
                Console.WriteLine(card.CardId);
            }
            Console.WriteLine("Cards in your deck: " + PlayerDeck.Cards.Count);
        }
    }
}
