using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Courses
{
    public class Assigment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public char Type { get; set; }
        public string Password { get; set; }
        public int CohortId { get; set; }
        public int RoleId { get; set; }
        public Course Course { get; set; }
        public ICollection<Identity.UserAssigment> UserAssigments { get; set; }
    }
}
