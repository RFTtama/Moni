using System;
using System.Collections.Generic;

namespace Moni
{
    static class ArrayConverters
    {
        /// <summary>
        /// Convert DateTime to Doubles(CAtD)
        /// </summary>
        /// <param name="dateTimes">DateTime</param>
        /// <returns>double[]</returns>
        public static double[] ConvertDateTimeToDouble(DateTime[] dateTimes)
        {
            List<double> ld = new List<double>();

            foreach (DateTime dt in dateTimes)
            {
                ld.Add(dt.ToOADate());
            }
            return ld.ToArray();
        }

        /// <summary>
        /// Convert Array to Doubles(CAtD)
        /// </summary>
        /// <typeparam name="T">Different Types Without DateTime</typeparam>
        /// <param name="ts">double[]</param>
        /// <returns></returns>
        public static double[] ConvertArrayToDouble<T>(T[] ts)
        {
            List<double> ld = new List<double>();

            foreach (T t in ts)
            {
                ld.Add(double.Parse(t.ToString()));
            }

            return ld.ToArray();
        }
    }
}
