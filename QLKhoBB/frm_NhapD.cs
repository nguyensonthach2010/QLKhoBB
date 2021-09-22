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
    public partial class frm_NhapD : DevExpress.XtraEditors.XtraForm
    {
        public frm_NhapD()
        {
            InitializeComponent();
            // truyền dữ liệu từ biến taikhoan từ form trangchu cho biến userthem
            userthem = Trangchu.taikhoan;
        }
        private void LoadData()
        {
            try
            {
                // đổ dữ liệu vào gridview
                string sql = "SELECT sohd , NhapKho.maloai, mavt, tenvt, loai,Kho ,slnhap,dvt,ngaynhap,NhapKho.manv,NhapKho.mancc,tenncc,ghichu FROM NhapKho INNER JOIN VatTu ON VatTu.maloai = NhapKho.maloai INNER JOIN NhanVien ON NhanVien.manv = NhapKho.manv INNER JOIN NCC ON NCC.mancc = NhapKho.mancc where NhapKho.ngaynhap >='" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy HH:mm:ss") + "' and NhapKho.ngaynhap<='" + Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy HH:mm:ss") + "'";
                gridControl1.DataSource = ConnectDB.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // gọi lại hàm loaddata
            LoadData();
        }
        string userthem = "";
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                //mở câu lệnh kết nối để đổ dữ liệu vào gridview
                string sql3 = "SELECT sohd , NhapKho.maloai, mavt, tenvt, loai,Kho ,slnhap,dvt,ngaynhap,NhapKho.manv,NhapKho.mancc,tenncc,ghichu FROM NhapKho INNER JOIN VatTu ON VatTu.maloai = NhapKho.maloai INNER JOIN NhanVien ON NhanVien.manv = NhapKho.manv INNER JOIN NCC ON NCC.mancc = NhapKho.mancc where NhapKho.ngaynhap >='" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy HH:mm:ss") + "' and NhapKho.ngaynhap<='" + Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy HH:mm:ss") + "'";
                // chọn đường dẫn lưu file và định dạng file
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    // tạo file name 
                    string exportFilePath = saveFileDialogExcel.FileName;
                    // đổ dữ liệu từ câu sql vào grid view
                    gridControl1.DataSource = ConnectDB.getTable(sql3);
                    // lưu file với file name và định dạnh ở trên vào đường dẫn thông qua gridview
                    gridControl1.ExportToXlsx(exportFilePath);
                    XtraMessageBox.Show("Xuất file Excel thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // ghi lại HĐ vào bảng lichsu
                    string sql2 = "insert into LichSu values('" + userthem + "',N'Xuất file Excel các mặt hàng nhập từ ngày "+ Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy HH:mm:ss") + " đến ngày "+ Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy HH:mm:ss") + "','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                    ConnectDB.Query(sql2);
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể Xuất file Excel", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// Tạo STT ở cột ngoài cùng của gridview
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