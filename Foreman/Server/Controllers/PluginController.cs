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
        public Tuple<byte[],byte[]> GetByName(string name)
        {
            string pathRazor = AppDomain.CurrentDomain.BaseDirectory+$"Plugins\\{name}\\{name}Razor.dll";
            string path = AppDomain.CurrentDomain.BaseDirectory+$"Plugins\\{name}\\{name}.dll";
            //return System.IO.File.Open(path,FileMode.Open);
            //var assembly = Assembly.LoadFile(path);
            var dll = System.IO.File.ReadAllBytes(path);
            var dllRazor = System.IO.File.ReadAllBytes(pathRazor);
            
            return new Tuple<byte[],byte[]>(dll, dllRazor);
            //var hash = new Hash(assembly);
            //var dllAsArray = (byte[])hash.GetType()
            //    .GetMethod("GetRawData", BindingFlags.Instance | BindingFlags.NonPublic)
            //    .Invoke(hash, new object[0]);
            //return dllAsArray;
        }
        [HttpGet("PluginNames")]
        public string[] PluginNames()
        {
            return db.Plugins.Select(x=>x.Name).ToArray();
        }
    }
}
