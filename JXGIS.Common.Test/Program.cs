using JXGIS.Common.BaseLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Data.SqlClient;
using JXGIS.Common.Entity;
using System.Web;
using System.Data.OleDb;
using Aspose.Cells;
using JXGIS.YieldEvaluation.Business;
using Oracle.ManagedDataAccess.Client;

namespace JXGIS.Common.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            //var DM_GY_HY = SystemUtils.OracleEFDbContext.DM_GY_HY.Where(p => string.IsNullOrEmpty(p.SJHY_DM)).ToList();
            //var DM_GY_SWJG = SystemUtils.OracleEFDbContext.DM_GY_SWJG.ToList();

            //var list = SystemUtils.OracleEFDbContext.V_QYMCSS.ToList();
            //var list2 = SystemUtils.OracleEFDbContext.DJ_NSRXX.Take(10).ToList();

            //list[1].NSRSBH = "hehe";

            using (OracleConnection con = new OracleConnection(SystemUtils.Config.OracleDbConStr.ToString()))
            {
                //exec SJJG_MCSS_DH(2016, '91330411146486234X');

                con.Open();

                OracleCommand com = new OracleCommand("SJJG_MCSS_DH", con);
                com.CommandType = CommandType.StoredProcedure;

                var nyear = new OracleParameter("nyear", OracleDbType.Int32);
                nyear.Direction = ParameterDirection.Input;
                nyear.Value = 2016;
                var qyid = new OracleParameter("qyid", OracleDbType.Varchar2);
                qyid.Value = "91330411146486234X";
                qyid.Direction = ParameterDirection.Input;
                com.Parameters.Add(nyear);
                com.Parameters.Add(qyid);

                var x = com.ExecuteNonQuery();
                con.Close();
            }



        }
    }
}