using Foreman.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Linq;
using System;
using System.Security.Policy;
using System.IO;

namespace Foreman.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PluginController : ControllerBase
    {
        private ApplicationContext db;
        public PluginController(ApplicationContext ac)
        {
            db = ac;
        }
        [HttpGet("GetByName/{name}")]
        public byte[] GetByName(string name)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory+$"Plugins\\{name}\\{name}.dll";
            var dll = System.IO.File.ReadAllBytes(path);

            return dll;
        }
        [HttpGet("PluginNames")]
        public string[] PluginNames()
        {
            return db.Plugins.Select(x=>x.Name).ToArray();
        }
    }
}
