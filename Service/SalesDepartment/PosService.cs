using Entity.Goodjob;
using IService.SalesDepartment;
using Microsoft.EntityFrameworkCore;
using Model.SalesDepartment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using IService.Common;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Text.RegularExpressions;
using ServiceStack;
using System.Security.Cryptography;
using Goodjob.Common;
using System.Dynamic;

namespace Service.SalesDepartment
{
    public class PosService : IPosService
    {
        private readonly GoodjobContext _context;
        private readonly IDicService _dicService;

        public PosService(GoodjobContext context, IDicService dicService)
        {
            _context = context;
            _dicService = dicService;
        }
        public async Task<(IEnumerable<EffectPositionInfo> item, int totalRecords, string memName)> GetData(int memId, int pageSize, int pageIndex)
        {

            var memName = await _context.MemInfos.Where(m => m.MemId == memId).Select(m => m.MemName).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(memName))
            {
                return (null, 0, "");
            }

            var parm = new { MemID = memId, PageSize=pageSize, PageIndex=pageIndex };
            var multiReader = await _context.Database.GetDbConnection().QueryMultipleAsync("Sales_Effect_PositionInfo", parm, commandType: CommandType.StoredProcedure);

            var totalRecords = multiReader.Read<int>().ToList().FirstOrDefault();

            var result = multiReader.Read<EffectPositionInfo>();

            multiReader.Dispose();
            return (result, totalRecords, memName);
        }


        public async Task<string> RefreshAll(int memId)
        {
            //原查存储过程 Mng_GetPosStateByID
            int[] ids = await _context.MemPositions.Where(m => m.PosState == 2 && m.MemId==memId).Select(m => m.PosId).ToArrayAsync();
            if (ids.Length > 0)
            {

                var posName = await RefreshSelect(ids);
                return posName;
            }
            return "无发布中职位";
        }

        public async Task<string> RefreshSelect(int[] ids)
        {
            //获取不刷新的职位ID和职位名称
            var l = await (from a in _context.MemPosNoRefreshes
                join b in _context.MemPositions on a.PosId equals b.PosId 
                where  a.IsFlag == true && ids.Contains(a.PosId)
                select new
                {
                    a.PosId,
                    b.PosName,
                }).ToListAsync();

            string posNames =string.Join(",", l.Select(s=>s.PosName));
            var ids1 = l.Select(s => s.PosId).ToArray();
            if (ids1.Length>0)
            {
                foreach (var i in ids)
                {
                    foreach (var j in ids1)
                    {
                        if(i==j) //去重
                            ids = ids.Where(s => s != i).ToArray();
                    }
                }
            }
            string posIds = string.Join(",", ids);
            string sql = $"UPDATE Mem_Position SET PostDate= Getdate() WHERE PosID in ({posIds});" +
                         $"UPDATE goodjob_Query.dbo.MemPos_Query SET PostDate=getdate() WHERE PosID  in ({posIds})";

            var result = await _context.Database.GetDbConnection().ExecuteAsync(sql, null);
            return posNames;
        }


        public async Task<int> Get_WxPushingCount(string where)
        {
            //原 statBLL.Get_WxPusRecordList

            string sql = "select count(*) from [Mng_WxPusRecord] where 1=1 and  DATEDIFF(DD,SendDateTime,GETDATE()) = 0  and (Result = '' or  CHARINDEX('待发送',Result) > 0) and id > 983 and memId != 0";
            var result = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<int>(sql);
            return result;
        }

        public async Task<(int count, int syscount, int sdcount, bool tf, string msg)> GetWxPusRecordCount(int memId,int userId)
        {
            var l = await _context.MngWxPusRecords.Where(m => m.SendDateTime.Date == DateTime.Today).ToListAsync(); //今日发
            var l1=l.Where(m=>m.MemId==memId).ToList();//给这家企业发的
            var l2 = l.Where(m => m.EplId == userId).ToList(); //当前登录用户 今天发的

            var list = l1.Select(m => m.SendType).ToList();

            int count=list.Count; //发送的总数
            int sysCount = list.Where(s=>s==1).Count();//系统发的数量
            int sdCount = list.Where(s => s == 2).Count();//手动发的数量

            var mngWxPushLimit = await _context.MngWxPushLimits.FirstOrDefaultAsync();
            if (mngWxPushLimit != null)
            {
                int dayCount = mngWxPushLimit.DayCount; //每日限量
                int c = mngWxPushLimit.EplDayCount;//业务员每日次数
                if (sdCount >= dayCount || l2.Count>= c)  //按钮不可见
                {
                    return (count, sysCount, sdCount,false, "超过日限制");
                }
                else
                {
                    var memInfo=await _context.MemInfos.Where(m=>m.MemId==memId).FirstOrDefaultAsync();
                    if (memInfo != null)
                    {
                        //string[] regionCid = mngWxPushLimit.RegionCid.Split(',');
                        //bool t = string.IsNullOrEmpty(regionCid.Where(r => r.Contains(memInfo.AddressC.ToString())).FirstOrDefault());
                        //if (t) //不在允许的地区
                        //{
                        //    //return (count, sysCount, sdCount, false);
                        //}
                        string[] callHit = mngWxPushLimit.CalingName.Split(',');
                        string callingName= NameProvider.GetIndustryName(memInfo.Calling);
                        for (int i = 0; i < callHit.Length; i++)
                        {
                            if (callingName.Contains(callHit[i])) //不在允许的行业范围
                            {
                                return (count, sysCount, sdCount, false, "不在允许的行业范围"+callingName + "-" + callHit[i]);
                            }
                        }
                        string[] memNameHit = mngWxPushLimit.MemKeyWored.Split(',');
                        for (int i = 0; i < memNameHit.Length; i++)
                        {
                            if (memInfo.MemName.Contains(memNameHit[i])) //不在允许的企业名
                            {
                                return (count, sysCount, sdCount, false,
                                    "不在允许的企业名" + memInfo.MemName + "-" + memNameHit[i]);
                            }
                        }
                        string[] memIdHit = mngWxPushLimit.MemId.Split(',');
                        for (int i = 0; i < memIdHit.Length; i++)
                        {
                            if (memIdHit[i].Contains(memInfo.MemId.ToString())) //不在允许的企业Id
                            {
                                return (count, sysCount, sdCount, false, "不在允许的企业Id:" + memId);
                            }
                        }
                    }
                    
                }
            }
            return (count, sysCount, sdCount, true, "");
        }

        public async Task<bool> EstimatePush(int memId)
        {
            var list = await _context.MemNotPushes.Where(m => m.MemId == memId).FirstOrDefaultAsync();

            if (list != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> PushNegation(int memId)
        {
            var memInfo=await _context.MemInfos.Where(m => m.MemId == memId).FirstOrDefaultAsync();
            if (memInfo == null)
            {
                return false;
            }
            var list = await _context.MemNotPushes.Where(m => m.MemId == memId).FirstOrDefaultAsync();
            if (list != null)
            {
                 _context.Remove(list);
            }
            else
            {
                MemNotPush m=new MemNotPush();
                m.MemId = memId;
                _context.Add(m);
            }
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<(string str, int count)> CreateWxShareStr(int[] ids)
        {
            var posList = await _context.MemPositions.Where(m => m.PosState == 2 && ids.Contains(m.PosId)).ToListAsync();
            if (posList.Count > 0)
            {
                int memId= posList[0].MemId;
                var memUsers = await _context.MemUsers.Where(m => m.MemId == memId).Select(m => m.MemberClass).FirstAsync();
                if (memUsers == -2)
                {
                    return ("该企业已被禁用", 0);
                }
                int count = 1;
                string PosName = string.Empty;
                string Welfa = string.Empty;
                string PosAddreass = string.Empty;
                var dic = await _dicService.GetSalaryNew();
                foreach (var posInfo in posList)
                {
                    if (!Regex.IsMatch(posInfo.Welfa, @"^[\u4e00-\u9fa5]{1,}$"))
                    {
                        string wa = Goodjob.Common.Dictionary.ArraryWelfa.GetWelfaName(posInfo.Welfa);
                        string[] ws = wa.Split('|');
                        foreach (var witem in ws)
                        {
                            if (!Welfa.Contains(witem))
                            {
                                Welfa += witem + "|";
                            }
                        }
                        if (posInfo.Welfa.Length > 1)
                        {
                            foreach (var item in posInfo.Welfa.Split('|'))
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    if (Convert.ToInt32(item) == 48)
                                    {
                                        Welfa += "大小周|";
                                    }
                                }
                            }
                        }
                    }

                    string sn = "";
                    if (posInfo.PosType == 3)
                    {

                        PosName += count + "、" + posInfo.PosName + "(兼职" + sn + ") &#10;";
                    }
                    else
                    {
                        if (posInfo.SalaryMin != 0)
                        {
                            string salaryMin;
                            string salaryMax;
                            if (posInfo.SalaryMin == posInfo.SalaryMax)
                            {
                                salaryMin= dic.Where(d=>d.Id==posInfo.SalaryMin).Select(d=>d.Name).FirstNonDefaultOrEmpty();
                                sn = salaryMin + "";
                            }
                            else
                            {
                                salaryMin = dic.Where(d => d.Id == posInfo.SalaryMin).Select(d => d.Name).FirstNonDefaultOrEmpty();
                                salaryMax = dic.Where(d => d.Id == posInfo.SalaryMax).Select(d => d.Name).FirstNonDefaultOrEmpty();
                                sn = salaryMin + "-" + salaryMax;
                            }
                        }
                        else if(!string.IsNullOrEmpty(posInfo.SalaryRange))
                        {
                            sn = posInfo.SalaryRange.Replace("月薪", "");
                        }
                        else
                        {
                            string salaryName;
                            if(posInfo.Salary == 20)
                            {
                                salaryName = "面议";
                            }
                            else
                            {
                                salaryName = dic.Where(d => d.Id == posInfo.Salary).Select(d => d.Name).FirstNonDefaultOrEmpty();
                            }
                            sn = salaryName;
                        }
                        PosName += count + "、" + posInfo.PosName + "(月薪" + sn + ") &#10;";
                    }
                    count++;
                }
                if (!string.IsNullOrEmpty(Welfa))
                {
                    Welfa = Welfa.Substring(0, Welfa.Length - 1);
                }

                string url;
                if (posList.Count > 1)
                {
                    url = "http://m.goodjob.cn/m" + memId + "/meminfo.html";
                }
                else
                {
                    url = "http://m.goodjob.cn/m" + memId + "/p" + posList[0].PosId + ".html"; 
                }
                var memInfo=await _context.MemInfos.Where(m=>m.MemId==memId).FirstAsync();
                string phone = "联系电话:{0} &#10;";
                if (string.IsNullOrEmpty(memInfo.Phone) || memInfo.PhoneFlag == false)
                {
                    if (memInfo.TelShowFlag)
                    {
                        string phonestr = "";
                        if (!string.IsNullOrEmpty(memInfo.ContactTelZ))
                        {
                            phonestr = memInfo.ContactTelZ + "-";
                        }
                        if (!string.IsNullOrEmpty(memInfo.ContactTel))
                        {
                            phonestr += memInfo.ContactTel;
                        }
                        if (!string.IsNullOrEmpty(memInfo.ContactTelE))
                        {
                            if (string.IsNullOrEmpty(phonestr))
                            {
                                phonestr = memInfo.ContactTelE;
                            }
                            else
                            {
                                phonestr += "-" + memInfo.ContactTelE;
                            }
                        }

                        phone = string.Format(phone, phonestr);
                    }
                    else
                    {
                        phone = "";
                    }
                }
                else
                {
                    phone = memInfo.PhoneFlag == false ? "" : string.Format(phone, memInfo.Phone);
                }

                if (posList.Count == 1) //只有一个岗位时，地址为公司的地址
                {
                    PosAddreass = memInfo.Address;
                }
                else
                {
                    //多个岗位时，地址取最后一个岗位的面试地址
                    PosAddreass = posList.FindLast(p => p.ExamAddress != "").ExamAddress;
                }
                string str= memInfo.MemName + " &#10;"
                                            + "（急聘）&#10;"
                                            + PosName
                                            + "福利:" + Welfa + " &#10;"
                                            + "地址:" + PosAddreass + " &#10;"
                                            + "联系人:" + memInfo.ContactPerson + " &#10;"
                                            + phone
                                            + "投递简历链接:" + url + "&#10;"
                                            + "【找工作,就来俊才网投简历】点击链接提前注册简历:#小程序://俊才网/2hmbYpF0HDX9hJk" + "&#10;"
                                            + "#公众号：俊才网 &#10;"
                                            + "#小程序：俊才网 &#10;";
                return (str, posList.Count);
            }

            return ("请选择发布中的岗位", 0);
        }

        public async Task<(string str, int count)> CreatePtWxShareStr(int memId)
        {
            var memUsers = await _context.MemUsers.Where(m => m.MemId == memId).Select(m => m.MemberClass).FirstAsync();
            if (memUsers == -2)
            {
                return ("该企业已被禁用", 0);
            }
            var list = await _context.MemPositionPartTimes.Where(m => m.PosState == 2 && m.MemId == memId).ToListAsync();
            if (list.Count > 0)
            {
                string PosName = string.Empty;
                string Welfa = string.Empty;
                int count = 1;
                foreach (var posInfo in list)
                {
                    Welfa = GetWelfaNameWeb(posInfo.Welfa);
                    string sn = "";
                    if (posInfo.PosType == 3)
                    {
                        //+ "(兼职" + sn + ") &#10;"
                        PosName += count + "、" + posInfo.PosName;
                    }
                    else
                    {
                        //+ "(月薪" + sn + ") &#10;"
                        PosName += count + "、" + posInfo.PosName;
                    }
                    count++;
                }

                var memInfo = await _context.MemInfos.Where(m => m.MemId == memId).FirstAsync();
                string phone = "联系电话:{0} &#10;";
                if (string.IsNullOrEmpty(memInfo.Phone) || memInfo.PhoneFlag == false)
                {
                    if (memInfo.TelShowFlag)
                    {
                        string phonestr = "";
                        if (!string.IsNullOrEmpty(memInfo.ContactTelZ))
                        {
                            phonestr = memInfo.ContactTelZ + "-";
                        }
                        if (!string.IsNullOrEmpty(memInfo.ContactTel))
                        {
                            phonestr += memInfo.ContactTel;
                        }
                        if (!string.IsNullOrEmpty(memInfo.ContactTelE))
                        {
                            if (string.IsNullOrEmpty(phonestr))
                            {
                                phonestr = memInfo.ContactTelE;
                            }
                            else
                            {
                                phonestr += "-" + memInfo.ContactTelE;
                            }
                        }

                        phone = string.Format(phone, phonestr);
                    }
                    else
                    {
                        phone = "";
                    }
                }
                else
                {
                    phone = memInfo.PhoneFlag == false ? "" : string.Format(phone, memInfo.Phone);
                }
                string url = "http://m.goodjob.cn/m" + memId + "/meminfo.html?Pos=true";
                if (list.Count == 1)
                {
                    url = "http://m.goodjob.cn/m" + memId + "/pt" + list[0].PosId + ".html";
                }
                string str= memInfo.MemName + " &#10;"
                                             + PosName + " &#10;"
                                             + "福利:" + Welfa + " &#10;"
                                             + "地址:" + memInfo.Address + " &#10;"
                                             + "联系人:" + memInfo.ContactPerson + " &#10;"
                                             + phone
                                             + "投递简历链接:" + url + "&#10;"
                                             + "【找工作,就来俊才网投简历】点击链接提前注册简历:#小程序://俊才网/2hmbYpF0HDX9hJk" + "&#10;"
                                             + "#公众号：俊才网 &#10;"
                                             + "#小程序：俊才网 &#10;";
                return (str, list.Count);
            }
            
            return ("该企业没有发布中的兼职岗位", 0);
        }
        private static string[,] WelfaDict = new string[,] {
            { "包住宿", "1" },
            { "包工作餐", "2" },
            { "交通补助", "3" },
            { "加班补助", "4" },
            { "有提成", "5" },
            { "奖金奖励", "6" },
            { "转正机会", "7" },
        };
        private static string WelfaName(int packageId)
        {
            for (int i = 0; i < WelfaDict.GetLength(0); i++)
            {
                if (packageId == Convert.ToInt32(WelfaDict.GetValue(i, 1)))
                {
                    return WelfaDict[i, 0];
                }

            }
            return string.Empty;
        }
        private static string GetWelfaNameWeb(string val)
        {
            string ab = string.Empty;
            string[] a = val.Split(',');
            foreach (string v in a)
            {
                if (!string.IsNullOrEmpty(v))
                {
                    ab += WelfaName(Convert.ToInt32(v)) + ",";
                }
            }
            if (!string.IsNullOrEmpty(ab))
            {
                ab = ab.Remove(ab.LastIndexOf(','), 1);
            }
            return ab;
        }

        public async Task<string> RefreshPtAll(int memId)
        {
            var posIds = await _context.MemPositionPartTimes.Where(m => m.MemId == memId && m.PosState == 2)
                .Select(m => m.PosId).ToArrayAsync();
            if (posIds.Length != 0)
            {
                string posStr = string.Join(",", posIds);
                string sql = $"UPDATE Mem_PositionPartTime SET ResulatDate= Getdate()  WHERE PosID in ({posStr})";

                var result = await _context.Database.GetDbConnection().ExecuteAsync(sql, null);
                if (result > 1)
                {
                    return "刷新成功";
                }
                return "刷新失败";
                //Mem_PositionPartTime;
            }

            return "该企业没有发布中的兼职岗位";
            throw new NotImplementedException();
        }

        public async Task<(string msg,bool result)> AddSimulationPosition(int[] posIds, int memId)
        {
            //排除不在岗位表的Id
            var id= await _context.MemPositions.Where(m => posIds.Contains(m.PosId)).Select(m => m.PosId).ToArrayAsync();
            if (id.Length == 0)
            {
                return ("本次选中的职位不存在职位表中", false);
            }
            string msg = String.Empty;
            foreach (var i in posIds)
            {
                bool tf=true;
                foreach (var j in id)
                {
                    if (i == j) //等于即存在
                    {
                        tf=false;
                        break;
                    }
                }
                if (tf) //不在岗位表中，移除该Id
                {
                    posIds = posIds.Where(s => s != i).ToArray();
                    if (string.IsNullOrEmpty(msg))
                        msg = "不存在Id:" + i;
                    else
                    {
                        msg += "," + i;
                    }
                }
            }
            string pushDatestr = DateTime.Now.AddDays(1).ToString("d") + " 08:30:00";
            if (DateTime.Now.DayOfWeek.Equals(DayOfWeek.Friday))
            {
                pushDatestr = DateTime.Now.AddDays(3).ToString("d") + " 08:30:00";
            }
            DateTime pushDate = Convert.ToDateTime(pushDatestr);
            //获取已经在推送的职位Id集合
            var ids = await _context.MemResumePushes
                .Where(m => m.InputDate == pushDate && m.MemId == memId && posIds.Contains(m.PosId))
                .Select(m => m.PosId).ToArrayAsync();
            string pushIng=String.Empty;
            foreach (var i in ids)
            {
                foreach (var j in posIds)
                {
                    if (i == j) //去重
                    {
                        posIds = posIds.Where(s => s != i).ToArray();
                        if (string.IsNullOrEmpty(pushIng))
                            pushIng = "已在推送列表中Id:" + i;
                        else
                        {
                            pushIng += "," + i;
                        }
                    }
                }
            }

            foreach (var posId in posIds)
            {
                await _context.MemResumePushes.AddAsync(new MemResumePush()
                {
                    MemId = memId,PosId = posId,InputDate = pushDate,HasPushed = 0,PushMyUserIds = ""
                });
            }
            
            if (!string.IsNullOrEmpty(msg))
                msg += "," + pushIng;
            else
            {
                msg =  pushIng;
            }
            if (await _context.SaveChangesAsync() > 0)
            {
                if (!string.IsNullOrEmpty(msg))
                    msg += "，其余添加成功";
                else
                {
                    msg = "添加成功";
                }

                return (msg, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(msg))
                    msg += ",添加失败";
                else
                {
                    msg = "添加失败";
                }
                return (msg, false);
            }
        }
    }
}
