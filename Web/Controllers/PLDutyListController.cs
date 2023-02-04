using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System;
using System.Linq;

namespace Web.Controllers
{
    public class PLDutyListController : Controller
    {
        IPatientService _patientService;
        IMedicineRecordService _medicineRecordService;
        INotificationRecordService _notificationRecordService;
        IHangfireErrorLogService _hangfireErrorLogService;
        IHangfireSuccessLogService _hangfireSuccessLogService;
        public PLDutyListController(IPatientService patientService, IMedicineRecordService medicineRecordService, INotificationRecordService notificationRecordService, IHangfireErrorLogService hangfireErrorLogService, IHangfireSuccessLogService hangfireSuccessLogService)
        {
            _patientService = patientService;
            _medicineRecordService = medicineRecordService;
            _notificationRecordService = notificationRecordService;
            _hangfireErrorLogService = hangfireErrorLogService;
            _hangfireSuccessLogService = hangfireSuccessLogService;
        }

        public IActionResult OptimusPrime1()
        {
            //DateTime closestHalfOrFullTime = DateTime.Now.AddHours(10);
            //DateTime closestHalfOrFullTime = DateTime.Parse("16.12.2022 11:01:05");

            //var info = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            //DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTime closestHalfOrFullTime = DateTime.Now; //TimeZoneInfo.ConvertTime(localServerTime, info);


            HangfireSuccessLog startLog = new HangfireSuccessLog();
            startLog.NotificationDate = closestHalfOrFullTime;
            startLog.StatusDescription = "Job Başladı..";
            startLog.StatusCode = "";
            startLog.SResponseFromServer = "";
            startLog.PatientPhone = "";

            _hangfireSuccessLogService.SaveLogToDb(startLog);

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

            while (attemp < 3 && patientsDataForNotification.Count > 0)
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
                            PatientId = notificationData.PatientId,
                            Token = notificationData.PatientNotificationToken
                        });

                        Dictionary<string, string> notificationRecordData = new Dictionary<string, string>();
                        notificationRecordData.Add("notificationRecordId", $"{notificationRecordId}");
                        notificationRecordData.Add("patientId", $"{users.First()}");

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

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            HangfireSuccessLog hangfireSuccessLog = new HangfireSuccessLog();
                            hangfireSuccessLog.NotificationDate = closestHalfOrFullTime;
                            hangfireSuccessLog.StatusDescription = "CANLI - SUCCESS";
                            hangfireSuccessLog.StatusCode = response.StatusCode.ToString();
                            hangfireSuccessLog.SResponseFromServer = response.Content;
                            hangfireSuccessLog.PatientPhone = notificationData.PatientPhoneNumber;

                            _hangfireSuccessLogService.SaveLogToDb(hangfireSuccessLog);
                        }
                        else
                        {
                            patientsDataForNotification.Remove(patientsDataForNotification.Single(p => p.PatientPhoneNumber == notificationData.PatientPhoneNumber));
                            throw new ArgumentOutOfRangeException(response.Content, notificationData.PatientPhoneNumber);
                        }
                    }
                    break;
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
            return RedirectToAction("Index", "Home");
        }

        public IActionResult OptimusPrime2()
        {
            _hangfireSuccessLogService.ClearOldSuccessLogs(48);

            return RedirectToAction("Index","Home");
        }

        public IActionResult OptimusPrime3()
        {   
            //_notificationRecordService.RemoveNotificationRecords();  // Kayıt silme işi iptal denildi

            DateTime closestHalfOrFullTime = DateTime.Now; //TimeZoneInfo.ConvertTime(localServerTime, info);

            HangfireSuccessLog startLog = new HangfireSuccessLog();
            startLog.NotificationDate = closestHalfOrFullTime;
            startLog.StatusDescription = "NotifRecord Job Başladı";
            startLog.StatusCode = "";
            startLog.SResponseFromServer = "";
            startLog.PatientPhone = "";

            _hangfireSuccessLogService.SaveLogToDb(startLog);

            var notificationRecordList = _notificationRecordService.GetAllSendNotifications();

            foreach (var notification in notificationRecordList)
            {
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

                        //string s = "[en, It's 13:30, time to get Parol]-[tr, Saat 13:30, Parol kullanmayı unutma.]";

                        string dbContentEn = notification.Content.Substring(0, notification.Content.IndexOf("-")).Replace("[", "").Replace("]", "");
                        string dbContentTr = notification.Content.Substring(notification.Content.IndexOf("-") + 1, notification.Content.Length - notification.Content.IndexOf("-") - 1).Replace("[", "").Replace("]", "");

                        string en = dbContentEn.Substring(0, 2);
                        string tr = dbContentTr.Substring(0, 2);

                        string contentEn = dbContentEn.Substring(4, dbContentEn.Length - 4);
                        string contentTr = dbContentTr.Substring(4, dbContentTr.Length - 4);

                        Dictionary<string, string> contents = new Dictionary<string, string>();
                        contents.Add(en, contentEn);
                        contents.Add(tr, contentTr);

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
                            hangfireSuccessLog.PatientPhone = _patientService.Get(notification.PatientId).PatientPhoneNumber;

                            _hangfireSuccessLogService.SaveLogToDb(hangfireSuccessLog);
                        }
                        else
                        {
                            notificationRecordList.Remove(notificationRecordList.Single(p => p.PatientId == notification.PatientId));
                            throw new ArgumentOutOfRangeException(response.Content, _patientService.Get(notification.PatientId).PatientPhoneNumber);
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

            return RedirectToAction("Index", "Home");
        }

    }
}
