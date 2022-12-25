using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Identity;
using Web.Models.Doctor;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : BaseController
    {
        private readonly UserManager<DoctorUser> _userManager;
        public DoctorController(UserManager<DoctorUser> userManager = null)
        {
            _userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(DoctorPasswordChangeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _userManager.ChangePasswordAsync(await _userManager.FindByIdAsync(User.Claims.GetClaim(ClaimTypes.NameIdentifier).Value), model.OldPassword, model.NewPassword);
                if (result != IdentityResult.Success)
                {
                    ModelState.AddModelError("OldPassword", "Please check the information and try again or enter a password in the password format");
                    return View(model);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("OldPassword", e.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}

