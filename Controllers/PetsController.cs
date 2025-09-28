using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeworkAssignment2.Models;

namespace HomeworkAssignment2.Controllers
{
    public class PetsController : Controller
    {
        private RescuePetDataService dataService = new RescuePetDataService();

        public ActionResult Index()
        {
            var pets = dataService.GetAllPetsByFilters(
                HomeController.TypeFilter,
                HomeController.BreedFilter,
                HomeController.LocationFilter,
                HomeController.StatusFilter
            );

            ViewBag.Types = dataService.GetDistinctTypes();
            ViewBag.Locations = dataService.GetDistinctLocations();
            ViewBag.Breeds = dataService.GetDistinctBreeds();
            ViewBag.Statuses = new[] { "All", "Available", "Adopted" };

            ViewBag.CurrentType = HomeController.TypeFilter;
            ViewBag.CurrentBreed = HomeController.BreedFilter;
            ViewBag.CurrentLocation = HomeController.LocationFilter;
            ViewBag.CurrentStatus = HomeController.StatusFilter;

            return View(pets);
        }

        public ActionResult Adopt(int id)
        {
            var pet = dataService.GetPetById(id);

            if (pet == null)
            {
                return HttpNotFound();
            }

            ViewBag.Users = dataService.GetAllUsers();
            return View(pet);
        }

        [HttpPost]
        public ActionResult Adopt(int petId, int adoptedByUserId)
        {
            try
            {
                dataService.CreateAdoption(petId, adoptedByUserId);
                TempData["Message"] = "Adoption successful!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Adoption failed. Please try again.";
                return RedirectToAction("Adopt", new { id = petId });
            }
        }
    }
}