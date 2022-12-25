using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOS
{
    public class PagingModelResponse<TModel>
    {
        public bool DidError { get; set; }

        public string ErrorMessage { get; set; }
        public List<TModel> Model { get; set; }
        public int ItemCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PageCount => ItemCount / PageSize;
    }
}
