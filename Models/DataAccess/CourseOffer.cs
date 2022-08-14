using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterSchool.Models.DataAccess
{
    public partial class CourseOffer
    {
        public CourseOffer()
        {
            TeachingRecords = new HashSet<TeachingRecord>();
        }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, 999.99, ErrorMessage = "Must between 0 and 999.99")]
        public decimal? Price { get; set; }

        [DisplayName("Image Name")]
        public string? ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile? ImageFile { get; set; }


        public virtual Course Course { get; set; } = null!;
        public virtual Location Location { get; set; } = null!;
        public virtual ICollection<TeachingRecord> TeachingRecords { get; set; }

        [NotMapped]
        public List<Instructor> Instructors { get; set; }
    }
}
