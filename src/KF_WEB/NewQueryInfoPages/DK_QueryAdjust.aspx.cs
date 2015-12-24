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
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// DK_QueryAdjust ��ժҪ˵����
	/// </summary>
	public partial class DK_QueryAdjust : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                if (!Page.IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("DKAdjust", this)) Response.Redirect("../login.aspx?wh=1");
                    
                    this.TextBoxBeginDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    this.TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

                    classLibrary.setConfig.GetAllBankList(ddlbanktype);
                    ddlbanktype.Items.Insert(0, new ListItem("��������", "0000"));
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

		}
		#endregion

		protected void btQuery_Click(object sender, System.EventArgs e)
		{
			//���汾�����������ļ��У���ҳ�棬ICE���ã����ݿ⡣
			//��������ѯ���ݡ�
            string starttime = DateTime.Parse(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00");
            string endtime = DateTime.Parse(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59");
			string banktype = ddlbanktype.SelectedValue;
			string spid = txbMerchant.Text.Trim();
			string coding = txbOrder.Text.Trim();
			string bank_list = txbMoney.Text.Trim();
			string bankaccno = txtBankOrder.Text.Trim();
			string uname = Textbox1.Text.Trim();

			Query_Service.Query_Service fs = new Query_Service.Query_Service();

			DataSet ds = fs.DK_QueryCheckDetailInfo(starttime,endtime,banktype,spid,coding,bank_list,bankaccno,uname);

			if(ds != null && ds.Tables.Count>0 && ds.Tables[0] != null)
			{
				ds.Tables[0].Columns.Add("Fbank_typename");
					ds.Tables[0].Columns.Add("Fpaynumname");
					ds.Tables[0].Columns.Add("Fendstatename");
					ds.Tables[0].Columns.Add("Fcheckstatename");

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();
					dr["Fbank_typeName"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
					dr["Fpaynumname"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());

					if(dr["Fendstate"].ToString() == "9")
					{
						dr["Fendstatename"] = "����ʧ��";
					}
					else
						dr["Fendstatename"] = "�����ɹ�";

					string tmp = dr["Fcheckstate"].ToString();
					if(tmp == "0")
					{
						dr["Fcheckstatename"] = "����������";
					}
					else if(tmp == "1")
					{
						dr["Fcheckstatename"] = "�ѷ�������";
					}
					else if(tmp == "2")
					{
						dr["Fcheckstatename"] = "������ִ��";
					}
					else if(tmp == "3")
					{
						dr["Fcheckstatename"] = "�����ѳ���";
					}
						
					dr.EndEdit();
				}
				

				dgInfo.DataSource = ds.Tables[0].DefaultView;
				dgInfo.DataBind();
			}
		}
	}
}
