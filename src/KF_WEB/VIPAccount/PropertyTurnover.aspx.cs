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
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Web.Mail;

namespace TENCENT.OSS.CFT.KF.KF_Web.VIPAccount
{
    /// <summary>
    /// Summary description for PropertyTurnover.
    /// </summary>
    public partial class PropertyTurnover : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                this.tbx_beginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
                this.tbx_endDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
            }
            this.btnBeginDate.Attributes.Add("onclick", "openModeBegin()");
            this.btnEndDate.Attributes.Add("onclick", "openModeEnd()");
            totalValue.Text = "0";
        }

        private void btn_query_Click(object sender, System.EventArgs e)
        {
            string strBeginDate = "", strEndDate = "";

            try
            {
                if (this.tbx_beginDate.Text.Trim() != "" && this.tbx_endDate.Text.Trim() != "")
                {
                    strBeginDate = DateTime.Parse(this.tbx_beginDate.Text).ToString("yyyy-MM-dd");
                    strEndDate = DateTime.Parse(this.tbx_endDate.Text).AddDays(1).ToString("yyyy-MM-dd");
                }
            }
            catch
            {
                ShowMsg("日期格式不正确！");
                return;
            }

            StartQuery(strBeginDate, strEndDate);
        }

        private void StartQuery(string strBeginDate, string strEndDate)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.QueryTurnover(tbx_acc.Text.Trim(), tbx_order.Text.Trim(), strBeginDate, strEndDate);
                int total = 0;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.ShowMsg("查询记录为空。");
                }
                else
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string svc_id = row["FSvc_id"].ToString();
                        if (FSvc_id.ContainsKey(svc_id))
                        {
                            row["FSvc_id"] = FSvc_id[svc_id];
                        }
                        total += Convert.ToInt32(row["FIncrease"]);
                    }
                }
                totalValue.Text = total.ToString();
                this.DataGrid_QueryResult.DataSource = ds;
                this.DataGrid_QueryResult.DataBind();
            }
            catch (Exception e)
            {
                ShowMsg("查询出错" + e.Message);
            }
        }


        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

        private Hashtable FSvc_id
        {
            get
            {
                if (_FSvc_id == null)
                {
                    //					|cert:首次开通证书|oneclick:首次开通一点通|firstvip:开通SVIP赠送|authname:首次开通实名认证|activate:激活|
                    //						closevip:关闭SVIP|present:赠送|normalmember:赠送VIP|creditcardAdd:信用卡还款任务|balancePay:余额支付
                    //						|quickPay:快捷支付|lotteryAdd:彩票任务|mobileAdd:手机充值任务|microPay:微支付|partnerConsume:合作方消费任务|
                    //						balanceRefund:余额退款|microRefund:微支付退款|quickRefund:快捷支付退款|batch_increase:SVIP激励财付值|batch_decrease:VIP非活跃扣减|
                    //						aticketAdd:航空机票任务|gift:赠送|xy:绑定心悦俱乐部会员|travelAdd:QQ网购酒店预定任务|qtktAdd:QQ网购订票任务|qgpAdd:QQ团购任务|
                    //						qcateAdd:QQ美食券搜索任务|icbc:绑定工行爱购卡|citic:绑定中信信用卡|"
                    _FSvc_id = new Hashtable();
                    _FSvc_id.Add("cert", "首次开通证书"); _FSvc_id.Add("oneclick", "首次开通一点通"); _FSvc_id.Add("firstvip", "开通SVIP赠送");
                    _FSvc_id.Add("authname", "首次开通实名认证"); _FSvc_id.Add("activate", "激活"); _FSvc_id.Add("closevip", "关闭SVIP");
                    _FSvc_id.Add("present", "赠送"); _FSvc_id.Add("normalmember", "赠送VIP");
                    _FSvc_id.Add("creditcardAdd", "信用卡还款任务"); _FSvc_id.Add("balancePay", "余额支付");
                    _FSvc_id.Add("quickPay", "快捷支付"); _FSvc_id.Add("lotteryAdd", "彩票任务");
                    _FSvc_id.Add("mobileAdd", "手机充值任务"); _FSvc_id.Add("microPay", "微支付"); _FSvc_id.Add("partnerConsume", "合作方消费任务");
                    _FSvc_id.Add("balanceRefund", "余额退款"); _FSvc_id.Add("microRefund", "微支付退款"); _FSvc_id.Add("quickRefund", "快捷支付退款");
                    _FSvc_id.Add("batch_increase", "SVIP激励财付值"); _FSvc_id.Add("batch_decrease", "VIP非活跃扣减");
                    _FSvc_id.Add("aticketAdd", "航空机票任务"); _FSvc_id.Add("gift", "赠送"); _FSvc_id.Add("xy", "绑定心悦俱乐部会员");
                    _FSvc_id.Add("travelAdd", "QQ网购酒店预定任务"); _FSvc_id.Add("qtktAdd", "QQ网购订票任务"); _FSvc_id.Add("qgpAdd", "QQ团购任务");
                    _FSvc_id.Add("qcateAdd", "QQ美食券搜索任务"); _FSvc_id.Add("icbc", "绑定工行爱购卡"); _FSvc_id.Add("citic", "绑定中信信用卡");
                }
                return _FSvc_id;
            }
        }

        private Hashtable _FSvc_id = null;

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_query.Click += new EventHandler(btn_query_Click);
        }
        #endregion
    }
}
