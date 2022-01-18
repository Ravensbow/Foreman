using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisplayedText.Services;
using DisplayedText.Data;
namespace DisplayedText.Controllers
{
    [ApiController]
    [Route("displayedtext/[controller]")]
    public class DisplayedController : ControllerBase
    {
        private DisplayedTextContext _context;
        private DisplayedTextService _service;

        public DisplayedController(DisplayedTextContext c, DisplayedTextService s)
        {
            _context = c;
            _service = s;
        }

        [HttpGet("Version")]
        public object Version()
        {
            return $"Plugin Controller v 1.0 {_service.Test()}";
        }

        [HttpPost("Add")]
        public IActionResult Add(Text text)
        {
            text.CreatedDate = DateTime.Now;
            _context.Add(text);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("All")]
        public Text[] All()
        {
            return _context.Texts.ToArray();
        }
    }
}
