using System.Collections.Generic;

namespace Monopoly
{
    public class Board
    {
        public Board(IEnumerable<Space> spaces)
        {
            Space previousSpace = null;

            foreach (var space in spaces)
            {
                if (previousSpace is null)
                {
                    GoSpace = space;
                }
                else
                {
                    previousSpace.Next = space;
                }

                space.Previous = previousSpace;

                previousSpace = space;
            }
                
            previousSpace.Next = GoSpace;
            GoSpace.Previous = previousSpace;
        }

        public Space GoSpace { get; }
    }
}
