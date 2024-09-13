using Commons.Tool;
using IService.ResumeSearch;
using ManageNew.Tool;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Common;
using Model.enums;
namespace ManageNew.Controllers.ResumeSearch
{
    /// <summary>
    /// 外网简历核对
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "简历搜索")]
    public class ExtranetResumeCheckController : ControllerBase
    {
        private readonly IExtranetResumeCheck _check;
        private readonly CheckPermission _checkPermission;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExtranetResumeCheckController(IExtranetResumeCheck check, CheckPermission checkPermission)
        {
            _check = check;
            _checkPermission = checkPermission;
        }
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckPersonPermission()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var result = await _checkPermission.CheckPersonPermission(PersonEnum.CheckResume, Convert.ToInt32(userId));
            if(!result)
                return Ok(ResultMode<bool>.Success(result));
            result = await _checkPermission.CheckPersonPermission(PersonEnum.ActiveResume, Convert.ToInt32(userId));
            if (!result)
                return Ok(ResultMode<bool>.Success(result));
            result = await _checkPermission.CheckPersonPermission(PersonEnum.DeleteResume, Convert.ToInt32(userId));
            return Ok(ResultMode<bool>.Success(result));

        }


        /// <summary>
        /// ExtranetResumeCheck.aspx 外网简历核对表,獲取數據
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(string? myUserId = "", string? name = "", string? self = "")
        {
            string sql = "  select top 100  a.[MyUserID] FROM [dbo].[My_Resume] a where 1 =1 ";
            bool yn = true;
            if (!string.IsNullOrEmpty(self))
            {
                yn = false;
                sql += " and a.SelfDescription like '%" + self + "%' ";
            }
            if (!string.IsNullOrEmpty(name))
            {
                yn = false;
                sql += " and a.PerName like '%" + name + "%' ";
            }
            if (!string.IsNullOrEmpty(myUserId))
            {
                yn = false;
                sql = " select top 100  a.[MyUserID] FROM [dbo].[My_Resume] a,[dbo].[My_RegisterForCrm] b where a.MyUserID =b.MyUserID and b.ExtranetID = '" + myUserId + "'";
            }
            if (yn)
            {
                return Ok(ResultMode<string>.Failed("请选择参数"));

            }

            string order = "order by UpdateDate desc ";
            var result = await _check.CreateNewsResumeListCollection(sql, order);
            return Ok(ResultMode<object>.Success(result));
        }
        /// <summary>
        /// 刷新外网简历
        /// </summary>
        /// <param name="myUserIds"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RefreshResume(string myUserIds)
        {
            bool yn = await _check.RefreshResume(myUserIds);
            return Ok(ResultMode<bool>.Success(yn));
        }
        /// <summary>
        /// 将简历设为公开
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateResumeStatus(int myUserId)
        {
            short status = 4;
            bool yn = await _check.UpdateResumeStatus(myUserId, status, string.Empty, 0);
            return Ok(ResultMode<bool>.Success(yn));
        }

        /// <summary>
        /// 获取工作经历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetWorkText(int myUserId)
        {
            string str = await _check.GetWorkText(myUserId);
            return Ok(ResultMode<string>.Success(str));
        }

        /// <summary>
        /// 返回账号密码
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserPass(int myUserId)
        {
            var str = await _check.GetUserPass(myUserId);
            if (str.Key != null)
                str.Key = CompanyTool.EncryptDESTool(str.Key.ToString());
            if (str.Value != null)
                str.Value = CompanyTool.EncryptDESTool(str.Value.ToString());
            //return Ok(ResultMode<object>.Success(str));
            return Ok(ResultMode<string>.Success(str.Key.ToString() +","+ str.Value.ToString()));
        }

        /// <summary>
        /// 修改工作经历
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateWorkText([FromForm] KeyValue ky)
        {
            bool yn = await _check.UpdateWorkText(ky.Id, ky.Name);
            return Ok(ResultMode<bool>.Success(yn));
        }
    }
}
