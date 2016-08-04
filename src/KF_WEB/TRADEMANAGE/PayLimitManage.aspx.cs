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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using CFT.CSOMS.BLL.TradeModule;



namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PayLimitManage 的摘要说明。
	/// </summary>
	public partial class PayLimitManage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected Wuqi.Webdiyer.AspNetPager pager;
		protected System.Web.UI.WebControls.DataGrid DataGrid2;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				Table2.Visible = false;
				BindChannel(ddlChannel);
			}
		}

		private void BindChannel(DropDownList ddl)
		{
			ddl.Items.Clear();
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.GetAllChannelList();
            
				if(ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
				{
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						ddl.Items.Add(new ListItem(dr["Fchannel_name"].ToString(),dr["Fchannel_id"].ToString()));
					}
				}

				ddl.Items.Insert(0,new ListItem("所有渠道","0"));
			}
			catch
			{}
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
			this.DataGrid1.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid1_PageIndexChanged);

		}
		#endregion

		private void ValidateDate()
		{
			ViewState["uid"] = tbQQID.Text.Trim();

			if(tbQQID.Text.Trim() == "")
			{
				throw new LogicException("请输入帐号");
			}

			ViewState["channelid"] = ddlChannel.SelectedValue;
			ViewState["auid"] = tbaqqID.Text.Trim();
			if(this.rbtKeap.Checked)
				ViewState["IsKeap"] = "true";
			else
				ViewState["IsKeap"] = "false";
		}

		protected void Button2_Click(object sender, System.EventArgs e)
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
				Table2.Visible = true;
				this.DataGrid1.CurrentPageIndex = 0;
				BindData();
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}


		private void BindData()
		{
			string u_ID = ViewState["uid"].ToString();
			int channelid = Int32.Parse(ViewState["channelid"].ToString());
			string  auid = ViewState["auid"].ToString();
			string IsKeap = ViewState["IsKeap"].ToString();

			if(IsKeap == "true")
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.GetPayLimitList(u_ID,channelid,auid);

				if(ds != null && ds.Tables.Count >0)
				{
					ds.Tables[0].Columns.Add("FdirectName",typeof(String));
					ds.Tables[0].Columns.Add("Flist_stateName",typeof(String));

					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						if(dr["Fdirect"].ToString() == "1")
							dr["FdirectName"] = "出";
						else if(dr["Fdirect"].ToString() == "2")
							dr["FdirectName"] = "入";

						if(dr["Flist_state"].ToString() == "1")
							dr["Flist_stateName"] = "正常";
						else if(dr["Flist_state"].ToString() == "2")
							dr["Flist_stateName"] = "作废";
					}

					DataGrid1.DataSource = ds.Tables[0].DefaultView;
					DataGrid1.DataBind();
				}
				else
				{
					throw new LogicException("没有找到记录！");
				}
			}
			else
			{
				if(auid == "")
				{
					throw new Exception("请输入对方帐号！");
				}

                //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //DataSet ds = qs.GetTrustLimitList(u_ID,auid);

                SettleService service = new SettleService();
                DataTable dt = service.GetTrustLimitList(u_ID, auid);

				if(dt != null  && dt.Rows.Count>0)
				{
					try
					{
						int Ftrust_rule = int.Parse(dt.Rows[0]["Ftrust_rule"].ToString().Trim());
						string Ftrust_ruleStr = Convert.ToString((long)Ftrust_rule,2);
						//最后一位为退款权限
						if(Ftrust_ruleStr.EndsWith("1"))
							this.lblTrustLimit.Text = "委托退款权限开通";
						else
							this.lblTrustLimit.Text = "委托退款权限未开通";

					}
					catch
					{
						this.lblTrustLimit.Text = "委托退款权限未开通";
					}
				}
				else
				{
					this.lblTrustLimit.Text = "委托退款权限未开通";
				}
			}
		}

		private void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}
	}
}
