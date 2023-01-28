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
    }
}
