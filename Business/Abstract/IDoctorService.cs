using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDoctorService
    {
        List<Doctor> GetAll();
        Doctor Get(Guid doctorId);
        void Add(Doctor doctor);
        void Update(Doctor doctor);
        void Delete(Doctor doctor);
        void Register(string email, string password, string securityStamp);
        Doctor GetByEmail(string email);
        void DoctorApprove(Guid doctorId, bool confirmed);
    }
}
