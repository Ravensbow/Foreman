using Foreman.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Linq;
using System;
using System.Security.Policy;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Foreman.Shared.Services;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Foreman.PluginManager;

namespace Foreman.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PluginController : ControllerBase
    {
        private ApplicationContext db;
        private IAuthorizeService _authorizeService;
        private IPluginService _pluginService;
        private Microsoft.Extensions.DependencyInjection.IServiceCollection services = null;

        public IAuthorizeService AuthorizeService { get { return _authorizeService; } }
        public IPluginService PluginService { get { return _pluginService; } }

        public PluginController(ApplicationContext ac, IAuthorizeService authorizeService, IPluginService pluginService, Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            db = ac;
            _authorizeService = authorizeService;   
            _pluginService = pluginService;
            this.services = services;
        }
        [HttpGet("GetByName/{name}")]
        public byte[] GetByName(string name)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory+$"Plugins\\{name}\\{name}.dll";
            var dll = System.IO.File.ReadAllBytes(path);
            var test = User.HasClaim("Institution", "1");
            return dll;
        }
        [HttpGet("PluginNames")]
        public string[] PluginNames()
        {
            return db.Plugins.Select(x=>x.Name).ToArray();
        }
        [HttpGet("GetPlugins")]
        public IActionResult GetPlugins()
        {
            return Ok(PluginService.GetAll());
        }
        [HttpPost("AddModule")]
        public IActionResult AddInstance(Shared.Data.Courses.CourseModule cm)
        {
            if (AuthorizeService.CanEditCourse(cm.CourseId))
                return Forbid("Brak uprawnień do edycji tego kursu");
            db.CourseModules.Add(cm);
            db.SaveChanges();
            return Ok();
        }
        [HttpGet("PluginNameById/{id}")]
        public IActionResult PluginNameById(int id)
        {
            return Ok(PluginService.GetPluginName(id));
        }
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            //Dodac Authorize

            Shared.Data.Plugin.Plugin plugin = db.Plugins.Find(id);
            if(plugin==null)
                return NotFound();

            try
            {
                //Usunąć CourseModule z  danym pluginem
                db.CourseModules.RemoveRange(db.CourseModules.Where(x => x.PluginId == plugin.Id));
                //Usunąć wpis z tabeli Plugins
                db.Plugins.Remove(plugin);
                db.SaveChanges();
            }
            catch (Exception e)
            {

                return Problem(e.Message);
            }

            //Usunąć tabele wtyczki
            Assembly assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + @"\Plugins\" + plugin.Name + @"\" + plugin.Name + ".dll");
            var part = new AssemblyPart(assembly);
            var atypes = assembly.GetTypes();
            var pluginClass = atypes.SingleOrDefault(t => t.GetInterface(nameof(IPlugin)) != null);

            var obj = Activator.CreateInstance(pluginClass);
            var pluginTabels = pluginClass.GetMethod(nameof(IPlugin.GetPluginDbTables)).Invoke(obj, new object[] { services }) as List<string>;

            if (pluginTabels != null && pluginTabels.Count > 0)
            {
                int count = 0;
                pluginTabels.ForEach(t => count += db.Database.ExecuteSqlRaw("DROP TABLE dbo."+t));
            }

            //Usunąć plik wtyczki z folderem z folderu Plugins
            Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Plugins\" + plugin.Name, true);


            return Ok();
        }

    }
}
