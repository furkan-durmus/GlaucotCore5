using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : BaseController
    {
        private readonly UserManager<DoctorUser> _userManager;
        private readonly IMedicineService _medicineService;
        private readonly IPatientService _patientService;
        private readonly IMedicineRecordService _medicineRecordService;
        private readonly IGlassRecordService _glassRecordService;
        private readonly IEyePressureRecordService _eyePressureRecordService;
        public DoctorController(UserManager<DoctorUser> userManager = null, IMedicineService medicineService = null, IPatientService patientService = null, IMedicineRecordService medicineRecordService = null, IGlassRecordService glassRecordService = null, IEyePressureRecordService eyePressureRecordService = null)
        {
            _userManager = userManager;
            _medicineService = medicineService;
            _patientService = patientService;
            _medicineRecordService = medicineRecordService;
            _glassRecordService = glassRecordService;
            _eyePressureRecordService = eyePressureRecordService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            int patients = _patientService.GetAll().Count;
            int medicines = _medicineService.GetAll().Count;

            return View(new MedsPatsCountViewModel { Medicines = medicines, Patients = patients });
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
                string imageName = "test.png";
                string savePath = "MedicineImages/test.png";
                int frequency = model.medicineDefaultFrequency == 0 ? 1 : model.medicineDefaultFrequency;
                if (model.medicineImage != null)
                {
                    imageName = model.MedicineName.Replace(" ", "-") + Path.GetExtension(model.medicineImage.FileName);

                    //Get url To Save
                    savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MedicineImages", imageName);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        model.medicineImage.CopyTo(stream);
                    }
                }

                _medicineService.Add(new Entities.Concrete.Medicine { MedicineName = model.MedicineName, MedicineDefaultFrequency = frequency, MedicineImagePath = "/MedicineImages/" + imageName });
                TempData["Result"] = "success";
            }
            catch (Exception)
            {
                TempData["Result"] = "fail";
            }

            return View();
        }

        public IActionResult EditMedicine([FromQuery] int id)
        {
            var medicine = _medicineService.Get(id);
            if (medicine == null)
                return RedirectToAction("AddMedicine");

            return View(new EditMedicineViewModel
            {
                MedicineDefaultFrequency = medicine.MedicineDefaultFrequency,
                MedicineId = medicine.MedicineId,
                ImagePath = medicine.MedicineImagePath,
                MedicineName = medicine.MedicineName
            });
        }

        [HttpPost]
        public IActionResult EditMedicine(EditMedicineViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var medicine = _medicineService.Get(model.MedicineId);
                if (medicine == null)
                    return RedirectToAction("AddMedicine");

                string imageName = "test.png";
                string savePath = "MedicineImages/test.png";
                if (model.MedicineImage != null)
                {
                    imageName = model.MedicineName.Replace(" ", "-") + Path.GetExtension(model.MedicineImage.FileName);

                    //Get url To Save
                    savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MedicineImages", imageName);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        model.MedicineImage.CopyTo(stream);
                    }
                }


                _medicineService.Update(new Entities.Concrete.Medicine
                {
                    MedicineId = model.MedicineId,
                    MedicineName = model.MedicineName,
                    MedicineDefaultFrequency = model.MedicineDefaultFrequency,
                    MedicineImagePath = model.MedicineImage == null ? medicine.MedicineImagePath : imageName
                });
                return RedirectToAction("AddMedicine");
            }
            catch (Exception)
            {
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult DeleteMedicine(int medicineId)
        {
            try
            {
                _medicineService.DeleteById(medicineId);
                return Json(new { ok = true });
            }
            catch (Exception)
            {
                return Json(new { ok = false });
            }
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
                new Business.DTOS.PatientDataTablesParam
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

        public IActionResult PatientDetail([FromQuery] Guid patient)
        {
            if (patient == Guid.Empty)
                return Redirect("/");

            var patientInf = _patientService.Get(patient);
            TempData["PName"] = patientInf.PatientName;
            TempData["PLastName"] = patientInf.PatientLastName;

            var glassRecord = _glassRecordService.GetAll(patient);
            var medicineList = _medicineRecordService.GetAll(patient);
            var eyePressureList = _eyePressureRecordService.GetAllPatientEyePressure(patient);

            var medicine = medicineList.Select(q => new MedicineInformation
            {
                MedicineName = _medicineService.Get(q.MedicineId).MedicineName,
                MedicineFrequency = q.MedicineFrequency,
                MedicineUsegeTimeList = q.MedicineUsegeTimeList,
                MedicineSideEffect = q.MedicineSideEffect
            }).ToList();

            var glass = glassRecord.Where(q => !q.IsActive).Select(q => new GlassRecordInformation
            {
                StartDate = q.StartDate.AddHours(10).ToString("dd-MM-yyyy HH:mm:ss"),
                EndDate = q.EndDate.AddHours(10).ToString("dd-MM-yyyy HH:mm:ss"),
                DiffDate = q.EndDate - q.StartDate
            }).ToList();

            var eyePressure = eyePressureList.Select(q => new EyePressure
            {
                EyePressureDate = q.EyePressureDate.ToString("dd-MM-yyyy HH:mm:ss"),
                LeftEyePressure = q.LeftEyePressure,
                RightEyePressure = q.RightEyePressure
            }).ToList();

            return View(new PatientDetailViewModel { Medicine = medicine, GlassRecord = glass, EyePressure = eyePressure });
        }
    }
}

