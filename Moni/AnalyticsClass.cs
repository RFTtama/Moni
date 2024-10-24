using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Moni
{
    class AnalyticsClass
    {
        private Clock _form;
        private readonly DifferentManager dm;
        private readonly List<long> send;
        private readonly List<long> receive;
        private readonly List<float> cpu;
        private readonly List<long> mem;
        private readonly List<int> gpu;
        private readonly List<float> disk;
        private readonly List<DateTime> dts;
        private uint mss;
        private uint processSpeed;
        private List<AnalyzedStatistics> analyzedStatistics;
        public readonly string[] HEAD = new string[] { "k", "M", "G", "T", "P" };//削除予定
        public readonly string[] HEAD_NORMAL = new string[] { "", "k", "M", "G", "T", "P" };//い
        private Stopwatch stopwatch;
        private Stopwatch processTime;
        private const string MaxSpeedFile = @".\tcData\MaxSpeed.dat";
        public static Task analysisTask;
        private int tryNum;
        private int overallTry;
        public bool isBusy;

        /// <summary>
        /// ビジーパー
        /// </summary>
        private double _overallBusyPer;

        public double overallBusyPer
        {
            get
            {
                return _overallBusyPer;
            }
        }

        System.Windows.Forms.Label lvl;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="form">フォーム</param>
        public AnalyticsClass(Clock form)
        {
            _form = form;
            dm = new DifferentManager(_form);
            analysisTask = null;
            send = new List<long>();
            receive = new List<long>();
            cpu = new List<float>();
            mem = new List<long>();
            gpu = new List<int>();
            disk = new List<float>();
            dts = new List<DateTime>();
            mss = 0;
            processSpeed = 0;
            analyzedStatistics = new List<AnalyzedStatistics>();
            lvl = _form.AnalyticsLabel;
            _overallBusyPer = 0.0;
            isBusy = false;
        }

        /// <summary>
        /// ディレクトリのファイルサイズを取得(表面のみ)
        /// </summary>
        /// <param name="di">取得するディレクトリ</param>
        /// <returns>ファイルサイズ</returns>
        public long GetFolderSize(DirectoryInfo di)
        {
            long size = 0;

            // ファイルの一覧を取得し、ファイルサイズを加算する
            foreach (FileInfo f in di.GetFiles())
            {
                size += f.Length; // ファイルサイズを加算（バイト）
            }
            return size;
        }

        /// <summary>
        /// ログファイルを解析する
        /// </summary>
        public void AnalysisLogFiles()
        {
            try
            {
                analysisTask = Task.Run(() =>
                {
                    isBusy = true;
                    LoadingProcess loading = new LoadingProcess();
                    stopwatch = new Stopwatch();
                    processTime = new Stopwatch();

                    processTime.Start();

                    lvl.Text = "初期化中";

                    string[] logFileNames;

                    List<string> datas = new List<string>();
                    tryNum = 0;
                    overallTry = 0;

                    //情報保存リスト初期化
                    send.Clear();
                    receive.Clear();
                    cpu.Clear();
                    mem.Clear();
                    gpu.Clear();
                    disk.Clear();
                    dts.Clear();

                    lvl.Text = "初期化終了";

                    try
                    {
                        lvl.Text = "ファイル情報取得中";
                        //ログフォルダにある全てのファイル名を取得する
                        logFileNames = Directory.GetFiles(
                                    @".\LogData\", "*.tc", SearchOption.AllDirectories);
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.ErrorOutput("ログ情報取得エラー", ex.Message, true);
                        lvl.Text = "ファイル情報取得失敗";
                        LogManager.logState = 2;
                        return;
                    }

                    lvl.Text = "ファイルサイズを計測中";

                    DirectoryInfo di = new DirectoryInfo(@".\LogData\");

                    ValueManager size = new ValueManager(GetFolderSize(di));

                    _form.DataSizeLabel.Text = "データ数: " + size.data.ToString("F1") + " (" + size.HeadToString() + "Bytes)";

                    uint process = 0;

                    lvl.Text = "ログ解析に入ります";

                    //性能モード
                    if (_form.SaveStyleUD.SelectedIndex == 1)
                    {
                        try
                        {
                            int cnt = 0;
                            overallTry += logFileNames.Length;
                            foreach (string fileName in logFileNames)
                            {
                                lvl.Text = "ログ取得中";
                                string date = fileName.Replace("ResourcesLog", "");
                                date = date.Replace(".\\LogData\\", "");
                                date = date.Replace(".tc", "");
                                date = date.Replace("_", "/");


                                while (true)
                                {
                                    tryNum++;
                                    try
                                    {
                                        //ファイルからデータを取得する
                                        using (StreamReader sr = new StreamReader(fileName))
                                        {
                                            stopwatch.Start();
                                            string str = sr.ReadLine();

                                            while (str != null)
                                            {
                                                string[] split = null;
                                                string[] baseTime = null;
                                                try
                                                {
                                                    str = str.Replace(" ", "");
                                                    split = str.Split(',');

                                                    //時間変換
                                                    baseTime = split[0].Split(':');
                                                    dts.Add(DateTime.Parse(date));
                                                }
                                                catch (Exception)
                                                {
                                                    str = sr.ReadLine();
                                                    if (dts[dts.Count - 1] == null)
                                                    {
                                                        dts.RemoveAt(dts.Count - 1);
                                                    }
                                                    break;
                                                }

                                                double numD;
                                                long numL;
                                                int head;
                                                double calcD;
                                                long calcL;
                                                int[] time = new int[3];

                                                for (int i = 0; i < time.Length; i++)
                                                {
                                                    time[i] = int.Parse(baseTime[i]);
                                                }
                                                dts[cnt] = new DateTime(dts[cnt].Year, dts[cnt].Month, dts[cnt].Day, time[0], time[1], time[2]);


                                                //受信速度変換
                                                numL = long.Parse(split[1]);
                                                head = ConvertHeadToNum(split[2]);
                                                calcL = numL * MoniMath.Pow(1000, head + 1);
                                                send.Add(calcL);

                                                //送信速度変換
                                                numL = long.Parse(split[3]);
                                                head = ConvertHeadToNum(split[4]);
                                                calcL = numL * MoniMath.Pow(1000, head + 1);
                                                receive.Add(calcL);

                                                //cpu変換
                                                cpu.Add(float.Parse(split[5]));

                                                //mem変換
                                                numD = double.Parse(split[6]);
                                                head = ConvertHeadToNum(split[7]);
                                                calcD = numD * MoniMath.Pow(1000, head + 1);
                                                mem.Add((long)calcD);

                                                //GPU変換
                                                gpu.Add(int.Parse(split[8]));

                                                //ディスク変換
                                                disk.Add(float.Parse(split[9]));

                                                //カウントアップ
                                                cnt++;
                                                process += 7;
                                                str = sr.ReadLine();
                                            }
                                        }
                                        break;
                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                        //ファイルにアクセスできるまで繰り返す
                                    }
                                    //ログの異常
                                    catch (System.FormatException)
                                    {
                                        //ログを修正する
                                        FixLogData(fileName);
                                        return;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.ErrorOutput("ログ解析中にエラー", ex.Message + ex, true);
                        }

                        stopwatch.Stop();
                    }//スピードモード
                    else if (_form.SaveStyleUD.SelectedIndex == 0)
                    {
                        try
                        {
                            overallTry += logFileNames.Length;
                            foreach (string fileName in logFileNames)
                            {
                                lvl.Text = "ログ取得中";
                                string date = fileName.Replace("ResourcesLog", "");
                                date = date.Replace(".\\LogData\\", "");
                                date = date.Replace(".tc", "");
                                date = date.Replace("_", "/");

                                //ファイルからデータを取得する
                                while (true)
                                {
                                    tryNum++;
                                    try
                                    {
                                        using (StreamReader sr = new StreamReader(fileName))
                                        {
                                            stopwatch.Start();
                                            string str = sr.ReadLine();
                                            while (str != null)
                                            {
                                                /*try
                                                {*/
                                                str = str.Replace(" ", "");
                                                dts.Add(DateTime.Parse(date));
                                                datas.Add(str);
                                                str = sr.ReadLine();
                                                process++;
                                                /*}
                                                catch (Exception)
                                                {
                                                    if(datas[datas.Count - 1] == null)
                                                    {
                                                        datas.RemoveAt(datas.Count - 1);
                                                        dts.RemoveAt(dts.Count - 1);
                                                    }
                                                }*/
                                            }
                                        }
                                        break;
                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                        //ファイルにアクセスできるまで繰り返す
                                    }
                                    //ログの異常
                                    catch (System.FormatException)
                                    {
                                        //ログを修正する
                                        FixLogData(fileName);
                                        return;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.ErrorOutput("ログ取得エラー", ex.Message, true);
                            lvl.Text = "ログ取得失敗";
                            LogManager.logState = 2;
                            return;
                        }

                        stopwatch.Stop();

                        if (datas.Count == 0)
                        {
                            ErrorLog.ErrorOutput("ログ情報エラー", "ログのデータ数が0です", true);
                            lvl.Text = "ログ取得失敗";
                            return;
                        }

                        lvl.Text = "ログ取得完了";
                        //分割したデータを格納しておくリスト
                        List<string[]> splitedDatas = new List<string[]>();

                        try
                        {
                            lvl.Text = "データ分割中";

                            stopwatch.Start();

                            //分割したデータをリストに格納する
                            foreach (string data in datas)
                            {
                                try
                                {
                                    splitedDatas.Add(data.Split(','));
                                    process++;
                                }
                                catch (Exception)
                                {
                                    break;
                                }
                            }

                            stopwatch.Stop();

                            lvl.Text = "データ分割完了";

                            //データを変換
                            int cnt = 0;

                            lvl.Text = "データ変換中";

                            stopwatch.Start();

                            foreach (string[] str in splitedDatas)
                            {
                                double numD;
                                long numL;
                                int head;
                                double calcD;
                                long calcL;
                                string[] baseTime;
                                int[] time = new int[3];

                                //時間変換
                                try
                                {
                                    baseTime = str[0].Split(':');
                                }
                                catch (Exception)
                                {
                                    break;
                                }
                                for (int i = 0; i < time.Length; i++)
                                {
                                    time[i] = int.Parse(baseTime[i]);
                                }
                                dts[cnt] = new DateTime(dts[cnt].Year, dts[cnt].Month, dts[cnt].Day, time[0], time[1], time[2]);


                                //受信速度変換
                                numL = long.Parse(str[1]);
                                head = ConvertHeadToNum(str[2]);
                                calcL = numL * MoniMath.Pow(1000, head + 1);
                                send.Add(calcL);

                                //送信速度変換
                                numL = long.Parse(str[3]);
                                head = ConvertHeadToNum(str[4]);
                                calcL = numL * MoniMath.Pow(1000, head + 1);
                                receive.Add(calcL);

                                //cpu変換
                                cpu.Add(float.Parse(str[5]));

                                //mem変換
                                numD = double.Parse(str[6]);
                                head = ConvertHeadToNum(str[7]);
                                calcD = numD * MoniMath.Pow(1000, head + 1);
                                mem.Add((long)calcD);

                                //GPU変換
                                gpu.Add(int.Parse(str[8]));

                                //ディスク変換
                                disk.Add(float.Parse(str[9]));

                                //カウントアップ
                                cnt++;
                                process++;
                            }
                        }
                        catch (System.FormatException)
                        {
                            dm.RemoveAllLogFiles();
                            lvl.Text = "データ変換失敗:未対応のログ";
                            LogManager.logState = 2;
                            return;
                        }
                        catch (Exception)
                        {
                            lvl.Text = "データ変換失敗:未知の不具合";
                            LogManager.logState = 2;
                            return;
                        }

                        stopwatch.Stop();

                        splitedDatas = null;

                        lvl.Text = "データ変換完了";
                    }

                    AnalysisStatistics();

                    processTime.Stop();

                    //ここから書き出し

                    TimeSpan ts = stopwatch.Elapsed;
                    TimeSpan ts2 = processTime.Elapsed;
                    string elapsedTime = string.Format("{0:00}分{1:00}秒{2:00}",
                        ts2.Minutes, ts2.Seconds,
                        ts2.Milliseconds / 10);

                    uint ms = (uint)(ts.Milliseconds + ts.Seconds * 1000 + ts.Minutes * 60 * 1000);

                    mss = ms;
                    processSpeed = process / ms;

                    string[] speedString;
                    uint totalSpeed;
                    uint totalNumber;
                    uint maxSpeed = 0;

                    try
                    {
                        using (StreamReader sr = new StreamReader(MaxSpeedFile))
                        {
                            speedString = sr.ReadLine().Split(',');
                            maxSpeed = uint.Parse(speedString[0]);
                            totalSpeed = uint.Parse(speedString[1]);
                            totalNumber = uint.Parse(speedString[2]);
                        }
                        totalNumber++;
                        totalSpeed += processSpeed;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        totalSpeed = processSpeed;
                        totalNumber = 1;
                    }
                    catch (FileNotFoundException)
                    {
                        totalSpeed = processSpeed;
                        totalNumber = 1;
                    }
                    catch (Exception e)
                    {
                        ErrorLog.ErrorOutput("PPMS算出エラー", e.Message, true);
                        totalSpeed = processSpeed;
                        totalNumber = 1;
                    }

                    if (processSpeed > maxSpeed)
                    {
                        maxSpeed = processSpeed;
                    }

                    using (StreamWriter sw = new StreamWriter(MaxSpeedFile))
                    {
                        sw.WriteLine(maxSpeed + "," + totalSpeed + "," + totalNumber);
                    }

                    long avgSpeed = totalSpeed / totalNumber;
                    float fos = ((float)tryNum / overallTry) * 100.0f;

                    _form.toolTip1.SetToolTip(_form.PpmsPic, "直近: " + processSpeed + " PPMS\r\n最大: " + maxSpeed + " PPMS" +
                        "\r\n平均: " + avgSpeed + " PPMS (" + totalNumber.ToString() + "回の解析)" +
                        "\r\n\r\nこの数値は1msで処理したプロセスの最大数を示し、" +
                        "\r\nコンピュータの処理能力を表しています" +
                        "\r\nPPMS: (Processes Per MilliSecond)" +
                        "\r\n\r\n※この数値はログの診断設定によって違います" +
                        "\r\n\r\nファイルアクセス成功率: " + fos.ToString("F2") + "%");

                    lvl.Text = "完了 (" + elapsedTime + ")";
                    isBusy = false;
                });
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("ログの解析中に不明なエラー", ex.Message + ex, true);
            }
        }

        /// <summary>
        /// 予想される計算速度を返す
        /// </summary>
        /// <param name="maxSpeed">最大速度PPMS</param>
        /// <param name="convertSpeed">計算速度PPMS</param>
        /// <returns>予想された速度PPMS</returns>
        private uint CalcRealProcessTime(uint maxSpeed, uint convertSpeed)
        {
            uint standard = maxSpeed;
            float magn = convertSpeed / (float)standard;
            long time = mss;
            float realProcessTime = time * magn;
            return (uint)realProcessTime;
        }

        /// <summary>
        /// 詳細な情報を解析し、AnalyzedStatisticsに変換する
        /// </summary>
        private void AnalysisStatistics()
        {
            lvl.Text = "詳細情報解析開始";
            analyzedStatistics.Clear();
            DateTime detailTime = dts[0];
            int activeTime = 0;
            int busyTime = 0;

            lvl.Text = "詳細情報解析中";

            stopwatch.Start();

            //csvに書き出し

            try
            {
                using (StreamWriter sw = new StreamWriter("./tcData/allResourceLogs.csv", false))
                {
                    sw.WriteLine("date time,cpu usage(%),mem usage(bytes),gpu usage(%),disk usage(%),network upload speed(Bps),network download speed(Bps)");
                    for (int i = 0; i < dts.Count; i++)
                    {
                        sw.WriteLine(dts[i].ToString("G") + "," + cpu[i].ToString() + "," + mem[i].ToString() + "," +
                            gpu[i].ToString() + "," + disk[i].ToString() + "," + send[i].ToString() + "," + receive[i].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("csv書き出しエラー", ex.Message, true);
            }

            for (int i = 0; i < dts.Count; i++)
            {
                if (detailTime.Year != dts[i].Year || detailTime.Month != dts[i].Month || detailTime.Day != dts[i].Day)
                {
                    analyzedStatistics.Add(new AnalyzedStatistics(detailTime, activeTime, busyTime));
                    detailTime = dts[i];
                    activeTime = 0;
                    busyTime = 0;
                }
                else
                {
                    activeTime++;
                    if (receive[i] >= 1000000 || send[i] >= 1000000 || cpu[i] >= 75 || mem[i] <= 1000000000 || gpu[i] >= 90.0f || disk[i] >= 100.0f)
                    {
                        busyTime++;
                    }
                }
            }

            stopwatch.Stop();

            analyzedStatistics.Add(new AnalyzedStatistics(detailTime, activeTime, busyTime));
            lvl.Text = "解析終了";
            lvl.Text = "詳細情報設定中";
            float activeTotal = 0.0f;
            float busyTotal = 0.0f;
            foreach (AnalyzedStatistics _as in analyzedStatistics)
            {
                activeTotal += _as.activePer;
                busyTotal += _as.busyPer;
            }

            double saveDates = analyzedStatistics.Count * 100.0;

            double avgBusyPer = busyTotal / saveDates * 100.0;
            double avgActivePer = activeTotal / saveDates * 100.0;

            double busy = avgBusyPer * avgActivePer / 100.0;
            //数日間ビジー率 過去日間でビジーだった%
            //数日間アクティブ率 過去日間でアクティブだった%

            //起動していた時間が短時間で、ビジー状態の場合
            //平均ビジー率が上がるが、アクティブ率が低いため全体ビジー率は低い

            //数日間ビジー率は日経過するまでは基本下がらない
            //数日間アクティブ率は累積するので増加する
            //1日目のビジー率が100%の場合数日間ビジー率は 100 / 　で10%
            //この場合数日間アクティブ率は10%なので全体ビジー率は1%

            _overallBusyPer = busy;

            int overallHours = 24 * analyzedStatistics.Count;
            double activeHours = overallHours * (avgActivePer / 100.0);
            double busyHours = activeHours * (avgBusyPer / 100.0);

            _form.SummaryDay.Text = analyzedStatistics.Count + "日分のログ\n\r";
            _form.SummaryActive.Text = "アクティブ: " + avgActivePer.ToString("F1") + "% (" + activeHours.ToString("F1") + "時間)\n\r";
            _form.SummaryBusy.Text = "ビジー: " + avgBusyPer.ToString("F1") + "% (" + busyHours.ToString("F1") + "時間)\n\r";
            _form.SummaryOverall.Text = "疲れ: " + overallBusyPer.ToString("F1") + "%\n\r";
            _form.ActiveRed.Width = (int)(avgActivePer / 100.0f * _form.redPicSize);
            _form.BusyRed.Width = (int)(avgBusyPer / 100.0f * _form.redPicSize);
            _form.OverallRed.Width = (int)(overallBusyPer / 100.0f * _form.redPicSize);

            lvl.Text = "設定終了";
            lvl.Text = "解析完了";

            Dispose();
        }


        /// <summary>
        /// チャートおよび分析に使用したメモリを解放する
        /// </summary>
        public void Dispose()
        {
            send.Clear();
            receive.Clear();
            cpu.Clear();
            mem.Clear();
            gpu.Clear();
            disk.Clear();
            dts.Clear();
            analyzedStatistics.Clear();
            lvl.Text = "データ破棄";
        }

        /// <summary>
        /// 接頭語を変換用数値に変換する
        /// </summary>
        /// <param name="str">接頭語</param>
        /// <returns>変換用数値</returns>
        private int ConvertHeadToNum(string str)
        {
            int i;
            for (i = 0; i < HEAD.Length; i++)
            {
                if (str == HEAD[i])
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 問題のあるログファイルを修正する
        /// </summary>
        /// <param name="fileName">ログファイル名</param>
        private void FixLogData(string fileName)
        {
            //↓読みながら別のファイルに上書きでメモリ節約可
            lvl.Text = "ログ修正中";
            using (StreamReader stream = new StreamReader(fileName))
            {
                string line = stream.ReadLine();
                while (line != null)
                {
                    try
                    {
                        line = line.Replace(" ", "");
                        string[] splitedStr = line.Split(',');
                        string[] timeStr = splitedStr[0].Split(':');
                        int[] timeInt = new int[3];
                        for (int i = 0; i < timeInt.Length; i++)
                        {
                            timeInt[i] = int.Parse(timeStr[i]);
                        }
                        using (StreamWriter writeFile = new StreamWriter(fileName + "_fixing", true))
                        {
                            writeFile.WriteLine(line);
                        }
                    }
                    catch (FormatException)
                    {
                    }
                    line = stream.ReadLine();
                }
                File.Replace(fileName + "_fixing", fileName, fileName + DateTime.Now.ToString("yyyy_MM_dd_H_mm_ss") + ".tcold");
            }

        }
    }

    /// <summary>
    /// 分析した詳細データを保持しておくクラス
    /// </summary>
    public class AnalyzedStatistics
    {
        public const int timeOfDay = 86400;

        private DateTime _dateTime;
        public DateTime dateTime
        {
            get
            {
                return _dateTime;
            }
        }

        private int _activeTime;
        public int activeTime
        {
            get
            {
                return _activeTime;
            }
        }

        private int _busyTime;
        public int busyTime
        {
            get
            {
                return _busyTime;
            }
        }

        public float busyPer
        {
            get
            {
                return ((float)busyTime / activeTime * 100.0f);
            }
        }

        public float activePer
        {
            get
            {
                return ((float)activeTime / AnalyzedStatistics.timeOfDay * 100.0f);
            }
        }

        /// <summary>
        /// コンストラクタ　ここでしか数値の変更はできない
        /// </summary>
        /// <param name="time">ログの日付</param>
        /// <param name="active">コンピュータが起動していた時間を格納する変数</param>
        /// <param name="busy">コンピュータがビジー状態だった時間を格納する変数</param>
        public AnalyzedStatistics(DateTime time, int active, int busy)
        {
            _dateTime = time;
            _activeTime = active;
            _busyTime = busy;
        }
    }
}
