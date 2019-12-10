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
            Board = board;

            Players = playerNames
                .Select((x, i) => new Player
                {
                    Id = i,
                    Name = x,
                    Balance = 15000000,
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
            var passedGo = false;

            for (int i = 0; i < amount; i++)
            {
                ActivePlayer.Location = ActivePlayer.Location.Next;
                if (!passedGo && ActivePlayer.Location.SpaceType == SpaceType.Go)
                {
                    passedGo = true;
                }
            }

            if (passedGo)
            {
                CreditPlayer(ActivePlayer, 2000000);
            }

            ProcessLocation(ActivePlayer, ActivePlayer.Location);
        }

        private void ProcessLocation(Player player, Space location)
        {
            if (location.Fine > 0)
            {
                FinePlayer(player, location.Fine);
            }
        }

        private void CreditPlayer(Player player, int credit)
        {
            ActivePlayer.Balance += credit;
        }

        private void FinePlayer(Player player, int fine)
        {
            player.Balance -= fine;
        }

        private void ChangeToNextPlayer()
        {
            var nextPlayerId = ActivePlayer.Id < Players.Length - 1
                ? ActivePlayer.Id + 1 : 0;

            ActivePlayer = Players[nextPlayerId];
        }
    }
}
