using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class SelectedDatabase
    {
        public const string LocalContext = @"NOT CREATED YET";

        public const string LiveServerTest = @"Server=rd-minio-win.guzelhosting.com\MSSQLSERVER2019;Database=tuncayal_GLOTEST;User Id=tuncayal_furkan;password=Ankara06-;TrustServerCertificate=True;";

        public const string LiveServerReal = "NOT CREATED YET";
    }
}
