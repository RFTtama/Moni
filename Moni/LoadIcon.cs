using System;

namespace Moni
{
    public static class LoadIcon
    {
        private static System.Timers.Timer timer = new System.Timers.Timer();
        public static Clock _form;
        private static uint statisticsCnt = 0;
        private static readonly System.Drawing.Bitmap[] sdb =
                {
                    Properties.Resources.load0,
                    Properties.Resources.load1,
                    Properties.Resources.load2
                };

        private static bool ready = false;

        internal static void TimerStart()
        {
            if (!ready)
            {
                timer.Interval = 200;
                timer.Elapsed += Timer_Tick;
                ready = true;
            }
            timer.Enabled = true;
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            SetStatisticsLoadIcon();
        }

        /// <summary>
        /// ログ解析アイコン関連
        /// </summary>
        private static void SetStatisticsLoadIcon()
        {
            try
            {
                _form.LoadPic.Image = sdb[(int)Math.Sin(Math.PI * statisticsCnt / 2.0) + 1];
            }
            catch (Exception)
            {

            }
            statisticsCnt++;
        }

        internal static void ResetStatisticsLoadIcon()
        {
            timer.Enabled = false;
            statisticsCnt = 0;
            try
            {
                _form.LoadPic.Image = null;
            }
            catch (Exception)
            {

            }
        }
    }

    /// <summary>
    /// ロードアイコンを表示したいときにインスタンスを生成する
    /// 破棄されるまでの間アイコンが表示される
    /// </summary>
    public class LoadingProcess
    {
        private static ushort launchNum = 0;
        public LoadingProcess()
        {
            launchNum++;
            LoadIcon.TimerStart();
        }

        ~LoadingProcess()
        {
            if (--launchNum <= 0)
            {
                LoadIcon.ResetStatisticsLoadIcon();
                launchNum = 0;
            }
        }
    }
}
