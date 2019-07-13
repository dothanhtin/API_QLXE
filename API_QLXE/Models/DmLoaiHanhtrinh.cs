using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace API_QLXE.Models
{
    [Table("QLXE.DM_LOAI_HANHTRINH")]
    public class DmLoaiHanhtrinh
    {
        public virtual string TEN_LOAI_HT { get; set; }
        public virtual short ID{ get; set; }
    }
}