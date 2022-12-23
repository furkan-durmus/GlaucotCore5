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
    public class LoginManager : ILoginService
    {
        IPatientDal _patientDal;

        public LoginManager(IPatientDal patientDal)
        {
            _patientDal = patientDal;
        }
        public bool CheckLoginIsValid(LoginPatient user)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(user.PatientPassword);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                user.PatientPassword = Convert.ToHexString(hashBytes); // .NET 5 +
            }

            return _patientDal.Get(p => p.PatientPhoneNumber == user.PatientPhoneNumber && p.PatientPassword == user.PatientPassword) != null ? true : false;
        }


        public bool CheckKeyIsValid(LoginPatient user)
        {
            string expectedSecretKey = "";
            foreach (char character in user.PatientPhoneNumber)
            {
                expectedSecretKey = expectedSecretKey + System.Convert.ToInt32(character);
            }
            foreach (char character in user.PatientPassword)
            {
                expectedSecretKey = expectedSecretKey + System.Convert.ToInt32(character);
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

        public Patient ResponsePatientId(LoginPatient user)
        {
            return _patientDal.Get(p => p.PatientPhoneNumber == user.PatientPhoneNumber && p.PatientPassword == user.PatientPassword);
        }
    }
}
