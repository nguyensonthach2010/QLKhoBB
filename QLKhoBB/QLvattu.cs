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
    public partial class QLvattu : DevExpress.XtraEditors.XtraForm
    {
        public QLvattu()
        {
            InitializeComponent();
        }

        private void QLvattu_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                //load data lên gridview
                string sql = "select * from VatTu order by maloai desc";
                gridControl1.DataSource = ConnectDB.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tb == DialogResult.Yes)
            {
                try
                {
                    //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                    string maloai = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                    string delete = "delete from VatTu where maloai ='"+maloai+"'";
                    ConnectDB.Query(delete);
                    LoadData();
                }catch(Exception ex)
                {
                    XtraMessageBox.Show(ex.Message);
                }
                
            }
            else
            {
                LoadData();
            }
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {

            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "mavt").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "maloai").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "tenvt").ToString() == "" )
            {
                // chuỗi thông báo lỗi
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form nhập!!";
            }

            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                string mavt = gridView1.GetRowCellValue(e.RowHandle, "mavt").ToString();
                string maloai = gridView1.GetRowCellValue(e.RowHandle, "maloai").ToString();
                string tenvt = gridView1.GetRowCellValue(e.RowHandle, "tenvt").ToString();
                string loai = gridView1.GetRowCellValue(e.RowHandle, "loai").ToString();
                string Kho = gridView1.GetRowCellValue(e.RowHandle, "Kho").ToString();

                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {
                        string insert = "insert into VatTu values('" + maloai + "','" + mavt + "',N'" + tenvt + "',N'" + loai + "',N'" + Kho + "')";
                        ConnectDB.Query(insert);
                        LoadData();
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
                    }
                }
                else
                {
                    try
                    {
                        string update = "update VatTu set mavt = '" + mavt + "', tenvt = N'" + tenvt + "', loai = N'" + loai + "' where maloai = '" + maloai + "' ";
                        ConnectDB.Query(update);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
                    }
                }
            }
            else
            {
                DialogResult tb = XtraMessageBox.Show(sErr, "Lỗi trong quá trình nhập!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (tb == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }
        /// <summary>
        /// Tạo cột STT bên cột ngoài cùng của gridview
        /// </summary>
        bool indicatorIcon = true;

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());
            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + GridPainter.Indicator.ImageSize.Width + 10;
        }
        
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

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //blinding dữ liệu tương ứng từ cột mã loại khi nhập vào các cell mã vật tư, tên vt, loại từ bảng Vật tư
            GridView view = sender as GridView;
            // nếu cột không có giá trị thì trả về....
            if (view == null) return;
            // nếu tên cột chọn khác Mã loại thì trả về....
            if (e.Column.Caption != "Mã loại") return;

            //blinding mavt
            view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
            string cellValue = "K2" + view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
            view.SetRowCellValue(e.RowHandle, view.Columns[4], cellValue);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Xuất file Excel từ gridview sau khi truyền dữ liệu từ câu sql vào gridview
            try
            {
                string sql3 = "select * from VatTu";
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    gridControl1.DataSource = ConnectDB.getTable(sql3);
                    gridControl1.ExportToXlsx(exportFilePath);
                    XtraMessageBox.Show("Xuất file Excel thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể Xuất file Excel", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}