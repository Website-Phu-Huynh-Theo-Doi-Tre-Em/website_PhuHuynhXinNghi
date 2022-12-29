<%@ Page Title="" Language="C#" MasterPageFile="~/Admin_MasterPage.master" AutoEventWireup="true" CodeFile="module_GiaoVienNhanXetThuongXuyen.aspx.cs" Inherits="website_PhuHuynhTheoDoiTre_admin_page_module_GiaoVienNhanXetThuongXuyen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headlink" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="hihead" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="himenu" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="hibodyhead" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="hibodywrapper" runat="Server">
    <link href="/admin_css/css_index/Loading.css" rel="stylesheet" />
    <script src="/js/jquery-1.11.0.min.js"></script>
    <style>
        .table_baogiang {
            /*max-width: 650px;*/
            margin: 0 auto;
        }

            .table_baogiang th, .table_baogiang td {
                text-align: center;
                border: 1px solid #2d3846;
            }

        .input_id_khgd {
            display: none;
        }

        .table_baogiang tr td a {
            display: block;
            text-align: end;
        }

        .table tr.tbheader {
            background: #a3a7a199;
        }
    </style>
    <style>
        .main-omt {
            border: 1px solid #32c5d2;
            background-color: #fff;
        }

            .main-omt .omt-header {
                background-color: #32c5d2;
                padding: 4px 7px;
                display: flex;
            }

        .omt-header .header-title {
            padding: 10px 10px;
            color: white;
        }

        .omt-header .omt__icon {
            font-size: 35px;
            padding: 5px 10px;
            color: white;
        }

        a.hocsinh__active {
            color: #ffffff !important;
            background-color: #FF4444 !important;
            border-color: #FF4444 !important;
        }

        .button__disable {
            cursor: not-allowed;
            pointer-events: none;
        }

        .modal-body {
            padding: unset;
        }

        .modal-dialog {
            max-width: 64rem;
            margin: 4% auto;
        }

        .fade.in {
            background: #80808059;
            opacity: 1;
        }

        .modal .modal-content {
            border-radius: 0;
            background: unset;
            border: none;
        }
    </style>
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <script>
        function DisplayLoadingIcon() {
            $("#img-loading-icon").show();
        }
    </script>
    <div class="loading" id="img-loading-icon" style="display: none">
        <div class="loading">Loading&#8230;</div>
    </div>
    <div class="main-omt">
        <div class="omt-header">
            <i class="fa fa-list-alt omt__icon" aria-hidden="true"></i>
            <h4 class="header-title">Nhận xét sức khỏe</h4>
        </div>
        <div class="omt-top">
            <div class="wrapper__select" style="width: 100%">
                <div style="margin-left: 20px; color: blue;">
                    <span>Lưu ý:</span><br />
                    <span>- Chiều cao, cân nặng của bé đánh số thập phân bởi dấu chấm (.)</span>
                </div>
                <div class="col-3" style="margin-right: 10px;">
                    <asp:DropDownList ID="ddlThang" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-3" style="margin-right: 10px;">
                    <asp:DropDownList ID="ddlLop" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-3">
                    <a href="#" id="btnXem" class="btn btn-primary" onclick="DisplayLoadingIcon()" runat="server" onserverclick="btnXem_ServerClick">Xem</a>
                </div>
            </div>
            <div class="modal fade" id="exampleModalLong" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <%--<div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle">Modal title</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>--%>
                        <div class="modal-body">
                            <embed src="../../file_huongdan/huongdan_sudung/mamnon_sotaysuckhoe.pdf" width="1030" height="650" type="application/pdf" />
                            <%--<iframe src="/uploads/media/default/0001/01/540cb75550adf33f281f29132dddd14fded85bfc.pdf" width="100%" height="500px"></iframe>--%>
                        </div>
                        <%--<div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>--%>
                    </div>
                </div>
            </div>
        </div>

        <div style="display: none">
            <input type="text" id="txtHocSinh" runat="server" />
            <input type="text" id="txtRatting" runat="server" />
        </div>
        <div id="div_KetQua" runat="server">
            <div class="fixed-table-container">
                <table border="1" cellspacing="0" cellpadding="5" class="table table-bordered table_baogiang">
                    <tr class="tbheader">
                        <th rowspan="2">STT</th>
                        <th rowspan="2">Danh sách học sinh</th>
                        <th colspan="2">Nhận xét sức khỏe</th>
                        <th rowspan="2">Ghi chú</th>
                    </tr>
                    <tr class="tbheader">
                        <th>Cân nặng (kg)</th>
                        <th>Chiều cao (cm)</th>
                    </tr>
                    <asp:Repeater ID="rpDanhSachHocSinh" runat="server">
                        <ItemTemplate>
                            <tr>
                                <th>
                                    <%=STT++%>
                                </th>
                                <th>
                                    <%#Eval("hocsinh_fullname")%>
                                </th>
                                <td>
                                    <input type="number" class="form-control" onkeypress="return isNumberKey(event)" id="txtcannang<%#Eval("hocsinh_id")%>" value="<%#Eval("hocsinh_cannang") %>" />
                                </td>
                                <td>
                                    <input type="number" class="form-control" onkeypress="return isNumberKey(event)" id="txtchieucao<%#Eval("hocsinh_id")%>" value="<%#Eval("hocsinh_chieucao") %>" />
                                </td>
                                <td>
                                    <input type="text" class="form-control" id="txtghichu<%#Eval("hocsinh_id")%>" value="<%#Eval("hocsinh_ghichu") %>" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div>
                <a class="btn btn-primary text-white mt-2" id="btnLuuTong" onclick="DisplayLoadingIcon();myContentTong()">Lưu</a>
            </div>
        </div>
        <div style="display: none">
            <input type="text" id="txtDanhSachHocSinhID" runat="server" />
            <input type="text" id="txtHocSinhDau" runat="server" />
            <input type="text" id="txtHocSinhCuoi" runat="server" />
            <input type="text" id="txtCanNangTong" runat="server" />
            <input type="text" id="txtChieuCaoTong" runat="server" />
            <input type="text" id="txtGhiChuTong" runat="server" />
            <a href="#" id="btnHoanThanh" runat="server" onserverclick="btnHoanThanh_ServerClick"></a>
        </div>
        <script>
            function myContentTong() {
                debugger;
                let sodau = document.getElementById("<%=txtHocSinhDau.ClientID%>").value;
                let socuoi = document.getElementById("<%=txtHocSinhCuoi.ClientID%>").value;
                let str_danhsachID = document.getElementById("<%=txtDanhSachHocSinhID.ClientID%>").value;
                let ds_hocsinh = str_danhsachID.split(';');
                //console.log(ds_hocsinh);
                var cannang = [];
                var chieucao = [];
                var ghichu = [];
                for (let i = 0; i < ds_hocsinh.length; i++) {
                    //alert(document.getElementById("txtcannang" + ds_hocsinh[i]).value);
                    //alert(document.getElementById("txtchieucao" + ds_hocsinh[i]).value);

                    var _cannang = (document.getElementById("txtcannang" + ds_hocsinh[i]).value);
                    var _chieucao = (document.getElementById("txtchieucao" + ds_hocsinh[i]).value);
                    var _ghichu = (document.getElementById("txtghichu" + ds_hocsinh[i]).value);
                    cannang.push(_cannang);
                    chieucao.push(_chieucao);
                    ghichu.push(_ghichu);
                }

                document.getElementById("<%=txtCanNangTong.ClientID%>").value = cannang.join(";");
                document.getElementById("<%=txtChieuCaoTong.ClientID%>").value = chieucao.join(";");
                document.getElementById("<%=txtGhiChuTong.ClientID%>").value = ghichu.join(";");
                document.getElementById("<%=btnHoanThanh.ClientID%>").click();
            }
        </script>
    </div>
    <style>
        .main-omt {
            border: 1px solid #32c5d2;
            background-color: #fff;
        }

            .main-omt .omt-header {
                background-color: #32c5d2;
                padding: 4px 7px;
                display: flex;
            }

        .omt-header .header-title {
            font-size: 20px;
            padding: 10px 10px;
            color: white;
        }

        .omt-header .omt__icon {
            font-size: 30px;
            padding: 8px 10px;
            color: white;
        }

        .omt-top {
            display: flex;
            /*padding: 0 20px;*/
        }

            .omt-top .form-omt {
                width: 17%;
                height: 40px !important;
                margin-right: 15px;
                margin-top: 10px;
            }

        .fixed-table-container {
            padding-top: 20px;
        }

        .table-left {
            width: 33.333% !important;
            min-height: 1px;
            padding-left: 15px;
            padding-right: 15px;
        }

        .portlet {
            width: 100% !important;
            border: 1px solid #e7ecf1 !important;
        }

        .portlet-title {
            border-bottom: 1px solid #eef1f5;
            min-height: 48px;
        }

        .caption-title {
            padding: 0;
            display: inline-block;
            margin-left: 5%;
            margin-top: 5px;
            font-size: 25px;
            font-weight: bold;
        }

        .group-tabs ul {
            width: 100%;
        }



            .group-tabs ul li {
                position: relative;
                display: inline-block;
                width: 100%;
                min-height: 20px;
                padding: 10px;
                border-bottom: 1px solid #F4F6F9;
            }

        .click {
            background-color: red;
        }

        .group-tabs ul li:hover {
            background-color: #F4F6F9;
            cursor: pointer;
        }

        .group-tabs ul li a {
            text-decoration: none;
        }

            .group-tabs ul li a:hover {
                text-decoration: none;
            }

        .label {
            font-size: 10px;
            font-weight: 600;
            padding: 1px 3px 1px 3px;
            position: absolute;
            top: 20%;
            right: 0px;
            background-color: #36c6d3;
            color: white;
        }

        .tab-content {
            width: 66.66667% !important;
            min-height: 1px;
            padding-left: 15px;
            padding-right: 15px;
        }

        .table-right {
            width: 100%;
            min-height: 1px;
        }

        .portright {
            width: 100% !important;
            border: 1px solid #e7ecf1 !important;
            margin-bottom: 15px;
        }

        .portright-title {
            border-bottom: 1px solid #eef1f5;
            min-height: 48px;
        }

        .fixed-table-container {
            display: flex;
        }

        .portright-body {
            padding: 10px 20px 20px 20px;
        }

        portright-row {
            margin-left: -15px;
            margin-right: -15px;
        }

        .mt-widget-2 {
            border: 1px solid #e7ecf1;
            position: relative;
        }

        .mt-body {
            padding-left: 15px;
            padding-top: 0px;
        }

        .mt-body-title {
            font-size: 20px;
            margin-top: 15px;
            font-weight: bold;
        }

        .mt-body-title-1 {
        }

        .form-group h3 {
            font-size: 22px;
            margin-top: 20px;
            margin-bottom: 10px;
            font-weight: 300;
        }

        .table-NX {
            vertical-align: top;
        }

        .form-group .table-title p {
            font-size: 0.9rem;
            color: #4f5f6f;
        }

        .table-title {
        }

        .table-raiting {
            position: relative;
            top: -15px;
            margin: 0;
        }

        .wrapper__select {
            margin: 10px;
        }

        /*        @import url(//netdna.bootstrapcdn.com/font-awesome/3.2.1/css/font-awesome.css);*/

        /*        fieldset, label {
            margin: 0;
            padding: 0;
        }

        body {
            margin: 20px;
        }

        h1 {
            font-size: 1.5em;
            margin: 10px;
        }*/

        /****** Style Star Rating Widget *****/

        .rating {
            border: none;
            float: left;
            margin-left: -6px;
        }

            .rating > input {
                display: none;
            }

            .rating > label:before {
                margin: 5px;
                font-size: -1.25em;
                font-family: FontAwesome;
                display: inline-block;
                content: "\f005";
            }

            .rating > .half:before {
                content: "\f089";
                position: absolute;
            }

            .rating > label {
                color: #ddd;
                float: right;
            }

            /***** CSS Magic to Highlight Stars on Hover *****/

            .rating > input:checked ~ label, /* show gold star when clicked */
            .rating:not(:checked) > label:hover, /* hover current star */
            .rating:not(:checked) > label:hover ~ label {
                color: #FFD700;
            }
                /* hover previous stars in list */

                .rating > input:checked + label:hover, /* hover current star when changing rating */
                .rating > input:checked ~ label:hover,
                .rating > label:hover ~ input:checked ~ label, /* lighten current selection */
                .rating > input:checked ~ label:hover ~ label {
                    color: #FFED85;
                }
    </style>
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 47 && charCode < 58)
                return true;
            else {
                return false;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="hibodybottom" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="hifooter" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="hifootersite" runat="Server">
</asp:Content>

