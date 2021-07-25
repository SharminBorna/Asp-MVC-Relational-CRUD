using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using work_01.CustomValidation;

namespace work_01.ViewModels
{
    public class TraineeVM
    {
        public int TraineeId { get; set; }
        [Required(ErrorMessage = "Please enter trainee name."), StringLength(50), Display(Name = "Trainee Name")]
        public string TraineeName { get; set; }
        [Required(ErrorMessage = "Please enter Join Date."), Display(Name = "Join Date")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomJoinDate(ErrorMessage = "Join Date must be less than or equal to Today's Date.")]
        public DateTime JoinDate { get; set; }
        public int CourseId { get; set; }
        public string cName { get; set; }
        public string PicturePath { get; set; }
        public HttpPostedFileBase Picture { get; set; }
    }
}