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
    public class HangfireSuccessLogManager : IHangfireSuccessLogService
    {
        IHangfireSuccessLogDal _hangfireSuccessLogDal;

        public HangfireSuccessLogManager(IHangfireSuccessLogDal hangfireSuccessLogDal)
        {
            _hangfireSuccessLogDal = hangfireSuccessLogDal;
        }

        public void ClearOldSuccessLogs(int hour)
        {
            _hangfireSuccessLogDal.DeleteLastNHours(hour);
        }

        public void SaveLogToDb(HangfireSuccessLog log)
        {
            _hangfireSuccessLogDal.Add(log);
        }
    }
}
