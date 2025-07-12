using DataAccess;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Biddify.Pages.Authen
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<UserEntity> signInManager;
        private readonly UserManager<UserEntity> userManager;
        private readonly IEmailService emailService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            SignInManager<UserEntity> _signInManager, 
            IEmailService _emailService, 
            UserManager<UserEntity> _userManager,
            ILogger<LoginModel> logger)
        {
            signInManager = _signInManager;
            emailService = _emailService;
            userManager = _userManager;
            _logger = logger;
        }

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; } = new LoginViewModel();

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; } = "/Home";

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? ReturnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Log received form data
                _logger.LogInformation(
                    "Received login attempt - Email: {Email}, Password provided: {HasPassword}, RememberMe: {RememberMe}",
                    LoginViewModel?.Email,
                    !string.IsNullOrEmpty(LoginViewModel?.Password),
                    LoginViewModel?.RememberMe);

                // Proceed with login attempt regardless of ModelState
                var result = await signInManager.PasswordSignInAsync(
                    LoginViewModel.Email,
                    LoginViewModel.Password,
                    LoginViewModel.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} logged in successfully", LoginViewModel.Email);
                    var user = await userManager.FindByEmailAsync(LoginViewModel.Email);

                    if (user == null)
                    {
                        _logger.LogWarning("User not found after successful login");
                        ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
                        return Page();
                    }

                    // Add role claim and identity
                    var claims = new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role.ToString()),
                        new System.Security.Claims.Claim("UserRole", user.Role.ToString()),
                        new System.Security.Claims.Claim("UserId", user.Id),
                        new System.Security.Claims.Claim("UserEmail", user.Email),
                        new System.Security.Claims.Claim("UserDisplayName", user.DisplayName)
                    };

                    // Remove existing claims and sign out
                    await signInManager.SignOutAsync();
                    var existingClaims = await userManager.GetClaimsAsync(user);
                    if (existingClaims.Any())
                    {
                        await userManager.RemoveClaimsAsync(user, existingClaims);
                    }

                    // Add new claims
                    await userManager.AddClaimsAsync(user, claims);

                    // Sign in again with new claims
                    await signInManager.SignInAsync(user, LoginViewModel.RememberMe);

                    if (!user.EmailConfirmed || user.Status == EUserStatus.Inactive)
                    {
                        if (await emailService.IsEmailVerifiedAsync(user.Email))
                        {
                            user.EmailConfirmed = true;
                            user.Status = EUserStatus.Active;
                            await userManager.UpdateAsync(user);
                        }
                        else
                        {
                            _logger.LogWarning("User {Email} account not active", LoginViewModel.Email);
                            ModelState.AddModelError(string.Empty, "Your account is not active. Please check your email to verify your account.");
                            await emailService.VerifyEmailAddressAsync(user.Email);
                            return Page();
                        }
                    }

                    _logger.LogInformation("Redirecting user to: {ReturnUrl}", ReturnUrl);
                    return LocalRedirect(ReturnUrl ?? "/Home");
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User {Email} account locked out", LoginViewModel.Email);
                    ModelState.AddModelError(string.Empty, "Account locked out. Please try again later.");
                    return Page();
                }

                _logger.LogWarning("Invalid login attempt for user {Email}", LoginViewModel.Email);
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login attempt");
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                return Page();
            }
        }
    }
}
