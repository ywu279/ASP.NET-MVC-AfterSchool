using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterSchool.Models.DataAccess
{
    public partial class Location
    {
        public Location()
        {
            CourseOffers = new HashSet<CourseOffer>();
        }

        [Display(Name = "Location ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Branch Name")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = null!;

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Address should be more than 5 charactors")]
        public string Address { get; set; } = null!;

        public virtual ICollection<CourseOffer> CourseOffers { get; set; }
    }
}
