using Monopoly.Tests.TestData;
using System.Linq;
using Xunit;

namespace Monopoly.Tests
{
    public class RulesTests
    {
        private Game subject;
        private Player firstPlayer;
        private int firstPlayerStartingBalance;

        private void CreateGame(int playerCount = 1)
        {
            var playerNames = Enumerable
                .Range(0, playerCount)
                .Select(x => x.ToString());

            subject = new Game(new Board(BoardSpacesTestData.Data), playerNames);

            firstPlayer = subject.Players.First();
            firstPlayerStartingBalance = firstPlayer.Balance;
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

        [Fact]
        public void PlayerLosesCacheOnFineSquare()
        {
            CreateGame();

            subject.Roll(2, 2);

            Assert.Equal(firstPlayerStartingBalance -= 1000000, subject.ActivePlayer.Balance);
        }

        [Fact]
        public void IfPlayerPassesGoCredit2Million()
        {
            CreateGame();

            SinglePlayerNavigateBoardOnceNoCosts();

            Assert.Equal(firstPlayerStartingBalance + 2000000, subject.ActivePlayer.Balance);
        }

        [Fact]
        public void CanPurchaseProperty()
        {
            CreateGame();

            SinglePlayerNavigateToFirstProperty();

            subject.PurchaseProperty();

            Assert.Equal(firstPlayerStartingBalance - firstPlayer.Location.Space.Cost, firstPlayer.Balance);
        }

        [Fact]
        public void CanCheckIfPropertyIsAvailable()
        {
            CreateGame();

            SinglePlayerNavigateToFirstProperty();

            Assert.Contains(MoveOption.Purchase, subject.ActivePlayerMoveOptions);
        }

        [Fact]
        public void CanRollAgainIfDouble()
        {
            CreateGame(2);

            subject.Roll(1, 1);

            Assert.Equal(firstPlayer.Id, subject.ActivePlayer.Id);
            Assert.Contains(MoveOption.Roll, subject.ActivePlayerMoveOptions);
        }

        // cannot purchase if not enough funds.
        // cannot purchase if not a property.
        // cannot purchase if already has owner.
        // can check if property available.

        private void SinglePlayerNavigateBoardOnceNoCosts()
        {
            subject.Roll(4, 6);
            subject.Roll(4, 6);
            subject.Roll(4, 4);
            subject.Roll(3, 4);
            subject.Roll(3, 2);
        }

        private void SinglePlayerNavigateToFirstProperty()
        {
            subject.Roll(1, 2);
        }
    }
}
