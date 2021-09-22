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
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid;

namespace QLKhoBB
{
    public partial class Tonki : DevExpress.XtraEditors.XtraForm
    {
        public Tonki()
        {
            InitializeComponent();
            userthem = Trangchu.taikhoan;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                // đổ dữ liệu vào gridview
                string sql = "SELECT Tonct.maloai, Tonct.tenvt , Tonct.mavt, Tonct.loai, sum(Tonct.Tondk) AS TonDau, sum(Tonct.Nhaptk) AS Nhap, sum(Tonct.Xuattk) AS Xuat, (sum(Tonct.Tondk)+sum(Tonct.Nhaptk)- sum(Tonct.Xuattk)) AS TonCuoi  FROM(Select dk.maloai, dk.tenvt, dk.mavt, dk.loai, Tondk, 0 as Nhaptk, 0 as Xuattk  From (Select a.maloai, a.tenvt, a.mavt, a.loai, (Sum(a.Nhap) - Sum(a.Xuat)) AS Tondk  From (Select N.maloai, H.tenvt, H.mavt, H.loai, Sum(N.slnhap) as Nhap, 0 as Xuat  From NhapKho N, VatTu H Where N.maloai = H.maloai and N.ngaynhap < '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy") + "'  Group By N.maloai, H.tenvt, H.mavt, H.loai UNION (Select X.maloai, H.tenvt, H.mavt, H.loai, 0 as Nhap, sum(X.slxuat) as Xuat From XuatKho X, VatTu H Where X.maloai = H.maloai and X.ngayxuat < '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy") + "' Group By X.maloai, H.tenvt, H.mavt, H.loai)) a GROUP BY a.maloai, a.tenvt, a.mavt, a.loai HAVING(Sum(a.Nhap - a.Xuat)) <> 0) dk Union Select mavt, tenvt, mavt, loai, 0 as Tondk, 0 as Nhaptk, 0 as Xuattk From VatTu Union Select N.maloai, H.tenvt, H.mavt, H.loai, 0 as Tondk, Sum(N.slnhap) as Nhaptk, 0 as Xuattk  From NhapKho N, VatTu H Where N.maloai = H.maloai and N.ngaynhap >= '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy") + "' and N.ngaynhap <= '" + Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy") + "'  Group By N.maloai, H.tenvt, H.mavt, H.loai Union Select X.maloai, H.tenvt, H.mavt, H.loai, 0 as Tondk, 0 as Nhaptk, sum(X.slxuat) as Xuattk  From XuatKho X, VatTu H Where X.maloai = H.maloai and X.ngayxuat >= '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy") + "' and X.ngayxuat <= '" + Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy") + "'  Group By X.maloai, H.tenvt, H.mavt, H.loai )  AS Tonct GROUP BY Tonct.maloai, Tonct.tenvt, Tonct.mavt, Tonct.loai HAVING(sum(Tonct.Tondk) + sum(Tonct.Nhaptk) - sum(Tonct.Xuattk)) <> 0";
                gridControl1.DataSource = ConnectDB.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
            }
        }
        string userthem = "";

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //X//Xuất file Excel từ gridview sau khi truyền dữ liệu từ câu sql vào gridview
            try
            {
                string sql3 = "SELECT Tonct.maloai, Tonct.tenvt , Tonct.mavt, Tonct.loai, sum(Tonct.Tondk) AS TonDau, sum(Tonct.Nhaptk) AS Nhap, sum(Tonct.Xuattk) AS Xuat, (sum(Tonct.Tondk)+sum(Tonct.Nhaptk)- sum(Tonct.Xuattk)) AS TonCuoi  FROM(Select dk.maloai, dk.tenvt, dk.mavt, dk.loai, Tondk, 0 as Nhaptk, 0 as Xuattk  From (Select a.maloai, a.tenvt, a.mavt, a.loai, (Sum(a.Nhap) - Sum(a.Xuat)) AS Tondk  From (Select N.maloai, H.tenvt, H.mavt, H.loai, Sum(N.slnhap) as Nhap, 0 as Xuat  From NhapKho N, VatTu H Where N.maloai = H.maloai and N.ngaynhap < '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy") + "'  Group By N.maloai, H.tenvt, H.mavt, H.loai UNION (Select X.maloai, H.tenvt, H.mavt, H.loai, 0 as Nhap, sum(X.slxuat) as Xuat From XuatKho X, VatTu H Where X.maloai = H.maloai and X.ngayxuat < '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy") + "' Group By X.maloai, H.tenvt, H.mavt, H.loai)) a GROUP BY a.maloai, a.tenvt, a.mavt, a.loai HAVING(Sum(a.Nhap - a.Xuat)) <> 0) dk Union Select mavt, tenvt, mavt, loai, 0 as Tondk, 0 as Nhaptk, 0 as Xuattk From VatTu Union Select N.maloai, H.tenvt, H.mavt, H.loai, 0 as Tondk, Sum(N.slnhap) as Nhaptk, 0 as Xuattk  From NhapKho N, VatTu H Where N.maloai = H.maloai and N.ngaynhap >= '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy") + "' and N.ngaynhap <= '" + Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy") + "'  Group By N.maloai, H.tenvt, H.mavt, H.loai Union Select X.maloai, H.tenvt, H.mavt, H.loai, 0 as Tondk, 0 as Nhaptk, sum(X.slxuat) as Xuattk  From XuatKho X, VatTu H Where X.maloai = H.maloai and X.ngayxuat >= '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy") + "' and X.ngayxuat <= '" + Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy") + "'  Group By X.maloai, H.tenvt, H.mavt, H.loai )  AS Tonct GROUP BY Tonct.maloai, Tonct.tenvt, Tonct.mavt, Tonct.loai HAVING(sum(Tonct.Tondk) + sum(Tonct.Nhaptk) - sum(Tonct.Xuattk)) <> 0";
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    gridControl1.DataSource = ConnectDB.getTable(sql3);
                    gridControl1.ExportToXlsx(exportFilePath);
                    XtraMessageBox.Show("Xuất file Excel thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    string sql2 = "insert into LichSu values('" + userthem + "',N'Xuất file Excel tồn kì từ ngày "+ Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy") + " đến ngày "+ Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy") + " của các mặt hàng','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                    ConnectDB.Query(sql2);
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể Xuất file Excel", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// Tạo cột STT bên cột ngoài cùng của gridview
        /// </summary>
        bool indicatorIcon = true;

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    string sText = (e.RowHandle + 1).ToString();
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString(sText, e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = sText;
                }
                if (!indicatorIcon)
                    e.Info.ImageIndex = -1;

                if (e.RowHandle == GridControl.InvalidRowHandle)
                {
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString("STT", e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = "STT";
                }
            }
            catch
            {
                XtraMessageBox.Show("Lỗi cột STT");
            }
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());
            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + GridPainter.Indicator.ImageSize.Width + 10;
        }
    }
}