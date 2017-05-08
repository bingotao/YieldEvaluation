using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.Common.Entity
{
    [Table("SYS_PLOCK")]
    public class SYS_PLOCK
    {
        [Key]
        [Column("PNAME")]
        public string PNAME { get; set; }

        [Column("LOCKSTATE")]
        public int LOCKSTATE { get; set; }

        [Column("MEMO")]
        public string MEMO { get; set; }
    }
}
