using Core.DataAccess;
using Entities.Concrete;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface INotificationRecordDal : IEntitiyRepository<NotificationRecord>
    {
        int AddWithReturnNotificationRecordId(NotificationRecord record);
        List<NotificationRecord> GetDoneNotifications();
    }
}
