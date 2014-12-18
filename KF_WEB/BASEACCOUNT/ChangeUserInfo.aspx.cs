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

using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// ChangeUserName ��ժҪ˵����
	/// </summary>
	public partial class ChangeUserName : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button Button2_Submit;
		protected System.Web.UI.WebControls.Button Button3_Cancel;

	

		public string iprov,icity;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			CheckInput();
			if (!Page.IsPostBack)
			{
				try
				{
					this.Label_uid.Text = Session["uid"].ToString();

					string szkey = Session["SzKey"].ToString();
					//int operid = Int32.Parse(Session["OperID"].ToString());

					//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"ChangeUserInfo")) Response.Redirect("../login.aspx?wh=1");
					 
					if(!classLibrary.ClassLib.ValidateRight("ChangeUserInfo",this)) Response.Redirect("../login.aspx?wh=1");
				}
				catch  //���û�е�½����û��Ȩ�޾�����
				{
					Response.Redirect("../login.aspx?wh=1");
				} 
				setInfoNull();
				initBasicInfo();
				
			}
			
			SetButtonVisible();
		}

		private void SetButtonVisible()
		{
			string szkey = Session["SzKey"].ToString();
			//int operid = Int32.Parse(Session["OperID"].ToString());
				
		}

		private void CheckInput()
		{
			#region			
			Button1.Attributes.Add("onclick", "return checkvlid(this);" );						
			#endregion
		}
		private void setInfoNull()
		{
			this.Label1_Fqqid.Text = "";
			this.TextBox2_Ftruename.Text = "";
			this.Textbox4_Company.Text = "";
			this.Textbox5_Fage.Text = "";
			this.Textbox6_Fphone.Text = "";
			this.Textbox7_Fmobile.Text = "";

			this.Textbox7_Femail.Text = "";
			this.Textbox10_Faddress.Text = "";
			this.Textbox11_Fpcode.Text = "";
			this.Textbox13_Fcreid.Text = "";
             
			this.TX_Memo.Text    = "";
			this.tbCrt.Text = "";
			this.TX_Fmodify_time.Text = "";

			this.DropDownList1_Sex.Visible = false;
			this.DropDownList2_certify.Visible = false;

		}

		private void BindInfo(int istr,int imax)
		{
			if(Session["uid"] == null)
			{
				Response.Redirect("../login.aspx?wh=1"); //���µ�½
			}

			Query_Service.Query_Service myService = new Query_Service.Query_Service();
			//myService.Finance_HeaderValue = classLibrary.setConfig.setFH(Session["uid"].ToString(),Request.UserHostAddress);
			myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);


			DataSet ds = myService.GetUserInfo(this.TX_QQID.Text.Trim(),istr,imax);
			if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1) 
			{
				throw new Exception("���ݿ��޴˼�¼");					
			}		
			//Response.Write("DS:" + ds.Tables[0].Rows[0][0].ToString());
			
			this.Label1_Fqqid.Text       = ds.Tables[0].Rows[0]["Fqqid"].ToString();
			this.TextBox2_Ftruename.Text = ds.Tables[0].Rows[0]["Ftruename"].ToString();
			this.DropDownList1_Sex.Visible = true;
			this.DropDownList1_Sex.SelectedValue  = ds.Tables[0].Rows[0]["Fsex"].ToString();
			this.Textbox4_Company.Text   = ds.Tables[0].Rows[0]["Fcompany_name"].ToString();
			this.Textbox5_Fage.Text      = ds.Tables[0].Rows[0]["Fage"].ToString();
			this.Textbox6_Fphone.Text    = ds.Tables[0].Rows[0]["Fphone"].ToString();
			this.Textbox7_Fmobile.Text   = ds.Tables[0].Rows[0]["Fmobile"].ToString();

			this.Textbox7_Femail.Text    = ds.Tables[0].Rows[0]["Femail"].ToString();
			this.Textbox10_Faddress.Text = ds.Tables[0].Rows[0]["Faddress"].ToString();
			this.Textbox11_Fpcode.Text   = ds.Tables[0].Rows[0]["Fpcode"].ToString();
			this.DropDownList2_certify.Visible = true;
			this.DropDownList2_certify.SelectedValue = ds.Tables[0].Rows[0]["Fcre_type"].ToString();
			this.Textbox13_Fcreid.Text   = classLibrary.setConfig.ConvertCreID(ds.Tables[0].Rows[0]["Fcreid"].ToString());

			this.TX_Memo.Text        = ds.Tables[0].Rows[0]["Fmemo"].ToString();
			this.TX_Fmodify_time.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();

			iprov = ds.Tables[0].Rows[0]["Farea"].ToString();
			icity = ds.Tables[0].Rows[0]["Fcity"].ToString();

			//��ȡһ��֤����Ϣ��furion 20080220
			this.tbCrt.Text = myService.GetUserCrtInfo(this.TX_QQID.Text.Trim());
		}

		private void BindLableInfo()
		{
			//sthis.TextBox1.Text ="";
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

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			//Response.Redirect("../TradeManage/Modify_Succ.aspx");
			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = Common.AllUserRight.GetServiceID("ChangeUserInfo") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "ִ����[��ѯQQ�ʻ�]����,��������[" + this.TX_QQID.Text.Trim()
					+ "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				string log = SensitivePowerOperaLib.MakeLog("get",struserdata,"[��ѯQQ�ʻ�]",this.TX_QQID.Text.Trim(),"1","1");

				if(!SensitivePowerOperaLib.WriteOperationRecord("ChangeUserInfo",log,this))
				{
					
				}

				BindInfo(1,1);
			}
			catch(SoapException eSoap)
			{
				setInfoNull(); //���ʧ�ܣ��������
				string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,str);
			}
			catch(Exception emsg)
			{
				WebUtils.ShowMessage(this.Page,emsg.Message);
			} 
		}

		private void initBasicInfo()
		{
			//�������ֵ��ж�ȡ���ݣ��󶨵�webҳ
			
			//��֤������
			setConfig.bindDic("CRE_TYPE",this.DropDownList2_certify); //��ѯ֤������
			
			//���Ա�
//			setConfig.bindDic("SEX",this.DropDownList1_Sex);//�Ա�
		}

		private void LinkButton1_Click(object sender, System.EventArgs e)
		{

			//һЩ��ʼֵ


			SetButtonVisible();

		}

	

		private void returnState()
		{
			//����֮����Ҫ�ָ�
			this.TextBox2_Ftruename.ReadOnly = true;
			this.Textbox4_Company.ReadOnly   = true; 
			this.Textbox5_Fage.ReadOnly      = true; 
			this.Textbox6_Fphone.ReadOnly    = true; 
			this.Textbox7_Fmobile.ReadOnly   = true; 
			this.Textbox7_Femail.ReadOnly    = true; 
			this.Textbox10_Faddress.ReadOnly = true; 
			this.Textbox11_Fpcode.ReadOnly   = true; 
			this.Textbox13_Fcreid.ReadOnly    = true; 
			this.TX_Memo.ReadOnly             = true; 
			this.tbCrt.ReadOnly = true;
			this.TX_Fmodify_time.ReadOnly     = false; 

			this.TextBox2_Ftruename.BorderWidth = 0;
			this.TextBox2_Ftruename.BackColor = Color.White;

			this.Textbox4_Company.BorderWidth = 0;
			this.Textbox4_Company.BackColor = Color.White;

			this.Textbox5_Fage.BorderWidth = 0;
			this.Textbox5_Fage.BackColor = Color.White;

			this.Textbox6_Fphone.BorderWidth = 0;
			this.Textbox6_Fphone.BackColor = Color.White;

			this.Textbox7_Fmobile.BorderWidth = 0;
			this.Textbox7_Fmobile.BackColor = Color.White;

			this.Textbox7_Femail.BorderWidth = 0;
			this.Textbox7_Femail.BackColor = Color.White;

			this.Textbox10_Faddress.BorderWidth = 0;
			this.Textbox10_Faddress.BackColor = Color.White;

			this.Textbox11_Fpcode.BorderWidth = 0;
			this.Textbox11_Fpcode.BackColor = Color.White;

			this.Textbox13_Fcreid.BorderWidth = 0;
			this.Textbox13_Fcreid.BackColor = Color.White;

			this.TX_Memo.BorderWidth =0;
			this.TX_Memo.BackColor = Color.White;

			this.tbCrt.BorderWidth = 0;
			this.tbCrt.BackColor = Color.White;
		}

		protected void Hcity_ServerChange(object sender, System.EventArgs e)
		{
//			 Hcity.Value
		}

		private void Button12_Click(object sender, System.EventArgs e)
		{
			Response.Write("ѡ���city:" + this.Hcity.Value);
			Response.Write("ѡ���area:" + this.Harea.Value);
		}

		private void Button3_Click(object sender, System.EventArgs e)
		{
			//��ť��ʾ�ĸ���

//			setInfoNull();
			returnState();

			SetButtonVisible();
		}

	}
}
