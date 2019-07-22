using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    [Table("QLXE.OTP")]
    public class Otp
    {
        [Key]
        public string OTP_CODE { get; set; }
        public string PHONE_NUMBER { get; set; }
        public DateTime CREATEDTIME { get; set; }
    }
}