using CFT.CSOMS.BLL.TradeModule;
using log4net;
using Microsoft.Office.Interop.Excel;
using SunLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;

namespace TENCENT.OSS.C2C.KF.KF_Web.BaseAccount
{
    public partial class logOnUserBatch : System.Web.UI.Page
    {
        string uid;
        string cancelPath = string.Empty;

        Query_Service qs = new Query_Service();
        Check_Service cs = new Check_Service();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                uid = Session["uid"].ToString();
                this.Label_uid.Text = uid;
                string szkey = Session["szkey"].ToString();
                if (!ClassLib.ValidateRight("BatchCancellation", this)) Response.Redirect("../login.aspx?wh=1");

                TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Finance_Header fh2 = setConfig.setFH(this);
                qs.Finance_HeaderValue = fh2;

                TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = setConfig.setFH_CheckService(this);
                cs.Finance_HeaderValue = fh;
            }
            catch  //如果没有登陆或者没有权限就跳出
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }

        protected void btn_batch_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File1.HasFile)
                {
                    WebUtils.ShowMessage(this.Page, "请选择上传文件！");
                    return;
                }
                if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
                {
                    cancelPath = string.Format("{0}PLFile\\batchcancel{1}.xls", 
                        Server.MapPath("~/"),DateTime.Now.ToString("yyyyMMddHHmmss"));
                    File1.PostedFile.SaveAs(cancelPath);
                    DataSet res_ds = PublicRes.readXls(cancelPath, "F1,F2");
                    System.Data.DataTable res_dt = res_ds.Tables[0];
                    if (res_dt != null && res_dt.Rows.Count > 3001)
                    {
                        WebUtils.ShowMessage(this.Page, "批量注销超过3000个帐号。");
                    }
                    Thread thread = new Thread(BatchCancel);
                    thread.Start();
                    WebUtils.ShowMessage(this.Page, "后台处理中，稍后请查收邮件。");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "文件格式不正确，请选择xls格式文件上传。");
                    return;
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }

        private void BatchCancel()
        {
            if (!File.Exists(cancelPath))
            {
                cancelPath = string.Format("{0}PLFile\\batchcancel{1}.xls",
                        Server.MapPath("~/"), DateTime.Now.ToString("yyyyMMddHHmmss"));
                
                File1.PostedFile.SaveAs(cancelPath);
            }
            int failNum = 0;
            DataSet res_ds = PublicRes.readXls(cancelPath, "F1,F2");
            System.Data.DataTable res_dt = res_ds.Tables[0];
            if (res_dt != null && res_dt.Rows.Count > 0)
            {
                res_dt.Columns.Add("F3",typeof(string));
                string account = string.Empty, reason=string.Empty;
                for (int i = 0; i < res_dt.Rows.Count; i++)
                {
                    account = res_dt.Rows[i][0].ToString();
                    reason = res_dt.Rows[i][1].ToString();

                    string msg = string.Empty;
                    if (CancelAccount(account, reason, out msg))
                    {
                        res_dt.Rows[i][2] = "OK";
                    }
                    else
                    {
                        failNum++;
                        res_dt.Rows[i][2] = msg.Length > 100 ? msg.Substring(0, 100) : msg;
                    }
                }
                #region

                string path = string.Empty;
                try
                {
                    #region 生成excel文件
                    var temp = DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                    path = Server.MapPath("~/") + "PLFile" + "\\" + temp + "批量注销" + uid + ".xls"; //附件

                    Application xlApp = new ApplicationClass();
                    Workbooks workbooks = xlApp.Workbooks;
                    Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    Worksheet worksheet = (Worksheet)workbook.Worksheets[1];//取得sheet1

                    Range range;
                    List<string> titList = new List<string>() { "帐号", "注销原因", "结果" };
                    for (int i = 0; i < titList.Count; i++)
                    {
                        range = (Range)worksheet.Cells[1, i + 1];
                        range.ColumnWidth = 40;
                        range.NumberFormatLocal = "@";
                        range.Font.Size = 15;
                        range.Value2 = titList[i];
                    }

                    workbook.Saved = true;
                    //workbook.SaveCopyAs(path);  //2007版本
                    workbook.SaveAs(path, 56, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                        XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                    range = null;
                    workbooks = null;
                    workbook = null;

                    if (xlApp != null)
                    {
                        xlApp.Workbooks.Close();
                        xlApp.Quit();
                        xlApp = null;
                    }

                    #region excel写入数据
                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + "Excel 12.0 Xml;HDR=YES;";
                    OleDbConnection conn = new OleDbConnection(strConn);
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;

                    foreach (DataRow dr in res_dt.Rows)
                    {
                        cmd.CommandText = string.Format(@"INSERT INTO [sheet1$] (帐号,注销原因,结果) VALUES ('{0}','{1}','{2}')",
                        dr["F1"].ToString(), dr["F2"].ToString(), dr["F3"].ToString());
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    #endregion

                    #endregion
                }
                catch (Exception ex)
                {
                    LogHelper.LogInfo("批量注销生成Excel失败！" + ex.Message + ", stacktrace" + ex.StackTrace);
                }
                finally
                {
                    GC.Collect();
                }

                try
                {
                    CommMailSend.SendInternalMail(uid, "", "您好，您本次批量注销总计：" + res_dt.Rows.Count + "笔，其中失败" + failNum + "笔，请查看附件。谢谢！", "", false, new string[] { path });
                }
                catch (Exception ex) { }
                #endregion
            }
        }

        private bool CancelAccount(string account, string reason,out string msg)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(reason))
            {
                if (reason.Length > 255)
                {
                    msg = "注销原因长度不得超过255。";
                }
                else
                {
                    try
                    {
                        Finance_Manage fm = new Finance_Manage();
                        if (!fm.checkUserReg(account, out msg))
                        {
                            msg = "帐号非法或者未注册！";
                            return result;
                        }
                        if (!(new TradeService()).QueryWXUnfinishedTrade(account))
                        {
                            msg = "此账号有未完成微信支付转账，禁止注销！";
                            return result;
                        }
                        if (!(new TradeService()).QueryWXUnfinishedHB(account))
                        {
                            msg = "此账号有未完成微信红包，禁止注销！";
                            return result;
                        }

                        var HasUnfinishedRepayment = new TradeService().QueryWXFCancelAsRepayMent(account, out msg);
                        if (HasUnfinishedRepayment)
                        {                          
                            return result;
                        }

                        if (qs.LogOnUsercheckOrder(account, "1"))
                        {
                            msg = "有未完成的交易单！";
                            return result;
                        }
                        if (qs.LogOnUserCheckYDT(account, "1"))
                        {
                            msg = "开通了一点通！";
                            return result;
                        }

                        #region 金额判断
                        //下面的流程
                        // 1:判断金额大于阀值,发起一个审批
                        //2:金额小于时,插入logonhistory表,调用接口注销,如果有邮箱,向邮箱发送邮件,反馈信息给一线人员.

                        var ds = qs.GetUserAccount(account, 1, 1, 1);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //分账冻结金额
                            string s_cron = PublicRes.objectToString(ds.Tables[0], "Fcon");

                            var freezeMoney = 0L;
                            if (s_fz_amt != "")
                            {
                                freezeMoney += long.Parse(s_fz_amt);
                            }
                            if (s_cron != "")
                            {
                                freezeMoney += long.Parse(s_cron);
                            }

                            if (freezeMoney > 0)
                            {
                                msg = "账户有冻结资金不支持注销！";
                                return result;
                            }

                        }

                        long balance = 0;//就是金额
                        DataSet ds1 = qs.GetChildrenInfo(account, "1");//主帐户余额
                        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            balance += long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
                        }
                        ds1 = qs.GetChildrenInfo(account, "80");//游戏子帐户
                        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            balance += long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
                        }
                        ds1 = qs.GetChildrenInfo(account, "82");//直通车子帐户
                        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            balance += long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
                        }
                        #endregion

                        #region 注销操作

                        string mainID = DateTime.Now.ToString("yyyyMMdd") + account;
                        string checkType = "logonUser";
                        if (balance < 10 * 10 * 200)//系统自动注销
                        {
                            if (!qs.LogOnUserDeleteUser(account, reason, Label_uid.Text, "", out msg))
                            {
                                return result;
                            }
                            else
                            {
                                //"系统自动销户成功！"
                                result = true;
                            }
                        }
                        else
                        {
                            string memo = "[注销QQ号码:" + account + "]注销原因:" + reason;
                            Param[] myParams = new Param[4];

                            myParams[0] = new Param();
                            myParams[0].ParamName = "fqqid";
                            myParams[0].ParamValue = account;

                            myParams[1] = new Param();
                            myParams[1].ParamName = "Memo";
                            myParams[1].ParamValue = memo;

                            myParams[2] = new Param();
                            myParams[2].ParamName = "returnUrl";
                            myParams[2].ParamValue = "/BaseAccount/InfoCenter.aspx?id=" + account;

                            myParams[3] = new Param();
                            myParams[3].ParamName = "fuser";
                            myParams[3].ParamValue = Session["uid"].ToString();

                            cs.StartCheck(mainID, checkType, memo, MoneyTransfer.FenToYuan(balance.ToString()), myParams);

                            msg = "销户申请提请成功！";
                            return result;
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        LogHelper.LogInfo("批量注销异常：" + ex.Message + ", stacktrace" + ex.StackTrace);
                    }
                }
            }
            else
            {
                msg = "帐号不能为空！";
            }
            return result;
        }
    }
}