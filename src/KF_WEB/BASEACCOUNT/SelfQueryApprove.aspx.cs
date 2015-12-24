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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// SelfQueryApprove ��ժҪ˵����
	/// </summary>
	public partial class SelfQueryApprove : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"DrawAndApprove")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("DrawAndApprove",this)) Response.Redirect("../login.aspx?wh=1");
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
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

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
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(eSys.Message) );
			}
		}


		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			string strszkey = Session["SzKey"].ToString().Trim();
			int ioperid = Int32.Parse(Session["OperID"].ToString());
			int iserviceid = Common.AllUserRight.GetServiceID("DrawAndApprove") ;
			string struserdata = Session["uid"].ToString().Trim();
			string content = struserdata + "ִ����[�����̻���˲�ѯ]����,��������[" + this.BoxCpName.Text.Trim()
				+ "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

			

			try
			{
				pager.RecordCount = GetCount(); 
				lblResultCount.Text = pager.RecordCount.ToString();
				BindData(1);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(eSys.Message));
			}
		}


		private int GetCount()
		{
			string filter = GetfilterString();
			ViewState["filter"] = filter;


            string strBeginTime = Convert.ToDateTime(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00");
            string strEndTime = Convert.ToDateTime(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59");

            string xx = BoxCpName.Text.Trim();
           // int index = int.Parse(BoxCpStatus.SelectedValue);
            string SPID = BoxCpNumber.Text.Trim();
            int? DraftFlag = 0;
            string CompanyName = BoxCpName.Text.Trim();
            int? Flag = -1;
            string WWWAdress = boxWWWAddress.Text.Trim();
            string Appid = null;//txtAppid.Text.Trim();
            DateTime? ApplyTimeStart = Convert.ToDateTime(strBeginTime);
            DateTime? ApplyTimeEnd = Convert.ToDateTime(strEndTime);
            string BankUserName = tbBankName.Text.Trim();
            string KFCheckUser = Session["uid"].ToString();
            string SuggestUser = tbSuggestUser.Text.Trim();
            string MerType = null;//ddlMerType.SelectedValue;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.GetSelfQueryListCount(SPID, DraftFlag, CompanyName, Flag, WWWAdress, Appid, ApplyTimeStart, ApplyTimeEnd, BankUserName, KFCheckUser, SuggestUser, MerType);
		}

		private void BindData(int index)
		{
			string filter = ViewState["filter"].ToString();

			int TopCount = pager.PageSize;
			int NotInCount = TopCount * (index-1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

			string log = SensitivePowerOperaLib.MakeLog("get",Session["uid"].ToString().Trim(),"[�����̻���˲�ѯ]",filter
				,TopCount.ToString(),NotInCount.ToString());

			if(!SensitivePowerOperaLib.WriteOperationRecord("DrawAndApprove",log,this))
			{
					
			}
            string strBeginTime = Convert.ToDateTime(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00");
            string strEndTime = Convert.ToDateTime(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59");

            string xx = BoxCpName.Text.Trim();
            // int index = int.Parse(BoxCpStatus.SelectedValue);
            string SPID = BoxCpNumber.Text.Trim();
            int? DraftFlag = 0;
            string CompanyName = BoxCpName.Text.Trim();
            int? Flag = -1;
            string WWWAdress = boxWWWAddress.Text.Trim();
            string Appid = null;//txtAppid.Text.Trim();
            DateTime? ApplyTimeStart = Convert.ToDateTime(strBeginTime);
            DateTime? ApplyTimeEnd = Convert.ToDateTime(strEndTime);
            string BankUserName = tbBankName.Text.Trim();
            string KFCheckUser = Session["uid"].ToString();
            string SuggestUser = tbSuggestUser.Text.Trim();
            string MerType = null;//ddlMerType.SelectedValue;
            DataSet ds = qs.GetSelfQueryList(SPID, DraftFlag, CompanyName, Flag, WWWAdress, Appid, ApplyTimeStart, ApplyTimeEnd, BankUserName, KFCheckUser, SuggestUser, MerType, TopCount, NotInCount);

			if(ds != null && ds.Tables.Count >0)
			{
				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}


		#region ��ò�ѯ�ַ���
		protected string GetfilterString()
		{
			string filter = " and DraftFlag=0 and Flag=-1 and KFCheckUser='" + Session["uid"].ToString() + "' ";

			if(BoxCpName.Text.Trim() != "")
			{
				filter += " and CompanyName like '%" + BoxCpName.Text.Trim() + "%' ";
			}

			if(BoxCpNumber.Text.Trim() != "")
			{
				filter += " and SPID='" + BoxCpNumber.Text.Trim() + "' ";
			}

			if(boxWWWAddress.Text.Trim() != "")
			{
				filter += " and WWWAdress like '%" + boxWWWAddress.Text.Trim() + "%' ";
			}

            if (TextBoxBeginDate.Value.Trim() != "")
			{
                filter += " and ApplyTime >='" + Convert.ToDateTime(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00") + "' ";
			}

            if (TextBoxEndDate.Value.Trim() != "")
			{
                filter += " and ApplyTime <='" + Convert.ToDateTime(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59") + "' ";
			}

			if(tbBankName.Text.Trim() != "")
			{
				filter += " and BankUserName like '%" + tbBankName.Text.Trim() + "%' ";
			}

			if(tbSuggestUser.Text.Trim() != "")
			{
				filter += " and SuggestUser = '" + tbSuggestUser.Text.Trim() + "' ";
			}

			return filter;			
		}

		#endregion
	}
}