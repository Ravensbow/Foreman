using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Services
{
    public interface IAuthorizeService
    {
        bool CanViewCourse(int courseId);
        bool CanEditCourse(int courseId);
        bool CanAddCourse(int? categoryId);
        bool CanViewCategory(int categoryId);
    }
}
