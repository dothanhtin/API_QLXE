using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace API_QLXE.Models
{
    public class QLXEContext: DbContext
    {
        public QLXEContext(): base("QLXE")
        {

        }
        public DbSet<Hanhtrinh> Hanhtrinhs { get; set; }
        public DbSet<DmLoaiHanhtrinh> DmLoaiHanhtrinhs { get; set; }
        public DbSet<DmTrangthaiHt> DmTrangthaiHts { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<Otp> Otps { get; set; }
    }
}