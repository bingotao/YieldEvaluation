using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.Entity
{
    [Table("V_QYMCSS")]
    public class V_QYMCSS
    {
        private static PropertyInfo[] props = typeof(V_QYMCSS).GetProperties();

        [Key]
        public long PK { get; set; }

        /// <summary>
        /// 入库年度
        /// </summary>

        [Column("RKND")]
        public string RKND { get; set; }

        /// <summary>
        /// 纳税人识别号
        /// </summary>
        [Column("NSRSBH")]
        public string NSRSBH { get; set; }

        [Column("RN")]
        public int? RN { get; set; }

        /// <summary>
        /// 纳税人名称
        /// </summary>
        [Column("NSRMC")]
        public string NSRMC { get; set; }

        /// <summary>
        /// 所在片区
        /// </summary>
        [Column("SZPQ")]
        public string SZPQ { get; set; }

        /// <summary>
        /// 所在片区代码
        /// </summary>
        [Column("SZPQ_DM")]
        public string SZPQ_DM { get; set; }

        /// <summary>
        /// 税务部门名称
        /// </summary>
        [Column("SWBMMC")]
        public string SWBMMC { get; set; }

        /// <summary>
        /// 税务机构代码
        /// </summary>
        [Column("SWJG_DM")]
        public string SWJG_DM { get; set; }

        /// <summary>
        /// 行业名称
        /// </summary>
        [Column("HYMC")]
        public string HYMC { get; set; }

        /// <summary>
        /// 行业代码
        /// </summary>
        [Column("HY_DM")]
        public string HY_DM { get; set; }

        /// <summary>
        /// 大行业
        /// </summary>
        [Column("DHY")]
        public string DHY { get; set; }

        /// <summary>
        /// 社会信用代码
        /// </summary>
        [Column("SHXYDM")]
        public string SHXYDM { get; set; }



        /// <summary>
        /// 国税企业所得税
        /// </summary>
        [Column("GS_QYSDS")]
        public double? GS_QYSDS { get; set; }

        /// <summary>
        /// 国税增值税
        /// </summary>
        [Column("GS_ZZS")]
        public double? GS_ZZS { get; set; }

        /// <summary>
        /// 免抵调
        /// </summary>
        [Column("MDT")]
        public double? MDT { get; set; }

        /// <summary>
        /// 土地使用税
        /// </summary>
        [Column("TDSYS")]
        public double? TDSYS { get; set; }

        /// <summary>
        /// 房产税
        /// </summary>
        [Column("FCS")]
        public double? FCS { get; set; }

        /// <summary>
        /// 营业税
        /// </summary>
        [Column("YYS")]
        public double? YYS { get; set; }


        /// <summary>
        /// 企业所得税
        /// </summary>
        [Column("QYSDS")]
        public double? QYSDS { get; set; }

        /// <summary>
        /// 城建税
        /// </summary>
        [Column("CJS")]
        public double? CJS { get; set; }

        /// <summary>
        /// 印花税
        /// </summary>
        [Column("YHS")]
        public double? YHS { get; set; }

        /// <summary>
        /// 土地增值税
        /// </summary>
        [Column("TDZZS")]
        public double? TDZZS { get; set; }

        /// <summary>
        /// 教育费附加
        /// </summary>
        [Column("JYFFJ")]
        public double? JYFFJ { get; set; }

        /// <summary>
        /// 地方教育费附加
        /// </summary>
        [Column("DFJYFFJ")]
        public double? DFJYFFJ { get; set; }

        /// <summary>
        /// 水利建设基金
        /// </summary>
        [Column("SLJSJJ")]
        public double? SLJSJJ { get; set; }

        /// <summary>
        /// 文化事业费
        /// </summary>
        [Column("WHSYF")]
        public double? WHSYF { get; set; }

        /// <summary>
        /// 主营业务收入
        /// </summary>
        [Column("ZYYWSR")]
        public double? ZYYWSR { get; set; }

        /// <summary>
        /// 增值税
        /// </summary>
        [Column("ZZS")]
        public double? ZZS { get; set; }

        /// <summary>
        /// 亩产税收
        /// </summary>
        [Column("MCSS")]
        public double? MCSS { get; set; }

        /// <summary>
        /// 应纳税收总额
        /// </summary>
        [Column("YNSSZE")]
        public double? YNSSZE { get; set; }

        /// <summary>
        /// 税收总额
        /// </summary>
        [Column("SSZE")]
        public double? SSZE { get; set; }

        /// <summary>
        /// 自用土地面积
        /// </summary>
        [Column("ZYTDMJ")]
        public double? ZYTDMJ { get; set; }

        /// <summary>
        /// 出租土地面积
        /// </summary>
        [Column("CUZTDMJ")]
        public double? CUZTDMJ { get; set; }

        /// <summary>
        /// 承租土地面积
        /// </summary>
        [Column("CENGZTDMJ")]
        public double? CENGZTDMJ { get; set; }

        [NotMapped]
        public double MCJZB { get; set; }

        [NotMapped]
        public double MCJZ
        {
            get; set;
        }

        public object this[string key]
        {
            get
            {
                var prop = props.Where(p => p.Name == key).FirstOrDefault();
                return prop != null ? prop.GetValue(this) : null;
            }
        }
    }
}
