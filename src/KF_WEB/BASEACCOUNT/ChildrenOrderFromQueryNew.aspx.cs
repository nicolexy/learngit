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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// ChildrenOrderFromQueryNew 的摘要说明。
	/// </summary>
	public partial class ChildrenOrderFromQueryNew : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("ChangeUserInfo", this)) Response.Redirect("../login.aspx?wh=1");
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

		}
		#endregion

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				int iType = 1;
			
				DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime);
				DateTime endTime   = DateTime.Parse(PublicRes.sEndTime);

				int istr = 1;
				int imax = 2;
		
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				DataSet ds = qs.GetPayListForChildren(this.tbFlistid.Text.Trim(),int.Parse(this.dd_curType.SelectedValue),iType,beginTime,endTime,istr,imax);				
				if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1) 
				{
					throw new Exception("数据库无此记录");					
				}			
			
				this.LB_Fbank_backid.Text = ds.Tables[0].Rows[0]["Fbank_backid"].ToString();
				this.LB_Fbank_listid.Text = ds.Tables[0].Rows[0]["Fbank_listid"].ToString();
				this.LB_Fspid.Text = ds.Tables[0].Rows[0]["Fspid"].ToString().Trim();
				this.LB_Fcurtype.Text = classLibrary.setConfig.convertMoney_type(ds.Tables[0].Rows[0]["Fcurtype"].ToString());
				this.DropDownList2_tradeState.SelectedValue = ds.Tables[0].Rows[0]["Fstate"].ToString();
				this.LB_Flstate.Text = classLibrary.setConfig.convertTradeState(ds.Tables[0].Rows[0]["Flstate"].ToString());
				this.LB_Fcreate_time.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
				this.LB_Fip.Text = ds.Tables[0].Rows[0]["Fip"].ToString();
				this.LB_FMemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
				this.LB_Fexplain.Text = ds.Tables[0].Rows[0]["Fexplain"].ToString();
				this.LB_Fcatch_desc.Text = ds.Tables[0].Rows[0]["Fcatch_desc"].ToString();
				this.LB_Fpay_type.Text = classLibrary.setConfig.cPay_type(ds.Tables[0].Rows[0]["Fpay_type"].ToString());
				this.LB_Fchannel_id.Text = ChannelNo(ds.Tables[0].Rows[0]["Fchannel_id"].ToString());
				this.LB_Fmediuid.Text = ds.Tables[0].Rows[0]["Fmediuid"].ToString();
				this.LB_Fmedinum.Text =  MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fmedinum"]));
				this.LB_Fmodify_time.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
				this.lbTradeType.Text  = classLibrary.setConfig.convertPayType(ds.Tables[0].Rows[0]["Ftrade_type"].ToString().Trim());
				this.lbAdjustFlag.Text = classLibrary.setConfig.convertAdjustSign(ds.Tables[0].Rows[0]["Fadjust_flag"].ToString().Trim());
				this.LB_Flistid.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();

				LB_Fchargeuid.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fchargeuid"]);
				LB_Fchargenum.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fchargenum"]));
				LB_Ftotalnum.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Ftotalnum"]));
				LB_Fbuyerpaytotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fbuyerpaytotal"]));
		
				LB_Fbuyerrefundtotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fbuyerrefundtotal"]));
				LB_Fsellerpaytotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fsellerpaytotal"]));
				LB_Fsellerrefundtotal.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fsellerrefundtotal"]));
				LB_Frolenum.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Frolenum"]);

				LB_Fuid0.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid0"]);
				LB_Fplanpaynum0.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum0"]));
				LB_Fpaynum0.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum0"]));
				LB_Frefund0.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund0"]));

				LB_Fuid1.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid1"]);
				LB_Fplanpaynum1.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum1"]));
				LB_Fpaynum1.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum1"]));
				LB_Frefund1.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund1"]));

				LB_Fuid2.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid2"]);
				LB_Fplanpaynum2.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum2"]));
				LB_Fpaynum2.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum2"]));
				LB_Frefund2.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund2"]));

				LB_Fuid3.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fuid3"]);
				LB_Fplanpaynum3.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fplanpaynum3"]));
				LB_Fpaynum3.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Fpaynum3"]));
				LB_Frefund3.Text = MoneyTransfer.FenToYuan(PublicRes.GetString(ds.Tables[0].Rows[0]["Frefund3"]));

				try
				{
					this.lb_buyerId.Text = ds.Tables[0].Rows[0]["Fbuyid"].ToString();
					this.lb_sellerID.Text = ds.Tables[0].Rows[0]["Fsaleid"].ToString();
				}
				catch(Exception ex)
				{
					string str = ex.Message;
				}
			}
			catch(Exception ex)
			{
				if(ex.Message.IndexOf("没有记录",0) >= 0)
				{
					Response.Write("<script language=javascript>alert('" + "查询结果为空！" + "')</script>");
				}
				else
				{
					WebUtils.ShowMessage(this.Page,ex.Message);
				}
			}
		}

		private string ChannelNo(string channelId)
		{
			if (channelId == null || channelId.Trim() == "")
			{
				return "";
			}
			else if (channelId.Trim() == "1")
			{
				return "财付通";
			}
			else if (channelId.Trim() == "2")
			{
				return "拍拍";
			}
			else if (channelId.Trim() == "3")
			{
				return "客户端小钱包";
			}
			else if (channelId.Trim() == "4")
			{
				return "手机支付";
			}
			else if (channelId.Trim() == "5")
			{
				return "第三方";
			}
			else if (channelId.Trim() == "6")
			{
				return "IVR";
			}
			else
			{
				return "未定义[" + channelId + "]";
			}
		}

	}
}
