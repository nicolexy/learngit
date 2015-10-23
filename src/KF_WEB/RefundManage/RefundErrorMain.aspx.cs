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

using TENCENT.OSS.C2C.Finance;
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// RefundErrorMain ��ժҪ˵����
	/// </summary>
	public partial class RefundErrorMain : System.Web.UI.Page
	{

		private string WorkType = "";
		private string WeekIndex = "0";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			// �ڴ˴������û������Գ�ʼ��ҳ��
			if (!IsPostBack)
			{           
				WeekIndex = Request.QueryString["WeekIndex"];
				try
				{
					DateTime dt = DateTime.Parse(WeekIndex);
				}
				catch
				{
					try
					{
						WeekIndex = DateTime.Parse(PublicRes.strNowTime).AddDays(-1).ToString("yyyy��MM��dd��");	
					}
					catch
					{
						WeekIndex = DateTime.Now.AddDays(-1).ToString("yyyy��MM��dd��");
					}

					if(WeekIndex == null || WeekIndex.Trim() == "")
						WeekIndex = DateTime.Now.AddDays(-1).ToString("yyyy��MM��dd��");
				} 
   
				ViewState["WorkType"] = WorkType;
				TextBoxBeginDate.Text = WeekIndex;
				TextBoxEndDate.Text = WeekIndex;

				classLibrary.setConfig.GetAllBankList(ddlBankType);
				ddlBankType.Items.Insert(0,new ListItem("��������","0000"));

				SetViewState();
			}
			else
			{
				WorkType = ViewState["WorkType"].ToString();
				WeekIndex = TextBoxBeginDate.Text.Trim();
			}
			//Ȩ���ж�
			try
			{
				//��һ�ν�������ȡ��Ҫ���еĹ������ͺ���һ�ܡ�
				WorkType = Request.QueryString["WorkType"];
				if(WorkType == null) WorkType = "batpay";

				try
				{
					this.Label_uid.Text = Session["uid"].ToString();
					//string sr = Session["key"].ToString();					
					
					if(WorkType == "task")
					{
						lbTitle.Text = "�쳣�˵�����";
						//this.Label1.Text = Session["uid"].ToString();
						string szkey = Session["SzKey"].ToString();
						int operid = Int32.Parse(Session["OperID"].ToString());
							
						// if(!AllUserRight.GetOneRightState("IDCCheck",sr)) Response.Redirect("../login.aspx?wh=1");
						//if(!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
						if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
						{
							Response.Redirect("../login.aspx?wh=1");
						}
					}		
				}
				catch  //���û�е�½����û��Ȩ�޾�����
				{
					Response.Redirect("../login.aspx?wh=1");
				}

				this.Label_uid.Text = Session["uid"].ToString();  
				//�������ͣ����ڣ�����ȫȷ���ˣ��Ϳ�����ȡ�������ˡ�
                
				InitGrid(this.TextBoxBeginDate.Text.Trim(),this.TextBoxEndDate.Text.Trim(),this.ddlBankType.SelectedValue,int.Parse(this.ddlState.SelectedValue),this.txtProposer.Text.Trim(),this.ddlRefundPath.SelectedValue);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"��ʾ����ʱ���������ԡ�"+PublicRes.GetErrorMsg(ex.Message));
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
			try
			{
				try
				{
					DateTime.Parse(TextBoxBeginDate.Text.Trim());
					DateTime.Parse(TextBoxEndDate.Text.Trim());
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"ѡ������ڷǷ���");
					return;
				}

				InitGrid(TextBoxBeginDate.Text.Trim(), TextBoxEndDate.Text.Trim(),ddlBankType.SelectedValue,int.Parse(ddlState.SelectedValue),this.txtProposer.Text,this.ddlRefundPath.SelectedValue);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,PublicRes.GetErrorMsg(ex.Message));
			}
		}


		private void SetViewState()
		{
			if(Request.QueryString["BeginDate"]!=null)
			{
				this.TextBoxBeginDate.Text=Request.QueryString["BeginDate"].ToString();
			}
			else
			{
				this.TextBoxBeginDate.Text=System.DateTime.Now.AddDays(-1).ToString("yyyy��MM��dd��");
			}

			if(Request.QueryString["EndDate"]!=null)
			{
				this.TextBoxEndDate.Text=Request.QueryString["EndDate"].ToString();
			}
			else
			{
				this.TextBoxEndDate.Text=System.DateTime.Now.AddDays(-1).ToString("yyyy��MM��dd��");
			}

			if(Request.QueryString["BankType"]!=null)
			{
				this.ddlBankType.SelectedValue=Request.QueryString["BankType"].ToString();
			}
			else
			{
				this.ddlBankType.SelectedValue="0000";
			}

			if(Request.QueryString["BatchState"]!=null)
			{
				this.ddlState.SelectedValue=Request.QueryString["BatchState"].ToString();
			}
			else
			{
				this.ddlState.SelectedValue="9999";
			}

			if(Request.QueryString["Proposer"]!=null)
			{
				this.txtProposer.Text=Request.QueryString["Proposer"].ToString();
			}
			else
			{
				this.txtProposer.Text="";
			}
			if(Request.QueryString["RefundPath"]!=null)
			{
				this.ddlRefundPath.SelectedValue=Request.QueryString["RefundPath"].ToString();
			}
			else
			{
				this.ddlRefundPath.SelectedValue="9999";
			}
		}

		public static string GetBankName(string strBankID)
		{
			//���ٴ����ݿ���ȡ,��������
			if(strBankID == "9999")
				return "��������";
			else
                return Transfer.convertbankType(strBankID);
		}

		public static void GetAllBankList(DropDownList ddl)
		{
			GetAllPayBankList(ddl);
			ddl.Items.Add(new ListItem("��������","9999"));
		}

		public static void GetAllPayBankList(DropDownList ddl)
		{
			ddl.Items.Clear();

			//			string allrefundbank = System.Configuration.ConfigurationManager.AppSettings["RefundFileBankType"].Trim();
			//string allrefundbank = BankLib.ZWDicClass.GetZWDicValue("RefundFileBankType",PublicRes.GetConnString("ZW"));
			string allrefundbank = "";
			string[] banklist = allrefundbank.Split('|');
            
			if(banklist.Length == 0)
				return;

			foreach(string strbank in banklist)
			{
				string strtmp = strbank.Trim();
				if(strtmp.Length >= 4)
				{
					strtmp = strtmp.Substring(0,4);
                    string bankname = Transfer.convertbankType(strtmp);
					ddl.Items.Add(new ListItem(bankname,strtmp));
				}
			}
		}

		private void InitGrid(string beginDate,string endDate,string banktype,int status,string proposer,string refundPath)
		{
			DataTable dt;

			BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.CFT.KF.KF_Web.BatchPay_Service.BatchPay_Service();
			dt = bs.RefundOther_InitGrid_R(beginDate,endDate,banktype,status,proposer,refundPath).Tables[0];

			ViewState["BeginDate"]=beginDate;
			ViewState["EndDate"]=endDate;
			ViewState["BankType"]=banktype;
			ViewState["BatchState"]=status;
			ViewState["Proposer"]=proposer;
			ViewState["RefundPath"]=refundPath;

			if(dt == null)
			{
				//��ȡ���ݳ���
				//WebUtils.ShowMessage(this.Page,"��ȡ����ʱ���������ԡ�");
				DataGrid1.DataSource = null;
				DataGrid1.DataBind();
				return;
			}

			switch(WorkType)
			{
				case "task" :
					ShowTask(dt);
					break;
			}            
		}

		private void ShowTask(DataTable dt)
		{
			foreach(DataRow dr in dt.Rows)
			{
				dr.BeginEdit();
				dr["Furl2"]= "batchid=" + dr["FBatchID"].ToString()+"&onlyView=false";
				dr["FUrl"] = "batchid=" + dr["FBatchID"].ToString() + "&WorkType=" + WorkType + "&WeekIndex=" + WeekIndex
					+"&BeginDate="+ViewState["BeginDate"].ToString()+"&EndDate="+ViewState["EndDate"].ToString()
					+"&BankType="+ViewState["BankType"].ToString()+"&BatchState="+ViewState["BatchState"].ToString()
					+"&Proposer="+ViewState["Proposer"].ToString()+"&RefundPath="+ViewState["RefundPath"].ToString();
				dr["FBankTypeName"] = GetBankName(dr["FBankType"].ToString());
				dr.EndEdit();
			}
		
			DataGrid1.DataSource = dt.DefaultView;
			DataGrid1.DataBind();
		}
	}
}
