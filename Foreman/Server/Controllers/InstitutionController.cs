using Foreman.Server.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Foreman.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InstitutionController : Controller
    {

        private readonly ApplicationContext _context;

        public InstitutionController(ApplicationContext db)
        {
            _context = db;
        }

        [HttpGet("{userId}")]
        public IActionResult GetInstitutionForManager(int userId)
        {
            try
            {
                var item = _context.Institutions.Single(i => i.OwnerId == userId);
                return Ok(item);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
