using System.Collections;
using Goodjob.Position;
using Goodjob.Resume;
using Model.Common;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IResumeShareVice
    {

        Task<(IEnumerable<ResumeViceInfo> item,int cout)> GetViceResume(Singles singles);
        Task<(List<ResumeViceInfo> item, int cout)> GetResumeShareViceList(Singles singles);
        /// <summary>
        /// 获取人才简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<ResumeInfo> GetResume(int myUserId);

        /// <summary>
        ///  用于搜索外部简历  基于  [My_RegisterForCrm]  [Sys_RegisterFrom]
        /// </summary>
        /// <returns></returns>
        Task<string> GetExtranetReusmeListQueryIDString(IDictionary dic);
        /// <summary>
        /// 判断 屏蔽该职位，简历编号
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="posId"></param>
        /// <returns></returns>
        Task<bool> IsShieldCompany(int myUserId, int posId);
        /// <summary>
        /// 发送推荐简历给企业,会添加记录 Log_CommendResume
        /// </summary>
        /// <param name="posId"></param>
        /// <param name="myUserId"></param>
        /// <param name="opertype">操作类型</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> CommendResumeLog(int posId,string myUserIds,int opertype,string userId);

        Task CommendResume(int posId, int resumeId, int opertype, string userId);
        /// <summary>
        /// 获取相关数据
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        Task<(int memId,string posName,string contactPerson,string email)> GetDataByPosId(int posId);
        /// <summary>
        /// 获取企业名称以及密码,账号类型
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        Task<(string name,string password,short memberClass)> GetNameAndPassWord(int memId);
        Task<IEnumerable<ResumeListQueryInfo>> GetResumeListQueryCollection(int myUserId);
        /// <summary>
        /// 搜索企业
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        Task<List<KeyValue>> GetCompanyList(string keyWord); 
        /// <summary>
        /// 根据企业Id获取在招岗位
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        Task<List<KeyValue>> GetPosListByMemId(int memId);
        /// <summary>
        /// 获取指定销售员下载指定简历的次数
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="salerId"></param>
        /// <returns></returns>
        Task<int> SalerDownloadResumeCount(int myUserId, int salerId);
        /// <summary>
        /// 原返回Dic_JobFunctions，现只需JobFunction_smallName
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<List<string>> GetJobFunction_s(int myUserId);
        Task<string> GetJobFunctionListBySmallID(int smallId);
        /// <summary>
        /// 检测是否已应聘或推荐
        /// </summary>
        /// <returns></returns>
        Task<bool> Checkcommendlogo(int posId, int myUserId);
        /// <summary>
        /// 獲取邮箱
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<string[]> GetPersonEmail(int[] myUserId);

        Task<PositionInfo> GetPositionById(int posId);

        /// <summary>
        /// 检查简历是否有隐藏简历
        /// </summary>
        /// <param name="resumeIds"></param>
        /// <returns></returns>
        Task<int[]> CheckResume(int[] resumeIds);
    }
}
