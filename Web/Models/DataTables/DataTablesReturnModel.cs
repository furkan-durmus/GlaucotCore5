﻿namespace Web.Models.DataTables
{
    public class DataTablesReturnModel
    {
        public int draw { get; set; }

        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }

        public object data { get; set; }

        public string error { get; set; }
    }
}
