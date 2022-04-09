using Foreman.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Foreman.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthorizeController : Controller
    {
        private readonly IAuthorizeService _authorizeService;   
        public IAuthorizeService AuthorizeService { get { return _authorizeService; } }
        public AuthorizeController(IAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService;
        }
        public bool CanViewCourse(int courseId)
        {
            return AuthorizeService.CanViewCourse(courseId);
        }
        public bool CanEditCourse(int courseId)
        {
            return AuthorizeService.CanEditCourse(courseId);
        }
        public bool CanViewCategory(int categoryId)
        {
            return AuthorizeService.CanViewCategory(categoryId);
        }
        public bool CanAddCourse(int? categoryId)
        {
            return AuthorizeService.CanAddCourse(categoryId);
        }
        public bool CanAddInstituion()
        {
            return AuthorizeService.CanAddInstitution();
        }
        public bool CanEditInstitution(int institutionId)
        {
            return AuthorizeService.CanEditInstitution(institutionId);
        }
    }
}
