using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JXGIS.Common.Entity
{
    [Table("HX_DM_QG.DM_GY_HY")]
    public class DM_GY_HY
    {
        [Key, Column("HY_DM")]
        public string HY_DM { get; set; }

        [Column("HYMC")]
        public string HYMC { get; set; }

        [Column("MLBZ")]
        public string MLBZ { get; set; }

        [Column("DLBZ")]
        public string DLBZ { get; set; }

        [Column("ZLBZ")]
        public string ZLBZ { get; set; }

        [Column("XLBZ")]
        public string XLBZ { get; set; }

        [Column("SJHY_DM")]
        public string SJHY_DM { get; set; }

        [Column("XYBZ")]
        public string XYBZ { get; set; }

        [Column("YXBZ")]
        public string YXBZ { get; set; }

        [ForeignKey("SJHY_DM")]
        public virtual List<DM_GY_HY> Children { get; set; }

        [NotMapped]
        public bool Checked { get; set; } = false;
    }
}
