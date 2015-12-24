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
using System.Xml;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
	/// <summary>
    /// QueryYTTrade ��ժҪ˵����
	/// </summary>
    public partial class FCRefundQuery : System.Web.UI.Page
	{
        public DateTime qbegindate, qenddate;
        protected ForeignCurrencyService FCBLLService = new ForeignCurrencyService();
        protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                }
                this.btnQuery.Attributes.Add("onclick", "return CheckEmail();");
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
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
            DateTime begindate = new DateTime(), enddate = new DateTime();

            try
            {
                string s_date = TextBoxBeginDate.Value;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Value;
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
			catch
			{
				throw new Exception("������������");
			}
            if (begindate.Year != enddate.Year)
            {
                throw new Exception("�벻Ҫ�����ѯ��");
            }
            string spid = txtspid.Text.ToString();
            string spListID = txtspListID.Text.ToString();
            string coding = txtCoding.Text.ToString();
            if (spid == "" && spListID == "" && coding == "")
            {
                throw new Exception("����������һ����ѯ�");
            }
            if (coding != "" && spid == "")
            {
                throw new Exception("�������̻���ţ�");
            }
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
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
                this.pager.RecordCount = 1000;
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

        private void BindData(int index)
        {
            try
            {
                string s_stime = TextBoxBeginDate.Value;
                string s_begindate = "";
                if (s_stime != null && s_stime != "")
                {
                    DateTime begindate = DateTime.Parse(s_stime);
                    s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
                }
                string s_etime = TextBoxEndDate.Value;
                string s_enddate = "";
                if (s_etime != null && s_etime != "")
                {
                    DateTime enddate = DateTime.Parse(s_etime);
                    s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
                }

                string spid = txtspid.Text.ToString();
                string coding = txtCoding.Text.ToString();
                string spListID = txtspListID.Text.ToString();

                int max = pager.PageSize;
                int start = max * (index - 1);
                TemplateControl temp = this;
                string ip = this.Page.Request.UserHostAddress;

                DataSet dsR = new DataSet();
                if (!string.IsNullOrEmpty(spid))
                {
                    DataSet ds = FCBLLService.MerInfoQuery(spid, "", ip);
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                        throw new Exception("��ѯ�����̻��ڲ�id��");
                    string uid = ds.Tables[0].Rows[0]["uid"].ToString();//�̻��ڲ�id
                    dsR = FCBLLService.QueryRefundBySpid(uid, coding, s_begindate, s_enddate, ddl_state.SelectedValue, start, max);
                }
                else if (!string.IsNullOrEmpty(spListID))
                {
                    dsR = FCBLLService.QueryRefundByTransId(spListID, s_begindate, s_enddate, ddl_state.SelectedValue, start, max);
                }
                if (dsR == null || dsR.Tables.Count < 1 || dsR.Tables[0].Rows.Count < 1)
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    throw new Exception("���ݿ��޴˼�¼");
                }

                dsR.Tables[0].Columns.Add("Fcreate_time_order"); //����ʱ��
                dsR.Tables[0].Columns.Add("Fpaynum_str_order"); //���׽��
                dsR.Tables[0].Columns.Add("Fsp_refund_num_str"); //�˿���
                dsR.Tables[0].Columns.Add("Frefund_state_str"); //�˿�״̬
                classLibrary.setConfig.FenToYuan_Table(dsR.Tables[0], "Fsp_refund_num", "Fsp_refund_num_str");//���׽��
                dsR.Tables[0].Columns.Add("Fprice_curtype_str"); //����
                foreach (DataRow row in dsR.Tables[0].Rows)
                {
                    string cur_type = row["Fprice_curtype"].ToString();
                    if (InternetBankDictionary.CurTypeAIMark.ContainsKey(cur_type))
                    {
                        row["Fprice_curtype_str"] = InternetBankDictionary.CurTypeAIMark[cur_type];
                    }
                    else
                    {
                        row["Fprice_curtype_str"] = cur_type;
                    }
                    row["Fsp_refund_num_str"] = row["Fprice_curtype_str"].ToString() + "-" + row["Fsp_refund_num_str"].ToString();
                    string tmp = row["Frefund_state"].ToString();
                    switch (tmp)
                    {
                        case "1":
                            row["Frefund_state_str"] = "������"; break;
                        case "2":
                            row["Frefund_state_str"] = "����������"; break;
                        case "3":
                            row["Frefund_state_str"] = "����ʧ��"; break;
                        case "4":
                            row["Frefund_state_str"] = "�˿�ɹ�"; break;
                        case "5":
                            row["Frefund_state_str"] = "�˿�ʧ��"; break;
                        case "6":
                            row["Frefund_state_str"] = "��������"; break;
                        case "7":
                            row["Frefund_state_str"] = "ת�����"; break;
                        case "8":
                            row["Frefund_state_str"] = "�ݲ�����"; break;
                        case "9":
                            row["Frefund_state_str"] = "�˿�������"; break;
                        case "10":
                            row["Frefund_state_str"] = "ת������ɹ�"; break;
                        case "11":
                            row["Frefund_state_str"] = "ת�������"; break;
                        case "99":
                            row["Frefund_state_str"] = "�˿�ع�"; break;
                        default:
                            row["Frefund_state_str"] = tmp; break;
                    }

                    string transId = row["Ftransaction_id"].ToString();//������
                    if (!string.IsNullOrEmpty(transId))
                    {
                        DataSet dsOrder = FCBLLService.QueryOrderByListId(transId);
                        if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                        {
                            dsOrder.Tables[0].Columns.Add("Fpaynum_str"); //����+���׽��
                            dsOrder.Tables[0].Columns.Add("Fprice_curtype_str");
                            classLibrary.setConfig.FenToYuan_Table(dsOrder.Tables[0], "Fpaynum", "Fpaynum_str");//���׽��
                            string cur_type2 = dsOrder.Tables[0].Rows[0]["Fprice_curtype"].ToString();
                            if (InternetBankDictionary.CurTypeAIMark.ContainsKey(cur_type2))
                            {
                                dsOrder.Tables[0].Rows[0]["Fprice_curtype_str"] = InternetBankDictionary.CurTypeAIMark[cur_type2];
                            }
                            else
                            {
                                dsOrder.Tables[0].Rows[0]["Fprice_curtype_str"] = cur_type2;
                            }
                            row["Fpaynum_str_order"] = dsOrder.Tables[0].Rows[0]["Fprice_curtype_str"].ToString() + "-" + dsOrder.Tables[0].Rows[0]["Fpaynum_str"].ToString();
                            row["Fcreate_time_order"] = dsOrder.Tables[0].Rows[0]["Fcreate_time"].ToString();
                        }
                        else
                        {
                            row["Fpaynum_str_order"] = "δ֪";
                            row["Fcreate_time_order"] = "δ֪";
                        }
                    }

                }
                DataGrid1.DataSource = dsR;
                DataGrid1.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

	}
}