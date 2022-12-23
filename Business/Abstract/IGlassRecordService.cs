using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGlassRecordService
    {
        List<GlassRecord> GetAll(Guid patientId);
        void Add(GlassRecord glassRecord);
        void Update(GlassRecord glassRecord);
        void Delete(GlassRecord glassRecord);
        string UpdateOrAddGlassRecord(Guid patientId);
    }
}
