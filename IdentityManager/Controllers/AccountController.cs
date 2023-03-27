using IdentityManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers;

#nullable enable

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region Register

    [HttpGet]
    public Task<IActionResult> Register(CancellationToken cancellationToken, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        var model = new RegisterViewModel();

        return Task.FromResult<IActionResult>(View(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken,
        string? returnUrl = null)
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
        {
            return View("Lockout");
        }

        ModelState.AddModelError(string.Empty, "Invalid login");

        return View(model);
    }

    [HttpGet]
    public Task<IActionResult> Login(CancellationToken cancellationToken, string? returnUrl = null)
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForGetPassword(ForgotPasswordModel model, CancellationToken cancellationToken)
    {
        return View();
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