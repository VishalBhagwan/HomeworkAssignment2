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

        public ActionResult Index(string type, string breed, string location, string status)
        {
            // Set default values to "All"
            type = type ?? "All";
            breed = breed ?? "All";
            location = location ?? "All";
            status = status ?? "All";

            // Get filtered pets from database
            var pets = dataService.GetAllPetsByFilters(type, breed, location, status);

            // Populate ViewBag for filter dropdowns
            ViewBag.Types = dataService.GetDistinctTypes();
            ViewBag.Locations = dataService.GetDistinctLocations();
            ViewBag.Breeds = dataService.GetDistinctBreeds();
            

            // Pass current selections to the view to show the current selected values
            ViewBag.CurrentType = type;
            ViewBag.CurrentBreed = breed;
            ViewBag.CurrentLocation = location;
            ViewBag.CurrentStatus = status;

            return View(pets);
        }

        // Displays the Adopt page when the adopt button is clicked.
        public ActionResult Adopt(int id)
        {
            // Fetches the details by their pet ID to display 
            var pet = dataService.GetPetById(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            // Can select who is adopting from list of users
            ViewBag.Users = dataService.GetAllUsers();
            return View(pet);
        }

        // When adoption form is submitted
        [HttpPost]
        public ActionResult Adopt(int petId, int adoptedByUserId)
        {
            try
            {
                // Creates adoption in database
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