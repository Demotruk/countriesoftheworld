using System.Collections.Generic;

namespace CountriesOfTheWorld.Models
{
    public class Region : RegionBase
    {
        public Region(string name) : base(name) { SubRegions = new Dictionary<string, SubRegion>(); }

        public Dictionary<string, SubRegion> SubRegions { get; private set; }

        public int Population
        {
            get
            {
                int count = 0;
                foreach (SubRegion subregion in SubRegions.Values)
                {
                    count += subregion.Population;
                }
                return count;
            }
        }

        public string Info() { return $"Population: {Population}"; }
    }
}