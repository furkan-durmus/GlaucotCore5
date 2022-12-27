using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class HangfireLog : IEntity
    {
        public int Id { get; set; }
        public string LogMessage { get; set; }
        public string LogInnerException { get; set; }
        public string LogSource { get; set; }
        public DateTime LogTime { get; set; }
    }
}
