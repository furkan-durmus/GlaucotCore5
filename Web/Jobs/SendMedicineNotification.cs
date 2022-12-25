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
    public class SendMedicineNotification
    {
        IMedicineRecordService _medicineRecordService;
        public SendMedicineNotification(IMedicineRecordService medicineRecordService)
        {
            _medicineRecordService = medicineRecordService;
        }
        public void SendNotificationWithOneSignal()
        {

            DateTime closestHalfOrFullTime = DateTime.Now;
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

            foreach (var notificationData in patientsDataForNotification)
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key=AAAAft2AqF8:APA91bFge8bRCb9Ghq0PYUqyk-tKjk7jJgq9QlYzdJAeAWiu9crwudE0_CCpaOP31L2fx87Fj_vlgoHAus2U8AmRIAZreFIVUYkAd1P1ajk95uIu4dba93IUt8JSQPLv-RF4C0c_IvSs"));
                tRequest.ContentType = "application/json";
                var payload = new
                {
                    to = notificationData.PatientNotificationToken,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        title = "Glaucot Medicine Reminder",
                        body = $"It's {notificationData.CurrentTime}, time to get {notificationData.MedicineName}",
                        badge = 1
                    },

                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    //result.Response = sResponseFromServer;
                                }
                        }
                    }
                }

            }

        }
    }
}
