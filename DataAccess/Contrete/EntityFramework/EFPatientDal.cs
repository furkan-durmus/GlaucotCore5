using Core.DataAccess.EntityFrameWork;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contrete.EntityFramework
{
    public class EFPatientDal : EFEntityRepositoryBase<Patient, GlaucotContext>, IPatientDal
    {
        public void UpdatePatientNotificationToken(GeneralMobilePatientRequest patient)
        {
            using (GlaucotContext context = new GlaucotContext())
            {
                Patient myPatient = (from p in context.Patients
                              where p.PatientId == patient.PatientId
                              select p).First();
                myPatient.PatientNotificationToken = patient.PatientNotificationToken;
                context.SaveChanges();
            }
        }
    }
}
