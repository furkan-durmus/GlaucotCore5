using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface INotificationRecordDal : IEntitiyRepository<NotificationRecord>
    {
        int AddWithReturnNotificationRecordId(NotificationRecord record);
    }
}
