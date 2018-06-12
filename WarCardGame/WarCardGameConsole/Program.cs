using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarCardGameConsole.Controllers;

namespace WarCardGameConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ManagerController manager = ManagerController.Instance;
            manager.Init();


            //game loop
            Console.WriteLine("Welcome to the console-tastic version of War, the classic card game.");
            bool isContinuing = true;
            while (isContinuing == true)
            {
                Console.WriteLine("To play a game, type 's' and then enter.  To play a game in fast mode, type 'f' and then enter.");
                char controlChar = '0';
                while (controlChar != ' ')
                {
                    controlChar = Console.ReadLine().ToLower()[0];
                    if (controlChar == 's')
                    {
                        //play regular game, where the control draws a card with the space key and check their own stats
                        manager.PlayGame();
                        break;
                    }
                    else if (controlChar == 'f')
                    {
                        //play the game in turbo mode, and see how fast C#'s Console.WriteLine() works
                        manager.PlayGameTurbo();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("I couldn't understand that command.");
                    }
                }
                Console.WriteLine("Would you like to play again? Type 'Y' for another game.");
                controlChar = Console.ReadLine().ToLower()[0];
                if (controlChar == 'y')
                {
                    isContinuing = true;
                }
                else
                {
                    Console.WriteLine("Good bye!");
                    isContinuing = false;
                }
            }
        }
    }
}
