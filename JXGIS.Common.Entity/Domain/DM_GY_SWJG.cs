using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JXGIS.Common.Entity
{
    [Table("HX_DM_ZDY.DM_GY_SWJG")]
    public class DM_GY_SWJG
    {
        [Key, Column("SWJG_DM")]
        public string SWJG_DM { get; set; }
        [Column("SWJGMC")]
        public string SWJGMC { get; set; }
        [Column("SWJGJC")]
        public string SWJGJC { get; set; }
        [Column("SWJGBZ")]
        public string SWJGBZ { get; set; }
        [Column("JGJC_DM")]
        public string JGJC_DM { get; set; }
        [Column("SWJGYZBM")]
        public string SWJGYZBM { get; set; }
        [Column("SWJGDZ")]
        public string SWJGDZ { get; set; }
        [Column("SWJGLXDH")]
        public string SWJGLXDH { get; set; }
        [Column("CZDH")]
        public string CZDH { get; set; }
        [Column("DZXX")]
        public string DZXX { get; set; }
        [Column("XZQHSZ_DM")]
        public string XZQHSZ_DM { get; set; }
        [Column("SWJGFZR_DM")]
        public string SWJGFZR_DM { get; set; }
        [Column("GDSLX_DM")]
        public string GDSLX_DM { get; set; }
        [Column("XYBZ")]
        public string XYBZ { get; set; }
        [Column("YXBZ")]
        public string YXBZ { get; set; }
        [Column("YXQSRQ")]
        public DateTime YXQSRQ { get; set; }
        [Column("YXZZRQ")]
        public DateTime YXZZRQ { get; set; }
        [Column("SWJGJG")]
        public string SWJGJG { get; set; }
        [Column("BSFWTBZ")]
        public string BSFWTBZ { get; set; }
        [Column("GHBZ")]
        public string GHBZ { get; set; }
        [Column("XSXH")]
        public int XSXH { get; set; }
        [Column("GJSWJGMC")]
        public string GJSWJGMC { get; set; }
        [Column("DSSWJGMC")]
        public string DSSWJGMC { get; set; }
        [Column("GSSWJGJG")]
        public string GSSWJGJG { get; set; }
        [Column("DSSWJGJG")]
        public string DSSWJGJG { get; set; }
        [Column("ZN_DM")]
        public string ZN_DM { get; set; }
        [Column("SWJGYWMC")]
        public string SWJGYWMC { get; set; }
    }
}
