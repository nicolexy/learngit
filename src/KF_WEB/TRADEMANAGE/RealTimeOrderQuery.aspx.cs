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
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using CFT.CSOMS.BLL.TransferMeaning;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// RealTimeOrderQuery ��ժҪ˵����
	/// </summary>
	public partial class RealTimeOrderQuery : System.Web.UI.Page
	{
		public string begintime = DateTime.Now.ToString("yyyy-MM-dd");
		public string endtime = DateTime.Now.ToString("yyyy-MM-dd");

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
	
				TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

				classLibrary.setConfig.GetAllBankList(ddlBankType);
				ddlBankType.Items.Insert(0,new ListItem("��������","0000"));

				Table2.Visible = false;				
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


		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(e.NewPageIndex);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message);
			}
		}

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;
			string u_ID = tbQQID.Text.Trim();

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

			if(begindate.AddDays(3).CompareTo(enddate) < 0)
			{
				throw new Exception("ѡ��ʱ��γ��������죬���������룡");
			}

			try
			{
				float tmp = float.Parse(tbFNum.Text.Trim());
			}
			catch
			{
				throw new Exception("��������ȷ�Ľ�");
			}

			ViewState["fnum"] = tbFNum.Text.Trim();
			ViewState["fstate"] = ddlStateType.SelectedValue;

			ViewState["uid"] = u_ID;
			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
			begintime = begindate.ToString("yyyy-MM-dd");
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
			endtime = enddate.ToString("yyyy-MM-dd");

			ViewState["sorttype"] = ddlSortType.SelectedValue;
			ViewState["banktype"] = ddlBankType.SelectedValue;

		}


		protected void Button2_Click(object sender, System.EventArgs e)
		{
			Table2.Visible = false;

			try
			{
				ValidateDate();

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
				WebUtils.ShowMessage(this.Page, eSys.Message);
			}
		}

		private int GetCount()
		{
			string u_ID = ViewState["uid"].ToString();
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			float fnum = float.Parse(ViewState["fnum"].ToString());
			int fstate = Int32.Parse(ViewState["fstate"].ToString());

			string banktype = ViewState["banktype"].ToString();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.GetRealTimeOrderListCount(u_ID,begindate,enddate,fstate,fnum,banktype);
		}

		private void BindData(int index)
		{
			string u_ID = ViewState["uid"].ToString();
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			string banktype = ViewState["banktype"].ToString();

			string sorttype = ViewState["sorttype"].ToString();

			begintime = begindate.ToString("yyyy-MM-dd");
			endtime = enddate.ToString("yyyy-MM-dd");

			float fnum = float.Parse(ViewState["fnum"].ToString());
			int fstate = Int32.Parse(ViewState["fstate"].ToString());

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetRealTimeOrderList(u_ID,begindate,enddate,fstate,fnum,banktype,sorttype, start,max);

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("FNewAmount",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"FAmount","FNewAmount");

				ds.Tables[0].Columns.Add("FStatusName",typeof(String));
				ds.Tables[0].Columns.Add("FSignName",typeof(String));

				ds.Tables[0].Columns.Add("FBankName",typeof(String));
				
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();

					string tmp = dr["FStatus"].ToString();
					if(tmp == "0") 
						tmp = "�ѽ���";
					else if(tmp == "1") 
						tmp = "δ����";
					else if(tmp == "2") 
						tmp = "���ֽ���";
					else if(tmp == "3") 
						tmp = "����ʧ��";
					else if(tmp == "3") 
						tmp = "�ȴ�����";
					else if(tmp == "4") 
						tmp = "�ȴ�����";
					else if(tmp == "9") 
						tmp = "֧���ɹ�";
					
					dr["FStatusName"] = tmp;

					tmp = dr["FSign"].ToString();
					if(tmp == "0") 
						tmp = "��ʼ״̬";
					else if(tmp == "1") 
						tmp = "���账��";
					else if(tmp == "2") 
						tmp = "��Ҫ����";
					else if(tmp == "3") 
						tmp = "����ʧ��";
					else if(tmp == "4") 
						tmp = "ԭ�����ѳɹ�";
					else if(tmp == "5") 
						tmp = "�����Ѿ��ɹ�";
					else if(tmp == "6") 
						tmp = "����ƥ��ʧ��";

					dr["FSignName"] = tmp;

					tmp = dr["FBank_Type"].ToString();
                    dr["FBankName"] = Transfer.returnDicStr("BANK_TYPE", tmp);
					dr.EndEdit();
				}

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}


	}
}
