using Foreman.Shared.Data.Identity;
using Foreman.Shared.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Foreman.Server.Data;

namespace Foreman.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<UserProfile> _userManager;
        private readonly IUserStore<UserProfile> _userStore;
        private readonly IUserEmailStore<UserProfile> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationContext _context;

        public AccountController(ApplicationContext context, SignInManager<UserProfile> signInManager, ILogger<LoginModel> logger, IUserStore<UserProfile> userStore, UserManager<UserProfile> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userStore = userStore;
            _userManager = userManager;
            _emailStore = GetEmailStore();
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return Ok(returnUrl);
            }
            else
            {
                return this.Problem("Invalid login attempt");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return this.Problem("User is not authenticated.");

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (user == null)
                return this.Problem("Unknow problem encountered by the server.");
            if(user.Email != model.Email)
            {
                string token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                bool success = (await _userManager.ChangeEmailAsync(user, model.Email, token)).Succeeded;

                if (!success)
                    return Problem("Unknow problem encountered by the server.");
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return this.Problem("User is not authenticated.");

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (user == null)
                return this.Problem("Unknow problem encountered by the server.");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
            if(result.Succeeded)
                return Ok(result);

            return Problem("Uknown problem encountered by the server");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var user = CreateUser();
            user.FirstName = registerModel.FirstName;
            user.LastName = registerModel.LastName;

            await _userStore.SetUserNameAsync(user, registerModel.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, registerModel.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if (registerModel.Institution != null)
                {
                    _context.InstitutionRequests.Add(new InstitutionRequest 
                    {
                        UserId = int.Parse(userId), 
                        InstitutionId = registerModel.Institution.Id,
                        RequestDate = DateTime.Now
                    });
                    _context.SaveChanges();
                }
                //var institutionClaim = new Claim("Institution", "1");
                //await _userManager.AddClaimAsync(user, institutionClaim);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
                await ConfirmEmail(int.Parse(userId), code);
                await _emailSender.SendEmailAsync(registerModel.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                return Ok( new { code, userId});
            }
            return Problem("Cos nie tego");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userid, string code)
        {
            var user = await _userManager.FindByIdAsync(userid.ToString());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userid}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return Problem("Error confirming your email.");
            }
        }
        public string IsAuth()
        {
            return User.Identity?.IsAuthenticated.ToString();
        }
        private UserProfile CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UserProfile>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserProfile)}'. " +
                    $"Ensure that '{nameof(UserProfile)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private IUserEmailStore<UserProfile> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<UserProfile>)_userStore;
        }
    }
}
