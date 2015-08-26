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
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.FundModule;
using CFT.CSOMS.BLL.CFTAccountModule;
using SunLibrary;
using CFT.CSOMS.BLL.TradeModule;


namespace TENCENT.OSS.C2C.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// ChangeQQ ��ժҪ˵����
	/// </summary>
	public partial class ChangeQQOld : System.Web.UI.Page
	{
	

		int       pageSize = 10;
		int       istr=0;
		string    Msg = "";

		protected void Page_Load(object sender, System.EventArgs e)
		{			
			// �ڴ˴������û������Գ�ʼ��ҳ��

//			try
//			{
//				string sr = Session["key"].ToString();
//				if (!AllUserRight.GetOneRightState("ChangeQQ",sr)) Response.Redirect("../login.aspx?wh=1");
//		   
//			}
//			catch  //���û�е�½����û��Ȩ�޾�����
//			{
//				Response.Redirect("../login.aspx?wh=1");
//			}

			try
			{
				//Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserAppeal")) Response.Redirect("../login.aspx?wh=1");
                if (!ClassLib.ValidateRight("UpdateAccountQQ", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				AspNetPager1.Visible = false;
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




		private int GetCount()
		{
			string qq = ViewState["qq"].ToString();
			string userid = ViewState["userid"].ToString();


			Query_Service qs = new Query_Service();
			
			return qs.GetChangeQQListCount(userid,qq);
		}

		private void BindData ()
		{
			string qq = ViewState["qq"].ToString();
			string userid = ViewState["userid"].ToString();

			//			int max = pager.PageSize;
			//			int start = max * (index-1) + 1;


			Query_Service qs = new Query_Service();

			Finance_Header fh = setConfig.setFH(this);
			//			fh.UserIP = Request.UserHostAddress;
			//			fh.UserName = Session["uid"].ToString();
			//
			//			fh.OperID = Int32.Parse(Session["OperID"].ToString());
			//			fh.SzKey = Session["SzKey"].ToString();
			//
			qs.Finance_HeaderValue = fh;
			//			qs.Finance_HeaderValue = setConfig.setFH(this);

			
			if (ViewState["newIndex"] != null)
				istr = Int32.Parse(ViewState["newIndex"].ToString());
			else
				istr = 0;



			//DataSet ds = qs.GetChangeQQList(begindate,enddate,userid,qq, start,max);
			DataSet ds = qs.GetChangeQQList(userid,qq, istr*pageSize + 1, pageSize);

			if (ds == null || ds.Tables.Count == 0 ||ds.Tables[0].Rows.Count == 0) 
			{
				AspNetPager1.Visible = false;
				Page.DataBind();  //���°� �����ʷ���ݣ������������
			
				Msg = "û����ѡ����Χ�ڵ����ݡ�";
				WebUtils.ShowMessage(this.Page,Msg);
				return ;
			}   
    
			AspNetPager1.RecordCount = GetCount(); 
			AspNetPager1.PageSize    = pageSize;
			AspNetPager1.CustomInfoText ="��¼������<font color=\"blue\"><b>"+AspNetPager1.RecordCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>" +AspNetPager1.PageCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"  +AspNetPager1.CurrentPageIndex.ToString()+"</b></font>";

			DataGrid1.DataSource = ds.Tables[0].DefaultView;
			DataGrid1.DataBind();
		}
			


		private void ValidateDate()
		{
			ViewState["qq"]		 = setConfig.replaceMStr(tbqueryQQ.Text.Trim());
			ViewState["userid"]	 = setConfig.replaceMStr(tbUserID.Text.Trim());
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			//��ѯ����ϸ.
			try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

//			try
//			{
//				AspNetPager1.RecordCount = GetCount(); 
//				BindData();
//			}
//			catch(SoapException eSoap) //����soap���쳣
//			{
//				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
//				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
//			}
//			catch(Exception eSys)
//			{
//				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message));
//			}


			ViewState["newIndex"] = null;  //������µ��һ�β�ѯ������ղ�ѯ�ķ�ҳ�������޷���ѯ�����ݣ����絥�ʣ�
			AspNetPager1.Visible = true;
			this.AspNetPager1.CurrentPageIndex = 1;
			BindData();

		}

		private void AspNetPager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
		
     		istr = e.NewPageIndex;
			AspNetPager1.CurrentPageIndex = istr;

			ViewState["newIndex"] = e.NewPageIndex -1;

			BindData();
		}


        //ԭ�ʺ����ͨ�˻����ͻ���ݶΪ0ʱ�������޸�ת��
        bool isHasBalance(string qq)
        {
            string uin = qq; //"1563686969"
            try
            {
                double totalBalance = 0;
                DataTable summaryTable = new FundService().GetUserFundSummary(uin);
                foreach (DataRow item in summaryTable.Rows)
                {
                    totalBalance += Convert.ToDouble(item["balance"].ToString());
                }
                if (totalBalance > 0)
                {
                    return true;
                }
            }
            catch
            {
            }

            try
            {
                double LCTBalance = 0;
                //���ͨ������89,ͳ�������ܺͣ�������ܺ�
                DataTable subAccountInfoTable = new LCTBalanceService().QuerySubAccountInfo(uin, 89);
                if (subAccountInfoTable != null && subAccountInfoTable.Rows.Count > 0)
                {
                    LCTBalance = Convert.ToDouble(subAccountInfoTable.Rows[0]["Fbalance"].ToString());
                }
                if (LCTBalance > 0)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
		
		
		protected void btnChangeQQ_Click(object sender, System.EventArgs e)
		{
			if(OldQQ.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this.Page,"��������ʺţ�");
                return;
			}

			if(NewQQ.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this.Page,"���������ʺţ�");
                return;
			}

            if (isHasBalance(OldQQ.Text.Trim())) 
            {
                WebUtils.ShowMessage(this.Page, "ԭ�ʺ����ͨ�˻����ͻ���ݶΪ0ʱ�������޸�ת����");
                return;
            }

            
            //try
            //{
            //    //ͨ��QQ��  ��ȡ΢�ŲƸ�ͨ�˺� 
            //    var openid = AccountService.GetQQID("WeChatQQ", OldQQ.Text).Replace("@wx.tenpay.com", "");
            //    if (!string.IsNullOrEmpty(openid))
            //    {
            //        var endDate = DateTime.Today.AddDays(+1);
            //        var startDate = endDate.AddDays(-15);
            //        var HasUnfinishedHB = (new TradeService()).QueryWXHasUnfinishedHB(openid, startDate, endDate);
            //        if (HasUnfinishedHB)
            //        {
            //            LogHelper.LogInfo("���˺���δ���΢�ź������ֹ�޸�!");
            //            WebUtils.ShowMessage(this.Page, "���˺���δ���΢�ź������ֹ�޸�!");
            //            return;
            //        }
            //    }   
            //}
            //catch (Exception)
            //{
            //    // �������쳣�����ѯ���쳣
            //}

 

			//����������
			//����������һ���������������̣���������ֱ��������
			TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param[] myParams = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param[3];

			myParams[0] = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param();
			myParams[0].ParamName = "OldQQ";
			myParams[0].ParamValue = OldQQ.Text.Trim();

			myParams[1] = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param();
			myParams[1].ParamName = "NewQQ";
			myParams[1].ParamValue = NewQQ.Text.Trim();

			myParams[2] = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param();
			myParams[2].ParamName = "Memo";
			myParams[2].ParamValue = "�޸��ʺţ�ԭ�ʺ�" +OldQQ.Text.Trim()+ "�����ʺ�" +  NewQQ.Text.Trim()
				+ "�����ɣ�" + TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr( tbMemo.Text.Trim());

//			TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header();
//			fh.UserIP = Request.UserHostAddress;
//			fh.UserName = Session["uid"].ToString();
//			fh.UserPassword = "";
//			fh.SzKey = Session["SzKey"].ToString();
//			fh.RightString = Session["SzKey"].ToString();
//			fh.OperID = Int32.Parse(Session["OperID"].ToString());


			TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Check_Service cs = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Check_Service();
//			cs.Finance_HeaderValue = fh;

			TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = setConfig.setFH_CheckService(this);
			//			fh.UserIP = Request.UserHostAddress;
			//			fh.UserName = Session["uid"].ToString();
			//
			//			fh.OperID = Int32.Parse(Session["OperID"].ToString());
			//			fh.SzKey = Session["SzKey"].ToString();
			//
			cs.Finance_HeaderValue = fh;
			//			qs.Finance_HeaderValue = setConfig.setFH(this);

			//��Ҫ����memo�Ͷ�����Ǯ��
			string strMemo = "�޸��ʺţ�ԭ�ʺ�" +OldQQ.Text.Trim()+ "�����ʺ�" +  NewQQ.Text.Trim()
				+ "�����ɣ�" + TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr( tbMemo.Text.Trim());

			try
			{
				cs.StartCheck(OldQQ.Text.Trim()  ,"ChangeQQ",strMemo,"0",myParams);
				PublicRes.writeSysLog(Session["uid"].ToString(),Request.UserHostAddress,"changeqq","�޸��ʺ���������",1,OldQQ.Text.Trim(),"");
				WebUtils.ShowMessage(this.Page,"���������ɹ���");
			}
			catch(Exception err)
			{
				PublicRes.writeSysLog(Session["uid"].ToString(),Request.UserHostAddress,"changeqq","�޸��ʺ���������",0,OldQQ.Text.Trim(),"");
				WebUtils.ShowMessage(this.Page,"��������ʧ�ܣ�����ԭ��" + PublicRes.GetErrorMsg(err.Message) + "��");
			}
		}
	}
}
