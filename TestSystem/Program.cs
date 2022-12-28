using Business.Constants;
using DataAccess.Contrete.EntityFramework;
using System;
using Web.Jobs;

namespace TestSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var myTest = new SendMedicineNotification(new MedicineRecordManager(new EFMedicineRecordDal()), new HangfireErrorLogManager(new EFHangfireErrorLogDal()), new HangfireSuccessLogManager(new EFHangfireSuccessLogDal()));
            //myTest.SendNotificationWithOneSignal();


            //var myTest = new ClearOldDataOfSuccessHangifireLog(new HangfireSuccessLogManager(new EFHangfireSuccessLogDal()));
            //myTest.ClearSuccessHangifireLog();
        }
    }
}
