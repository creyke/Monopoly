using System.IO;
using Xunit;

namespace Monopoly.Tests
{
    public class BoardTests
    {
        [Fact]
        public void CanLoadBoardFromFile()
        {
            using var fileStream = new FileStream("./Data/Board.xlsx", FileMode.Open);
            var board = new Board(fileStream);
        }
    }
}
