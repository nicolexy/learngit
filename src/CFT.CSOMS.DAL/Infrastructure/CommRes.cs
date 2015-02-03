using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.DataAccess;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using TENCENT.OSS.CFT.KF.Common;
using System.Data;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Collections;

namespace CFT.CSOMS.DAL.Infrastructure
{
    public class CommRes
    {
        /// <summary>
        /// 通过组件ICE形式调middle接口，返回DataSet
        /// </summary>
        /// <param name="serviceName">接口名</param>
        /// <param name="inmsg">请求串</param>
        /// <param name="secret">是否加密 true：加密 false：不加密</param>
        /// <param name="errMsg">错误输出</param>
        /// <returns></returns>
        public static DataSet GetOneTableFromICE(string serviceName, string inmsg, bool secret, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;

            errMsg = "";

            string sReply;
            short iResult;
            string sMsg;


            if (ICEAccessFactory.ICEMiddleInvoke(serviceName, inmsg, secret, out sReply, out iResult, out sMsg))
            {
                if (iResult==0)
                {
                    //对sreply进行解析
                    if (sReply == null || sReply.Trim() == "")
                    {
                        errMsg = "调用查询失败,无返回结果  serviceName=" + serviceName + " | inmsg=" + inmsg;
                        return null;
                    }
                    else
                    {
                        string[] strlist1 = sReply.Split('&');

                        if (strlist1.Length == 0)
                        {
                            errMsg = "调用查询失败,返回结果有误" + sReply;
                            return null;
                        }

                        ht = new Hashtable(strlist1.Length);
                        dsresult = new DataSet();
                        DataTable dt = new DataTable();
                        dsresult.Tables.Add(dt);
                        DataRow drfield = dt.NewRow();

                        drfield.BeginEdit();
                        foreach (string strtmp in strlist1)
                        {
                            string[] strlist2 = strtmp.Split('=');
                            if (strlist2.Length != 2)
                            {
                                continue;
                            }
                            dt.Columns.Add(strlist2[0]);

                            drfield[strlist2[0]] = IceDecode(strlist2[1].Trim());

                            ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                        }
                        drfield.EndEdit();
                        dt.Rows.Add(drfield);

                        if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                        {
                            errMsg = "调用查询失败,返回结果有误" + sReply;
                            return null;
                        }
                    }
                    return dsresult;
                }
                else
                {
                    errMsg = "调用查询失败iResult=" + iResult + "|err=" + sMsg;
                    return null;
                }
            }
            else
            {
                errMsg = "调用查询失败了:" + sMsg;
                return null;
            }
        }

        /// <summary>
        /// 测试middle接口
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="inmsg"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public DataSet GetOneTableFromICETest(string serviceName, string inmsg, bool secret)
        {
            string msg = "";
            DataSet ds= GetOneTableFromICE(serviceName, inmsg, secret, out msg);
            return ds;
        }

        public static string IceDecode(string str)
        {
            if (str == null)
                return "";

            return str.Replace("%26", "&").Replace("%3d", "=").Replace("%3D", "=").Replace("%25", "%");
        }

    }

}
