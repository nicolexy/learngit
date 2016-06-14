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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using CFT.CSOMS.BLL.WechatPay;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Configuration;
using CFT.CSOMS.BLL.TransferMeaning;
using CFT.CSOMS.COMMLIB;
using CFT.Apollo.Logging;


namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
	/// <summary>
    /// CreditCardRefundQuery 的摘要说明。
	/// </summary>
    public partial class CreditCardRefundQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                }

            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
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
            this.dd_queryDate.SelectedIndexChanged += new EventHandler(this.dd_queryDate_SelectedIndexChanged);
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        private void ValidateDate()
        {
            DateTime begindate=new DateTime(), enddate=new DateTime();
            string s_date = TextBoxBeginDate.Value;
            string e_date = TextBoxEndDate.Value;
            try
            {
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
            catch
            {
                throw new Exception("日期输入有误！");
            }

            if (this.dd_queryDate.SelectedValue == "2" && !string.IsNullOrEmpty(s_date) && !string.IsNullOrEmpty(e_date) && begindate.AddDays(3) < enddate)
            {
                throw new Exception("日期间隔大于3天，请重新输入！");
            }

            string no = this.cftNo.Text;
            if (string.IsNullOrEmpty(no)) 
            {
                throw new Exception("请输入查询条件！");
            }

        }

        private void dd_queryDate_SelectedIndexChanged(object sender, EventArgs e) 
        {
            var d = this.dd_queryDate.SelectedIndex;
            if (d == 0)
            {
                this.showQueryDate.Visible = false;
            }
            else 
            {
                this.showQueryDate.Visible = true;
            }
        }

        public void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                clearDT();
                this.pager.RecordCount = 1000;
                BindData(1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fetch_no = e.Item.Cells[9].Text;
            GetDetail(fetch_no);
        }

        private void GetDetail(string fetch_no)
        {
            //需要注意分页情况
            clearDT();
            string s_begindate = ViewState["s_begindate"].ToString();
            string s_enddate = ViewState["s_enddate"].ToString();
            var dt = new WechatPayService().QueryCreditCardRefund(fetch_no, "3", s_begindate, s_enddate, 0, 1);
            if (dt != null)
            {
                lb_c1.Text = PublicRes.objectToString(dt, "Fwx_fetch_no", true);
                lb_c2.Text = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(dt, "Fnum", true));
                lb_c3.Text = PublicRes.objectToString(dt, "Fstate_str", true);
                lb_c4.Text = PublicRes.objectToString(dt, "Frefund_state_str", true);
                lb_c5.Text = Transfer.convertbankType(PublicRes.objectToString(dt, "Fbank_type"));
                lb_c6.Text = PublicRes.objectToString(dt, "Fcard_id", true);
                lb_c7.Text = PublicRes.objectToString(dt, "Fcard_name", true);
                lb_c8.Text = PublicRes.objectToString(dt, "Fopenid", true);
                lb_c9.Text = PublicRes.objectToString(dt, "Fpay_time", true);
                lb_c10.Text = PublicRes.objectToString(dt, "Fmodify_time", true);
                lb_c11.Text = PublicRes.objectToString(dt, "Fcft_fetch_no", true);
                lb_Fstandby2.Text = MoneyTransfer.FenToYuan(PublicRes.objectToString(dt, "Fcoupon_fee", true));
                lb_wx_trans_id.Text = PublicRes.objectToString(dt, "wx_trans_id", true);
                lb_trade_id.Text = PublicRes.objectToString(dt, "trade_no", true);
                lbl_channel_id.Text = PublicRes.objectToString(dt, "Fchannel_id_str", true);
            }
        }


        private void clearDT()
        {
            lb_c1.Text = "";
            lb_c2.Text = "";
            lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";

            lb_c8.Text = "";
            lb_c9.Text = "";
            lb_c10.Text = "";
            lb_c11.Text = "";
        }

        private void BindData(int index)
        {
            clearDT();
            string s_begindate = "";
            string s_enddate = "";

            if (this.dd_queryDate.SelectedValue == "1")
            {
                DateTime d1 = DateTime.Now;
                DateTime d2 = d1.AddMonths(-1);
                s_begindate = d2.ToString("yyyy-MM-dd");
                s_enddate = d1.ToString("yyyy-MM-dd");
            }
            else
            {
                string s_stime = TextBoxBeginDate.Value;
                if (!string.IsNullOrEmpty(s_stime))
                {
                    DateTime begindate = DateTime.Parse(s_stime);
                    s_begindate = begindate.ToString("yyyy-MM-dd");
                }
                string s_etime = TextBoxEndDate.Value;
                if (!string.IsNullOrEmpty(s_etime))
                {
                    DateTime enddate = DateTime.Parse(s_etime);
                    s_enddate = enddate.ToString("yyyy-MM-dd");
                }
            }

            ViewState["s_begindate"] = s_begindate;
            ViewState["s_enddate"] = s_enddate;
            int max = pager.PageSize;
            int start = max * (index - 1);

            string queryType = dd_queryType.SelectedValue;
            string No = cftNo.Text;
            var dt = new WechatPayService().QueryCreditCardRefund(No, queryType, s_begindate, s_enddate, start, max);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Fnum_str", typeof(string));
                dt.Columns.Add("Fcard_id_str", typeof(string));
              
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Fcard_id_str"] = classLibrary.setConfig.ConvertID(dr["Fcard_id"].ToString(), 4, 4);
                }

                classLibrary.setConfig.FenToYuan_Table(dt, "Fnum", "Fnum_str");
                DataGrid1.DataSource = dt.DefaultView;
                DataGrid1.DataBind();
            }
            else 
            {
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
            }
            
        }

        protected void dd_queryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dd_queryType.SelectedValue == "2")
            {
                this.dd_queryDate.SelectedIndex = 1;
                this.dd_queryDate.Enabled = false;
                this.showQueryDate.Visible = true;
            }
            else
            {
                this.dd_queryDate.SelectedIndex = 0;
                this.dd_queryDate.Enabled = true;
                this.showQueryDate.Visible = false;
            }
        }
      
    }
	
}