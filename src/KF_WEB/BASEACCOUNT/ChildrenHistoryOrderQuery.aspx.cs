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
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
    /// ChildrenHistoryOrderQuery ��ժҪ˵����
	/// </summary>
    public partial class ChildrenHistoryOrderQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        public DateTime qbegindate, qenddate;

        protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("PayManagement", this)) Response.Redirect("../login.aspx?wh=1");

                    string sbegindate = Request.QueryString["qbegindate"];
                    if (sbegindate != null && sbegindate != "")
                    {
                        qbegindate = DateTime.Parse(sbegindate);
                        TextBoxBeginDate.Value = qbegindate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        qbegindate = DateTime.Now.AddDays(-30);
                        TextBoxBeginDate.Value = qbegindate.ToString("yyyy-MM-dd");
                    }
                    string senddate = Request.QueryString["qenddate"];
                    if (senddate != null && senddate != "")
                    {
                        qenddate = DateTime.Parse(senddate);
                        TextBoxEndDate.Value = qenddate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        qenddate = DateTime.Now;
                        TextBoxEndDate.Value = qenddate.ToString("yyyy-MM-dd");
                    }
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
                begindate = DateTime.Parse(TextBoxBeginDate.Value);
                enddate = DateTime.Parse(TextBoxEndDate.Value);
			}
			catch
			{
				throw new Exception("������������");
			}
            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
            }
            TimeSpan span = enddate.Subtract(begindate);
            int dayDiff = span.Days;
            if (dayDiff > 30) {
                throw new Exception("ֻ�ܲ�ѯһ�����������ʷ��¼��");
            }

            string cft_no = tbCft.Text;
            if (cft_no == "") {
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
            string s_stime = TextBoxBeginDate.Value;
            DateTime begindate = DateTime.Parse(s_stime);
            string s_etime = TextBoxEndDate.Value;
            DateTime enddate = DateTime.Parse(s_etime);

            string cft_no = tbCft.Text.ToString().Trim();
            string no_type = ddlType.SelectedValue.ToString();

            int max = pager.PageSize;
            int start = 1 + max * (index - 1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.GetChildrenBankRollList(cft_no, begindate, enddate, no_type, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("Faction_type_str", typeof(String)); //��������
                ds.Tables[0].Columns.Add("Fcurtype_str", typeof(String)); //��������
                ds.Tables[0].Columns.Add("Ftype_str", typeof(String)); //��������
                ds.Tables[0].Columns.Add("Fsubject_str", typeof(String)); //���/��Ŀ
                ds.Tables[0].Columns.Add("Fpaynum_str", typeof(String)); //���
                ds.Tables[0].Columns.Add("Fbalance_str", typeof(String)); //�˻����

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "Fpaynum_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fbalance", "Fbalance_str");

                foreach (DataRow dr in ds.Tables[0].Rows) 
                {
                    string s_actiontype = Transfer.convertActionType(dr["Faction_type"].ToString());
                    string s_curtype = Transfer.convertMoney_type(dr["Fcurtype"].ToString());
                    string s_type = Transfer.convertTradeType(dr["Ftype"].ToString());
                    string s_subject = Transfer.convertSubject(dr["Fsubject"].ToString());

                    dr.BeginEdit();
                    dr["Faction_type_str"] = s_actiontype;
                    dr["Fcurtype_str"] = s_curtype;
                    dr["Ftype_str"] = s_type;
                    dr["Fsubject_str"] = s_subject;
                    dr.EndEdit();
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

	}
}