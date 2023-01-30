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
    public class NotificationRecordManager : INotificationRecordService
    {
        private readonly INotificationRecordDal _notificationRecordDal;

        public NotificationRecordManager(INotificationRecordDal notificationRecordDal)
        {
            _notificationRecordDal = notificationRecordDal;
        }

        public int AddNotificationRecord(NotificationRecord record)
        {
            return _notificationRecordDal.AddWithReturnNotificationRecordId(record);
        }

        public bool ApproveNotificationRecord(int notificationRecordId)
        {
            var notificationRecord = _notificationRecordDal.Get(q => q.NotificationRecordId == notificationRecordId);
            if(notificationRecord == null)
                return false;

            notificationRecord.Status = Core.NotificationRecordStatus.Done;
            _notificationRecordDal.Update(notificationRecord);
            return true;
        }

        public bool DelayNotificationRecord(int notificationRecordId)
        {
            var notificationRecord = _notificationRecordDal.Get(q => q.NotificationRecordId == notificationRecordId);
            if (notificationRecord == null)
                return false;

            notificationRecord.Cycle += 1;
            notificationRecord.CreateDate = DateTime.Now;
            _notificationRecordDal.Update(notificationRecord);
            return true;
        }

        public List<NotificationRecord> GetAllSendNotifications()
        {
            // status 0 ve createDate'i şu an ki zamandan 6dk dan daha büyükse cycle'ı 5'ten küçükse (bu 3 durum sağlanıyorsa) send notification'daki işlemler tekranlanacak
            return _notificationRecordDal.GetAll(q => q.Status == Core.NotificationRecordStatus.None && q.Cycle < 5 && q.CreateDate < DateTime.Now.AddMinutes(-6));
        }

        public void RemoveNotificationRecords()
        {
            var doneNotifications = _notificationRecordDal.GetDoneNotifications();
            foreach (var doneNotification in doneNotifications)
            {
                _notificationRecordDal.Delete(doneNotification);
            }
        }
    }
}
