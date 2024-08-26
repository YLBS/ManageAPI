using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.GoodjobOther;
using IService.Common;
using Microsoft.EntityFrameworkCore;
using Model.Common;

namespace Service.Common
{
    public class DicService : IDicService
    {
        private readonly Goodjob_OtherContext _context;

        public DicService(Goodjob_OtherContext context)
        {
            _context = context;
        }
        public async Task<List<KeyValue>> GetSalaryNew()
        {
            var list = await _context.DicSalaryNews.OrderBy(d => d.OrderId).Select(d => new KeyValue
            {
                Id = d.Id,
                Name = d.Name
            }).ToListAsync();
            return list;
        }
    }
}
