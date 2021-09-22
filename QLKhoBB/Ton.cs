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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.Drawing;

namespace QLKhoBB
{
    public partial class Ton : DevExpress.XtraEditors.XtraForm
    {
        public Ton()
        {
            InitializeComponent();
        }

        private void Ton_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            // đổ dữ liệu vào gridview
            try
            {
                string sql = "select MaVT, TenVT, MaLoai, Loai , Kho, Sum(Nhap) as tongnhhap , SUM(Xuat) as tongxuat, (SUM(Nhap) - SUM(Xuat)) as Ton from (select maloai as MaLoai, mavt as MaVT, tenvt as TenVT, loai as Loai, Kho, 0 as Nhap, 0 as Xuat From VatTu union Select N.maloai as MaLoai, H.mavt as MaVT, H.tenvt as TenVT, H.loai as Loai, H.Kho as Kho, Sum(N.slnhap) as Nhap, 0 as Xuat  From NhapKho N, VatTu H Where N.maloai = H.maloai  Group By N.maloai, H.tenvt, H.mavt, H.loai, H.Kho having SUM(N.slnhap) > 0 union Select X.maloai as MaLoai, H.mavt as MaVT, H.tenvt as TenVT, H.loai as Loai, H.Kho as Kho, 0 as Nhap, Sum(X.slxuat) as Xuat   From XuatKho X, VatTu H Where X.maloai = H.maloai Group By X.maloai, H.tenvt, H.mavt, H.loai, H.Kho having SUM(X.slxuat) > 0 ) as hangton Group by Loai, MaLoai , TenVT ,MaVT, Kho";
                gridControl1.DataSource = ConnectDB.getTable(sql);

            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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