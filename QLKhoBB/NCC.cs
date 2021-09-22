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
using System.Data.SqlClient;

namespace QLKhoBB
{
    public partial class NCC : DevExpress.XtraEditors.XtraForm
    {
        public NCC()
        {
            InitializeComponent();
        }
        
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            // chuỗi thông báo lỗi
            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "mancc").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "tenncc").ToString() == "")
            {
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form nhập!!";
            }

            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                string mancc = gridView1.GetRowCellValue(e.RowHandle, "mancc").ToString();
                string tenncc = gridView1.GetRowCellValue(e.RowHandle, "tenncc").ToString();
                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {
                        string insert = "insert into NCC values('" + mancc + "',N'" + tenncc + "')";
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
                        string update = "update NCC set tenncc = N'" + tenncc + "' where mancc = '" + mancc + "' ";
                        ConnectDB.Query(update);
                        LoadData();
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
        bool indicatorIcon = true;
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        private void LoadData()
        {
            try
            {
                string sql = "select * from NCC";
                gridControl1.DataSource = ConnectDB.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
            }
        }
        private void NCC_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Xuất file Excel từ gridview sau khi truyền dữ liệu từ câu sql vào gridview
            DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tb == DialogResult.Yes)
            {
                string mancc = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "mancc").ToString();
                try
                {
                    string delete = "delete from NCC where mancc ='" + mancc + "'";
                    ConnectDB.Query(delete);
                    LoadData();
                }
                catch
                {
                    XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
                }
                  //lưu giá trị hiển thị trên gridview vào các biến tương ứng
            }
            else
            {
                LoadData();
            }
        }
    }
}