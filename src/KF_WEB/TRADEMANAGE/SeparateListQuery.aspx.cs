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
namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SeparateListQuery ��ժҪ˵����
	/// </summary>
	public partial class SeparateListQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Web.UI.WebControls.DataGrid dgListFlist;
	
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
				this.rtnList.Checked = true;
				TextBoxBeginDate.Text = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).ToString("yyyy-MM-dd");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void CheckData()
		{
			ViewState["IsrtnList"] = true;

			if(this.rtnList.Checked)
			{
				if(this.txtFlistid.Text.Trim().Length != 28)
					throw new Exception("������28λ�����ţ�");
				else
					ViewState["Flistid"] = this.txtFlistid.Text.Trim();
			}
			else if(this.rtbSpid.Checked)
			{
				ViewState["IsrtnList"] = false;

				DateTime begindate;
				DateTime enddate;
				try
				{
					begindate = DateTime.Parse(TextBoxBeginDate.Text);
					enddate = DateTime.Parse(TextBoxEndDate.Text);
					ViewState["begindate"] = begindate;
					ViewState["enddate"] = enddate;
				}
				catch
				{
					throw new Exception("������������");
				}

				if(begindate.CompareTo(enddate) > 0)
				{
					throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
				}

				if(begindate.Year != enddate.Year || begindate.Month != enddate.Month)
				{
					throw new Exception("��������²�ѯ��");
				}

				if(this.txtFspid.Text.Trim() == "")
					throw new Exception("�������̻��ţ�");
				else
					ViewState["Fspid"] = this.txtFspid.Text.Trim();
			}
			else
				throw new Exception("��ѡ��һ�ֲ�ѯ��ʽ��");
		}

		private void BindData(int index)
		{
			int max = pager.PageSize;
			int start = max * (index-1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds;
			if(ViewState["IsrtnList"].ToString() == "True")
			{
				ds = qs.GetSeparateListForFlistid(ViewState["Flistid"].ToString(),start,max);
				this.DataGrid1.Columns[2].HeaderText = "����ʱ��";
			}
			else
			{
				ds = qs.GetSeparateListForFspid(ViewState["Fspid"].ToString(),DateTime.Parse(ViewState["begindate"].ToString()),DateTime.Parse(ViewState["enddate"].ToString()),start,max);
				this.DataGrid1.Columns[2].HeaderText = "֧��ʱ��";
			}

			DataTable dt = ds.Tables[0];
			//dt.Columns.Add("Time",typeof(string));
			dt.Columns.Add("FpaynumStr",typeof(string));
			dt.Columns.Add("Fsttl_feeStr",typeof(string));
			dt.Columns.Add("Frefund_feeStr",typeof(string));
			dt.Columns.Add("Frf_feeStr",typeof(string));
			dt.Columns.Add("Foffer_feeStr",typeof(string));
			dt.Columns.Add("Fadjustin_feeStr",typeof(string));
			dt.Columns.Add("Fadjustout_feeStr",typeof(string));

			foreach(DataRow dr in dt.Rows)
			{
				//dr["Time"] = DateTime.Parse(dr["TimeStr"].ToString());
				dr["FpaynumStr"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
				dr["Fsttl_feeStr"] = MoneyTransfer.FenToYuan(dr["Fsttl_fee"].ToString());
				dr["Frefund_feeStr"] = MoneyTransfer.FenToYuan(dr["Frefund_fee"].ToString());
				dr["Frf_feeStr"] =  MoneyTransfer.FenToYuan(dr["Frf_fee"].ToString());
				dr["Foffer_feeStr"] = MoneyTransfer.FenToYuan(dr["Foffer_fee"].ToString());
				if(dr["Foffer_sign"].ToString() == "2")//Foffer_sign:1����2��
					dr["Foffer_feeStr"] = "-" + dr["Foffer_feeStr"].ToString();
				dr["Fadjustin_feeStr"] =  MoneyTransfer.FenToYuan(dr["Fadjustin_fee"].ToString());
				dr["Fadjustout_feeStr"] =  MoneyTransfer.FenToYuan(dr["Fadjustout_fee"].ToString());
				//Fadjustin_fee��Fadjustout_fee

				/*Foffer_fee���������ɵ�
				Fadjustin_fee��Fadjustout_fee������һ������������˵����������
				Fadjustin_fee��Fadjustout_fee��Ϊ��û��*/
                string spid = dr["Flistid"].ToString().Substring(0, 10);
                if (!PublicRes.isWhiteOfSeparate(spid, Session["uid"].ToString())) 
                {
                    dr.BeginEdit();
                    string s_fcoding = dr["Fcoding"].ToString();
                    dr["Fcoding"] = classLibrary.setConfig.ConvertID(s_fcoding, 0, 4);
                    dr.EndEdit();
                }
			}
			this.DataGrid1.DataSource = dt.DefaultView;
			this.DataGrid1.DataBind();

		}
		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				CheckData();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				pager.RecordCount= 10000;
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

	}
}
