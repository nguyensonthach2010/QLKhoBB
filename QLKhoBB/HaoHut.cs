using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace QLKhoBB
{
    public partial class HaoHut : DevExpress.XtraEditors.XtraForm
    {
        public HaoHut()
        {
            InitializeComponent();
        }

        private void HaoHut_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dong();
        }
        private void Dong()
        {
             string sql = "drop table Tonn";
             ConnectDB.getTable(sql);
        }
        private void HaoHut_Load(object sender, EventArgs e)
        {

        }
        private void LoadData()
        {
            try
            {
                string sql = "select distinct Tonn.MaVT, Tonn.TenVT, Tonn.Kho ,Tonn.Loai, Tonn.MaLoai, Tonn.tongnhhap, Tonn.tongxuat , Tonn.Ton, tontt from Ton full join Tonn on Tonn.MaLoai = Ton.MaVT order by Tonn.Ton desc, tontt desc";
                gridControl1.DataSource = ConnectDB.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dong();
            string sql = "select MaVT, TenVT, MaLoai, Loai,  Kho ,Sum(Nhap) as tongnhhap , SUM(Xuat) as tongxuat, (SUM(Nhap) - SUM(Xuat)) as Ton into Tonn from (select maloai as MaLoai, mavt as MaVT, tenvt as TenVT, loai as Loai, 0 as Nhap, 0 as Xuat, Kho as Kho From VatTu union Select N.maloai as MaLoai, H.mavt as MaVT, H.tenvt as TenVT, H.loai as Loai, Sum(N.slnhap) as Nhap, 0 as Xuat, H.Kho as Kho From NhapKho N, VatTu H Where N.maloai = H.maloai  Group By N.maloai, H.tenvt, H.mavt, H.loai, H.Kho having SUM(N.slnhap) > 0 union Select X.maloai as MaLoai, H.mavt as MaVT, H.tenvt as TenVT, H.loai as Loai, 0 as Nhap, Sum(X.slxuat) as Xuat, H.Kho as Kho  From XuatKho X, VatTu H Where X.maloai = H.maloai Group By X.maloai, H.tenvt, H.mavt, H.loai, H.Kho having SUM(X.slxuat) > 0 ) as hangton Group by Loai, MaLoai , TenVT ,MaVT, Kho";
            ConnectDB.getTable(sql);
            XtraMessageBox.Show("Đã load xong dữ liệu! Bấm kiểm tra hao hụt để load lại chương trình! ");
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                string maloai = gridView1.GetRowCellValue(e.RowHandle, "MaLoai").ToString();
                string tontt = gridView1.GetRowCellValue(e.RowHandle, "tontt").ToString();
                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                { }
                else
                {
                    try
                    {
                        string update = "update Ton set tontt = '" + tontt + "' where MaVT = '"+maloai+"' ";
                        ConnectDB.Query(update);
                        LoadData();
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể kết nối tới CSDL!!");

                    }
                }
            }
            else
            {
                DialogResult tb = XtraMessageBox.Show(sErr, "Lỗi trong quá trình nhập!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (tb == DialogResult.OK)
                {
                    // load lại form
                    LoadData();
                }
            }
        }
    }
}