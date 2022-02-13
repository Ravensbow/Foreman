//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Identity
{
    public class UserProfile : IdentityUser<int>
    {
        public int? InstitutionId { get; set; }
        public int? OwnedInstitutionId { get; set; }
        public Institution Institution { get; set; }
        public Institution OwnedInstitution { get; set; }
        public ICollection<UserAssigment> UserAssigments { get; set; }
        public ICollection<Courses.CategoryAssigment> CategoryAssigments { get; set; }
        public ICollection<InstitutionRequest> InstitutionRequests { get; set; }
    }

    public class Role : IdentityRole<int>
    {
        
    }
}
