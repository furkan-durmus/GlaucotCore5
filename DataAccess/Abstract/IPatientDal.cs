using Core.DataAccess;
using DataAccess.DTOS;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IPatientDal : IEntitiyRepository<Patient>
    {
        void UpdatePatientNotificationToken(GeneralMobilePatientRequest patient);
        Task<PagingModelResponse<Patient>> PatientDalDataTable(string textSearch, int orderCol, bool orderDescending,
    int start, int length);
    }
}
