using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    [Table("QLXE.XE")]
    public class Xe
    {
        public int ID { get; set; }
        public string SO_XE { get; set; }
        public string TEN_XE { get; set; }
        public string LOAI_XE { get; set; }
        public int LOAI_NL { get; set; }
        public int NAM_SD { get; set; }
        public int XY_LANH { get; set; }
        public int DMNL_CHUAN { get; set; }
        public int DMNL_TT { get; set; }
        public int DM_THAYNHOT { get; set; }
        public int HAN_DK { get; set; }
        public DateTime NGAY_DK { get; set; }
        public int DONVI_CHA_ID { get; set; }
        public int DM_CAU { get; set; }
        public int TRANG_THAI { get; set; }
        public int KM { get; set; }
        public int CREATE_DATE { get; set; }
        public float TON_QTNL { get; set; }
        public int? KM_QT { get; set; }

        //public Xe()
        //{
        //    this.DMNL_TT = 0;
        //    this.DM_CAU = 0;
        //    this.TRANG_THAI = 0;
        //    this.KM = 0;
        //    this.TON_QTNL = 0;
        //}
    }

    public class xe
    {
        public int id { get; set; }
        public string so_xe { get; set; }
        public string ten_xe { get; set; }
        public string loai_xe { get; set; }
        public int loai_nl { get; set; }
        public int nam_sd { get; set; }
        public int xy_lanh { get; set; }
        public int dmnl_chuan { get; set; }
        public int dmnl_tt { get; set; }
        public int dm_thaynhot { get; set; }
        public int han_dk { get; set; }
        public DateTime ngay_dk { get; set; }
        public int donvi_cha_id { get; set; }
        public int dm_cau { get; set; }
        public int trang_thai { get; set; }
        public int km { get; set; }
        public int create_date { get; set; }
        public float ton_qtnl { get; set; }
        public int? km_qt { get; set; }
    }

    public class xeView
    {
        public int id { get; set; }
        public string so_xe { get; set; }
        public string ten_xe { get; set; }
        public string loai_xe { get; set; }
        public int km { get; set; }
        public int? km_qt { get; set; }
    }
}