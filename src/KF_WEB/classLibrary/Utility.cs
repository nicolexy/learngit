using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
    public class Utility
    {
        public static string GetDataCellValue(object dataCell)
        {
            if (dataCell == null)
                return string.Empty;

            return dataCell.ToString();

        }
    }

    public static class IEnumerableEx
    {
        /// <summary>
        /// 将数组转换成字符串，用指定的间隔字符间隔
        /// </summary>
        /// <param name="collections"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToString(this IEnumerable collections, string separator)
        {
            var newValueBuilder = new StringBuilder();
            foreach (var item in collections)
            {
                newValueBuilder.Append(item.ToString());
                newValueBuilder.Append(separator);
            }
            return newValueBuilder.Remove(newValueBuilder.Length - separator.Length, separator.Length)
                .ToString();
        }
    }
}