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
        Task<IEnumerable<ResumeViceInfo>> GetData(string filter);
        Task<List<PositionListInfo>> GetVicePos(int vid);
        Task<IEnumerable<PositionListInfo>> GetSalerCompanyByPosWord(string salerUserId,string  keyWord);
        /// <summary>
        /// 添加副简历
        /// </summary>
        /// <param name="memId"></param>
        /// <param name="salerUserId"></param>
        /// <param name="userId"></param>
        /// <param name="posId"></param>
        /// <returns></returns>
        Task<int> AddMemLibCommendVice(int memId, int salerUserId,int userId,int posId);
    }
}
