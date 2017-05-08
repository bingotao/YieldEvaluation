using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JXGIS.Common.Entity;

namespace JXGIS.Common.BaseLib
{
    public class OracleEFDbContext : DbContext
    {
        static readonly string _conStr = (string)SystemUtils.Config.OracleDbConStr;
        public OracleEFDbContext() : base(_conStr)
        {
            this.Database.Initialize(false);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string connectionString = _conStr;
            int indexOf = connectionString.IndexOf("USER ID", StringComparison.OrdinalIgnoreCase);
            string str = connectionString.Substring(indexOf);
            int startIndexOf = str.IndexOf("=", StringComparison.OrdinalIgnoreCase);
            int lastIndexOf = str.IndexOf(";", StringComparison.OrdinalIgnoreCase);
            string uid = str.Substring(startIndexOf + 1, lastIndexOf - startIndexOf - 1).Trim().ToUpper();
            modelBuilder.HasDefaultSchema(uid);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<DM_GY_SWJG> DM_GY_SWJG { get; set; }

        public DbSet<DM_GY_HY> DM_GY_HY { get; set; }

        public DbSet<QYGM0> QYGM { get; set; }

        public DbSet<DJ_NSRXX> DJ_NSRXX { get; set; }

        public DbSet<V_QYMCSS> V_QYMCSS { get; set; }
        public DbSet<SYS_PLOCK> SYS_PLOCK { get; set; }

        public DbSet<SJJGLS> SJJGLS { get; set; }
    }
}
