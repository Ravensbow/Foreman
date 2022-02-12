using Foreman.Shared.Services;
using Foreman.Server.Data;
using Microsoft.AspNetCore.Http;

namespace Foreman.Server.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly HttpContext httpContext;
        private readonly ApplicationContext db;
        public AuthorizeService(IHttpContextAccessor httpc, ApplicationContext ap)
        {
            httpContext = httpc.HttpContext;
            db = ap;
        }

        public bool CanAddCourse(int? categoryId)
        {
            if (categoryId.HasValue)
                return httpContext.User.HasClaim("CategoryManager", categoryId.Value.ToString());
            else
                return httpContext.User.IsInRole("Admin");
        }

        public bool CanEditCourse(int courseId)
        {
            int? categoryId = db.Courses.Find(courseId)?.CourseCategoryId;
            return (httpContext.User.HasClaim("CourseManager", courseId.ToString()) || (categoryId.HasValue && httpContext.User.HasClaim("CategoryManager", categoryId.ToString())));
        }

        public bool CanViewCategory(int categoryId)
        {
            int? institution = db.CourseCategories.Find(categoryId)?.InstitutionId;
            if (!institution.HasValue || httpContext.User.HasClaim("Institution", institution.Value.ToString()))
                return true;
            return false;
        }

        public bool CanViewCourse(int courseId)
        {
            int? institution = db.Courses.Find(courseId)?.InstitutionId;
            if (!institution.HasValue || httpContext.User.HasClaim("Institution", institution.Value.ToString()))
                return true;
            return false;
        }
    }
}
