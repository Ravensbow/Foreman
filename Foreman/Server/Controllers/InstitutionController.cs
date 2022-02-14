using Foreman.Server.Data;
using Foreman.Server.Services;
using Foreman.Shared.Data.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Foreman.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InstitutionController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly AuthorizeService _authorizeService;

        public InstitutionController(ApplicationContext db, AuthorizeService authorizeService)
        {
            _context = db;
            _authorizeService = authorizeService;

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

        [HttpGet]
        public IActionResult GetInstitutions()
        {
            try
            {
                if (!_authorizeService.CanAddInstitution())
                    return Forbid();

                var itemList = _context.Institutions
                    .Include(i => i.Owner)
                    .Include(i => i.InstitutionRequests)
                    .ThenInclude(ir => ir.User)
                    .Include(i => i.Members)
                    .OrderBy(i => i.Id)
                    .ToList();

                return Ok(JsonConvert.SerializeObject(itemList, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                }));

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateInstitution(Institution institution)
        {
            try
            {
                if (!_authorizeService.CanAddInstitution())
                    return Forbid();

                _context.Institutions.Add(institution);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteInstitution(Institution institution)
        {
            try
            {
                if (!_authorizeService.CanDeleteInstitution(institution.Id))
                    return Forbid();
                _context.Institutions.Remove(institution);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateInstitution(Institution institution)
        {
            try
            {
                if (!_authorizeService.CanEditInstitution(institution.Id))
                {
                    return Forbid();
                }

                var updated = DataTool.Update<Institution>(institution, _context);

                return Ok(institution.Id);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
