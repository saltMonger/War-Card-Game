using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarCardGameConsole;
using WarCardGameConsole.Controllers;

namespace WarCardGameTests
{
    [TestClass]
    public class GamePlayTests
    {
        [TestMethod]
        public void TestStalemate()
        {
            // Arrange - setup manager and fill decks
            ManagerController manager = ManagerController.Instance;
            manager.BareBonesInit();

            manager.ComputerDeck.Cards.Enqueue(new WarCardGameConsole.Models.CardModel { CardId = "C2", CardRank = 2 });
            manager.PlayerDeck.Cards.Enqueue(new WarCardGameConsole.Models.CardModel { CardId = "C2", CardRank = 2 });

            //Act - proceed through WAR twice
            manager.TakeTurn(true);
            bool isContinuing = manager.TakeTurn(true);

            //Assert - isContinuing should be false, since there are no more cards to play
            Assert.IsFalse(isContinuing);
        }

        [TestMethod]
        public void TestPlayerWins()
        {
            //Arrange - setup manager and fill decks
            ManagerController manager = ManagerController.Instance;
            manager.BareBonesInit();
            
            manager.PlayerDeck.Cards.Enqueue(new WarCardGameConsole.Models.CardModel { CardId = "C2", CardRank = 2 });

            //Act - proceed through turn once
            bool isContinuing = manager.TakeTurn(true);

            //Assert - isContinuing should be false, since the computer has no more cards to play
            Assert.IsFalse(isContinuing);
        }

        [TestMethod]
        public void TestComputerWins()
        {
            //Arrange - setup manager and fill decks
            ManagerController manager = ManagerController.Instance;
            manager.BareBonesInit();

            manager.ComputerDeck.Cards.Enqueue(new WarCardGameConsole.Models.CardModel { CardId = "C2", CardRank = 2 });

            //Act - proceed through turn once
            bool isContinuing = manager.TakeTurn(true);

            //Assert - isContinuing should be false, since the player has no more cards to play
            Assert.IsFalse(isContinuing);
        }
    }
}
