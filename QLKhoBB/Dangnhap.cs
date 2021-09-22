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
    public partial class Dangnhap : DevExpress.XtraEditors.XtraForm
    {
        public Dangnhap()
        {
            InitializeComponent();
        }
        private bool validate()
        {   //hàm kiểm tra dữ liệu nhập vào có rỗng hay k
            if (txtusername.Text == "" || txtpassword.Text == "")
            {
                XtraMessageBox.Show("Bạn phải điền đầy đủ các trường !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        public static string tk = ""; // tạo biến public để truyền tham số của biến username sang các form khác
        private void btn_dn_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (validate())  //kiểm tra dữ liệu k rỗng thì :
                {
                    //khai báo chuỗi lệnh sql
                    string sql = @"select * from NhanVien where username = '" + txtusername.Text + "' and password = '" + txtpassword.Text + "'";
                    DataTable data = ConnectDB.getTable(sql);

                    if (data.Rows.Count <= 0)  //gọi hàm getTable từ lớp DbHelper có giá trị truyền vào là chuỗi lênh select để lấy thông tin từ bảng nếu có số dòng <= 0 thì:
                    {
                        XtraMessageBox.Show("Sai tài khoản hoặc mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else  //nếu số dòng lấy được > 0 thì :
                        if (data.Rows[0]["trangthai"].ToString() == "Active") // kiểm tra dữ liệu cột trạng thái trong sql nếu là Active thì mới được quyền truy cập
                    {
                        Quyen.nhomnd = data.Rows[0]["nhomnd"].ToString();
                        tk = txtusername.Text;
                        this.Visible = false;  //cho form này ẩn đi
                        new Trangchu().ShowDialog();  //hiện form chinh
                    }
                    else
                    {
                        XtraMessageBox.Show("Tài khoản này hiện đang bị khoá. Liên hệ admin để mở khoá tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if(checkEdit1.Checked)
            {
                // kiểm tra nếu chọn nhớ mật khẩu thì ghi lại user và pass không thì để trống
                Properties.Settings.Default.users = txtusername.Text;
                Properties.Settings.Default.pass = txtpassword.Text;
                Properties.Settings.Default.Save();
            }else
            {
                Properties.Settings.Default.users = "";
                Properties.Settings.Default.pass = "";
                Properties.Settings.Default.Save();
            }    
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tb == DialogResult.Yes)
                Application.Exit();
        }

        private void Dangnhap_Load(object sender, EventArgs e)
        {

            DevExpress.LookAndFeel.DefaultLookAndFeel themes = new DevExpress.LookAndFeel.DefaultLookAndFeel(); // cài đặt giao diện mặc định cho form
            themes.LookAndFeel.SkinName = "Xmas 2008 Blue";
            // Load biến mặc định được truyền ở trên lên form 
            txtpassword.Text = Properties.Settings.Default.pass;
            txtusername.Text = Properties.Settings.Default.users;
        }
    }
}