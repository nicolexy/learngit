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
using CFT.CSOMS.BLL.TransferMeaning;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// ChildrenQuery ��ժҪ˵����
	/// </summary>
	public partial class ChildrenQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		public string iFramePath;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			BindData();
		}

		private void BindData()
		{
			try
			{
				//"Fcurtype" == "1" ��ͨ
				//"Fcurtype" == "2" ����ȯ
				//"Fcurtype" == "80" ��Ϸ
				//"Fcurtype" == "81" ��������
				//"Fcurtype" == "82" ֱͨ��
				this.PanelInfo.Visible = false;

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				this.lblCommonQQ.Text = this.tbQQ.Text.Trim();
				this.lblGameQQ.Text = this.tbQQ.Text.Trim();
				this.lblFLQQ.Text = this.tbQQ.Text.Trim();
				this.lblZTCQQ.Text = this.tbQQ.Text.Trim();

				this.lblCommonMoney.Text = "";
				this.lblCommonType.Text = "";

				this.lblFLMoney.Text = "";
				this.lblFLType.Text = "";
				this.btnFL.Enabled = false;
				this.btnFLState.Enabled = false;

				this.lblGameMoney.Text = "";
				this.lblGameType.Text = "";
				this.btnGame.Enabled = false;
				this.btnGameState.Enabled = false;


				this.lblZTCMoney.Text = "";
				this.lblZTCType.Text = "";
				this.btnZTC.Enabled = false;
				this.btnZTCState.Enabled = false;
				
				string ErrorMsg = "";
				try
				{
					DataSet ds1 = qs.GetChildrenInfo(this.tbQQ.Text.Trim(),"1");
					if(ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
					{
						this.lblCommonMoney.Text = classLibrary.setConfig.FenToYuan(ds1.Tables[0].Rows[0]["Fbalance"].ToString());
                        this.lblCommonType.Text = Transfer.accountState(ds1.Tables[0].Rows[0]["Fstate"].ToString());
					}
				}
				catch(Exception ex)
				{
					ErrorMsg += "���˻���ѯʧ�ܣ�" + ex.Message;
				}

				try
				{
					DataSet ds80 = qs.GetChildrenInfo(this.tbQQ.Text.Trim(),"80");
					if(ds80 != null && ds80.Tables.Count > 0 && ds80.Tables[0].Rows.Count > 0)
					{
						this.lblGameMoney.Text = classLibrary.setConfig.FenToYuan(ds80.Tables[0].Rows[0]["Fbalance"].ToString());
						string Fstate = ds80.Tables[0].Rows[0]["Fstate"].ToString();
                        this.lblGameType.Text = Transfer.accountState(Fstate);
						this.btnGame.Enabled = true;
						if(Fstate == "1")
						{
							this.btnGameState.Text = "����";
							this.btnGameState.Enabled = true;
						}
						else if(Fstate == "2")
						{
							this.btnGameState.Text = "�ⶳ";
							this.btnGameState.Enabled = true;
						}
					}
				}
				catch(Exception ex)
				{
					ErrorMsg += "��Ϸ�˻���ѯʧ�ܣ�" + ex.Message;
				}
				
				try
				{
					DataSet ds81 = qs.GetChildrenInfo(this.tbQQ.Text.Trim(),"81");
					if(ds81 != null && ds81.Tables.Count > 0 && ds81.Tables[0].Rows.Count > 0)
					{
						this.lblFLMoney.Text = ds81.Tables[0].Rows[0]["Fbalance"].ToString() + "����";
						string Fstate = ds81.Tables[0].Rows[0]["Fstate"].ToString();
                        this.lblFLType.Text = Transfer.accountState(Fstate);
						this.btnFL.Enabled = true;
						if(Fstate == "1")
						{
							this.btnFLState.Text = "����";
							this.btnFLState.Enabled = true;
						}
						else if(Fstate == "2")
						{
							this.btnFLState.Text = "�ⶳ";
							this.btnFLState.Enabled = true;
						}
					}
				}
				catch(Exception ex)
				{
					ErrorMsg += "�����˻���ѯʧ�ܣ�" + ex.Message;
				}

				try
				{
					DataSet ds82 = qs.GetChildrenInfo(this.tbQQ.Text.Trim(),"82");
					if(ds82 != null && ds82.Tables.Count > 0 && ds82.Tables[0].Rows.Count > 0)
					{
						this.lblZTCMoney.Text = classLibrary.setConfig.FenToYuan(ds82.Tables[0].Rows[0]["Fbalance"].ToString());
						string Fstate = ds82.Tables[0].Rows[0]["Fstate"].ToString();
                        this.lblZTCType.Text = Transfer.accountState(Fstate);
						this.btnZTC.Enabled = true;
						if(Fstate == "1")
						{
							this.btnZTCState.Text = "����";
							this.btnZTCState.Enabled = true;
						}
						else if(Fstate == "2")
						{
							this.btnZTCState.Text = "�ⶳ";
							this.btnZTCState.Enabled = true;
						}
					}
				}
				catch(Exception ex)
				{
					ErrorMsg += "ֱͨ���˻���ѯʧ�ܣ�" + ex.Message;
				}

				if(ErrorMsg != "")
				{
					WebUtils.ShowMessage(this.Page,ErrorMsg);
				}

			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,eSys.Message);
			}
		}

		protected void btnGame_Click(object sender, System.EventArgs e)
		{
			//"Fcurtype" == "1" ��ͨ
			//"Fcurtype" == "2" ����ȯ
			//"Fcurtype" == "80" ��Ϸ
			//"Fcurtype" == "81" ��������
			//"Fcurtype" == "82" ֱͨ��
			this.PanelInfo.Visible = true;
			Session["QQID"] = this.lblGameQQ.Text;
			iFramePath = "bankrollLog.aspx?type=QQID&Fcurtype=80";
		}

		protected void btnFL_Click(object sender, System.EventArgs e)
		{
			this.PanelInfo.Visible = true;
			Session["QQID"] = this.lblFLQQ.Text;
			iFramePath = "bankrollLog.aspx?type=QQID&Fcurtype=81";
		}

		protected void btnZTC_Click(object sender, System.EventArgs e)
		{
			this.PanelInfo.Visible = true;
			Session["QQID"] = this.lblZTCQQ.Text;
			iFramePath = "bankrollLog.aspx?type=QQID&Fcurtype=82";
		}

		protected void btnGameState_Click(object sender, System.EventArgs e)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				if(this.btnGameState.Text == "����")
				{
					qs.ChildrenFreezeOrUnfreeze(this.lblGameQQ.Text,"80","2");
					WebUtils.ShowMessage(this.Page,"����ɹ���");
				}
				else if(this.btnGameState.Text == "�ⶳ")
				{
					qs.ChildrenFreezeOrUnfreeze(this.lblGameQQ.Text,"80","1");
					WebUtils.ShowMessage(this.Page,"�ⶳ�ɹ���");
				}
				this.btnGameState.Enabled = false;
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,eSys.Message);
			}
		}

		protected void btnFLState_Click(object sender, System.EventArgs e)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				if(this.btnFLState.Text == "����")
				{
					qs.ChildrenFreezeOrUnfreeze(this.lblGameQQ.Text,"81","2");
					WebUtils.ShowMessage(this.Page,"����ɹ���");
				}
				else if(this.btnFLState.Text == "�ⶳ")
				{
					qs.ChildrenFreezeOrUnfreeze(this.lblGameQQ.Text,"81","1");
					WebUtils.ShowMessage(this.Page,"�ⶳ�ɹ���");
				}
				this.btnFLState.Enabled = false;
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,eSys.Message);
			}
		}

		protected void btnZTCState_Click(object sender, System.EventArgs e)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				if(this.btnZTCState.Text == "����")
				{
					qs.ChildrenFreezeOrUnfreeze(this.lblGameQQ.Text,"82","2");
					WebUtils.ShowMessage(this.Page,"����ɹ���");
				}
				else if(this.btnZTCState.Text == "�ⶳ")
				{
					qs.ChildrenFreezeOrUnfreeze(this.lblGameQQ.Text,"82","1");
					WebUtils.ShowMessage(this.Page,"�ⶳ�ɹ���");
				}
				this.btnFLState.Enabled = false;
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,eSys.Message);
			}
		}


	}
}
