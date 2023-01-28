using Business.Abstract;
using Entities.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using RestSharp;
using System.Collections;
using System.Linq;

namespace Web.Jobs
{
    public class SendMedicineNotificationWithOneSignal
    {
        IMedicineRecordService _medicineRecordService;
        IHangfireErrorLogService _hangfireErrorLogService;
        IHangfireSuccessLogService _hangfireSuccessLogService;
        private readonly INotificationRecordService _notificationRecordService;

        public SendMedicineNotificationWithOneSignal(IMedicineRecordService medicineRecordService, IHangfireErrorLogService hangfireErrorLogService, INotificationRecordService notificationRecordService, IHangfireSuccessLogService hangfireSuccessLogService)
        {
            _medicineRecordService = medicineRecordService;
            _hangfireErrorLogService = hangfireErrorLogService;
            _notificationRecordService = notificationRecordService;
            _hangfireSuccessLogService = hangfireSuccessLogService;
        }
        public void SendNotificationWithOneSignal()
        {

            //DateTime closestHalfOrFullTime = DateTime.Now.AddHours(10);
            //DateTime closestHalfOrFullTime = DateTime.Parse("16.12.2022 17:01:05");

            var info = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset closestHalfOrFullTime = TimeZoneInfo.ConvertTime(localServerTime, info);

            int minuteOfTime = closestHalfOrFullTime.Minute;
            if (minuteOfTime < 15)
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes(-minuteOfTime).AddSeconds(-closestHalfOrFullTime.Second);
            else if (minuteOfTime < 30)
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes((30 - minuteOfTime)).AddSeconds(-closestHalfOrFullTime.Second);
            else if (minuteOfTime < 45)
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes(-(minuteOfTime - 30)).AddSeconds(-closestHalfOrFullTime.Second);
            else
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes((60 - minuteOfTime)).AddSeconds(-closestHalfOrFullTime.Second);

            string myExactTime = closestHalfOrFullTime.ToString("HH:mm");

            List<NotificationMedicine> patientsDataForNotification = _medicineRecordService.GetDataForMedicineNotification(myExactTime);

            var attemp = 0;

            while (attemp < 3)
            {
                try
                {
                    foreach (var notificationData in patientsDataForNotification)
                    {
                        List<string> users = new List<string>();
                        users.Add(notificationData.PatientNotificationToken);

                        List<Dictionary<string, string>> buttons = new List<Dictionary<string, string>>();
                        buttons.Add(new Dictionary<string, string>() { { "id", "id_confirm" }, { "text", "Onayla" }, });
                        buttons.Add(new Dictionary<string, string>() { { "id", "id_delay" }, { "text", "Ertele" }, });

                        Dictionary<string, string> contents = new Dictionary<string, string>();
                        contents.Add("en", $"It's {notificationData.CurrentTime}, time to get {notificationData.MedicineName}");
                        contents.Add("tr", $"Saat {notificationData.CurrentTime}, {notificationData.MedicineName} kullanmayı unutma.");

                        Dictionary<string, string> headings = new Dictionary<string, string>();
                        headings.Add("en", $"Glaucot Medicine Reminder");
                        headings.Add("tr", $"Glaucot İlaç Hatırlatıcı");

                        int notificationRecordId = _notificationRecordService.AddNotificationRecord(new NotificationRecord
                        {
                            Cycle = 0,
                            Status = Core.NotificationRecordStatus.None,
                            Content = string.Join("-", contents),   // [en, It's qwe, time to get asd]-[tr, Saat qwe, asd kullanmayı unutma.]
                            Title = string.Join("-", headings),   // [en, Glaucot Medicine Reminder]-[tr, Glaucot İlaç Hatırlatıcı]
                            CreateDate = DateTime.Now,
                            PatientId = Guid.Parse(users.First()),
                            Token = notificationData.PatientNotificationToken
                        });

                        Dictionary<string, string> notificationRecordData = new Dictionary<string, string>();
                        headings.Add("notificationRecordId", $"{notificationRecordId}");
                        headings.Add("patientId", $"{users.First()}");

                        OneSignalNotification oneSignalNotification = new OneSignalNotification();
                        oneSignalNotification.app_id = "f196579d-dc71-404a-9d92-c5311836d8c1";
                        oneSignalNotification.name = "INTERNAL_CAMPAIGN_NAME";
                        oneSignalNotification.include_player_ids = users;
                        oneSignalNotification.buttons = buttons;
                        oneSignalNotification.contents = contents;
                        oneSignalNotification.priority = 10;
                        oneSignalNotification.ttl = 0;
                        oneSignalNotification.content_available = true;
                        oneSignalNotification.android_channel_id = "9113e0b5-9b25-46c3-8abe-f56ba4827261";
                        oneSignalNotification.headings = headings;
                        oneSignalNotification.data = notificationRecordData;

                        string serilizedRequestData = JsonConvert.SerializeObject(oneSignalNotification);

                        var client = new RestClient("https://onesignal.com/api/v1/notifications");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", "Basic OGUzNmMyYTEtYWNiNi00MWQ3LWJiMTUtYjhjZTQyMzI1N2E3");
                        request.AddHeader("Content-Type", "application/json");

                        request.AddParameter("application/json", serilizedRequestData, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);

                    }
                    break;
                }
                catch (Exception e)
                {
                    attemp = +1;
                    HangfireErrorLog hangfireErrorLog = new();
                    hangfireErrorLog.LogSource = e?.Source;
                    hangfireErrorLog.LogMessage = e?.Message;
                    hangfireErrorLog.LogStackTrace = e?.StackTrace;
                    hangfireErrorLog.LogInnerException = e.InnerException?.Message;
                    hangfireErrorLog.LogTime = DateTime.Now;
                    _hangfireErrorLogService.SaveLogToDb(hangfireErrorLog);
                }
            }


        }
    }
}
