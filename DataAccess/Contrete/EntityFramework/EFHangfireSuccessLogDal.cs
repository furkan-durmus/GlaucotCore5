using Core.DataAccess.EntityFrameWork;
using Core.Entities;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contrete.EntityFramework
{
    public class EFHangfireSuccessLogDal : EFEntityRepositoryBase<HangfireSuccessLog, GlaucotContext>, IHangfireSuccessLogDal
    {
        public void DeleteLastNHours(int hour)
        {
            //var info = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            //DateTime localServerTime = DateTime.Now;
            //DateTime ExactTime = DateTime.Now; //TimeZoneInfo.ConvertTime(localServerTime, info);
            DateTime LastTime = DateTime.Now;


            using (GlaucotContext context = new GlaucotContext())
            {
                var myEntitySuccess = (from hangfireSuccessLogs in context.HangfireSuccessLogs
                                       where hangfireSuccessLogs.NotificationDate < LastTime
                                       select hangfireSuccessLogs).ToList();

                foreach (var successLog in myEntitySuccess)
                {
                    context.HangfireSuccessLogs.Remove(successLog);
                }

                var myEntityError = (from HangfireErrorLog in context.HangfireErrorLogs
                                     where HangfireErrorLog.LogTime < LastTime
                                     select HangfireErrorLog).ToList();

                foreach (var errorLog in myEntityError)
                {
                    context.HangfireErrorLogs.Remove(errorLog);
                }

                context.SaveChanges();
            }
        }
    }
}
