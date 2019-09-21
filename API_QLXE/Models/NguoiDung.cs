using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    [Table("QLXE.NGUOIDUNG")]
    public class NguoiDung
    {
        public string USERNAME { get; set; }
        public string PASS { get; set; }
        public string PASSFINAL { get; set; }
        [Key]
        public int NHANVIEN_ID { get; set; }
        public DateTime? NGAY_LOGIN { get; set; }
        public int? TRANG_THAI { get; set; }
        public string GHI_CHU { get; set; }
        public DateTime? CREATE_DATE { get; set; }
    }
}