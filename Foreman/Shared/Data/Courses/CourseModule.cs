using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Courses
{
    public class CourseModule
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int? CourseSectionId { get; set; }
        public bool IsVisible { get; set; }
        public int? PluginId { get; set; }
        public int InstanceId { get; set; }

        public Course Course { get; set; }
        public CourseSection CourseSection { get; set; }
    }
}
