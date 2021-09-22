using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLKhoBB
{
    public partial class Trangchu : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Trangchu()
        {
            InitializeComponent();
            string sql1 = "select MaVT, TenVT, MaLoai, Loai,  Kho ,Sum(Nhap) as tongnhhap , SUM(Xuat) as tongxuat, (SUM(Nhap) - SUM(Xuat)) as Ton into Tonn from (select maloai as MaLoai, mavt as MaVT, tenvt as TenVT, loai as Loai, 0 as Nhap, 0 as Xuat, Kho as Kho From VatTu union Select N.maloai as MaLoai, H.mavt as MaVT, H.tenvt as TenVT, H.loai as Loai, Sum(N.slnhap) as Nhap, 0 as Xuat, H.Kho as Kho From NhapKho N, VatTu H Where N.maloai = H.maloai  Group By N.maloai, H.tenvt, H.mavt, H.loai, H.Kho having SUM(N.slnhap) > 0 union Select X.maloai as MaLoai, H.mavt as MaVT, H.tenvt as TenVT, H.loai as Loai, 0 as Nhap, Sum(X.slxuat) as Xuat, H.Kho as Kho  From XuatKho X, VatTu H Where X.maloai = H.maloai Group By X.maloai, H.tenvt, H.mavt, H.loai, H.Kho having SUM(X.slxuat) > 0 ) as hangton Group by Loai, MaLoai , TenVT ,MaVT, Kho";
            ConnectDB.getTable(sql1);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // kiểm tra quyền của tài khoản
            if (Quyen.nhomnd == "Nhập" || Quyen.nhomnd == "Admin")
            {
                NhapKho nk = new NhapKho();
                nk.MdiParent = this;
                nk.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ nhóm tài khoản Nhập và Admin mới có thể Nhập kho!!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // tạo biến nhận dữ liệu của biến tk form đăng nhập
        public static string taikhoan = ""; 
        private void skin()
        {
            // cài đặt giao diện mặc định của form
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.XtraBars.Helpers.SkinHelper.InitSkinGallery(skinRibbonGalleryBarItem1, true);
            DevExpress.LookAndFeel.DefaultLookAndFeel themes = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            themes.LookAndFeel.SkinName = "Xmas 2008 Blue";
        }

        private void Trangchu_Load(object sender, EventArgs e)
        {
            skin();
            // nhận dữ liệu từ biến tk của form đăng nhập và gán 
            taikhoan = Dangnhap.tk; 
            barHeaderItem1.Caption = "Bạn đang đăng nhập với user :" + " " + taikhoan;
            
            HaoHut hh = new HaoHut();
            hh.MdiParent = this;
            hh.Show();
            
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QLvattu vt = new QLvattu();
            vt.MdiParent = this;
            vt.Show();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // kiểm tra quyền của tài khoản
            if (Quyen.nhomnd == "Xuất" || Quyen.nhomnd == "Admin")
            {
                XuatKho xk = new XuatKho();
                xk.MdiParent = this;
                xk.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ nhóm tài khoản Xuất và Admin mới có thể Xuất kho!!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Ton t = new Ton();
            t.MdiParent = this;
            t.Show();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tonki tk = new Tonki();
            tk.MdiParent = this;
            tk.Show();
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm_NhapD nd = new frm_NhapD();
            nd.MdiParent = this;
            nd.Show();
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm_XuatD nd = new frm_XuatD();
            nd.MdiParent = this;
            nd.Show();
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // kiểm tra quyền của tài khoản
            if (Quyen.nhomnd == "Admin")
            {
                QLtaikhoan qltk = new QLtaikhoan();
                qltk.MdiParent = this;
                qltk.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ Admin mới có thể quản lý tài khoản !!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Doimk doi = new Doimk();
            doi.Show();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
            Dangnhap dn = new Dangnhap();
            dn.Show();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // kiểm tra quyền của tài khoản
            if (Quyen.nhomnd == "Admin")
            {
                Restore rss = new Restore();
                rss.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ Admin mới có thể phục hồi dữ liệu !!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // kiểm tra quyền của tài khoản
            if (Quyen.nhomnd == "Admin")
            {
                Backup bk = new Backup();
                bk.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ Admin mới có thể Back up dữ liệu !!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            History hs = new History();
            hs.MdiParent = this;
            hs.Show();
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            NCC ncc = new NCC();
            ncc.Show();
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HaoHut hh = new HaoHut();
            hh.Show();
            string sql = "select MaVT, TenVT, MaLoai, Loai,  Kho ,Sum(Nhap) as tongnhhap , SUM(Xuat) as tongxuat, (SUM(Nhap) - SUM(Xuat)) as Ton into Tonn from (select maloai as MaLoai, mavt as MaVT, tenvt as TenVT, loai as Loai, 0 as Nhap, 0 as Xuat, Kho as Kho From VatTu union Select N.maloai as MaLoai, H.mavt as MaVT, H.tenvt as TenVT, H.loai as Loai, Sum(N.slnhap) as Nhap, 0 as Xuat, H.Kho as Kho From NhapKho N, VatTu H Where N.maloai = H.maloai  Group By N.maloai, H.tenvt, H.mavt, H.loai, H.Kho having SUM(N.slnhap) > 0 union Select X.maloai as MaLoai, H.mavt as MaVT, H.tenvt as TenVT, H.loai as Loai, 0 as Nhap, Sum(X.slxuat) as Xuat, H.Kho as Kho  From XuatKho X, VatTu H Where X.maloai = H.maloai Group By X.maloai, H.tenvt, H.mavt, H.loai, H.Kho having SUM(X.slxuat) > 0 ) as hangton Group by Loai, MaLoai , TenVT ,MaVT, Kho";
            ConnectDB.getTable(sql);
        }

        private void Trangchu_FormClosing(object sender, FormClosingEventArgs e)
        {
            string sql = "drop table Tonn";
            ConnectDB.getTable(sql);
        }
    }
}
