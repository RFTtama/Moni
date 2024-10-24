
namespace Moni
{
    partial class Clock
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            TimeLabel = new Label();
            DateTimer = new System.Windows.Forms.Timer(components);
            DateLabel = new Label();
            SecondLabel = new Label();
            HelpPanel = new Panel();
            MemUsingPic = new PictureBox();
            label6 = new Label();
            DriveUD = new DomainUpDown();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            Sent = new Label();
            NetName = new Label();
            RedPic2 = new PictureBox();
            label3 = new Label();
            RedPic5 = new PictureBox();
            BluePic5 = new PictureBox();
            BusyLabel = new Label();
            VersionLabel = new Label();
            RedPic4 = new PictureBox();
            BluePic4 = new PictureBox();
            RedPic3 = new PictureBox();
            GPULabel = new Label();
            Received = new Label();
            CPULabel = new Label();
            MaxMemLabel = new Label();
            BluePic3 = new PictureBox();
            BluePic2 = new PictureBox();
            MemoryLabel = new Label();
            ResourceTimer = new System.Windows.Forms.Timer(components);
            panel1 = new Panel();
            vScrollBar1 = new VScrollBar();
            panel2 = new Panel();
            LoadPic = new PictureBox();
            NewsLabel = new Label();
            MinimizePic = new PictureBox();
            ClosePic = new PictureBox();
            GameLabel = new Label();
            FacePic = new PictureBox();
            FaceMenuStrip = new ContextMenuStrip(components);
            顔色変更ToolStripMenuItem = new ToolStripMenuItem();
            メニュー切り替えToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            その他ToolStripMenuItem = new ToolStripMenuItem();
            最新情報を見るToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            最小化ToolStripMenuItem = new ToolStripMenuItem();
            終了ToolStripMenuItem = new ToolStripMenuItem();
            InfoSlideTimer = new System.Windows.Forms.Timer(components);
            FaceTimer = new System.Windows.Forms.Timer(components);
            toolTip1 = new ToolTip(components);
            NetNameTimer = new System.Windows.Forms.Timer(components);
            HelpPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MemUsingPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RedPic2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RedPic5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BluePic5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RedPic4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BluePic4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RedPic3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BluePic3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BluePic2).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LoadPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MinimizePic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ClosePic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FacePic).BeginInit();
            FaceMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // TimeLabel
            // 
            TimeLabel.AutoSize = true;
            TimeLabel.Font = new Font("MS UI Gothic", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            TimeLabel.Location = new Point(1, 2);
            TimeLabel.Margin = new Padding(4, 0, 4, 0);
            TimeLabel.Name = "TimeLabel";
            TimeLabel.Size = new Size(79, 29);
            TimeLabel.TabIndex = 0;
            TimeLabel.Text = "00:00";
            TimeLabel.Visible = false;
            // 
            // DateTimer
            // 
            DateTimer.Tick += DateTimer_Tick;
            // 
            // DateLabel
            // 
            DateLabel.AutoSize = true;
            DateLabel.Font = new Font("MS UI Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            DateLabel.Location = new Point(2, 175);
            DateLabel.Margin = new Padding(4, 0, 4, 0);
            DateLabel.Name = "DateLabel";
            DateLabel.Size = new Size(35, 13);
            DateLabel.TabIndex = 2;
            DateLabel.Text = "0000";
            DateLabel.Visible = false;
            // 
            // SecondLabel
            // 
            SecondLabel.AutoSize = true;
            SecondLabel.Font = new Font("MS UI Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            SecondLabel.Location = new Point(85, 21);
            SecondLabel.Margin = new Padding(4, 0, 4, 0);
            SecondLabel.Name = "SecondLabel";
            SecondLabel.Size = new Size(21, 13);
            SecondLabel.TabIndex = 3;
            SecondLabel.Text = "00";
            SecondLabel.Visible = false;
            // 
            // HelpPanel
            // 
            HelpPanel.BackColor = Color.LightSteelBlue;
            HelpPanel.Controls.Add(MemUsingPic);
            HelpPanel.Controls.Add(label6);
            HelpPanel.Controls.Add(DriveUD);
            HelpPanel.Controls.Add(pictureBox2);
            HelpPanel.Controls.Add(pictureBox1);
            HelpPanel.Controls.Add(Sent);
            HelpPanel.Controls.Add(NetName);
            HelpPanel.Controls.Add(RedPic2);
            HelpPanel.Controls.Add(label3);
            HelpPanel.Controls.Add(RedPic5);
            HelpPanel.Controls.Add(BluePic5);
            HelpPanel.Controls.Add(BusyLabel);
            HelpPanel.Controls.Add(VersionLabel);
            HelpPanel.Controls.Add(RedPic4);
            HelpPanel.Controls.Add(BluePic4);
            HelpPanel.Controls.Add(RedPic3);
            HelpPanel.Controls.Add(GPULabel);
            HelpPanel.Controls.Add(Received);
            HelpPanel.Controls.Add(CPULabel);
            HelpPanel.Controls.Add(MaxMemLabel);
            HelpPanel.Controls.Add(BluePic3);
            HelpPanel.Controls.Add(BluePic2);
            HelpPanel.Controls.Add(MemoryLabel);
            HelpPanel.Location = new Point(251, 0);
            HelpPanel.Margin = new Padding(4);
            HelpPanel.Name = "HelpPanel";
            HelpPanel.Size = new Size(238, 1111);
            HelpPanel.TabIndex = 5;
            // 
            // MemUsingPic
            // 
            MemUsingPic.Image = Properties.Resources.face_0;
            MemUsingPic.Location = new Point(200, 62);
            MemUsingPic.Margin = new Padding(4);
            MemUsingPic.Name = "MemUsingPic";
            MemUsingPic.Size = new Size(23, 25);
            MemUsingPic.SizeMode = PictureBoxSizeMode.Zoom;
            MemUsingPic.TabIndex = 84;
            MemUsingPic.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(169, 156);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(41, 15);
            label6.TabIndex = 44;
            label6.Text = "ドライブ";
            // 
            // DriveUD
            // 
            DriveUD.Location = new Point(170, 172);
            DriveUD.Margin = new Padding(4);
            DriveUD.Name = "DriveUD";
            DriveUD.ReadOnly = true;
            DriveUD.Size = new Size(46, 23);
            DriveUD.TabIndex = 43;
            DriveUD.Text = "ALL";
            toolTip1.SetToolTip(DriveUD, "指定したドライブの情報を表示します");
            DriveUD.SelectedItemChanged += DriveUD_SelectedItemChanged;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.downArrow;
            pictureBox2.Location = new Point(4, 223);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(18, 19);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 42;
            pictureBox2.TabStop = false;
            toolTip1.SetToolTip(pictureBox2, "下り(受信)の通信速度です");
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.upArrow;
            pictureBox1.Location = new Point(115, 223);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(18, 19);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 41;
            pictureBox1.TabStop = false;
            toolTip1.SetToolTip(pictureBox1, "上り(送信)の通信速度です");
            // 
            // Sent
            // 
            Sent.AutoSize = true;
            Sent.Location = new Point(132, 225);
            Sent.Margin = new Padding(4, 0, 4, 0);
            Sent.Name = "Sent";
            Sent.Size = new Size(52, 15);
            Sent.TabIndex = 40;
            Sent.Text = "取得中…";
            toolTip1.SetToolTip(Sent, "上り(送信)の通信速度です");
            // 
            // NetName
            // 
            NetName.AutoSize = true;
            NetName.Location = new Point(6, 206);
            NetName.Margin = new Padding(4, 0, 4, 0);
            NetName.Name = "NetName";
            NetName.Size = new Size(86, 15);
            NetName.TabIndex = 39;
            NetName.Text = "リソース取得中…";
            toolTip1.SetToolTip(NetName, "ネットワークインタフェースの名称です\r\n内蔵のネットワークインタフェースが複数ある場合は\r\n使用率が一番高いものが指定されます");
            NetName.MouseEnter += NetName_MouseEnter;
            // 
            // RedPic2
            // 
            RedPic2.Image = Properties.Resources.red;
            RedPic2.Location = new Point(5, 48);
            RedPic2.Margin = new Padding(4);
            RedPic2.Name = "RedPic2";
            RedPic2.Size = new Size(154, 12);
            RedPic2.SizeMode = PictureBoxSizeMode.StretchImage;
            RedPic2.TabIndex = 11;
            RedPic2.TabStop = false;
            toolTip1.SetToolTip(RedPic2, "CPUの使用率です\r\n数値が高い状態が長く続いているほど\r\nボトルネックの可能性が高いです\r\n");
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("MS UI Gothic", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            label3.Location = new Point(9, 8);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(31, 12);
            label3.TabIndex = 33;
            label3.Text = "情報";
            // 
            // RedPic5
            // 
            RedPic5.Image = Properties.Resources.red;
            RedPic5.Location = new Point(5, 176);
            RedPic5.Margin = new Padding(4);
            RedPic5.Name = "RedPic5";
            RedPic5.Size = new Size(154, 12);
            RedPic5.SizeMode = PictureBoxSizeMode.StretchImage;
            RedPic5.TabIndex = 27;
            RedPic5.TabStop = false;
            toolTip1.SetToolTip(RedPic5, "ドライブの使用率です\r\n数値が高い状態が長く続いているほど\r\nボトルネックの可能性が高いです");
            // 
            // BluePic5
            // 
            BluePic5.Image = Properties.Resources.blue;
            BluePic5.Location = new Point(5, 176);
            BluePic5.Margin = new Padding(4);
            BluePic5.Name = "BluePic5";
            BluePic5.Size = new Size(154, 12);
            BluePic5.SizeMode = PictureBoxSizeMode.StretchImage;
            BluePic5.TabIndex = 28;
            BluePic5.TabStop = false;
            toolTip1.SetToolTip(BluePic5, "ドライブの使用率です\r\n数値が高い状態が長く続いているほど\r\nボトルネックの可能性が高いです");
            // 
            // BusyLabel
            // 
            BusyLabel.AutoSize = true;
            BusyLabel.Location = new Point(9, 158);
            BusyLabel.Margin = new Padding(4, 0, 4, 0);
            BusyLabel.Name = "BusyLabel";
            BusyLabel.Size = new Size(86, 15);
            BusyLabel.TabIndex = 26;
            BusyLabel.Text = "リソース取得中…";
            toolTip1.SetToolTip(BusyLabel, "ドライブの使用率です\r\n数値が高い状態が長く続いているほど\r\nボトルネックの可能性が高いです");
            // 
            // VersionLabel
            // 
            VersionLabel.AutoSize = true;
            VersionLabel.Location = new Point(145, 11);
            VersionLabel.Margin = new Padding(4, 0, 4, 0);
            VersionLabel.Name = "VersionLabel";
            VersionLabel.Size = new Size(59, 15);
            VersionLabel.TabIndex = 2;
            VersionLabel.Text = "Ver 0.0.0.0";
            // 
            // RedPic4
            // 
            RedPic4.Image = Properties.Resources.red;
            RedPic4.Location = new Point(5, 132);
            RedPic4.Margin = new Padding(4);
            RedPic4.Name = "RedPic4";
            RedPic4.Size = new Size(154, 12);
            RedPic4.SizeMode = PictureBoxSizeMode.StretchImage;
            RedPic4.TabIndex = 23;
            RedPic4.TabStop = false;
            toolTip1.SetToolTip(RedPic4, "GPUの使用率です\r\nゲームをプレイしているときなどは\r\n数値が高くなる傾向にあります\r\n");
            // 
            // BluePic4
            // 
            BluePic4.Image = Properties.Resources.blue;
            BluePic4.Location = new Point(5, 132);
            BluePic4.Margin = new Padding(4);
            BluePic4.Name = "BluePic4";
            BluePic4.Size = new Size(154, 12);
            BluePic4.SizeMode = PictureBoxSizeMode.StretchImage;
            BluePic4.TabIndex = 24;
            BluePic4.TabStop = false;
            toolTip1.SetToolTip(BluePic4, "GPUの使用率です\r\nゲームをプレイしているときなどは\r\n数値が高くなる傾向にあります\r\n");
            // 
            // RedPic3
            // 
            RedPic3.Image = Properties.Resources.red;
            RedPic3.Location = new Point(5, 89);
            RedPic3.Margin = new Padding(4);
            RedPic3.Name = "RedPic3";
            RedPic3.Size = new Size(154, 12);
            RedPic3.SizeMode = PictureBoxSizeMode.StretchImage;
            RedPic3.TabIndex = 14;
            RedPic3.TabStop = false;
            toolTip1.SetToolTip(RedPic3, "メモリの使用率です\r\n数値が高い状態が長く続いているほど\r\nボトルネックの可能性が高いです\r\n");
            // 
            // GPULabel
            // 
            GPULabel.AutoSize = true;
            GPULabel.Location = new Point(9, 113);
            GPULabel.Margin = new Padding(4, 0, 4, 0);
            GPULabel.Name = "GPULabel";
            GPULabel.Size = new Size(86, 15);
            GPULabel.TabIndex = 22;
            GPULabel.Text = "リソース取得中…";
            toolTip1.SetToolTip(GPULabel, "GPUの使用率です\r\nゲームをプレイしているときなどは\r\n数値が高くなる傾向にあります\r\n");
            // 
            // Received
            // 
            Received.AutoSize = true;
            Received.Location = new Point(20, 225);
            Received.Margin = new Padding(4, 0, 4, 0);
            Received.Name = "Received";
            Received.Size = new Size(52, 15);
            Received.TabIndex = 6;
            Received.Text = "取得中…";
            toolTip1.SetToolTip(Received, "下り(受信)の通信速度です");
            // 
            // CPULabel
            // 
            CPULabel.AutoSize = true;
            CPULabel.Location = new Point(12, 29);
            CPULabel.Margin = new Padding(4, 0, 4, 0);
            CPULabel.Name = "CPULabel";
            CPULabel.Size = new Size(86, 15);
            CPULabel.TabIndex = 7;
            CPULabel.Text = "リソース取得中…";
            toolTip1.SetToolTip(CPULabel, "CPUの使用率です\r\n数値が高い状態が長く続いているほど\r\nボトルネックの可能性が高いです\r\n");
            // 
            // MaxMemLabel
            // 
            MaxMemLabel.AutoSize = true;
            MaxMemLabel.Location = new Point(159, 88);
            MaxMemLabel.Margin = new Padding(4, 0, 4, 0);
            MaxMemLabel.Name = "MaxMemLabel";
            MaxMemLabel.Size = new Size(43, 15);
            MaxMemLabel.TabIndex = 16;
            MaxMemLabel.Text = "取得中";
            toolTip1.SetToolTip(MaxMemLabel, "コンピュータの最大メモリです");
            // 
            // BluePic3
            // 
            BluePic3.Image = Properties.Resources.blue;
            BluePic3.Location = new Point(5, 89);
            BluePic3.Margin = new Padding(4);
            BluePic3.Name = "BluePic3";
            BluePic3.Size = new Size(154, 12);
            BluePic3.SizeMode = PictureBoxSizeMode.StretchImage;
            BluePic3.TabIndex = 15;
            BluePic3.TabStop = false;
            toolTip1.SetToolTip(BluePic3, "メモリの使用率です\r\n数値が高い状態が長く続いているほど\r\nボトルネックの可能性が高いです\r\n");
            // 
            // BluePic2
            // 
            BluePic2.Image = Properties.Resources.blue;
            BluePic2.Location = new Point(5, 48);
            BluePic2.Margin = new Padding(4);
            BluePic2.Name = "BluePic2";
            BluePic2.Size = new Size(154, 12);
            BluePic2.SizeMode = PictureBoxSizeMode.StretchImage;
            BluePic2.TabIndex = 12;
            BluePic2.TabStop = false;
            toolTip1.SetToolTip(BluePic2, "CPUの使用率です\r\n数値が高い状態が長く続いているほど\r\nボトルネックの可能性が高いです\r\n");
            // 
            // MemoryLabel
            // 
            MemoryLabel.AutoSize = true;
            MemoryLabel.Location = new Point(9, 70);
            MemoryLabel.Margin = new Padding(4, 0, 4, 0);
            MemoryLabel.Name = "MemoryLabel";
            MemoryLabel.Size = new Size(86, 15);
            MemoryLabel.TabIndex = 13;
            MemoryLabel.Text = "リソース取得中…";
            toolTip1.SetToolTip(MemoryLabel, "メモリの使用率です\r\n数値が高い状態が長く続いているほど\r\nボトルネックの可能性が高いです\r\n");
            // 
            // ResourceTimer
            // 
            ResourceTimer.Interval = 1000;
            ResourceTimer.Tick += ResourceTimer_Tick;
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightSkyBlue;
            panel1.Controls.Add(vScrollBar1);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(HelpPanel);
            panel1.Controls.Add(SecondLabel);
            panel1.Controls.Add(LoadPic);
            panel1.Controls.Add(NewsLabel);
            panel1.Controls.Add(MinimizePic);
            panel1.Controls.Add(ClosePic);
            panel1.Controls.Add(DateLabel);
            panel1.Controls.Add(TimeLabel);
            panel1.Controls.Add(GameLabel);
            panel1.Controls.Add(FacePic);
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(490, 1115);
            panel1.TabIndex = 7;
            // 
            // vScrollBar1
            // 
            vScrollBar1.LargeChange = 50;
            vScrollBar1.Location = new Point(478, 0);
            vScrollBar1.Maximum = 660;
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(10, 211);
            vScrollBar1.SmallChange = 30;
            vScrollBar1.TabIndex = 32;
            vScrollBar1.Scroll += vScrollBar1_Scroll;
            // 
            // panel2
            // 
            panel2.BackColor = Color.DodgerBlue;
            panel2.Location = new Point(240, -1);
            panel2.Margin = new Padding(4);
            panel2.Name = "panel2";
            panel2.Size = new Size(12, 212);
            panel2.TabIndex = 17;
            panel2.Click += panel2_Click;
            // 
            // LoadPic
            // 
            LoadPic.Location = new Point(210, 5);
            LoadPic.Margin = new Padding(4);
            LoadPic.Name = "LoadPic";
            LoadPic.Size = new Size(23, 25);
            LoadPic.SizeMode = PictureBoxSizeMode.Zoom;
            LoadPic.TabIndex = 35;
            LoadPic.TabStop = false;
            // 
            // NewsLabel
            // 
            NewsLabel.AutoSize = true;
            NewsLabel.Font = new Font("ＭＳ Ｐゴシック", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            NewsLabel.Location = new Point(4, 195);
            NewsLabel.Margin = new Padding(4, 0, 4, 0);
            NewsLabel.Name = "NewsLabel";
            NewsLabel.Size = new Size(0, 11);
            NewsLabel.TabIndex = 36;
            // 
            // MinimizePic
            // 
            MinimizePic.BackColor = Color.Transparent;
            MinimizePic.Location = new Point(210, 60);
            MinimizePic.Margin = new Padding(4);
            MinimizePic.Name = "MinimizePic";
            MinimizePic.Size = new Size(23, 25);
            MinimizePic.SizeMode = PictureBoxSizeMode.Zoom;
            MinimizePic.TabIndex = 34;
            MinimizePic.TabStop = false;
            MinimizePic.Click += MinimizePic_Click;
            MinimizePic.MouseEnter += MinimizePic_MouseEnter;
            MinimizePic.MouseLeave += MinimizePic_MouseLeave;
            // 
            // ClosePic
            // 
            ClosePic.BackColor = Color.Transparent;
            ClosePic.Location = new Point(210, 32);
            ClosePic.Margin = new Padding(4);
            ClosePic.Name = "ClosePic";
            ClosePic.Size = new Size(23, 25);
            ClosePic.SizeMode = PictureBoxSizeMode.Zoom;
            ClosePic.TabIndex = 33;
            ClosePic.TabStop = false;
            ClosePic.Click += ClosePic_Click;
            ClosePic.MouseEnter += ClosePic_MouseEnter;
            ClosePic.MouseLeave += ClosePic_MouseLeave;
            // 
            // GameLabel
            // 
            GameLabel.AutoSize = true;
            GameLabel.Font = new Font("ＭＳ Ｐゴシック", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            GameLabel.Location = new Point(131, 296);
            GameLabel.Margin = new Padding(4, 0, 4, 0);
            GameLabel.Name = "GameLabel";
            GameLabel.Size = new Size(0, 12);
            GameLabel.TabIndex = 38;
            // 
            // FacePic
            // 
            FacePic.ContextMenuStrip = FaceMenuStrip;
            FacePic.Image = Properties.Resources.face_1;
            FacePic.Location = new Point(13, 9);
            FacePic.Margin = new Padding(4);
            FacePic.Name = "FacePic";
            FacePic.Size = new Size(211, 212);
            FacePic.SizeMode = PictureBoxSizeMode.StretchImage;
            FacePic.TabIndex = 31;
            FacePic.TabStop = false;
            // 
            // FaceMenuStrip
            // 
            FaceMenuStrip.Items.AddRange(new ToolStripItem[] { 顔色変更ToolStripMenuItem, メニュー切り替えToolStripMenuItem, toolStripMenuItem1, その他ToolStripMenuItem, toolStripMenuItem2, 最小化ToolStripMenuItem, 終了ToolStripMenuItem });
            FaceMenuStrip.Name = "FaceMenuStrip";
            FaceMenuStrip.Size = new Size(149, 126);
            // 
            // 顔色変更ToolStripMenuItem
            // 
            顔色変更ToolStripMenuItem.Name = "顔色変更ToolStripMenuItem";
            顔色変更ToolStripMenuItem.Size = new Size(148, 22);
            顔色変更ToolStripMenuItem.Text = "顔色変更";
            顔色変更ToolStripMenuItem.Click += 顔色変更ToolStripMenuItem_Click;
            // 
            // メニュー切り替えToolStripMenuItem
            // 
            メニュー切り替えToolStripMenuItem.Name = "メニュー切り替えToolStripMenuItem";
            メニュー切り替えToolStripMenuItem.Size = new Size(148, 22);
            メニュー切り替えToolStripMenuItem.Text = "メニュー切り替え";
            メニュー切り替えToolStripMenuItem.Click += panel2_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(145, 6);
            // 
            // その他ToolStripMenuItem
            // 
            その他ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 最新情報を見るToolStripMenuItem });
            その他ToolStripMenuItem.Name = "その他ToolStripMenuItem";
            その他ToolStripMenuItem.Size = new Size(148, 22);
            その他ToolStripMenuItem.Text = "その他";
            // 
            // 最新情報を見るToolStripMenuItem
            // 
            最新情報を見るToolStripMenuItem.Name = "最新情報を見るToolStripMenuItem";
            最新情報を見るToolStripMenuItem.Size = new Size(152, 22);
            最新情報を見るToolStripMenuItem.Text = "最新情報を見る";
            最新情報を見るToolStripMenuItem.Click += NetPic_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(145, 6);
            // 
            // 最小化ToolStripMenuItem
            // 
            最小化ToolStripMenuItem.Name = "最小化ToolStripMenuItem";
            最小化ToolStripMenuItem.Size = new Size(148, 22);
            最小化ToolStripMenuItem.Text = "最小化";
            最小化ToolStripMenuItem.Click += MinimizePic_Click;
            // 
            // 終了ToolStripMenuItem
            // 
            終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
            終了ToolStripMenuItem.Size = new Size(148, 22);
            終了ToolStripMenuItem.Text = "終了";
            終了ToolStripMenuItem.Click += ClosePic_Click;
            // 
            // InfoSlideTimer
            // 
            InfoSlideTimer.Interval = 10;
            InfoSlideTimer.Tick += InfoSlideTimer_Tick;
            // 
            // FaceTimer
            // 
            FaceTimer.Tick += FaceTimer_Tick;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 50000;
            toolTip1.InitialDelay = 500;
            toolTip1.IsBalloon = true;
            toolTip1.ReshowDelay = 100;
            // 
            // NetNameTimer
            // 
            NetNameTimer.Interval = 50;
            NetNameTimer.Tick += NetNameTimer_Tick;
            // 
            // Clock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 976);
            ControlBox = false;
            Controls.Add(panel1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Clock";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Moni";
            Activated += Clock_Activated;
            Deactivate += Clock_Deactivate;
            FormClosing += Clock_FormClosing;
            Move += Clock_Move;
            HelpPanel.ResumeLayout(false);
            HelpPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MemUsingPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)RedPic2).EndInit();
            ((System.ComponentModel.ISupportInitialize)RedPic5).EndInit();
            ((System.ComponentModel.ISupportInitialize)BluePic5).EndInit();
            ((System.ComponentModel.ISupportInitialize)RedPic4).EndInit();
            ((System.ComponentModel.ISupportInitialize)BluePic4).EndInit();
            ((System.ComponentModel.ISupportInitialize)RedPic3).EndInit();
            ((System.ComponentModel.ISupportInitialize)BluePic3).EndInit();
            ((System.ComponentModel.ISupportInitialize)BluePic2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LoadPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)MinimizePic).EndInit();
            ((System.ComponentModel.ISupportInitialize)ClosePic).EndInit();
            ((System.ComponentModel.ISupportInitialize)FacePic).EndInit();
            FaceMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer DateTimer;
        private System.Windows.Forms.Panel HelpPanel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label Received;
        private System.Windows.Forms.Timer ResourceTimer;
        private System.Windows.Forms.Label CPULabel;
        private System.Windows.Forms.PictureBox RedPic2;
        private System.Windows.Forms.PictureBox BluePic2;
        private System.Windows.Forms.Label MemoryLabel;
        private System.Windows.Forms.PictureBox RedPic3;
        private System.Windows.Forms.PictureBox BluePic3;
        private System.Windows.Forms.Label MaxMemLabel;
        private System.Windows.Forms.Timer InfoSlideTimer;
        private System.Windows.Forms.PictureBox RedPic4;
        private System.Windows.Forms.PictureBox BluePic4;
        private System.Windows.Forms.Label GPULabel;
        private System.Windows.Forms.Label BusyLabel;
        private System.Windows.Forms.PictureBox RedPic5;
        private System.Windows.Forms.PictureBox BluePic5;
        private System.Windows.Forms.PictureBox FacePic;
        private System.Windows.Forms.Timer FaceTimer;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label NetName;
        private System.Windows.Forms.Label Sent;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer NetNameTimer;
        public System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.DomainUpDown DriveUD;
        public System.Windows.Forms.Label TimeLabel;
        public System.Windows.Forms.Label DateLabel;
        public System.Windows.Forms.Label SecondLabel;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip FaceMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 顔色変更ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem メニュー切り替えToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem その他ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最新情報を見るToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最小化ToolStripMenuItem;
        public System.Windows.Forms.PictureBox LoadPic;
        private System.Windows.Forms.PictureBox MemUsingPic;
        private System.Windows.Forms.Label NewsLabel;
        private System.Windows.Forms.PictureBox MinimizePic;
        private System.Windows.Forms.PictureBox ClosePic;
        private System.Windows.Forms.Label GameLabel;
    }
}

