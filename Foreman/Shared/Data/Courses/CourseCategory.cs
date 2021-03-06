using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Courses
{
    public class CourseCategory
    {
        public int Id { get; set; }
        public int? ParentCategoryId { get; set; }
        public int? InstitutionId { get; set; }
        public bool IsVisible { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Identity.Institution Institution { get; set; }
        public CourseCategory ParentCategory { get; set; }
        public ICollection<Course> Courses { get; set;}
        public ICollection<CategoryAssigment> CategoryAssigments { get; set; }
    }
}
