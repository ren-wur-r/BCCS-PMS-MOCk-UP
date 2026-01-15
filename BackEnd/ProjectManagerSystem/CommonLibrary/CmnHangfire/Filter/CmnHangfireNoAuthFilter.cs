using Hangfire.Dashboard;

namespace CommonLibrary.CmnHangfire.Filter;

/// <summary>通用-Hangfire-不驗證-過濾</summary>
public class CmnHangfireNoAuthFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        ////DashboardContext.Request 提供 Method、Path、LocalIpAddress、RemoteIpAddress 等基本屬性
        //var clientIp = context.Request.RemoteIpAddress;
        ////不足的話，可以轉成 OwinContext
        //var owinCtx = new OwinContext(context.GetOwinEnvironment());
        //var isLogin = owinCtx.Request.User.Identity.IsAuthenticated;
        //var loginUser = owinCtx.Request.User.Identity.Name.Split('\\').Last();
        ////依據來源IP、登入帳號決定可否存取
        ////例如：已登入者可存取
        //return isLogin;
        return true;
    }
}
