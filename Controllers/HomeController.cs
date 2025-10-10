using System.Linq;
using System.Web.Mvc;
using HomeworkAssignment2.Models;

namespace HomeworkAssignment2.Controllers
{
    public class HomeController : Controller
    {
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
    }
}