using Business.Abstract;
using Entities.Concrete;
using Hangfire;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System;
using System.Linq;

namespace Web.Jobs
{
    public class CheckNotificationRecords
    {
        private readonly IMedicineRecordService _medicineRecordService;
        private readonly INotificationRecordService _notificationRecordService;
        private readonly IHangfireErrorLogService _hangfireErrorLogService;
        private readonly IHangfireSuccessLogService _hangfireSuccessLogService;
        public CheckNotificationRecords(INotificationRecordService notificationRecordService, IHangfireErrorLogService hangfireErrorLogService, IHangfireSuccessLogService hangfireSuccessLogService, IMedicineRecordService medicineRecordService)
        {
            _notificationRecordService = notificationRecordService;
            _hangfireErrorLogService = hangfireErrorLogService;
            _hangfireSuccessLogService = hangfireSuccessLogService;
            _medicineRecordService = medicineRecordService;
        }

        public void CheckNotification()
        {
            //_notificationRecordService.RemoveNotificationRecords();  // Kayıt silme işi iptal denildi

            var notificationRecordList = _notificationRecordService.GetAllSendNotifications();

            //DateTime closestHalfOrFullTime = DateTime.Now.AddHours(10);
            //DateTime closestHalfOrFullTime = DateTime.Parse("16.12.2022 11:01:05");
            //var info = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            //DateTime localServerTime = DateTime.Now;
            DateTime closestHalfOrFullTime = DateTime.Now; //TimeZoneInfo.ConvertTime(localServerTime, info);
            int minuteOfTime = closestHalfOrFullTime.Minute;
            if (minuteOfTime < 15)
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes(-minuteOfTime).AddSeconds(-closestHalfOrFullTime.Second);
            else if (minuteOfTime < 30)
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes((30 - minuteOfTime)).AddSeconds(-closestHalfOrFullTime.Second);
            else if (minuteOfTime < 45)
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes(-(minuteOfTime - 30)).AddSeconds(-closestHalfOrFullTime.Second);
            else
                closestHalfOrFullTime = closestHalfOrFullTime.AddMinutes((60 - minuteOfTime)).AddSeconds(-closestHalfOrFullTime.Second);
            HangfireSuccessLog startLog = new HangfireSuccessLog();
            startLog.NotificationDate = closestHalfOrFullTime;
            startLog.StatusDescription = "NotifRecord Job Başladı";
            startLog.StatusCode = "";
            startLog.SResponseFromServer = "";
            startLog.PatientPhone = "";

            _hangfireSuccessLogService.SaveLogToDb(startLog);

            string myExactTime = closestHalfOrFullTime.AddMinutes(-30).ToString("HH:mm");
            List<NotificationMedicine> patientsDataForNotification = _medicineRecordService.GetDataForMedicineNotification(myExactTime);
            
            foreach ( var notification in notificationRecordList)
            {
                var patientData = patientsDataForNotification.Where(q => q.PatientId == notification.PatientId).FirstOrDefault();

                if (patientData == null)
                    continue;

                var attemp = 0;

                while (attemp < 3)
                {
                    try
                    {
                        List<string> users = new List<string>();
                        users.Add(notification.Token);

                        List<Dictionary<string, string>> buttons = new List<Dictionary<string, string>>();
                        buttons.Add(new Dictionary<string, string>() { { "id", "id_confirm" }, { "text", "Onayla" }, });
                        buttons.Add(new Dictionary<string, string>() { { "id", "id_delay" }, { "text", "Ertele" }, });

                        Dictionary<string, string> contents = new Dictionary<string, string>();
                        contents.Add("en", $"It's {patientData.CurrentTime}, time to get {patientData.MedicineName}");
                        contents.Add("tr", $"Saat {patientData.CurrentTime}, {patientData.MedicineName} kullanmayı unutma.");

                        Dictionary<string, string> headings = new Dictionary<string, string>();
                        headings.Add("en", $"Glaucot Medicine Reminder");
                        headings.Add("tr", $"Glaucot İlaç Hatırlatıcı");

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

                        string serilizedRequestData = JsonConvert.SerializeObject(oneSignalNotification);

                        var client = new RestClient("https://onesignal.com/api/v1/notifications");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", "Basic OGUzNmMyYTEtYWNiNi00MWQ3LWJiMTUtYjhjZTQyMzI1N2E3");
                        request.AddHeader("Content-Type", "application/json");

                        request.AddParameter("application/json", serilizedRequestData, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            HangfireSuccessLog hangfireSuccessLog = new HangfireSuccessLog();
                            hangfireSuccessLog.NotificationDate = closestHalfOrFullTime;
                            hangfireSuccessLog.StatusDescription = "CANLI - SUCCESS";
                            hangfireSuccessLog.StatusCode = response.StatusCode.ToString();
                            hangfireSuccessLog.SResponseFromServer = response.Content;
                            hangfireSuccessLog.PatientPhone = patientData.PatientPhoneNumber;

                            _hangfireSuccessLogService.SaveLogToDb(hangfireSuccessLog);
                        }
                        else
                        {
                            patientsDataForNotification.Remove(patientsDataForNotification.Single(p => p.PatientPhoneNumber == patientData.PatientPhoneNumber));
                            throw new ArgumentOutOfRangeException(response.Content, patientData.PatientPhoneNumber);
                        }
                    }
                    catch (Exception e)
                    {
                        attemp = attemp + 1;

                        HangfireErrorLog hangfireErrorLog = new();
                        hangfireErrorLog.LogSource = e?.Source;
                        hangfireErrorLog.LogMessage = e?.Message;
                        hangfireErrorLog.LogStackTrace = e?.StackTrace;
                        hangfireErrorLog.LogInnerException = e.InnerException?.Message;
                        hangfireErrorLog.LogTime = closestHalfOrFullTime;
                        _hangfireErrorLogService.SaveLogToDb(hangfireErrorLog);
                    }
                }
            }            
        }
    }
}
