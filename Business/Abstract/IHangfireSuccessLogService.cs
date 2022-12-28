using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IHangfireSuccessLogService
    {
        void SaveLogToDb(HangfireSuccessLog log);
        void ClearOldSuccessLogs(int hour);
    }
}
