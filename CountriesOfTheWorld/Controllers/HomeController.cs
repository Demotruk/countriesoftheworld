using CountriesOfTheWorld.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CountriesOfTheWorld.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var countriesService = new CountriesService();

            ViewBag.CountryData = countriesService.AllCountries();

            return View();
        }
    }
}