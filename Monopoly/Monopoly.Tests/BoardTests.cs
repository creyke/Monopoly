using Monopoly.Providers.Spaces;
using System.Threading.Tasks;
using Xunit;

namespace Monopoly.Tests
{
    public class BoardTests
    {
        [Fact]
        public async Task CanLoadBoardFromFile()
        {
            var spaces = await new ExcelSpacesProvider("./Data/Board.xlsx").GetSpacesAsync();
            var board = new Board(spaces);

            Assert.NotNull(board.GoSpace);
        }
    }
}
