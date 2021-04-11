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

        private static List<Country> countriesDictionary;
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
        }

        public List<Country> AllCountries()
        {
            if (countriesDictionary == null)
            {
                var webRequest = WebRequest.Create(_allCountriesEndpoint);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";

                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                var result = sr.ReadToEnd();
                countriesDictionary = JsonConvert.DeserializeObject<List<Country>>(result);
            }
            return countriesDictionary;
        }

        public void BuildDictionaries()
        {
            BuildSubregionDictionary();
            BuildRegionsDictionary();
        }

        public void BuildSubregionDictionary()
        {
            foreach(Country country in AllCountries())
            {
                if(!subRegions.ContainsKey(country.subregion))
                {
                    var subRegion = new SubRegion(country.subregion);
                    subRegions.Add(country.subregion, subRegion);
                }

                subRegions[country.subregion].Countries.Add(country.name, country);
            }
        }

        public void BuildRegionsDictionary()
        {
            foreach(SubRegion subRegion in subRegions.Values)
            {
                if(!regions.ContainsKey(subRegion.Countries.Values.ToList<Country>()[0].region))
                {
                    var region = new Region(subRegion.Countries.Values.ToList<Country>()[0].region);
                    regions.Add(subRegion.Countries.Values.ToList<Country>()[0].region, region);
                }

                subRegion.Region = regions[subRegion.Countries.Values.ToList<Country>()[0].region];
                regions[subRegion.Countries.Values.ToList<Country>()[0].region].SubRegions.Add(subRegion.Name, subRegion);
            }
        }

        public Country GetCountry(string countryCode)
        {
            if (!countryDictionary.ContainsKey(countryCode))
            {

                var webRequest = WebRequest.Create(_getCountryByCodeEndpoint + countryCode);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";

                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                var result = sr.ReadToEnd();
                countryDictionary.Add(countryCode, JsonConvert.DeserializeObject<Country>(result));
            }
            return countryDictionary[countryCode];
        }

        public SubRegion GetSubRegion(string name)
        {
            if(subRegions.Count == 0) { BuildDictionaries(); }

            return subRegions[name];
        }

        public Region GetRegion(string name)
        {
            if(regions.Count == 0) { BuildDictionaries(); }

            return regions[name];
        }
    }
}