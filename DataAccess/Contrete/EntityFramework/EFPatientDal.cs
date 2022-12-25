using Core.DataAccess.EntityFrameWork;
using DataAccess.Abstract;
using DataAccess.DTOS;
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

        public async Task<PagingModelResponse<Patient>> PatientDalDataTable(string textSearch, int orderCol, bool orderDescending,
    int start, int length)
        {
            using (GlaucotContext context = new GlaucotContext())
            {
                var query = context.Patients.AsQueryable();

                if (textSearch != null)
                {
                    int.TryParse(textSearch, out int age);
                    query = query.Where(q => q.PatientName.Contains(textSearch) || q.PatientAge == age || q.PatientLastName.Contains(textSearch) || q.PatientPhoneNumber.Contains(textSearch));
                }

                if (orderCol == 0)
                    query = orderDescending ? query.OrderByDescending(v => v.PatientName) : query.OrderBy(v => v.PatientName);
                else if (orderCol == 1)
                    query = orderDescending
                        ? query.OrderByDescending(v => v.PatientLastName)
                        : query.OrderBy(v => v.PatientLastName);
                else if (orderCol == 2)
                    query = orderDescending ? query.OrderByDescending(v => v.PatientAge) : query.OrderBy(v => v.PatientAge);
                else if (orderCol == 3)
                    query = orderDescending
                        ? query.OrderByDescending(v => v.PatientGender)
                        : query.OrderBy(v => v.PatientGender);
                else if (orderCol == 4)
                    query = orderDescending
                        ? query.OrderByDescending(v => v.PatientPhoneNumber)
                        : query.OrderBy(v => v.PatientPhoneNumber);

                var count = await query.CountAsync();

                var data = await query.Skip(start).Take(length).ToListAsync();

                return new PagingModelResponse<Patient>
                {
                    PageSize = data.Count,
                    ItemCount = count,
                    DidError = false,
                    Model = data,
                    PageNumber = start / length
                };
            }
        }

    }
}
