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
using System.Data.Sql;
using System.Data.SqlClient;

namespace QLKhoBB
{
    public partial class Connect : DevExpress.XtraEditors.XtraForm
    {
        public Connect()
        {
            InitializeComponent();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            // kiểm tra check box nếu được tích thì mở user và pass
            if(checkEdit1.Checked == true)
            {
                txttk.Enabled = true;
                txtmk.Enabled = true;
            }
            else
            {
                txttk.Enabled = false;
                txtmk.Enabled = false;
            }    
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // tạo biến connString để connect
            string connString = "";
            if(!checkEdit1.Checked)
            {
                // nếu người dùng không tích check box thì truy cập vào database local
                connString = "Data Source= " + txtsvname.Text + ";Initial Catalog=QLKho; Integrated Security = True";
            }
            else
            {
                // nếu người dùng tích check box thì truy cập vào database với user và pass được nhập
                connString = "Data Source= " + txtsvname.Text + ";Initial Catalog=QLKho;User ID= " + txttk.Text + ";Password= " + txtmk.Text + " ";
            }
            SqlConnection connection = new SqlConnection(connString);
            try
            {
                //mở chuỗ kết nối 
                connection.Open();
                // gán chuỗi kết nối ở trên cho biến strCon và lưu lại (biến connect mặc định) 
                QLKhoBB.Properties.Settings.Default.strCon = connString;
                QLKhoBB.Properties.Settings.Default.Save();
                XtraMessageBox.Show("Kết nối thành công! Hãy khởi động lại chương trình !!");
                Application.Exit();
            }
            catch
            {
                XtraMessageBox.Show("Kết nối thất bại! Hãy kiểm tra lại thông tin Sever !!");
                //đóng chuỗi kết nối nêu kết nối thất bại
                connection.Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}