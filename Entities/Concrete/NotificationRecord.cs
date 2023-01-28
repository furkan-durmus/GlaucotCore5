using Core;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class NotificationRecord : IEntity
    {
        public int NotificationRecordId { get; set; }
        public Guid PatientId { get; set; }
        public string Token { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Cycle { get; set; }
        public NotificationRecordStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
