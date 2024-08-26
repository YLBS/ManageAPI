using System.Collections;
using IService.SalesDepartment;
using Microsoft.Extensions.Caching.Memory;
using Model;
using Model.enums;
using Model.SalesDepartment;
using Newtonsoft.Json.Linq;
using Service.SalesDepartment;

namespace ManageNew.CacheManageTool
{
    /// <summary>
    /// 检查权限
    /// </summary>
    public class CheckPermission
    {

        private  readonly IMemoryCache _cache;
        private readonly IMyCompany _company;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="company"></param>
        public CheckPermission(IMemoryCache cache, IMyCompany company)
        {
            _cache = cache;
            _company = company;
        }
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckCompanyViewPermission(CompanyViewEnum pagePermission,int userId)
        {
            int rank = await GetUserPermissionRank((int)SectionEnum.CompanyView, userId);

            //只有 rank 和 pagePermission 相等，或者 rank = 255,才返回true,为有权限

            pagePermission = pagePermission & (CompanyViewEnum.All ^ (CompanyViewEnum)rank);

            return ((int)pagePermission).Equals(0);
        }

        private async Task<int> GetUserPermissionRank(int sectionId,int userId)
        {
            var dict = await  GetUserSectionFromHashTable(userId);

            var rank = dict.Where(s => s.SectionId == sectionId).Select(s=>s.Rank).LastOrDefault();

            return rank;
        }

        private async Task<IEnumerable<SectionModel>> GetUserSectionFromHashTable(int userId)
        {
            string cacheKey = "UserSectionHashTable" + userId;
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<SectionModel> result))
            {
                result = await _company.GetUserSectionDictionary(userId);

                _cache.Set(cacheKey, result);
            }
            return result;
        }
    }
}
