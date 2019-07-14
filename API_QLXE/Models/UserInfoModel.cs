using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    public class UserInfoModel
    {
        public string USERNAME { get; set; }
        public string PASS { get; set; }
        public string PASSFINAL { get; set; }
        public string NHANVIEN_ID { get; set; }
        public DateTime? NGAY_LOGIN { get; set; }
        public int? TRANG_THAI { get; set; }
        public string GHI_CHU { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string TEN_NV { get; set; }
        public string SDT_LH { get; set; }
        public int DONVI_ID { get; set; }
        public int? DONVI_CHA_ID { get; set; }
        public string TEN_DV_CHA { get; set; }
    }
}