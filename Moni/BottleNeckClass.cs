using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moni
{
    internal class BottleNeckClass
    {
        public string name;
        private int _errorTimes;
        public int errorTimes
        {
            get
            {
                return _errorTimes;
            }
            set
            {
                if (value == 0)
                {
                    _errorTimes = 0;
                    _errorFlg = false;
                }
                else
                {
                    if (++_errorTimes >= 1)
                    {
                        _errorFlg = true;
                    }
                    else
                    {
                        _errorFlg = false;
                    }
                }
            }
        }
        private bool _errorFlg;
        public bool errorFlg
        {
            get
            {
                return _errorFlg;
            }
            set
            {

            }
        }

        public BottleNeckClass(string arg)
        {
            this.name = arg;
            this.errorTimes = 0;
            this.errorFlg = false;
        }
    }
}
