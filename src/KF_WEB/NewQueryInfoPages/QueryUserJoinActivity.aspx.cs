using CFT.CSOMS.BLL.TransferMeaning;
using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryUserJoinActivity ��ժҪ˵����
	/// </summary>
    public partial class QueryUserJoinActivity : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
                    TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
                    
                    setConfig.GetActivityList(ddlActId);
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
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
            DateTime begindate, enddate;

            try
            {
                string s_date = TextBoxBeginDate.Text;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Text;
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
			catch
			{
				throw new Exception("������������");
			}
            string cft_no = txtCftNo.Text.Trim();

            if (cft_no == "")
            {
                throw new Exception("�Ƹ�ͨ�˺Ų���Ϊ�գ�");
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
            
            string s_stime = TextBoxBeginDate.Text;
            string s_begindate = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
            }
            string s_etime = TextBoxEndDate.Text;
            string s_enddate = "";
            if (s_etime != null && s_etime != "")
            {
                DateTime enddate = DateTime.Parse(s_etime);
                s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
            }

            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryUserJoinActivity(cft_no, s_begindate, s_enddate, act_id, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("Feffective_date", typeof(String));//��Ч��
                ds.Tables[0].Columns.Add("Fstate_str", typeof(String));//����״̬
                ds.Tables[0].Columns.Add("Fjoin_str", typeof(String));//�����ʸ�
                ds.Tables[0].Columns.Add("Fjoin_res", typeof(String));//������
                ds.Tables[0].Columns.Add("Ffail", typeof(String));//ʧ��ԭ��
                ds.Tables[0].Columns.Add("Fprize_name", typeof(String));//��Ʒ

                /*
                Hashtable ht1 = new Hashtable();
                ht1.Add("100", "�����");
                ht1.Add("101", "�ɹ�");
                ht1.Add("102", "ʧ��");
                ht1.Add("200", "�齱��");
                ht1.Add("201", "�齱�ɹ�");
                ht1.Add("202", "�齱ʧ��");
                ht1.Add("301", "������");
                ht1.Add("302", "�����ɹ�");
                ht1.Add("303", "����ʧ��");
                */

                foreach (DataRow dr in ds.Tables[0].Rows) 
                {
                    string s_bdate = dr["FBeginDate"].ToString();
                    string s_edate = dr["FEndDate"].ToString();
                    string s_state = dr["FState"].ToString();
                    if (s_state == "100" || s_state == "102")
                    {
                        dr["Fjoin_str"] = "��";
                        dr["Fstate_str"] = "ʧ��";
                        dr["Ffail"] = dr["FPrizeInfo"].ToString();
                    }
                    else {
                        dr["Fjoin_str"] = "��";
                        dr["Fstate_str"] = "�ɹ�";
                    }
                    if (s_state == "302")
                    {
                        dr["Fjoin_res"] = "�ѻ�";
                    }
                    else
                    {
                        dr["Fjoin_res"] = "δ��";
                    }


                    dr["Feffective_date"] = s_bdate + "��" + s_edate;
                    dr["Fprize_name"] = Transfer.returnDicStr("FPrizeType", dr["FPrizeType"].ToString());
                }

                //classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "FState", "Fstate_str", ht1);

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
                throw new Exception("��¼������");
			}
		}

	}
}