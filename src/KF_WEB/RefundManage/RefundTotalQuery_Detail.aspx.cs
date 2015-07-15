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
using System.Web.Services.Protocols;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.Common;
namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// RefundTotalQuery_Detail 的摘要说明。
	/// </summary>
    public partial class RefundTotalQuery_Detail : System.Web.UI.Page
	{
    
		private void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面


            try
            {
                string uid = Session["uid"].ToString();
                string szkey = Session["szkey"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch  //如果没有登陆或者没有权限就跳出
            {
                Response.Redirect("../login.aspx?wh=1");
            }


			if(!this.IsPostBack)
			{
				string refundID = Request.QueryString["RefundID"];

				string batchId = Request.QueryString["batchid"];

				if(refundID == null || refundID.Trim() == ""||batchId==null||batchId.Trim()=="")
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
					return;
				}

				try
				{
					BindInfo(refundID,batchId);
					ViewState["refundID"] = refundID;
					ViewState["batchId"] = batchId;
					
				}
				catch(LogicException err)
				{
					WebUtils.ShowMessage(this.Page,err.Message);
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region 本地方法
		
		private void BindInfo(string refundID,string batchid)
		{

            FINANCE_RefundSERVICE.Query_Service qs = new FINANCE_RefundSERVICE.Query_Service();

			DataSet ds =  qs.GetRefundTotalDetail(refundID,batchid);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];				

				if(dr["FPaylistid"]!=null&&dr["FPaylistid"].ToString()!=""&&dr["FPaylistid"].ToString().Length>10)
				{
					labFspid.Text = PublicRes.GetString(dr["FPaylistid"].ToString().Substring(0,10));
				}

				labFPaylistid.Text = PublicRes.GetString(dr["FPaylistid"]);
				labFbank_listid.Text = PublicRes.GetString(dr["Fbank_listid"]);
				labFbank_backid.Text = PublicRes.GetString(dr["Fbank_backid"]);
				labFbanktype.Text = classLibrary.setConfig.returnDicStr("BANK_TYPE",PublicRes.GetInt(dr["Fbanktype"]));

				
				labFamt.Text = PublicRes.GetString(dr["FamtName"]);
				labFreturnamt.Text = PublicRes.GetString(dr["FreturnamtName"]);
				labFbuyid.Text = PublicRes.GetString(dr["Fbuyid"]);

				labFbuy_name.Text = PublicRes.GetString(dr["Fbuy_name"]);
				labFbuy_bankid.Text = PublicRes.GetString(dr["Fbuy_bankid"]);
				labFPay_time.Text = PublicRes.GetString(dr["FPay_time"]);
				labFListid.Text = PublicRes.GetString(dr["FListid"]);
				labFCreateTime.Text = PublicRes.GetString(dr["FCreateTime"]);
				labFstate.Text = PublicRes.GetString(dr["FstateName"]);

				labFlstate.Text = PublicRes.GetString(dr["FlstateName"]);
				labFAdjustType.Text = PublicRes.GetString(dr["FAdjustTypeName"]);
				labFreturnState.Text = PublicRes.GetString(dr["FreturnStateName"]);
				labFrefundType.Text = PublicRes.GetString(dr["FrefundTypeName"]);
				labFrefundPath.Text = PublicRes.GetString(dr["FrefundPathName"]);

				labFmodify_time.Text = PublicRes.GetString(dr["Fmodify_Time"]);
				labFmemo.Text = PublicRes.GetString(dr["Fmemo"]);
				labFexplain.Text = PublicRes.GetString(dr["Fexplain"]);

				this.labFoldid.Text = PublicRes.GetString(dr["Foldid"]);
				this.labFCreateMemo.Text = PublicRes.GetString(dr["FCreateMemo"]);
				labFoldid.NavigateUrl="RefundErrorQuery_Detail.aspx?oldid="+PublicRes.GetString(dr["Foldid"]);
				
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}
		#endregion
	}
}
