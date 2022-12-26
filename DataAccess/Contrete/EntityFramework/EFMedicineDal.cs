using Core.DataAccess.EntityFrameWork;
using DataAccess.Abstract;
using DataAccess.DTOS;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contrete.EntityFramework
{
    public class EFMedicineDal : EFEntityRepositoryBase<Medicine,GlaucotContext> , IMedicineDal
    {
        public async Task<PagingModelResponse<Medicine>> MedicineDalDataTable(string textSearch, int orderCol, bool orderDescending,
int start, int length)
        {
            using (GlaucotContext context = new GlaucotContext())
            {
                var query = context.Medicines.AsQueryable();

                if (textSearch != null)
                {
                    int.TryParse(textSearch, out int age);
                    query = query.Where(q => q.MedicineName.Contains(textSearch));
                }

                if (orderCol == 0)
                    query = orderDescending ? query.OrderByDescending(v => v.MedicineName) : query.OrderBy(v => v.MedicineName);

                var count = await query.CountAsync();

                var data = await query.Skip(start).Take(length).ToListAsync();

                return new PagingModelResponse<Medicine>
                {
                    PageSize = data.Count,
                    ItemCount = count,
                    DidError = false,
                    Model = data,
                    PageNumber = start / length
                };
            }
        }
    }
}
