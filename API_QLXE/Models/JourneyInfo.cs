using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    public class JourneyInfo
    {
        public virtual int ID { get; set; }
        public virtual string XE { get; set; }
        public virtual string TAI_XE { get; set; }
        public virtual DateTime? NGAY_DK { get; set; }
        public virtual string MUC_DICH { get; set; }
        public virtual string NOI_DI { get; set; }
        public virtual string NOI_DEN { get; set; }
        public virtual DateTime? TG_DI { get; set; }
        public virtual DateTime? TG_VE { get; set; }
        public virtual string GIO_XUAT_PHAT { get; set; }
        public virtual string KM_DI { get; set; }
        public virtual string KM_VE { get; set; }
        public virtual string TG_CAU { get; set; }
        public virtual string SO_NGUOI { get; set; }
        public virtual string USER_DK { get; set; }
        public virtual string USER_DUYET { get; set; }
        public virtual string TRANG_THAI { get; set; }
        public virtual string LOAI_HT { get; set; }
        public virtual DateTime? NGAY_IN { get; set; }
        public virtual DateTime? NGAY_DUYET { get; set; }
        public virtual DateTime? NGAY_HTRA { get; set; }
        public virtual DateTime? NGAY_HT { get; set; }
        public virtual string LY_DO { get; set; }
        public virtual string NGAY_DK1 { get; set; }
        public virtual string TG_DI1 { get; set; }
        public virtual string TG_VE1 { get; set; }
        public virtual string TEN_TTHT { get; set; }
        public virtual string TEN_DK { get; set; }
        public virtual string TEN_ND { get; set; }
        public virtual string SO_XE { get; set; }
        public virtual string KMDI1 { get; set; }
        public virtual string TEN_TX { get; set; }
        public virtual string DONVI_ID { get; set; }
        public virtual string TEN_DONVI_CON { get; set; }
        public virtual string TEN_LOAI_HT { get; set; }
    }
}