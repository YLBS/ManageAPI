﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IMyCompany
    {
        Task<(IEnumerable<MySalesCompanyListInfo> item,int count)> GetData(string filter);
        Task<bool> AddPubInformation(InputPubInformationDto dto);
        /// <summary>
        /// 检测用户是否有查看该企业的权限
        /// </summary>
        Task<bool> EnableViewCompany(int memId, int userId, int[] departId);
        /// <summary>
        /// 获取用户的所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<SectionModel>> GetUserSectionDictionary(int userId);
    }
}
