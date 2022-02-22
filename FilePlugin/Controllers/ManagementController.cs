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
        
        [HttpPost("Delete/{instanceId:int}")]
        public IActionResult Delete(int instanceId)
        {
            var instance = _context.Files.Find(instanceId);
            if (instance == null)
                return NotFound();

            if (!AuthorizeService.CanEditCourse(instance.CourseId))
                return Forbid("Brak uprawnień do edycji tego kursu");
            var fileId = _fileService.GetFileInfo(instance.FileHash, instanceId, Config.FilePlugin).Id;
            _fileService.DeleteFile(fileId);
            _context.Files.Remove(instance);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("Add/{sectionId:int?}")]
        public IActionResult Add(Models.FileInstanceModel model, [FromRoute]int? sectionId)
        {
            if (!AuthorizeService.CanEditCourse(model.CourseId))
                return Forbid("Brak uprawnień do edycji tego kursu");
            int? pluginId = PluginService.GetPluginId(Config.FilePlugin);
            if (pluginId == null)
                return NotFound();
            model.CreatedDate = DateTime.Now;
            model.UserCreatorId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.File.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.File.Component = Config.FilePlugin;
            model.FileHash = _fileService.HashToString(_fileService.HashFunction(model.File.FileData));
            _context.Add(model as Data.FilePluginInstance);
            _context.SaveChanges();
            model.File.ContextId = model.Id;
            _fileService.StoreFile(model.File);
            var courseModule = new CourseModule { CourseId = model.CourseId, InstanceId = model.Id, IsVisible = true, PluginId = pluginId, CourseSectionId = sectionId };
            PluginService.AddPluginInstance(courseModule);
            return Ok(courseModule.Id);
        }

        [HttpGet("{id}")]
        public FilePluginInstance? Get(int id)
        {
            return _context.Files.Find(id);
        }
        [HttpGet("GetFileInfo/{contextId}")]
        public IActionResult GetFileInfo(int contextId)
        {
            var instance = _context.Files.Find(contextId);
            if (instance == null)
                return NotFound();
            var file = _fileService.GetFileInfo(instance.FileHash, contextId, Config.FilePlugin);
            if (file == null)
                return NotFound();
            return Ok(file);
        }
        [HttpGet("GetFile/{contextId}")]
        public IActionResult GetFile(int contextId)
        {
            var instance = _context.Files.Find(contextId);
            if (instance == null)
                return NoContent();
            var file = _fileService.GetFileInfo(instance.FileHash, contextId, Config.FilePlugin);
            var f = _fileService.GetFile(instance.FileHash);
            if (f.IsFailure || file == null)
                return this.NoContent();
            return new FileContentResult(f.Value, file.MimeType)
            {
                FileDownloadName = file.Filename
            };

        }
    }
}
