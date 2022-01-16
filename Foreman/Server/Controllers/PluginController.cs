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
            string path = AppDomain.CurrentDomain.BaseDirectory+$"Plugins\\{name}\\{name}Razor.dll";
            //return System.IO.File.Open(path,FileMode.Open);
            var assembly = Assembly.LoadFile(path);
            var test = System.IO.File.ReadAllBytes(path);
            return test;
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
