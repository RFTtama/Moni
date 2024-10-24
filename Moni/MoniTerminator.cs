using System.IO;

namespace Moni
{
    class MoniTerminator
    {
        private const string fileName = @".\tcData\Safe.flg";
        public MoniTerminator()
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {

                }
                ErrorLog.ErrorOutput("Moni終了エラー", "Moniの異常終了を確認しました", true);
            }
            catch (FileNotFoundException)
            {

            }
            catch (DirectoryNotFoundException)
            {

            }
            try
            {
                CreateKey();
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(@".\tcData");
                CreateKey();
            }
        }

        private void CreateKey()
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("Don't delete this file");
            }
        }

        public void IsSafe()
        {
            File.Delete(fileName);
        }
    }
}
