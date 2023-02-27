using Core;
using System;
using System.Collections.Generic;

namespace Web.Models.Doctor
{
    public class PatientNotificationRecordViewModel
    {
        public List<NotificationRecordDetail> NotificationRecordDetail { get; set; }
        public string MedicineName { get; set; }
    }


    public class NotificationRecordDetail
    {
        public int Cycle { get; set; }
        public NotificationRecordStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
