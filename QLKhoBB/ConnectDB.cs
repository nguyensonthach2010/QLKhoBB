using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKhoBB
{
    static class ConnectDB
    {
        //tạo biến kết nối
        public static SqlConnection connect = new SqlConnection(QLKhoBB.Properties.Settings.Default.strCon);


        //hàm mở kết nối database

        public static SqlConnection Open()
        {
            connect = new SqlConnection(QLKhoBB.Properties.Settings.Default.strCon); //khai báo một biến kết nối mới với giá trị truyền vào là Chuỗi connString
            try
            {
                connect.Open();  //mở kết nối
            }
            catch (Exception ex)
            {  //nếu không kết nối được
                XtraMessageBox.Show(ex.Message.ToString());   //hiện thông báo lỗi ngoại lê
            }
            return connect; //trả về biến kết nối
        }

        //hàm lấy tất cả dữ liệu từ 1 hoặc nhiều bảng trong CSDL trả về là dữ liệu dạng bảng (DataTable)
        public static DataTable getTable(string sql)
        {
            DataTable result = new DataTable();   //tạo mới 1 biến lưu dữ liệu dạng bảng
            SqlCommand cmd = new SqlCommand(sql, Open());  //tạo biến xử lý lệnh thao tác với CSDL, với giá trị truyền vào là chuỗi sql và biến kết nối (connect được lấy ra từ hàm Open())
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();  //tạo một biến trung gian lưu dữ liệu
                da.SelectCommand = cmd;
                da.Fill(result);  //làm đầy biến result
            }
            catch
            {
                XtraMessageBox.Show("Đóng chương trình!", "Thông báo", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            return result; //trả về biến result
        }
        public static int Query(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Open());  //tạo đối tượng xử các lệnh sql
            try
            {
                return cmd.ExecuteNonQuery();   //xử lý lệnh sql trả về giá trị là số dòng bị ảnh hưởng >= 0
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_"))
                {
                    XtraMessageBox.Show("Dữ liệu bạn đang xóa đang được sử dụng ở bảng khác, hãy kiểm tra lại !");
                }
                else
                    XtraMessageBox.Show(ex.Message.ToString());
            }
            return -1;
        }
    }
}
