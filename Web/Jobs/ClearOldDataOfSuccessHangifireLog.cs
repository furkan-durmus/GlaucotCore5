using Business.Abstract;

namespace Web.Jobs
{
    public class ClearOldDataOfSuccessHangifireLog
    {
        IHangfireSuccessLogService _hangfireSuccessLogService;
        public ClearOldDataOfSuccessHangifireLog( IHangfireSuccessLogService hangfireSuccessLogService)
        {
            _hangfireSuccessLogService = hangfireSuccessLogService;
        }

        public void ClearSuccessHangifireLog()
        {
            //_hangfireSuccessLogService.ClearOldSuccessLogs(48);
        }
    }
}
