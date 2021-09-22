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
using System.Data.SqlClient;

namespace QLKhoBB
{
    public partial class Restore : DevExpress.XtraEditors.XtraForm
    {
        public Restore()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // mở đường dẫn và chọn file back up có đuôi .bak
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SQL SEVER database backup file|*.bak";
            dlg.Title = "Phục hồi Database";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textEdit11.Text = dlg.FileName;
                simpleButton2.Enabled = true;
            }
        }
        //kết nối tới chuỗi strCon đã được truyền từ form Connect
        string connString = QLKhoBB.Properties.Settings.Default.strCon;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                //Thực thi câu lệnh để Restore DB
                SqlConnection sqlcon = new SqlConnection(connString);
                //mở chuỗi kết nối
                sqlcon.Open();
                string sql = string.Format("ALTER DATABASE [QLKho] SET SINGLE_USER WITH ROLLBACK IMMEDIATE ");
                //thực thi câu lệnh
                SqlCommand cmd = new SqlCommand(sql, sqlcon);
                cmd.ExecuteNonQuery();
                string sql1 = "Use master restore database [QLKho] from disk ='" + textEdit11.Text + "' with replace;";
                SqlCommand cmd1 = new SqlCommand(sql1, sqlcon);
                cmd1.ExecuteNonQuery();
                string sql3 = string.Format("alter database [QLKho] set MULTI_USER");
                SqlCommand cmd2 = new SqlCommand(sql3, sqlcon);
                cmd2.ExecuteNonQuery();
                DialogResult dr = XtraMessageBox.Show("Phục hồi thành công dữ liệu. Khởi động lại chương trình!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            catch
            {
                XtraMessageBox.Show("Phục hồi dữ liệu không thành công!");
            }
        }
    }
}