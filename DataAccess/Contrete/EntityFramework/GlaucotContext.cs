using Core;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contrete.EntityFramework
{
    public class GlaucotContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(SelectedDatabase.LiveServerTest);
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<GlassRecord> GlassRecords { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineRecord> MedicineRecords { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<OTP> OTPs { get; set; }
    }
}
