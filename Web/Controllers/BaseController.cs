using Microsoft.AspNetCore.Mvc;
using System;
using Web.Extensions;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected Guid DoctorId => Guid.Parse(User.Claims.GetClaim("DoctorId")?.Value ?? throw new NullReferenceException("DoctorId null"));
    }
}
