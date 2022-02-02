using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisplayedText.Services;
using DisplayedText.Data;
using Foreman.Shared.Services;
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

        public ICourseService CourseService { get { return _courseService; } }

        public ManagementController(DisplayedTextContext c, DisplayedTextService s, ICourseService courseService)
        {
            _context = c;
            _service = s;
            _courseService = courseService;
        }

        [HttpGet("Version")]
        public object Version()
        {
            return $"Plugin Controller v 1.0 {_service.Test()}";
        }

        [HttpPost("Add")]
        //[TypeFilter(typeof(IsCourseManager), Arguments = new object[] { typeof(Text) })]
        //[IsCourseManager(typeof(Text))]
        public IActionResult Add(Text text)
        {
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
