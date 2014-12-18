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
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// batpayDetail ��ժҪ˵����
	/// </summary>
	public partial class batpayDetail : System.Web.UI.Page
	{
		private string BatchID;

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
			}
			catch
			{
				Label1.Text = "";
			}

			Button2.Attributes.Add("onclick","return CheckData();");

			if (!IsPostBack)
			{
				BatchID = Request.QueryString["BatchID"];
				if(BatchID == null) BatchID = "0";
				ViewState["BatchID"] = BatchID;

				batPay.GetAllPayBankList(ddlBankType);
				ddlBankType.Items.Insert(0,new ListItem("��������","0000"));
			}

			if(BatchID == null || BatchID == "")
				BatchID = ViewState["BatchID"].ToString();
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

		private int GetCount(string batchID)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.BatpayDetail_GetCount(batchID,Convert.ToInt32(this.ddlState.SelectedValue),this.tbUserName.Text.Trim(),this.tbBankAcc.Text.Trim(),this.ddlBankType.SelectedValue);
		}

		private void BindData(int index)
		{
			try
			{
				int max = pager.PageSize;
				int start = max * (index-1) + 1;

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.BatpayDetail_BindData(max,start,BatchID,Convert.ToInt32(this.ddlState.SelectedValue),this.tbUserName.Text.Trim(),this.tbBankAcc.Text.Trim(),this.ddlBankType.SelectedValue);

				if(ds!=null && ds.Tables.Count>0)
				{
					DataGrid1.DataSource = ds.Tables[0].DefaultView;
				}
				else
				{
					DataGrid1.DataSource = null;
				}

				if(!BatchID.EndsWith("9999"))
				{
					DataGrid1.Columns[6].Visible = true;
					DataGrid1.Columns[7].Visible = false;
				}
				else
				{
					DataGrid1.Columns[6].Visible = true;
					DataGrid1.Columns[7].Visible = true;
				}

				DataGrid1.DataBind();
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message);
			}
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			pager.RecordCount= GetCount(BatchID); 
			BindData(1);
		}
	}
}
