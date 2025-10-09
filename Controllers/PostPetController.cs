using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeworkAssignment2.Models;
using System.IO;
using System.Globalization;

namespace HomeworkAssignment2.Controllers
{
    public class PostPetController : Controller
    {
        private RescuePetDataService dataService = new RescuePetDataService();

        // Display the page
        public ActionResult Create()
        {
            // Populates dropdowns
            ViewBag.Users = dataService.GetAllUsers();
            ViewBag.Types = new[] { "Dog", "Cat", "Bird", "Rabbit", "Other" };
            ViewBag.Locations = new[] { "Gauteng", "Western Cape", "KwaZulu-Natal", "Eastern Cape", "Free State" };
            ViewBag.Genders = new[] { "Male", "Female" };

            return View();
        }

        // When user submits form
        [HttpPost]
        public ActionResult Create(Pet pet, System.Web.HttpPostedFileBase petImage, int postedByUserId)
        {
            try
            {
                // Check if age was entered and convert it to a number
                if (!string.IsNullOrEmpty(Request.Form["Age"]))
                {
                    decimal ageValue;
                    if (decimal.TryParse(Request.Form["Age"], NumberStyles.Any, CultureInfo.InvariantCulture, out ageValue))
                    {
                        // Store age as int
                        pet.Age = (int)ageValue;
                    }
                    else
                    {
                        TempData["Error"] = "Please enter a valid age.";
                        return RedirectToAction("Create");
                    }
                }

                // Check if weight was entered 
                if (!string.IsNullOrEmpty(Request.Form["Weight"]))
                {
                    decimal weightValue;
                    if (decimal.TryParse(Request.Form["Weight"], NumberStyles.Any, CultureInfo.InvariantCulture, out weightValue))
                    {
                        pet.Weight = weightValue;
                    }
                    else
                    {
                        TempData["Error"] = "Please enter a valid weight.";
                        return RedirectToAction("Create");
                    }
                }

                // Image upload
                if (petImage != null && petImage.ContentLength > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        // Converts image to base64 to be stored in database
                        petImage.InputStream.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        pet.ImageBase64 = System.Convert.ToBase64String(imageBytes);
                    }
                }

                pet.PostedByUserId = postedByUserId;
                pet.Status = "Available";

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to post pet. Please try again. Error: " + ex.Message;
                return RedirectToAction("Create");
            }
        }
    }
}