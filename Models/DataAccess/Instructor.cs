using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterSchool.Models.DataAccess
{
    public partial class Instructor
    {
        public Instructor()
        {
            TeachingRecords = new HashSet<TeachingRecord>();
        }

        public int Id { get; set; }


        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"[a-zA-Z ,.'-]+", ErrorMessage = "Not a valid name! Only accept charactors like [a-zA-Z ,.'-]")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"[a-zA-Z ,.'-]+", ErrorMessage = "Not a valid name! Only accept charactors like [a-zA-Z ,.'-]")]
        public string LastName { get; set; } = null!;

        public string? Description { get; set; }

        [Display(Name = "Image Name")]
        public string? ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public virtual ICollection<TeachingRecord> TeachingRecords { get; set; }
    }
}
