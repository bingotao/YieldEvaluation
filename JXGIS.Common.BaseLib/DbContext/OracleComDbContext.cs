using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.BaseLib
{
    public class OracleComDbContext
    {
        private OracleDataAdapter _DBAdapter = null;
        private string _connectionString = null;
        private OracleConnection _connection = null;
        public OracleComDbContext()
        {
            this._connectionString = SystemUtils.Config.OracleDbConStr;
            this._DBAdapter = new OracleDataAdapter();
            this._connection = new OracleConnection(this._connectionString);

        }

        public OracleComDbContext(string sConnectionString)
        {
            this._connectionString = sConnectionString;
            this._DBAdapter = new OracleDataAdapter();
            this._connection = new OracleConnection(this._connectionString);
        }

        public DataTable ExecuteQuery(string selectSQL, params OracleParameter[] OracleParameters)
        {
            OracleCommand cmd = new OracleCommand(selectSQL, this._connection);
            cmd.Parameters.AddRange(OracleParameters);
            this._DBAdapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            this._DBAdapter.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 获取分页的DataTable
        /// </summary>
        /// <param name="selectSQL"></param>
        /// <param name="startRecordNum">从第几条数据开始取 从1开始编号</param>
        /// <param name="recordCount">获取的行数</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string selectSQL, int startRecordNum, int recordCount, params OracleParameter[] OracleParameters)
        {
            OracleCommand cmd = new OracleCommand(selectSQL, this._connection);
            cmd.Parameters.AddRange(OracleParameters);
            this._DBAdapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            this._DBAdapter.Fill(startRecordNum - 1, recordCount, dt);
            return dt;
        }

        public static OracleComDbContext Context
        {
            get { return new OracleComDbContext(); }
        }
    }
}
