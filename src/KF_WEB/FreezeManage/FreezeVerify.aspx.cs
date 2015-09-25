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
using System.IO;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.FreezeManage
{
	/// <summary>
	/// FreezeVerify1 的摘要说明。
	/// </summary>
	public partial class FreezeVerify1 : System.Web.UI.Page
	{

        public string uid;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            uid = Session["uid"] as string;
			if(!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
				ViewState["FID"] = Request.QueryString["fid"].ToString().Trim();
				ViewState["FFreezeID"] = Request.QueryString["ffreeze_id"].ToString().Trim();
				SetAllBtnVisible(false);

				BindData(1);

				string[] fastReplyBuff = getData.GetFreezeFastReplay(this,false);

				if(fastReplyBuff != null)
				{
					this.ddl_fastReply1.Items.Clear();
					foreach(string str in fastReplyBuff)
					{
						if(str != null && str.Trim().Length > 0)
						{
							this.ddl_fastReply1.Items.Add(str);
						}
						else
						{
							
						}
					}
				}
			}

			this.ddl_fastReply1.SelectedIndexChanged += new EventHandler(ddl_fastReply1_SelectedIndexChanged);
		}



		private void SetAllBtnVisible(bool canSee)
		{
			this.btn_Del.Visible = canSee;
			this.btn_Finish1.Visible = canSee;
			this.btn_Finish2.Visible = canSee;
			this.btn_hangUp.Visible = canSee;
			if(!canSee)
				this.btn_addRecord.Visible = canSee;
		}


		public void BindData(int iIndex)
		{
			SetAllBtnVisible(false);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

			this.tbx_payAccount.Text = ViewState["FFreezeID"].ToString();

			// 查询申诉相关资料，8为新定义的冻结申诉,state为0表示未处理的
			//DataSet ds2 = qs.GetCFTUserAppealList(ViewState["FFreezeID"].ToString(),"","",99,8,"",0,1);

			DataSet ds2 = qs.GetCFTUserAppealDetail(int.Parse(ViewState["FID"].ToString()));

			if(ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"查询用户申诉资料结果为空");
				return;
			}
			else
			{
				DataRow dr2 = ds2.Tables[0].Rows[0];

				string str = dr2["FState"].ToString();

				ViewState["Fstate"] = dr2["FState"].ToString();
				ViewState["isFreezeListHas"] = dr2["isFreezeListHas"].ToString();

				this.tbx_cerNO.Text = dr2["cre_id"].ToString();
				this.tbx_userSubBindMobile.Text = dr2["contact_no"].ToString();
				this.tbx_lastAddr.Text = dr2["rec_cftadd"].ToString();
				this.tbx_bindMobile.Text = dr2["mobile"].ToString();
				this.tbx_email.Text = dr2["FEmail"].ToString();
				this.tbx_phoneNo.Text = dr2["contact_no"].ToString();
				this.tbx_DC.Text = dr2["DC_timeaddress"].ToString();
				this.tbx_userQA.Text = dr2["reason"].ToString();

				string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();
				if(!url.EndsWith("/"))
					url += "/";

				string image_cre = dr2["cre_image"].ToString().Trim();
				string image_bank = dr2["prove_banlance_image"].ToString().Trim();
				string image_other1 = dr2["otherImage1"].ToString();
				string image_other2 = dr2["otherImage2"].ToString();
				string image_other3 = dr2["otherImage3"].ToString();
				string image_other4 = dr2["otherImage4"].ToString();
				string image_other5 = dr2["otherImage5"].ToString();

				this.Image1.ImageUrl = url + image_cre;
				this.Image2.ImageUrl = url + image_bank;

				if(image_other1.Trim() != "")
				{
					this.Image3.ImageUrl = url + image_other1;
					this.Image3.Visible = true;
				}

				if(image_other2.Trim() != "")
				{
					this.Image4.ImageUrl = url + image_other2;
					this.Image4.Visible = true;
				}

				if(image_other3.Trim() != "")
				{
					this.Image5.ImageUrl = url + image_other3;
					this.Image5.Visible = true;
					this.td_bpic1.Style.Add("display","block");
					this.table_bPics.Style.Add("display","block");
				}
				else
				{
					this.Image5.Visible = false;
					this.td_bpic1.Style.Add("display","none");
					this.table_bPics.Style.Add("display","none");
				}

				if(image_other4.Trim() != "")
				{
					this.Image6.ImageUrl = url + image_other4;
					this.Image6.Visible = true;
					this.td_bpic2.Style.Add("display","block");
					this.table_bPics.Style.Add("display","block");
				}
				else
				{
					this.Image6.Visible = false;
					this.td_bpic2.Style.Add("display","none");
					this.table_bPics.Style.Add("display","none");
				}

				if(image_other5.Trim() != "")
				{
					this.Image7.ImageUrl = url + image_other5;
					this.Image7.Visible = true;
					this.td_bpic3.Style.Add("display","block");
					this.table_bPics.Style.Add("display","block");
				}
				else
				{
					this.Image7.Visible = false;
					this.td_bpic3.Style.Add("display","none");
					this.table_bPics.Style.Add("display","none");
				}

				DataSet dsuser =  qs.GetAppealUserInfo(dr2["Fuin"].ToString());

				if(dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count == 0)
				{
					//return;
				}
				else
				{
					DataRow dr3 = dsuser.Tables[0].Rows[0];

					this.tbx_userName.Text =  PublicRes.GetString(dr3["Ftruename"]);
					this.tbx_regCreNO.Text = PublicRes.GetString(dr3["Fcreid"]);
					this.tbx_restFin.Text = classLibrary.setConfig.FenToYuan((double.Parse(dr3["FBalance"].ToString()) - double.Parse(dr3["Fcon"].ToString())).ToString());
				}
			}

			if(ViewState["isFreezeListHas"].ToString() == "1")
			{
				if(ViewState["Fstate"].ToString() == "1" || ViewState["Fstate"].ToString() == "2")
				{
					// 已结单
					this.btn_addRecord.Visible = true;
				}
				else if(ViewState["Fstate"].ToString() == "7")
				{
					// 已作废
				}
				else
				{
					// 挂起或者未处理
					SetAllBtnVisible(true);
				}
			}
			else
			{
				// 如果该用户不处于冻结状态，则只允许对这个申诉进行作废处理
				if(ViewState["Fstate"].ToString() != "7")
				{
					this.btn_Del.Visible = true;
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

		protected void btn_hangUp_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);

			try
			{
				if(qs.CreateFreezeDiary_2(ViewState["FID"].ToString(),8,Session["uid"].ToString()
					,handleResult,"",this.tbx_userName.Text.Trim(),this.tbx_email.Text.Trim()))
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

		protected void btn_Finish1_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);

			try
			{
				// 结单的话，将FSourceType设置为结单状态
				if(qs.CreateFreezeDiary_2(ViewState["FID"].ToString(),1,Session["uid"].ToString()
					,handleResult,"",this.tbx_userName.Text.Trim(),this.tbx_email.Text.Trim()))
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}


		protected void btn_Finish2_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);

			//qs.UpdateFreezeDiary(ViewState["QueryFID"].ToString(),"[结单(已解冻)]","[" + Session["uid"] + "]",fastReplay);

			try
			{
				// 结单的话，将FSourceType设置为结单状态
				if(qs.CreateFreezeDiary_2(ViewState["FID"].ToString(),2,Session["uid"].ToString(),handleResult,"",this.tbx_userName.Text.Trim(),this.tbx_email.Text.Trim()))
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}


		protected void btn_Del_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);

			try
			{
				// 作废的话，将FSourceType设置为作废状态
				if(qs.CreateFreezeDiary_2(ViewState["FID"].ToString(),7,Session["uid"].ToString(),handleResult,"",this.tbx_userName.Text.Trim(),this.tbx_email.Text.Trim()))
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}


		protected void btn_addRecord_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);

			try
			{
				if(qs.CreateFreezeDiary_2(ViewState["FID"].ToString(),100,Session["uid"].ToString()
					,handleResult,"",this.tbx_userName.Text.Trim(),this.tbx_email.Text.Trim()))
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

		private void ddl_fastReply1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string strFastReply = this.ddl_fastReply1.SelectedValue;

			this.tbx_handleResult.Text += (strFastReply);
		}

		protected void btn_manageFastReply_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("./FastReplyManagePage.aspx",false);
		}
	}
}
