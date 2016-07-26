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
    /// ComplainBussinessInput ��ժҪ˵����
	/// </summary>
    public partial class ComplainBussinessInput : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        public string qbussid, qpage;
        public DateTime qbegindate;
        public DateTime qenddate;

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
                    qbussid = Request.QueryString["qbussid"];
                    if (qbussid != null && qbussid != "")
                    {
                        bussId.Text = qbussid;
                        ViewState["qbussid"] = qbussid;
                    }
                    else {
                        ViewState["qbussid"] = "";
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

                    ViewState["qbegindate"] = qbegindate;
                    ViewState["qenddate"] = qenddate;

                    qpage = Request.QueryString["qpage"];
                    if (qpage != null && qpage != "")
                    {
                        ViewState["qpage"] = qpage;
                    }
                    else {
                        qpage = "1";
                        ViewState["qpage"] = qpage;
                    }
                    

                }
                else {
                    qpage = ViewState["qpage"].ToString();
                    //qbegindate = DateTime.Parse(ViewState["qbegindate"].ToString());
                    //qenddate = DateTime.Parse(ViewState["qenddate"].ToString());
                    //qbussid = ViewState["qbussid"].ToString();
                }
                
                int ipage = 1;
                if (qpage != null && qpage != "")
                {
                    ipage = int.Parse(qpage);
                }
                else {
                    qpage = "1";
                }

               // BindData(ipage);
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

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
            }
		}

        protected void Button1_Click(object sender, System.EventArgs e)
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

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            //����
            string bussid = bussId.Text.Trim();
            string stime = TextBoxBeginDate.Text;
            string etime = TextBoxEndDate.Text;
            //string spage = this.pager.CurrentPageIndex.ToString();
            Response.Redirect("ComplainBussinessDetail.aspx?qbussid=" + bussid + "&begindate=" + stime + "&enddate=" + etime + "&qpage=" + qpage);
        }

		private int GetCount()
		{
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            begindate = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            enddate = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

            string bussid = bussId.Text.Trim();
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.GetComplainBussCount(bussid, begindate, enddate);
		}

		private void BindData(int index)
		{
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            begindate = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            enddate = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

            string bussid = bussId.Text.Trim();

            //query
            qpage = index.ToString();
            qbussid = bussId.Text.Trim();
            qbegindate = begindate;
            qenddate = enddate;

            pager.RecordCount = GetCount();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

            pager.CurrentPageIndex = index;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.GetComplainBussList(bussid, begindate, enddate, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[5].FindControl("lbDel");
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                
                lb.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С�ɾ����������');";
            }
        }
        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) 
        {
            string bussid = e.Item.Cells[0].Text.Trim(); //�̻���
            switch (e.CommandName) 
            {
                case "DEL": //Del
                    ToDel(bussid);
                    break;
            }
        }

        private void ToDel(string bussid) 
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            qs.DelComplainBuss(bussid);
        }
	}
}