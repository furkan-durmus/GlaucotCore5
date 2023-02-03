using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class HangfireSuccessLog : IEntity
    {
        public int Id { get; set; }
        public string PatientPhone { get; set; }
        public DateTime? NotificationDate { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public string? SResponseFromServer { get; set; }
       
    }
}
