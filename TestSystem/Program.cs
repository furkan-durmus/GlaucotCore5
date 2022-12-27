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
            var myTest = new SendMedicineNotification(new MedicineRecordManager(new EFMedicineRecordDal()),new HangfireLogManager(new EFHangfireLogDal()));
            myTest.SendNotificationWithOneSignal();

        }
    }
}
