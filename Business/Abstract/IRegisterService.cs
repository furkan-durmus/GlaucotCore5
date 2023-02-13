using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IRegisterService
    {
        bool CheckKeyIsValid(RegisterPatient user);
        bool CheckPhoneIsExist(string userPhone);
        bool CheckUserIsActive(string userPhone);
        
 
    }
}
