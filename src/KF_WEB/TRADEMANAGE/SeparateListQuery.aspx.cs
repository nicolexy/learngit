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
namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SeparateListQuery 的摘要说明。
	/// </summary>
	public partial class SeparateListQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Web.UI.WebControls.DataGrid dgListFlist;
	
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
				this.rtnList.Checked = true;
				TextBoxBeginDate.Text = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).ToString("yyyy-MM-dd");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void CheckData()
		{
			ViewState["IsrtnList"] = true;

			if(this.rtnList.Checked)
			{
				if(this.txtFlistid.Text.Trim().Length != 28)
					throw new Exception("请输入28位订单号！");
				else
					ViewState["Flistid"] = this.txtFlistid.Text.Trim();
			}
			else if(this.rtbSpid.Checked)
			{
				ViewState["IsrtnList"] = false;

				DateTime begindate;
				DateTime enddate;
				try
				{
					begindate = DateTime.Parse(TextBoxBeginDate.Text);
					enddate = DateTime.Parse(TextBoxEndDate.Text);
					ViewState["begindate"] = begindate;
					ViewState["enddate"] = enddate;
				}
				catch
				{
					throw new Exception("日期输入有误！");
				}

				if(begindate.CompareTo(enddate) > 0)
				{
					throw new Exception("终止日期小于起始日期，请重新输入！");
				}

				if(begindate.Year != enddate.Year || begindate.Month != enddate.Month)
				{
					throw new Exception("不允许跨月查询！");
				}

				if(this.txtFspid.Text.Trim() == "")
					throw new Exception("请输入商户号！");
				else
					ViewState["Fspid"] = this.txtFspid.Text.Trim();
			}
			else
				throw new Exception("请选择一种查询方式！");
		}

		private void BindData(int index)
		{
			int max = pager.PageSize;
			int start = max * (index-1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds;
			if(ViewState["IsrtnList"].ToString() == "True")
			{
				ds = qs.GetSeparateListForFlistid(ViewState["Flistid"].ToString(),start,max);
				this.DataGrid1.Columns[2].HeaderText = "创建时间";
			}
			else
			{
				ds = qs.GetSeparateListForFspid(ViewState["Fspid"].ToString(),DateTime.Parse(ViewState["begindate"].ToString()),DateTime.Parse(ViewState["enddate"].ToString()),start,max);
				this.DataGrid1.Columns[2].HeaderText = "支付时间";
			}

			DataTable dt = ds.Tables[0];
			//dt.Columns.Add("Time",typeof(string));
			dt.Columns.Add("FpaynumStr",typeof(string));
			dt.Columns.Add("Fsttl_feeStr",typeof(string));
			dt.Columns.Add("Frefund_feeStr",typeof(string));
			dt.Columns.Add("Frf_feeStr",typeof(string));
			dt.Columns.Add("Foffer_feeStr",typeof(string));
			dt.Columns.Add("Fadjustin_feeStr",typeof(string));
			dt.Columns.Add("Fadjustout_feeStr",typeof(string));

			foreach(DataRow dr in dt.Rows)
			{
				//dr["Time"] = DateTime.Parse(dr["TimeStr"].ToString());
				dr["FpaynumStr"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
				dr["Fsttl_feeStr"] = MoneyTransfer.FenToYuan(dr["Fsttl_fee"].ToString());
				dr["Frefund_feeStr"] = MoneyTransfer.FenToYuan(dr["Frefund_fee"].ToString());
				dr["Frf_feeStr"] =  MoneyTransfer.FenToYuan(dr["Frf_fee"].ToString());
				dr["Foffer_feeStr"] = MoneyTransfer.FenToYuan(dr["Foffer_fee"].ToString());
				if(dr["Foffer_sign"].ToString() == "2")//Foffer_sign:1正，2负
					dr["Foffer_feeStr"] = "-" + dr["Foffer_feeStr"].ToString();
				dr["Fadjustin_feeStr"] =  MoneyTransfer.FenToYuan(dr["Fadjustin_fee"].ToString());
				dr["Fadjustout_feeStr"] =  MoneyTransfer.FenToYuan(dr["Fadjustout_fee"].ToString());
				//Fadjustin_fee和Fadjustout_fee

				/*Foffer_fee！＝０　可调
				Fadjustin_fee－Fadjustout_fee！＝０一定调过　＝０说明可能跳过
				Fadjustin_fee和Fadjustout_fee都为０没调*/
                string spid = dr["Flistid"].ToString().Substring(0, 10);
                if (!PublicRes.isWhiteOfSeparate(spid, Session["uid"].ToString())) 
                {
                    dr.BeginEdit();
                    string s_fcoding = dr["Fcoding"].ToString();
                    dr["Fcoding"] = classLibrary.setConfig.ConvertID(s_fcoding, 0, 4);
                    dr.EndEdit();
                }
			}
			this.DataGrid1.DataSource = dt.DefaultView;
			this.DataGrid1.DataBind();

		}
		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				CheckData();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				pager.RecordCount= 10000;
				BindData(1);
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

	}
}
