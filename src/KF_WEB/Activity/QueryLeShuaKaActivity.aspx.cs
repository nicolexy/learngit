using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.ActivityModule;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.KF_Web.Activity
{
	/// <summary>
    /// QueryLeShuaKaActivity 的摘要说明。
	/// </summary>
    public partial class QueryLeShuaKaActivity : System.Web.UI.Page
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
            DateTime begindate, enddate;

            try
            {
                string s_date = TextBoxBeginDate.Value;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Value;
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
                throw new Exception("财付通账号不能为空！");
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
                BindData(1);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
			}
			catch(Exception eSys)
			{
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString() + ", stacktrace" + eSys.StackTrace);
			}
		}

        private void BindData(int index)
		{

            string s_stime = TextBoxBeginDate.Value;
            string s_begindate = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
            }
            string s_etime = TextBoxEndDate.Value;
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

            DataSet ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, s_enddate);

			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("FBitMaskStr", typeof(String));//属性组合
                ds.Tables[0].Columns.Add("FStateStr", typeof(String));//状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("0", "待领取");
                ht1.Add("1", "领取中");
                ht1.Add("2", "领取成功，未到账");
                ht1.Add("3", "发送奖品中");
                ht1.Add("4", "领取成功，已到账，需展示");
                ht1.Add("5", "领取成功，已到账，仍然展示");

                Hashtable ht2 = new Hashtable();
                ht2.Add("caipiao_sli", "彩票沉默用户");
                ht2.Add("credit_new", "信用卡新用户");
                ht2.Add("non_fastpay", "非快捷用户");
                ht2.Add("paipai_mc_silent", "拍拍话费充值沉默用户");
                ht2.Add("cft_mc_silent", "财付通话费充值沉默用户");
                ht2.Add("yixun_silent", "易迅沉默用户");
                ht2.Add("weixin_pay", "微信支付用户");
                ht2.Add("dianping_new", "点评团用户");
                ht2.Add("huanledou_new", "欢乐豆新用户");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "FState", "FStateStr", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "FBitMask", "FBitMaskStr", ht2);

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
			}
		}

	}
}