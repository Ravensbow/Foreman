﻿using Foreman.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Linq;
using System;
using System.Security.Policy;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Foreman.Shared.Services;

namespace Foreman.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PluginController : ControllerBase
    {
        private ApplicationContext db;
        private IAuthorizeService _authorizeService;

        public IAuthorizeService AuthorizeService { get { return _authorizeService; } }

        public PluginController(ApplicationContext ac, IAuthorizeService authorizeService)
        {
            db = ac;
            _authorizeService = authorizeService;   
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
        [HttpPost("AddModule")]
        public IActionResult AddInstance(Shared.Data.Courses.CourseModule cm)
        {
            if (AuthorizeService.CanEditCourse(cm.CourseId))
                return Forbid("Brak uprawnień do edycji tego kursu");
            db.CourseModules.Add(cm);
            db.SaveChanges();
            return Ok();
        }
    }
}
