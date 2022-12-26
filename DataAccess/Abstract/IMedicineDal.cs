using Core.DataAccess;
using DataAccess.DTOS;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IMedicineDal : IEntitiyRepository<Medicine>
    {
        Task<PagingModelResponse<Medicine>> MedicineDalDataTable(string textSearch, int orderCol, bool orderDescending,
int start, int length);
    }
}
