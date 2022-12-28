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
            var info = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset ExactTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            DateTime LastTime = ExactTime.AddHours(-hour+3).UtcDateTime;
          

            using (GlaucotContext context = new GlaucotContext())
            {
                var myEntity = (from hangfireSuccessLogs in context.HangfireSuccessLogs
                               where hangfireSuccessLogs.NotificationDate < LastTime
                                select hangfireSuccessLogs).ToList();

                foreach (var successLog in myEntity)
                {
                    context.HangfireSuccessLogs.Remove(successLog);
                }
                
                context.SaveChanges();
            }
        }
    }
}
