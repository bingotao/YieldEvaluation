using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.Entity
{
    public class Error
    {
        public int Index { get; set; }

        public string Title { get; set; }
        public QYGM0 Company { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }

}