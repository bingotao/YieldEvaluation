using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.Entity
{
    [Table("QYGM")]
    public class QYGM0
    {
        [Key]
        [Column("NSRSBH", Order = 0)]
        public string NSRSBH { get; set; }

        [Column("NSRMC")]
        public string NSRMC { get; set; }

        [Column("SHXYDM")]
        public string SHXYDM { get; set; }

        [Key]
        [Column("ZGSWJ_DM", Order = 1)]
        public string ZGSWJ_DM { get; set; }

        [Key]
        [Column("SSND", Order = 2)]
        public int SSND { get; set; }

        [Column("QYGM")]
        public int QYGM { get; set; } = 0;

        [NotMapped]
        public string QYGMMC
        {
            get
            {
                switch (this.QYGM)
                {
                    case 1:
                        return "规上";
                    case 2:
                        return "规下";
                    default:
                        return "未确定";
                }
            }
        }

        [NotMapped]
        public int INDEX { get; set; }
    }
}
