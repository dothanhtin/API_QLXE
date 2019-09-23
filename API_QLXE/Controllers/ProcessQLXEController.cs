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
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API_QLXE.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    [RoutePrefix("api/quanlyxe")]
    //[Route("api/ProcessQLXE")]
    public class ProcessQLXEController : ApiController
    {
        //connection string
        public string connstring = ConfigurationManager.ConnectionStrings["QLXE"].ConnectionString;

        #region Category
        [HttpGet]
        [Route("laydanhsachxe")]
        public IHttpActionResult getListVehicle()
        {
            try
            {
                QLXEContext cn = new QLXEContext();
                var res = cn.Xes.Select(p => new xeView
                {
                    id=p.ID,
                    so_xe=p.SO_XE,
                    ten_xe=p.TEN_XE,
                    loai_xe=p.LOAI_XE,
                    km=p.KM,
                    km_qt=p.KM_QT
                }).ToList();
                if (res.Count > 0) return Json(res);
                return Json(0); //nothing
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("laydanhsachtaixe")]
        public IHttpActionResult getListDriver()
        {
            try
            {
                try
                {
                    QLXEContext cn = new QLXEContext();
                    var res = (from item1 in cn.TaiXes.ToList()
                               join item2 in cn.nhanViens.ToList() on item1.NHANVIEN_ID equals item2.NHANVIEN_ID
                               select new taixeView{
                                   id=item1.ID,
                                   nhanvien_id=item1.NHANVIEN_ID,
                                   ten_taixe=item2.TEN_NV
                               }).ToList();
                    if (res.Count > 0) return Json(res);
                    return Json(0); //nothing
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CRUD Journey
        [HttpGet]
        [Route("laydanhsachhanhtrinh")]
        public IHttpActionResult getAllJourney()
        {
            try
            {
                QLXEContext cn = new QLXEContext();
                //var res = cn.Hanhtrinhs.AsQueryable().ToList();
                var res = (from item1 in cn.Hanhtrinhs
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
                               trangthaicode = item1.TRANG_THAI,
                               trangthai = item3.TEN_TTHT,
                               loaihanhtrinh = item2.TEN_LOAI_HT,
                               //ngayin = item1.NGAY_IN
                           }).DistinctBy(s => s.id).OrderByDescending(s => s.id).ToList();
                return Json(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("crudhanhtrinh")]
        public IHttpActionResult CrudJourney(object obj)
        {
            try
            {
                string formatStartDate, formatEndDate, formatSignDate;
                DateTime? tgdi = null, tgve = null, tgdk = null;
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
                string userdk = p.userdk;
                CrudJourneyOrcl(optTC, idht, tgdk, tgdi, tgve, mucdich, noidi, noiden, tgxp, songuoi, userdk);
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(0);
                throw ex;
            }
        }

        [HttpPost]
        [Route("layhanhtrinhtheoid")]
        public IHttpActionResult getJourneyById(object obj)
        {
            try
            {
                QLXEContext cn = new QLXEContext();
                string jsonStr = JsonConvert.SerializeObject(obj);
                var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
                int id = int.Parse((string)p.id);
                //var journey = cn.Hanhtrinhs.Find(id);
                var res = (from item1 in cn.Hanhtrinhs.Where(k => k.ID == id)
                           join item2 in cn.DmLoaiHanhtrinhs on item1.LOAI_HT equals item2.ID
                           join item3 in cn.DmTrangthaiHts on item1.TRANG_THAI equals item3.ID
                           select new HanhtrinhViewModel
                           {
                               id = item1.ID,
                               //xe=item1.XE,
                               //taixe=item1.TAI_XE,
                               thoigiandk = item1.NGAY_DK,
                               mucdich = item1.MUC_DICH,
                               noidi = item1.NOI_DI,
                               noiden = item1.NOI_DEN,
                               thoigiandi = item1.TG_DI,
                               thoigianve = item1.TG_VE,
                               thoigianxp = item1.GIO_XUAT_PHAT,
                               //kmdi=item1.KM_DI,
                               //kmve=item1.KM_VE,
                               //thoigiancau=item1.TG_CAU,
                               slnguoi = item1.SO_NGUOI,
                               //userdk=item1.USER_DK,
                               //nguoiduyet=item1.NGUOI_DUYET,
                               trangthaicode = item1.TRANG_THAI,
                               trangthai = item3.TEN_TTHT,
                               loaihanhtrinh = item2.TEN_LOAI_HT,
                               //ngayin=item1.NGAY_IN
                           }).DistinctBy(s => s.id).ToList();
                return Json(res.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("xoahanhtrinhdb")]
        public IHttpActionResult deleteJourneyById(object obj)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    QLXEContext cn = new QLXEContext();
                    string jsonStr = JsonConvert.SerializeObject(obj);
                    var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
                    int id = int.Parse((string)p.id);
                    var journey = cn.Hanhtrinhs.Find(id);
                    cn.Hanhtrinhs.Remove(journey);
                    cn.SaveChanges();
                    scope.Complete();
                    return Json(1);
                }
            }
            catch (Exception ex)
            {
                return Json(0);
                throw ex;
            }
        }
        #endregion

        #region Get list journeys of user
        [HttpPost]
        [Route("laydanhsachhanhtrinhTheoNguoiDung")]
        public IHttpActionResult getListJourneyByUserId([FromBody]GetListJourneyByUserIdViewModel getListJourneyByUserIdViewModel)
        {
            try
            {
                List<JourneyInfo> journeyInfos = new List<JourneyInfo>();
                DataTable dt = getListJourneyByUserIdOrcl(getListJourneyByUserIdViewModel);
                //List<DataRow> dataRows = new List<DataRow>(dt.Select());
                journeyInfos = (from DataRow row in dt.Rows
                                select new JourneyInfo
                                {
                                    id = int.Parse(row["ID"].ToString()),
                                    xe = row["XE"].ToString(),
                                    tai_xe = row["TAI_XE"].ToString(),
                                    ngay_dk = string.IsNullOrEmpty(row["NGAY_DK"].ToString()) ? null : (DateTime?)row["NGAY_DK"],
                                    muc_dich = row["MUC_DICH"].ToString(),
                                    noi_di = row["NOI_DI"].ToString(),
                                    noi_den = row["NOI_DEN"].ToString(),
                                    tg_di = string.IsNullOrEmpty(row["TG_DI"].ToString()) ? null : (DateTime?)row["TG_DI"],
                                    tg_ve = string.IsNullOrEmpty(row["TG_VE"].ToString()) ? null : (DateTime?)row["TG_VE"],
                                    gio_xuat_phat = row["GIO_XUAT_PHAT"].ToString(),
                                    km_di = row["KM_DI"].ToString(),
                                    km_ve = row["KM_VE"].ToString(),
                                    tg_cau = row["TG_CAU"].ToString(),
                                    so_nguoi = row["SO_NGUOI"].ToString(),
                                    user_dk = row["USER_DK"].ToString(),
                                    user_duyet = row["USER_DUYET"].ToString(),
                                    trang_thai = row["TRANG_THAI"].ToString(),
                                    loai_ht = row["LOAI_HT"].ToString(),
                                    ngay_in = string.IsNullOrEmpty(row["NGAY_IN"].ToString()) ? null : (DateTime?)row["NGAY_IN"],
                                    ngay_duyet = string.IsNullOrEmpty(row["NGAY_DUYET"].ToString()) ? null : (DateTime?)row["NGAY_DUYET"],
                                    ngay_htra = string.IsNullOrEmpty(row["NGAY_HTRA"].ToString()) ? null : (DateTime?)row["NGAY_HTRA"],
                                    ngay_ht = string.IsNullOrEmpty(row["NGAY_HT"].ToString()) ? null : (DateTime?)row["NGAY_HT"],
                                    ly_do = row["LY_DO"].ToString(),
                                    //NGAY_DK1 = string.IsNullOrEmpty(row["NGAY_DK1"].ToString()) ? null : (DateTime?)row["NGAY_DK1"],
                                    //TG_DI1 = string.IsNullOrEmpty(row["TG_DI1"].ToString()) ? null : (DateTime?)row["TG_DI1"],
                                    //TG_VE1 = string.IsNullOrEmpty(row["TG_VE1"].ToString()) ? null : (DateTime?)row["TG_VE1"],
                                    ngay_dk1 = row["NGAY_DK1"].ToString(),
                                    tg_di1 = row["TG_DI1"].ToString(),
                                    tg_ve1 = row["TG_VE1"].ToString(),
                                    ten_ttht = row["TEN_TTHT"].ToString(),
                                    ten_dk = row["TEN_DK"].ToString(),
                                    ten_nd = row["TEN_ND"].ToString(),
                                    so_xe = row["SO_XE"].ToString(),
                                    kmdi1 = row["KMDI1"].ToString(),
                                    ten_tx = row["TEN_TX"].ToString(),
                                    donvi_id = row["DONVI_ID"].ToString(),
                                    ten_donvi_con = row["TEN_DONVI_CON"].ToString(),
                                    ten_loai_ht = row["TEN_LOAI_HT"].ToString()
                                }).ToList();
                return Json(journeyInfos);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("laydanhsachhanhtrinhTheoNguoiDuyet")]
        public IHttpActionResult getListJourneyWaitApprovingByUserId([FromBody]GetListJourneyWaitApprovingByUserIdViewModel getListJourneyWaitApprovingByUserIdView)
        {
            try
            {
                List<JourneyInfo> journeyInfos = new List<JourneyInfo>();
                DataTable dt = getListJourneyWaitApprovingByUserIdOrcl(getListJourneyWaitApprovingByUserIdView.dv);
                //List<DataRow> dataRows = new List<DataRow>(dt.Select());
                journeyInfos = (from DataRow row in dt.Rows
                                select new JourneyInfo
                                {
                                    id = int.Parse(row["ID"].ToString()),
                                    xe = row["XE"].ToString(),
                                    tai_xe = row["TAI_XE"].ToString(),
                                    ngay_dk = string.IsNullOrEmpty(row["NGAY_DK"].ToString()) ? null : (DateTime?)row["NGAY_DK"],
                                    muc_dich = row["MUC_DICH"].ToString(),
                                    noi_di = row["NOI_DI"].ToString(),
                                    noi_den = row["NOI_DEN"].ToString(),
                                    tg_di = string.IsNullOrEmpty(row["TG_DI"].ToString()) ? null : (DateTime?)row["TG_DI"],
                                    tg_ve = string.IsNullOrEmpty(row["TG_VE"].ToString()) ? null : (DateTime?)row["TG_VE"],
                                    gio_xuat_phat = row["GIO_XUAT_PHAT"].ToString(),
                                    km_di = row["KM_DI"].ToString(),
                                    km_ve = row["KM_VE"].ToString(),
                                    tg_cau = row["TG_CAU"].ToString(),
                                    so_nguoi = row["SO_NGUOI"].ToString(),
                                    user_dk = row["USER_DK"].ToString(),
                                    user_duyet = row["USER_DUYET"].ToString(),
                                    trang_thai = row["TRANG_THAI"].ToString(),
                                    loai_ht = row["LOAI_HT"].ToString(),
                                    ngay_in = string.IsNullOrEmpty(row["NGAY_IN"].ToString()) ? null : (DateTime?)row["NGAY_IN"],
                                    ngay_duyet = string.IsNullOrEmpty(row["NGAY_DUYET"].ToString()) ? null : (DateTime?)row["NGAY_DUYET"],
                                    ngay_htra = string.IsNullOrEmpty(row["NGAY_HTRA"].ToString()) ? null : (DateTime?)row["NGAY_HTRA"],
                                    ngay_ht = string.IsNullOrEmpty(row["NGAY_HT"].ToString()) ? null : (DateTime?)row["NGAY_HT"],
                                    ly_do = row["LY_DO"].ToString(),
                                    //NGAY_DK1 = string.IsNullOrEmpty(row["NGAY_DK1"].ToString()) ? null : (DateTime?)row["NGAY_DK1"],
                                    //TG_DI1 = string.IsNullOrEmpty(row["TG_DI1"].ToString()) ? null : (DateTime?)row["TG_DI1"],
                                    //TG_VE1 = string.IsNullOrEmpty(row["TG_VE1"].ToString()) ? null : (DateTime?)row["TG_VE1"],
                                    ngay_dk1 = row["NGAY_DK1"].ToString(),
                                    tg_di1 = row["TG_DI1"].ToString(),
                                    tg_ve1 = row["TG_VE1"].ToString(),
                                    ten_ttht = row["TEN_TTHT"].ToString(),
                                    ten_dk = row["TEN_DK"].ToString(),
                                    ten_nd = row["TEN_ND"].ToString(),
                                    so_xe = row["SO_XE"].ToString(),
                                    kmdi1 = row["KMDI1"].ToString(),
                                    ten_tx = row["TEN_TX"].ToString(),
                                    sdt_tx= row["SDT_TX"].ToString(),
                                    donvi_id = row["DONVI_ID"].ToString(),
                                    ten_donvi_con = row["TEN_DONVI_CON"].ToString(),
                                    ten_loai_ht = row["TEN_LOAI_HT"].ToString(),
                                    ngay_in1= row["NGAY_IN1"].ToString()
                                }).ToList();
                return Json(journeyInfos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("laydanhsachhanhtrinhTheoTaiXe")]
        public IHttpActionResult getListJourneyByDriverId([FromBody]GetListJourneyByUserIdViewModel getListJourneyByUserIdViewModel)
        {
            try
            {
                List<JourneyInfo> journeyInfos = new List<JourneyInfo>();
                DataTable dt = getListJourneyByDriverIdOrcl(getListJourneyByUserIdViewModel);
                //List<DataRow> dataRows = new List<DataRow>(dt.Select());
                journeyInfos = (from DataRow row in dt.Rows
                                select new JourneyInfo
                                {
                                    id = int.Parse(row["ID"].ToString()),
                                    xe = row["XE"].ToString(),
                                    tai_xe = row["TAI_XE"].ToString(),
                                    ngay_dk = string.IsNullOrEmpty(row["NGAY_DK"].ToString()) ? null : (DateTime?)row["NGAY_DK"],
                                    muc_dich = row["MUC_DICH"].ToString(),
                                    noi_di = row["NOI_DI"].ToString(),
                                    noi_den = row["NOI_DEN"].ToString(),
                                    tg_di = string.IsNullOrEmpty(row["TG_DI"].ToString()) ? null : (DateTime?)row["TG_DI"],
                                    tg_ve = string.IsNullOrEmpty(row["TG_VE"].ToString()) ? null : (DateTime?)row["TG_VE"],
                                    gio_xuat_phat = row["GIO_XUAT_PHAT"].ToString(),
                                    km_di = row["KM_DI"].ToString(),
                                    km_ve = row["KM_VE"].ToString(),
                                    tg_cau = row["TG_CAU"].ToString(),
                                    so_nguoi = row["SO_NGUOI"].ToString(),
                                    user_dk = row["USER_DK"].ToString(),
                                    user_duyet = row["USER_DUYET"].ToString(),
                                    trang_thai = row["TRANG_THAI"].ToString(),
                                    loai_ht = row["LOAI_HT"].ToString(),
                                    ngay_in = string.IsNullOrEmpty(row["NGAY_IN"].ToString()) ? null : (DateTime?)row["NGAY_IN"],
                                    ngay_duyet = string.IsNullOrEmpty(row["NGAY_DUYET"].ToString()) ? null : (DateTime?)row["NGAY_DUYET"],
                                    ngay_htra = string.IsNullOrEmpty(row["NGAY_HTRA"].ToString()) ? null : (DateTime?)row["NGAY_HTRA"],
                                    ngay_ht = string.IsNullOrEmpty(row["NGAY_HT"].ToString()) ? null : (DateTime?)row["NGAY_HT"],
                                    ly_do = row["LY_DO"].ToString(),
                                    //NGAY_DK1 = string.IsNullOrEmpty(row["NGAY_DK1"].ToString()) ? null : (DateTime?)row["NGAY_DK1"],
                                    //TG_DI1 = string.IsNullOrEmpty(row["TG_DI1"].ToString()) ? null : (DateTime?)row["TG_DI1"],
                                    //TG_VE1 = string.IsNullOrEmpty(row["TG_VE1"].ToString()) ? null : (DateTime?)row["TG_VE1"],
                                    ngay_dk1 = row["NGAY_DK1"].ToString(),
                                    tg_di1 = row["TG_DI1"].ToString(),
                                    tg_ve1 = row["TG_VE1"].ToString(),
                                    ten_ttht = row["TEN_TTHT"].ToString(),
                                    ten_dk = row["TEN_DK"].ToString(),
                                    ten_nd = row["TEN_ND"].ToString(),
                                    so_xe = row["SO_XE"].ToString(),
                                    kmdi1 = row["KMDI1"].ToString(),
                                    ten_tx = row["TEN_TX"].ToString(),
                                    sdt_tx = row["SDT_TX"].ToString(),
                                    donvi_id = row["DONVI_ID"].ToString(),
                                    ten_donvi_con = row["TEN_DONVI_CON"].ToString(),
                                    ten_loai_ht = row["TEN_LOAI_HT"].ToString(),
                                    ngay_in1 = row["NGAY_IN1"].ToString()
                                }).ToList();
                return Json(journeyInfos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update Approval
        [Route("capnhatduyet")]
        [HttpPost]
        public IHttpActionResult UpdateApproval([FromBody]ApprovalViewModel approvalViewModel)
        {
            try
            {
                UpdateApprovalOrcl(approvalViewModel);
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(0);
                throw ex;
            }
        }
        #endregion

        #region Update Kilometers
        [Route("capnhatkm")]
        [HttpPost]
        public IHttpActionResult UpdateKilometers([FromBody]KilometerViewModel kilometerViewModel)
        {
            try
            {
                UpdateKilometersOrcl(kilometerViewModel);
                return Json(1);
            }
            catch(Exception ex)
            {
                return Json(0);
                throw ex;
            }
        }
        #endregion

        #region Manage users
        [HttpPost]
        [Route("kiemtratendangnhap")]
        public IHttpActionResult checkUsername([FromBody]UserNameViewModel userNameViewModel)
        {
            try
            {
                var res = Logincheck(userNameViewModel.username);
                if (res.Rows.Count > 0) return Json(1);
                return Json(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("dangnhap")]
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
        [Route("doimatkhau")]
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
        [Route("guiotp")]
        public IHttpActionResult sendOTP(object obj)
        {
            QLXEContext cn = new QLXEContext();
            string jsonStr = JsonConvert.SerializeObject(obj);
            var p = JsonConvert.DeserializeObject<dynamic>(jsonStr);
            string phonenumber = p.phonenumber;
            try
            {
                using (TransactionScope scope = new TransactionScope())
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
                    scope.Complete();
                    //Gửi SMS
                    string message = String.Format("Ma OTP xac thuc tai khoan dang nhap {0} cua quy khach vao he thong QLXE la:{1}. Ma co hieu luc trong vong 5 phut.", phonenumber, OTPCode);
                    string res = send_sms(phonenumber, message);
                    return Json(res);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("xacthucotp")]
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region called function
        public void CrudJourneyOrcl(int optTC, int idht, DateTime? tgdk, DateTime? tgdi, DateTime? tgve, string mucdich, string noidi, string noiden, string tgxp, int songuoi, string userdk)
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
                    cmd.Parameters.Add("userdk", OracleDbType.Varchar2).Value = userdk;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object checklogin(string username, string password_md5)
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
                                              username = row["USERNAME"].ToString(),
                                              //PASS=row["PASS"].ToString(),
                                              //PASSFINAL = row["PASSFINAL"].ToString(),
                                              nhanvien_id = row["NHANVIEN_ID"].ToString(),
                                              ngay_login = (DateTime)row["NGAY_LOGIN"],
                                              trang_thai = int.Parse(row["TRANG_THAI"].ToString()),
                                              ghi_chu = row["GHI_CHU"].ToString(),
                                              create_date = (DateTime)row["CREATE_DATE"],
                                              ten_nv = row["TEN_NV"].ToString(),
                                              sdt_lh = row["SDT_LH"].ToString(),
                                              donvi_id = int.Parse(row["DONVI_ID"].ToString()),
                                              donvi_cha_id = int.Parse(row["DONVI_CHA_ID"].ToString()),
                                              ten_dv_cha = row["TEN_DV_CHA"].ToString()
                                          }).ToList();
                        return new { code = 1, data = userInfoModels };
                    }
                    return new { code = 0, data = userInfoModels };
                }
                return new { code = -1, data = userInfoModels };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Logincheck(string username)
        {
            using (OracleConnection cn = new OracleConnection(connstring))
            {
                OracleCommand cmd = new OracleCommand("QUANTRIHETHONG.Logincheck", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("user", OracleDbType.Varchar2).Value = username;
                cmd.Parameters.Add("Param1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable d = new DataTable();
                da.Fill(d);
                return d;
            }
        }
        public DataTable Loginchecktaoss(string username, string password_md5)
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
        public int changePassProc(string username, string newpass_md5)
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
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
        }
        public void UpdateKilometersOrcl(KilometerViewModel kilometerViewModel)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(connstring))
                {
                    OracleCommand cmd = new OracleCommand("HANHTRINHXE.Capnhatkm", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("idxe", OracleDbType.Decimal).Value = kilometerViewModel.idxe;
                    cmd.Parameters.Add("kmdongho", OracleDbType.Decimal).Value = kilometerViewModel.kmdongho;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getListJourneyByUserIdOrcl(GetListJourneyByUserIdViewModel getListJourneyByUserIdViewModel)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(connstring))
                {
                    OracleCommand cmd = new OracleCommand("HANHTRINHXE.GetCngridhtdvidk", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("userdk", OracleDbType.Varchar2).Value = getListJourneyByUserIdViewModel.userdk;
                    cmd.Parameters.Add("dv", OracleDbType.Varchar2).Value = getListJourneyByUserIdViewModel.dv;
                    cmd.Parameters.Add("Param1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable d = new DataTable();
                    da.Fill(d);
                    return d;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getListJourneyWaitApprovingByUserIdOrcl(string dv)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(connstring))
                {
                    OracleCommand cmd = new OracleCommand("HANHTRINHXE.GetCngridhtdvidk", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("dv", OracleDbType.Varchar2).Value = dv;
                    cmd.Parameters.Add("Param1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable d = new DataTable();
                    da.Fill(d);
                    return d;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getListJourneyByDriverIdOrcl(GetListJourneyByUserIdViewModel getListJourneyByUserIdViewModel)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(connstring))
                {
                    OracleCommand cmd = new OracleCommand("HANHTRINHXE.GetCngridhtdvidk", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("tx", OracleDbType.Varchar2).Value = getListJourneyByUserIdViewModel.userdk;
                    cmd.Parameters.Add("dv", OracleDbType.Varchar2).Value = getListJourneyByUserIdViewModel.dv;
                    cmd.Parameters.Add("Param1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable d = new DataTable();
                    da.Fill(d);
                    return d;
                }
            }
            catch (Exception ex)
            {
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
            return kq;
        }

        private static Random random = new Random();
        public static string GetRandomString(int length)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void UpdateApprovalOrcl(ApprovalViewModel approvalViewModel)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(connstring))
                {
                    OracleCommand cmd = new OracleCommand("HANHTRINHXE.Capnhatduyet", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("optTC", OracleDbType.Decimal).Value = approvalViewModel.optTC;
                    cmd.Parameters.Add("idht", OracleDbType.Decimal).Value = approvalViewModel.idht;
                    cmd.Parameters.Add("idxe", OracleDbType.Decimal).Value = approvalViewModel.idxe;
                    cmd.Parameters.Add("taixe", OracleDbType.Decimal).Value = approvalViewModel.taixe;
                    cmd.Parameters.Add("tgdi", OracleDbType.Date).Value = approvalViewModel.tgdi;
                    cmd.Parameters.Add("tgve", OracleDbType.Date).Value = approvalViewModel.tgve;
                    cmd.Parameters.Add("gioxuatphat", OracleDbType.Varchar2).Value = approvalViewModel.gioxuatphat;
                    cmd.Parameters.Add("kmdi", OracleDbType.Decimal).Value = approvalViewModel.kmdi;
                    cmd.Parameters.Add("kmve", OracleDbType.Decimal).Value = approvalViewModel.kmve;
                    cmd.Parameters.Add("tgcau", OracleDbType.Decimal).Value = approvalViewModel.tgcau;
                    cmd.Parameters.Add("userduyet", OracleDbType.Varchar2).Value = approvalViewModel.userduyet;
                    cmd.Parameters.Add("lydo", OracleDbType.Varchar2).Value = approvalViewModel.lydo;
                    cmd.Parameters.Add("ngayin", OracleDbType.Date).Value = approvalViewModel.ngayin;
                    cmd.Parameters.Add("ngayht", OracleDbType.Date).Value = approvalViewModel.ngayht;
                    cmd.Parameters.Add("ngayduyet", OracleDbType.Date).Value = approvalViewModel.ngayduyet;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}