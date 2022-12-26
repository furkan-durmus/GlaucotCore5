using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Identity;
using Web.Models.DataTables;
using Web.Models.Doctor;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : BaseController
    {
        private readonly UserManager<DoctorUser> _userManager;
        private readonly IMedicineService _medicineService;
        private readonly IPatientService _patientService;
        public DoctorController(UserManager<DoctorUser> userManager = null, IMedicineService medicineService = null, IPatientService patientService = null)
        {
            _userManager = userManager;
            _medicineService = medicineService;
            _patientService = patientService;
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

        public IActionResult AddMedicine()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMedicine(DoctorAddMedicineViewModel model)
        {
            try
            {
                var medicines = _medicineService.GetAllMedicineName();

                if (medicines.Contains(model.MedicineName.ToLower()))
                {
                    ModelState.AddModelError("MedicineName", "This medicine is already registered");
                    return View(model);
                }

                _medicineService.Add(new Entities.Concrete.Medicine { MedicineName = model.MedicineName });
                TempData["Result"] = "success";
            }
            catch (Exception)
            {
                TempData["Result"] = "fail";
            }

            return View();
        }

        [HttpPost]
        public IActionResult EditMedicine(EditMedicineViewModel model)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MedicineHandler(MedicineDataTablesViewModel model)
        {
            var response = new DataTablesReturnModel { draw = model.draw };

            var result = await _medicineService.MedicineDataTable(
                new Business.DTOS.MedicineDataTablesParam
                {
                    TextSearch = model.TextSearch,
                    OrderCol = model.order.FirstOrDefault()?.column ?? 0,
                    OrderDesc = model.order.FirstOrDefault()?.dir?.Equals("desc") ?? false,
                    Size = model.length,
                    Start = model.start
                });

            response.data = result.Model;
            response.error = result.ErrorMessage;
            response.recordsFiltered = result.ItemCount;
            response.recordsTotal = result.ItemCount;

            return Json(response);
        }

        public IActionResult Patients()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PatientsHandler(PatientDataTablesViewModel model)
        {
            var response = new DataTablesReturnModel { draw = model.draw };

            var result = await _patientService.PatientsDataTable(
                new Business.DTOS.PatientDataTablesParam { 
                    TextSearch = model.TextSearch,
                    OrderCol = model.order.FirstOrDefault()?.column ?? 0,
                    OrderDesc = model.order.FirstOrDefault()?.dir?.Equals("desc") ?? false,
                    Size = model.length,
                    Start = model.start
                });

            response.data = result.Model;
            response.error = result.ErrorMessage;
            response.recordsFiltered = result.ItemCount;
            response.recordsTotal = result.ItemCount;

            return Json(response);
        }
    }
}

