using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using work_01.CustomValidation;

namespace work_01.Models.Annotation
{
    public class TraineeType
    {
        public int TraineeId { get; set; }
        [Required, StringLength(50), Display(Name = "Trainee Name")]
        public string TraineeName { get; set; }
        [Required, Display(Name = "Join Date")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomJoinDate(ErrorMessage = "Join Date must be less than or equal to Today's Date")]
        public DateTime JoinDate { get; set; }
        public int CourseId { get; set; }
        public string PicturePath { get; set; }
    }
}