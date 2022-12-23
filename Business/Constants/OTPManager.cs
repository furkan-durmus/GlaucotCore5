using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Contrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class OTPManager : IOTPService
    {
        IOTPDal _OTPDal;

        public OTPManager(IOTPDal oTPDal)
        {
            _OTPDal = oTPDal;
        }

        public bool CheckOTP(RegisterPatient user)
        {
            return _OTPDal.Get(o => o.PatientPhoneNumber == user.PatientPhoneNumber && o.OTPCode == user.OTPCode) != null ? true : false;
        }

        public void Create(OTP userOTP)
        {
            _OTPDal.Add(userOTP);
        }


        public bool CheckAcceptableSmsLimit(RegisterPatient user)
        {
            bool dataExist = _OTPDal.Get(o => o.PatientPhoneNumber == user.PatientPhoneNumber && o.CreateDate >= DateTime.Now.AddMinutes(-15)) != null ? true : false;
            if (dataExist)
            {
                var data = _OTPDal.GetAll(o => o.PatientPhoneNumber == user.PatientPhoneNumber && o.CreateDate >= DateTime.Now.AddMinutes(-15));
                List<OTP> userOTPHistory = data != null ? data.ToList() : new();

                return userOTPHistory.Count > 2 ? false : true;
            }
            return true;

        }
    }
}
