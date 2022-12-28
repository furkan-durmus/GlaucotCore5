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
    public class HangfireErrorLogManager : IHangfireErrorLogService
    {
        IHangfireErrorLogDal _hangfireErrorLogDal;

        public HangfireErrorLogManager(IHangfireErrorLogDal hangfireErrorLogDal)
        {
            _hangfireErrorLogDal = hangfireErrorLogDal;
        }

        public void SaveLogToDb(HangfireErrorLog log)
        {
            _hangfireErrorLogDal.Add(log);
        }
    }
}
