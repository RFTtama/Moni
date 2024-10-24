using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Moni
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
# if !DEBUG
            Mutex mutex = new Mutex(false, "MoniResourcesMonitor");
# endif
# if DEBUG
            Mutex mutex = new Mutex(false, "MoniResourcesMonitorDebug");
# endif

            bool hasHandle = false;

            try
            {
                try
                {
                    hasHandle = mutex.WaitOne(0, false);
                }
                catch (System.Threading.AbandonedMutexException)
                {
                    hasHandle = true;
                }
                if (hasHandle == false)
                {
                    MessageBox.Show("Moniはすでに実行されています", "Moni実行エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Splash.ShowSplash();
                Application.Run(new Clock());
                GC.Collect();
            }
            finally
            {
                if (hasHandle)
                {
                    mutex.ReleaseMutex();
                }
                mutex.Close();
            }
        }
    }

    public static class Splash
    {
        private static Form startForm = new Form();

        public static void ShowSplash()
        {
            Label newLabel = new Label();
            newLabel.Text = "Moni> Developed -by @RFT\r\nLaunching...\r\nPlease wait...";

            newLabel.Font = new Font("Lucida Console", 12);
            newLabel.AutoSize = false;
            newLabel.ForeColor = Color.FromArgb(0, 255, 0);
            newLabel.TextAlign = ContentAlignment.TopLeft;
            newLabel.Dock = DockStyle.Fill;

            startForm.Controls.Add(newLabel);

            startForm.Size = new Size(300, 50);
            startForm.BackColor = Color.FromArgb(0, 0, 0);
            startForm.Text = "";
            startForm.StartPosition = FormStartPosition.CenterScreen;
            startForm.Icon = null;
            startForm.FormBorderStyle = FormBorderStyle.None;
            startForm.ShowInTaskbar = false;
            startForm.UseWaitCursor = true;

            startForm.Show();
        }

        public static void Close()
        {
            startForm.Close();
        }
    }
}
