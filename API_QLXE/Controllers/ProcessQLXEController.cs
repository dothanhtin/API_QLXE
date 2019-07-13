using API_QLXE.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API_QLXE.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public class ProcessQLXEController: ApiController
    {
        public string connstring = ConfigurationManager.ConnectionStrings["QLXE"].ConnectionString;
        [HttpGet]
        public IHttpActionResult getAllJourney()
        {
            try
            {
                QLXEContext cn = new QLXEContext();
                //var res = cn.Hanhtrinhs.AsQueryable().ToList();
                var res= (from item1 in cn.Hanhtrinhs
                          join item2 in cn.DmLoaiHanhtrinhs on item1.LOAI_HT equals item2.ID
                          join item3 in cn.DmTrangthaiHts on item1.TRANG_THAI equals item3.ID
                          select new HanhtrinhViewModel
                          {
                              id = item1.ID,
                              //xe = item1.XE,
                              //taixe = item1.TAI_XE,
                              thoigiandk = item1.NGAY_DK,
                              mucdich = item1.MUC_DICH,
                              noidi = item1.NOI_DI,
                              noiden = item1.NOI_DEN,
                              thoigiandi = item1.TG_DI,
                              thoigianve = item1.TG_VE,
                              thoigianxp = item1.GIO_XUAT_PHAT,
                              //kmdi = item1.KM_DI,
                              //kmve = item1.KM_VE,
                              //thoigiancau = item1.TG_CAU,
                              slnguoi = item1.SO_NGUOI,
                              //userdk = item1.USER_DK,
                              //nguoiduyet = item1.NGUOI_DUYET,
                              trangthaicode=item1.TRANG_THAI,
                              trangthai = item3.TEN_TTHT,
                              loaihanhtrinh = item2.TEN_LOAI_HT,
                              //ngayin = item1.NGAY_IN
                          }).DistinctBy(s => s.id).OrderByDescending(s=>s.id).ToList();
                return Json(res);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IHttpActionResult CrudJourney(object obj)
        {
            try
            {
                string formatStartDate, formatEndDate,formatSignDate;
                DateTime? tgdi = null, tgve = null, tgdk=null;
                string jsonStr = JsonConvert.SerializeObject(obj);
                var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
                int optTC = int.Parse((string)p.optTC);
                int idht = int.Parse((string)p.idht);
                formatStartDate = p.thoigiandi;
                formatEndDate = p.thoigianve;
                formatSignDate = p.thoigiandk;
                if (formatStartDate != null)
                {
                    tgdi = DateTime.Parse(formatStartDate).Date;
                }
                if (formatStartDate != null)
                {
                    tgve = DateTime.Parse(formatEndDate).Date;
                }
                if (formatSignDate != null)
                {
                    tgdk = DateTime.Parse(formatSignDate).Date;
                }
                string mucdich = p.mucdich;
                string noidi = p.noidi;
                string noiden = p.noiden;
                string tgxp = p.thoigianxp;
                int songuoi = p.slnguoi;
                CrudJourneyOrcl(optTC, idht, tgdk, tgdi, tgve, mucdich, noidi, noiden, tgxp, songuoi);
                return Json(1);
            }
            catch(Exception ex)
            {
                return Json(0);
                throw ex;
            }
        }

        [HttpPost]
        public IHttpActionResult getJourneyById(object obj)
        {
            try
            {
                QLXEContext cn = new QLXEContext();
                string jsonStr = JsonConvert.SerializeObject(obj);
                var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
                int id = int.Parse((string)p.id);
                //var journey = cn.Hanhtrinhs.Find(id);
                var res = (from item1 in cn.Hanhtrinhs.Where(k=>k.ID==id)
                           join item2 in cn.DmLoaiHanhtrinhs on item1.LOAI_HT equals item2.ID
                           join item3 in cn.DmTrangthaiHts on item1.TRANG_THAI equals item3.ID
                           select new HanhtrinhViewModel
                           {
                               id=item1.ID,
                               //xe=item1.XE,
                               //taixe=item1.TAI_XE,
                               thoigiandk=item1.NGAY_DK,
                               mucdich=item1.MUC_DICH,
                               noidi=item1.NOI_DI,
                               noiden=item1.NOI_DEN,
                               thoigiandi=item1.TG_DI,
                               thoigianve=item1.TG_VE,
                               thoigianxp=item1.GIO_XUAT_PHAT,
                               //kmdi=item1.KM_DI,
                               //kmve=item1.KM_VE,
                               //thoigiancau=item1.TG_CAU,
                               slnguoi=item1.SO_NGUOI,
                               //userdk=item1.USER_DK,
                               //nguoiduyet=item1.NGUOI_DUYET,
                               trangthaicode = item1.TRANG_THAI,
                               trangthai =item3.TEN_TTHT,
                               loaihanhtrinh=item2.TEN_LOAI_HT,
                               //ngayin=item1.NGAY_IN
                           }).DistinctBy(s => s.id).ToList();
                return Json(res.FirstOrDefault());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IHttpActionResult deleteJourneyById(object obj)
        {
            try
            {
                QLXEContext cn = new QLXEContext();
                string jsonStr = JsonConvert.SerializeObject(obj);
                var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
                int id = int.Parse((string)p.id);
                var journey = cn.Hanhtrinhs.Find(id);
                cn.Hanhtrinhs.Remove(journey);
                cn.SaveChanges();
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(0);
                throw ex;
            }
        }
        public void CrudJourneyOrcl(int optTC,int idht,DateTime? tgdk,DateTime? tgdi,DateTime? tgve,string mucdich,string noidi,string noiden,string tgxp,int songuoi)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(connstring))
                {
                    OracleCommand cmd = new OracleCommand("HANHTRINHXE.Capnhatdkx", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("optTC", OracleDbType.Decimal).Value = optTC;
                    cmd.Parameters.Add("idht", OracleDbType.Decimal).Value = idht;
                    cmd.Parameters.Add("ngaydk", OracleDbType.Date).Value = tgdk;
                    cmd.Parameters.Add("mucdich", OracleDbType.Varchar2).Value = mucdich;
                    cmd.Parameters.Add("noidi", OracleDbType.Varchar2).Value = noidi;
                    cmd.Parameters.Add("noiden", OracleDbType.Varchar2).Value = noiden;
                    cmd.Parameters.Add("tgdi", OracleDbType.Date).Value = tgdi;
                    cmd.Parameters.Add("tgve", OracleDbType.Date).Value = tgve;
                    cmd.Parameters.Add("gioxuatphat", OracleDbType.Varchar2).Value = tgxp;
                    cmd.Parameters.Add("songuoi", OracleDbType.Decimal).Value = songuoi;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}