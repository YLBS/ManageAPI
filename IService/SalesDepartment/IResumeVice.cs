using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IResumeVice
    {

        Task<IEnumerable<ResumeViceInfo>> GetData_N(string keyWord,int depId,int depUserId); 
        Task<IEnumerable<ResumeViceInfo>> GetViceResume(string filter);
        Task<List<PositionListInfo>> GetVicePos(int vid);
        Task<IEnumerable<PositionListInfo>> GetSalerCompanyByPosWord(string salerUserId,string  keyWord);
        /// <summary>
        /// 添加副简历
        /// </summary>
        /// <param name="memIdPosIds"></param>
        /// <param name="viceIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> AddMemLibCommendVice(string memIdPosIds, string viceIds, int userId);
    }
}
