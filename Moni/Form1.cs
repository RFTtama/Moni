using Microsoft.Win32;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Moni
{
    public partial class Clock : Form
    {
        private readonly string[] HEAD_NORMAL = new string[] { "", "k", "M", "G", "T", "P" };
        private MoniTerminator mt = new MoniTerminator();
        private DateTime dt;
        private string nvidiaSmiFile;
        private const string LB = "\r\n";
        private int tellBef;
        public int redPicSize;
        private ValueManager actTotalMem;
        private readonly string gpuFileName = @".\tcData\fileName.txt";
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
        private List<BottleNeckClass> cwList = new List<BottleNeckClass>();
        private int day;
        private DateTime bootTime = DateTime.Now;

        public Clock()
        {
            LoadIcon._form = this;
            LoadingProcess loading = new LoadingProcess();
            try
            {
                InitializeComponent();

                SystemEvents.SessionEnding += new SessionEndingEventHandler(Detect_EndWindows);
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
                this.redPicSize = RedPic2.Width;
                System.Reflection.Assembly asm =
                    System.Reflection.Assembly.GetExecutingAssembly();
                System.Version ver = asm.GetName().Version;
                VersionLabel.Text = "Ver " + ver.ToString();
                this.tellBef = -1;

                DriveUD.Items.Add("ALL");
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
                cwList.Add(new BottleNeckClass("NetWorkWarning"));
                cwList.Add(new BottleNeckClass("CPUWarning"));
                cwList.Add(new BottleNeckClass("MemoryWarning"));
                cwList.Add(new BottleNeckClass("GPUWarning"));
                cwList.Add(new BottleNeckClass("DiskWarning"));
                ResourceTimer.Enabled = true;
                FaceTimer.Enabled = true;
                DateTimer.Enabled = true;

                this._ready = true;

                Splash.Close();

            }
            catch (Exception ex)
            {
                ErrorLog.ErrorOutput("初期化時に不明なエラー", ex + ex.Message, true);
                this.Close();
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
                this.day = dt.Day;
                this.DateLabel.Text = dt.ToString("yyyy/MM/dd");
                this.SecondLabel.Text = dt.ToString("ss");
                this.TimeLabel.Text = dt.ToString("HH:mm");
                int min = int.Parse(dt.ToString("mm"));
                int hour = int.Parse(dt.ToString("HH"));
                if (min == 0 && this.tellBef != hour && checkBox3.Enabled == true)
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

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

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
                    MemoryLabel.Text = "使用可能メモリ: " + avaMem.ToString("F2") + "(" + HEAD_NORMAL[memHead] + "B)";
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
                    LB + "最大: " + maxProcess.data.ToString("F1") + "(" + maxProcess.HeadToString() + "Bytes)" );
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
                    if (breakingTimer > 6000 && CheckNight())
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
                            if (CheckNight())
                            {
                                defMabataki = 40;
                            }
                            else
                            {
                                defMabataki = 50;
                            }
                            mabataki += rand.Next(-10, 11);
                        }
                        else
                        {
                            FacePic.Image = Properties.Resources.face_0;
                        }
                    }
                    breakingTimer++;
                }
                foreach (BottleNeckClass cw in cwList)
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

                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorOutput("ドライブ選択エラー", ex.Message, true);
                }
            }
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
                    ColorTranslator.ToWin32(CD.Color);
                }
            }
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
            }
            catch (System.ComponentModel.Win32Exception)
            {
                //アプリ終了した際に発生する
                //多分消えたアプリに値を設定できない?
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
