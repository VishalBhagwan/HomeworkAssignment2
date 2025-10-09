using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeworkAssignment2.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        /*
        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<Adoption> Adoptions { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }
        */
    }
}