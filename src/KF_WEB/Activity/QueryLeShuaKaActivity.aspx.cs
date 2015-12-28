using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.ActivityModule;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.KF_Web.Activity
{
	/// <summary>
    /// QueryLeShuaKaActivity ��ժҪ˵����
	/// </summary>
    public partial class QueryLeShuaKaActivity : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    
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
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr + ", stacktrace" + eSoap.StackTrace);
			}
			catch(Exception eSys)
			{
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString() + ", stacktrace" + eSys.StackTrace);
			}
		}

        private void BindData(int index)
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

            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, s_enddate);

			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("FBitMaskStr", typeof(String));//�������
                ds.Tables[0].Columns.Add("FStateStr", typeof(String));//״̬

                Hashtable ht1 = new Hashtable();
                ht1.Add("0", "����ȡ");
                ht1.Add("1", "��ȡ��");
                ht1.Add("2", "��ȡ�ɹ���δ����");
                ht1.Add("3", "���ͽ�Ʒ��");
                ht1.Add("4", "��ȡ�ɹ����ѵ��ˣ���չʾ");
                ht1.Add("5", "��ȡ�ɹ����ѵ��ˣ���Ȼչʾ");

                Hashtable ht2 = new Hashtable();
                ht2.Add("caipiao_sli", "��Ʊ��Ĭ�û�");
                ht2.Add("credit_new", "���ÿ����û�");
                ht2.Add("non_fastpay", "�ǿ���û�");
                ht2.Add("paipai_mc_silent", "���Ļ��ѳ�ֵ��Ĭ�û�");
                ht2.Add("cft_mc_silent", "�Ƹ�ͨ���ѳ�ֵ��Ĭ�û�");
                ht2.Add("yixun_silent", "��Ѹ��Ĭ�û�");
                ht2.Add("weixin_pay", "΢��֧���û�");
                ht2.Add("dianping_new", "�������û�");
                ht2.Add("huanledou_new", "���ֶ����û�");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "FState", "FStateStr", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "FBitMask", "FBitMaskStr", ht2);

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
			}
		}

	}
}