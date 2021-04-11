using CountriesOfTheWorld.Services;
using System.Web.Mvc;

namespace CountriesOfTheWorld.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var countriesService = new CountriesService();

            ViewBag.CountryData = countriesService.AllCountries();
            ViewBag.Service = new CountriesService();

            return View();
        }
    }
}