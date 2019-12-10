using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Game
    {
        private static readonly SpaceType[] PurchasableSpaceTypes = new SpaceType[]
        {
            SpaceType.Property, SpaceType.Airport, SpaceType.Utility
        };

        public Board Board { get; }
        public Player[] Players { get; }
        public Player ActivePlayer { get; private set; }
        public List<MoveOption> ActivePlayerMoveOptions { get; }

        public Game(Board board, IEnumerable<string> playerNames)
        {
            Board = board;

            Players = playerNames
                .Select((x, i) => new Player
                {
                    Id = i,
                    Name = x,
                    Balance = 15000000,
                    Location = Board.GoSpace,
                    Holdings = new List<SpaceState>()
                })
                .ToArray();

            ActivePlayer = Players.First();

            ActivePlayerMoveOptions = new List<MoveOption> { MoveOption.Roll };
        }

        public void Roll(int firstDice, int secondDice)
        {
            MoveActivePlayer(firstDice + secondDice);
        }

        private void MoveActivePlayer(int amount)
        {
            var passedGo = false;

            for (int i = 0; i < amount; i++)
            {
                ActivePlayer.Location = ActivePlayer.Location.Next;
                if (!passedGo && ActivePlayer.Location.Space.SpaceType == SpaceType.Go)
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

        private void ProcessLocation(Player player, SpaceState location)
        {
            ActivePlayerMoveOptions.Clear();

            if (location.Space.Fine > 0)
            {
                DebitPlayer(player, location.Space.Fine);
            }

            if (IsPurchasable(location))
            {
                ActivePlayerMoveOptions.Add(MoveOption.Purchase);
            }
            else
            {
                ChangeToNextPlayer();
            }
        }

        private static bool IsPurchasable(SpaceState location)
        {
            return PurchasableSpaceTypes.Contains(location.Space.SpaceType)
                && location.Owner is null;
        }

        private void CreditPlayer(Player player, int credit)
        {
            ActivePlayer.Balance += credit;
        }

        private void DebitPlayer(Player player, int fine)
        {
            player.Balance -= fine;
        }

        private void ChangeToNextPlayer()
        {
            var nextPlayerId = ActivePlayer.Id < Players.Length - 1
                ? ActivePlayer.Id + 1 : 0;

            ActivePlayer = Players[nextPlayerId];

            ActivePlayerMoveOptions.Add(MoveOption.Roll);
        }

        public void PurchaseProperty()
        {
            var property = ActivePlayer.Location;

            if (property.Space.SpaceType != SpaceType.Property)
            {
                return;
            }

            if (property.Owner != null)
            {
                throw new Exception("Property already has an owner.");
            }

            if (ActivePlayer.Balance < property.Space.Cost)
            {
                throw new Exception("Cannot afford purchase.");
            }

            DebitPlayer(ActivePlayer, property.Space.Cost);

            property.Owner = ActivePlayer;
            ActivePlayer.Holdings.Add(property);
        }
    }
}
