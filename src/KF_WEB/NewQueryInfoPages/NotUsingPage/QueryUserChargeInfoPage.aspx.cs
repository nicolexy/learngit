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

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryUserChargeInfoPage ��ժҪ˵����
	/// </summary>
	public class QueryUserChargeInfoPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.RadioButton rtnList;
		protected System.Web.UI.WebControls.RadioButton rtbSpid;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox TextBoxBeginDate;
		protected System.Web.UI.WebControls.ImageButton ButtonBeginDate;
		protected System.Web.UI.WebControls.Button btnQuery;
		protected Wuqi.Webdiyer.AspNetPager pager;
		protected System.Web.UI.WebControls.DropDownList dd_querySubject;
		protected System.Web.UI.WebControls.TextBox tbx_findDate;
		protected System.Web.UI.WebControls.DataGrid DataGrid_QueryResult;
		protected System.Web.UI.WebControls.TextBox tbx_findDate_byID;
		protected System.Web.UI.WebControls.ImageButton imgbtn_findDate;
		protected System.Web.UI.WebControls.TextBox tbx_tradeID;
		protected System.Web.UI.WebControls.Label lb_pageTitle;
		protected System.Web.UI.WebControls.Label lb_operatorID;
		protected System.Web.UI.WebControls.Label lb_listID;
		protected System.Web.UI.WebControls.Label lb_3;
		protected System.Web.UI.WebControls.TextBox tbx_3;
		protected System.Web.UI.WebControls.Label lb_4;
		protected System.Web.UI.WebControls.TextBox tbx_4;
		protected System.Web.UI.WebControls.Label lb_1;
		protected System.Web.UI.WebControls.TextBox tbx_1;
		protected System.Web.UI.WebControls.Label lb_2;
		protected System.Web.UI.WebControls.TextBox tbx_2;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			// ����ʱ���õ�QQID��
			Session["QQID"] = "1204168901@mch.tenpay.com";

			if(!this.IsPostBack)
			{
				this.tbx_findDate.Text = DateTime.Now.ToShortDateString();

				this.tbx_findDate_byID.Text = DateTime.Now.ToShortDateString();

				this.lb_operatorID.Text = Session["QQID"].ToString();
			}
	
			this.lb_pageTitle.Text = this.dd_querySubject.SelectedValue;

			this.btnQuery.Click +=new EventHandler(btnQuery_Click);

			this.dd_querySubject.SelectedIndexChanged += new EventHandler(dd_querySubject_SelectedIndexChanged);

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
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.pager_PageChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// һҳDataGrid��ʾ��������
		/// </summary>
		private const int IElemCount = 6;


		private void btnQuery_Click(object sender, EventArgs e)
		{
			StartQuery();
		}



		private void StartQuery()
		{
			/*
			int selectedIndex = this.dd_querySubject.SelectedIndex;

			DateTime findDate = DateTime.Now;

			// ��������Ľ��׵�����������һ�����  
			if(this.rtnList.Checked)
			{
				try
				{
					findDate = DateTime.Parse(this.tbx_findDate_byID.Text);
				}
				catch
				{
					Response.Write("<script language=javascript>alert('�������ڸ�ʽ����ȷ��');</script>");

					return;
				}

				switch(selectedIndex)
				{
					case 0:
					{
						

						break;
					}	
					case 1:
					{
						

						break;
					}
					case 2:
					{
						Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

						DataSet queryResult = query.GetUserChargeInfo_ByListID(this.tbx_tradeID.Text,findDate);

						this.DataGrid_QueryResult.DataSource = queryResult;

						this.DataGrid_QueryResult.DataBind();

						break;
					}	
					case 3:
					{
						Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

						DataSet queryResult = query.GetUserGetCashInfo_ByListID(this.tbx_tradeID.Text,findDate);

						this.DataGrid_QueryResult.DataSource = queryResult;

						this.DataGrid_QueryResult.DataBind();

						break;
					}
					case 4:
					{
						Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

						DataSet queryResult = query.GetUserUndoInfo_ByListID(this.tbx_tradeID.Text,findDate);

						this.DataGrid_QueryResult.DataSource = queryResult;

						this.DataGrid_QueryResult.DataBind();

						break;
					}	
					case 5:
					{
						Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

						DataSet queryResult = query.GetUserDealInfo_ByListID(this.tbx_tradeID.Text,findDate);

						this.DataGrid_QueryResult.DataSource = queryResult;

						this.DataGrid_QueryResult.DataBind();

						break;
					}
					default:
					{
						Response.Write("<script language=javascript>alert('����ʧ�ܣ���֧�ֵ�������')</script>");

						break;
					}
				}
			}
			else
			{
				try
				{
					findDate = DateTime.Parse(this.tbx_findDate.Text);
				}
				catch
				{
					Response.Write("<script language=javascript>alert('�������ڸ�ʽ����ȷ��');</script>");

					return;
				}

				switch(selectedIndex)
				{
					case 0:
					{
						

						break;
					}	
					case 1:
					{
						break;
					}
					case 2:
					{
						Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

						DataSet queryResult = query.GetUserChargeInfo_ByDetail(Session["QQID"].ToString(),findDate,int.Parse(this.tbx_2.Text),int.Parse(tbx_3.Text),int.Parse(this.tbx_4.Text),this.pager.PageCount,IElemCount);

						this.DataGrid_QueryResult.DataSource = queryResult;

						this.DataGrid_QueryResult.DataBind();

						break;
					}	
					case 3:
					{
						Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

						DataSet queryResult = query.GetUserGetCashInfo_ByDetail(Session["QQID"].ToString(),findDate,this.pager.PageCount,IElemCount);

						this.DataGrid_QueryResult.DataSource = queryResult;

						this.DataGrid_QueryResult.DataBind();

						break;
					}
					case 4:
					{
						Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

						DataSet queryResult = query.GetUserUndoInfo_ByDetail(Session["QQID"].ToString(),findDate,this.pager.PageCount,IElemCount);

						this.DataGrid_QueryResult.DataSource = queryResult;

						this.DataGrid_QueryResult.DataBind();

						break;
					}	
					case 5:
					{
						Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

						DataSet queryResult = query.GetUserDealInfo_ByDetail(Session["QQID"].ToString(),findDate,this.pager.PageCount,IElemCount);

						this.DataGrid_QueryResult.DataSource = queryResult;

						this.DataGrid_QueryResult.DataBind();

						break;
					}
					default:
					{
						Response.Write("<script language=javascript>alert('����ʧ�ܣ���֧�ֵ�������')</script>");

						break;
					}
				}
			}
			*/
		}


		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			// ������һҳ���൱���ٴ�����
			this.StartQuery();
		}



		private void dd_querySubject_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch(this.dd_querySubject.SelectedIndex)
			{
				case 0:
				{
					this.lb_1.Text = "ǩԼ��";
					
					this.lb_2.Text = "֤����";
					this.lb_3.Text = "����";
					this.lb_4.Text = "״̬";

					this.lb_2.Visible = true;
					this.lb_3.Visible = true;
					this.lb_4.Visible = true;

					this.tbx_2.Visible = true;
					this.tbx_3.Visible = true;
					this.tbx_4.Visible = true;

					this.StartQuery();

					break;
				}
				case 1:
				{
					this.lb_1.Text = "ǩԼ��";
					
					this.lb_2.Text = "֤����";
					this.lb_3.Text = "����";
					this.lb_4.Text = "״̬";

					this.lb_2.Visible = true;
					this.lb_3.Visible = true;
					this.lb_4.Visible = true;

					this.tbx_2.Visible = true;
					this.tbx_3.Visible = true;
					this.tbx_4.Visible = true;

					break;
				}
				case 2:
				{
					this.lb_1.Text = "�Ƹ�ͨ�ʺ�";
					
					this.lb_2.Text = "����״̬";
					this.lb_3.Text = "��������";
					this.lb_4.Text = "��������";

					this.tbx_2.Visible = false;
					this.tbx_3.Visible = false;
					this.tbx_4.Visible = false;

					this.StartQuery();

					break;
				}
				case 3:
				{
					this.lb_1.Text = "�Ƹ�ͨ�ʺ�";
					
					this.lb_2.Visible = false;
					this.lb_3.Visible = false;
					this.lb_4.Visible = false;

					this.tbx_2.Visible = false;
					this.tbx_3.Visible = false;
					this.tbx_4.Visible = false;

					break;
				}
				case 4:
				{
					this.lb_1.Text = "�Ƹ�ͨ�ʺ�";
					
					this.lb_2.Visible = false;
					this.lb_3.Visible = false;
					this.lb_4.Visible = false;

					this.tbx_2.Visible = false;
					this.tbx_3.Visible = false;
					this.tbx_4.Visible = false;

					break;
				}
				case 5:
				{
					this.lb_1.Text = "�Ƹ�ͨ�ʺ�";
					
					this.lb_2.Visible = false;
					this.lb_3.Visible = false;
					this.lb_4.Visible = false;

					this.tbx_2.Visible = false;
					this.tbx_3.Visible = false;
					this.tbx_4.Visible = false;

					break;
				}
				default:
				{
					break;
				}
			}
		}



		private void ShowMsg(string args)
		{
			Response.Write("<script language=javascript>alert('" + args +  "')</script>");
		}

		
	}
}
