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
        private readonly INotificationRecordService _notificationRecordService;
        private readonly IHangfireErrorLogService _hangfireErrorLogService;
        private readonly IHangfireSuccessLogService _hangfireSuccessLogService;
        private readonly IPatientService _patientService;
        public CheckNotificationRecords(INotificationRecordService notificationRecordService, IHangfireErrorLogService hangfireErrorLogService, IHangfireSuccessLogService hangfireSuccessLogService, IPatientService patientService)
        {
            _notificationRecordService = notificationRecordService;
            _hangfireErrorLogService = hangfireErrorLogService;
            _hangfireSuccessLogService = hangfireSuccessLogService;
            _patientService = patientService;
        }

        public void CheckNotification()
        {
            ////_notificationRecordService.RemoveNotificationRecords();  // Kayıt silme işi iptal denildi

            //DateTime closestHalfOrFullTime = DateTime.Now; //TimeZoneInfo.ConvertTime(localServerTime, info);

            //HangfireSuccessLog startLog = new HangfireSuccessLog();
            //startLog.NotificationDate = closestHalfOrFullTime;
            //startLog.StatusDescription = "NotifRecord Job Başladı";
            //startLog.StatusCode = "";
            //startLog.SResponseFromServer = "";
            //startLog.PatientPhone = "";

            //_hangfireSuccessLogService.SaveLogToDb(startLog);

            //var notificationRecordList = _notificationRecordService.GetAllSendNotifications();

            //foreach ( var notification in notificationRecordList)
            //{
            //    var attemp = 0;

            //    while (attemp < 3)
            //    {
            //        try
            //        {
            //            List<string> users = new List<string>();
            //            users.Add(notification.Token);

            //            List<Dictionary<string, string>> buttons = new List<Dictionary<string, string>>();
            //            buttons.Add(new Dictionary<string, string>() { { "id", "id_confirm" }, { "text", "Onayla" }, });
            //            buttons.Add(new Dictionary<string, string>() { { "id", "id_delay" }, { "text", "Ertele" }, });

            //            //string s = "[en, It's 13:30, time to get Parol]-[tr, Saat 13:30, Parol kullanmayı unutma.]";

            //            string dbContentEn = notification.Content.Substring(0, notification.Content.IndexOf("-")).Replace("[", "").Replace("]", "");
            //            string dbContentTr = notification.Content.Substring(notification.Content.IndexOf("-") + 1, notification.Content.Length - notification.Content.IndexOf("-") - 1).Replace("[", "").Replace("]", "");

            //            string en = dbContentEn.Substring(0, 2);
            //            string tr = dbContentTr.Substring(0, 2);

            //            string contentEn = dbContentEn.Substring(4, dbContentEn.Length - 4);
            //            string contentTr = dbContentTr.Substring(4, dbContentTr.Length - 4);

            //            Dictionary<string, string> contents = new Dictionary<string, string>();
            //            contents.Add(en, contentEn);
            //            contents.Add(tr, contentTr);

            //            Dictionary<string, string> headings = new Dictionary<string, string>();
            //            headings.Add("en", $"Glaucot Medicine Reminder");
            //            headings.Add("tr", $"Glaucot İlaç Hatırlatıcı");

            //            OneSignalNotification oneSignalNotification = new OneSignalNotification();
            //            oneSignalNotification.app_id = "f196579d-dc71-404a-9d92-c5311836d8c1";
            //            oneSignalNotification.name = "INTERNAL_CAMPAIGN_NAME";
            //            oneSignalNotification.include_player_ids = users;
            //            oneSignalNotification.buttons = buttons;
            //            oneSignalNotification.contents = contents;
            //            oneSignalNotification.priority = 10;
            //            oneSignalNotification.ttl = 0;
            //            oneSignalNotification.content_available = true;
            //            oneSignalNotification.android_channel_id = "9113e0b5-9b25-46c3-8abe-f56ba4827261";
            //            oneSignalNotification.headings = headings;

            //            string serilizedRequestData = JsonConvert.SerializeObject(oneSignalNotification);

            //            var client = new RestClient("https://onesignal.com/api/v1/notifications");
            //            var request = new RestRequest(Method.POST);
            //            request.AddHeader("Authorization", "Basic OGUzNmMyYTEtYWNiNi00MWQ3LWJiMTUtYjhjZTQyMzI1N2E3");
            //            request.AddHeader("Content-Type", "application/json");

            //            request.AddParameter("application/json", serilizedRequestData, ParameterType.RequestBody);
            //            IRestResponse response = client.Execute(request);

            //            if (response.StatusCode == HttpStatusCode.OK)
            //            {
            //                HangfireSuccessLog hangfireSuccessLog = new HangfireSuccessLog();
            //                hangfireSuccessLog.NotificationDate = closestHalfOrFullTime;
            //                hangfireSuccessLog.StatusDescription = "CANLI - SUCCESS";
            //                hangfireSuccessLog.StatusCode = response.StatusCode.ToString();
            //                hangfireSuccessLog.SResponseFromServer = response.Content;
            //                hangfireSuccessLog.PatientPhone = _patientService.Get(notification.PatientId).PatientPhoneNumber;

            //                _hangfireSuccessLogService.SaveLogToDb(hangfireSuccessLog);
            //            }
            //            else
            //            {
            //                notificationRecordList.Remove(notificationRecordList.Single(p => p.PatientId == notification.PatientId));
            //                throw new ArgumentOutOfRangeException(response.Content, _patientService.Get(notification.PatientId).PatientPhoneNumber);
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            attemp = attemp + 1;

            //            HangfireErrorLog hangfireErrorLog = new();
            //            hangfireErrorLog.LogSource = e?.Source;
            //            hangfireErrorLog.LogMessage = e?.Message;
            //            hangfireErrorLog.LogStackTrace = e?.StackTrace;
            //            hangfireErrorLog.LogInnerException = e.InnerException?.Message;
            //            hangfireErrorLog.LogTime = closestHalfOrFullTime;
            //            _hangfireErrorLogService.SaveLogToDb(hangfireErrorLog);
            //        }
            //    }
            //}            
        }
    }
}
