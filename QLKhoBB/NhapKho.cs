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
    public partial class NhapKho : DevExpress.XtraEditors.XtraForm  
    {
        public NhapKho()
        {
            InitializeComponent();
            // truyền dữ liệu cho biến userthem
            userthem = Trangchu.taikhoan;
        }

        private void NhapKho_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                //load data lên gridview
                string sql = "SELECT id, sohd , NhapKho.maloai, mavt, tenvt, loai,Kho ,slnhap,dvt,ngaynhap,NhapKho.manv,NhapKho.mancc,tenncc,ghichu FROM NhapKho  INNER JOIN VatTu ON VatTu.maloai = NhapKho.maloai INNER JOIN NhanVien ON NhanVien.manv = NhapKho.manv INNER JOIN NCC ON NCC.mancc = NhapKho.mancc order by id desc";
                gridControl1.DataSource = ConnectDB.getTable(sql);
            }catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL!!");
            }
            
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            // kiem tra cell cua mot dong dang Edit xem co rong ko?
            if (gridView1.GetRowCellValue(e.RowHandle, "sohd").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "maloai").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "slnhap").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "dvt").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "ngaynhap").ToString() == ""
                    || gridView1.GetRowCellValue(e.RowHandle, "manv").ToString() == "" || gridView1.GetRowCellValue(e.RowHandle, "mancc").ToString() == "")

            {
                // chuỗi thông báo lỗi
                bVali = false;
                sErr = sErr + "Vui lòng điền đầy đủ thông tin!! Nhấn OK để load lại form nhập!!";
            }

            if (bVali)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                string sohd = gridView1.GetRowCellValue(e.RowHandle, "sohd").ToString();
                string maloai =gridView1.GetRowCellValue(e.RowHandle, "maloai").ToString();
                string slnhap = gridView1.GetRowCellValue(e.RowHandle, "slnhap").ToString();
                string dvt = gridView1.GetRowCellValue(e.RowHandle, "dvt").ToString();
                string ngaynhap = gridView1.GetRowCellValue(e.RowHandle, "ngaynhap").ToString();
                string manv = gridView1.GetRowCellValue(e.RowHandle, "manv").ToString();
                string mancc = gridView1.GetRowCellValue(e.RowHandle, "mancc").ToString();
                string ghichu = gridView1.GetRowCellValue(e.RowHandle, "ghichu").ToString();
                string id = gridView1.GetRowCellValue(e.RowHandle, "id").ToString();
                GridView view = sender as GridView;
                //kiểm tra xem dòng đang chọn có phải dòng mới không nếu đúng thì insert không thì update
                if (view.IsNewItemRow(e.RowHandle))
                {
                    try
                    {
                        string insert = "insert into NhapKho values('" + sohd + "','" + maloai + "','" + slnhap + "','" + dvt + "','" + Convert.ToDateTime(ngaynhap).ToString("MM/dd/yyyy") + "','" + manv + "','" + ghichu + "','" + mancc + "')";
                        ConnectDB.Query(insert);
                        LoadData();
                        // ghi lại HĐ
                        string sql2 = "insert into LichSu values('" + userthem + "',N'Nhập kho mặt hàng [ID]: " + id + " , [SHĐ]: " + sohd + " , [Mã loại] :" + maloai + ", [Số lượng nhập] : " + slnhap + ", [Đơn vị tính] : " + dvt + ",[Ngày nhập] : " + ngaynhap + ", [Người nhập] : " + manv + ", [Nhà cung cấp] : " + mancc + " ,[Ghi chú] : " + ghichu + " ','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                        ConnectDB.Query(sql2);
                    }catch
                    {
                        XtraMessageBox.Show("Không thể kết nối tới CSDL!!");
                    }
                }
                else
                {
                    try
                    {
                        string update = "update NhapKho set maloai = '" + maloai + "', slnhap = '" + slnhap + "', dvt = '" + dvt + "',ngaynhap = '" + Convert.ToDateTime(ngaynhap).ToString("MM/dd/yyyy") + "', manv = '" + manv + "', mancc ='" + mancc + "' ,ghichu='" + ghichu + "', sohd = '" + sohd + "'where id = '"+ id +"'";
                        ConnectDB.Query(update);
                        LoadData();
                        // ghi lại HĐ
                        string sql2 = "insert into LichSu values('" + userthem + "',N'Sửa phiếu nhập kho hàng [ID]: " + id + " , [SHĐ]: " + sohd + " , [Mã loại] :" + maloai + ", [Số lượng nhập] : " + slnhap + ", [Đơn vị tính] : " + dvt + ",[Ngày nhập] : " + ngaynhap + ", [Người nhập] : " + manv + ", [Nhà cung cấp] : " + mancc + " ,[Ghi chú] : " + ghichu + " ','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                        ConnectDB.Query(sql2);
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
                if(tb == DialogResult.OK)
                {
                    // load lại form
                    LoadData();
                }    
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //blinding dữ liệu tương ứng từ cột mã loại khi nhập vào các cell mã vật tư, tên vt, loại từ bảng Vật tư
                GridView view = sender as GridView;
                // nếu cột không có giá trị thì trả về....
                if (view == null) return;
                {
                    switch (e.Column.Caption.ToString())
                    {
                        case "Mã loại":
                            string sql2 = @"select loai,tenvt,mavt,Kho from VatTu where maloai = '" + view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString() + "' ";
                            // thực thi câu lệnh sql thành dạng bảng
                            DataTable tb = ConnectDB.getTable(sql2);
                            // gọi câu lệnh sql để blinding dữ liệu tương ứng từ bảng nhân viên từ user người dùng đăng nhập
                            string sql3 = "select manv from NhanVien where username = '" + userthem + "'";
                            // thực thi câu lệnh sql thành dạng bảng
                            DataTable tb1 = ConnectDB.getTable(sql3);
                            
                            //blinding dvt
                            view.SetRowCellValue(e.RowHandle, view.Columns[6], "");
                            string cellValue4 = "Cái" + view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                            view.SetRowCellValue(e.RowHandle, view.Columns[6], cellValue4);

                            //blinding mavt
                            view.SetRowCellValue(e.RowHandle, view.Columns[2], "");
                            string cellValue = "" + tb.Rows[0]["mavt"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                            view.SetRowCellValue(e.RowHandle, view.Columns[2], cellValue);

                            //blinding tenvt
                            view.SetRowCellValue(e.RowHandle, view.Columns[3], "");
                            string cellValue1 = "" + tb.Rows[0]["tenvt"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                            view.SetRowCellValue(e.RowHandle, view.Columns[3], cellValue1);

                            //blinding loai
                            view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
                            string cellValue2 = "" + tb.Rows[0]["loai"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                            view.SetRowCellValue(e.RowHandle, view.Columns[4], cellValue2);

                            view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                            string cellValue0 = "" + tb.Rows[0]["Kho"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], cellValue0);

                            //blinding manv
                            view.SetRowCellValue(e.RowHandle, view.Columns[8], "");
                            string cellValue3 = "" + tb1.Rows[0]["manv"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[8]).ToString();
                            view.SetRowCellValue(e.RowHandle, view.Columns[8], cellValue3);
                            
                            break;

                        case "NCC":
                            // gọi câu lệnh sql để blinding dữ liệu tương ứng từ bảng ncc
                            string sql4 = "select tenncc from NCC where mancc = '" + view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString() + "'";
                            // thực thi câu lệnh sql thành dạng bảng
                            DataTable tbl = ConnectDB.getTable(sql4);
                            //blinding ncc
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], "");
                            string cellValue6 = "" + tbl.Rows[0]["tenncc"].ToString().Trim() + "" + view.GetRowCellValue(e.RowHandle, view.Columns[12]).ToString();
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], cellValue6);
                            break;
                    }
                }
            }catch
            {
                XtraMessageBox.Show("Vui lòng kiểm tra lại dữ liệu nhập!!");
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tb == DialogResult.Yes)
            {
                //lưu giá trị hiển thị trên gridview vào các biến tương ứng
                string sohd = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                string maloai = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "maloai").ToString();
                string slnhap = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "slnhap").ToString();
                string dvt = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dvt").ToString();
                string ngaynhap = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ngaynhap").ToString();
                string manv = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "manv").ToString();
                string mancc = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "mancc").ToString();
                string ghichu = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ghichu").ToString();
                string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
                try
                {
                    string delete = "delete from NhapKho where id ='" + id + "'";
                    ConnectDB.Query(delete);
                    LoadData();
                    //Lưu lại HĐ
                    string sql2 = "insert into LichSu values('" + userthem + "',N'Xoá phiếu nhập mặt hàng [ID]: " + id + ", [SHĐ]: " + sohd + " , [Mã loại] :" + maloai + ", [Số lượng nhập] : " + slnhap + ", [Đơn vị tính] : " + dvt + ",[Ngày nhập] : " + ngaynhap + ", [Người nhập] : " + manv + ", [Nhà cung cấp] : " + mancc + " ,[Ghi chú] : " + ghichu + " ','" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "')";
                    ConnectDB.Query(sql2);
                }catch
                {
                    XtraMessageBox.Show("Lỗi! Hãy thử lại!");
                    LoadData();
                }
               
            }else
            {
                LoadData();
            }    
        }
        string userthem = "";

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Xuất file Excel từ gridview sau khi truyền dữ liệu từ câu sql vào gridview
            try
            {
                string sql3 = "SELECT id, sohd , NhapKho.maloai, mavt, tenvt, loai,Kho ,slnhap,dvt,ngaynhap,NhapKho.manv,NhapKho.mancc,tenncc,ghichu FROM NhapKho INNER JOIN VatTu ON VatTu.maloai = NhapKho.maloai INNER JOIN NhanVien ON NhanVien.manv = NhapKho.manv INNER JOIN NCC ON NCC.mancc = NhapKho.mancc";
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    gridControl1.DataSource = ConnectDB.getTable(sql3);
                    gridControl1.ExportToXlsx(exportFilePath);
                    XtraMessageBox.Show("Xuất file Excel thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string sql2 = "insert into LichSu values('" + userthem + "',N'Xuất file Excel của các mặt hàng đã nhập kho','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
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
            ImportN im = new ImportN();
            im.Show();
        }

        private void gridView1_InvalidRowException_1(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            // hàm không cho người dùng đổi dòng khi nhập bị lỗi
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            LoadData();
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