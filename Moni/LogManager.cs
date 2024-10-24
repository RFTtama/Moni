using System;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows.Forms;

namespace Moni
{
    /// <summary>
    /// ログを管理するクラス
    /// </summary>
    static class LogManager
    {
        private static Clock f;                //フォーム
        public static string stateLabel        //現在の状態:フォームのやつも変更される
        {
            set
            {
                f.StateLabel.Text = value;
            }
        }
        private static short _logState;

        /// <summary>
        /// ログの状態
        /// </summary>
        public static short logState       //ログの状態番号:フォームのやつのいろがかわる
        {
            get
            {
                return _logState;
            }
            set
            {
                switch (value)
                {
                    case 0://正常
                        f.LedPanel.BackColor = Color.LightSkyBlue;
                        break;

                    case 1://注意
                        f.LedPanel.BackColor = Color.Yellow;
                        break;

                    case 2://警告
                        f.LedPanel.BackColor = Color.Red;
                        f.LogTimer.Enabled = false;
                        SaveData.enableLogSave = false;
                        break;

                    case 3://停止中
                        f.LedPanel.BackColor = Color.Black;
                        f.LogTimer.Enabled = false;
                        SaveData.enableLogSave = false;
                        break;

                    default://不明
                        f.LedPanel.BackColor = Color.Red;
                        f.LogTimer.Enabled = false;
                        SaveData.enableLogSave = false;
                        break;
                }
                _logState = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="clock"></param>
        public static void LogManagerConstructor(Clock clock)
        {
            f = clock;
            SaveData.SaveDataConstructor(f);
        }

        /// <summary>
        /// ログ保存の状態を返す
        /// </summary>
        /// <returns>ログ保存の状態</returns>
        public static string GetLogState()
        {
            switch (logState)
            {
                case 0:
                    return "正常";

                case 1:
                    return "注意";

                case 2:
                    return "警告";

                case 3:
                    return "停止";

                default:
                    return "不明";
            }
        }


        public const int STATE_NORMAL = 0;
        public const int STATE_WARNING = 1;
        public const int STATE_ERROR = 2;
        public const int STATE_STOP = 3;
        public const int STATE_UNKNOWN = 4;

        /// <summary>
        /// 現在の状態をまとめてセットする
        /// </summary>
        /// <param name="logStateNum"></param>
        /// <param name="stateStr"></param>
        public static void SetState(short logStateNum, string stateStr)
        {
            logState = logStateNum;
            stateLabel = stateStr;
        }
    }

    public static class SaveData
    {
        public static readonly string filePath = @".\tcData\config.cfg";
        private static int _faceColor = ColorTranslator.ToWin32(Color.LightSkyBlue);

        /// <summary>
        /// 色
        /// </summary>
        public static int faceColor
        {
            get
            {
                return _faceColor;
            }
            set
            {
                _faceColor = value;
                f.panel1.BackColor = ColorTranslator.FromWin32(value);
            }
        }
        private static bool _topMost = false;

        /// <summary>
        /// トップモスト
        /// </summary>
        public static bool topMost
        {
            get
            {
                return _topMost;
            }
            set
            {
                _topMost = value;
                f.TopMost = _topMost;

            }
        }
        private static bool _transparent = false;

        /// <summary>
        /// 透明化にするか
        /// </summary>
        public static bool transparent
        {
            get
            {
                return _transparent;
            }
            set
            {
                _transparent = value;
            }
        }
        private static bool _dateTimerEnabled = false;

        /// <summary>
        /// 時計を表示するか否か
        /// </summary>
        public static bool dateTimerEnabled
        {
            get
            {
                return _dateTimerEnabled;
            }
            set
            {
                _dateTimerEnabled = value;
                f.TimeLabel.Visible = dateTimerEnabled;
                f.SecondLabel.Visible = dateTimerEnabled;
                f.DateLabel.Visible = dateTimerEnabled;
                f.checkBox3.Enabled = dateTimerEnabled;
            }
        }

        /// <summary>
        /// 時間経過を知らせるか否か
        /// </summary>
        public static bool tellClock = false;
        private static bool _enableLogSave = false;

        /// <summary>
        /// ログの保存を有効化するか否か
        /// </summary>
        public static bool enableLogSave
        {
            get
            {
                return _enableLogSave;
            }
            set
            {
                _enableLogSave = value;
                f.LogTimer.Enabled = enableLogSave;
            }
        }

        private static bool _apiEnabled = false;

        /// <summary>
        /// api機能の有効化
        /// </summary>
        public static bool apiEnabled
        {
            get
            {
                return _apiEnabled;
            }
            set
            {
                _apiEnabled = value;
                f.apiTimer.Enabled = apiEnabled;
                f.apiLabel.Visible = apiEnabled;
            }
        }

        /// <summary>
        /// ログの保存日数
        /// </summary>
        public static int saveDate = 7;

        //ふぉむ
        private static Clock f;

        /// <summary>
        /// コンストラクタ(仮)
        /// </summary>
        /// <param name="clk">フォーム</param>
        public static void SaveDataConstructor(Clock clk)
        {
            f = clk;
        }

        /// <summary>
        /// セーブデータを読み取る
        /// </summary>
        public static void ReadSaveDatas()
        {
            LoadingProcess loading = new LoadingProcess();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string dat = sr.ReadLine();
                    string[] dataStr = dat.Split(',');
                    int[] datas = new int[dataStr.Length];
                    for (int i = 0; i < dataStr.Length; i++)
                    {
                        datas[i] = int.Parse(dataStr[i]);
                    }
                    if (datas[0] == 1)
                    {
                        topMost = true;
                    }
                    else
                    {
                        topMost = false;
                    }
                    if (datas[1] == 1)
                    {
                        dateTimerEnabled = true;
                    }
                    else
                    {
                        dateTimerEnabled = false;
                    }
                    if (datas[2] == 1)
                    {
                        tellClock = true;
                    }
                    else
                    {
                        tellClock = false;
                    }
                    if (datas[3] == 1)
                    {
                        enableLogSave = true;
                    }
                    else
                    {
                        enableLogSave = false;
                    }
                    faceColor = datas[4];
                    f.DriveUD.SelectedIndex = datas[5];
                    if (datas[6] == 1)
                    {
                        transparent = true;
                    }
                    else
                    {
                        transparent = false;
                    }
                    f.SaveStyleUD.SelectedIndex = datas[7];
                    if (datas[8] == 1)
                    {
                        apiEnabled = true;
                    }
                    else
                    {
                        apiEnabled = false;
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                MessageBox.Show("設定を更新します", "更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                try
                {
                    DataSave();
                    MessageBox.Show("更新完了!", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorOutput("設定更新エラー", ex.Message, true);
                }
            }
            catch (FileNotFoundException)
            {
                try
                {
                    DataSave();
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorOutput("設定初期化エラー", ex.Message, true);
                    f.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("設定読取エラー", ex.Message, true);
                f.Close();
            }
            return;
        }

        /// <summary>
        /// セーブデータを書き出す
        /// </summary>
        public static void DataSave()
        {
            LoadingProcess loading = new LoadingProcess();
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    string data = string.Empty;
                    bool[] bs;
                    bs = FitBoolDatas();
                    foreach (bool b in bs)
                    {
                        if (b)
                        {
                            data += "1";
                        }
                        else
                        {
                            data += "0";
                        }
                        data += ",";
                    }
                    data += faceColor;
                    data += "," + f.DriveUD.SelectedIndex;
                    if (transparent)
                    {
                        data += "," + "1";
                    }
                    else
                    {
                        data += "," + "0";
                    }
                    data += "," + f.SaveStyleUD.SelectedIndex;
                    if (apiEnabled)
                    {
                        data += "," + "1";
                    }
                    else
                    {
                        data += "," + "0";
                    }
                    sw.WriteLine(data);                 //書き込み開始
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("設定保存エラー", ex.Message, true);
                return;
            }
            return;
        }

        /// <summary>
        /// ブーリアンデータをまとめる
        /// </summary>
        /// <param name="bls">いれるやつ</param>
        private static bool[] FitBoolDatas()
        {
            bool[] vs = new bool[]
                    {
                        topMost, dateTimerEnabled, tellClock, enableLogSave
                    };
            return vs;
        }
    }
}
