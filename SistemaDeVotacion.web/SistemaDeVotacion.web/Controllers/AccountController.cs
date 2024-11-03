using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaDeVotacion.BlockchainServicio;
using SistemaDeVotacion.Domain;
using SistemaDeVotacion.Domain.Dto;
using SistemaDeVotacion.web.Models;

namespace SistemaDeVotacion.web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserService _userService;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, UserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

       var dto = new RegisterUserDto { Email = model.Email, Password = model.Password };

        var result = await _userService.RegisterUserAsync(dto);

        if (result.responseIdentity.Succeeded)
        {
            
            await _signInManager.SignInAsync(result.userIdentity, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.responseIdentity.Errors)
            ModelState.AddModelError("Password", error.Description);

        return View(model);
    }

    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}