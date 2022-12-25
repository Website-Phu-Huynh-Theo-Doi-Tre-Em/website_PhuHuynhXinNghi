using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class website_PhuHuynhTheoDoiTre_admin_page_module_GiaoVienNhanXetThuongXuyen : System.Web.UI.Page
{
    dbcsdlDataContext db = new dbcsdlDataContext();
    public int STT = 1;
    public string hotenhocsinh, hocsinh_image;
    cls_Alert alert = new cls_Alert();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["UserName"] != null)
        {
            var checkNamHoc = (from nh in db.tbHoctap_NamHocs orderby nh.namhoc_id descending select nh).FirstOrDefault();
            if (!IsPostBack)
            {
                div_KetQua.Visible = false;
                var listNV = from l in db.tbLops
                             join gvtl in db.tbGiaoVienTrongLops on l.lop_id equals gvtl.lop_id
                             join gv in db.admin_Users on gvtl.taikhoan_id equals gv.username_id
                             where gv.username_username == Request.Cookies["UserName"].Value
                             && gvtl.namhoc_id == checkNamHoc.namhoc_id
                             select l;
                ddlLop.Items.Clear();
                ddlLop.Items.Insert(0, "Chọn lớp");
                ddlLop.AppendDataBoundItems = true;
                ddlLop.DataTextField = "lop_name";
                ddlLop.DataValueField = "lop_id";
                ddlLop.DataSource = listNV;
                ddlLop.DataBind();
                var listThang = from t in db.tbThangs select t;
                ddlThang.Items.Clear();
                ddlThang.Items.Insert(0, "Chọn tháng");
                ddlThang.AppendDataBoundItems = true;
                ddlThang.DataTextField = "thang_name";
                ddlThang.DataValueField = "thang_id";
                ddlThang.DataSource = listThang;
                ddlThang.DataBind();
            }
        }
        else
        {
            Response.Redirect("/website_PhuHuynhTheoDoiTre/admin_page/admin_Login.aspx");
        }
        //loadData();
    }
    private void loadData()
    {
        if (ddlThang.SelectedValue == "Chọn tháng" || ddlLop.SelectedValue == "Chọn lớp")
        {
            alert.alert_Error(Page, "Vui lòng chọn tháng và lớp để hiển thỉ danh sách học sinh!", "");
        }
        else
        {
            div_KetQua.Visible = true;
            var checkuserid = (from u in db.admin_Users where u.username_username == Request.Cookies["UserName"].Value select u).First();
            var checkNamHoc = (from nh in db.tbHoctap_NamHocs orderby nh.namhoc_id descending select nh).FirstOrDefault();
            // load data đổ vào var danh sách
            //kiểm tra xem tháng đó đã nhận xét chưa, nếu đã nhận xét rồi thì hiện thị lên, nếu chưa thì set rỗng
            if (db.tbThongTinSucKhoes.Any(x => x.suckhoe_thang == ddlThang.SelectedValue
                                                && x.namhoc_id == checkNamHoc.namhoc_id
                                               && x.lop_id == Convert.ToInt32(ddlLop.SelectedValue)))
            {
                var getData = from hs in db.tbHocSinhs
                              join hstl in db.tbHocSinhTrongLops on hs.hocsinh_id equals hstl.hocsinh_id
                              //join hs in db.tbHocSinhs on hstl.hocsinh_id equals hs.hocsinh_id
                              //join t in db.tbHocTap_Tuans on hstl.namhoc_id equals t.namhoc_id
                              //join gvtl in db.tbGiaoVienTrongLops on l.lop_id equals gvtl.lop_id
                              where hstl.lop_id == Convert.ToInt32(ddlLop.SelectedValue)
                              //&& gvtl.namhoc_id == checkNamHoc.namhoc_id
                              && hstl.namhoc_id == checkNamHoc.namhoc_id
                              //&& hstl.hidden == false
                              select new
                              {
                                  STT = STT + 1,
                                  hocsinh_fullname = hs.hocsinh_name,
                                  hstl.hstl_id,
                                  hs.hocsinh_id,
                                  hocsinh_cannang = (from cn in db.tbThongTinSucKhoes
                                                     where cn.hocsinh_id == hs.hocsinh_id && cn.suckhoe_thang == ddlThang.SelectedValue
                                                     //&& cn.lop_id == l.lop_id 
                                                     && cn.namhoc_id == checkNamHoc.namhoc_id
                                                     orderby cn.suckhoe_id descending
                                                     select cn.suckhoe_cannang).FirstOrDefault(),
                                  hocsinh_chieucao = (from cn in db.tbThongTinSucKhoes
                                                      where cn.hocsinh_id == hs.hocsinh_id && cn.suckhoe_thang == ddlThang.SelectedValue
                                                      //&& cn.lop_id == l.lop_id
                                                      && cn.namhoc_id == checkNamHoc.namhoc_id
                                                      orderby cn.suckhoe_id descending
                                                      select cn.suckhoe_chieucao).FirstOrDefault(),
                                  hocsinh_ghichu = (from cn in db.tbThongTinSucKhoes
                                                    where cn.hocsinh_id == hs.hocsinh_id && cn.suckhoe_thang == ddlThang.SelectedValue
                                                    //&& cn.lop_id == l.lop_id
                                                    && cn.namhoc_id == checkNamHoc.namhoc_id
                                                    orderby cn.suckhoe_id descending
                                                    select cn.suckhoe_ghichu).FirstOrDefault(),
                              };
                // đẩy dữ liệu vào gridivew
                rpDanhSachHocSinh.DataSource = getData;
                rpDanhSachHocSinh.DataBind();
                //txtHocSinhDau.Value = getData.FirstOrDefault().hocsinh_id.ToString();
                //txtHocSinhCuoi.Value = getData.OrderByDescending(x => x.hocsinh_id).FirstOrDefault().hocsinh_id.ToString();
                string listId = string.Join(";", getData.Select(x => x.hocsinh_id).ToArray());
                txtDanhSachHocSinhID.Value = listId;
            }
            else
            {
                var getData = from hs in db.tbHocSinhs
                              join hstl in db.tbHocSinhTrongLops on hs.hocsinh_id equals hstl.hocsinh_id
                              //join hs in db.tbHocSinhs on hstl.hocsinh_id equals hs.hocsinh_id
                              where hstl.lop_id == Convert.ToInt32(ddlLop.SelectedValue)
                              //&& hstl.hidden == false 
                              && hstl.namhoc_id == checkNamHoc.namhoc_id
                              //&& hstl.hidden == false
                              select new
                              {
                                  STT = STT + 1,
                                  hocsinh_fullname = hs.hocsinh_name,
                                  hstl.hstl_id,
                                  hs.hocsinh_id,
                                  hocsinh_cannang = "",
                                  hocsinh_chieucao = "",
                                  hocsinh_ghichu = "",
                              };
                // đẩy dữ liệu vào gridivew
                rpDanhSachHocSinh.DataSource = getData;
                rpDanhSachHocSinh.DataBind();
                //txtHocSinhDau.Value = getData.FirstOrDefault().hocsinh_id.ToString();
                //txtHocSinhCuoi.Value = getData.OrderByDescending(x => x.hocsinh_id).FirstOrDefault().hocsinh_id.ToString();
                string listId = string.Join(";", getData.Select(x => x.hocsinh_id).ToArray());
                txtDanhSachHocSinhID.Value = listId;
            }
        }
    }
    protected void btnXem_ServerClick(object sender, EventArgs e)
    {
        loadData();
    }

    protected void btnHoanThanh_ServerClick(object sender, EventArgs e)
    {
        try
        {
            if (txtCanNangTong.Value.Replace(";", "") == "" || txtChieuCaoTong.Value.Replace(";", "") == "")
            {
                alert.alert_Warning(Page, "Vui lòng nhập chiều cao, cân nặng cho các bé!", "");
            }
            else
            {
                db.Connection.Open();
                using (var transaction = db.Connection.BeginTransaction())
                {
                    db.Transaction = transaction;
                    try
                    {
                        string[] arrListCanNang = txtCanNangTong.Value.Split(';');
                        string[] arrListChieuCao = txtChieuCaoTong.Value.Split(';');
                        string[] arrListGhiChu = txtGhiChuTong.Value.Split(';');
                        string[] arrHocSinh = txtDanhSachHocSinhID.Value.Split(';');
                        var checkuserid = (from u in db.admin_Users where u.username_username == Request.Cookies["UserName"].Value select u).First();
                        var checkNamHoc = (from nh in db.tbHoctap_NamHocs orderby nh.namhoc_id descending select nh).First();
                        for (int i = 0; i < arrHocSinh.Length; i++)
                        {
                            //kiểm tra xem hs đó đã được nhận xét chưa. Nếu đã nhận xét rồi thì cập nhật, ngược lại là thêm mới
                            var checkSucKhoe = from sk in db.tbThongTinSucKhoes
                                               where sk.hocsinh_id == Convert.ToInt32(arrHocSinh[i])
                                               && sk.suckhoe_thang == ddlThang.SelectedValue
                                               && sk.lop_id == Convert.ToInt16(ddlLop.SelectedValue)
                                               && sk.namhoc_id == checkNamHoc.namhoc_id
                                               orderby sk.suckhoe_id descending
                                               select sk;
                            if (checkSucKhoe.Count() > 0)
                            {
                                checkSucKhoe.First().suckhoe_chieucao = arrListChieuCao[i].ToString();
                                checkSucKhoe.First().suckhoe_cannang = arrListCanNang[i].ToString();
                                checkSucKhoe.First().suckhoe_ghichu = arrListGhiChu[i].ToString();
                                checkSucKhoe.First().suckhoe_ngay = DateTime.Now;
                                checkSucKhoe.First().username_id = checkuserid.username_id;
                                db.SubmitChanges();
                            }
                            else
                            {
                                tbThongTinSucKhoe insert = new tbThongTinSucKhoe();
                                insert.lop_id = Convert.ToInt16(ddlLop.SelectedValue);
                                insert.suckhoe_thang = ddlThang.SelectedValue;
                                insert.hocsinh_id = Convert.ToInt32(arrHocSinh[i]);
                                insert.suckhoe_chieucao = arrListChieuCao[i].ToString();
                                insert.suckhoe_cannang = arrListCanNang[i].ToString();
                                if (arrListGhiChu.Length == arrListCanNang.Length)
                                    insert.suckhoe_ghichu = arrListGhiChu[i].ToString();
                                insert.suckhoe_ngay = DateTime.Now;
                                insert.username_id = checkuserid.username_id;
                                insert.namhoc_id = checkNamHoc.namhoc_id;
                                insert.suckhoe_tinhtrang = "Đã nhận xét";
                                db.tbThongTinSucKhoes.InsertOnSubmit(insert);
                                db.SubmitChanges();
                            }
                        }
                        alert.alert_Success(Page, "Hoàn thành nhận xét", "");
                        loadData();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            alert.alert_Error(Page, "Lỗi! Vui lòng liên hệ IT", "");
        }
    }
}