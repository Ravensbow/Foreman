using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Courses
{
    public class CourseSection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public int CourseId { get; set; }

        public Course Course { get; set; }
        public ICollection<CourseModule> CourseModules { get; set; }
    }
}
