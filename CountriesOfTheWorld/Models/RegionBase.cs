namespace CountriesOfTheWorld.Models
{
    public abstract class RegionBase
    {
        public RegionBase(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}