using Microsoft.AspNetCore.Mvc;
using Foreman.Shared.Data.Courses;
using Foreman.Server.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult GetCourseById(int id)
        {
            var course = _context.Courses.Include(x => x.CourseModules).SingleOrDefault(x=>x.Id==id);
            if(course.InstitutionId != null && !User.HasClaim("Institution",course.InstitutionId.ToString()))
                return Forbid();
            return Ok(course);
        }
        [HttpGet("GetCategorys/{id?}")]
        public IActionResult GetCategorys(int? id)
        {
            try
            {
                string[] institutions = User.Claims.Where(c => c.Type == "Institution").Select(c => c.Value).ToArray();
                if (id == null)
                {
                    
                    return Ok(_context.CourseCategories.Where(x =>
                            x.ParentCategoryId == null
                            && x.IsVisible == true
                            && (x.InstitutionId == null || institutions.Contains(x.InstitutionId.Value.ToString())))
                        .ToList());
                }
                else
                {
                    return Ok(_context.CourseCategories.Where(x =>
                            x.ParentCategoryId == id.Value
                            && x.IsVisible == true
                            && (x.InstitutionId == null || institutions.Contains(x.InstitutionId.Value.ToString())))
                        .ToList()
                        .OrderBy(x=>(x.InstitutionId==null)?0:1).ThenBy(x=>x.Name));
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
