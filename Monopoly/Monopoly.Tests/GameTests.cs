using Monopoly.Tests.TestData;
using System.Linq;
using Xunit;

namespace Monopoly.Tests
{
    public class GameTests
    {
        private Game subject;
        private int player1StartingBalance;

        private void CreateGame(int playerCount = 1)
        {
            var playerNames = Enumerable
                .Range(0, playerCount)
                .Select(x => x.ToString());

            subject = new Game(new Board(BoardSpacesTestData.Data), playerNames);

            player1StartingBalance = subject.Players.First().Balance;
        }

        [Fact]
        public void CanCreate()
        {
            CreateGame();

            Assert.Equal(15000000, subject.ActivePlayer.Balance);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(6, 6, 12)]
        [InlineData(20, 20, 0)]
        public void CanRollAndMove(int firstDice, int secondDice, int exceptedSpaceId)
        {
            CreateGame();

            subject.Roll(firstDice, secondDice);

            Assert.Equal(exceptedSpaceId, subject.ActivePlayer.Location.Id);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 0)]
        public void SwitchPlayerAfterRoll(int numRolls, int exceptedPlayerId)
        {
            CreateGame(2);

            for (int i = 0; i < numRolls; i++)
            {
                subject.Roll(1, 1);
            }

            Assert.Equal(exceptedPlayerId, subject.ActivePlayer.Id);
        }

        [Fact]
        public void PlayerLosesCacheOnFineSquare()
        {
            CreateGame();

            subject.Roll(2, 2);

            Assert.Equal(player1StartingBalance -= 1000000, subject.ActivePlayer.Balance);
        }

        [Fact]
        public void IfPlayerPassesGoCredit2Million()
        {
            CreateGame();

            SinglePlayerNavigateBoardOnceNoCosts();

            Assert.Equal(player1StartingBalance + 2000000, subject.ActivePlayer.Balance);
        }

        private void SinglePlayerNavigateBoardOnceNoCosts()
        {
            subject.Roll(4, 6);
            subject.Roll(4, 6);
            subject.Roll(4, 4);
            subject.Roll(3, 4);
            subject.Roll(3, 2);
        }
    }
}
