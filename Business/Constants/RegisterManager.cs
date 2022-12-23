using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class RegisterManager : IRegisterService
    {
        IPatientDal _patientDal;


        public RegisterManager(IPatientDal patientDal, IOTPDal otpDal)
        {
            _patientDal = patientDal;
        }

        public bool CheckPhoneIsExist(string userPhone)
        {
            return _patientDal.Get(p => p.PatientPhoneNumber == userPhone) != null ? true : false;
        }


        public bool CheckKeyIsValid(RegisterPatient user)
        {
            string expectedSecretKey = "";
            foreach (char character in user.PatientPhoneNumber)
            {
                expectedSecretKey = expectedSecretKey + System.Convert.ToInt32(character);
            }
            if (user.PatientPassword != null)
            {
                foreach (char character in user.PatientPassword)
                {
                    expectedSecretKey = expectedSecretKey + System.Convert.ToInt32(character);
                }
            }
            string mykeyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(expectedSecretKey));

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(mykeyBase64);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                expectedSecretKey = Convert.ToHexString(hashBytes); // .NET 5 +
            }
            return expectedSecretKey == user.SecretKey ? true : false;
        }

    }
}
