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
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SynRecordQuery_Detail 的摘要说明。
	/// </summary>
	public partial class SynRecordQuery_Detail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				labUid.Text = Session["uid"].ToString();
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				
				string tranid = Request.QueryString["tranid"];
				string createtime = Request.QueryString["createtime"];
				string flag = Request.QueryString["flag"];

				if(tranid == null || tranid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
				}


				try
				{
					ViewState["tranid"] = tranid;
					ViewState["createtime"] = createtime;
					ViewState["flag"] = flag;

					BindInfo(tranid,createtime,flag);
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

		}
		#endregion

		private void BindInfo(string tranid, string createtime, string flag)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			DataSet ds =  qs.GetSynRecordQueryDetail(tranid,createtime,Int32.Parse(flag));

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];

				labFtransaction_id.Text = PublicRes.GetString(dr["Ftransaction_id"]);
				labFpay_statusName.Text = PublicRes.GetString(dr["Fpay_statusName"]);
				labFsyn_count.Text = PublicRes.GetString(dr["Fsyn_count"]);
				labFsyn_statusName.Text = PublicRes.GetString(dr["Fsyn_statusName"]);
				labFrec_statusName.Text = PublicRes.GetString(dr["Frec_statusName"]);
				labFsyn_typeName.Text = PublicRes.GetString(dr["Fpay_statusName"]);
				labFpay_typeName.Text = PublicRes.GetString(dr["Fpay_typeName"]);
				labFpay_channelName.Text = PublicRes.GetString(dr["Fpay_channelName"]);
				labFverName.Text = PublicRes.GetString(dr["FverName"]);
				labFsp_id.Text = PublicRes.GetString(dr["Fsp_id"]);
				labFsp_billno.Text = PublicRes.GetString(dr["Fsp_billno"]);
				labFsp_timeName.Text = PublicRes.GetString(dr["Fsp_timeName"]);
                labFbank_typeName.Text = PublicRes.GetString(Transfer.returnDicStr("BANK_TYPE", PublicRes.GetInt(dr["Fbank_type"])));
				labFbank_billno.Text = PublicRes.GetString(dr["Fbank_billno"]);
				labFbank_ret_billno.Text = PublicRes.GetString(dr["Fbank_ret_billno"]);
				labFpurchaser_uin.Text = PublicRes.GetString(dr["Fpurchaser_uin"]);
				labFpurchaser_true_name.Text = PublicRes.GetString(dr["Fpurchaser_true_name"]);
				labFbargainor_uin.Text = PublicRes.GetString(dr["Fbargainor_uin"]);
				labFbargainor_true_name.Text = PublicRes.GetString(dr["Fbargainor_true_name"]);
				labFgoods_tag.Text = PublicRes.GetString(dr["Fgoods_tag"]);
				labFdesc.Text = PublicRes.GetString(dr["Fdesc"]);
				labFtotal_feeName.Text = PublicRes.GetString(dr["Ftotal_feeName"]);
				labFpriceName.Text = PublicRes.GetString(dr["FpriceName"]);
				labFtransport_feeName.Text = PublicRes.GetString(dr["Ftransport_feeName"]);
				labFprocedure_feeName.Text = PublicRes.GetString(dr["Fprocedure_feeName"]);
				labFfee_typeName.Text = PublicRes.GetString(dr["Ffee_typeName"]);
				labFfee1Name.Text = PublicRes.GetString(dr["Ffee1Name"]);
				labFfee2Name.Text = PublicRes.GetString(dr["Ffee2Name"]);
				labFfee3Name.Text = PublicRes.GetString(dr["Ffee3Name"]);
				labFvfeeName.Text = PublicRes.GetString(dr["FvfeeName"]);
				labFrp_feeName.Text = PublicRes.GetString(dr["Frp_feeName"]);
				labFrb_feeName.Text = PublicRes.GetString(dr["Frb_feeName"]);
				labFclient_ip.Text = PublicRes.GetString(dr["Fclient_ip"]);
				labFsp_ip.Text = PublicRes.GetString(dr["Fsp_ip"]);
				labFcreat_timeName.Text = PublicRes.GetString(dr["Fcreat_timeName"]);
				labFpay_timeName.Text = PublicRes.GetString(dr["Fpay_timeName"]);
				labFlast_syn_timeName.Text = PublicRes.GetString(dr["Flast_syn_timeName"]);
				labFlast_modify_timeName.Text = PublicRes.GetString(dr["Flast_modify_timeName"]);
				Fsyn_resultName.Text = PublicRes.GetString(dr["Fsyn_resultName"]);		
		
				labFMemo.Text = PublicRes.GetString(dr["FMemo"]);
				labFsp_mac.Text = PublicRes.GetString(dr["Fsp_mac"]);
				labFReturn_Url.Text = PublicRes.GetString(dr["Freturn_url"]);

				Button1.Visible = flag == "0";
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			//把同步次数重置为指定值，把同步状态修改成失败。
			try
			{
				string tranid = ViewState["tranid"].ToString();
				string createtime = ViewState["createtime"].ToString();
				string flag = ViewState["flag"].ToString();
			
				int inum = Int32.Parse(tbNewCount.Text.Trim());

				if(inum < 0 )
				{
					throw new LogicException("请给出正确的同步次数");
				}

				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				qs.ResetSynRecordState(tranid,createtime,inum);

				BindInfo(tranid,createtime,flag);
			}
			catch(Exception err)
			{
				string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
				WebUtils.ShowMessage(this.Page,"同步失败：" + errStr);
			}
		}
	}
}
