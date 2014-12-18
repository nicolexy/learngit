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

namespace TENCENT.OSS.CFT.KF.KF_Web.FreezeManage
{
	/// <summary>
	/// FreezeDiary ��ժҪ˵����
	/// </summary>
	public partial class FreezeDiary : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!IsPostBack)
			{
				ViewState["FFreezeListID"] = Request.QueryString["FFreezeListID"].Trim();

				if(Request.QueryString["state"] != null && Request.QueryString["state"] == "s")
				{
					WebUtils.ShowMessage(this,"�����ɹ�");
				}
				else if(Request.QueryString["state"] != null && Request.QueryString["state"] == "f")
				{
					WebUtils.ShowMessage(this,"����ʧ�ܣ���ȷ�϶��ᵥ�Ĵ�����־״̬");
				}

				this.lb_operatorID.Text = Session["uid"].ToString();

				BindData_ForDiary(1);
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


		private void BindData_ForDiary(int index)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			string tdeid = ViewState["FFreezeListID"].ToString();

			DataSet ds =  qs.GetFreezeDiary("",tdeid,"","","","","","",1,20);

			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"�ö��ᵥû�д�����־");
				return;
			}	

			ds.Tables[0].Columns.Add("DiaryHandleResult",typeof(string));

			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				dr["DiaryHandleResult"] = dr["FCreateDate"].ToString() + "  " + dr["FHandleUser"].ToString()
                    + " ִ���� " + ConvertHandleTypeToString(dr["FHandleType"].ToString()) + " �������û�����Ϊ��" + dr["FMemo"].ToString() + "���ͷ�������Ϊ��" + dr["FHandleResult"].ToString();
			}

			this.Datagrid1.DataSource = ds;
			this.Datagrid1.DataBind();
		}

		private string ConvertHandleTypeToString(string type)
		{
			switch(type)
			{
				case "1":
				{
					return "�ᵥ(�ѽⶳ)";
				}
				case "2":
				{
					return "��������";
				}
				case "7":
				{
					return "����";
				}
				case "8":
				{
					return "����";
				}
				case "100":
				{
					return "���䴦����";
				}
				default:
				{
					return "δ֪����" + type;
				}
			}
		}
	}
}
