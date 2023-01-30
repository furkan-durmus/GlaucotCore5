using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class NotificationRecordRequest
    {
        public Guid PatientId { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string SecretKey { get; set; }
        public int NotificationRecordId { get; set; }
    }
}
