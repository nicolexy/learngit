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

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
    /// QueryDiscountCode ��ժҪ˵����
	/// </summary>
    public partial class AutomaticRechargeQuery : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    this.pager.RecordCount = 1000;
                    this.pager2.RecordCount = 1000;
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
            this.pager2.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage2);

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}
        public void ChangePage2(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager2.CurrentPageIndex = e.NewPageIndex;
            GetDetailList((int)ViewState["rid"], e.NewPageIndex);
        }

		private void ValidateDate()
		{
            string cftno = cftNo.Text.ToString();
          
                if (string.IsNullOrEmpty(cftno))
                {
                    throw new Exception("������Ƹ�ͨ�˺ţ�");
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

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            int rid = e.Item.ItemIndex;
            
            GetDetailList(rid,1);
        }

        private void GetDetailList(int rid, int index)
        {
            try
            {
                pager2.CurrentPageIndex = index;
                DataTable g_dt = (DataTable)ViewState["g_dt"];
                if (g_dt != null)
                {
                    string plan_id = g_dt.Rows[rid]["plan_id"].ToString();
                    string uin = g_dt.Rows[rid]["withhold_uin"].ToString();
                    int max = pager2.PageSize;
                    int start = max * (index - 1);
                    ViewState["rid"] = rid;

                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                    DataSet ds = qs.QueryAutomaticRechargeBillList(uin, plan_id, start, max);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("bill_amount_str", typeof(String));//��ֵ���
                        ds.Tables[0].Columns.Add("pay_amount_str", typeof(String));//֧�����
                        ds.Tables[0].Columns.Add("FstateName", typeof(String));//����״̬
                        ds.Tables[0].Columns.Add("trans_id_url", typeof(String));//������
                        DataTable dtAll = null;
                        dtAll = ds.Tables[0].Clone();

                        classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "bill_amount", "bill_amount_str");
                        classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "pay_amount", "pay_amount_str");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string state = dr["bill_status"].ToString();
                            switch (state)
                            {
                                case "0":
                                    dr["FstateName"] = "��ѯǷ�ѳɹ�";
                                    break;
                                case "1":
                                    dr["FstateName"] = "���ɽ��׵��ɹ�";
                                    break;
                                case "2":
                                    dr["FstateName"] = "֧���ɹ�";
                                    break;
                                case "3":
                                    dr["FstateName"] = "������(֪ͨ�̻�����ǰ����)";
                                    break;
                                case "4":
                                    dr["FstateName"] = "���ʳɹ�";
                                    break;
                                case "5":
                                    dr["FstateName"] = "����ʧ��";
                                    break;
                                case "6":
                                    dr["FstateName"] = "�˿�ɹ�";
                                    break;
                                case "7":
                                    dr["FstateName"] = "���׹ر�";
                                    break;
                            }

                            string transId = dr["trans_id"].ToString();
                            dr["trans_id_url"] = "<a href=" + "../TradeManage/TradeLogQuery.aspx?id=" + transId + " target=_blank >" + transId + "</a>";

                            if (dr["bill_state"].ToString() == "1")
                            {
                                dtAll.ImportRow(dr);
                            }
                        }
                        this.div2.Visible = true;
                        DataGrid2.DataSource = dtAll.DefaultView;
                        DataGrid2.DataBind();
                    }
                    else
                    {
                        this.div2.Visible = false;
                        DataGrid2.DataSource = null;
                        DataGrid2.DataBind();
                        WebUtils.ShowMessage(this, "��ѯ���Ϊ��");
                        return;
                    }
                }
            } 
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }



        private void BindData(int index)
        {
            try
            {
                this.div2.Visible = false;
                pager.CurrentPageIndex = index;

                string s_cftno = cftNo.Text.ToString().Trim();               
                int max = pager.PageSize;
                int start = max * (index - 1);

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                DataSet ds = qs.QueryAutomaticRecharge(s_cftno, start, max);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("threshold_amount_str", typeof(String));//��Ͷ��
                    ds.Tables[0].Columns.Add("bankType", typeof(String));//�ۿʽ
                    DataTable dtAll = null;
                    dtAll = ds.Tables[0].Clone();
                    ViewState["g_dt"] = ds.Tables[0];

                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "threshold_amount", "threshold_amount_str");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["bankType"] = "δ֪";
                        if (dr["sign_state"].ToString() == "2")
                        {
                            //�ۿʽ��ѯ 
                            DataSet dsKK = qs.GetBankTypeKK(dr["withhold_uin"].ToString(), dr["withhold_uid"].ToString());
                            if (dsKK != null && dsKK.Tables.Count > 0 && dsKK.Tables[0].Rows.Count > 0)
                            {
                                string Fbank_type = dsKK.Tables[0].Rows[0]["Fbank_type"].ToString();
                                dr["bankType"] = classLibrary.getData.GetBankNameFromBankCode(Fbank_type);
                            }
                            dtAll.ImportRow(dr);
                        }
                    }
                    this.div1.Visible = true;
                    DataGrid1.DataSource = dtAll.DefaultView;
                    DataGrid1.DataBind();
                    return;
                }
                else
                {
                    this.div1.Visible = false;
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    WebUtils.ShowMessage(this, "��ѯ���Ϊ��");
                    return;
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }

        }
   
    
    
    }
}