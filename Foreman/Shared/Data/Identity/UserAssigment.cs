using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Identity
{
    public class UserAssigment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AssigmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime AssigmentStart { get; set; }
        public DateTime AssigmentEnd { get; set; }

        public Courses.Assigment Assigment { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
