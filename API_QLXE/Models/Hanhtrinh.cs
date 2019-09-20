using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    [Table("QLXE.HANHTRINH")]
    public class Hanhtrinh
    {
        public virtual int ID { get; set; }
        public virtual short? XE { get; set; }
        public virtual short? TAI_XE { get; set; }
        public virtual DateTime? NGAY_DK { get; set; }
        public virtual string MUC_DICH { get; set; }
        public virtual string NOI_DI { get; set; }
        public virtual string NOI_DEN { get; set; }
        public virtual DateTime? TG_DI { get; set; }
        public virtual DateTime? TG_VE { get; set; }
        public virtual string GIO_XUAT_PHAT { get; set; }
        public virtual long? KM_DI { get; set; }
        public virtual decimal? KM_VE { get; set; }
        public virtual long? TG_CAU { get; set; }
        public virtual short? SO_NGUOI { get; set; }
        public virtual string USER_DK { get; set; }
        public virtual string USER_DUYET { get; set; }
        public virtual short? TRANG_THAI { get; set; }
        public virtual short? LOAI_HT { get; set; }
        public virtual DateTime? NGAY_IN { get; set; }
    }
}