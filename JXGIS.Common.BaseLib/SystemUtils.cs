using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JXGIS.Common.BaseLib
{
    public static class SystemUtils
    {
        private static readonly object _lockConfig = new object();
        private static readonly object _lockSQLEFDbContext = new object();
        private static readonly object _lockMySQLEFDbContext = new object();
        private static readonly object _lockSQLComContext = new object();
        private static readonly object _lockMySQLComContext = new object();
        private static readonly object _lockOracleEFDbContext = new object();
        private static readonly object _lockOracleComContext = new object();

        private static dynamic _Config;
        private static SQLEFDbContext _SQLEFDbContext;
        private static SQLComDbContext _SQLComDbContext;
        private static OracleComDbContext _OracleComDbContext;
        private static OracleEFDbContext _OracleEFDbContext;

        public static string BaseUrl
        {
            get
            {
                string appPath = HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.ApplicationPath;
                return appPath == "/" ? string.Empty : appPath;
            }
        }

        private static string configPath
        {
            get
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["SysParFilePath"];
                return AppDomain.CurrentDomain.BaseDirectory + (string.IsNullOrEmpty(path) ? "Config\\SystemParameters.json" : path);
            }
        }

        public static dynamic Config
        {
            get
            {
                if (_Config == null)
                {
                    lock (_lockConfig)
                    {
                        using (StreamReader sr = new StreamReader(configPath))
                        {
                            string json = sr.ReadToEnd();
                            _Config = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        }
                    }
                }
                return _Config;
            }
        }
        public static SQLEFDbContext SQLEFDbContext
        {
            get
            {
                if (SystemUtils._SQLEFDbContext == null)
                    lock (_lockSQLEFDbContext)
                        SystemUtils._SQLEFDbContext = new SQLEFDbContext();
                return SystemUtils._SQLEFDbContext;
            }
        }


        public static SQLComDbContext SQLComDbContext
        {
            get
            {
                if (SystemUtils._SQLComDbContext == null)
                    lock (_lockSQLComContext)
                        SystemUtils._SQLComDbContext = new SQLComDbContext();
                return SystemUtils._SQLComDbContext;
            }
        }



        public static OracleComDbContext OracleComDbContext
        {
            get
            {
                if (SystemUtils._OracleComDbContext == null)
                    lock (_lockOracleComContext)
                        SystemUtils._OracleComDbContext = new OracleComDbContext();
                return SystemUtils._OracleComDbContext;
            }
        }

        public static OracleEFDbContext OracleEFDbContext
        {
            get
            {
                if (SystemUtils._OracleEFDbContext == null)
                    lock (_lockOracleEFDbContext)
                        SystemUtils._OracleEFDbContext = new OracleEFDbContext();
                return SystemUtils._OracleEFDbContext;
            }
        }
    }
}