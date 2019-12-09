using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Game
    {
        public Board Board { get; }
        public Player[] Players { get; }
        public Player ActivePlayer { get; private set; }

        public Game(Board board, IEnumerable<string> playerNames)
        {
            this.Board = board;

            Players = playerNames
                .Select((x, i) => new Player
                {
                    Id = i,
                    Name = x,
                    Location = Board.GoSpace
                })
                .ToArray();

            ActivePlayer = Players.First();
        }

        public void Roll(int firstDice, int secondDice)
        {
            MoveActivePlayer(firstDice + secondDice);

            ChangeToNextPlayer();
        }

        private void MoveActivePlayer(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ActivePlayer.Location = ActivePlayer.Location.Next;
            }
        }

        private void ChangeToNextPlayer()
        {
            var nextPlayerId = ActivePlayer.Id < Players.Length - 1
                ? ActivePlayer.Id + 1 : 0;

            ActivePlayer = Players[nextPlayerId];
        }
    }
}
