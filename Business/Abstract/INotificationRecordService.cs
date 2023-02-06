using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface INotificationRecordService
    {
        int AddNotificationRecord(NotificationRecord record);
        void RemoveNotificationRecords();
        List<NotificationRecord> GetAllSendNotifications();
        bool ApproveNotificationRecord(int notificationRecordId);
        bool DelayNotificationRecord(int notificationRecordId);
        bool CycleIncrease(int notificationRecordId);
    }
}
