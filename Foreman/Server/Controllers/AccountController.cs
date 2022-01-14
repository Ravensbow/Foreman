using Foreman.Shared.Data.Identity;
using Foreman.Shared.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

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

        public AccountController(SignInManager<UserProfile> signInManager, ILogger<LoginModel> logger, IUserStore<UserProfile> userStore, UserManager<UserProfile> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userStore = userStore;
            _userManager = userManager;
            _emailStore = GetEmailStore();
            _emailSender = emailSender;
            _logger = logger;
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
                return this.Problem("Invalid login attempt.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, registerModel.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, registerModel.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(registerModel.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                //{
                //    return RedirectToPage("RegisterConfirmation", new { email = registerModel.Email, returnUrl = returnUrl });
                //}
                //else
                //{
                //    await _signInManager.SignInAsync(user, isPersistent: false);
                //    return LocalRedirect(returnUrl);
                //}
                return Ok();
            }
            return Problem("Cos nie tego");
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
        //[HttpPost]
        //public IActionResult ChangePassword()
        //{
        //    return View();
        //}


    }
}
