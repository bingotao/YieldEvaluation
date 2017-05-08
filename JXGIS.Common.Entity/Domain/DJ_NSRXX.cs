using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.Entity
{
    [Table("HX_DJ.DJ_NSRXX")]
    public class DJ_NSRXX
    {
        [Key]
        [Column("DJXH")]
        public double DJXH { get; set; }

        [Column("NSRSBH")]
        public string NSRSBH { get; set; }

        [Column("ZGSWJ_DM")]
        public string ZGSWJ_DM { get; set; }

        [Column("SHXYDM")]
        public string SHXYDM { get; set; }
    }
}