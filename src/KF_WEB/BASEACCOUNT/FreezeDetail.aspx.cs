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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// FreezeDetail 的摘要说明。
	/// </summary>
	public partial class FreezeDetail : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
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

				string tdeid = Request.QueryString["fid"];


				if(tdeid == null || tdeid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
				}

				try
				{
					string strszkey = Session["SzKey"].ToString().Trim();
					int ioperid = Int32.Parse(Session["OperID"].ToString());
                    int iserviceid = Common.AllUserRight.GetServiceID("InfoCenter");
					string struserdata = Session["uid"].ToString().Trim();
					string content = struserdata + "执行了[查看冻结详情]操作,操作对象[" + tdeid
						+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

					Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);


					string log = SensitivePowerOperaLib.MakeLog("get",struserdata,"[查看冻结详情]",tdeid);

                    if (!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", log, this))
					{
						
					}

					ViewState["UserName"] = Session["uid"].ToString().Trim();
					BindInfo(tdeid);
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


		private void BindInfo(string tdeid)
		{
			this.btnSave.Visible = false;

			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			DataSet ds =  qs.GetFreezeListDetail(tdeid);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];

				lblfid.Text = tdeid;
				labFUserName.Text = PublicRes.GetString(dr["FUserName"]);
				labFContact.Text = PublicRes.GetString(dr["FContact"]);
				string tmp = PublicRes.GetString(dr["FFreezeType"]);
				if(tmp == "1")
				{
					labFFreezeTypeName.Text = "冻结帐户";
				}
				else if(tmp == "2")
				{
					labFFreezeTypeName.Text = "锁定交易单";
				}

				labFFreezeID.Text = PublicRes.GetString(dr["FFreezeID"]);
				labFFreezeUserID.Text = PublicRes.GetString(dr["FFreezeUserID"]);
				labFFreezeTime.Text = PublicRes.GetDateTime(dr["FFreezeTime"]);
				txtFFreezeReason.Text = PublicRes.GetString(dr["FFreezeReason"]);
				labFHandleUserID.Text = PublicRes.GetString(dr["FHandleUserID"]);
				labFHandleTime.Text = PublicRes.GetDateTime(dr["FHandleTime"]);
				labFHandleResult.Text = PublicRes.GetString(dr["FHandleResult"]);
                labFfreezeChannel.Text = GetFreezeChannelStr(PublicRes.GetString(dr["Ffreeze_channel"]));
				this.btnSave.Visible = true;
				
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

        private string GetFreezeChannelStr(string type) 
        {
            switch (type) 
            {
                case "1":
                    return "风控冻结";
                case "2":
                    return "拍拍冻结";
                case "3":
                    return "用户冻结";
                case "4":
                    return "商户冻结";
                case "5":
                    return "BG接口冻结";
                case "6":
                    return "涉嫌可疑交易冻结";
                default:
                    return "无冻结渠道";
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

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				string UserName = ViewState["UserName"].ToString();
				string UserIP   = Request.UserHostAddress;
				string FreezeReason = classLibrary.setConfig.replaceMStr(txtFFreezeReason.Text.Trim());

				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				qs.UpdateFreezeListDetail(lblfid.Text.Trim(),FreezeReason,UserName,UserIP);
				WebUtils.ShowMessage(this.Page,"保存成功！");
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"保存失败！" + ex.Message);
			}
		}
	}
}
