using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IPosService
    {
        Task<(IEnumerable<EffectPositionInfo> item,int totalRecords,string memName)> GetData(int memId, int pageSize, int pageIndex);
        /// <summary>
        /// 刷新选中职位，返回不刷新的职位名称
        /// </summary>
        /// <returns></returns>
        Task<string> RefreshSelect(int[] ids);
        /// <summary>
        /// 刷新所有发布中的岗位
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>

        Task<string> RefreshAll(int memId);
        /// <summary>
        /// 获取还在排队推送中的总数
        /// </summary>
        /// <param name="where">拼接的条件</param>
        /// <returns></returns>
        Task<int> Get_WxPushingCount(string where);

        Task<(int count,int syscount,int sdcount,bool tf)> GetWxPusRecordCount(int memId);
        /// <summary>
        /// 判断企业是否在不推送名单内,在就不推送，返回false
        /// </summary>
        /// <returns></returns>
        Task<bool> EstimatePush(int memId);
        /// <summary>
        /// 推送取反
        /// </summary>
        /// <returns></returns>

        Task<bool> PushNegation(int memId);
        /// <summary>
        /// 创建微信分享的字符串
        /// </summary>
        /// <returns></returns>

        Task<(string str, int count)> CreateWxShareStr(int[] ids);
        /// <summary>
        /// 生成兼职微信分享的字符串
        /// </summary>
        /// <returns></returns>
        Task<(string str, int count)> CreatePtWxShareStr(int memId);
        /// <summary>
        /// 刷新发布中的兼职岗位
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        Task<string> RefreshPtAll(int memId);
        /// <summary>
        /// 添加岗位推送
        /// </summary>
        /// <returns></returns>
        Task<(string msg, bool result)> AddSimulationPosition(int[] posIds,int memId);
    }
}
