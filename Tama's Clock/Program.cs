using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public static void ShowSplash()
        {
            Label newLabel = new Label();
            newLabel.Text = "Moni 準備中";
            newLabel.AutoSize = false;
            newLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            newLabel.Dock = DockStyle.Fill;
            startForm.Controls.Add(newLabel);

            startForm.Size = new System.Drawing.Size(80, 40);
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
