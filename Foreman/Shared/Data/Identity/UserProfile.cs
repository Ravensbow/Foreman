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

    }

    public class UserRole : IdentityUserRole<int>
    {

    }

    public class UserClaim : IdentityUserClaim<int>
    {

    }

    public class UserLogin : IdentityUserLogin<int>
    {

    }

    public class Role : IdentityRole<int>
    {

    }

    public class UserStore : UserStore<UserProfile, Role, ApplicationContext>
}
