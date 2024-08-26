using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Model;

namespace ManageNew.ExceptionFilter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            // 创建一个错误响应
            var problemDetails = new ProblemDetails
            {
                Title = "请求发生错误",
                Status = StatusCodes.Status500InternalServerError,
                Detail = context.Exception.Message,
               // Instance = context.Exception.StackTrace
            };
            // 设置结果
            context.Result = new ObjectResult(ResultMode<string>.Failed500(context.Exception.Message))
            {
                StatusCode = 500
            };

            // 标记异常已经处理，以避免后续的过滤器处理
            context.ExceptionHandled = true;
        }
    }
}
