using Newtonsoft.Json;

namespace Monopoly
{
    public class Space
    {
        public string Name { get; set; }
        public SpaceType SpaceType { get; set; }
        public string PropertyGroup { get; set; }
        public int Fine { get; set; }
        public int Cost { get; set; }
        public int Rent1Apt { get; set; }
        public int Rent2Apt { get; set; }
        public int Rent3Apt { get; set; }
        public int Rent4Apt { get; set; }
        public int RentHotel { get; set; }
        public int CostPerApt { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
