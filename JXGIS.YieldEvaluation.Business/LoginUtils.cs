using JXGIS.Common.BaseLib;
using JXGIS.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.YieldEvaluation.Business
{
    public class LoginUtils
    {
        private static string _User = "User";
        public static bool ValidateUser(string userName, string password)
        {
            if (ValidateUser()) return true;
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return false;
            var bSuccess = false;
            List<User> users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(SystemUtils.Config.Users.ToString());
            var user = users.Where(u => u.UserName == userName && u.Password == password).FirstOrDefault();
            bSuccess = user == null ? false : true;
            if (bSuccess) RegisterUser(user);
            return bSuccess;
        }

        private static void RegisterUser(User user)
        {
            System.Web.HttpContext.Current.Session.Add(_User, user);
        }

        public static bool ValidateUser()
        {
#if DEBUG
            return true;
#endif
            return GetUser() != null;
        }

        public static User GetUser()
        {
            return System.Web.HttpContext.Current.Session[_User] as User;
        }
    }
}
