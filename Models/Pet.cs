using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeworkAssignment2.Models
{
    public class Pet
    {
        public int PetId {  get; set; }
        public string Type { get; set; }
        public string Breed { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public decimal Weight { get; set; }
        public string PetStory { get; set; }
        public string Status { get; set; } //Available/Adopted
        public string ImageBase64 {  get; set; }
        public int PostedByUserId { get; set; }
        public string PostedByUserName { get; set; }
    }
}