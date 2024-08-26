using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Common;

namespace IService.Common
{
    public interface ICity
    {
        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetProvince();
        /// <summary>
        /// 获取城市，不适合用于广东
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetCity(int provinceId);
        /// <summary>
        /// 获取广东的城市
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetCityForGd();
        /// <summary>
        /// 获取县区
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetDistrict(int cityId);
        /// <summary>
        /// 获取镇
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<List<KeyValue>> GetTownForGz(int cityId);
    }
}
