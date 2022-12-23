using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOTPService
    {
        void Create(OTP userOTP);
        bool CheckOTP(RegisterPatient user);
        bool CheckAcceptableSmsLimit(RegisterPatient user);
    }
}
