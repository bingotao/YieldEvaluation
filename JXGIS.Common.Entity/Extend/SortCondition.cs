using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.Entity
{
    public class SortCondition
    {
        public List<string> HYDM { get; set; }

        public int SSND { get; set; }

        public double MJ_Min { get; set; }

        public double MJ_Max { get; set; }

        public string ZGSWJ_DM { get; set; }

        public int QYGM { get; set; }

        public string SZPQ { get; set; }
    }
}
