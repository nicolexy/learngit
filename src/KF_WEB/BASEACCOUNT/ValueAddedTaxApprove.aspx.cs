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
	/// ValueAddedTaxApprove ��ժҪ˵����
	/// </summary>
	public partial class ValueAddedTaxApprove : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Web.UI.WebControls.TextBox txtCompanyName;
		protected System.Web.UI.WebControls.Label lblSpid;
		protected System.Web.UI.WebControls.Label lblCompanyName;
		protected System.Web.UI.WebControls.Label lblTaxInvoiceFlag;
		protected System.Web.UI.WebControls.Label lblTaxerType;
		protected System.Web.UI.WebControls.Label lblTaxInvoiceType;
		protected System.Web.UI.WebControls.Label lblTaxerCompanyName;
		protected System.Web.UI.WebControls.Label lblTaxerID;
		protected System.Web.UI.WebControls.Label lblTaxerBasebankName;
		protected System.Web.UI.WebControls.Label lblTaxerBaseBankAcct;
		protected System.Web.UI.WebControls.Label lblTaxerReceiverName;
		protected System.Web.UI.WebControls.Label lblTaxerReceiverAddr;
		protected System.Web.UI.WebControls.Label lblTaxerReceiverPostalCode;
		protected System.Web.UI.WebControls.Label lblTaxerUserType;
		protected System.Web.UI.WebControls.Label lblTaxerReceiverPhone;
		protected System.Web.UI.WebControls.TextBox txtTaxInvoiceMemo;
		protected System.Web.UI.WebControls.Button btnAllModify;
		protected System.Web.UI.WebControls.Button btnLittleModify;
		protected System.Web.UI.WebControls.Panel PanelDetail;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

				if(!classLibrary.ClassLib.ValidateRight("DrawAndApprove",this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					ViewState["uid"] = Session["uid"].ToString();
				}
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(pager.CurrentPageIndex);
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

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				pager.RecordCount = GetCount();
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

			return 100;
		}

		private void BindData(int index)
		{
			string filter = ViewState["filter"].ToString();

			int TopCount = pager.PageSize;
			int NotInCount = TopCount * (index-1);

            string strFlag = null;
            if (this.ddlFlag.SelectedValue != "")
            {
                if (this.ddlFlag.SelectedValue == "2|3")
                {
                    strFlag = "2,3";
                }
                else
                {
                    strFlag = this.ddlFlag.SelectedValue;
                }
            }

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.GetApplyValueAddedTax(txtSpid.Text.Trim(),strFlag,TopCount,NotInCount);

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("ApplyTypeStr",typeof(string));
				ds.Tables[0].Columns.Add("FlagStr",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					if(dr["ApplyType"].ToString() == "0")
                    {
                        dr["ApplyTypeStr"] = "��������";
                    }
					else if(dr["ApplyType"].ToString() == "1")
                    {
                        dr["ApplyTypeStr"] = "ȫ���޸�����";
                    }
                    else if (dr["ApplyType"].ToString() == "49")
                    {
                        dr["ApplyTypeStr"] = "spoa����";
                    }
                    else
                    {
                        dr["ApplyTypeStr"] = dr["ApplyType"].ToString();
                    }
						

					if(dr["Flag"].ToString() == "1")
						dr["FlagStr"] = "����������Ȩ����ϴ�";
					else if(dr["Flag"].ToString() == "2")
						dr["FlagStr"] = "�������������";
					else if(dr["Flag"].ToString() == "3")
						dr["FlagStr"] = "ȫ���޸������";
					else if(dr["Flag"].ToString() == "4")
						dr["FlagStr"] = "���ͨ��";
					else if(dr["Flag"].ToString() == "5")
						dr["FlagStr"] = "��˲�ͨ��";
					else if(dr["Flag"].ToString() == "6")
						dr["FlagStr"] = "�ռ�����Ϣ�޸���";
					else if(dr["Flag"].ToString() == "7")
						dr["FlagStr"] = "�ռ�����Ϣ�޸ĳɹ�";
					else if(dr["Flag"].ToString() == "8")
						dr["FlagStr"] = "�ռ�����Ϣ�޸�ʧ��";
					else if(dr["Flag"].ToString() == "9")
						dr["FlagStr"] = "���̻��ύȫ���޸�����";
					else if(dr["Flag"].ToString() == "10")
						dr["FlagStr"] = "ȫ���޸���Ȩ����ϴ�";
                    else if (dr["Flag"].ToString() == "11")
                        dr["FlagStr"] = "spoa���";
					else 
						dr["FlagStr"] = dr["Flag"].ToString();
				}
				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

		#region ��ò�ѯ�ַ���
		private string GetfilterString()
		{
			string filter = " a.taskid = b.taskid ";
            string filterNew = " ";

            if (this.txtSpid.Text.Trim() != "")
            {
                filter += " and Spid='" + this.txtSpid.Text.Trim() + "' ";
                filterNew += " and Spid='" + this.txtSpid.Text.Trim() + "' ";
            }

			if(this.ddlFlag.SelectedValue != "")
			{
                if (this.ddlFlag.SelectedValue == "2|3")
                {
                    filter += " and (Flag = 2 or Flag = 3) ";
                    filterNew += " and (Flag = 2 or Flag = 3) ";
                }
                else
                {
                    filter += " and Flag = " + this.ddlFlag.SelectedValue + " ";
                    filterNew += " and Flag = " + this.ddlFlag.SelectedValue + " ";
                }
			}
            return filterNew;
		}

		#endregion

	}
}
