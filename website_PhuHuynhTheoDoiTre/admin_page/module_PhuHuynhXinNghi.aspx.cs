using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class website_PhuHuynhTheoDoiTre_admin_page_module_PhuHuynhXinNghi : System.Web.UI.Page
{
    dbcsdlDataContext db = new dbcsdlDataContext();
    cls_Alert alert = new cls_Alert();
    private int _id;
    private static int _idUser;
    private static int id_Lop;
    private static int id_NamHoc;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["UserName"] != null)
        {
            var checkuserid = (from u in db.admin_Users where u.username_username == Request.Cookies["UserName"].Value select u).First();
            _idUser = checkuserid.username_id;
            var checkNamHoc = (from nh in db.tbHoctap_NamHocs orderby nh.namhoc_id descending select nh).First();
            id_NamHoc = checkNamHoc.namhoc_id;
            if (!IsPostBack)
            {
                Session["_id"] = 0;
            }
            if (db.tbGiaoVienTrongLops.Any(x => x.taikhoan_id == checkuserid.username_id && x.namhoc_id == checkNamHoc.namhoc_id))
            {
                id_Lop = Convert.ToInt32((from u in db.admin_Users
                                          join gvtl in db.tbGiaoVienTrongLops on u.username_id equals gvtl.taikhoan_id
                                          where u.username_username == Request.Cookies["UserName"].Value
                                          && gvtl.namhoc_id == checkNamHoc.namhoc_id
                                          select gvtl).FirstOrDefault().lop_id);
                var listHocSinh = from hs in db.tbHocSinhs
                                  join hstl in db.tbHocSinhTrongLops on hs.hocsinh_id equals hstl.hocsinh_id
                                  join l in db.tbLops on hstl.lop_id equals l.lop_id
                                  where hstl.namhoc_id == checkNamHoc.namhoc_id && l.lop_id == id_Lop
                                  //&& hstl.hidden == false
                                  select hs;
                ddlHocSinh.DataSource = listHocSinh;
                ddlHocSinh.DataBind();
                loadData();
                
            }
            else
                Response.Redirect("/Admin_Default.aspx");
        }
        else
        {
            Response.Redirect("/website_PhuHuynhTheoDoiTre/admin_page/admin_Login.aspx");
        }
    }
    private void loadData()
    {
        // load data đổ vào var danh sách
        var getData = from hs in db.tbHocSinhs
                      join hstl in db.tbHocSinhTrongLops on hs.hocsinh_id equals hstl.hocsinh_id
                      join xn in db.tbPhuHuynhXinNghis on hstl.hstl_id equals xn.hstl_id
                      where hstl.lop_id == id_Lop
                      && xn.namhoc_id == id_NamHoc
                      //&& hstl.hidden == false
                      orderby xn.phuhuynhxinnghi_id descending, xn.phuhuynhxinnghi_xacnhan ascending
                      select new
                      {
                          xn.phuhuynhxinnghi_id,
                          hs.hocsinh_name,
                          xn.phuhuynhxinnghi_ngaybatdau,
                          xn.phuhuynhxinnghi_ngayketthuc,
                          xn.phuhuynhxinnghi_lydo,
                          phuhuynhxinnghi_ngaydangki = xn.phuhuynhxinnghi_ngaydangki.Value.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                          phuhuynhxinnghi_xacnhan = xn.phuhuynhxinnghi_xacnhan == false ? "Chưa xác nhận" : "Đã xác nhận"
                      };
        // đẩy dữ liệu vào gridivew
        grvList.DataSource = getData;
        grvList.DataBind();
    }

    protected void btnThemMoi_Click(object sender, EventArgs e)
    {
        // Khi nhấn nút thêm thì mật định session id = 0 để thêm mới
        Session["_id"] = 0;
        setNull();
        div_ThemMoi.Visible = true;
        div_CapNhat.Visible = false;
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Insert", "popupControl.Show();", true);
    }

    protected void btnXacNhan_Click(object sender, EventArgs e)
    {
        _id = Convert.ToInt32(grvList.GetRowValues(grvList.FocusedRowIndex, new string[] { "phuhuynhxinnghi_id" }));
        tbPhuHuynhXinNghi update = (from xn in db.tbPhuHuynhXinNghis
                                    where xn.phuhuynhxinnghi_id == _id
                                    select xn).SingleOrDefault();
        if (update.phuhuynhxinnghi_xacnhan == true)
        {
            alert.alert_Warning(Page, "Dữ liệu đã xác nhận", "");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "HiddenLoadingIcon()", true);
        }
        else
        {
            update.phuhuynhxinnghi_xacnhan = true;
            update.username_id = _idUser;

            var getData = (from phxn in db.tbPhuHuynhXinNghis
                           where phxn.phuhuynhxinnghi_id == _id
                           select phxn
                           ).FirstOrDefault();
            DateTime ngayBatDau = Convert.ToDateTime(getData.phuhuynhxinnghi_ngaybatdau);
            DateTime ngayKetThuc = Convert.ToDateTime(getData.phuhuynhxinnghi_ngayketthuc);

            TimeSpan soNgayNghi = ngayKetThuc - ngayBatDau;
            //for (int i = 0; i <= soNgayNghi.Days; i++)
            //{
           
            //}
            //lưu thông báo cho phụ huynh
            var getEmail = (from hs in db.tbHocSinhs
                            join hstl in db.tbHocSinhTrongLops on hs.hocsinh_id equals hstl.hocsinh_id
                            where hstl.hstl_id == getData.hstl_id
                             && hstl.namhoc_id == id_NamHoc
                            select hs).FirstOrDefault();

            //gửi mail về cho ph
            db.SubmitChanges();
            alert.alert_Success(Page, "Đã gửi mail thông báo xác nhận về phụ huynh", "");
            string message = "Thông báo xin nghỉ học của phụ huynh bé " + getEmail.hocsinh_name + " đã được giáo viên chủ nhiệm xác nhận. Xem chi tiết tại đây.</a>";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Detail", "grvList.Refresh();HiddenLoadingIcon()", true);
            // đẩy dữ liệu vào gridivew
        }
    }

    private bool SendMail(string email, string message)
    {
        if (email != "")
        {
            try
            {
                var fromAddress = "huynhquoctoan1642002@gmail.com";//  Email Address from where you send the mail 
                var toAddress = email;
                const string fromPassword = "neiabcekdjluofid";
                string subject, title;
                title = "Thông báo";
                subject = "<!DOCTYPE html><html><head><title></title></head><body ><div>" +
                "<h3 style=\"margin-top:0px; text-align:center; color:#029ada\">" + message + "</h3>" +
                "</div></body></html>";
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;
                }
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(fromAddress, "Trường Mầm non Việt Nhật");
                mm.Subject = title;
                mm.To.Add(toAddress);
                mm.IsBodyHtml = true;
                mm.Body = subject;
                smtp.Send(mm);
                return true;
            }
            catch
            {
                return false;
            }
        }
        else
            return false;
    }

    protected void btnCapNhatNgayNghi_Click(object sender, EventArgs e)
    {
        _id = Convert.ToInt32(grvList.GetRowValues(grvList.FocusedRowIndex, new string[] { "phuhuynhxinnghi_id" }));
        // đẩy id vào session
        Session["_id"] = _id;
        var getData = (from nc in db.tbPhuHuynhXinNghis
                       join hstl in db.tbHocSinhTrongLops on nc.hstl_id equals hstl.hstl_id
                       join hs in db.tbHocSinhs on hstl.hocsinh_id equals hs.hocsinh_id
                       where nc.phuhuynhxinnghi_id == _id
                       select new
                       {
                           nc.phuhuynhxinnghi_id,
                           nc.phuhuynhxinnghi_ngaybatdau,
                           nc.phuhuynhxinnghi_ngayketthuc,
                           hs.hocsinh_name,

                       }).Single();
        txtHoTen.Text = getData.hocsinh_name;
        txtTuNgay.Value = getData.phuhuynhxinnghi_ngaybatdau.Value.ToString("yyyy-MM-dd").Replace(' ', 'T');
        txtDenNgay.Value = getData.phuhuynhxinnghi_ngayketthuc.Value.ToString("yyyy-MM-dd").Replace(' ', 'T');
        div_ThemMoi.Visible = false;
        div_CapNhat.Visible = true;
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Detail", "popupControl.Show();", true);
    }

    private void setNull()
    {
        ddlHocSinh.Value = "";
        dteTuNgay.Value = "";
        dteDenNgay.Value = "";
        txtLyDo.Value = "";
    }

    protected void btnLuu_Click(object sender, EventArgs e)
    {
        try
        {
            tbPhuHuynhXinNghi update = (from xn in db.tbPhuHuynhXinNghis
                                                     where xn.phuhuynhxinnghi_id == Convert.ToInt32(Session["_id"].ToString())
                                                     select xn).SingleOrDefault();
            update.phuhuynhxinnghi_ngaybatdau = Convert.ToDateTime(txtTuNgay.Value);
            update.phuhuynhxinnghi_ngayketthuc = Convert.ToDateTime(txtDenNgay.Value);
            db.SubmitChanges();
            alert.alert_Success(Page, "Cập nhật thành công!", "");
            loadData();
            popupControl.ShowOnPageLoad = false;
            string message = "Giáo viên chủ nhiệm đã cập nhật lại ngày nghỉ của bé. Xem chi tiết tại đây.</a>";
            //SendMail("ducpn@vjis.edu.vn", message);

            setNull();
        }
        catch { }
    }

    protected void btnLuuThemMoi_Click(object sender, EventArgs e)
    {
        try
        {
            div_CapNhat.Visible = false;
            if (Session["_id"].ToString() == "0")
            {
                if (ddlHocSinh.Value == null)
                {
                    alert.alert_Warning(Page, "Vui lòng chọn học sinh cần xin nghỉ", "");
                }
                else if (dteDenNgay.Value == "")
                {
                    alert.alert_Warning(Page, "Vui lòng chọn ngày kết thúc", "");
                }
                else
                {
                    var checkHS = (from hstl in db.tbHocSinhTrongLops
                                   where hstl.hocsinh_id == Convert.ToInt32(ddlHocSinh.SelectedItem.Value)
                                   && hstl.namhoc_id == id_NamHoc
                                   select hstl).First();
                    tbPhuHuynhXinNghi insert = new tbPhuHuynhXinNghi();
                    //Gắn dữ liệu vào
                    insert.phuhuynhxinnghi_ngaybatdau = Convert.ToDateTime(dteTuNgay.Value);
                    insert.phuhuynhxinnghi_ngayketthuc = Convert.ToDateTime(dteDenNgay.Value);
                    insert.phuhuynhxinnghi_lydo = txtLyDo.Value;
                    insert.hstl_id = checkHS.hstl_id;
                    insert.lop_id = id_Lop;
                    insert.namhoc_id = id_NamHoc;
                    insert.hocki_id = 1;
                    insert.phuhuynhxinnghi_xacnhan = true;
                    insert.username_id = _idUser;
                    insert.phuhuynhxinnghi_ngaydangki = DateTime.Now;
                    //Insert data
                    db.tbPhuHuynhXinNghis.InsertOnSubmit(insert);
                    //Sava data
                    db.SubmitChanges();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Alert", "swal('Đăng ký thành công!','','success').then(function(){grvList.Refresh();})", true);
                    popupControl.ShowOnPageLoad = false;
                }
            }
            else
            {
                tbPhuHuynhXinNghi update = (from xn in db.tbPhuHuynhXinNghis
                                                         where xn.phuhuynhxinnghi_id == Convert.ToInt32(Session["_id"].ToString())
                                                         select xn).SingleOrDefault();
                update.phuhuynhxinnghi_ngaybatdau = Convert.ToDateTime(txtTuNgay.Value);
                update.phuhuynhxinnghi_ngayketthuc = Convert.ToDateTime(txtDenNgay.Value);
                db.SubmitChanges();
                alert.alert_Success(Page, "Cập nhật thành công!", "");
                loadData();
                popupControl.ShowOnPageLoad = false;
                string message = "Giáo viên chủ nhiệm đã cập nhật lại ngày nghỉ của bé. Xem chi tiết tại đây.</a>";
            }
            setNull();
        }
        catch (Exception ex)
        {
            alert.alert_Error(Page, "Đã có lỗi xảy ra, vui lòng liên hệ IT", "");
        }
    }
}