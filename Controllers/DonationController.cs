using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeworkAssignment2.Models;

namespace HomeworkAssignment2.Controllers
{
    public class DonationController : Controller
    {
        private RescuePetDataService dataService = new RescuePetDataService();
        private decimal donationGoal = 10000.00m; // Fixed amount

        public ActionResult Donate()
        {
            // List of all user from the database
            ViewBag.Users = dataService.GetAllUsers();

            // Total donation amount so far from database
            var totalDonated = dataService.GetTotalDonations();
            var progressPercentage = (totalDonated / donationGoal) * 100;

            ViewBag.TotalDonated = totalDonated;
            ViewBag.DonationGoal = donationGoal;
            ViewBag.ProgressPercentage = System.Math.Min(progressPercentage, 100);
            // Calaculate progress

            return View();
        }

        // When user submits the donation form
        [HttpPost]
        public ActionResult Donate(int donatedByUserId, decimal amount)
        {
            try
            {
                // Checks if donation amount is positive
                if (amount <= 0)
                {
                    TempData["Error"] = "Donation amount must be greater than zero.";
                    return RedirectToAction("Donate");
                }

                // Save donation in database
                dataService.InsertDonation(donatedByUserId, amount);

                TempData["Message"] = "Thank you for your donation of R" + amount.ToString("0.00") + "!";
                return RedirectToAction("Donate");
            }
            catch
            {
                TempData["Error"] = "Donation failed. Please try again.";
                return RedirectToAction("Donate");
            }
        }
    }
}