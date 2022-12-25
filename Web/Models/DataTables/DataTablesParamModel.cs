using System.Collections.Generic;

namespace Web.Models.DataTables
{
    public class DataTablesParamModel
    {
        public int draw { get; set; }

        public int start { get; set; }

        public int length { get; set; }

        public DataTablesSearchModel search { get; set; }

        public List<DataTablesOrderModel> order { get; set; }
    }

    public class DataTablesOrderModel
    {
        public DataTablesOrderModel()
        {
            column = -1;
            dir = "";
        }

        public int column { get; set; }

        public string dir { get; set; }
    }

    public class DataTablesSearchModel
    {
        public string value { get; set; }

        public bool regex { get; set; }
    }
}
