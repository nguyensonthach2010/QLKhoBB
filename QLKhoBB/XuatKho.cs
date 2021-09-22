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
    public partial class XuatKho : DevExpress.XtraEditors.XtraForm
    {
        public XuatKho()
        {
            InitializeComponent();
            // truyền dữ liệu cho biến userthem
            userthem = Trangchu.taikhoan;
        }
        string userthem = "";
        private void LoadData()
        {
            try
            {
                //load data lên gridview
                string sql = "SELECT id, sohd , XuatKho.maloai, mavt, tenvt, loai,Kho ,slxuat,dvt,ngayxuat,XuatKho.manv,dvgn,ghichu FROM XuatKho INNER JOIN VatTu ON VatTu.maloai = XuatKho.maloai INNER JOIN NhanVien ON NhanVien.manv = XuatKho.manv order by id desc";
                gridControl1.DataSource = ConnectDB.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
            }
        }

        private void XuatKho_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "sohd").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "maloai").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "slxuat").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "dvt").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "ngayxuat").ToString() == ""
                    || gridView1.GetRowCellValue(e.RowHandle, "manv").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "dvgn").ToString() == "")

            {
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form Xuất!!";
            }

            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                string sohd = gridView1.GetRowCellValue(e.RowHandle, "sohd").ToString();
                string maloai = gridView1.GetRowCellValue(e.RowHandle, "maloai").ToString();
                string slxuat = gridView1.GetRowCellValue(e.RowHandle, "slxuat").ToString();
                string dvt = gridView1.GetRowCellValue(e.RowHandle, "dvt").ToString();
                string ngayxuat = gridView1.GetRowCellValue(e.RowHandle, "ngayxuat").ToString();
                string manv = gridView1.GetRowCellValue(e.RowHandle, "manv").ToString();
                string dvgn = gridView1.GetRowCellValue(e.RowHandle, "dvgn").ToString();
                string ghichu = gridView1.GetRowCellValue(e.RowHandle, "ghichu").ToString();
                string id = gridView1.GetRowCellValue(e.RowHandle, "id").ToString();

                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {
                        string insert = "insert into XuatKho values('" + sohd + "','" + maloai + "','" + slxuat + "','" + dvt + "','" + Convert.ToDateTime(ngayxuat).ToString("MM/dd/yyyy") + "','" + manv + "',N'" + ghichu + "',N'" + dvgn + "')";
                        ConnectDB.Query(insert);
                        LoadData();
                        // ghi lại HĐ
                        string sql2 = "insert into LichSu values('" + userthem + "',N'Xuất mặt hàng [ID]: " + id + ", [SHĐ]: " + sohd + " , [Mã loại] :" + maloai + ", [Số lượng Xuất] : " + slxuat + ", [Đơn vị tính] : N" + dvt + ",[Ngày Xuất] : " + ngayxuat + ", [Người Xuất] : " + manv + ", [Người nhận] : N" + dvgn + " ,[Ghi chú] : N" + ghichu + " ','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                        ConnectDB.Query(sql2);
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
                        string update = "update XuatKho set maloai = '" + maloai + "', slxuat = '" + slxuat + "', dvt = '" + dvt + "',ngayxuat = '" + Convert.ToDateTime(ngayxuat).ToString("MM/dd/yyyy") + "', manv = '" + manv + "', dvgn =N'" + dvgn + "' ,ghichu=N'" + ghichu + "', sohd = '" + sohd + "'where id = '" +id+ "' ";
                        ConnectDB.Query(update);
                        LoadData();

                        // ghi lại HĐ
                        string sql2 = "insert into LichSu values('" + userthem + "',N'Sửa phiếu Xuất kho hàng [ID]: "+ id +" ,[SHĐ]: " + sohd + " , [Mã loại] :" + maloai + ", [Số lượng Xuất] : " + slxuat + ", [Đơn vị tính] : " + dvt + ",[Ngày Xuất] : " + ngayxuat + ", [Người Xuất] : " + manv + ", [Người nhận] : " + dvgn + " ,[Ghi chú] : " + ghichu + " ','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                        ConnectDB.Query(sql2);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
                    }
                }
            }
            else
            {
                DialogResult tb = XtraMessageBox.Show(sErr, "Lỗi trong quá trình Xuất!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (tb == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //blinding dữ liệu tương ứng từ cột mã loại khi Xuất vào các cell mã vật tư, tên vt, loại từ bảng Vật tư
                GridView view = sender as GridView;
                // nếu cột không có giá trị thì trả về....
                if (view == null) return;

                // nếu tên cột chọn khác Mã loại thì trả về....
                if (e.Column.Caption != "Mã loại") return;

                // gọi câu lệnh sql để blinding dữ liệu tương ứng từ bảng vật tư khi nhập mã loại
                string sql2 = @"select loai,tenvt,mavt,Kho from VatTu where maloai = '" + view.GetRowCellValue(e.RowHandle, "maloai").ToString() + "' ";
                DataTable tb = ConnectDB.getTable(sql2);

                // gọi câu lệnh sql để blinding dữ liệu tương ứng từ bảng nhân viên từ user người dùng đăng nhập
                string sql3 = "select manv from NhanVien where username = '" + userthem + "'";
                DataTable tb1 = ConnectDB.getTable(sql3);

                //blinding dvt
                view.SetRowCellValue(e.RowHandle, "dvt", "");
                string cellValue4 = "Cái" + view.GetRowCellValue(e.RowHandle, "dvt").ToString();
                view.SetRowCellValue(e.RowHandle, "dvt", cellValue4);

                //blinding mavt
                view.SetRowCellValue(e.RowHandle, "mavt", "");
                string cellValue = "" + tb.Rows[0]["mavt"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, "mavt").ToString();
                view.SetRowCellValue(e.RowHandle, "mavt", cellValue);

                //blinding tenvt
                view.SetRowCellValue(e.RowHandle, "tenvt", "");
                string cellValue1 = "" + tb.Rows[0]["tenvt"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, "tenvt").ToString();
                view.SetRowCellValue(e.RowHandle, "tenvt", cellValue1);

                //blinding loai
                view.SetRowCellValue(e.RowHandle, "loai", "");
                string cellValue2 = "" + tb.Rows[0]["loai"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, "loai").ToString();
                view.SetRowCellValue(e.RowHandle, "loai", cellValue2);

                view.SetRowCellValue(e.RowHandle, "Kho", "");
                string cellValue0 = "" + tb.Rows[0]["Kho"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, "Kho").ToString();
                view.SetRowCellValue(e.RowHandle, "Kho", cellValue0);

                //blinding manv
                view.SetRowCellValue(e.RowHandle, "manv", "");
                string cellValue3 = "" + tb1.Rows[0]["manv"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, "manv").ToString();
                view.SetRowCellValue(e.RowHandle, "manv", cellValue3);
            }catch
            {
                XtraMessageBox.Show("Không tìm thấy vật tư có mã loại bạn nhập!!");
            }
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //lưu giá trị hiển thị trên gridview vào các biến tương ứng
            string sohd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "sohd").ToString();
            string maloai = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "maloai").ToString();
            string slxuat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "slxuat").ToString();
            string dvt = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dvt").ToString();
            string ngayxuat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ngayxuat").ToString();
            string manv = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "manv").ToString();
            string dvgn = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dvgn").ToString();
            string ghichu = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ghichu").ToString();
            string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();

            DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tb == DialogResult.Yes)
            {
                try
                {
                    string delete = "delete from XuatKho where id ='" + id + "'";
                    ConnectDB.Query(delete);
                    LoadData();
                    //Lưu lại HĐ
                    string sql2 = "insert into LichSu values('" + userthem + "',N'Xoá phiếu xuất mặt hàng [ID]: " + id + " ,[SHĐ]: " + sohd + " , [Mã loại] :" + maloai + ", [Số lượng xuất] : " + slxuat + ", [Đơn vị tính] : " + dvt + ",[Ngày xuất] : " + ngayxuat + ", [Người xuất] : " + manv + ", [Người nhận] : " + dvgn + " ,[Ghi chú] : " + ghichu + " ','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                    ConnectDB.Query(sql2);
                }
                catch
                {
                    XtraMessageBox.Show("Không thế kết nối tới CSDL!!");
                }
            }
            else
            {
                LoadData();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Xuất file Excel từ gridview sau khi truyền dữ liệu từ câu sql vào gridview
            try
            {
                string sql3 = "SELECT id, sohd , XuatKho.maloai, mavt, tenvt, loai,Kho ,slxuat,dvt,ngayxuat,XuatKho.manv,dvgn,ghichu FROM XuatKho INNER JOIN VatTu ON VatTu.maloai = XuatKho.maloai INNER JOIN NhanVien ON NhanVien.manv = XuatKho.manv";
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    gridControl1.DataSource = ConnectDB.getTable(sql3);
                    gridControl1.ExportToXlsx(exportFilePath);
                    XtraMessageBox.Show("Xuất file Excel thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string sql2 = "insert into LichSu values('" + userthem + "',N'Xuất file Excel của các mặt hàng đã xuất kho','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                    ConnectDB.Query(sql2);
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể Xuất file Excel", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            ImportX im = new ImportX();
            im.Show();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            // hàm không cho người dùng đổi dòng khi nhập bị lỗi
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
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