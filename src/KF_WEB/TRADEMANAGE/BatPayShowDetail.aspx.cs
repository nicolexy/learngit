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


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// BatPayShowDetail ��ժҪ˵����
	/// </summary>
	public partial class BatPayShowDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Web.UI.WebControls.HyperLink hlBack;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{                
				string BatchID = Request.QueryString["BatchID"];
				if(BatchID == null) BatchID = "0"; 
				ViewState["BatchID"] = BatchID;
                
				GetAllPayBankList(ddlBankType);
				ddlBankType.Items.Insert(0,new ListItem("��������","0000"));
			}
		}

		private void GetAllPayBankList(DropDownList ddl)
		{
			ddl.Items.Clear();
			ddl.Items.Add(new ListItem("��������","1001"));
			ddl.Items.Add(new ListItem("��������","1002"));
			ddl.Items.Add(new ListItem("�������ÿ�","1050"));
			ddl.Items.Add(new ListItem("��������","1003"));
			ddl.Items.Add(new ListItem("ũҵ����","1005"));
			ddl.Items.Add(new ListItem("�ַ�����","1004"));
			ddl.Items.Add(new ListItem("����ƽ������","1010"));
			ddl.Items.Add(new ListItem("��ͨ����","1020"));
			ddl.Items.Add(new ListItem("�㶫��չ����","1027"));
			ddl.Items.Add(new ListItem("��������","1021"));
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

		private void BindData(int index)
		{
			int state = Int32.Parse(ViewState["state"].ToString());
			string username = ViewState["username"].ToString();
			string bankacc = ViewState["bankacc"].ToString();
			string count = ViewState["count"].ToString();
			string paybank = ViewState["paybank"].ToString();
			string BatchID = ViewState["BatchID"].ToString();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			DataSet ds = qs.ShowDetail_BindData(max,start,BatchID,state,username,bankacc,count,paybank);

			if(ds!=null && ds.Tables.Count>0)
			{
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
			}
			else
			{
				DataGrid1.DataSource = null;
			}

			if(BatchID.Substring(8,4) != "9999")
			{
				DataGrid1.Columns[5].Visible = true;
				DataGrid1.Columns[6].Visible = false;
			}
			else
			{
				DataGrid1.Columns[5].Visible = true;
				DataGrid1.Columns[6].Visible = true;
			}

			DataGrid1.DataBind();
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(this.tbUserName.Text.Trim() == "" && this.tbBankAcc.Text.Trim() == "")
				{
					throw new Exception("�û������������ʺ���������һ�");
				}
				ViewState["count"] = Common.MoneyTransfer.YuanToFen(tbCount.Text.Trim());	
				ViewState["state"] = ddlState.SelectedValue;
				ViewState["username"] = tbUserName.Text.Trim();
				ViewState["bankacc"] = tbBankAcc.Text.Trim();
				ViewState["paybank"] = ddlBankType.SelectedValue;

				pager.RecordCount= 10000;//GetCount(BatchID);
				BindData(1);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,ex.Message);
			}
		}


	}
}
