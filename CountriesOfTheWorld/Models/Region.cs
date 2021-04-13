using System.Collections.Generic;

namespace CountriesOfTheWorld.Models
{
    public class Region : RegionBase
    {
        private List<Country> _countries;

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

        public List<Country> Countries
        {
            get
            {
                if (_countries == null)
                {

                    var countries = new List<Country>();
                    foreach (SubRegion subRegion in SubRegions.Values)
                    {
                        foreach (Country country in subRegion.Countries.Values)
                        {
                            countries.Add(country);
                        }
                    }

                    _countries = countries;
                }
                return _countries;
            }
        }

        public string Info() { return $"Population: {Population}"; }
    }
}