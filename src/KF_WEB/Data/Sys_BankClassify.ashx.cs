using CFT.CSOMS.BLL.SysManageModule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace TENCENT.OSS.CFT.KF.KF_Web.Data
{
    /// <summary>
    /// Summary description for Sys_BankClassify
    /// </summary>
    public class Sys_BankClassify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Request.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;
            string action = request["Action"].ToLower();

            if (action == "getbank")
            {
                int totalNum = 0;
                DataSet ds = new BankClassifyService().QueryBankBusiInfo(1, "", "", 0, 0, 0, 1000, out totalNum);
                string json = string.Empty;
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string bankname = request["bankname"];
                        if (!string.IsNullOrEmpty(bankname))
                        {
                            DataRow[] rows = dt.Select("bank_name like '%" + bankname + "%'");
                            DataTable tempDt = new DataTable();
                            tempDt = dt.Clone();
                            foreach (DataRow item in rows)
                            {
                                tempDt.Rows.Add(item.ItemArray);
                            }
                            json = JsonConvert.SerializeObject(tempDt);
                        }
                        else
                            json = JsonConvert.SerializeObject(dt);
                        
                    }
                }
                context.Response.Write(json);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}