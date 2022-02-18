using Foreman.Server.Data;
using Foreman.Server.Services;
using Foreman.Shared.Data.Identity;
using Foreman.Shared.Services;
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
        private readonly IAuthorizeService _authorizeService;

        public InstitutionController(ApplicationContext db, IAuthorizeService authorizeService)
        {
            _context = db;
            _authorizeService = authorizeService;

        }

        [HttpGet("{userId:int}")]
        public IActionResult GetInstitutionForManager(int userId)
        {
            try
            {
                var item = _context.Institutions
                    .Include(i => i.Members)
                    .Single(i => i.OwnerId == userId);

                return Ok(JsonConvert.SerializeObject(item, new JsonSerializerSettings
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

        [HttpGet("{institutionId:int}")]
        public IActionResult GetInstitution(int institutionId)
        {
            try
            {
                if (!_authorizeService.CanAddInstitution())
                    return Forbid();
                var item = _context.Institutions
                    .Include(i => i.Members)
                    .Include(i => i.InstitutionRequests)
                    .Single(i => i.Id == institutionId);
                return Ok(JsonConvert.SerializeObject(item, new JsonSerializerSettings
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

        [HttpGet("{institutionId:int}")]
        public IActionResult GetRequests(int institutionid)
        {
            try
            {
                if (!_authorizeService.CanEditInstitution(institutionid))
                    return Forbid();

                var itemList = _context.InstitutionRequests.Include(ir => ir.User)
                .OrderBy(ir => ir.RequestDate)
                .ToList();

                return Ok(JsonConvert.SerializeObject(itemList, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                }));
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AcceptRequest(int userId, int institutionId)
        {
            try
            {
                if (!_authorizeService.CanEditInstitution(institutionId))
                    return Forbid();
                var user = _context.Users.Where(u => u.Id == userId).Single();
                var request = _context.InstitutionRequests.Where(ir => ir.UserId == userId && ir.InstitutionId == institutionId).Single();

                user.InstitutionId = institutionId;
                request.IsAccepted = true;

                DataTool.Update(user, _context);
                DataTool.Update(request, _context);

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult KickUser(int userId, int institutionId)
        {
            try
            {
                if (!_authorizeService.CanEditInstitution(institutionId))
                    return Forbid();
                var user = _context.Users.Where(u => u.Id == userId && u.InstitutionId == institutionId).Single();

                user.InstitutionId = null;

                DataTool.Update(user, _context);

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult RefuseRequest(int userId, int institutionId)
        {
            try
            {
                if (!_authorizeService.CanEditInstitution(institutionId))
                    return Forbid();
                var request = _context.InstitutionRequests.Where(ir => ir.UserId == userId && ir.InstitutionId == institutionId).Single();

                request.IsAccepted = false;

                DataTool.Update(request, _context);

                return Ok();
            }
            catch (Exception ex)
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
                if (!_authorizeService.CanAddInstitution())
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
