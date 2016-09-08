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
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// AppealDSettings ��ժҪ˵����
	/// </summary>
	public partial class AppealDSettings : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label_uid.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if (!IsPostBack)
			{
				pager.RecordCount= GetCount();
				BindData(1);
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

		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			pager.RecordCount= GetCount();
			BindData(1);
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(e.NewPageIndex);
			}
			catch(SoapException eSoap)
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message);
			}
		}

		private int GetCount()
		{
			string FSpid = this.tb_spid.Text.Trim();
			string FUser = this.tb_user.Text.Trim();
			int FPriType = int.Parse(this.ddl_pritype.SelectedValue.Trim());
			int FState   = int.Parse(this.ddl_state.SelectedValue.Trim());

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			int Count = qs.WS_AppealDQueryCount(FSpid, FUser, FPriType, FState);

			this.lb_msg.Text = "��¼������" + Count;

			return Count;
		}

		private void BindData(int index)
		{
			try
			{
				string FSpid = this.tb_spid.Text.Trim();
				string FUser = this.tb_user.Text.Trim();
				int FPriType = int.Parse(this.ddl_pritype.SelectedValue.Trim());
				int FState   = int.Parse(this.ddl_state.SelectedValue.Trim());

				int max = pager.PageSize;
				int start = max * (index-1) + 1;

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.WS_AppealDQuery(FSpid, FUser, FPriType, FState, start,max);
					
				DataColumn dc0 = new DataColumn("FPriTypeS",typeof(string));
				DataColumn dc1 = new DataColumn("FAmountSetS",typeof(string));		
				DataColumn dc2 = new DataColumn("FStateS",typeof(string));

				ds.Tables[0].Columns.Add(dc0);
				ds.Tables[0].Columns.Add(dc1);
				ds.Tables[0].Columns.Add(dc2);
		
				for( int i=0; i< ds.Tables[0].Rows.Count; i++)
				{				
					ds.Tables[0].Rows[i]["FPriTypeS"] = DicFPriTypeS(int.Parse(ds.Tables[0].Rows[i]["FPriType"].ToString()));
					ds.Tables[0].Rows[i]["FAmountSetS"] = ds.Tables[0].Rows[i]["FAmountSet"].ToString();
					ds.Tables[0].Rows[i]["FStateS"] = DicFState(int.Parse(ds.Tables[0].Rows[i]["FState"].ToString()));					
				}

				this.DataGrid1.DataSource = ds.Tables[0].DefaultView;
				this.DataGrid1.DataBind();			
			}
			catch(Exception ex)
			{
				this.lb_msg.Text = ex.Message;
			}
		}

		private string DicFPriTypeS(int pritype)
		{
			switch(pritype)
			{
				case 1: return "������ȫ��תָ���Է��ʻ�";
				case 2: return "������ȫ��ת�����ʻ�";
				case 3: return "������ȫ�����汾�ʻ�";
				case 4: return "�ʻ����ȫ��ת�����˻�";
				case 5: return "�ʻ����ȫ��תָ���Է��ʻ�";
				case 6: return "�ʻ��������ָ��������תָ���Է��ʻ�";
				case 7: return "�ʻ��������ָ��������ת�����˻�";
				case 8: return "����ת�Է��ʻ���ʹ�Է��ʻ���ָ����ʣ�ಿ��ת�����˻�";
				case 9: return "תָ�������ָ���Է��ʻ�";
				case 10: return "T+0����";
				default: return "unknown";
			}
		}

		private string DicFState(int state)
		{
			switch(state)
			{
				case 1: return "�̻�����";
				case 2: return "�̻�����";
				case 3: return "�Ƹ�ͨ����ͨ��";
				case 4: return "�ܾ�";
				case 5: return "����";
				case 6: return "����";
				default: return "unknown";
			}
		}

	}
}
