using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilePlugin.Services;
using FilePlugin.Data;
using Foreman.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Foreman.Shared.Data.Courses;
//using Foreman.Shared.Filters;

namespace FilePlugin.Controllers
{
    [ApiController]
    [Route("FilePlugin/[controller]")]
    public class ManagementController : ControllerBase
    {
        public ManagementController()
        {
            
        }

        [HttpGet("Version")]
        public object Version()
        {
            return Config.version;
        }

        [HttpPost("Add/{sectionId:int?}")]
        public IActionResult Add(FilePluginInstance model, [FromRoute]int? sectionId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public FilePluginInstance? Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
