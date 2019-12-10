using Monopoly.Providers.Spaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Monopoly.Simulator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var provider = new ExcelSpacesProvider("BoardSpaces.xlsx");
            var spaces = await provider.GetSpacesAsync();
            var board = new Board(spaces);
            var game = new Game(board, new string[] { "Roger", "CPU" });

            var dice = new Random();

            while (!game.Players.Any(x => x.Balance <= 0))
            {
                var player = game.ActivePlayer;
                var roll = (dice.Next(1, 6), dice.Next(1, 6));
                game.Roll(roll.Item1, roll.Item2);

                Console.WriteLine($"Player {player.Name} rolled {roll.Item1} + {roll.Item2} = {roll.Item1 + roll.Item2}...");
                Console.WriteLine($"  Moved to {game.ActivePlayer.Location.Space.Name}. Balance {player.Balance}.");
            }

            var winner = game.Players.First(x => x.Balance > 0);

            Console.WriteLine($"{winner.Name} won with a balance of {winner.Balance}!");

            Console.Read();
        }
    }
}
