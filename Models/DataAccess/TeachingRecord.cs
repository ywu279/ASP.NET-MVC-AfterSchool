using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterSchool.Models.DataAccess
{
    public partial class TeachingRecord
    {
        public int CourseId { get; set; }

        public int LocationId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        public int InstructorId { get; set; }
        public byte[]? Timestamp { get; set; }


        public virtual CourseOffer CourseOffer { get; set; } = null!;
        public virtual Instructor Instructor { get; set; } = null!;
    }
}
