using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    public class UserInfoModel
    {
        public string username { get; set; }
        //public string PASS { get; set; }
        //public string PASSFINAL { get; set; }
        public string nhanvien_id { get; set; }
        public DateTime? ngay_login { get; set; }
        public int? trang_thai { get; set; }
        public string ghi_chu { get; set; }
        public DateTime? create_date { get; set; }
        public string ten_nv { get; set; }
        public string sdt_lh { get; set; }
        public int donvi_id { get; set; }
        public int? donvi_cha_id { get; set; }
        public string ten_dv_cha { get; set; }
    }
}