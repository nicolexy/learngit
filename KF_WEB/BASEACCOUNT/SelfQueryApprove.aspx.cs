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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// SelfQueryApprove 的摘要说明。
	/// </summary>
	public partial class SelfQueryApprove : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"DrawAndApprove")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("DrawAndApprove",this)) Response.Redirect("../login.aspx?wh=1");

				ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
				ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
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
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(e.NewPageIndex);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + classLibrary.setConfig.replaceMStr(eSys.Message) );
			}
		}


		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			string strszkey = Session["SzKey"].ToString().Trim();
			int ioperid = Int32.Parse(Session["OperID"].ToString());
			int iserviceid = Common.AllUserRight.GetServiceID("DrawAndApprove") ;
			string struserdata = Session["uid"].ToString().Trim();
			string content = struserdata + "执行了[自助商户审核查询]操作,操作对象[" + this.BoxCpName.Text.Trim()
				+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

			

			try
			{
				pager.RecordCount = GetCount(); 
				lblResultCount.Text = pager.RecordCount.ToString();
				BindData(1);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + classLibrary.setConfig.replaceMStr(eSys.Message));
			}
		}


		private int GetCount()
		{
			string filter = GetfilterString();
			ViewState["filter"] = filter;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return Convert.ToInt32(qs.GetSelfQueryListCount(filter).Tables[0].Rows[0][0]);
		}

		private void BindData(int index)
		{
			string filter = ViewState["filter"].ToString();

			int TopCount = pager.PageSize;
			int NotInCount = TopCount * (index-1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

//			Finance_Header fh = new Finance_Header();
//			fh.UserIP = Request.UserHostAddress;
//			fh.UserName = Session["uid"].ToString();
//			fh.OperID = Int32.Parse(Session["OperID"].ToString());
//			fh.SzKey = Session["SzKey"].ToString();
//
//			qs.Finance_HeaderValue = fh;
			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			//			fh.UserIP = Request.UserHostAddress;
			//			fh.UserName = Session["uid"].ToString();
			//
			//			fh.OperID = Int32.Parse(Session["OperID"].ToString());
			//			fh.SzKey = Session["SzKey"].ToString();
			//
			qs.Finance_HeaderValue = fh;

			string log = SensitivePowerOperaLib.MakeLog("get",Session["uid"].ToString().Trim(),"[自助商户审核查询]",filter
				,TopCount.ToString(),NotInCount.ToString());

			if(!SensitivePowerOperaLib.WriteOperationRecord("DrawAndApprove",log,this))
			{
					
			}

			DataSet ds = qs.GetSelfQueryList(filter,TopCount,NotInCount);

			if(ds != null && ds.Tables.Count >0)
			{
				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}


		#region 获得查询字符串
		protected string GetfilterString()
		{
			string filter = " and DraftFlag=0 and Flag=-1 and KFCheckUser='" + Session["uid"].ToString() + "' ";

			if(BoxCpName.Text.Trim() != "")
			{
				filter += " and CompanyName like '%" + BoxCpName.Text.Trim() + "%' ";
			}

			if(BoxCpNumber.Text.Trim() != "")
			{
				filter += " and SPID='" + BoxCpNumber.Text.Trim() + "' ";
			}

			if(boxWWWAddress.Text.Trim() != "")
			{
				filter += " and WWWAdress like '%" + boxWWWAddress.Text.Trim() + "%' ";
			}

			if(TextBoxBeginDate.Text.Trim() != "")
			{
				filter += " and ApplyTime >='" + Convert.ToDateTime(TextBoxBeginDate.Text.Trim()).ToString("yyyy-MM-dd 00:00:00") + "' ";
			}

			if(TextBoxEndDate.Text.Trim() != "")
			{
				filter += " and ApplyTime <='" + Convert.ToDateTime(TextBoxEndDate.Text.Trim()).ToString("yyyy-MM-dd 23:59:59") + "' ";
			}

			if(tbBankName.Text.Trim() != "")
			{
				filter += " and BankUserName like '%" + tbBankName.Text.Trim() + "%' ";
			}

			if(tbSuggestUser.Text.Trim() != "")
			{
				filter += " and SuggestUser = '" + tbSuggestUser.Text.Trim() + "' ";
			}

			return filter;			
		}

		#endregion
	}
}