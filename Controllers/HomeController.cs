using System.Linq;
using System.Web.Mvc;
using HomeworkAssignment2.Models;

namespace HomeworkAssignment2.Controllers
{
    public class HomeController : Controller
    {
        public static string TypeFilter = "All";
        public static string BreedFilter = "All";
        public static string LocationFilter = "All";
        public static string StatusFilter = "Available";

        private RescuePetDataService dataService = new RescuePetDataService();

        public ActionResult Index()
        {
            // Retreives the data so that it can be displayed
            var adoptedCount = dataService.GetAdoptedPetsCount();
            var adoptions = dataService.GetRecentAdoptions(10);

            ViewBag.AdoptedCount = adoptedCount;
            ViewBag.Adoptions = adoptions;

            return View();
        }

        // Filters which update their filter variable above
        public ActionResult SetTypeFilter(string type)
        {
            TypeFilter = type;
            return RedirectToAction("Index", "Pets");
        }

        public ActionResult SetBreedFilter(string breed)
        {
            BreedFilter = breed;
            return RedirectToAction("Index", "Pets");
        }

        public ActionResult SetLocationFilter(string location)
        {
            LocationFilter = location;
            return RedirectToAction("Index", "Pets");
        }

        public ActionResult SetStatusFilter(string status)
        {
            StatusFilter = status;
            return RedirectToAction("Index", "Pets");
        }

        public ActionResult ResetFilters()
        {
            TypeFilter = "All";
            BreedFilter = "All";
            LocationFilter = "All";
            StatusFilter = "Available";
            return RedirectToAction("Index", "Pets");
        }


    }
}