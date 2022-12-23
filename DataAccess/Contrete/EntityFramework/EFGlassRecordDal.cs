using Core.DataAccess.EntityFrameWork;
using Core.Entities;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contrete.EntityFramework
{
    public class EFGlassRecordDal : EFEntityRepositoryBase<GlassRecord, GlaucotContext>, IGlassRecordDal
    {
        public GlassRecord GetLastRecordOfPatient(Guid patientId)
        {
            using (GlaucotContext context = new GlaucotContext())
            {
                return (from glassRecords in context.GlassRecords
                       where glassRecords.PatientId == patientId
                       orderby glassRecords.StartDate descending
                       select new GlassRecord
                       {
                           Id = glassRecords.Id,
                           PatientId = glassRecords.PatientId,
                           StartDate = glassRecords.StartDate,
                           EndDate = glassRecords.EndDate,
                           IsActive = glassRecords.IsActive

                       }).FirstOrDefault(); 
            }
        }
    }
}
