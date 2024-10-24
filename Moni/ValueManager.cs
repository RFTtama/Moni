using System;

namespace Moni
{
    public class ValueManager
    {
        private static readonly string[] HEAD = new string[] { "", "k", "M", "G", "T", "P" };
        private long _num;

        /// <summary>
        /// 数値(バイト単位)
        /// </summary>
        public long num
        {
            get
            {
                return _num;
            }
        }

        private short _head;

        /// <summary>
        /// 数値の接頭部
        /// </summary>
        public short head
        {
            get
            {
                return _head;
            }
        }

        private double _data;

        /// <summary>
        /// 数値のデータ部
        /// </summary>
        public double data
        {
            get
            {
                return _data;
            }
        }

        private void DevideData(long val)
        {
            _num = val;
            _data = val;
            _head = 0;
            while (_data > 1000.0)
            {
                _data /= 1000.0;
                _head++;
            }
        }

        private void DevideData(double data, short head)
        {
            _head = head;
            _data = data;
            long ans = (long)Math.Pow(1000, head);
            ans = (long)((double)ans * data);
            _num = ans;
        }

        public ValueManager(long val)
        {
            DevideData(val);
        }

        public ValueManager(double data, short head)
        {
            DevideData(data, head);
        }

        /// <summary>
        /// 接頭部を文字に変換する
        /// </summary>
        /// <returns></returns>
        public string HeadToString()
        {
            return HEAD[head];
        }
    }
}
