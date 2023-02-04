using Business.Abstract;
using Entities.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Web.Jobs
{
    public class SendMedicineNotificationWithFireBase
    {
        IMedicineRecordService _medicineRecordService;
        IHangfireErrorLogService _hangfireErrorLogService;
        IHangfireSuccessLogService _hangfireSuccessLogService;
        public SendMedicineNotificationWithFireBase(IMedicineRecordService medicineRecordService, IHangfireErrorLogService hangfireErrorLogService, IHangfireSuccessLogService hangfireSuccessLogService)
        {
            _medicineRecordService = medicineRecordService;
            _hangfireErrorLogService = hangfireErrorLogService;
            _hangfireSuccessLogService = hangfireSuccessLogService;
        }
        public void SendNotificationWithOneFireBase()
        {

            ////DateTime closestHalfOrFullTime = DateTime.Now.AddHours(10);
            //DateTime closestHalfOrFullTime = DateTime.Parse("16.12.2022 11:01:05 AM");

            ////var info = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            ////DateTimeOffset localServerTime = DateTimeOffset.Now;
            ////DateTimeOffset closestHalfOrFullTime = TimeZoneInfo.ConvertTime(localServerTime, info);

            //int minuteOfTime = closestHalfOrFullTime.Minute;
            //if (minuteOfTime < 15)
            //    closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes(-minuteOfTime).AddSeconds(-closestHalfOrFullTime.Second);
            //else if (minuteOfTime < 30)
            //    closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes((30 - minuteOfTime)).AddSeconds(-closestHalfOrFullTime.Second);
            //else if (minuteOfTime < 45)
            //    closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes(-(minuteOfTime - 30)).AddSeconds(-closestHalfOrFullTime.Second);
            //else
            //    closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes((60 - minuteOfTime)).AddSeconds(-closestHalfOrFullTime.Second);

            //string myExactTime = closestHalfOrFullTime.ToString("HH:mm");

            //List<NotificationMedicine> patientsDataForNotification = _medicineRecordService.GetDataForMedicineNotification(myExactTime);

            //var attemp = 0;

            //while (attemp < 3)
            //{
            //    try
            //    {
            //        foreach (var notificationData in patientsDataForNotification)
            //        {
            //            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            //            tRequest.Method = "post";
            //            //serverKey - Key from Firebase cloud messaging server  
            //            tRequest.Headers.Add(string.Format("Authorization: key=AAAAft2AqF8:APA91bFge8bRCb9Ghq0PYUqyk-tKjk7jJgq9QlYzdJAeAWiu9crwudE0_CCpaOP31L2fx87Fj_vlgoHAus2U8AmRIAZreFIVUYkAd1P1ajk95uIu4dba93IUt8JSQPLv-RF4C0c_IvSs"));
            //            tRequest.ContentType = "application/json";
            //            var payload = new
            //            {
            //                to = notificationData.PatientNotificationToken,
            //                priority = "high",
            //                content_available = true,
            //                notification = new
            //                {
            //                    title = "Glaucot Medicine Reminder",
            //                    body = $"It's {notificationData.CurrentTime}, time to get {notificationData.MedicineName}",
            //                    badge = 1
            //                },
            //                data = new
            //                {
            //                    title = "Glaucot Medicine Reminder",
            //                    body = $"It's {notificationData.CurrentTime}, time to get {notificationData.MedicineName}",
            //                    badge = 1
            //                },

            //            };

            //            string postbody = JsonConvert.SerializeObject(payload).ToString();
            //            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            //            tRequest.ContentLength = byteArray.Length;
            //            using (Stream dataStream = tRequest.GetRequestStream())
            //            {
            //                dataStream.Write(byteArray, 0, byteArray.Length);
            //                using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
            //                {
            //                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
            //                    {
            //                        if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
            //                            {
            //                                String sResponseFromServer = tReader.ReadToEnd();
            //                                //result.Response = sResponseFromServer;

            //                                HangfireSuccessLog hangfireSuccessLog = new HangfireSuccessLog();
            //                                hangfireSuccessLog.NotificationDate = tResponse.LastModified.AddHours(10);
            //                                hangfireSuccessLog.StatusDescription = tResponse.StatusDescription;
            //                                hangfireSuccessLog.StatusCode = tResponse.StatusCode.ToString();
            //                                hangfireSuccessLog.SResponseFromServer = sResponseFromServer;
            //                                hangfireSuccessLog.PatientPhone = notificationData.PatientPhoneNumber;

            //                                _hangfireSuccessLogService.SaveLogToDb(hangfireSuccessLog);

            //                            }



            //                    }



            //                }
            //            }

            //        }
            //        break;
            //    }
            //    catch (Exception e)
            //    {
            //        attemp = +1;
            //        HangfireErrorLog hangfireErrorLog = new();
            //        hangfireErrorLog.LogSource = e?.Source;
            //        hangfireErrorLog.LogMessage = e?.Message;
            //        hangfireErrorLog.LogStackTrace = e?.StackTrace;
            //        hangfireErrorLog.LogInnerException = e.InnerException?.Message;
            //        hangfireErrorLog.LogTime = DateTime.Now;
            //        _hangfireErrorLogService.SaveLogToDb(hangfireErrorLog);
            //    }
            //}


        }
    }
}
