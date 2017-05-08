using JXGIS.Common.BaseLib;
using JXGIS.YieldEvaluation.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JXGIS.YieldEvaluation.Web.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult Login(string userName, string password)
        {
            ReturnObject ro = null;
            try
            {
                var success = LoginUtils.ValidateUser(userName, password);
                ro = new ReturnObject(success);
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }

            string rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            return Content(rs);
        }
    }
}