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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// KFTotalQuery ��ժҪ˵����
	/// </summary>
	public partial class KFTotalQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPick")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				TextBoxBeginDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
				TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

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
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;

			try
			{
				begindate = DateTime.Parse(TextBoxBeginDate.Value);
                enddate = DateTime.Parse(TextBoxEndDate.Value);
			}
			catch
			{
				throw new Exception("������������");
			}

			if(this.tbUser.Text.Trim() == "")
			{
				throw new Exception("������ͷ���Ա��");
			}

			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
			}

			if(begindate.AddMonths(1) < enddate)
			{
				throw new Exception("���ڼ������һ���£����������룡");
			}
			
			ViewState["User"] = this.tbUser.Text.Trim();
			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
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
				pager.RecordCount= 10; 
				BindData(1);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
			}
		}
		
		
		private void BindData(int index)
		{
			string User = ViewState["User"].ToString();
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			DataSet ds = qs.GetKFTotalQueryList(User,begindate,enddate,this.ddlOperationType.SelectedValue,start,max);

			DataGrid1.DataSource = ds.Tables[0].DefaultView;
			DataGrid1.DataBind();
		}
		

	}
}
