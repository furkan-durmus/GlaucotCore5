using Core.DataAccess.EntityFrameWork;
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
    public class EFNotificationRecodDal : EFEntityRepositoryBase<NotificationRecord, GlaucotContext>, INotificationRecordDal
    {
        public int AddWithReturnNotificationRecordId(NotificationRecord record)
        {
            using (GlaucotContext context = new GlaucotContext())
            {
                var myEntity = context.Entry(record);
                myEntity.State = EntityState.Added;
                context.SaveChanges();
                return record.NotificationRecordId;
            }
        }

        public List<NotificationRecord> GetDoneNotifications()
        {
            using (GlaucotContext context = new GlaucotContext())
            {
                return context.Set<NotificationRecord>().Where(q => q.Status == Core.NotificationRecordStatus.Done).ToList();
            }
        }
    }
}
