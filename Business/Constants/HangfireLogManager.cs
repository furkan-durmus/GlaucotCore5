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
    public class HangfireLogManager : IHangfireLogService
    {
        IHangfireLogDal _hangfireLogDal;

        public HangfireLogManager(IHangfireLogDal hangfireLogDal)
        {
            _hangfireLogDal = hangfireLogDal;
        }

        public void SaveLogToDb(HangfireLog log)
        {
            _hangfireLogDal.Add(log);
        }
    }
}
