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

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
    /// ComplainUserInput ��ժҪ˵����
	/// </summary>
    public partial class ComplainUserInput : System.Web.UI.Page
	{
        public string qlistid;
        public string qbussid;
        public DateTime qbegindate;
        public DateTime qenddate;
        public string qorderid;
        public string qcomptype;
        public string qstatus;
        public string qpage;

        protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

                if (!IsPostBack)
                {
                    //TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
                    //TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");

                    qbussid = Request.QueryString["qbussid"];
                    if (qbussid != null && qbussid != "")
                    {
                        // ViewState["bussid"] = qbussid;
                        bussId.Text = qbussid;
                    }
                    else
                    {
                        // ViewState["bussid"] = "";
                    }

                    string sbegindate = Request.QueryString["begindate"];
                    if (sbegindate != null && sbegindate != "")
                    {
                        qbegindate = DateTime.Parse(sbegindate);
                        TextBoxBeginDate.Text = qbegindate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        qbegindate = Convert.ToDateTime("2013-05-28");
                        TextBoxBeginDate.Text = qbegindate.ToString("yyyy-MM-dd");
                    }
                    string senddate = Request.QueryString["enddate"];
                    if (senddate != null && senddate != "")
                    {
                        qenddate = DateTime.Parse(senddate);
                        TextBoxEndDate.Text = qenddate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        qenddate = DateTime.Now;
                        TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    //ViewState["begindate"] = DateTime.Parse(qbegindate.ToString("yyyy-MM-dd 00:00:00"));
                    //ViewState["enddate"] = DateTime.Parse(qenddate.ToString("yyyy-MM-dd 23:59:59"));

                    qorderid = Request.QueryString["orderid"];
                    if (qorderid != null && qorderid != "")
                    {
                        // ViewState["cftorderid"] = qorderid;
                        cftOrderId.Text = qorderid;
                    }
                    else
                    {
                        //ViewState["cftorderid"] = "";
                    }

                    qcomptype = Request.QueryString["comptype"];
                    if (qcomptype != null && qcomptype != "")
                    {
                        ViewState["fcomptype"] = qcomptype;
                        ddlComplainType.SelectedValue = qcomptype;
                    }
                    else
                    {
                        ViewState["fcomptype"] = "0";
                        ddlComplainType.SelectedValue = "0";
                    }
                    qstatus = Request.QueryString["status"];
                    if (qstatus != null && qstatus != "")
                    {
                        //ViewState["fstate"] = qstatus;
                        ddlComplainStatus.SelectedValue = qstatus;
                    }
                    else
                    {
                        //ViewState["fstate"] = "0";
                        ddlComplainStatus.SelectedValue = "0";
                    }
                    qpage = Request.QueryString["qpage"];
                    if (qpage != null && qpage != "")
                    {
                        ViewState["qpage"] = qpage;
                    }
                    else
                    {
                        qpage = "1";
                        ViewState["qpage"] = qpage;
                    }
                    qlistid = Request.QueryString["listid"];
                    if (qlistid != null && qlistid != "")
                    {
                        ViewState["listid"] = qlistid;
                    }
                    else {
                        ViewState["listid"] = "";
                    }

                }
                else {
                    qpage = ViewState["qpage"].ToString();
                    qlistid = ViewState["listid"].ToString();
                }

                int ipage = 1;
                if (qpage != null && qpage != "")
                {
                    ipage = int.Parse(qpage);
                }
                else
                {
                    qpage = "1";
                }
                
               
                //BindData(ipage);
              
                
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
			//pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;

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

		}

        public void Button1_Click(object sender, System.EventArgs e)
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

        public void btnNew_Click(object sender, System.EventArgs e)
        {
            //����
            string bussid = bussId.Text.Trim();
            string cftorderid = cftOrderId.Text.Trim();
            string stime = TextBoxBeginDate.Text;
            string etime = TextBoxEndDate.Text;
            string comtype = ddlComplainType.SelectedValue;
            string comstate = ddlComplainStatus.SelectedValue;
            //string spage = pager.CurrentPageIndex.ToString();

            //?bussid=" + qbussid + "&begindate=" + qbegindate + "&enddate=" + qenddate + "&orderid=" + qorderid + "&comptype=" + qcomptype + "&status=" + qstatus + "&qpage=" + qpage;
            Response.Redirect("ComplainUserDetail.aspx?qbussid=" + bussid + "&begindate=" + stime + "&enddate=" + etime + "&orderid=" + cftorderid + "&comptype=" + comtype + "&status=" + comstate + "&qpage=" + qpage);
        }

        private int GetCount()
		{
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            begindate =  DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            enddate = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

            string bussid = bussId.Text.Trim();
            string cftorderid = cftOrderId.Text.Trim();
            int comptype = int.Parse(ddlComplainType.SelectedValue);
            int compstatus = int.Parse(ddlComplainStatus.SelectedValue);


            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.GetUserComplainCount(bussid, cftorderid, comptype, compstatus, begindate, enddate);
		}

        private void BindData(int index)
		{
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            begindate = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            enddate = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

            string bussid = bussId.Text.Trim();
            string cftorderid = cftOrderId.Text.Trim();
            int comptype = int.Parse(ddlComplainType.SelectedValue);
            int compstatus = int.Parse(ddlComplainStatus.SelectedValue);

            int count = GetCount();
            pager.RecordCount = count;
            Label9.Text = count.ToString();

            pager.CurrentPageIndex = index;

            //query
            qbussid = bussId.Text.Trim();
            qorderid = cftOrderId.Text.Trim();
            qbegindate = begindate;
            qenddate = enddate;
            qcomptype = ddlComplainType.SelectedValue;
            qstatus = ddlComplainStatus.SelectedValue;
            qpage = index.ToString();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.GetUserComplainList(bussid, cftorderid, comptype, compstatus, begindate, enddate, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("Fcomp_type_str", typeof(String));
                ds.Tables[0].Columns.Add("Fstatus_str", typeof(String));
                ds.Tables[0].Columns.Add("Forder_fee_str", typeof(String));

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "���Ҫ�󲹷���");
                ht1.Add("2", "��������˿�");
                ht1.Add("3", "��Ҷ���Ʒ����������");
                ht1.Add("4", "���׾���");

                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "��֪ͨ�̻�");
                ht2.Add("2", "�Ѵ߰��̻�");
                ht2.Add("3", "�̻��Ѵ𸴽��");
                ht2.Add("4", "�ᵥ");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Forder_fee", "Forder_fee_str");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcomp_type", "Fcomp_type_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fstatus", "Fstatus_str", ht2);

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

        private void remind_Click(string listid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string msg = "";
            Query_Service.UserComplainClass cb = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.UserComplainClass();

            if (listid != "")
            {
                //�߰�
                cb.FListId = int.Parse(listid);
                if (qs.RemindUserComplain(cb, out msg))
                {
                    string bussid = bussId.Text.Trim();
                    string cftorderid = cftOrderId.Text.Trim();
                    string stime = TextBoxBeginDate.Text;
                    string etime = TextBoxEndDate.Text;
                    string comtype = ddlComplainType.SelectedValue;
                    string comstate = ddlComplainStatus.SelectedValue;
                    //string spage = pager.CurrentPageIndex.ToString();

                    string url = "ComplainUserInput.aspx?qbussid=" + bussid + "&begindate=" + stime + "&enddate=" + etime + "&orderid=" + cftorderid + "&comptype=" + comtype + "&status=" + comstate + "&qpage=" + qpage;
                    WebUtils.ShowMessageAndRedirect(this.Page, "�߰�ɹ�", url);
                    //BindData(1);
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, msg);
                }
            }
        }

	}
}