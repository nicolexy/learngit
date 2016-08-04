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
using CFT.CSOMS.BLL.TradeModule;



namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PayLimitManage ��ժҪ˵����
	/// </summary>
	public partial class PayLimitManage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected Wuqi.Webdiyer.AspNetPager pager;
		protected System.Web.UI.WebControls.DataGrid DataGrid2;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				Table2.Visible = false;
				BindChannel(ddlChannel);
			}
		}

		private void BindChannel(DropDownList ddl)
		{
			ddl.Items.Clear();
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.GetAllChannelList();
            
				if(ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
				{
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						ddl.Items.Add(new ListItem(dr["Fchannel_name"].ToString(),dr["Fchannel_id"].ToString()));
					}
				}

				ddl.Items.Insert(0,new ListItem("��������","0"));
			}
			catch
			{}
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
			this.DataGrid1.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid1_PageIndexChanged);

		}
		#endregion

		private void ValidateDate()
		{
			ViewState["uid"] = tbQQID.Text.Trim();

			if(tbQQID.Text.Trim() == "")
			{
				throw new LogicException("�������ʺ�");
			}

			ViewState["channelid"] = ddlChannel.SelectedValue;
			ViewState["auid"] = tbaqqID.Text.Trim();
			if(this.rbtKeap.Checked)
				ViewState["IsKeap"] = "true";
			else
				ViewState["IsKeap"] = "false";
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
				this.DataGrid1.CurrentPageIndex = 0;
				BindData();
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


		private void BindData()
		{
			string u_ID = ViewState["uid"].ToString();
			int channelid = Int32.Parse(ViewState["channelid"].ToString());
			string  auid = ViewState["auid"].ToString();
			string IsKeap = ViewState["IsKeap"].ToString();

			if(IsKeap == "true")
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.GetPayLimitList(u_ID,channelid,auid);

				if(ds != null && ds.Tables.Count >0)
				{
					ds.Tables[0].Columns.Add("FdirectName",typeof(String));
					ds.Tables[0].Columns.Add("Flist_stateName",typeof(String));

					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						if(dr["Fdirect"].ToString() == "1")
							dr["FdirectName"] = "��";
						else if(dr["Fdirect"].ToString() == "2")
							dr["FdirectName"] = "��";

						if(dr["Flist_state"].ToString() == "1")
							dr["Flist_stateName"] = "����";
						else if(dr["Flist_state"].ToString() == "2")
							dr["Flist_stateName"] = "����";
					}

					DataGrid1.DataSource = ds.Tables[0].DefaultView;
					DataGrid1.DataBind();
				}
				else
				{
					throw new LogicException("û���ҵ���¼��");
				}
			}
			else
			{
				if(auid == "")
				{
					throw new Exception("������Է��ʺţ�");
				}

                //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //DataSet ds = qs.GetTrustLimitList(u_ID,auid);

                SettleService service = new SettleService();
                DataTable dt = service.GetTrustLimitList(u_ID, auid);

				if(dt != null  && dt.Rows.Count>0)
				{
					try
					{
						int Ftrust_rule = int.Parse(dt.Rows[0]["Ftrust_rule"].ToString().Trim());
						string Ftrust_ruleStr = Convert.ToString((long)Ftrust_rule,2);
						//���һλΪ�˿�Ȩ��
						if(Ftrust_ruleStr.EndsWith("1"))
							this.lblTrustLimit.Text = "ί���˿�Ȩ�޿�ͨ";
						else
							this.lblTrustLimit.Text = "ί���˿�Ȩ��δ��ͨ";

					}
					catch
					{
						this.lblTrustLimit.Text = "ί���˿�Ȩ��δ��ͨ";
					}
				}
				else
				{
					this.lblTrustLimit.Text = "ί���˿�Ȩ��δ��ͨ";
				}
			}
		}

		private void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}
	}
}
