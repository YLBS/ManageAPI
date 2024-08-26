using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.GoodBoss;
using Entity.GoodjobOther;
using IService.Common;
using Microsoft.EntityFrameworkCore;
using Model.Common;
using Newtonsoft.Json.Linq;

namespace Service.Common
{
    public class City : ICity
    {
        private readonly Goodjob_OtherContext _context;

        public City(Goodjob_OtherContext context)
        {
            _context = context;
        }
        public async Task<List<KeyValue>> GetCity(int provinceId)
        {
            int[] array = { 10, 724, 725 };
            var list = await _context.DicCities.Where(d=>d.ProvinceId== provinceId && !array.Contains(d.Id)).OrderBy(d => d.OrderId).Select(d => new KeyValue
            {
                Id = d.Id,
                Name = d.Pname
            }).ToListAsync();
            return list;
        }

        public async Task<List<KeyValue>> GetCityForGd()
        {
            var list = await _context.DicCities.Where(d => d.ProvinceId == 1 && d.IsShow==0).OrderBy(d => d.OrderId).Select(d => new KeyValue
            {
                Id = d.Id,
                Name = d.Pname
            }).ToListAsync();
            return list;
        }

        public async Task<List<KeyValue>> GetDistrict(int cityId)
        {
            var list = await _context.DicCities.Where(d => d.IsShow == cityId).OrderBy(d => d.OrderId).Select(d => new KeyValue
            {
                Id = d.Id,
                Name = d.Pname
            }).ToListAsync();
            return list;
        }

        public async Task<List<KeyValue>> GetProvince()
        {
           // string sql = @"select ID,PName  from  Goodjob_Other.dbo.Dic_Province order by orderid asc";

            var list = await _context.DicProvinces.OrderBy(d=>d.OrderId).Select(d=>new KeyValue
            {
                Id = d.Id,
                Name = d.Pname
            }).ToListAsync();
            return list;
        }

        public async Task<List<KeyValue>> GetTownForGz(int cityId)
        {

            var list = await _context.DicTowns.Where(d => d.CityId == cityId).OrderBy(d => d.OrderId).Select(d => new KeyValue
            {
                Id = d.Id,
                Name = d.Pname
            }).ToListAsync();
            return list;
        }
    }
}
