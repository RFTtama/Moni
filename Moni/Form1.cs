using Microsoft.Win32;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Moni
{
    public partial class Clock : Form
    {
        private DifferentManager dm;
        private AnalyticsClass ac;
        private MoniTerminator mt = new MoniTerminator();
        private DateTime dt;
        private string nvidiaSmiFile;
        private const string LB = "\r\n";
        private int tellBef;
        public int redPicSize;
        private ValueManager actTotalMem;
        private readonly string gpuFileName = @".\tcData\fileName.txt";
        private readonly string apiFileName = @".\tcData\apiKey.txt";
        private int netInd = 0;
        private long[] speedBef;
        private string netName;
        private bool _ready = false;
        public bool ready
        {
            get
            {
                return _ready;
            }
        }
        private bool gpuOk;
        private List<PerformanceCounter> pcList = new List<PerformanceCounter>();
        private List<Bottleneck> cwList = new List<Bottleneck>();
        private int day;
        private DateTime bootTime = DateTime.Now;

        public Clock()
        {
            LoadIcon._form = this;
            LoadingProcess loading = new LoadingProcess();
            try
            {
                InitializeComponent();

#if !DEBUG

                try
                {
                    /*Moni更新動作*/

                    //アップデート用ディレクトリ名を設定
                    string updateDirectoryName = "MoniLatest";

                    //アップデート用ディレクトリが存在していたら削除する
                    if (Directory.Exists(@".\" + updateDirectoryName))
                    {
                        Directory.Delete(@".\" + updateDirectoryName, true);
                    }

                    //アップデート用ディレクトリを作成
                    Directory.CreateDirectory(updateDirectoryName);

                    //最新ファイルをダウンロード
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        wc.DownloadFile("https://github.com/RFTtama/Moni-released/archive/refs/heads/main.zip", @".\" + updateDirectoryName + @"\main.zip");
                    }

                    //ファイルを展開
                    ZipFile.ExtractToDirectory(@".\" + updateDirectoryName + @"\main.zip", @".\" + updateDirectoryName);

                    //もとのファイルを削除
                    File.Delete(@".\" + updateDirectoryName + @"\main.zip");

                    //ファイルを移動
                    File.Move(@".\" + updateDirectoryName + @"\Moni-released-main\Moni.exe", @".\" + updateDirectoryName + @"\Moni.exe");

                    //移動元のディレクトリを削除
                    Directory.Delete(@".\" + updateDirectoryName + @"\Moni-released-main");

                    //最新ファイルのバージョンを取得
                    FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(@".\" + updateDirectoryName + @"\Moni.exe");

                    //int型に変換
                    int newVersionNum = int.Parse(fileInfo.FileVersion.Replace(".", string.Empty));

                    //現行ファイルのバージョンを取得
                    FileVersionInfo version = FileVersionInfo.GetVersionInfo(
                            System.Reflection.Assembly.GetExecutingAssembly().Location);

                    //int型に変換
                    int oldVersionNum = int.Parse(version.FileVersion.ToString().Replace(".", string.Empty));

                    if (newVersionNum > oldVersionNum)
                    {
                        UpdatePanel.Visible = true;
                    }
                    else
                    {
                        //削除
                        Directory.Delete(@".\" + updateDirectoryName, true);
                    }
                }catch (Exception ex)
                {
                    ErrorLog.ErrorOutput("更新エラー", ex.Message, true);
                }

#endif

                SystemEvents.SessionEnding += new SessionEndingEventHandler(Detect_EndWindows);
                LogManager.LogManagerConstructor(this);
                ErrorLog.mainForm = this;
                dm = new DifferentManager(this);
                ac = new AnalyticsClass(this);
                SaveStyleUD.SelectedIndex = 1;
                dt = DateTime.Now;
                day = dt.Day;
                HoursUD.Value = int.Parse(dt.ToString("HH"));
                MinutesUD.Value = int.Parse(dt.ToString("mm"));
                this.Width = 228;
                this.Height = 208;
                panel1.Width = 420;
                panel1.Height = 170;
                try
                {
                    using (StreamReader sr = new StreamReader(gpuFileName))
                    {
                        nvidiaSmiFile = sr.ReadLine();
                    }
                }
                catch (Exception)
                {
                    SearchGpuFile();
                }
                LogManager.logState = 3;
                this.redPicSize = RedPic2.Width;
                System.Reflection.Assembly asm =
                    System.Reflection.Assembly.GetExecutingAssembly();
                System.Version ver = asm.GetName().Version;
                VersionLabel.Text = "Ver " + ver.ToString();
                this.tellBef = -1;

                DriveUD.Items.Add("ALL");
                //設定読み込み
                SaveData.ReadSaveDatas();
                List<(string machine, string category, string counter, string instance)> counterList = new List<(string machine, string category, string counter, string instance)>();

                counterList.Add((".", "Processor", "% Processor Time", "_Total"));                                      //cpu                               
                counterList.Add((".", "Memory", "Available MBytes", ""));                                               //memory
                counterList.Add((".", "PhysicalDisk", "% Disk Time", "_Total"));

                try
                {
                    for (int i = 0; i < counterList.Count; i++)
                    {
                        pcList.Add(new PerformanceCounter(counterList[i].category, counterList[i].counter, counterList[i].instance, counterList[i].machine));
                    }
                }
                catch (System.InvalidOperationException)
                {
                    string msg = "リソースの取得に失敗しました";
                    ErrorLog.ErrorOutput("リソース取得エラー", msg, true);
                    this.Close();
                }
                SetDesc();
                NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
                long[,] net = new long[nis.Length, 2];
                speedBef = new long[2];
                netName = string.Empty;
                for (int i = 0; i < nis.Length; i++)
                {
                    IPv4InterfaceStatistics ipv4 = nis[i].GetIPv4Statistics();
                    net[i, 0] = ipv4.BytesReceived;
                    net[i, 1] = ipv4.BytesSent;
                    if (net[netInd, 0] + net[netInd, 1] <= net[i, 0] + net[i, 1])
                    {
                        netInd = i;
                        netName = nis[i].Description;
                        speedBef[0] = net[netInd, 0];
                        speedBef[1] = net[netInd, 1];
                    }
                }
                cwList.Add(new Bottleneck("NetWorkWarning"));
                cwList.Add(new Bottleneck("CPUWarning"));
                cwList.Add(new Bottleneck("MemoryWarning"));
                cwList.Add(new Bottleneck("GPUWarning"));
                cwList.Add(new Bottleneck("DiskWarning"));
                if (SaveData.enableLogSave) dm.CheckLogFiles();
                checkBox1.Checked = SaveData.topMost;
                checkBox2.Checked = SaveData.dateTimerEnabled;
                checkBox3.Checked = SaveData.tellClock;
                checkBox4.Checked = SaveData.transparent;
                checkBox5.Checked = SaveData.apiEnabled;
                ResourceTimer.Enabled = true;
                FaceTimer.Enabled = true;
                DateTimer.Enabled = true;

                apiLabel.Text = "ニュースを取得中…";

                this._ready = true;

                GC.Collect();
                GCTimer.Enabled = true;

                if (checkBox5.Checked) UpdateApi();

                Splash.Close();

            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("初期化時に不明なエラー", ex + ex.Message, true);
                this.Close();
            }
        }

        private void UpdateApi()
        {
            try
            {
                string apiKey = null;
                try
                {
                    using (StreamReader sr = new StreamReader(apiFileName))
                    {
                        apiKey = sr.ReadLine();
                    }
                }
                catch (FileNotFoundException)
                {
                    ErrorLog.ErrorOutput("無効なAPIキー", "APIキーが設定されていません", true);
                    chandeApiSettings();
                    return;

                }
                if (apiKey == null || apiKey == "")
                {
                    ErrorLog.ErrorOutput("無効なAPIキー", "APIキーが無効です", true);
                    chandeApiSettings();
                    return;
                }
                apiTextBox.Text = apiKey;
                string url = "https://newsapi.org/v2/top-headlines?country=jp&apiKey=" + apiKey;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("API更新エラー", ex.Message, true);
                chandeApiSettings();
            }
        }

        private string GetGpuData(string arg)
        {
            string result = string.Empty;
            Process p = new Process();
            p.StartInfo.FileName = this.nvidiaSmiFile;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = arg;
            p.Start();
            result = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return result;
        }


        private void SearchGpuFile()
        {
            try
            {
                string[] fileList = Directory.GetFileSystemEntries(@"C:\Windows\System32\DriverStore\FileRepository\", @"nvidia-smi.exe", SearchOption.AllDirectories);
                nvidiaSmiFile = fileList[0];
                gpuOk = true;
                try
                {
                    using (StreamWriter sw = new StreamWriter(gpuFileName))
                    {
                        sw.WriteLine(this.nvidiaSmiFile);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorOutput("GPUパス保存エラー", ex.Message, true);
                }

            }
            catch (IndexOutOfRangeException)
            {
                ErrorLog.ErrorOutput("GPUファイル探索エラー", "NVIDIA製のグラフィックカードを搭載していないまたは" +
                    "nvidia-smiがインストールされていない可能性があります", false);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("GPUファイル探索エラー", ex.Message, false);
            }
            nvidiaSmiFile = null;
            gpuOk = false;
        }

        private void DateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                this.dt = DateTime.Now;
                if (this.day != dt.Day && SaveData.enableLogSave)
                {
                    dm.CheckLogFiles();
                }
                this.day = dt.Day;
                this.DateLabel.Text = dt.ToString("yyyy/MM/dd");
                this.SecondLabel.Text = dt.ToString("ss");
                this.TimeLabel.Text = dt.ToString("HH:mm");
                int min = int.Parse(dt.ToString("mm"));
                int hour = int.Parse(dt.ToString("HH"));
                if (min == 0 && SaveData.tellClock && this.tellBef != hour && checkBox3.Enabled == true)
                {
                    Task task = Task.Run(() =>
                    {
                        Console.Beep();
                    });
                    this.tellBef = hour;
                }
                if (AlarmBox.Checked == true)
                {
                    string alarmTime = HoursUD.Value + ":" + MinutesUD.Value;
                    string nowTime = int.Parse(dt.ToString("HH")) + ":" + int.Parse(dt.ToString("mm"));
                    if (alarmTime == nowTime)
                    {
                        AlarmTimer.Enabled = true;
                    }
                    else
                    {
                        AlarmTimer.Enabled = false;
                    }
                }
                else
                {
                    AlarmTimer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("時間同期処理で不明なエラー", ex + "" + ex.Message, false);
            }
        }

        /// <summary>
        /// シャットダウンを検知してフォームを閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Detect_EndWindows(object sender, SessionEndingEventArgs e)
        {
            this.Dispose();
        }

        private const int moveLength = 4;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveData.topMost = checkBox1.Checked;
            SaveData.DataSave();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SaveData.dateTimerEnabled = checkBox2.Checked;
            SaveData.DataSave();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SaveData.tellClock = checkBox3.Checked;
            SaveData.DataSave();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            SaveData.transparent = checkBox4.Checked;
            SaveData.DataSave();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            chandeApiSettings();
        }

        private void chandeApiSettings()
        {
            SaveData.apiEnabled = checkBox5.Checked;
            if (!checkBox5.Checked) NewsMover.Enabled = checkBox5.Checked;
            SaveData.DataSave();
        }

        private string LogStr = string.Empty;
        private void ResourceTimer_Tick(object sender, EventArgs e)
        {

            try
            {
                NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
                IPv4InterfaceStatistics ipv4 = nis[netInd].GetIPv4Statistics();
                ValueManager receive = new ValueManager(ipv4.BytesReceived - speedBef[0]);
                ValueManager sent = new ValueManager(ipv4.BytesSent - speedBef[1]);
                speedBef[0] = receive.num + speedBef[0];
                speedBef[1] = sent.num + speedBef[1];
                if (receive.head >= 2)
                {
                    Received.ForeColor = Color.Red;
                }
                else
                {
                    Received.ForeColor = Color.Black;
                }
                if (sent.head >= 2)
                {
                    Sent.ForeColor = Color.Red;
                }
                else
                {
                    Sent.ForeColor = Color.Black;
                }
                if (receive.head >= 2 || sent.head >= 2)
                {
                    cwList[0].errorTimes++;
                }
                else
                {
                    cwList[0].errorTimes = 0;
                }
                if (!slideLeft)
                {
                    NetName.Text = netName;
                    Received.Text = receive.data.ToString("F0") + "(" + receive.HeadToString() + "B/s)";
                    Sent.Text = sent.data.ToString("F0") + "(" + sent.HeadToString() + "B/s)";
                }
                float cpuUses = pcList[0].NextValue();
                if (cpuUses >= 75.0f)
                {
                    cwList[1].errorTimes++;
                    CPULabel.ForeColor = Color.Red;
                }
                else
                {
                    cwList[1].errorTimes = 0;
                    CPULabel.ForeColor = Color.Black;
                }
                if (!slideLeft)
                {
                    CPULabel.Text = "CPU使用率: " + cpuUses.ToString("F2") + "(%)";
                    RedPic2.Width = (int)(this.redPicSize * (cpuUses / 100.0f));
                }
                float actAvaMem = pcList[1].NextValue();
                float avaMem = actAvaMem;
                int memHead = 2;
                while (avaMem / 1000.0f > 1.0f)
                {
                    avaMem /= 1000.0f;
                    memHead++;
                }
                if (avaMem <= 1.0f && memHead <= 3)
                {
                    cwList[2].errorTimes++;
                    MemoryLabel.ForeColor = Color.Red;
                }
                else
                {
                    cwList[2].errorTimes = 0;
                    MemoryLabel.ForeColor = Color.Black;
                }
                for (int i = 0; i < 2; i++)
                {
                    actAvaMem *= 1000.0f;
                }
                if (!slideLeft)
                {
                    MemoryLabel.Text = "使用可能メモリ: " + avaMem.ToString("F2") + "(" + ac.HEAD_NORMAL[memHead] + "B)";
                    MaxMemLabel.Text = actTotalMem.data.ToString("F2") + actTotalMem.HeadToString() + "B";
                    RedPic3.Width = (int)((actTotalMem.num - actAvaMem) / actTotalMem.num * redPicSize);
                }
                int gpuValue = 0;
                //string gpuProcesses;
                if (gpuOk)
                {
                    try
                    {
                        gpuValue = int.Parse(GetGpuData(@"--query-gpu=utilization.gpu --format=csv,noheader,nounits"));
                        //gpuProcesses = GetGpuData(@"--query-compute-apps=process_name,used_memory --format=csv,noheader,nounits");
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.ErrorOutput("GPUデータ取得エラー", ex.Message, true);
                        gpuValue = 0;
                        gpuOk = false;
                    }
                    if (!slideLeft)
                    {
                        GPULabel.Text = "GPU使用率: " + gpuValue + "(%)";
                        RedPic4.Width = (int)((float)gpuValue / 100.0f * redPicSize);
                    }
                    if (gpuValue >= 90)
                    {
                        cwList[3].errorTimes++;
                        GPULabel.ForeColor = Color.Red;
                    }
                    else
                    {
                        cwList[3].errorTimes = 0;
                        GPULabel.ForeColor = Color.Black;
                    }
                }
                else
                {
                    GPULabel.Text = "取得不可";
                }
                float busyPer = pcList[2].NextValue();

                if (busyPer >= 100.0f)
                {
                    busyPer = 100.0f;
                    cwList[4].errorTimes++;
                    BusyLabel.ForeColor = Color.Red;
                }
                else if (busyPer >= 50.0f)
                {
                    BusyLabel.ForeColor = Color.Yellow;
                }
                else
                {
                    cwList[4].errorTimes = 0;
                    BusyLabel.ForeColor = Color.Black;
                }
                if (!slideLeft)
                {
                    RedPic5.Width = (int)(busyPer / 100.0f * redPicSize);
                    BusyLabel.Text = "ディスク使用率: " + busyPer.ToString("F2") + "(%)";
                }
                ValueManager process = new ValueManager(Process.GetCurrentProcess().WorkingSet64);

                ValueManager maxProcess = new ValueManager(Process.GetCurrentProcess().PeakWorkingSet64);

                double usingPercentage = ((double)process.num / actTotalMem.num * 100.0);

                Process[] processes = Process.GetProcesses();

                string taskStr = string.Empty;
                string heavyName = string.Empty;
                long heavyMem = 0;

                foreach (Process p in processes)
                {
                    if (p.WorkingSet64 > heavyMem)
                    {
                        heavyMem = p.WorkingSet64;
                        heavyName = p.ProcessName;
                    }
                    taskStr += heavyName + ", " + heavyMem + LB;
                }

                //現在ゲームを起動中であると予想された場合にそのゲームを記録
                if (gpuValue >= 50)//gpuValueの閾値は変更できるようにする
                {
                    GameLabel.Text = heavyName + "を実行中";
                }

                if (!slideLeft) toolTip1.SetToolTip(MemUsingPic, "Moniメモリ使用量: " + process.data.ToString("F1") + "(" +
                    process.HeadToString() + "Bytes) " + usingPercentage.ToString("F1") + "%" +
                    LB + "最大: " + maxProcess.data.ToString("F1") + "(" + maxProcess.HeadToString() + "Bytes)" +
                    LB + "GC周期: " + GCTimer.Interval + "ms");


                if (SaveData.enableLogSave)
                {
                    if (gpuOk)
                    {
                        LogStr += dt.ToString("HH:mm:ss") + "," + receive.data.ToString("F0") + "," + receive.HeadToString() + "," + sent.data.ToString("F0") + "," + sent.HeadToString() + "," +
                            cpuUses.ToString("F2") + "," + avaMem.ToString() + "," + ac.HEAD_NORMAL[memHead] + "," + gpuValue + "," +
                            busyPer.ToString("F2") + LB;
                        LogManager.SetState(0, "正常");
                    }
                    else
                    {
                        LogStr += dt.ToString("HH:mm:ss") + "," + receive.data.ToString("F0") + "," + receive.HeadToString() + "," + sent.data.ToString("F0") + "," + sent.HeadToString() + "," +
                            cpuUses.ToString("F2") + "," + avaMem.ToString() + "," + ac.HEAD_NORMAL[memHead] + "," + 0 + "," +
                            busyPer.ToString("F2") + LB;
                        LogManager.SetState(1, "異常あり");
                    }
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(@".\LogData\" + dt.ToString("yyyy_MM_dd") + "ResourcesLog.tc", true))
                        {
                            sw.Write(LogStr);
                        }
                        LogStr = string.Empty;
                    }
                    catch (UnauthorizedAccessException)
                    {

                    }
                    catch (DirectoryNotFoundException)
                    {
                        LogManager.SetState(1, "ディレクトリ生成中");
                        Directory.CreateDirectory(@".\LogData");
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.ErrorOutput("リソース保存失敗", ex.Message, false);
                    }
                }
                else
                {
                    LogManager.SetState(3, "停止中");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("リソース情報処理で不明なエラー", ex + "" + ex.Message, false);
            }
        }

        private bool slideLeft = true;

        private void panel2_Click(object sender, EventArgs e)
        {
            InfoSlideTimer.Enabled = true;
        }

        private void InfoSlideTimer_Tick(object sender, EventArgs e)
        {
            if (slideLeft)
            {
                if (panel1.Left - moveLength > -210)
                {
                    panel1.Left -= moveLength;
                }
                else
                {
                    panel1.Left = -210;
                    InfoSlideTimer.Enabled = false;
                    slideLeft = false;
                }
            }
            else
            {
                if (panel1.Left + moveLength < 0)
                {
                    panel1.Left += moveLength;
                }
                else
                {
                    panel1.Left = 0;
                    InfoSlideTimer.Enabled = false;
                    slideLeft = true;
                }
            }
        }

        private async void Clock_FormClosing(object sender, FormClosingEventArgs e)
        {
            FaceTimer.Enabled = false;
            ResourceTimer.Enabled = false;
            AlarmTimer.Enabled = false;
            DateTimer.Enabled = false;
            mt.IsSafe();
            if (AnalyticsClass.analysisTask != null)
            {
                await AnalyticsClass.analysisTask;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            SaveData.enableLogSave = !SaveData.enableLogSave;
            if (SaveData.enableLogSave == false)
            {
                LogUpdateLabel.Text = "停止";
            }
            SaveData.DataSave();
        }

        private int faceTimer = 0;
        private int breakingTimer = 0;
        private int defMabataki = 50;
        private int mabataki = 50;

        private void FaceTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Random rand = new Random();
                if (((DateTime.Now - windowMovingTime).TotalSeconds < 1) && ((DateTime.Now - bootTime).TotalSeconds > 3))
                {
                    FacePic.Image = Properties.Resources.face_scary;
                }
                else if (cwList[1].errorFlg)
                {
                    if (faceTimer % 2 == 0)
                    {
                        FacePic.Image = Properties.Resources.face_cpu_0;
                    }
                    else
                    {
                        FacePic.Image = Properties.Resources.face_cpu_1;
                    }
                }
                else if (cwList[4].errorFlg)
                {
                    if (faceTimer % 2 == 0)
                    {
                        FacePic.Image = Properties.Resources.face_disk_0;
                    }
                    else
                    {
                        FacePic.Image = Properties.Resources.face_disk_1;
                    }
                }
                else if (cwList[0].errorFlg)
                {
                    if (faceTimer % 2 == 0)
                    {
                        FacePic.Image = Properties.Resources.face_net_0;
                    }
                    else
                    {
                        FacePic.Image = Properties.Resources.face_net_1;
                    }
                }
                else if (cwList[2].errorFlg)
                {
                    if (faceTimer % 2 == 0)
                    {
                        FacePic.Image = Properties.Resources.face_mem_0;
                    }
                    else
                    {
                        FacePic.Image = Properties.Resources.face_mem_1;
                    }
                }
                else if (cwList[3].errorFlg && gpuOk)
                {
                    if (faceTimer % 2 == 0)
                    {
                        FacePic.Image = Properties.Resources.face_gpu_0;
                    }
                    else
                    {
                        FacePic.Image = Properties.Resources.face_gpu_1;
                    }
                }
                else
                {
                    if (breakingTimer > 6000 && dm.CheckNight())
                    {
                        if (faceTimer % 40 < 20)
                        {
                            FacePic.Image = Properties.Resources.face_sleep1;
                        }
                        else
                        {
                            FacePic.Image = Properties.Resources.face_sleep2;
                        }
                    }
                    else
                    {
                        if (faceTimer % mabataki == 0)
                        {
                            mabataki = defMabataki;
                            FacePic.Image = Properties.Resources.face_2;
                            if (dm.CheckNight())
                            {
                                defMabataki = 40;
                            }
                            else
                            {
                                defMabataki = 50;
                            }
                            mabataki += rand.Next(-10, 11);
                        }
                        else if (ac.overallBusyPer >= 70.0)
                        {
                            FacePic.Image = Properties.Resources.face_sleepy;
                        }
                        else
                        {
                            FacePic.Image = Properties.Resources.face_0;
                        }
                    }
                    breakingTimer++;
                }
                foreach (Bottleneck cw in cwList)
                {
                    if (cw.errorFlg)
                    {
                        breakingTimer = 0;
                    }
                }
                faceTimer++;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("Moniの表情変更で不明なエラー", ex + "" + ex.Message, false);
            }
        }

        private void FacePic_Click(object sender, EventArgs e)
        {
            breakingTimer = 0;
            if (AlarmBox.Checked == true && AlarmTimer.Enabled == true)
            {
                AlarmTimer.Enabled = false;
                AlarmBox.Checked = false;
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            HelpPanel.Top = -vScrollBar1.Value;
        }

        private void AlarmTimer_Tick(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                Console.Beep();
            });
        }

        private void DriveUD_SelectedItemChanged(object sender, EventArgs e)
        {
            if (ready)
            {
                string driveName;
                if ((string)DriveUD.SelectedItem == "ALL")
                {
                    driveName = "_Total";
                }
                else
                {
                    driveName = DriveUD.SelectedItem + "";
                }
                try
                {
                    pcList[2] = new PerformanceCounter("PhysicalDisk", "% Disk Time", driveName, ".");
                    SaveData.DataSave();

                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorOutput("ドライブ選択エラー", ex.Message, true);
                }
            }
        }

        private int logUpdateTime = 240;
        private const int UpdateInterval = 300;

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            if (!ready) return;
            logUpdateTime++;
            if (logUpdateTime > UpdateInterval && !ac.isBusy)
            {
                logUpdateTime = 0;
                UpdateLog();
            }
            if (!slideLeft)
            {
                if (UpdateInterval - logUpdateTime >= 60)
                {
                    LogUpdateLabel.Text = "更新:" + (int)((UpdateInterval - logUpdateTime) / 60 + 1) + "m";
                }
                else
                {
                    LogUpdateLabel.Text = "更新:" + (UpdateInterval - logUpdateTime);
                }
            }
        }

        /// <summary>
        /// ログの解析を開始する
        /// </summary>
        private void UpdateLog()
        {
            try
            {
                try
                {
                    string[] files = Directory.GetFiles(
                                @".\LogData\", "*.tc", SearchOption.AllDirectories);
                    if (files.Length == 0)
                    {
                        AnalyticsLabel.Text = "ログの保存が未許可";
                        return;
                    }
                }
                catch (Exception)
                {
                    AnalyticsLabel.Text = "ログの取得失敗";
                    return;
                }
                ac.AnalysisLogFiles();
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("ログ解析エラー", ex.Message, true);
            }
            return;
        }

        private void SetDesc()
        {
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            int j = 0;

            foreach (DriveInfo driveInfo in driveInfos)
            {
                if (driveInfo.DriveType == DriveType.Fixed)
                {
                    string name = driveInfo.Name;
                    name = name.Trim('\\');
                    DriveUD.Items.Add(j + " " + name);
                    j++;
                }
            }
            DescBox.Text = "コンピュータ情報" + LB;
            /*
            using (ManagementClass mc = new ManagementClass("Win32_OperatingSystem"))
            {
                mc.Get();
                mc.Scope.Options.EnablePrivileges = true;
                float memorySize;
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    string memStr;
                    foreach (ManagementObject mo in moc)
                    {
                        memStr = mo["TotalVisibleMemorySize"] + LB;
                        memorySize = float.Parse(memStr);
                        DescBox.Text += mo["Caption"] + LB;
                        mo.Dispose();
                        actTotalMem = new ValueManager((long)(memorySize * 1000.0f));
                        DescBox.Text += "物理メモリ数: " + actTotalMem.data.ToString("F2") + actTotalMem.HeadToString() + "B" + LB;
                    }
                }
            }
            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                mc.Get();
                mc.Scope.Options.EnablePrivileges = true;
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        DescBox.Text += mo["Name"] + LB;
                        mo.Dispose();
                    }
                }
            }
            */

            if (this.nvidiaSmiFile != null)
            {
                try
                {
                    string result = GetGpuData(@"--query-gpu=name --format=csv,noheader,nounits");
                    DescBox.Text += result;
                    gpuOk = true;
                }
                catch (Exception)
                {
                    SearchGpuFile();
                    if (nvidiaSmiFile != null)
                    {
                        Close();
                    }
                }
            }
            else
            {
                DescBox.Text += "GPUのデータが参照できません" + LB;
            }
            DescBox.Text += LB + "ドライブ情報" + LB;
            foreach (DriveInfo driveInfo in driveInfos)
            {
                if (driveInfo.DriveType == DriveType.Fixed)
                {
                    ValueManager valueManager = new ValueManager(driveInfo.TotalSize);
                    double avaPer = ((double)valueManager.num - driveInfo.AvailableFreeSpace) / valueManager.num * 100.0;
                    DescBox.Text += driveInfo.Name + ": " + valueManager.data.ToString("F0") + valueManager.HeadToString() + "B " + avaPer.ToString("F1") + "%使用中(" + driveInfo.DriveFormat + ")" + LB;
                }
            }
            DescBox.Text += LB + DateTime.Now.ToString() + "に更新" + LB;
        }

        private void NetName_MouseEnter(object sender, EventArgs e)
        {
            NetNameTimer.Enabled = true;
        }

        private void NetNameTimer_Tick(object sender, EventArgs e)
        {
            if (NetName.Left >= -199)
            {
                NetName.Left -= 1;
            }
            else
            {
                NetName.Left = 5;
                NetNameTimer.Enabled = false;
            }
        }

        private void ClosePic_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MinimizePic_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void NetPic_Click(object sender, EventArgs e)
        {
            Process.Start("https://sites.google.com/view/moni-toppage/%E3%83%9B%E3%83%BC%E3%83%A0");
        }

        private void AlarmBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AlarmBox.Checked)
            {
                AlarmPanel.Enabled = true;
            }
            else
            {
                AlarmPanel.Enabled = false;
            }
        }

        private void 顔色変更ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("顔の色を変更します" + LB + "変更しますか?",
                "顔色指定", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                ColorDialog CD = new ColorDialog();
                CD.Color = Color.LightSkyBlue;
                if (CD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SaveData.faceColor = ColorTranslator.ToWin32(CD.Color);
                    SaveData.DataSave();
                }
            }
        }

        private void スタートアップに指定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dm.SetStartUp();
        }

        private void SaveStyleUD_SelectedItemChanged(object sender, EventArgs e)
        {
            if (!ready) return;
            SaveData.DataSave();
            try
            {
                File.Move(@".\tcData\MaxSpeed.dat", @".\tcData\MaxSpeed" + DateTime.Now.ToString() + ".dat");
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("MaxSpeedファイル変更処理中にエラー", ex.Message, false);
            }
        }


        private void GCTimer_Tick(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void resourceUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!ready) return;
            SetDesc();
        }

        private void MinimizePic_MouseEnter(object sender, EventArgs e)
        {
            MinimizePic.Image = Properties.Resources.minimum;
        }

        private void MinimizePic_MouseLeave(object sender, EventArgs e)
        {
            MinimizePic.Image = null;
        }

        private void ClosePic_MouseEnter(object sender, EventArgs e)
        {
            ClosePic.Image = Properties.Resources.batu;
        }

        private void ClosePic_MouseLeave(object sender, EventArgs e)
        {
            ClosePic.Image = null;
        }

        private bool updateSlideRight = true;

        private void UpdatePanel_MouseEnter(object sender, EventArgs e)
        {
            updateSlideRight = true;
            UpdateAlert.Enabled = true;
        }

        private void UpdatePanel_MouseLeave(object sender, EventArgs e)
        {
            updateSlideRight = false;
            UpdateAlert.Enabled = true;
        }

        private void UpdateAlert_Tick(object sender, EventArgs e)
        {
            if (updateSlideRight)
            {
                if (UpdatePanel.Left < 0)
                {
                    UpdatePanel.Left += 3;
                }
                else
                {
                    UpdatePanel.Left = 0;
                    UpdateAlert.Enabled = false;
                }
            }
            else
            {
                if (UpdatePanel.Left > -147)
                {
                    UpdatePanel.Left -= 3;
                }
                else
                {
                    UpdatePanel.Left = -147;
                    UpdateAlert.Enabled = false;
                }
            }
        }

        private void Clock_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (UpdatePanel.Visible == true)
            {
                try
                {
                    Process proc = new Process();

                    proc.StartInfo.FileName = @"MoniInstaller.exe";
                    proc.Start();
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorOutput("アプリ更新エラー", ex + ex.Message, true);
                }
            }
        }

        DateTime windowMovingTime;

        private void Clock_Move(object sender, EventArgs e)
        {
            //Control c = (Control)sender;
            windowMovingTime = DateTime.Now;
        }

        private void Clock_Activated(object sender, EventArgs e)
        {
            this.Opacity = 1.0f;
        }

        private void Clock_Deactivate(object sender, EventArgs e)
        {
            try
            {
                if (SaveData.transparent && this != null)
                {
                    this.Opacity = 0.5f;
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                //アプリ終了した際に発生する
                //多分消えたアプリに値を設定できない?
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            try
            {
                string apiText = apiTextBox.Text;
                if (apiText != null && apiText != "")
                {
                    using (StreamWriter sw = new StreamWriter(apiFileName, false))
                    {
                        sw.WriteLine(apiText);
                    }
                }
                else
                {
                    ErrorLog.ErrorOutput("APIキーエラー", "APIキーが無効です", true);
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("APIキー保存エラー", ex.Message, true);
            }
        }

        private int apiUpdateNum = 0;
        private List<string> newsList = new List<string>();
        private int currentNews = 0;

        private void apiTimer_Tick(object sender, EventArgs e)
        {
        }

        private void NewsMover_Tick(object sender, EventArgs e)
        {
            apiLabel.Left -= 10;
            if (apiLabel.Right <= 2)
            {
                apiLabel.Left = 215;
                currentNews++;
                if (currentNews >= 20) currentNews = 0;
                apiLabel.Text = newsList[currentNews];
            }
        }
    }
}
