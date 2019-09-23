using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    [Table("QLXE.QUANLY_XE")]
    public class QuanLyXe
    {
        public int ID { get; set; }
        public int XE { get; set; }
        public int TAI_XE { get; set; }
        public DateTime TU_NGAY { get; set; }
        public DateTime? DEN_NGAY { get; set; }
        public int TRANG_THAI { get; set; }
        public string GHI_CHU { get; set; }
    }
}