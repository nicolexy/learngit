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

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryDiscountCode ��ժҪ˵����
	/// </summary>
    public partial class QueryDiscountCode : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    
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

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
            string ccftno = cftNo.Text.ToString();
            string cdkno = cdkNo.Text.ToString();

            if (this.rbCftno.Checked)
            {
                if (string.IsNullOrEmpty(ccftno))
                {
                    throw new Exception("�������˺ţ�");
                }
            }
            if (this.rbCdkno.Checked)
            {
                if (string.IsNullOrEmpty(cdkno))
                {
                    throw new Exception("���������κţ�");
                }
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
                clearDT();
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

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            int rid = e.Item.ItemIndex;
            
            GetDetail(rid);
        }

        private void GetDetail(int rid)
        {
            //��Ҫע���ҳ���
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null)
            {
                lb_c1.Text = g_dt.Rows[rid]["Fbillno"].ToString();//���������Ӧ���ֵ�
                lb_c2.Text = g_dt.Rows[rid]["Fstartdate"].ToString();//����ʱ��
                lb_c3.Text = g_dt.Rows[rid]["Fuserbillno"].ToString();//�Ի��������ֵ�
                lb_c4.Text = g_dt.Rows[rid]["Fmodifytime"].ToString();//ʹ��ʱ��
                lb_c5.Text = g_dt.Rows[rid]["Fuserpay_str"].ToString();//�Ի����
                lb_c6.Text = g_dt.Rows[rid]["Fvaliddate"].ToString();//��Ч��
                lb_c7.Text = "";//�Ƿ���������
                lb_c8.Text = g_dt.Rows[rid]["Fstate_str"].ToString();//ҵ��״̬
                lb_c9.Text = "";//��ע
                
                
            }
        }

        private void clearDT()
        {
            lb_c1.Text = "";
            lb_c2.Text = "";
            lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";

            lb_c8.Text = "";
            lb_c9.Text = "";
            
        }

        private void BindData(int index)
		{
            string s_cftno = "";
            string s_cdkno = "";

            if (this.rbCftno.Checked)
            {
                s_cftno = cftNo.Text.ToString().Trim();

            }
            else if (this.rbCdkno.Checked)
            {
                s_cdkno = cdkNo.Text.ToString().Trim();
            }
            string s_status = ddlStatus.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryDiscountCode(s_cftno, s_cdkno, s_status, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
                ViewState["g_dt"] = ds.Tables[0];

                ds.Tables[0].Columns.Add("Fcdk_name", typeof(String));//������������
                ds.Tables[0].Columns.Add("Fcdkeytype_str", typeof(String));//����
                ds.Tables[0].Columns.Add("Fstate_str", typeof(String));//״̬
                ds.Tables[0].Columns.Add("Fbankid_wh", typeof(String));//��β��

                ds.Tables[0].Columns.Add("Famount_str", typeof(String));//��ֵ
                ds.Tables[0].Columns.Add("Ffeelimit_str", typeof(String));//��ͻ����
                ds.Tables[0].Columns.Add("Fuserpay_str", typeof(String));//�Ի����

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "����");
                ht1.Add("2", "��ʹ��");
                ht1.Add("3", "������");
                ht1.Add("4", "�����ɹ�");
                ht1.Add("999", "������");
                ht1.Add("0", "δʹ��");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string cdkeytype_str = "";
                    string s_cdkeyType = dr["Fcdkeytype"].ToString();
                    if (!string.IsNullOrEmpty(s_cdkeyType))
                    {
                        int key = Int32.Parse(s_cdkeyType);
                        if (key < 10000)
                        {
                            cdkeytype_str = "���ÿ������������";
                        }
                        else if (key > 10000)
                        {
                            cdkeytype_str = "��������������";
                        }
                        else
                        {
                            cdkeytype_str = "δ֪���ͣ�" + key;
                        }
                    }
                    else {
                        cdkeytype_str = "δ֪���ͣ�" + s_cdkeyType;
                    }
                    
                    //��β��
                    string s_bankid = dr["Fbankid"].ToString();
                    if (s_bankid.Length > 4) {
                        s_bankid = s_bankid.Substring(s_bankid.Length-4, 4);
                    }
                  
                    dr.BeginEdit();
                    dr["Fcdkeytype_str"] = cdkeytype_str;
                    dr["Fbankid_wh"] = s_bankid;
                    dr.EndEdit();
                }

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fstate", "Fstate_str", ht1);
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Famount", "Famount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ffeelimit", "Ffeelimit_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fuserpay", "Fuserpay_str");

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
                DataGrid1.DataSource = null;
				throw new LogicException("û���ҵ���¼��");
			}
		}

	}
}