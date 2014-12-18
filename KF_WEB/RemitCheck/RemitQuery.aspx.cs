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
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.RemitCheck
{
	/// <summary>
	/// RemitQuery ��ժҪ˵����
	/// </summary>
	public partial class RemitQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//			try
			//			{
			//				string sr = Session["key"].ToString();
			//				if (!AllUserRight.GetOneRightState("AgentFeeManage",sr)) Response.Redirect("../login.aspx?wh=1");
			//
			//			}
			//			catch
			//			{
			//				Response.Redirect("../login.aspx?wh=1");
			//			}

			try
			{
				//Label1.Text = Session["uid"].ToString();
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
				BindSpid();
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
			labErrMsg.Text = "";	
			
			try
			{
				pager.RecordCount= GetCount(); 
				BindData(1);
			}
			catch(Exception ex)
			{
				string msg=ex.Message.Replace("\'","\\\'");
				WebUtils.ShowMessage(this.Page,msg);
			}
		}

		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}

		
		private void BindSpid()
		{
			Agent_Service.Agent_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Agent_Service.Agent_Service();
			string[] spids = null;
			try
			{
				spids=qs.GetRemitSpid();
			}
			catch
			{
				ShowMsg("spidδ���ã�");
				return;
			}
			if(spids!=null)
			{
				this.ddlSpid.Items.Clear();
				foreach(string spid in spids)
				{
					if(spid.Length==10)
					{
						ddlSpid.Items.Add(spid);
					}
				}
			}
		}

		private void BindData(int index)
		{
			string tranType = this.ddlTrantype.SelectedValue;
			string dataType = this.ddlDataType.SelectedValue;
			string remitType = this.ddlRemitType.SelectedValue;
			string tranState = this.ddlTranState.SelectedValue;
			string spid=ddlSpid.SelectedValue;
			string remitRec = this.tbRemitRec.Text.Trim();
			string listID=tblistID.Text.Trim();
			
			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Agent_Service.Agent_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Agent_Service.Agent_Service();

			DataSet ds = qs.GetRemitDataList("",tranType,dataType,remitType,tranState,spid,remitRec,listID, start,max);

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("FremitfeeName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fremit_fee","FremitfeeName");

				ds.Tables[0].Columns.Add("FremitpayfeeName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fremit_pay_fee","FremitpayfeeName");

				ds.Tables[0].Columns.Add("FprocedureName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fprocedure","FprocedureName");

				ds.Tables[0].Columns.Add("FotherprocedureName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fother_procedure","FotherprocedureName");

				ds.Tables[0].Columns.Add("FtranstateName",typeof(String));
				ds.Tables[0].Columns.Add("FtrantypeName",typeof(String));
				ds.Tables[0].Columns.Add("FremittypeName",typeof(String));
				ds.Tables[0].Columns.Add("FdatatypeName",typeof(String));
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();

					string tmp = dr["Ftran_type"].ToString();
					if(tmp == "1") 
						tmp = "���";
					else if(tmp == "2") 
						tmp = "�˻�";
					else if(tmp == "3") 
						tmp = "�Ļ�";
					else if(tmp == "4") 
						tmp = "�����˻�";
					else 
						tmp="״̬δ֪:"+tmp;
					dr["FtrantypeName"] = tmp;


					tmp = dr["Ftran_state"].ToString();
					if(tmp == "1") 
						tmp = "�ɹ�";
					else if(tmp == "2") 
						tmp = "ʧ��";
					else if(tmp == "3") 
						tmp = "����";
					else if(tmp == "4") 
						tmp = "����ɹ�";
					else if(tmp == "5") 
						tmp = "����ʧ��";
					else 
						tmp = "״̬δ֪:"+tmp;
					dr["FtranstateName"] = tmp;


					tmp = dr["Fremit_type"].ToString();
					if(tmp == "1") 
						tmp = "����ַ���";
					else if(tmp == "2") 
						tmp = "��������";
					else if(tmp == "3") 
						tmp = "���˻��";
					else 
						tmp = "״̬δ֪:"+tmp;
					dr["FremittypeName"] = tmp;


					tmp = dr["Fdata_type"].ToString();
					if(tmp == "1") 
						tmp = "���ֱ�ӳɹ�";
					else if(tmp == "2") 
						tmp = "������";
					else if(tmp == "3") 
						tmp = "���ʧ��";
					else if(tmp == "4") 
						tmp = "�������ɹ�";
					else if(tmp=="5")
						tmp="�������ʧ��";
					else if(tmp == "6") 
						tmp = "�˻�ɹ�";
					else if(tmp == "7") 
						tmp = "�˻����";
					else if(tmp == "8") 
						tmp = "�˻�ʧ��";
					else if(tmp=="9")
						tmp="�˻�����ɹ�";
					else if(tmp == "10") 
						tmp = "�˻�����ʧ��";
					else if(tmp == "11") 
						tmp = "�Ļ�ɹ�";
					else if(tmp == "12") 
						tmp = "�Ļ����";
					else if(tmp=="13")
						tmp="�Ļ�ʧ��";
					else if(tmp=="14")
						tmp="�Ļ����ɹ�";
					else if(tmp=="15")
						tmp="�Ļ����ʧ��";
					else if(tmp=="16")
						tmp="�����˻�";
					else if(tmp=="21")
						tmp="�ʴ������˻�";
					else
						tmp = "״̬δ֪:"+tmp;
					dr["FdatatypeName"] = tmp;


					dr.EndEdit();
				}

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"û���ҵ���¼");
			}
		}

		
		
		private int GetCount()
		{
			string tranType = this.ddlTrantype.SelectedValue;
			string dataType = this.ddlDataType.SelectedValue;
			string remitType = this.ddlRemitType.SelectedValue;
			string tranState = this.ddlTranState.SelectedValue;
			string spid=ddlSpid.SelectedValue;
			string remitRec = this.tbRemitRec.Text.Trim();
			string listID=tblistID.Text.Trim();
			
			Agent_Service.Agent_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Agent_Service.Agent_Service();
			return qs.GetRemitListCount("",tranType,dataType,remitType,tranState,spid,remitRec,listID);
		}


		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(e.NewPageIndex);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceHtmlStr(eSys.Message));
			}
		}
	}
}
