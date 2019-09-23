using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    [Table("QLXE.TAIXE")]
    public class TaiXe
    {
        [Key]
        public int ID { get; set; }
        public int NHANVIEN_ID { get; set; }
        public string DAU_BANG { get; set; }
        public int TRANG_THAI { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string GHI_CHU { get; set; }
    }

    public class taixe
    {
        public int id { get; set; }
        public int nhanvien_id { get; set; }
        public string ten_taixe { get; set; }
        public string dau_bang { get; set; }
        public int trang_thai { get; set; }
        public DateTime create_date { get; set; }
        public string ghi_chu { get; set; }
    }

    public class taixeView
    {
        public int id { get; set; }
        public int nhanvien_id { get; set; }
        public string ten_taixe { get; set; }
    }
}