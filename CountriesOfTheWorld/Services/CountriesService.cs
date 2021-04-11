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

        public CountriesService()
        {
            _allCountriesEndpoint = _countriesEndpoint + "all";
        }

        public List<Country> AllCountries()
        {
            var webRequest = WebRequest.Create(_allCountriesEndpoint);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json";

            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            StreamReader sr = new StreamReader(webResponse.GetResponseStream());
            var result = sr.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Country>>(result);
        }
    }
}