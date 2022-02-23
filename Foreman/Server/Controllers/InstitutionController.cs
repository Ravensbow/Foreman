using Foreman.Server.Data;
using Foreman.Server.Services;
using Foreman.Shared.Data.Identity;
using Foreman.Shared.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Foreman.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InstitutionController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly IAuthorizeService _authorizeService;
        private readonly UserManager<UserProfile> _userManager;

        public InstitutionController(ApplicationContext db, IAuthorizeService authorizeService, UserManager<UserProfile> manager)
        {
            _context = db;
            _authorizeService = authorizeService;
            _userManager = manager;

        }

        [HttpGet]
        public IActionResult GetPotentialInstitutionManagers()
        {
            try
            {
                var adminsList = _userManager.GetUsersInRoleAsync("Admin")
                    .GetAwaiter()
                    .GetResult()
                    .Select(x => x.Id);

                var itemList = _context.Users
                    .Where(u => u.OwnedInstitution == null && !adminsList.Contains(u.Id))
                    .ToList();
                return Ok(JsonConvert.SerializeObject(itemList));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{userId:int}")]
        public IActionResult GetInstitutionForManager(int userId)
        {
            try
            {
                var item = _context.Institutions
                    .AsNoTracking()
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
                    .AsNoTracking()
                    .Include(i => i.Owner)
                    .Include(i => i.Members)
                    .Include(i => i.InstitutionRequests)
                    .ThenInclude(ir => ir.User)
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

                var itemList = _context.InstitutionRequests
                .AsNoTracking()
                .Include(ir => ir.Institution)
                .Include(ir => ir.User)
                .OrderBy(ir => ir.RequestDate)
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

        [HttpPost("{userId}")]
        public IActionResult AcceptRequest([FromRoute] int userId, [FromBody] int institutionId)
        {
            try
            {
                if (!_authorizeService.CanEditInstitution(institutionId))
                    return Forbid();

                var request = _context.InstitutionRequests
                    .Include(r => r.Institution)
                    .Include(r => r.User)
                    .Single(r => r.UserId == userId && r.InstitutionId == institutionId);

                request.User.InstitutionId = institutionId;
                request.IsAccepted = true;
                request.AnswerDate = DateTime.Now;
                var institutionClaim = new Claim("Institution", request.InstitutionId.ToString());
                _userManager.AddClaimAsync(request.User, institutionClaim).GetAwaiter().GetResult();

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("{userId}")]
        public IActionResult KickUser([FromRoute] int userId, [FromBody] int institutionId)
        {
            try
            {
                if (!_authorizeService.CanEditInstitution(institutionId))
                    return Forbid();
                var institution = _context.Institutions
                    .Include(i => i.Members)
                    .Include(i => i.Owner)
                    .Single(i => i.Id == institutionId);
                var user = institution.Members.Single(u => u.Id == userId);
                institution.Members.Remove(user);
                if (institution.OwnerId == user.Id)
                    institution.Owner = null;

                _userManager.RemoveClaimAsync(user, new Claim("Institution", institution.Id.ToString()));

                _context.SaveChanges();


                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("{userId}")]
        public IActionResult RefuseRequest([FromRoute] int userId, [FromBody] int institutionId)
        {
            try
            {
                if (!_authorizeService.CanEditInstitution(institutionId))
                    return Forbid();
                var request = _context.InstitutionRequests.Where(ir => ir.UserId == userId && ir.InstitutionId == institutionId).Single();

                request.IsAccepted = false;
                request.AnswerDate = DateTime.Now;

                _context.SaveChanges();

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

                _context.Institutions.Update(institution);
                if(institution.Owner != null)
                {
                    var navigationProp = new List<UserProfile>();
                    navigationProp.Add(institution.Owner);
                    institution.Members = navigationProp;
                }
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
                var model = _context.Institutions
                    .Include(i => i.Members)
                    .Include(i => i.Owner)
                    .Single(i => i.Id == institution.Id);

                model.Name = institution.Name;
                model.Owner = institution.Owner;
                model.Description = institution.Description;
                model.ModifiedDate = DateTime.Now;
                if(institution.Owner != null && !model.Members.Contains(institution.Owner))
                    model.Members.Add(institution.Owner);
                _context.Update(model);
                _context.SaveChanges();
              
                return Ok(institution.Id);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
