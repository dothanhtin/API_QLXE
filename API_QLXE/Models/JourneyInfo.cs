using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    public class JourneyInfo
    {
        public virtual int id { get; set; }
        public virtual string xe { get; set; }
        public virtual string tai_xe { get; set; }
        public virtual DateTime? ngay_dk { get; set; }
        public virtual string muc_dich { get; set; }
        public virtual string noi_di { get; set; }
        public virtual string noi_den { get; set; }
        public virtual DateTime? tg_di { get; set; }
        public virtual DateTime? tg_ve { get; set; }
        public virtual string gio_xuat_phat { get; set; }
        public virtual string km_di { get; set; }
        public virtual string km_ve { get; set; }
        public virtual string tg_cau { get; set; }
        public virtual string so_nguoi { get; set; }
        public virtual string user_dk { get; set; }
        public virtual string user_duyet { get; set; }
        public virtual string trang_thai { get; set; }
        public virtual string loai_ht { get; set; }
        public virtual DateTime? ngay_in { get; set; }
        public virtual DateTime? ngay_duyet { get; set; }
        public virtual DateTime? ngay_htra { get; set; }
        public virtual DateTime? ngay_ht { get; set; }
        public virtual string ly_do { get; set; }
        public virtual string ngay_dk1 { get; set; }
        public virtual string tg_di1 { get; set; }
        public virtual string tg_ve1 { get; set; }
        public virtual string ten_ttht { get; set; }
        public virtual string ten_dk { get; set; }
        public virtual string ten_nd { get; set; }
        public virtual string so_xe { get; set; }
        public virtual string kmdi1 { get; set; }
        public virtual string ten_tx { get; set; }
        public virtual string sdt_tx { get; set; }
        public virtual string donvi_id { get; set; }
        public virtual string ten_donvi_con { get; set; }
        public virtual string ten_loai_ht { get; set; }
        public virtual string ngay_in1 { get; set; }
    }
}