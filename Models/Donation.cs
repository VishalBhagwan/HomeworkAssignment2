using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeworkAssignment2.Models
{
    public class Donation
    {
        public int DonationID {  get; set; }
        public int DonatedByUserID { get; set; }
        public decimal DonationAmount { get; set; }
        public DateTime DonationDate { get; set; }
        public string DonatedByUserName { get; set; }
    }
}