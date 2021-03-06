﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.BaseLib
{

    public class SQLEFDbContext : DbContext
    {
        public SQLEFDbContext() : base((string)SystemUtils.Config.SQLDbConStr)
        {
            this.Database.Initialize(false);
        }

        //重新OnModelCreating方法
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
        }

        
    }
}