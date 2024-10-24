using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moni
{
    public class Bottleneck
    {
        public string name;
        private int _errorTimes;
        public int errorTimes
        {
            get
            {
                return _errorTimes;
            }
            set
            {
                if (value == 0)
                {
                    _errorTimes = 0;
                    _errorFlg = false;
                }
                else
                {
                    if (++_errorTimes >= 1)
                    {
                        _errorFlg = true;
                    }
                    else
                    {
                        _errorFlg = false;
                    }
                }
            }
        }
        private bool _errorFlg;
        public bool errorFlg
        {
            get
            {
                return _errorFlg;
            }
            set
            {

            }
        }

        public Bottleneck()
        {
            this.name = null;
            this.errorTimes = 0;
            this.errorFlg = false;
        }

        public Bottleneck(string arg)
        {
            this.name = arg;
            this.errorTimes = 0;
            this.errorFlg = false;
        }
    }

    public class DifferentManager
    {
        private Clock _form = null;

        public DifferentManager(Clock fm)
        {
            _form = fm;
        }

        /// <summary>
        /// 数日おきにログを削除する関数
        /// </summary>
        public void CheckLogFiles()
        {
            try
            {
                string[] files = System.IO.Directory.GetFiles(
                    @".\LogData\", "*.tc", System.IO.SearchOption.AllDirectories);

                List<string> filesList = new List<string>();

                foreach (string file in files)
                {
                    filesList.Add(file);
                }

                /*for (int i = 0; i < files.Length; i++)
                {
                    string name = Path.GetFileNameWithoutExtension(files[i]);
                    string day = name.Replace("ResourcesLog", "");
                    string[] date = day.Split('_');
                    int[] datei = new int[] { int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]) };
                    DateTime logDt = new DateTime(datei[0], datei[1], datei[2]);
                    DateTime dt = DateTime.Now;
                    TimeSpan between = dt - logDt;
                    if (SaveData.saveDate >= 0)
                    {
                        if (between.Days >= SaveData.saveDate)
                        {
                            File.Delete(files[i]);
                        }
                    }
                }*/
                if (SaveData.saveDate >= 0)
                {
                    while (filesList.Count > SaveData.saveDate)
                    {
                        File.Delete(filesList[0]);
                        filesList.RemoveAt(0);
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(@".\LogData");
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("ログ管理エラー", ex.Message, true);
            }
        }

        /// <summary>
        /// 全てのログファイルを削除する
        /// </summary>
        public void RemoveAllLogFiles()
        {
            try
            {
                string[] files = System.IO.Directory.GetFiles(
                        @".\LogData\", "*", System.IO.SearchOption.AllDirectories);
                foreach (string removeFile in files)
                {
                    File.Delete(removeFile);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("ログ削除エラー", ex.Message, true);
            }
        }

        public void SetStartUp()
        {

            //ショートカットの作成先
            try
            {

                string shortcutPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                    @"Moni.lnk");

                string[] fileList = Directory.GetFileSystemEntries(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                    @"Moni.lnk", SearchOption.TopDirectoryOnly);

                if (fileList.Length <= 0)
                {
                    DialogResult result = MessageBox.Show("Moniをスタートアップに登録しますか?",
                        "スタートアップに指定", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result != DialogResult.Yes)
                    {
                        return;
                    }

                    //実行パス
                    string targetPath = Application.ExecutablePath;
                    //string targetPath = Path.GetFullPath(@"./MoniInstaller.exe"); 

                    //WshShell
                    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

                    //WshShortcutを作成
                    IWshRuntimeLibrary.IWshShortcut shortcut =
                        (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);

                    //リンクを指定
                    shortcut.TargetPath = targetPath;

                    //パロメ
                    shortcut.Arguments = "/a /b /c";

                    //作業フォルダ
                    shortcut.WorkingDirectory = Application.StartupPath;

                    //アイコン
                    shortcut.IconLocation = Application.ExecutablePath + ",0";

                    //作成
                    shortcut.Save();

                    //後始末
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shortcut);
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shell);
                    Task task = Task.Run(() =>
                    {
                        MessageBox.Show("スタートアップ登録済み");
                    });
                }
                else
                {
                    DialogResult result = MessageBox.Show("Moniをスタートアップから削除しますか?",
                        "スタートアップから削除", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result != DialogResult.Yes)
                    {
                        return;
                    }

                    foreach (string str in fileList)
                    {
                        File.Delete(str);
                    }
                    Task task = Task.Run(() =>
                    {
                        MessageBox.Show("スタートアップ削除済み");
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("ショートカット設定エラー", ex.Message, true);
            }
        }

        /// <summary>
        /// 夜(22時から5時までの間)にtrueを返す
        /// </summary>
        /// <returns></returns>
        public bool CheckNight()
        {
            DateTime dt = DateTime.Now;
            if ((dt.Hour >= 22 || dt.Hour <= 5))
            {
                return true;
            }
            return false;
        }
    }
}
