﻿using Microsoft.AspNetCore.Mvc;
using Foreman.Shared.Data.Courses;
using Foreman.Server.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Foreman.Shared.Models;
using Foreman.Shared.Models.Category;
using MudBlazor;
using System.Collections.Generic;

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
            var course = _context.Courses.Include(x=>x.Category).Include(x => x.CourseModules).Include(x => x.CourseSections).ThenInclude(s => s.CourseModules).SingleOrDefault(x => x.Id == id);
            if (course.InstitutionId != null && !User.HasClaim("Institution", course.InstitutionId.ToString()))
                return Forbid();
            return Ok(course);
        }

        [HttpGet("GetCategory/{categoryId}")]
        public IActionResult GetCategoryById(int categoryId)
        {
            try
            {
                string[] institutions = User.Claims.Where(c => c.Type == "Institution").Select(c => c.Value).ToArray();

                var category = _context.CourseCategories
                .SingleOrDefault(x => x.Id == categoryId && (x.InstitutionId == null || institutions.Contains(x.InstitutionId.Value.ToString())));


                return Ok(category);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
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
                    var temp = _context.CourseCategories.Include(x => x.Courses).Where(x =>
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
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("GetCourses/{categoryId}")]
        public IActionResult GetCourses(int categoryId)
        {
            try
            {
                string[] institutions = User.Claims.Where(c => c.Type == "Institution").Select(c => c.Value).ToArray();
                var temp = _context.Courses.Include(x => x.Category).Where(x =>
                          x.CourseCategoryId == categoryId
                          && x.IsVisible == true
                          && (x.InstitutionId == null || institutions.Contains(x.InstitutionId.Value.ToString())))
                    .OrderBy(x => (x.InstitutionId == null) ? 0 : 1).ThenBy(x => x.FullName).ToList();
                return Ok(JsonConvert.SerializeObject(temp, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                }));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("GetBreadcrumbs/{id}/{isCourse}")]
        public IActionResult GetBreadcrumbs(int id, bool isCourse)
        {
            var bread = new List<BreadcrumbItem>();
            Course course = null;
            CourseCategory category = null;
            if (isCourse)
            {
                course = _context.Courses.Include(c => c.Category).ThenInclude(cc => cc.ParentCategory).SingleOrDefault(x => x.Id == id);

                if (course == null)
                    return NotFound("Nie istnieje kategoria");

                bread.Add(new BreadcrumbItem(course.ShortName, "/course/show/" + course.Id));
                if (course.CourseCategoryId.HasValue)
                    bread.Add(new BreadcrumbItem(course.Category.Name, "/category/show/" + course.CourseCategoryId));
            }
            else
            {
                category = _context.CourseCategories.Include(c => c.ParentCategory).SingleOrDefault(x => x.Id == id);
                if (category == null)
                    return NotFound("Nie istnieje kategoria");
                bread.Add(new BreadcrumbItem(category.Name, "/category/show/" + category.Id));
            }

            CourseCategory cc = isCourse ? course?.Category?.ParentCategory : category?.ParentCategory;
            while (cc?.Id != null)
            {
                bread.Add(new BreadcrumbItem(cc.Name, "/category/show/" + cc.Id));

                cc = cc.ParentCategory;
            }
            bread.Add(new BreadcrumbItem("All categories", "/category/show"));
            bread.Reverse();
            return Ok(JsonConvert.SerializeObject(bread, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            }));

        }

        [HttpPost("CreateCategory")]
        public IActionResult CreateCategory(CategoryModel model)
        {
            try
            {
                string[] insitutions = User.Claims.Where(c => c.Type == "Institution").Select(c => c.Value).ToArray();
                if (model.InstitutionId != null && !insitutions.Contains(model.InstitutionId.Value.ToString()))
                    return Problem("Cannot create a category for institution that user is not a part of.");

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
        public IActionResult EditCategory(CategoryModel model)
        {
            try
            {
                string[] insitutions = User.Claims.Where(c => c.Type == "Institution").Select(c => c.Value).ToArray();
                if (model.InstitutionId != null && !insitutions.Contains(model.InstitutionId.Value.ToString()))
                    return Problem("Cannot edit a category for institution that user is not a part of.");


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

                string[] insitutions = User.Claims.Where(c => c.Type == "Institution").Select(c => c.Value).ToArray();
                if (!insitutions.Contains(dRecord.InstitutionId.Value.ToString()))
                    return Problem("Cannot create a category for institution that user is not a part of.");

                _context.CourseCategories.Remove(dRecord);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("SearchCategory/{search?}")]
        public IActionResult SearchCategory(string search)
        {
            List<CourseCategory> categories;
            string[] institutions = User.Claims.Where(c => c.Type == "Institution").Select(c => c.Value).ToArray();
            string[] categoryManager = User.Claims.Where(c => c.Type == "CategoryManager").Select(c => c.Value).ToArray();
            if (string.IsNullOrEmpty(search))
            {
                categories =_context.CourseCategories
                .Where(x => x.IsVisible == true && 
                    categoryManager.Contains(x.Id.ToString()))
                .ToList();
            }
            else
            {
                categories =_context.CourseCategories
                    .Where(x => x.IsVisible == true && x.Name.StartsWith(search) &&
                        categoryManager.Contains(x.Id.ToString()))
                    .ToList();
            }

            return Ok(JsonConvert.SerializeObject(categories, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            }));

        }
    }
}
