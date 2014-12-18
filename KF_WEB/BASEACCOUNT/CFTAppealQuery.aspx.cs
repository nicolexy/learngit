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

	/// CFTAppealQuery ��ժҪ˵����

	/// </summary>

	public partial class CFTAppealQuery : System.Web.UI.Page

	{
















	

		public static string rooturl

		{

			get

			{

				string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();



				if(!url.EndsWith("/"))

					url += "/";



				return url;

			}

		}

		protected void Page_Load(object sender, System.EventArgs e)

		{

			// �ڴ˴������û������Գ�ʼ��ҳ��

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

				TextBoxBeginDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy��MM��dd��");

				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");



				Table2.Visible = false;		

		

				if(Request.QueryString["user"] != null)

				{

					tbFuin.Text = Request.QueryString["user"].Trim();



					if(tbFuin.Text.StartsWith("�ϼ�"))

						tbFuin.Text = "";

				}



				if(Request.QueryString["state"] != null)

				{

					ddlState.SelectedValue = Request.QueryString["state"].Trim();

				}



				if(Request.QueryString["begin"] != null)

				{

					TextBoxBeginDate.Text = DateTime.Parse(Request.QueryString["begin"].Trim()).ToString("yyyy��MM��dd��");

				}



				if(Request.QueryString["end"] != null)

				{

					TextBoxEndDate.Text = DateTime.Parse(Request.QueryString["end"].Trim()).ToString("yyyy��MM��dd��");

					Button2_Click(null,null);

				}

				this.rbtnFuin.Checked = true;

			}

			if(rbtnFuin.Checked)
			{
				txtQQ.Enabled = false;
				TextBoxBeginDate.Enabled = true;
				ButtonBeginDate.Enabled = true;
				TextBoxEndDate.Enabled = true;
				ButtonEndDate.Enabled = true;
				tbFuin.Enabled = true;
				ddlState.Enabled = true;
			}
			else
			{
				txtQQ.Enabled = true;
				TextBoxBeginDate.Enabled = false;
				ButtonBeginDate.Enabled = false;
				TextBoxEndDate.Enabled = false;
				ButtonEndDate.Enabled = false;
				tbFuin.Enabled = false;
				ddlState.Enabled = false;
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
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
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

			ViewState["rbtnFuin"] = rbtnFuin.Checked;

			if(this.rbtnFuin.Checked)
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


				ViewState["fstate"] = ddlState.SelectedValue;
				string stmp = tbFuin.Text.Trim();
				ViewState["fuin"] = classLibrary.setConfig.replaceMStr(stmp);
				ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
				ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
			}
			else
			{
				ViewState["fQQ"] = txtQQ.Text.Trim();
			}

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

				pager.RecordCount= GetCount(); 

				BindData(1);

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



		private int GetCount()

		{
			return 10000;
/*
			bool Isfuin = Convert.ToBoolean(ViewState["rbtnFuin"]);

			string fuin = "";
			string fQQ = "";
			DateTime begindate = DateTime.MinValue;
			DateTime enddate = DateTime.MinValue;
			int fstate= 0;

			if(Isfuin)
			{
				fuin = ViewState["fuin"].ToString();
				begindate = (DateTime)ViewState["begindate"];
				enddate = (DateTime)ViewState["enddate"];
				fstate = Int32.Parse(ViewState["fstate"].ToString());
			}
			else
			{
				fQQ = ViewState["fQQ"].ToString();
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			return qs.GetCFTQueryAppealCount(fuin,begindate,enddate,fstate,Isfuin,fQQ);
*/
		}



		private void BindData(int index)

		{
/*
			bool Isfuin = Convert.ToBoolean(ViewState["rbtnFuin"]);

			string fuin = "";
			string fQQ = "";
			DateTime begindate = DateTime.MinValue;
			DateTime enddate = DateTime.MinValue;
			int fstate= 0;

			if(Isfuin)
			{
				fuin = ViewState["fuin"].ToString();
				begindate = DateTime.Parse(ViewState["begindate"].ToString());
				enddate = DateTime.Parse(ViewState["enddate"].ToString());
				fstate = Int32.Parse(ViewState["fstate"].ToString());
			}
			else
			{
				fQQ = ViewState["fQQ"].ToString();
			}

			int max = pager.PageSize;

			int start = max * (index-1) + 1;



			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();



			Finance_Header fh = new Finance_Header();

			fh.UserIP = Request.UserHostAddress;

			fh.UserName = Session["uid"].ToString();



			fh.OperID = Int32.Parse(Session["OperID"].ToString());

			fh.SzKey = Session["SzKey"].ToString();

			//fh.RightString = Session["key"].ToString();



			qs.Finance_HeaderValue = fh;



			DataSet ds = qs.GetCFTQueryAppealList(fuin,begindate,enddate,fstate,start,max,Isfuin,fQQ);



			if(ds != null && ds.Tables.Count >0)

			{

				DataGrid1.DataSource = ds.Tables[0].DefaultView;

				DataGrid1.DataBind();

			}

			else

			{

				throw new LogicException("û���ҵ���¼��");

			}
*/
		}



		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)

		{

			string stmp = e.Item.Cells[0].Text.Trim();

			int strlen = stmp.Length;



			if(strlen > 6)

			{

				stmp = "***" + stmp.Substring(3,strlen-3);

				e.Item.Cells[0].Text = stmp;

			}

		}

	}

}
