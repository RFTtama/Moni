using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moni
{
    public static class MoniMath
    {

        /// <summary>
        /// べき乗
        /// </summary>
        /// <param name="a">元の数</param>
        /// <param name="x">乗数</param>
        /// <returns>結果</returns>
        public static int Pow(int a, int x)
        {
            int ans = 1;
            for (int i = 0; i < x; i++)
            {
                ans *= a;
            }
            return ans;
        }

        /// <summary>
        /// 確率から当たりを出す
        /// </summary>
        /// <param name="per">確率</param>
        /// <returns>当たりかどうか true:当たり false:はずれ</returns>
        public static bool GetOdds(int per)
        {
            Random rand = new Random();
            if (per <= 0)
            {
                return false;
            }
            else if (per >= 100)
            {
                return true;
            }
            int randNum = rand.Next(0, 100);
            if (randNum < per)
            {
                return true;
            }
            return false;
        }
    }
}
