using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.Entity
{
    public class SortResult
    {
        public List<V_QYMCSS> rows { get; set; }

        public List<V_QYMCSS> footer { get; set; }

        public int total { get; set; }
    }
}
