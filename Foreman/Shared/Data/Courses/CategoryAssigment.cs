using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Courses
{
    public class CategoryAssigment
    {
        public int CateogryId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public CourseCategory Category { get; set; }
        public Identity.UserProfile UserProfile { get; set; }
    }
}
