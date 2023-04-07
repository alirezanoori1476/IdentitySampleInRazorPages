using IdentityManager.Models;
using IdentityManager.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers;

#nullable enable

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IEmailService _emailService;

    public AccountController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region Register

    [HttpGet]
    public Task<IActionResult> Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        var model = new RegisterViewModel();

        return Task.FromResult<IActionResult>(View(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUserClass
        {
            UserName = model.Email,
            Email = model.Email,
            Name = model.Name
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callBackUrl = Url.Action("ConfirmEmail", "Account", new
            {
                UserId = user.Id,
                Code = code
            }, protocol: HttpContext.Request.Scheme);

            await _emailService.SendEmail(model.Email, $"Please Confirm your account by clicking here: <a href=\\ + {callBackUrl} + \\>link</a>");

            await _signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(returnUrl);
        }

        AddErrors(result);

        return View(model);
    }

    #endregion

    #region Login

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
            return Redirect(returnUrl);

        if (result.IsLockedOut)
            return View("Lockout");

        ModelState.AddModelError(string.Empty, "Invalid login");

        return View(model);
    }

    [HttpGet]
    public Task<IActionResult> Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        return Task.FromResult<IActionResult>(View());
    }

    #endregion

    #region Logout

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    #endregion

    #region ForGetPassword

    [HttpGet]
    public IActionResult ForGetPassword()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ForGetPasswordConfirmation()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForGetPassword(ForgotPasswordModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return Redirect("ForGetPasswordConfirmation");

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        var callBackUrl = Url.Action("ResetPassword", "Account", new
        {
            UserId = user.Id,
            Code = code
        }, protocol: HttpContext.Request.Scheme);

        await _emailService.SendEmail(model.Email, $"Please reset your password by clicking here: <a href=\\ + {callBackUrl} + \\>link</a>");

        return Redirect("ForGetPasswordConfirmation");
    }

    #endregion

    #region Reset Password

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return Redirect("ResetPasswordConfirmation");

        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

        if (result.Succeeded)
            return Redirect("ResetPasswordConfirmation");

        AddErrors(result);

        return View();
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation(string? code = null)
    {
        return code == null ? View("Error") : View();
    }

    #endregion

    #region Confirm Email

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string? userId, string? code)
    {
        if (userId == null || code == null)
            return View("Error");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return View("Error");

        var result = await _userManager.ConfirmEmailAsync(user, code);

        return View(result.Succeeded ? "Confirm Email" : "Error");
    }

    #endregion

    #region Helper Methods

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
            ModelState.AddModelError(error.Code, error.Description);
    }

    #endregion
}