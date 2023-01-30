using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public enum NotificationRecordStatus {
        None,
        Done
    }

    public enum NotificationRecordType
    {
        Approve = 0,
        Delay = 1
    }
}
