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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// CFDQuery ��ժҪ˵����
	/// </summary>
	public partial class CFDQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
            try
            {
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
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


		private void BindData(int index)
		{
			if(this.tbQQID.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"������QQ����");
				return;
			}

			this.pager.CurrentPageIndex = index;

			try
			{
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				//qs.Finance_HeaderValue = classLibrary.setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);
				qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

				DataSet ds = qs.QueryCFDInfo(this.tbQQID.Text.Trim());

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"��ѯ���Ϊ��");
					return;
				}

				ds.Tables[0].Columns.Add("FstateName",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					switch(dr["Fstate"].ToString())
					{
						case "1":
						{
							dr["FstateName"] = "�Ѱ�";break;
						}
						case "2":
						{
							dr["FstateName"] = "δ��";break;
						}
						default:
						{
							dr["FstateName"] = "δ֪״̬" + dr["Fstate"].ToString();break;
						}
					}
				}

				this.DataGrid1.DataSource = ds;
				this.DataGrid1.DataBind();
			}
			catch (System.Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			BindData(1);
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			BindData(e.NewPageIndex);
		}
	}
}
