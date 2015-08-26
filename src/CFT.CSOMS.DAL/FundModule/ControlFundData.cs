using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.DAL.FundModule
{
    public class ControlFundData
    {
        public DataSet RemoveControledFinLogQuery(string qqid)
        {
            using (var da = CommLib.DbConnectionString.Instance.GetConnection("DataSource_ht"))
            {
                da.OpenConn();

                string Sql = "Select * from  c2c_fmdb.t_log_ConFinRemove where Fuin='" + qqid + "' Order by FmodifyTime DESC";
                DataSet ds = da.dsGetTotalData(Sql);
                return ds;
            }
        }

        public bool RemoveUserControlFin(string uid, string cur_type, string balance, string opera, int type)
        {
            if (string.IsNullOrEmpty(uid.Trim()))
            {
                throw new ArgumentNullException("uid为空！");
            }
            if (string.IsNullOrEmpty(opera.Trim()))
            {
                throw new ArgumentNullException("opera为空！");
            }

            string cgi = "";
            string msg = "";
            cgi = System.Configuration.ConfigurationManager.AppSettings["UserControlFinCgi"].ToString();
            cgi += "uid=" + uid;
            if (type == 3)//解绑指定金额
            {
                if (string.IsNullOrEmpty(cur_type.Trim()))
                {
                    throw new ArgumentNullException("cur_type为空！");
                }
                if (string.IsNullOrEmpty(balance.Trim()))
                {
                    throw new ArgumentNullException("balance为空！");
                }
                cgi += "&type=3";
                cgi += "&curtype=" + cur_type;
                cgi += "&balance=" + balance;//传分
                cgi += "&opera=" + opera;
            }
            else if (type == 4)//解绑所有子账户余额
            {
                cgi += "&type=4";
                cgi += "&opera=" + opera;
            }

            // 测试 cgi = "http://check.cf.com/cgi-bin/v1.0/BauClrBan.cgi?uid=400061433&type=1009&sum=2850&opera=1100000000";
            // LogHelper.LogInfo("RemoveUserControlFin send req:" + cgi);
            string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(cgi, "", out msg);
            if (msg != "")
            {
                throw new Exception(msg);
            }
            //LogHelper.LogInfo("RemoveUserControlFin return:" + res);

            if (res.IndexOf("执行成功") == -1)
                throw new Exception("解除失败：" + res);
            else
                return true;
        }

        public int RemoveControledFinLogInsert(string qqid, string FbalanceStr, string FtypeText, string cur_type, DateTime ApplyTime, string ApplyUser)
        {
            using (var da = CommLib.DbConnectionString.Instance.GetConnection("DataSource_ht"))
            {
                da.OpenConn();

                string Sql = "insert into  c2c_fmdb.t_log_ConFinRemove (Fuin ,FbalanceStr,FtypeText,FcurType ,FmodifyTime ,FupdateUser) " +
                        "values('" + qqid + "','" + FbalanceStr + "','" + FtypeText + "','" + cur_type + "','" + ApplyTime.ToString() + "','" + ApplyUser + "')";
                return da.ExecSqlNum(Sql);
            }
        }

        public DataTable QueryUserControledRecordCgi(string uid, string opera)
        {

            string cgi = "";
            string msg = "";
            cgi = System.Configuration.ConfigurationManager.AppSettings["UserControlFinCgi"].ToString();
            cgi += "uid=" + uid;
            cgi += "&type=1";
            cgi += "&opera=" + opera;

            // 测试 cgi = "http://check.cf.com/cgi-bin/v1.0/BauClrBan_new.cgi?type=1&uid=441845935&opera=yukini";
            //LogHelper.LogInfo("QueryUserControledRecordCgi send req:" + cgi);
            string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(cgi, "", out msg);
            if (msg != "")
            {
                throw new Exception(msg);
            }
            //LogHelper.LogInfo("QueryUserControledRecordCgi return:" + res);

            DataTable dt = new DataTable();
            dt.Columns.Add("cur_type", typeof(String));
            dt.Columns.Add("balance", typeof(String));

            if (res.IndexOf("执行成功") == -1)
            {
                throw new Exception("cgi查询失败，返回：" + res);
            }
            res = res.Replace("执行成功 ", "");
            string[] ans = res.Split(' ');
            DataRow drfield = dt.NewRow();

            foreach (string strtmp in ans)
            {
                string[] strlist2 = strtmp.Split(',');
                if (strlist2.Length != 2)
                {
                    continue;
                }
                string[] para = strlist2[0].Split(':');
                if (para.Length != 2)
                {
                    continue;
                }

                drfield.BeginEdit();

                drfield["cur_type"] = PublicRes.getCgiString(para[1].Trim());

                para = strlist2[1].Split(':');
                if (para.Length != 2)
                {
                    continue;
                }
                drfield["balance"] = PublicRes.getCgiString(para[1].Trim());

                drfield.EndEdit();
                dt.Rows.Add(drfield);
            }

            return dt;
        }

    }
}
