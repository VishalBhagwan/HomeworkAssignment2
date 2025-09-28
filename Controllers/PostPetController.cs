using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeworkAssignment2.Models;
using System.IO;

namespace HomeworkAssignment2.Controllers
{
    public class PostPetController : Controller
    {
        private RescuePetDataService dataService = new RescuePetDataService();

        public ActionResult Create()
        {
            ViewBag.Users = dataService.GetAllUsers();
            ViewBag.Types = new[] { "Dog", "Cat", "Bird", "Rabbit", "Other" };
            ViewBag.Locations = new[] { "Gauteng", "Western Cape", "KwaZulu-Natal", "Eastern Cape", "Free State" };
            ViewBag.Genders = new[] { "Male", "Female" };

            return View();
        }

        [HttpPost]
        public ActionResult Create(Pet pet, System.Web.HttpPostedFileBase petImage, int postedByUserId)
        {
            try
            {
                if (petImage != null && petImage.ContentLength > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        petImage.InputStream.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        pet.ImageBase64 = System.Convert.ToBase64String(imageBytes);
                    }
                }

                pet.PostedByUserId = postedByUserId;
                pet.Status = "Available";

                dataService.InsertPet(pet);

                TempData["Message"] = "Successfully posted " + pet.Name + " for adoption!";
                return RedirectToAction("Index", "Pets");
            }
            catch
            {
                TempData["Error"] = "Failed to post pet. Please try again.";
                return RedirectToAction("Create");
            }
        }
    }
}