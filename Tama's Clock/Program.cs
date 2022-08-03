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

            Mutex mutex = new Mutex(false, "MoniResourcesMonitor");

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
                if(hasHandle == false)
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

    static class Splash
    {
        private static Form startForm = new Form();
        private static System.Timers.Timer timer = new System.Timers.Timer();
        private static Label newLabel = new Label();
        private static int splashCnt = 0;

        public static void ShowSplash()
        {
            newLabel.Text = "Moni> Developed -by @RFT\r\n\tLaunching...";
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

            timer.Elapsed += splashAnimation;
            timer.Interval = 500;
            startForm.Show();
            timer.Enabled = true;
        }

        private static void splashAnimation(object sender, EventArgs e)
        {
            if (splashCnt % 2 == 1)
            {
                newLabel.Text = "Moni> Developed -by @RFT\r\n\tLaunching...";
            }
            else
            {
                newLabel.Text = "Moni> Developed -by @RFT\r\n\tLaunching..";
            }
            splashCnt++;
        }

        public static void Close()
        {
            timer.Enabled = false;
            timer.Close();
            startForm.Close();
            newLabel.Dispose();
        }
    }
}
