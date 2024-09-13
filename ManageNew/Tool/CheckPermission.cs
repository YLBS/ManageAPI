using System.Collections;
using System.Collections.Generic;
using IService.SalesDepartment;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Model;
using Model.enums;
using Model.SalesDepartment;
using Newtonsoft.Json.Linq;
using Service.SalesDepartment;

namespace ManageNew.Tool
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
        /// 检查权限——企业信息查看,无权限返回false
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckCompanyViewPermission(CompanyViewEnum pagePermission,int userId)
        {
            int rank = await GetUserPermissionRank((int)SectionEnum.CompanyView, userId);

            pagePermission = pagePermission & (CompanyViewEnum.All ^ (CompanyViewEnum)rank);

            return ((int)pagePermission).Equals(0);
        }
        /// <summary>
        /// 检查权限——人才信息查看,无权限返回false
        /// </summary>
        /// <param name="pagePermission"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckPersonViewPermission(PersonViewEnum pagePermission, int userId)
        {
            int rank = await GetUserPermissionRank((int)SectionEnum.CompanyView, userId);

            pagePermission = pagePermission & (PersonViewEnum.All ^ (PersonViewEnum)rank);

            return ((int)pagePermission).Equals(0);
        }
        /// <summary>
        /// 检查权限—人才相关管理,无权限返回false
        /// </summary>
        /// <param name="pagePermission"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckPersonPermission(PersonEnum pagePermission, int userId)
        {
            int rank = await GetUserPermissionRank((int)SectionEnum.CompanyView, userId);

            pagePermission = pagePermission & (PersonEnum.All ^ (PersonEnum)rank);

            return ((int)pagePermission).Equals(0);
        }

        private async Task<int> GetUserPermissionRank(int sectionId,int userId)
        {
            int rank = 0;
            var dict = await  GetUserSectionFromHashTable(userId);
            if (dict.Contains(sectionId))
            {
                rank = Convert.ToInt32(dict[sectionId]);
            }
            //dict.TryGetValue(sectionId, out int rank);
            // var rank = dict.Where(s => s.SectionId == sectionId).Select(s=>s.Rank).LastOrDefault();

            return rank;
        }

        private async Task<IDictionary> GetUserSectionFromHashTable(int userId)
        {
            string cacheKey = "UserSectionHashTable" + userId;
            if (!_cache.TryGetValue(cacheKey, out IDictionary dic))
            {
                dic = new Dictionary<int, int>();
                var result = await _company.GetUserSectionDictionary(userId);
                foreach (var section in result)
                {
                    if (!dic.Contains(section.SectionId))
                        dic.Add(section.SectionId, section.Rank);
                    else
                    {
                        int oldRank = Convert.ToInt32(dic[section.SectionId]);
                        dic[section.SectionId] = oldRank | section.Rank;
                    }
                }
                _cache.Set(cacheKey, dic);
            }
            return dic;
        }
    }
}
