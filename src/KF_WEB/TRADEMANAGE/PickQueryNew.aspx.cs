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
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PickQuery ��ժҪ˵����
	/// </summary>
    public partial class PickQueryNew : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		public string  begintime = DateTime.Now.ToString("yyyy-MM-dd");
		public string endtime = DateTime.Now.ToString("yyyy-MM-dd");
        PickService pickservice = new PickService();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
                    TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-01");
					TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

					classLibrary.setConfig.GetAllBankList(ddlBankType);
					ddlBankType.Items.Insert(0,new ListItem("��������","0000"));

                    PublicRes.GetCashTypeList(ddlCashType);
                    ddlCashType.Items.Insert(0, new ListItem("��������", "0000"));

					Table2.Visible = false;				
				}
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
            catch (Exception ef) {
                LogError("TradeManage.PickQueryNew", "public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)", ef);
                WebUtils.ShowMessage(this.Page, "���÷������" + ef.Message);
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

            if (u_ID != null && u_ID.Trim() != "")
            {
                if (begindate.AddDays(30).CompareTo(enddate) < 0)
                {
                    throw new Exception("ѡ��ʱ��γ�������ʮ�죬���������룡");
                }
            }
            else
            {
                if (begindate.AddDays(3).CompareTo(enddate) < 0)
                {
                    throw new Exception("ѡ��ʱ��γ��������죬���������룡");
                }
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

			//furion 20060324 �������в�ѯ����
			ViewState["banktype"] = ddlBankType.SelectedValue;

			tbQQID.Text = u_ID;
			//furion 20070129 ����������ʺŵĲ�ѯ 0Ϊ��ͨ�ʺ�,1Ϊ�����ʺ�
			ViewState["idtype"] = ddlIDType.SelectedValue.Trim();
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
            {
                LogError("TradeManage.PickQueryNew", "protected void Button2_Click(object sender, System.EventArgs e)", err);
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				Table2.Visible = true;
				pager.RecordCount= 10000; 
				BindData(1);
			}
			catch(SoapException eSoap) //����soap���쳣
            {
                LogError("TradeManage.PickQueryNew", "protected void Button2_Click(object sender, System.EventArgs e),����soap���쳣:", eSoap);
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
            {
                LogError("TradeManage.PickQueryNew", "protected void Button2_Click(object sender, System.EventArgs e),��ȡ����ʧ��:", eSys);
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
			}
		}

		private void BindData(int index)
		{
			string u_ID = ViewState["uid"].ToString();
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			begintime = begindate.ToString("yyyy-MM-dd");
			endtime = enddate.ToString("yyyy-MM-dd");

			float fnum = float.Parse(ViewState["fnum"].ToString());
			int fstate = Int32.Parse(ViewState["fstate"].ToString());

			//furion ��������
			string banktype = ViewState["banktype"].ToString();
			//furion ���������ʺ� 0Ϊ�ʺ�,1Ϊ�����ʺ�
			int idtype = Int32.Parse(ViewState["idtype"].ToString());

            //yinhuang 2013/8/1
            string cash_type = ddlCashType.SelectedValue;

			string sorttype = ViewState["sorttype"].ToString();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

            DataSet ds = pickservice.GetPickList(idtype, u_ID, begindate, enddate, fstate, fnum, banktype, sorttype, cash_type, start, max);
			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0)
			{
				ds.Tables[0].Columns.Add("FNewNumFlag",typeof(String));
                ds.Tables[0].Columns.Add("FaBankID_str", typeof(String)); //�����˺�
                ds.Tables[0].Columns.Add("Fabank_type_str", typeof(String)); //��������
                ds.Tables[0].Columns.Add("Fbank_type_str", typeof(String)); //��������
                ds.Tables[0].Columns.Add("Fsign_str", typeof(String)); //��Ʊ
                ds.Tables[0].Columns.Add("Fcharge_str", typeof(String)); //������
                ds.Tables[0].Columns.Add("Fnum_str", typeof(String)); //���ֽ��
	
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fcharge", "Fcharge_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fnum", "Fnum_str");

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
                    dr["FaBankID_str"] = classLibrary.setConfig.ConvertID(dr["FaBankID"].ToString(), 4, 4);
                    if (PublicRes.GetString(dr["Fsign"]) == "7")
                    {
                        dr["Fsign_str"] = "��";
                    }
                    else {
                        dr["Fsign_str"] = "��";
                    }
                    dr["Fabank_type_str"] = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(dr["Fabank_type"].ToString());
                    dr["Fbank_type_str"] = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(dr["Fbank_type"].ToString());
                   
				}

				ds.Tables[0].Columns.Add("FStateName",typeof(String));
                classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fsign","FStateName","TCLIST_SIGN");

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
                throw new Exception("û���ҵ���¼��");
			}
		}
	}
}