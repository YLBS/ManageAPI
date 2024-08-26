using IService.Common;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers.Common
{
    /// <summary>
    /// 获取城市的接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "共享接口")]
    public class CityController : ControllerBase
    {
        private readonly ICity _common;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="common"></param>
        public CityController(ICity common)
        {
            _common = common;
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetProvince()
        {
            var result = await _common.GetProvince();
            return Ok(ResultMode<Object>.Success(result));
        }
        /// <summary>
        /// 获取城市，不适合用于广东
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCity(int provinceId)
        {
            var result = await _common.GetCity(provinceId);
            if (result.Count == 0)
                return Ok(ResultMode<Object>.Success(null));
            return Ok(ResultMode<Object>.Success(result));
        }
        /// <summary>
        /// 获取广东的城市
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCityForGd()
        {
            var result = await _common.GetCityForGd();
            return Ok(ResultMode<Object>.Success(result));
        }
        /// <summary>
        /// 获取县区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDistrict(int cityId)
        {
            var result = await _common.GetDistrict(cityId);
            if (result.Count == 0)
                return Ok(ResultMode<Object>.Success(null));
            return Ok(ResultMode<Object>.Success(result));
        }
        /// <summary>
        /// 获取镇
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTown(int cityId)
        {
            var result = await _common.GetTownForGz(cityId);
            if(result.Count==0)
                return Ok(ResultMode<Object>.Success(null));
            return Ok(ResultMode<Object>.Success(result));
        }
    }
}
