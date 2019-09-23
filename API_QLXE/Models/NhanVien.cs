using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    [Table("QLXE.NHANVIEN")]
    public class NhanVien
    {
        [Key]
        public int NHANVIEN_ID { get; set; }
        public string MA_NV { get; set; }
        public string TEN_NV { get; set; }
        public int GIOITINH { get; set; }
        public string SDT_LH { get; set; }
        public DateTime NGAY_SN { get; set; }
        public int DONVI_ID { get; set; }
        public int TRANG_THAI { get; set; }
        public string GHI_CHU { get; set; }
        public string EMAIL { get; set; }
    }
}