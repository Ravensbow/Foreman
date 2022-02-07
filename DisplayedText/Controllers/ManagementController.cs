using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisplayedText.Services;
using DisplayedText.Data;
using Foreman.Shared.Services;
using Microsoft.AspNetCore.Authorization;
//using Foreman.Shared.Filters;

namespace DisplayedText.Controllers
{
    [ApiController]
    [Route("displayedtext/[controller]")]
    public class ManagementController : ControllerBase
    {
        private DisplayedTextContext _context;
        private DisplayedTextService _service;
        private ICourseService _courseService;
        private IAuthorizeService _authorizeService;

        public ICourseService CourseService { get { return _courseService; } }
        public IAuthorizeService AuthorizeService { get { return _authorizeService; } }

        public ManagementController(DisplayedTextContext c, DisplayedTextService s, ICourseService courseService, IAuthorizeService authorizeService)
        {
            _context = c;
            _service = s;
            _courseService = courseService;
            _authorizeService = authorizeService;
        }

        [HttpGet("Version")]
        public object Version()
        {
            return $"Plugin Controller v 1.0 {_service.Test()}";
        }

        [HttpPost("Add")]
        [Authorize]
        public IActionResult Add(Text text)
        {
            if (AuthorizeService.CanEditCourse(text.CourseId))
                return Forbid("Brak uprawnień do edycji tego kursu");
            text.CreatedDate = DateTime.Now;
            _context.Add(text);
            _context.SaveChanges();
            return Ok(text.Id);
        }

        [HttpGet("All")]
        public Text[] All()
        {
            return _context.Texts.ToArray();
        }
        [HttpGet("{id}")]
        public Text? Get(int id)
        {
            return _context.Texts.Find(id);
        }
        [HttpGet("GetCategory/{id}")]
        public IActionResult GetCategory(int id)
        {
            var categoryId = _courseService.GetCategoryId(id);
            return Ok(categoryId);
        }
    }
}
