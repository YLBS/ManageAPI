namespace ManageNew.Extensions
{
    /// <summary>
    /// 出错路由配置
    /// </summary>
    public class ErrorRouteConfig
    {
        private readonly IApplicationBuilder _app;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="app"></param>
        public ErrorRouteConfig(IApplicationBuilder app)
        {
            _app = app;
        }
        /// <summary>
        /// 错误页
        /// </summary>
        public void ConfigureErrorRoutes()
        {
            _app.Map("/error-401", Handle401);
            _app.Map("/error-403", Handle403);
            _app.Map("/error-404", Handle404);
            _app.Map("/error-405", Handle405);
            _app.Map("/error-500", Handle500);

        }
        private void Handle401(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("未授权.");
            });
        }
        private void Handle403(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("权限不足.");
            });
        }
        private void Handle404(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("没找着，路径错误.");
            });
        }

        private void Handle405(IApplicationBuilder app)
        {
            app.Run(async context =>
            {

                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                await context.Response.WriteAsync($"请求方式错误，请用{context.Response.Headers.Allow}");
            });
        }
        private void Handle500(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("代码出错啦!");
            });
        }
    }
}
