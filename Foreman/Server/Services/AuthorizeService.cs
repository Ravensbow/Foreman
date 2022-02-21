using Foreman.Shared.Services;
using Foreman.Server.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
                return httpContext.User.IsInRole("Admin") || httpContext.User.HasClaim("CategoryManager", categoryId.Value.ToString());
            else
                return httpContext.User.IsInRole("Admin");
        }

        public bool CanCreateCategory(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                //Jesli jest InstitutionManager to moze tworzyc Kategorie w danej instytucji
                int? institutionId = db.CourseCategories.Find(categoryId.Value)?.InstitutionId;
                if (!institutionId.HasValue)
                    return httpContext.User.IsInRole("Admin");
                return httpContext.User.IsInRole("Admin") || (httpContext.User.HasClaim("Institution", institutionId.Value.ToString()) && httpContext.User.HasClaim("InstitutionManager", institutionId.Value.ToString()));
            }
            else
                return httpContext.User.IsInRole("Admin");
        }
        public bool CanEditCategory(int categoryId)
        {
            int? institutionId = db.CourseCategories.Find(categoryId)?.InstitutionId;
            if (!institutionId.HasValue)
                return httpContext.User.IsInRole("Admin");
            return 
                httpContext.User.IsInRole("Admin") || 
                (
                    httpContext.User.HasClaim("Institution", institutionId.Value.ToString()) 
                    && 
                    (
                        httpContext.User.HasClaim("InstitutionManager", institutionId.Value.ToString()) 
                        || 
                        httpContext.User.HasClaim("CategoryManager", categoryId.ToString())
                     )
                );
        }

        public bool CanEditCourse(int courseId)
        {
            int? categoryId = db.Courses.Find(courseId)?.CourseCategoryId;
            return httpContext.User.IsInRole("Admin") || (httpContext.User.HasClaim("CourseManager", courseId.ToString()) || (categoryId.HasValue && httpContext.User.HasClaim("CategoryManager", categoryId.ToString())));
        }

        public bool CanViewCategory(int categoryId)
        {
            int? institution = db.CourseCategories.Find(categoryId)?.InstitutionId;
            if (httpContext.User.IsInRole("Admin") || !institution.HasValue || httpContext.User.HasClaim("Institution", institution.Value.ToString()))
                return true;
            return false;
        }

        public bool CanViewCourse(int courseId)
        {
            int? institution = db.Courses.Find(courseId)?.InstitutionId;
            if (httpContext.User.IsInRole("Admin") || !institution.HasValue || httpContext.User.HasClaim("Institution", institution.Value.ToString()))
                return true;
            return false;
        }

        public bool CanAddInstitution()
        {
            // To chyba tylko admin moze w sumie
            return httpContext.User.IsInRole("Admin");
        }

        public bool CanEditInstitution(int institutionId)
        {
            bool institution = db.Institutions.Any(x => x.Id == institutionId);

            if (!institution)
                return false;

            return httpContext.User.IsInRole("Admin")
                ||
                (
                    httpContext.User.HasClaim("Institution", institutionId.ToString())
                    && httpContext.User.HasClaim("InstitutionManager", institutionId.ToString())
                );
        }
    }
}
