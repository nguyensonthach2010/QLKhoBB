using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;

namespace QLKhoBB
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                ConnectDB.connect.Open();
                ConnectDB.connect.Close();
                Application.Run(new Dangnhap());
            }
            catch
            {
                Application.Run(new Connect());
            }
        }
    }
}
