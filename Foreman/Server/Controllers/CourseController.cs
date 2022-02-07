using Microsoft.AspNetCore.Mvc;
using Foreman.Shared.Data.Courses;
using Foreman.Server.Data;
using Microsoft.AspNetCore.Authorization;

namespace Foreman.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public CourseController(ApplicationContext ap)
        {
            _context = ap;
        }
        [HttpGet("GetCourseById/{id}")]
        public ActionResult GetCourseById(int id)
        {
            var course = _context.Courses.Find(id);
            if(course.InstitutionId != null && !User.HasClaim("Institution",course.InstitutionId.ToString()))
                return Forbid();
            return Ok(course);
        }
    }
}
