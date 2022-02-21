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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
//using Foreman.Shared.Filters;

namespace FilePlugin.Controllers
{
    [ApiController]
    [Route("FilePlugin/[controller]")]
    public class ManagementController : ControllerBase
    {
        private FilePluginContext _context;
        private ICourseService _courseService;
        private IAuthorizeService _authorizeService;
        private IPluginService _pluginService;
        private IFileService _fileService;

        public ICourseService CourseService { get { return _courseService; } }
        public IAuthorizeService AuthorizeService { get { return _authorizeService; } }
        public IPluginService PluginService { get { return _pluginService; } }
        public ManagementController(FilePluginContext c, ICourseService courseService, IAuthorizeService authorizeService, IPluginService pluginService, IFileService fileService)
        {
            _context = c;
            _courseService = courseService;
            _authorizeService = authorizeService;
            _pluginService = pluginService;
            _fileService = fileService;
        }

        [HttpGet("Version")]
        public object Version()
        {
            return Config.version;
        }

        [HttpPost("Add/{sectionId:int?}")]
        public IActionResult Add(Models.FileInstanceModel model, [FromRoute]int? sectionId)
        {
            System.Diagnostics.Debug.WriteLine("FILEPLUGIN_1");
            if (!AuthorizeService.CanEditCourse(model.CourseId))
                return Forbid("Brak uprawnień do edycji tego kursu");
            int? pluginId = PluginService.GetPluginId(Config.FilePlugin);
            if (pluginId == null)
                return NotFound();
            System.Diagnostics.Debug.WriteLine("FILEPLUGIN_2");
            model.CreatedDate = DateTime.Now;
            model.UserCreatorId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.File.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.File.Component = Config.FilePlugin;
            model.FileHash = _fileService.HashToString(model.File.FileData);
            System.Diagnostics.Debug.WriteLine("FILEPLUGIN_3");
            _context.Add(model as Data.FilePluginInstance);
            _context.SaveChanges();
            System.Diagnostics.Debug.WriteLine("FILEPLUGIN_4");
            model.File.ContextId = model.Id;
            _fileService.StoreFile(model.File);
            System.Diagnostics.Debug.WriteLine("FILEPLUGIN_5");
            var courseModule = new CourseModule { CourseId = model.CourseId, InstanceId = model.Id, IsVisible = true, PluginId = pluginId, CourseSectionId = sectionId };
            PluginService.AddPluginInstance(courseModule);
            System.Diagnostics.Debug.WriteLine("FILEPLUGIN_6");
            return Ok(courseModule.Id);
        }

        [HttpGet("{id}")]
        public FilePluginInstance? Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
