namespace Monopoly
{
    public class SpaceState
    {
        public int Id { get; set; }
        public Space Space { get; set; }
        public SpaceState Previous { get; set; }
        public SpaceState Next { get; set; }
        public Player Owner { get; set; }
    }
}
