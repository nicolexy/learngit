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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Mail;
using System.IO;
using CFT.CSOMS.BLL.SPOA;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// SelfQueryDetail 的摘要说明。
	/// </summary>
	public partial class SelfQueryDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected TENCENT.OSS.CFT.KF.KF_Web.Control.FunctionControl NewFunction;
   
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"DrawAndApprove")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("DrawAndApprove",this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
                   
					string id = Request.QueryString["id"];
                    string SPID = Request.QueryString["SPID"];
                    if (id == null || id == "")
						Response.Redirect("../login.aspx?wh=1");
					else
					{
						ViewState["ApplyCpInfoID"] = id;
                        ViewState["SPID"] = SPID;
						ShowInfo(1);
					}
				}
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
		}

        /// <summary>
        /// 合作形式列表
        /// </summary>
        private Hashtable getSpidTypeList()
        {
            Hashtable htSpidList = new Hashtable();
            try
            {
                DataSet dsSpidList = new SPOAService().GetBDSpidList();
                if (dsSpidList != null && dsSpidList.Tables.Count > 0 && dsSpidList.Tables[0].Rows.Count > 0)
                {
                    if (htSpidList.Count == 0)
                    {
                        foreach (DataRow row in dsSpidList.Tables[0].Rows)
                            htSpidList.Add(row["value"].ToString(), row["text"].ToString());
                    }
                }
                return htSpidList;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "合作形式列表查询异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return htSpidList;
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
            this.dgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgList_PageIndexChanged);
        }
		#endregion

        private void dgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            this.dgList.CurrentPageIndex = e.NewPageIndex;
            ShowInfo(e.NewPageIndex);
        }
        private void ShowInfo(int index)
		{
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.GetSelfQueryInfo(ViewState["ApplyCpInfoID"].ToString());
                this.CompanyName_value.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                this.CompanyAddress_value.Text = ds.Tables[0].Rows[0]["CompanyAddress"].ToString();
                this.WWWAdress_value.Text = ds.Tables[0].Rows[0]["WWWAdress"].ToString();
                this.Postalcode_value.Text = ds.Tables[0].Rows[0]["Postalcode"].ToString();
                this.TradeType_value.Text = ds.Tables[0].Rows[0]["TradeTypeStr"].ToString();
                this.SuggestUser_value.Text = ds.Tables[0].Rows[0]["SuggestUser"].ToString();
                this.AreaID_value.Text = ds.Tables[0].Rows[0]["AreaIDStr"].ToString();
                this.ContactUser_value.Text = ds.Tables[0].Rows[0]["ContactUser"].ToString();
                this.BDID_value.Text = ds.Tables[0].Rows[0]["BDIDStr"].ToString();
                this.ContactPhone_value.Text = ds.Tables[0].Rows[0]["ContactPhone"].ToString();
                this.ContactEmail_value.Text = ds.Tables[0].Rows[0]["ContactEmail"].ToString();
                this.ContactMobile_value.Text = ds.Tables[0].Rows[0]["ContactMobile"].ToString();
                this.ContactQQ_value.Text = ds.Tables[0].Rows[0]["ContactQQ"].ToString();
                this.UserType_value.Text = ds.Tables[0].Rows[0]["UserTypeStr"].ToString();
                //echo 20141020 身份证和银行帐号需展示前3位和后5位
                this.IdentityCardNum_value.Text = classLibrary.setConfig.ConvertID(ds.Tables[0].Rows[0]["IdentityCardNum"].ToString(), 3, 5);
                this.BankUserName_value.Text = ds.Tables[0].Rows[0]["BankUserName"].ToString();
                this.AreaCode_value.Text = ds.Tables[0].Rows[0]["AreaCodeStr"].ToString();
                this.BankType_value.Text = ds.Tables[0].Rows[0]["BankTypeStr"].ToString();
                this.CityCode_value.Text = ds.Tables[0].Rows[0]["CityCodeStr"].ToString();
                this.BankName_value.Text = ds.Tables[0].Rows[0]["BankName"].ToString();
                this.BankAccounts_value.Text = classLibrary.setConfig.ConvertID(ds.Tables[0].Rows[0]["BankAccounts"].ToString(), 3,5);
                this.JSMemo_value.Text = ds.Tables[0].Rows[0]["JSMemo"].ToString();
                this.OtherMemo_value.Text = ds.Tables[0].Rows[0]["OtherMemo"].ToString();
                this.ErrorMemo_value.Text = ds.Tables[0].Rows[0]["ErrorMemo"].ToString();
                this.NewFunction.SetFunCheck(ds.Tables[0].Rows[0]["FunctionStr"].ToString());
                //echo 20141021上级商户
                this.txtSuperiorSpid.Text = ds.Tables[0].Rows[0]["SuperiorSpid"].ToString();
                //lxl 20140703
               // DataSet ds1 = new SPOAService().GetSpInfo(" and SPID='" + ViewState["SPID"].ToString() + "'", 1, 0);
                DataSet ds1 = new SPOAService().GetSpInfo(ViewState["SPID"].ToString(), "", "", "", "", "", 1, 0);
                
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    string spidType = ds1.Tables[0].Rows[0]["BDSpidType"].ToString();               
                    Hashtable htSpidList = getSpidTypeList();
                    if (htSpidList.Contains(spidType))
                        this.txtBDSpidType.Text = htSpidList[spidType].ToString();
                   // this.txtBDSpidType.Text = ds1.Tables[0].Rows[0]["BDSpidType"].ToString();
                    this.txtInType.Text = ds1.Tables[0].Rows[0]["InType"].ToString();
                    this.txtAPPID.Text = ds1.Tables[0].Rows[0]["APPID"].ToString();
                    this.txtIsWXBeil.Text = ds1.Tables[0].Rows[0]["IsWXBeilName"].ToString();
                    this.txtWXBeil.Text = ds1.Tables[0].Rows[0]["WXBeil"].ToString();

                    this.txtChangelessBeil.Text = ds1.Tables[0].Rows[0]["ChangelessBeil"].ToString();
                    this.txtChangelessBeilType.Text = ds1.Tables[0].Rows[0]["ChangelessBeilType"].ToString();
                    this.txtQQID.Text = ds1.Tables[0].Rows[0]["QQID"].ToString();
                    this.txtSPID.Text = ds1.Tables[0].Rows[0]["SPID"].ToString();
                }

                // checktype有以下类型：
                //0 商户申请审批
                //1 费率申请审批
                //在本查询中固定  checktype =0
                //DataSet dsCheckInfo = new SPOAService().GetCheckInfo(ViewState["SPID"].ToString(), 0);
                //获取审批信息传主键 14/8/19 yinhuang
                DataSet dsCheckInfo = new SPOAService().GetCheckInfo(ViewState["ApplyCpInfoID"].ToString(), 0);

                if (dsCheckInfo != null && dsCheckInfo.Tables.Count > 0 && dsCheckInfo.Tables[0].Rows.Count > 0)
                {
                    dgList.DataSource = dsCheckInfo.Tables[0].DefaultView;
                    dgList.DataBind();
                }
                else
                {
                    dgList.DataSource = null;
                    dgList.DataBind();
                }

                if (Request.QueryString["mode"] == "approve")
                {
                    this.btnApprove.Visible = true;
                    this.btnReject.Visible = true;

                    if (ds.Tables[0].Rows[0]["Flag"].ToString() == "-1" && ds.Tables[0].Rows[0]["KFCheckUser"].ToString() == Session["uid"].ToString())
                    {
                        this.btnApprove.Enabled = true;
                        this.btnReject.Enabled = true;
                    }
                    else
                    {
                        this.btnApprove.Enabled = false;
                        this.btnReject.Enabled = false;
                    }
                }
                else
                {
                    this.btnApprove.Visible = false;
                    this.btnReject.Visible = false;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
		}

		protected void btnApprove_Click(object sender, System.EventArgs e)
		{
			ApproveORReject(true);
		}

		protected void btnReject_Click(object sender, System.EventArgs e)
		{
			if(this.ErrorMemo_value.Text.Trim() == "")
				WebUtils.ShowMessage(this.Page,"请输入作废备注！");
			else
			{
				ApproveORReject(false);
			}
		}

		private void ApproveORReject(bool Result)
		{
			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = TENCENT.OSS.CFT.KF.Common.AllUserRight.GetServiceID("DrawAndApprove") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "执行了[自助商户审批]操作,操作对象[" + this.CompanyName_value.Text
					+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				string log = classLibrary.SensitivePowerOperaLib.MakeLog("edit",struserdata,"[自助商户审批]",ViewState["ApplyCpInfoID"].ToString(),
					Session["uid"].ToString().Trim(),Result.ToString(),this.ErrorMemo_value.Text.Trim());

				if(!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("DrawAndApprove",log,this))
				{
					
				}
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				qs.ApproveTicket(ViewState["ApplyCpInfoID"].ToString(),Session["uid"].ToString().Trim(),Result,this.ErrorMemo_value.Text.Trim());

				if(Result)
					WebUtils.ShowMessage(this.Page,"审核同意成功！");
				else
				{
					SendEmail(this.ContactEmail_value.Text.Trim());
					WebUtils.ShowMessage(this.Page,"拒绝成功！");
				}

				ShowInfo(1);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

		private void SendEmail(string email)
		{
			if(PublicRes.IgnoreLimitCheck)
				return ;

			try
			{
				TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail=new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
				newMail.SendMail(email,"","您的自助商户申请被拒绝",GetOutMailContent(),true,null);
			}
			catch(Exception err)
			{
				throw new Exception(err.Message);
			}			
		}

		private string GetOutMailContent()
		{
			string pattern = "";
			StreamReader sr = new StreamReader(Server.MapPath(Request.ApplicationPath).Trim().ToLower() 
				+ "\\Email\\SelfBusinessReject.htm",System.Text.Encoding.GetEncoding("GB2312"));
			try
			{
				pattern = sr.ReadToEnd();
			}
			finally
			{
				sr.Close();
			}
			pattern = String.Format(pattern,this.CompanyName_value.Text,this.ErrorMemo_value.Text.Trim());
			return pattern;
		}

	}
}
