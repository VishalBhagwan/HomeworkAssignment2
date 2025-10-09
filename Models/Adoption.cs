using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeworkAssignment2.Models
{
    public class Adoption
    {
        public int AdoptionId {  get; set; }
        public int PetId { get; set; }
        public int AdoptedByUserId { get; set; }
        public DateTime AdoptionDate { get; set; }
        public string PetName { get; set; }
        public string AdoptedByUserName { get; set; }
    }
}