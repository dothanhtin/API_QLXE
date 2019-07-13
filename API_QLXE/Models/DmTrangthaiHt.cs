using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    [Table("QLXE.DM_TRANGTHAI_HT")]
    public class DmTrangthaiHt
    {
        public virtual short ID { get; set; }
        public virtual string TEN_TTHT { get; set; }
        public virtual string GHI_CHU { get; set; }
    }
}