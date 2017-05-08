using JXGIS.Common.BaseLib;
using JXGIS.YieldEvaluation.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JXGIS.YieldEvaluation.Web.Attributes
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 重写自定义授权检查
        /// </summary>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return LoginUtils.ValidateUser();
        }

        /// <summary>
        /// 重写未授权的 HTTP 请求处理
        /// </summary>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var jsonResult = new JsonResult();
            jsonResult.Data = new ReturnObject("未授权，请先登录！");
            filterContext.Result = jsonResult;
        }
    }
}