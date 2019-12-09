using Monopoly.Providers.Spaces;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Monopoly.Tests
{
    public class GameTests
    {
        private static readonly IEnumerable<Space> Spaces;

        private Game subject;

        static GameTests() 
        {
            Spaces = new ExcelSpacesProvider("./Data/Board.xlsx")
                .GetSpacesAsync().Result;
        }

        private void CreateGame(int playerCount = 1)
        {
            var playerNames = Enumerable
                .Range(0, playerCount)
                .Select(x => x.ToString() );

            subject = new Game(new Board(Spaces), playerNames);
        }

        [Fact]
        public void CanCreate()
        {
            CreateGame();
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(6, 6, 12)]
        [InlineData(20, 20, 0)]
        public void CanRollAndMove(int firstDice, int secondDice, int exceptedSpaceId)
        {
            CreateGame();

            var player = subject.ActivePlayer;

            subject.Roll(firstDice, secondDice);

            Assert.Equal(exceptedSpaceId, player.Location.Id);
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
    }
}
