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
using ExcelDataReader;
using System.IO;
using Z.Dapper.Plus;
using System.Data.SqlClient;

namespace QLKhoBB
{
    public partial class ImportN : DevExpress.XtraEditors.XtraForm
    {
        public ImportN()
        {
            InitializeComponent();
            // truyền dữ liệu từ biến tk ở form dangnhap vào biến userthem
            userthem = Dangnhap.tk;
        }
        string userthem = "";
        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //tạo datatable từ combox sau khi chọn sheet sau khi mở được file excel
                DataTable dt = tableCollection[comboBoxEdit1.SelectedItem.ToString()];
                if (dt != null)
                {
                    //tạo List
                    List<Nhap> nhap = new List<Nhap>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // gọi lại lớp nhap
                        Nhap nhap1 = new Nhap();
                        // truyền dữ liệu cho các biến trong class nhap thông qua tên các cột trong excel
                        nhap1.sohd = dt.Rows[i]["Số HĐ"].ToString();
                        nhap1.maloai = dt.Rows[i]["Mã loại"].ToString();
                        nhap1.slnhap = dt.Rows[i]["Số lượng nhập"].ToString();
                        nhap1.dvt = dt.Rows[i]["Đơn vị tính"].ToString();
                        // ép kiểu cột ngày nhập về dạng datetime 
                        nhap1.ngaynhap = Convert.ToDateTime(dt.Rows[i]["Ngày nhập"].ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                        nhap1.manv = dt.Rows[i]["Người nhập"].ToString();
                        nhap1.mancc = dt.Rows[i]["NCC"].ToString();
                        nhap1.ghichu = dt.Rows[i]["Ghi chú"].ToString();
                        //đổ dữ liệu vào List từ lớp nhap vừa được truyền dữ liệu
                        nhap.Add(nhap1);
                    }
                    //gắn datasource cho datagridview
                    nhapBindingSource1.DataSource = nhap;
                }
            }
            catch
            {
                XtraMessageBox.Show("Vui lòng kiểm tra lại định dạng hoặc các trường của file excel", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        DataTableCollection tableCollection;
        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            // Mở tab Explorer để chọn nơi có file excel cần import
            try
            {
                // hiển thị các file đuôi xlsx, xls
                using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Workbook(*.xlsx;*.xls)|*.xlsx;*.xls" })
                {
                    // nếu click ok sau khi chọn file
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //lấy filename đã chọn
                        textEdit1.Text = openFileDialog.FileName;
                        //mở file 
                        using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            //đọc file
                            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                // tạo dataset từ file excel
                                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                });
                                tableCollection = result.Tables;
                                // làm sạch combobox
                                comboBoxEdit1.Properties.Items.Clear();
                                //tạo data table từ dữ liệu của sheet được chọn
                                foreach (DataTable table in tableCollection)
                                    // hiển thị sheet
                                    comboBoxEdit1.Properties.Items.Add(table.TableName);
                            }
                        }
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("Có lỗi xảy ra! ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //gọi chuỗi kết nối tới database
                string conn = QLKhoBB.Properties.Settings.Default.strCon;
                //gọi bảng trong database
                DapperPlusManager.Entity<Nhap>().Table("NhapKho");
                // tạo 1 list nhận dữ liệu từ datagridview
                List<Nhap> nhaps = nhapBindingSource1.DataSource as List<Nhap>;
                if (nhaps != null)
                {
                    //nếu list nhap không trống thì truyền dữ liệu vào database
                    using (IDbConnection db = new SqlConnection(conn))
                    {
                        db.BulkInsert(nhaps);
                    }
                }
                DialogResult tb = XtraMessageBox.Show("Import thành công!! Hãy load lại dữ liệu Nhập kho !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (tb == DialogResult.OK)
                {
                    this.Close();
                }
                //Ghi lại HĐ
                string sql2 = "insert into LichSu values('" + userthem + "',N'Import file Excel vào nhập kho','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                ConnectDB.Query(sql2);
            }
            catch
            {
                XtraMessageBox.Show("Có lỗi xảy ra!. Không thể Import vào CSDL!. Lưu ý thoát file excel trước khi import và số hóa đơn không được trùng trong CSDL!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}