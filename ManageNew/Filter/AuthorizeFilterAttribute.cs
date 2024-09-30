
using ManageNew.Tool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.enums;
namespace ManageNew.Filter
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Path.StartsWithSegments("/api/Test") || context.HttpContext.Request.Path.StartsWithSegments("/api/auth"))
            {
                return;
            }
            string userIdStr = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value; //用户Id
            if (userIdStr == null) 
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Content = "未授权。",
                    ContentType = "text/plain; charset=utf-8"
                };
                return;
            }
            int.TryParse(userIdStr, out int userId);
            if (!await IsAuthorized(context.HttpContext, userId))
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Content = "权限不足。",
                    ContentType = "text/plain; charset=utf-8"
                };
            }
        }

        private async Task<bool> IsAuthorized(HttpContext httpContext,int userId)
        {
            var checkPermission = httpContext.RequestServices.GetService<CheckPermission>();
            if (checkPermission == null)
                return false;
            //外网简历核对,新注册不完整简历,待删除简历,简历完整度查询，外网导入简历管理，
            if ( httpContext.Request.Path.StartsWithSegments("/api/ExtranetResumeCheck") ||
                 httpContext.Request.Path.StartsWithSegments("/api/NewResume") || 
                 httpContext.Request.Path.StartsWithSegments("/api/ResumeList")|| 
                 httpContext.Request.Path.StartsWithSegments("/api/ResumeComplete") ||
                 httpContext.Request.Path.StartsWithSegments("/api/ExtranetResumeManage")) 
            {
                var result = await checkPermission.CheckPersonPermission(PersonEnum.CheckResume, userId);
                if (!result)
                    return false;
                result = await checkPermission.CheckPersonPermission(PersonEnum.ActiveResume, userId);
                if (!result)
                    return false;
                result = await checkPermission.CheckPersonPermission(PersonEnum.DeleteResume, userId);
                if (!result)
                    return false;
                return true;
            }
            //审核简历
            if (httpContext.Request.Path.StartsWithSegments("/api/CheckResume"))
            {
                var result = await checkPermission.CheckPersonPermission(PersonEnum.CheckResume, Convert.ToInt32(userId));
                return result;
            }
            //简历搜索，外网简历推荐
            if (httpContext.Request.Path.StartsWithSegments("/api/ResumeSearch") || httpContext.Request.Path.StartsWithSegments("/api/ExtranetResumeRecommend"))
            {
                var result = await checkPermission.CheckPersonViewPermission(PersonViewEnum.SearchResume, Convert.ToInt32(userId));
                return result;
            }
            //获取未加密和加密的账号密码,获取账号密码，获取加密后的账户密码
            if (httpContext.Request.Path.StartsWithSegments("/api/ResumeList/GetUserPass") || httpContext.Request.Path.StartsWithSegments("/api/ResumeList/GetUserPassN") || httpContext.Request.Path.StartsWithSegments("/api/ResumeList/GetInfo") || httpContext.Request.Path.StartsWithSegments("/api/NewResume/GetUserPass") || httpContext.Request.Path.StartsWithSegments("/api/NewResume/GetUserPassN") || httpContext.Request.Path.StartsWithSegments("/api/ExtranetResumeCheck/GetUserPass"))
            {
                var result = await checkPermission.CheckPersonViewPermission(PersonViewEnum.ViewPersonUsername, Convert.ToInt32(userId));
                return result;
            }
            //删除简历
            if (httpContext.Request.Path.StartsWithSegments("/api/NewResume/Del")) 
            {
                var result = await checkPermission.CheckPersonPermission(PersonEnum.DeleteResume, Convert.ToInt32(userId));
                return result;
            }
            //查看简历
            if (httpContext.Request.Path.StartsWithSegments("/api/ResumeShareVice/CheckPersonViewPermission") || httpContext.Request.Path.StartsWithSegments("/api/ResumeShareVice/GetResume")) 
            {
                var result = await checkPermission.CheckPersonViewPermission(PersonViewEnum.ViewResume, Convert.ToInt32(userId));
                return result;
            }
            //企业服务信息查看
            if (httpContext.Request.Path.StartsWithSegments("/api/MyCompany/CheckCompanyViewPermission") || httpContext.Request.Path.StartsWithSegments("/api/MyCompany/GetCompanyServiceById"))
            {
                var result = await checkPermission.CheckCompanyViewPermission(CompanyViewEnum.ViewCompanyServiceInfo, Convert.ToInt32(userId));
                return result;
            }
            //企业账户密码查看
            if (httpContext.Request.Path.StartsWithSegments("/api/MyCompany/CheckViewCompanyUsername") || httpContext.Request.Path.StartsWithSegments("/api/MyCompany/GetMemUserNameAndPassWord"))
            {
                var result = await checkPermission.CheckCompanyViewPermission(CompanyViewEnum.ViewCompanyUsername, Convert.ToInt32(userId));
                return result;
            }
            //审核简历
            if (httpContext.Request.Path.StartsWithSegments("/api/CheckResume"))
            {
                var result = await checkPermission.CheckPersonPermission(PersonEnum.CheckResume, Convert.ToInt32(userId));
                return result;
            }
            return true;
        }
    }
}
