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
        private decimal donationGoal = 10000.00m;

        public ActionResult Donate()
        {
            ViewBag.Users = dataService.GetAllUsers();

            var totalDonated = dataService.GetTotalDonations();
            var progressPercentage = (totalDonated / donationGoal) * 100;

            ViewBag.TotalDonated = totalDonated;
            ViewBag.DonationGoal = donationGoal;
            ViewBag.ProgressPercentage = System.Math.Min(progressPercentage, 100);

            return View();
        }

        [HttpPost]
        public ActionResult Donate(int donatedByUserId, decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    TempData["Error"] = "Donation amount must be greater than zero.";
                    return RedirectToAction("Donate");
                }

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