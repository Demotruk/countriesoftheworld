using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CountriesOfTheWorld.Models
{
    public class SubRegion : RegionBase
    {
        public SubRegion(string name) : base(name)
        {
            Countries = new Dictionary<string, Country>();
        }

        public Dictionary<string, Country> Countries { get; private set; }

        public int Population
        {
            get
            {
                int count = 0;
                foreach (Country country in Countries.Values)
                {
                    count += country.population;
                }
                return count;
            }
        }

        public Region Region { get; set; }

        public string Info() { return $"Population: {Population}"; }
    }
}