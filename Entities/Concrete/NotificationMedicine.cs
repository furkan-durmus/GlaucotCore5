using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class NotificationMedicine
    {
        public string MedicineName { get; set; }
        public string PatientNotificationToken { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string CurrentTime { get; set; }
    }
}
