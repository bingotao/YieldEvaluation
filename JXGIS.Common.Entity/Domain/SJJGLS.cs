using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.Entity
{
    [Table("SJJGLS")]
    public class SJJGLS
    {
        [Key]
        [Column("LOGID")]
        public int LOGID { get; set; }

        [Column("LOGTYPE")]
        public string LOGTYPE { get; set; }

        [Column("LOGCONTENT")]
        public string LOGCONTENT { get; set; }

        [Column("LOGDATE")]
        public DateTime LOGDATE { get; set; }
    }
}
