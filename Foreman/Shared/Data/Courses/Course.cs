using System;
using System.Collections.Generic;
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
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public CourseCategory Category { get; set; }
        public ICollection<CourseModule> CourseModules { get; set; }
        public ICollection<CourseSection> CourseSections{ get; set; }

    }
}
