using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterSchool.Models.DataAccess
{
    public partial class Category
    {
        public Category()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        [RegularExpression(@"[a-zA-Z ]+", ErrorMessage = "Not a valid name! Only accept alphabetic characters and space.")]
        public string Name { get; set; } = null!;

        [NotMapped]
        public string FullCategoryName
        {
            get
            {
                return Id + " - " + Name;
            }
        }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
