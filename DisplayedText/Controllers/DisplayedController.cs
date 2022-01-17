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

        [HttpGet("Add/{text}")]
        public IActionResult Add(string text)
        {
            _context.Add(new Text { Content = text });
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
