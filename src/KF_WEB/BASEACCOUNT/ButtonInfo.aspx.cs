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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;

using System.Reflection;
using Wuqi.Webdiyer;
using System.Configuration;

using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// ButtonInfo ��ժҪ˵����
	/// </summary>
	public partial class ButtonInfo : System.Web.UI.Page
	{

		System.Data.DataSet ds;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				BindData(0,1); 
			}
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex=e.NewPageIndex;
			BindData(0,pager.CurrentPageIndex);
		}
		
		private void BindData(int i,int pageIndex)
		{	    
			if(Session["uid"] == null)
			{
				WebUtils.ShowMessage(this.Page,"��ʱ�������µ�½��");
				Response.Write("<script language=javascript>window.parent.location='../login.aspx?wh=1'</script>");
			}
			else
			{
				try
				{
					string selectStr = Session["QQID"].ToString();

					DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());
					DateTime endTime   = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());

					int pageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());

					int istr = 1 + pageSize * (pageIndex-1);   //1��ʾ��һҳ
					int imax = pageSize;

					ds = classLibrary.setConfig.returnDataSet(selectStr,1,beginTime,endTime,0,"ButtonInfo",istr,imax,Session["uid"].ToString(),Request.UserHostAddress); 

					//��ѯ���׵��ļ�¼��
					int total;
					if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
						total = Int32.Parse(ds.Tables[0].Rows[0]["total"].ToString().Trim());
					else
						total = 0;
					pager.RecordCount= total;
					pager.PageSize   = pageSize;

					pager.CustomInfoText="��¼������<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
					pager.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
					pager.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";

					DataGrid2.DataSource = ds.Tables[0];
					DataGrid2.DataBind();
					if (ds!=null)
						ds.Dispose();
				}
				catch(SoapException eSoap) //����soap��
				{
					string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
					WebUtils.ShowMessage(this.Page,"��ѯ������ϸ��"+ str);
				}
				catch(Exception e)
				{
					Response.Write("<font color=red>��ѯ������ϸ��" + e.Message + "</font>");
				}		
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
			this.DataGrid2.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid2_ItemDataBound);

		}
		#endregion

		private void DataGrid2_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemIndex>=0)
			{
				HyperLink hl = (HyperLink)e.Item.Cells[2].Controls[0];
				if (hl.Text.Length>30)
					hl.Text = hl.Text.Substring(0,30)+"...";
				e.Item.Cells[3].Text = setConfig.FenToYuan(e.Item.Cells[3].Text);
				e.Item.Cells[4].Text = e.Item.Cells[4].Text=="0" ? "���" : "����";
				e.Item.Cells[5].Text = e.Item.Cells[5].Text=="-1" ? "��֧��" : setConfig.FenToYuan(e.Item.Cells[5].Text);
				e.Item.Cells[6].Text = e.Item.Cells[6].Text=="-1" ? "��֧��" : setConfig.FenToYuan(e.Item.Cells[6].Text);
				string status = e.Item.Cells[11].Text;
				if (status=="0")
				{
					e.Item.Cells[11].Text = "��Ч";
					e.Item.ForeColor = System.Drawing.Color.Gray;
				}
				else if (status=="1")
					e.Item.Cells[11].Text = "��Ч";
				else if (status=="2")
					e.Item.Cells[11].Text = "����";
			}
		}
	}
}
