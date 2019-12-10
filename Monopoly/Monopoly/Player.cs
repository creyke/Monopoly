using System.Collections.Generic;

namespace Monopoly
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        public SpaceState Location { get; set; }
        public List<SpaceState> Holdings { get; set; }
    }
}
