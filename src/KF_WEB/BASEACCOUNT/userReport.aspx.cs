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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// userReport ��ժҪ˵����
	/// </summary>
	public partial class userReport : System.Web.UI.Page
	{   

		public    DataSet ds;   //����ԴDataSet
		public    string  Msg;
		int       istr=0;
		int       pageSize = 20;     //Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			//if (Session["uid"] == null || !AllUserRight.ValidRight(Session["szkey"].ToString(),Int32.Parse(Session["OperID"].ToString()),PublicRes.GROUPID, "UserReport")) Response.Redirect("../login.aspx?wh=1");

			if(!classLibrary.ClassLib.ValidateRight("UserReport",this)) Response.Redirect("../login.aspx?wh=1");

			pageSize = Int32.Parse(ddlPageSize.SelectedValue);

			if (!Page.IsPostBack)
			{
			    this.TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
				
				if (!bindaData())
				{
					//WebUtils.ShowMessage(this.Page,Msg);
					return;
				}

		    }
			
			//Page.DataBind();
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
			//�����ѯ���
			DateTime bgDate = DateTime.Now;
			DateTime edDate = DateTime.Now;
			string whereStr = "";   //�����ж����
  
			try
			{
                bgDate = DateTime.Parse(this.TextBoxBeginDate.Value.Trim());
                edDate = DateTime.Parse(this.TextBoxEndDate.Value.Trim());
			}
			catch
			{
				Msg = "���ڸ�ʽ����������飡";
				return false;
			}

			if (this.txbCustom.Text != "" && this.txbCustom.Text.Trim() != "")
			{
				whereStr = " and uin = '" + this.txbCustom.Text.Trim() + "' " ;  //����Q������
			}
			

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			if (ViewState["newIndex"] != null)
				istr = Int32.Parse(ViewState["newIndex"].ToString());
			else
				istr = 0;

			int iStartIndex = istr*pageSize;

			string log = classLibrary.SensitivePowerOperaLib.MakeLog("get",Session["uid"].ToString().Trim(),"[��ѯ���Ͷ��]",iStartIndex.ToString(),
				pageSize.ToString(),bgDate.ToString(),edDate.ToString(),whereStr);

			if(!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("UserReport",log,this))
			{
					
			}

			if (!qs.getUserReports(istr*pageSize,pageSize,bgDate,edDate,whereStr,out ds,out Msg)) return false;

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

		private void AspNetPager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			istr = e.NewPageIndex;
			AspNetPager1.CurrentPageIndex = istr;

			ViewState["newIndex"] = e.NewPageIndex -1;

			bindaData();
		}

		protected void btQuery_Click(object sender, System.EventArgs e)
		{
			string strszkey = Session["SzKey"].ToString().Trim();
			int ioperid = Int32.Parse(Session["OperID"].ToString());
			int iserviceid = Common.AllUserRight.GetServiceID("UserReport") ;
			string struserdata = Session["uid"].ToString().Trim();
			string content = struserdata + "ִ����[��ѯ���Ͷ��]����,��������[" + ""
				+ "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

			

			ViewState["newIndex"] = null;  //������µ��һ�β�ѯ������ղ�ѯ�ķ�ҳ�������޷���ѯ�����ݣ����絥�ʣ�
			AspNetPager1.Visible  = true;
			this.AspNetPager1.CurrentPageIndex = 1;

			bindaData();
		}

		protected void ddlCondition_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//����ѡ�������˵��仯�����������ǿգ���ʾ����Ҫ��ȷ��ѯ����յ�ǰ��ҳ�� ViewState["newIndex"]	
			if (txbCustom.Text != "" && txbCustom.Text.Trim() !="")
			{
				ViewState["newIndex"] = null;
				AspNetPager1.Visible = true;
			}
		}
	}
}
