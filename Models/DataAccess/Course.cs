using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterSchool.Models.DataAccess
{
    public partial class Course
    {
        public Course()
        {
            CourseOffers = new HashSet<CourseOffer>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = "";

        [Required]
        public int CategoryId { get; set; }


        [NotMapped]
        public string FullCourseName
        {
            get
            {
                return Id + " - " + Name;
            }
        }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<CourseOffer> CourseOffers { get; set; }

    }
}
