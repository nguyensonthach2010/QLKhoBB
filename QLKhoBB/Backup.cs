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
    public partial class Backup : DevExpress.XtraEditors.XtraForm
    {
        public Backup()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //chọn đường dẫn đích lưu file back up khi click vào nút Brownse
            FolderBrowserDialog dlg = new FolderBrowserDialog(); 
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textEdit11.Text = dlg.SelectedPath;
                simpleButton2.Enabled = true;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                // kiểm tra xem đã chọn đường dẫn chưa
                if (textEdit11.Text == string.Empty)
                {
                    XtraMessageBox.Show("Vui lòng chọn đường dẫn lưu file backup", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //thực thi câu lệnh backup sql
                    string sql = "BACKUP DATABASE [QLKho] TO DISK ='" + textEdit11.Text + "\\" + "DATABASE" + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".bak'";  //thực thi câu lệnh backup khi nhấn nút backup
                    ConnectDB.Query(sql);
                    XtraMessageBox.Show("Back up dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    simpleButton2.Enabled = false;
                }
            }
            catch
            {
                XtraMessageBox.Show("Có lỗi xảy ra!. ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Backup_Load(object sender, EventArgs e)
        {

        }
    }
}