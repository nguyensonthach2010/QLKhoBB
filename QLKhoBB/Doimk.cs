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

namespace QLKhoBB
{
    public partial class Doimk : DevExpress.XtraEditors.XtraForm
    {
        public Doimk()
        {
            InitializeComponent();
        }
        private bool validate()
        {
            // hàm kiểm tra các textbox có rỗng hay không
            if (txtpassht.Text == "" || txtpassmoi.Text == "" || txtxacnhan.Text == "")
            {
                XtraMessageBox.Show("Bạn phải điền đầy đủ các trường !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void bnt_doimk_Click(object sender, EventArgs e)
        {
            if(validate())
            {
                try
                {
                    // lọc Nhân viên có username và password nhập vào
                    string sql = @"select * from NhanVien where username = '" + user + "' and password = '" + txtpassht.Text + "'";  
                    DataTable data = ConnectDB.getTable(sql);
                    // kiểm tra oass hiện tại
                    if (data.Rows.Count <= 0)
                    {
                        XtraMessageBox.Show("Mật khẩu hiện tại sai !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    // kiểm tra người dùng nhập mã mới có trùng với mã xác nhận không
                    if (txtpassmoi.Text != txtxacnhan.Text) 
                    {
                        XtraMessageBox.Show("Xác nhận lại mật khẩu mới không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // cập nhật mật khẩu mới
                        string sql1 = @"update NhanVien set password='" + txtpassmoi.Text + "' where username= '" + user + "'"; 
                        if (ConnectDB.Query(sql1) == -1)
                        {
                            XtraMessageBox.Show("Đổi mật khẩu không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            XtraMessageBox.Show("Đổi mật khẩu thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                }
                catch
                {
                    XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string user = "";

        private void Doimk_Load(object sender, EventArgs e)
        {
            // tạo biến user để nhận dữ liệu từ biến public taikhoan từ form chính, gán dữ liệu cho textbox txtuser
            user = Trangchu.taikhoan; 
            txtuser.Text = user;
        }
    }
}