using Microsoft.AspNetCore.Mvc;
using Foreman.Shared.Data.Courses;
using Foreman.Server.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Foreman.Shared.Models;
using Foreman.Shared.Models.Category;

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
            var course = _context.Courses.Include(x => x.CourseModules).Include(x=>x.CourseSections).ThenInclude(s=>s.CourseModules).SingleOrDefault(x=>x.Id==id);
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
                    var temp = _context.CourseCategories.Include(x => x.Courses).Where(x =>
                            x.ParentCategoryId == null
                            && x.IsVisible == true
                            && (x.InstitutionId == null || institutions.Contains(x.InstitutionId.Value.ToString())))
                        .OrderBy(x => (x.InstitutionId == null) ? 0 : 1).ThenBy(x => x.Name).ToList();
                    return Ok(JsonConvert.SerializeObject(temp, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    }));
                }
                else
                {
                    var temp = _context.CourseCategories.Include(x=>x.Courses).Where(x =>
                            x.ParentCategoryId == id.Value
                            && x.IsVisible == true
                            && (x.InstitutionId == null || institutions.Contains(x.InstitutionId.Value.ToString())))
                        .OrderBy(x => (x.InstitutionId == null) ? 0 : 1).ThenBy(x => x.Name).ToList();
                    return Ok(JsonConvert.SerializeObject(temp, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    }));
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("CreateCategory")]
        public IActionResult CreateCategory(CategoryModel model)
        {
            try
            {
                CourseCategory newRecord = new CourseCategory()
                {
                    InstitutionId = model.InstitutionId,
                    ParentCategoryId = model.ParentCategoryId,
                    Name = model.Name,
                    Description = model.Description,
                    IsVisible = model.IsVisible,
                    CreatedDate = DateTime.Now
                };
                _context.CourseCategories.Add(newRecord);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("EditCategory")]
        public IActionResult EditCategory( CategoryModel model)
        {
            try
            {
                var updRecord = _context.CourseCategories.Where(c => c.Id == model.Id).Single();

                updRecord.Name = model.Name;
                updRecord.Description = model.Description;
                updRecord.IsVisible = model.IsVisible;
                updRecord.InstitutionId = model.InstitutionId;
                updRecord.ParentCategoryId = model.ParentCategoryId;

                updRecord.ModifiedDate = DateTime.Now;

                _context.Update(updRecord);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int categoryId)
        {
            try
            {
                var dRecord = _context.CourseCategories.Where(x => x.Id == categoryId).Single();
                _context.CourseCategories.Remove(dRecord);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("SearchCategory")]
        public IActionResult SearchCategory(string search)
        {
            var categories = _context.CourseCategories
                .Where(x => x.IsVisible == true && x.Name.Contains(search))
                .ToList();
            return Ok(categories);

        }
    }
}
