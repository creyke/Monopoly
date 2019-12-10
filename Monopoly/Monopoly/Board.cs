using System.Collections.Generic;

namespace Monopoly
{
    public class Board
    {
        public Board(IEnumerable<Space> spaces)
        {
            SpaceState previousSpace = null;

            var spaceId = 0;

            foreach (var space in spaces)
            {
                var state = new SpaceState
                {
                    Id = spaceId++,
                    Space = space
                };

                if (previousSpace is null)
                {
                    GoSpace = state;
                }
                else
                {
                    previousSpace.Next = state;
                }

                state.Previous = previousSpace;

                previousSpace = state;
            }
            
            previousSpace.Next = GoSpace;
            GoSpace.Previous = previousSpace;
        }

        public SpaceState GoSpace { get; }
    }
}
