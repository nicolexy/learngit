using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;
using System.Collections;
using CFT.CSOMS.BLL.WechatPay;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
	/// <summary>
    /// AddedValueTicketQuery ��ժҪ˵����yinhuang 2014/06/18
	/// </summary>
    public partial class AddedValueTicketQuery : System.Web.UI.Page
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
            string ccftno = cftNo.Text.ToString().Trim();
            string wxno = wxNo.Text.ToString().Trim();
            if (string.IsNullOrEmpty(ccftno) && string.IsNullOrEmpty(wxno))
            {
                throw new Exception("������Ƹ�ͨ�˺Ż���΢��֧���˺ţ�");
            }
            if ((!string.IsNullOrEmpty(ccftno)) && (!string.IsNullOrEmpty(wxno)))
            {
                throw new Exception("��ֻ����Ƹ�ͨ�˺Ż���΢��֧���˺�����һ����ѯ��");
            }

            string acc_id = "";
            int acc_type = 0;
            if (!string.IsNullOrEmpty(ccftno))
            {
                acc_id = ccftno;
                acc_type = 1;
            }
            if (!string.IsNullOrEmpty(wxno))
            {
                acc_id = wxno;
                acc_type = 2;
            }
            ViewState["acc_id"] = acc_id;
            ViewState["acc_type"] = acc_type;
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
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
			}
		}

        private void BindData(int index)
		{
            //string cft_no = cftNo.Text.ToString();
            //string wxno = wxNo.Text.ToString();


            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new WechatPayService().QueryAddedValueTicket((int)ViewState["acc_type"], ViewState["acc_id"].ToString(), ddlState.SelectedValue, "", "", start, max);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count >0)
			{
                //ds.Tables[0].Columns.Add("txn_amt_str", typeof(String));

                //Hashtable ht1 = new Hashtable();
                //ht1.Add("11", "����");

                //classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "txn_dist", "txn_dist_str", ht1);


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