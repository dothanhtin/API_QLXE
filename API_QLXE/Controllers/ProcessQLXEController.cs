using API_QLXE.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API_QLXE.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public class ProcessQLXEController: ApiController
    {
        //connection string
        public string connstring = ConfigurationManager.ConnectionStrings["QLXE"].ConnectionString;

        #region CRUD Journey
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
        #endregion

        #region Manage users
        [HttpPost]
        public IHttpActionResult userLogin(object obj)
        {
            string jsonStr = JsonConvert.SerializeObject(obj);
            var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
            string username = p.username;
            string password_md5 = p.password_md5;
            var res = checklogin(username, password_md5);
            return Json(res);
        }

        [HttpPost]
        public IHttpActionResult changePassword(object obj)
        {
            string jsonStr = JsonConvert.SerializeObject(obj);
            var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
            string username = p.username;
            string newpass_md5 = p.newpass_md5;
            int res = changePassProc(username, newpass_md5);
            return Json(res);
        }

        [HttpPost]
        public IHttpActionResult sendOTP(object obj)
        {
            QLXEContext cn = new QLXEContext();
            string jsonStr = JsonConvert.SerializeObject(obj);
            var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
            string phonenumber = p.phonenumber;
            try
            {
                //Tạo mã
                string OTPCode = GetRandomString(6);
                //Lưu
                Otp otp = new Otp();
                otp.OTP_CODE = OTPCode;
                otp.PHONE_NUMBER = phonenumber;
                otp.CREATEDTIME = DateTime.Now;
                cn.Otps.Add(otp);
                cn.SaveChanges();
                //Gửi SMS
                string message = String.Format("Ma OTP xac thuc tai khoan dang nhap {0} cua quy khach vao he thong QLXE la:{1}. Ma co hieu luc trong vong 5 phut.",phonenumber, OTPCode);
                string res = send_sms(phonenumber,OTPCode);
                return Json(res); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IHttpActionResult validateOTP(object obj)
        {
            QLXEContext cn = new QLXEContext();
            try
            {
                string jsonStr = JsonConvert.SerializeObject(obj);
                var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
                string number = p.number;
                string otpcode = p.otpcode;
                var res = cn.Otps.AsQueryable().Where(s => s.OTP_CODE == otpcode && s.PHONE_NUMBER == number).FirstOrDefault();
                if (res != null)
                    return Json(1);
                return Json(0);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region called function
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

        public object checklogin(string username,string password_md5)
        {
            QLXEContext cn = new QLXEContext();
            List<UserInfoModel> userInfoModels = new List<UserInfoModel>();
            try
            {
                //check stage 1
                string check_md5_userpass_input = MD5(username + password_md5);
                var userinfo = cn.NguoiDungs.Where(s => s.USERNAME == username).ToList().FirstOrDefault();
                if (userinfo != null)
                {
                    string check_md5_userpass_sys = MD5(userinfo.USERNAME + userinfo.PASS);
                    if (check_md5_userpass_sys == check_md5_userpass_input)
                    {
                        //return user info
                        DataTable dt = Loginchecktaoss(username, password_md5);
                        userInfoModels = (from DataRow row in dt.Rows
                                          select new UserInfoModel
                                          {
                                              USERNAME= row["USERNAME"].ToString(),
                                              PASS=row["PASS"].ToString(),
                                              PASSFINAL = row["PASSFINAL"].ToString(),
                                              NHANVIEN_ID = row["NHANVIEN_ID"].ToString(),
                                              NGAY_LOGIN = (DateTime)row["NGAY_LOGIN"],
                                              TRANG_THAI = int.Parse(row["TRANG_THAI"].ToString()),
                                              GHI_CHU = row["GHI_CHU"].ToString(),
                                              CREATE_DATE = (DateTime)row["CREATE_DATE"],
                                              TEN_NV = row["TEN_NV"].ToString(),
                                              SDT_LH = row["SDT_LH"].ToString(),
                                              DONVI_ID = int.Parse(row["DONVI_ID"].ToString()),
                                              DONVI_CHA_ID = int.Parse(row["DONVI_CHA_ID"].ToString()),
                                              TEN_DV_CHA = row["TEN_DV_CHA"].ToString()
                                          }).ToList();
                        return new { code = 1, data = userInfoModels };
                    }
                    return new { code = 0, data = userInfoModels };
                }
                return new { code = -1, data = userInfoModels };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Loginchecktaoss(string username,string password_md5)
        {
            using (OracleConnection cn = new OracleConnection(connstring))
            {
                OracleCommand cmd = new OracleCommand("QUANTRIHETHONG.LOGINCHECKTAOSS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("user", OracleDbType.Varchar2).Value = username;
                cmd.Parameters.Add("passdn", OracleDbType.Varchar2).Value = password_md5;
                cmd.Parameters.Add("Param1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable d = new DataTable();
                da.Fill(d);
                return d;
            }
        }
        public int changePassProc(string username,string newpass_md5)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(connstring))
                {
                    OracleCommand cmd = new OracleCommand("quantringuoidung.Changepass", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_user", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("passnew", OracleDbType.Varchar2).Value = newpass_md5;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
                throw ex;
            }
        }
        public static string tokenGenerator()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 20);
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToUpper();
        }

        public static string MD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }

        private string send_sms(string sdt, string noidung)
        {
            WsSendSmsDemo.AuthHeader au = new WsSendSmsDemo.AuthHeader();
            au.Username = "tthuy";
            au.Password = "dhdv678";
            WsSendSmsDemo.Service1 sendsms = new WsSendSmsDemo.Service1();
            sendsms.AuthHeaderValue = au;
            string kq = sendsms.sendsms(sdt, noidung);
            return "[{\"kequa\":\"" + kq + "\"}]";
        }

        private static Random random = new Random();
        public static string GetRandomString(int length)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
}