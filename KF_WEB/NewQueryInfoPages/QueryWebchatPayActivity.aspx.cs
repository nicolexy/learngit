using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Collections;
using CFT.CSOMS.BLL.ActivityModule;
using CFT.CSOMS.COMMLIB;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryWebchatPayActivity 的摘要说明。
	/// </summary>
    public partial class QueryWebchatPayActivity : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

            this.ddlActId.SelectedIndexChanged += new EventHandler(ddlActIdSelectedIndexChanged);

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                    TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                    
                    //setConfig.GetActivityList(ddlActId, true);
                }
                 
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
		}

        private void ddlActIdSelectedIndexChanged(object sender, EventArgs e) 
        {
            switch (this.ddlActId.SelectedIndex) 
            {
                case 0:
                    {
                        this.PanelDetail.Visible = false;
                        this.Table2.Visible = true;

                        this.txtTime1.Visible = true;
                        this.txtTime2.Visible = true;
                        this.Label3.Text = "财付通订单号：";
                        break;
                    }
                case 1:
                    {
                        this.PanelDetail.Visible = true;
                        this.Table2.Visible = false;

                        this.txtTime1.Visible = false;
                        this.txtTime2.Visible = false;
                        this.Label3.Text = "微信号：";
                        break;
                    }
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
            this.SendDG.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.SendDG_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
            this.sendPager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.SendPage);
            this.receivePager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ReceivePage);
		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}
        public void SendPage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            sendPager.CurrentPageIndex = e.NewPageIndex;
            BindSendData(e.NewPageIndex);
        }
        public void ReceivePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            this.receivePager.CurrentPageIndex = e.NewPageIndex;
            BindReceiveData(e.NewPageIndex);
        }

		private void ValidateDate()
		{
            DateTime begindate, enddate;

            try
            {
                string s_date = TextBoxBeginDate.Text;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Text;
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
			catch
			{
				throw new Exception("日期输入有误！");
			}

            string cft_no = txtCftNo.Text.Trim();

            if (cft_no == "")
            {
                throw new Exception("参数不能为空！");
            }
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
                this.pager.RecordCount = 1000;
                this.receivePager.RecordCount = 1000;
                this.sendPager.RecordCount = 1000;
                BindData(1);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
                string errStr = PublicRes.GetErrorMsg(eSys.Message.ToString());
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + errStr);
			}
		}

        private void BindData(int index)
		{
            
            string s_stime = TextBoxBeginDate.Text;
            string s_begindate = "";
            string xykTime = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
                xykTime = begindate.ToString("yyyy-MM-01 00:00:00");
            }
            string s_etime = TextBoxEndDate.Text;
            string s_enddate = "";
            if (s_etime != null && s_etime != "")
            {
                DateTime enddate = DateTime.Parse(s_etime);
                s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
            }

            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

			//Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds = qs.QueryWebchatPayActivity(cft_no, s_begindate, s_enddate, act_id, start, max);
            DataSet ds = null;
            if (act_id == "wxzfact")
            {
                ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, s_enddate);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
                else
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                }
            }
            else if (act_id == "xyk") 
            {
                
                string wxUin =  WeChatHelper.GetUINFromWeChatName(cft_no);
                this.lblAccUin.Text = wxUin;
                //lblTotal.Text = "0";

                //发送
                ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, xykTime, s_enddate, "send");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //lblTotal.Text = ds.Tables[0].Rows[0]["FSendCount"].ToString();

                    SendDG.DataSource = ds.Tables[0].DefaultView;
                    SendDG.DataBind();
                }
                else 
                {
                    SendDG.DataSource = null;
                    SendDG.DataBind();
                }

                ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, xykTime, s_enddate, "rec");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
                {
                    ReceiveDG.DataSource = ds.Tables[0].DefaultView;
                    ReceiveDG.DataBind();
                }
                else
                {
                    ReceiveDG.DataSource = null;
                    ReceiveDG.DataBind();
                }

                SendDetailDG.DataSource = null;
                SendDetailDG.DataBind();
            }
            
			
		}

        private void BindSendData(int index)
        {
            string s_stime = TextBoxBeginDate.Text;
            string s_begindate = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyy-MM-01 00:00:00");
            }
            
            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, "", "send");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SendDG.DataSource = ds.Tables[0].DefaultView;
                SendDG.DataBind();
            }
            else
            {
                SendDG.DataSource = null;
                SendDG.DataBind();
            }

            SendDetailDG.DataSource = null;
            SendDetailDG.DataBind();
        }

        private void BindReceiveData(int index)
        {
            string s_stime = TextBoxBeginDate.Text;
            string s_begindate = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyy-MM-01 00:00:00");
            }
            
            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, "", "rec");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ReceiveDG.DataSource = ds.Tables[0].DefaultView;
                ReceiveDG.DataBind();
            }
            else
            {
                ReceiveDG.DataSource = null;
                ReceiveDG.DataBind();
            }

            SendDetailDG.DataSource = null;
            SendDetailDG.DataBind();
        }

        private void SendDG_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            string sendid = e.Item.Cells[0].Text.Trim();
            GetDetail(sendid);
        }
        private void GetDetail(string sendid) 
        {
            DataSet ds = new ActivityService().QueryXYKSendDetail(sendid);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SendDetailDG.DataSource = ds.Tables[0].DefaultView;
                SendDetailDG.DataBind();
            }
            else
            {
                SendDetailDG.DataSource = null;
                SendDetailDG.DataBind();
            }
        }
	}
}