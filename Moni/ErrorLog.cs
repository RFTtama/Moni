using System;
using System.IO;
using System.Windows.Forms;

namespace Moni
{
    static class ErrorLog
    {
        public static void ErrorOutput(string name, string msg, bool show)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@".\tcData\errorLog.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss [") + name + "]" + msg + "\r\n");
                }
                if (show) MessageBox.Show(msg, name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                //無視
            }
        }
    }
}
