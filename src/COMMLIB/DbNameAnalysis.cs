using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commLib
{
    public class DbNameAnalysis
    {

        #region 单例
        private static DbNameAnalysis _instance = null;

        private static readonly object _locker = new object();

        public static DbNameAnalysis Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new DbNameAnalysis();
                        }
                    }
                }
                return _instance;
            }
        }

        private DbNameAnalysis()
        { }
        #endregion


        /// <summary>
        /// 返回拆分之后的库名
        /// </summary>
        /// <param name="strDbName">s数据库前缀</param>
        ///         /// <param name="uid"></param>
        /// <returns></returns>
        public string GetDbnameByUid(string strDbName, string uid)
        {
            if (uid.Length < 2)
            {
                throw new ArgumentException("参数异常：Uid必须大于两位");
            }

            var index = int.Parse(uid.Substring(uid.Length - 2, 2));
            var step = 3;
            for (int i = 0; i <= 99; i++)
            {
                if (index >= i && index <= i + step)
                {
                    var min = i.ToString();
                    var max = (i + step).ToString();
                    if (i < 10)
                    {
                        min = "0" + i;
                    }
                    if (int.Parse(max) < 10)
                    {
                        max = "0" + max;
                    }
                    return strDbName += "(" + min + "-" + max + ")";
                }
                i += step;
            }

            return "";
        }

    }
}
