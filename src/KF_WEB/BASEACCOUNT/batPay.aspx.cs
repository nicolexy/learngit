using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// batPay 的摘要说明。
    /// </summary>
    public partial class batPay : System.Web.UI.Page
    {

        private string WeekIndex = "0";


        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"TradeLogQuery")) Response.Redirect("../login.aspx?wh=1");
                if (!classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                WeekIndex = Request.QueryString["WeekIndex"];
                try
                {
                    DateTime dt = DateTime.Parse(WeekIndex);
                }
                catch
                {
                    WeekIndex = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }

                TextBoxBeginDate.Value = WeekIndex;
            }
            else
            {
                WeekIndex = TextBoxBeginDate.Value.Trim();
            }

            try
            {
                InitGrid();
            }
            catch
            {
                WebUtils.ShowMessage(this.Page, "显示数据时出错，请重试。");
            }
        }

        private void InitGrid()
        {
            DataSet ds = new DataSet();
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                ds = qs.BatPay_InitGrid(WeekIndex);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据时出错:" + ex.Message);
                return;
            }
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ShowBatPay(ds.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "展示数据时出错:" + ex.Message);
                return;
            }
        }


        private void ShowBatPay(DataTable dt)
        {
            DateTime BeginDate = DateTime.Parse(WeekIndex);
            string strDateUse = BeginDate.ToString("yyyyMMdd");

            dt.Columns.Add("Detail", typeof(string));

            if (dt.Rows.Count == 0)
            {
                if (CanVisible(strDateUse))
                {
                    DataRow dr = dt.NewRow();
                    dr["FBatchID"] = strDateUse + "1001";

                    string strDate = BeginDate.ToString("yyyy年MM月dd日");
                    dr["FDate"] = strDate;
                    dr["FStatusName"] = "尚未汇总付款数据";

                    if (!CanStartTask(strDateUse + "1001"))
                    {
                        dr["FStatusName"] = "尚未有数据快照，或者有正在处理的任务。";
                    }
                    dr["FUrl"] = "BatchID=" + strDateUse + "1001";

                    dr["FBankID"] = "所有银行";

                    dt.Rows.Add(dr);
                }
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int iStatus = Int32.Parse(dr["FStatus"].ToString());
                    dr.BeginEdit();
                    dr["FStatusName"] = GetStatusName(iStatus, dr["FBatchID"].ToString());
                    if (iStatus == 5 && (dr["FPayCount"] == null || dr["FPayCount"].ToString() == "0"))
                    {
                        dr["FStatusName"] = "此银行无付款记录";
                    }
                    else
                    {
                        dr["Detail"] = "详细";
                    }

                    dr["FUrl"] = "BatchID=" + dr["FBatchID"].ToString();

                    dr["FBankID"] = GetBankName(dr["FBankType"].ToString());

                    string tmp = dr["FDate"].ToString();

                    tmp = tmp.Substring(0, 4) + "年" + tmp.Substring(4, 2) + "月" + tmp.Substring(6, 2) + "日";
                    dr["FDate"] = tmp;
                    dr.EndEdit();
                }
            }

            dt.DefaultView.Sort = "FBankType";
            DataGrid1.DataSource = dt.DefaultView;
            DataGrid1.DataBind();
        }

        public string GetBankName(string strBankID)
        {
            DataTable dt;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            dt = qs.BatPay_GetBank().Tables[0];

            if (dt == null)
            {
                WebUtils.ShowMessage(this.Page, "读取数据时出错！请重试。");
                return "";
            }

            foreach (DataRow dr in dt.Rows)
            {
                string Fbank_type = dr["Fbank_type"].ToString();
                string FFlag_name = dr["FFlag2_2"].ToString();
                if (strBankID != null && strBankID == Fbank_type)
                {
                    return FFlag_name;
                }
            }
            return "无此银行";     
        }

        public static void GetAllPayBankList(DropDownList ddl)
        {
            DataTable dt;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            dt = qs.BatPay_GetBank().Tables[0];

            if (dt != null)
            {

                ddl.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    string Fbank_type = dr["Fbank_type"].ToString();
                    string FFlag_name = dr["FFlag2_2"].ToString();

                    ddl.Items.Add(new ListItem(FFlag_name, Fbank_type));
                }

            }
        }

        private static bool CanVisible(string strDate)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.BatPay_CanVisible(strDate);
        }

        public static string GetStatusName(int iStatus, string strBatchID)
        {
            string[] strArray = new string[30];
            strArray[0] = "可以汇总付款数据";
            strArray[1] = "付款数据已汇总";
            strArray[2] = "付款数据已审核";
            strArray[3] = "付款任务单已生成";
            strArray[4] = "付款已完成";
            strArray[5] = "付款结果已生效";

            strArray[6] = "付款单已生成";
            strArray[9] = "<FONT color=red>付款数据生成失败</FONT>";  //FBatchNoExp
            strArray[10] = "付款单生效执行结果"; //FBatchNoImp  FStatusDes
            strArray[11] = "<FONT color=red>付款数据生成后回写业务系统失败</FONT>";

            strArray[20] = "付款数据已提交审批";
            strArray[21] = "付款任务单已完成";
            strArray[22] = "付款结果一次上传成功";
            strArray[23] = "付款结果二次上传成功";
            strArray[24] = "付款调整完成";
            strArray[25] = "付款挂帐已完成";

            strArray[7] = "转入财务审批流程";

            string tmp = "";
            if (strBatchID != "0" && iStatus > 8 && iStatus < 12) tmp = GetErrorMsg(strBatchID, iStatus);

            if (tmp != "")
                return strArray[iStatus] + "：" + tmp;
            else
                return strArray[iStatus];

        }

        private static string GetErrorMsg(string strBatchID, int iStatus)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.BatPay_GetErrorMsg(strBatchID, iStatus);
        }

        public static bool CanStartTask(string strBatchID)
        {
            if (CheckSnapFinish(strBatchID))
            {
                if (CheckFinish11(strBatchID))
                {
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    return qs.BatPay_SixCheck(strBatchID);
                }
            }
            return false;
        }

        private static bool CheckSnapFinish(string strBatchID)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.BatPay_CheckSnapFinish(strBatchID);
        }

        private static bool CheckFinish11(string strBatchID)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.BatPay_CheckFinish11(strBatchID);
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion

    }
}
