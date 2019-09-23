using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    public class ApprovalViewModel
    {
        public int optTC { get; set; }
        public int idht { get; set; }
        public int idxe { get; set; }
        public int taixe { get; set; }
        public DateTime tgdi { get; set; }
        public DateTime tgve { get; set; }
        public string gioxuatphat { get; set; }
        public int kmdi { get; set; }
        public int kmve { get; set; }
        public int tgcau { get; set; }
        public string userduyet { get; set; }
        public string lydo { get; set; }
        public DateTime ngayin { get; set; }
        public DateTime ngayht { get; set; }
        public DateTime ngayduyet { get; set; }
    }
    public class UserNameViewModel
    {
        public string username { get; set; }
    }
    public class KilometerViewModel
    {
        public int idxe { get; set; }
        public int kmdongho { get; set; }
    }
    public class GetListJourneyByUserIdViewModel
    {
        public string userdk { get; set; }
        public string dv { get; set; }
    }
    public class GetListJourneyWaitApprovingByUserIdViewModel
    {
        public string dv { get; set; }
    }
}