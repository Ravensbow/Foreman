using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Courses
{
    public class Course
    {
        public int Id { get; set; }
        public int? CourseCategoryId { get; set; }
        public int? InstitutionId { get; set; }
        [Required]
        [Display(Name = "Short name")]
        public string ShortName { get; set; }
        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Visible")]
        public bool IsVisible { get; set; }
        [Display(Name = "Start date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End date")]
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Identity.Institution Institution { get; set; }
        public CourseCategory Category { get; set; }
        public ICollection<CourseModule> CourseModules { get; set; }
        public ICollection<CourseSection> CourseSections{ get; set; }
        public ICollection<Assigment> Assigments{ get; set; }

    }
}
