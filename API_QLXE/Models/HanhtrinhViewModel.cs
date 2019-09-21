using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    public class HanhtrinhViewModel
    {
        public virtual int id { get; set; }
        //public virtual short? xe { get; set; }
        //public virtual short? taixe { get; set; }
        public virtual DateTime? thoigiandk { get; set; }
        public virtual string mucdich { get; set; }
        public virtual string noidi { get; set; }
        public virtual string noiden { get; set; }
        public virtual DateTime? thoigiandi { get; set; }
        public virtual DateTime? thoigianve { get; set; }
        public virtual string thoigianxp { get; set; }
        //public virtual long? kmdi { get; set; }
        //public virtual decimal? kmve { get; set; }
        //public virtual long? thoigiancau { get; set; }
        public virtual short? slnguoi { get; set; }
        //public virtual string userdk { get; set; }
        //public virtual string userduyet { get; set; }
        public virtual short? trangthaicode { get; set; }
        public virtual string trangthai { get; set; }
        public virtual string loaihanhtrinh { get; set; }
        //public virtual DateTime? ngayin { get; set; }
    }
}