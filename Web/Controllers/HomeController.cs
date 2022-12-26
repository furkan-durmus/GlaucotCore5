using Business.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Identity;
using Web.Models;
using Web.Models.Home;

namespace Web.Controllers
{
    public class HomeController : Controller // test
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDoctorService _doctorService;
        private readonly UserManager<DoctorUser> _userManager;
        private readonly SignInManager<DoctorUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IDoctorService doctorService, UserManager<DoctorUser> userManager, SignInManager<DoctorUser> signInManager)
        {
            _logger = logger;
            _doctorService = doctorService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index","Doctor");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            //if (!ModelState.IsValid)
            //    return View(model);

            //var loginResult = _doctorService.Login(model.UserName,model.UserPassword);
            //if (loginResult != null)
            //{
            //    var login = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.Name, loginResult.DoctorName),
            //        new Claim(ClaimTypes.Surname, loginResult.DoctorLastName),
            //        new Claim(ClaimTypes.Role, "Doctor"),
            //        new Claim(ClaimTypes.Email, loginResult.DoctorEmail),
            //        new Claim("Id",loginResult.DoctorId.ToString())
            //    };
            //    var userIdentity = new ClaimsIdentity(login, CookieAuthenticationDefaults.AuthenticationScheme);
            //    var principal = new ClaimsPrincipal(userIdentity);
            //    var authProperties = new AuthenticationProperties();
            //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            //    return RedirectToAction("Index","Doctor");
            //}
            //ModelState.AddModelError("UserName", "Please check email or password");



            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Email", "Please check your email or password");
                return View(model);
            }
            return RedirectToAction("Index", "Doctor");
            //return View(model);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index","Doctor");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAccountViewModel model)
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            if (!ModelState.IsValid)
                return View(model);

            var sonuc = await _userManager.CreateAsync(new DoctorUser { DoctorEmail = model.Email }, model.Password);
            if (!sonuc.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var identityError in sonuc.Errors)
                {
                    sb.AppendLine(identityError.Description);
                }
                ModelState.AddModelError("Email", sb.ToString());
                return View(model);
            }

            //var user = await _userManager.FindByNameAsync(model.Email);
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //await _mediator.Send(new UpdateKullaniciTokenCommand { Token = token, KullaniciId = user.Id });

            return Redirect("/");
        }
    }
}