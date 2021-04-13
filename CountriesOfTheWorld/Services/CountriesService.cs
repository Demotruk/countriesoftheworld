using CountriesOfTheWorld.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace CountriesOfTheWorld.Services
{
    public class CountriesService
    {
        private readonly string _countriesEndpoint = ConfigurationManager.AppSettings.Get("CountriesApiEndpoint");
        private readonly string _allCountriesEndpoint;
        private readonly string _getCountryByCodeEndpoint;

        private static List<Country> countriesList;
        private static Dictionary<string, Country> countryDictionary;

        private static Dictionary<string, Region> regions;
        private static Dictionary<string, SubRegion> subRegions;


        public CountriesService()
        {
            _allCountriesEndpoint = _countriesEndpoint + "all";
            _getCountryByCodeEndpoint = _countriesEndpoint + "alpha/";

            countryDictionary = new Dictionary<string, Country>();
            regions = new Dictionary<string, Region>();
            subRegions = new Dictionary<string, SubRegion>();

            BuildDictionaries();
        }

        public List<Country> AllCountries()
        {
            if (countriesList == null)
            {
                var webRequest = WebRequest.Create(_allCountriesEndpoint);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";

                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                var result = sr.ReadToEnd();
                countriesList = JsonConvert.DeserializeObject<List<Country>>(result);
            }

            return countriesList;
        }

        public void BuildDictionaries()
        {
            BuildCountriesDictionary();
            BuildSubregionDictionary();
            BuildRegionsDictionary();
        }

        private void BuildCountriesDictionary()
        {
            foreach (Country country in AllCountries())
            {
                countryDictionary.Add(country.alpha3Code, country);
            }
            foreach (Country country in AllCountries())
            {
                foreach (string borderCountryCode in country.borders)
                {
                    if (!country.BorderCountries.ContainsKey(borderCountryCode))
                    {
                        var borderCountry = GetCountry(borderCountryCode);

                        country.BorderCountries.Add(borderCountryCode, borderCountry);
                    }
                }

            }
        }

        public void BuildSubregionDictionary()
        {
            foreach (Country country in AllCountries())
            {
                if (!subRegions.ContainsKey(country.subregion))
                {
                    var subRegion = new SubRegion(country.subregion);
                    subRegions.Add(country.subregion, subRegion);
                }

                subRegions[country.subregion].Countries.Add(country.name, country);
                country.SubRegion = subRegions[country.subregion];
            }
        }

        public void BuildRegionsDictionary()
        {
            foreach (SubRegion subRegion in subRegions.Values)
            {
                if (!regions.ContainsKey(subRegion.Countries.Values.ToList()[0].region))
                {
                    var region = new Region(subRegion.Countries.Values.ToList()[0].region);
                    regions.Add(subRegion.Countries.Values.ToList()[0].region, region);
                }

                subRegion.Region = regions[subRegion.Countries.Values.ToList()[0].region];
                regions[subRegion.Countries.Values.ToList()[0].region].SubRegions.Add(subRegion.Name, subRegion);
            }
        }

        public Country GetCountry(string countryCode)
        {
            return countryDictionary[countryCode];
        }

        public SubRegion GetSubRegion(string name)
        {
            return subRegions[name];
        }

        public Region GetRegion(string name)
        {
            return regions[name];
        }
    }
}