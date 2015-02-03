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

using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.RemitCheck
{
	/// <summary>
	/// RemitQuery 的摘要说明。
	/// </summary>
	public partial class RemitQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//			try
			//			{
			//				string sr = Session["key"].ToString();
			//				if (!AllUserRight.GetOneRightState("AgentFeeManage",sr)) Response.Redirect("../login.aspx?wh=1");
			//
			//			}
			//			catch
			//			{
			//				Response.Redirect("../login.aspx?wh=1");
			//			}

			try
			{
				//Label1.Text = Session["uid"].ToString();
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
				BindSpid();
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

		}
		#endregion



		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			labErrMsg.Text = "";	
			
			try
			{
				pager.RecordCount= GetCount(); 
				BindData(1);
			}
			catch(Exception ex)
			{
				string msg=ex.Message.Replace("\'","\\\'");
				WebUtils.ShowMessage(this.Page,msg);
			}
		}

		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}

		
		private void BindSpid()
		{
			Agent_Service.Agent_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Agent_Service.Agent_Service();
			string[] spids = null;
			try
			{
				spids=qs.GetRemitSpid();
			}
			catch
			{
				ShowMsg("spid未配置！");
				return;
			}
			if(spids!=null)
			{
				this.ddlSpid.Items.Clear();
				foreach(string spid in spids)
				{
					if(spid.Length==10)
					{
						ddlSpid.Items.Add(spid);
					}
				}
			}
		}

		private void BindData(int index)
		{
			string tranType = this.ddlTrantype.SelectedValue;
			string dataType = this.ddlDataType.SelectedValue;
			string remitType = this.ddlRemitType.SelectedValue;
			string tranState = this.ddlTranState.SelectedValue;
			string spid=ddlSpid.SelectedValue;
			string remitRec = this.tbRemitRec.Text.Trim();
			string listID=tblistID.Text.Trim();
			
			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Agent_Service.Agent_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Agent_Service.Agent_Service();

			DataSet ds = qs.GetRemitDataList("",tranType,dataType,remitType,tranState,spid,remitRec,listID, start,max);

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("FremitfeeName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fremit_fee","FremitfeeName");

				ds.Tables[0].Columns.Add("FremitpayfeeName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fremit_pay_fee","FremitpayfeeName");

				ds.Tables[0].Columns.Add("FprocedureName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fprocedure","FprocedureName");

				ds.Tables[0].Columns.Add("FotherprocedureName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fother_procedure","FotherprocedureName");

				ds.Tables[0].Columns.Add("FtranstateName",typeof(String));
				ds.Tables[0].Columns.Add("FtrantypeName",typeof(String));
				ds.Tables[0].Columns.Add("FremittypeName",typeof(String));
				ds.Tables[0].Columns.Add("FdatatypeName",typeof(String));
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();

					string tmp = dr["Ftran_type"].ToString();
					if(tmp == "1") 
						tmp = "汇款";
					else if(tmp == "2") 
						tmp = "退汇";
					else if(tmp == "3") 
						tmp = "改汇";
					else if(tmp == "4") 
						tmp = "逾期退汇";
					else 
						tmp="状态未知:"+tmp;
					dr["FtrantypeName"] = tmp;


					tmp = dr["Ftran_state"].ToString();
					if(tmp == "1") 
						tmp = "成功";
					else if(tmp == "2") 
						tmp = "失败";
					else if(tmp == "3") 
						tmp = "挂起";
					else if(tmp == "4") 
						tmp = "挂起成功";
					else if(tmp == "5") 
						tmp = "挂起失败";
					else 
						tmp = "状态未知:"+tmp;
					dr["FtranstateName"] = tmp;


					tmp = dr["Fremit_type"].ToString();
					if(tmp == "1") 
						tmp = "按地址汇款";
					else if(tmp == "2") 
						tmp = "按密码汇款";
					else if(tmp == "3") 
						tmp = "入账汇款";
					else 
						tmp = "状态未知:"+tmp;
					dr["FremittypeName"] = tmp;


					tmp = dr["Fdata_type"].ToString();
					if(tmp == "1") 
						tmp = "汇款直接成功";
					else if(tmp == "2") 
						tmp = "汇款挂起";
					else if(tmp == "3") 
						tmp = "汇款失败";
					else if(tmp == "4") 
						tmp = "汇款挂起后成功";
					else if(tmp=="5")
						tmp="汇款挂起后失败";
					else if(tmp == "6") 
						tmp = "退汇成功";
					else if(tmp == "7") 
						tmp = "退汇挂起";
					else if(tmp == "8") 
						tmp = "退汇失败";
					else if(tmp=="9")
						tmp="退汇挂起后成功";
					else if(tmp == "10") 
						tmp = "退汇挂起后失败";
					else if(tmp == "11") 
						tmp = "改汇成功";
					else if(tmp == "12") 
						tmp = "改汇挂起";
					else if(tmp=="13")
						tmp="改汇失败";
					else if(tmp=="14")
						tmp="改汇挂起成功";
					else if(tmp=="15")
						tmp="改汇挂起失败";
					else if(tmp=="16")
						tmp="逾期退汇";
					else if(tmp=="21")
						tmp="邮储线下退汇";
					else
						tmp = "状态未知:"+tmp;
					dr["FdatatypeName"] = tmp;


					dr.EndEdit();
				}

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"没有找到记录");
			}
		}

		
		
		private int GetCount()
		{
			string tranType = this.ddlTrantype.SelectedValue;
			string dataType = this.ddlDataType.SelectedValue;
			string remitType = this.ddlRemitType.SelectedValue;
			string tranState = this.ddlTranState.SelectedValue;
			string spid=ddlSpid.SelectedValue;
			string remitRec = this.tbRemitRec.Text.Trim();
			string listID=tblistID.Text.Trim();
			
			Agent_Service.Agent_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Agent_Service.Agent_Service();
			return qs.GetRemitListCount("",tranType,dataType,remitType,tranState,spid,remitRec,listID);
		}


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
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + classLibrary.setConfig.replaceHtmlStr(eSys.Message));
			}
		}
	}
}
