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
using TENCENT.OSS.CFT.KF.KF_Web.Control;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Text.RegularExpressions;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// QueryQQ ��ժҪ˵����
	/// </summary>
	public partial class QueryQQ : System.Web.UI.Page
	{
		public    DataSet ds;   //����ԴDataSet
		public    string  Msg;

		int       istr=0;
		int       pageSize = 20;     //Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			//if (Session["uid"] == null || !AllUserRight.ValidRight(Session["szkey"].ToString(),Int32.Parse(Session["OperID"].ToString()),PublicRes.GROUPID, "UserReport")) Response.Redirect("../login.aspx?wh=1");

			if(Session["uid"] == null || !classLibrary.ClassLib.ValidateRight("UserReport",this)) Response.Redirect("../login.aspx?wh=1");

			pageSize = Int32.Parse(ddlPageSize.SelectedValue);


			if (!Page.IsPostBack)
			{
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
			this.AspNetPager1.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.AspNetPager1_PageChanged);

		}
		#endregion

		private bool bindaData()
		{

			if (txbPara.Text == "" && this.txbPara.Text.Trim() == "")
			{
				Msg = "û�������ѯ�����ݡ�";
				WebUtils.ShowMessage(this.Page,Msg);
				return false;
			}

			string strContent	= setConfig.replaceMStr(txbPara.Text.Trim());
			string whereStr		= "";
			string dbName		= "";

			int nCondition = int.Parse(ddlCondition.SelectedIndex.ToString());

			if (0 == nCondition)
			{

				if (!IsNumeric(strContent))
				{
					Msg = "������������ݸ�ʽ���ԡ�";
					WebUtils.ShowMessage(this.Page,Msg);
					return false;
				}

				whereStr="where Fuid="		+ strContent;	//���ڲ�ID
				int nInternalID		= int.Parse(strContent);
				string strDBName	= string.Concat("c2c_db_" + nInternalID%100);
				string strTbl		= string.Concat("t_user_" + (nInternalID/100)%10);
				dbName				= strDBName + "." + strTbl;

			}
			else
			{

				if (1 == nCondition)
				{
					if (!IsNumeric(strContent))
					{
						Msg = "������������ݸ�ʽ���ԡ�";
						WebUtils.ShowMessage(this.Page,Msg);
						return false;
					}
						
					whereStr	= "where Fbankid='"	+ strContent + "'";	//�����п���
					dbName		= "c2c_analy_db.t_bank_user";
				}
				else if (2 == nCondition)
				{
					whereStr	= "where Fcreid='"	+ strContent + "'";	//�����֤��
					dbName		= "c2c_analy_db.t_user_info";
				}
				else if (3 == nCondition)
				{
					whereStr	= "where Ftruename='" + strContent + "'";//������
					dbName		= "c2c_analy_db.t_user_info";
				}
				else
				{
					return false;
				}						
				
			}
		


			if (ViewState["newIndex"] != null)
				istr = Int32.Parse(ViewState["newIndex"].ToString());
			else
				istr = 0;

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			if (!qs.GetQQByType(nCondition, dbName, istr*pageSize, pageSize, whereStr, out ds,out Msg)) return false;

			if (ds == null || ds.Tables.Count == 0 ||ds.Tables[0].Rows.Count == 0) 
			{
				AspNetPager1.Visible = false;
				Page.DataBind();  //���°� �����ʷ���ݣ������������

				Msg = "û����ѡ����Χ�ڵ����ݡ�";
				WebUtils.ShowMessage(this.Page,Msg);
				return false;
			}

			AspNetPager1.PageSize    = pageSize;
			AspNetPager1.RecordCount = Int32.Parse(ds.Tables[0].Rows[0]["icount"].ToString());
			
			AspNetPager1.CustomInfoText ="��¼������<font color=\"blue\"><b>"+AspNetPager1.RecordCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>" +AspNetPager1.PageCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"  +AspNetPager1.CurrentPageIndex.ToString()+"</b></font>";

			
			this.dgInfo.DataSource = ds.Tables[0].DefaultView;
			this.dgInfo.DataBind();

			return true;
		}


		public static bool IsNumeric(string value)
		{
			return Regex.IsMatch(value, @"^\d*$");
		}

		private void clickEvent()
		{
			ViewState["newIndex"] = null;  //������µ��һ�β�ѯ������ղ�ѯ�ķ�ҳ�������޷���ѯ�����ݣ����絥�ʣ�
			AspNetPager1.Visible = true;
			this.AspNetPager1.CurrentPageIndex = 1;

		}

		private void AspNetPager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			istr = e.NewPageIndex;
			AspNetPager1.CurrentPageIndex = istr;

			ViewState["newIndex"] = e.NewPageIndex -1;

			bindaData();
		}

		protected void btQuery_Click(object sender, System.EventArgs e)
		{

			clickEvent();
			bindaData();
		}

	}
}



