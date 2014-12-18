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
	/// CFTUserPickTJ ��ժҪ˵����
	/// </summary>
	public partial class CFTUserPickTJ : System.Web.UI.Page
	{
	
		public string pagebegindate
		{
			get{return ViewState["begindate"].ToString();}
		}

		public string pageenddate
		{
			get{return ViewState["enddate"].ToString();}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPickTJ")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("CFTUserPickTJ",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				TextBoxBeginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy��MM��dd��");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");

				DateTime begindate;
				DateTime enddate;

				begindate = DateTime.Parse(TextBoxBeginDate.Text);
				enddate = DateTime.Parse(TextBoxEndDate.Text);

				ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
				ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

				Table2.Visible = false;				
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

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;

			try
			{
				begindate = DateTime.Parse(TextBoxBeginDate.Text);
				enddate = DateTime.Parse(TextBoxEndDate.Text);
			}
			catch
			{
				throw new Exception("������������");
			}

			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
			}

			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
		}


		public string GetUrl(int state, string user)
		{
			string stmp = "CftAppealQuery.aspx?state=" + state + "&begin=" + pagebegindate.Substring(0,10) 
				+ "&end=" + pageenddate.Substring(0,10) + "&user=" + user;
			return stmp;
		}

		private void BindData()
		{/* ҳ���ѱ��ϳ� begin
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			Finance_Header fh = new Finance_Header();
			fh.UserIP = Request.UserHostAddress;
			fh.UserName = Session["uid"].ToString();

			fh.OperID = Int32.Parse(Session["OperID"].ToString());
			fh.SzKey = Session["SzKey"].ToString();
			//fh.RightString = Session["key"].ToString();

			qs.Finance_HeaderValue = fh;

			string msg = "";
			DataSet ds = qs.GetCFTUserPickTJ(begindate,enddate,out msg);

			if(ds != null && ds.Tables.Count >0)
			{
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��" + msg);
			}

			//�ٲ�ѯһ�¸�ͳ����Ϣ.
			string[] countlist = qs.GetAppealSum(begindate,enddate);
			if(countlist != null || countlist.Length == 3)
			{
				labSumCount.Text = countlist[0].Trim();
				labHandleCount.Text = countlist[1].Trim();
				labothercount.Text = countlist[2].Trim();
			}
			 ҳ���ѱ��ϳ� end */
		}

		protected void btnTJ_Click(object sender, System.EventArgs e)
		{
			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = Common.AllUserRight.GetServiceID("CFTUserPickTJ") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "ִ����[��������ͳ��]����,��������[" + " "
					+ "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				string log = SensitivePowerOperaLib.MakeLog("get",struserdata,"[��������ͳ��]","");

				if(!SensitivePowerOperaLib.WriteOperationRecord("CFTUserPickTJ",log,this))
				{
					
				}

				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				Table2.Visible = true;
				BindData();
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
			}
		}
	}
}
