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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;


namespace TENCENT.OSS.CFT.KF.KF_Web.settleRefund
{
	/// <summary>
	/// settleRefund ��ժҪ˵����
	/// </summary>
	public partial class settleRefund : System.Web.UI.Page
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if(!IsPostBack)
            {
                this.rtnList.Checked = true;
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.pager_PageChanged);

        }
		#endregion


        public void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);        
        }

        private string getRefundStatusStr(int iStatus, int iFreezeFstatus)
        {
            string strStatus;
            switch(iStatus)
            {
                case 4:
                    strStatus="�˿�ɹ�";
                    break;
                case 5:
                    strStatus="���˻���ʧ��";
                    break;
                case 6:
                    strStatus="�渶�˿�ɹ�";
                    break;
                case 12:
                    strStatus="���˻�����";
                    break;
                case 13:
                    strStatus="���ʻ��˳ɹ�";
                    break;
                case 14:
                    strStatus="���˻��˳ɹ���b2c�˿���";
                    break;
                case 15:
                    strStatus="b2c�˿�ɹ������˻�����";
                    break;
                case 16:
                    strStatus="�渶�˿���";
                    break;
                case 17:
                    strStatus="�渶�˿�ɹ������˻���ʧ��";
                    break;
                case 18:
                    strStatus="���˻��˳ɹ���b2c�˿�ʧ��";
                    break;
                case 19:
                    strStatus="�渶�˿�ʧ��";
                    break;
                case 20:
                switch(iFreezeFstatus)
                {
                    case 0:
                        strStatus="��ʼ״̬";
                        break;
                    case 1:
                        strStatus="����������";
                        break;
                    case 2:
                        strStatus="����ɹ�";
                        break;
                    case 3:
                        strStatus="����ʧ��";
                        break;
                    case 4:
                        strStatus="�ⶳ������";
                        break;
                    case 5:
                        strStatus="�ⶳ�ɹ�";
                        break;
                    default:
                        strStatus = string.Format("����ⶳ����״̬��%d", iFreezeFstatus);
                        break;
                }
                    break;
                default:
                    strStatus = string.Format("%d", iStatus);
                    break;

            }
            return strStatus;
        }

        private void BindData(int index)
        {
            int max = pager.PageSize;
            int start = max * (index-1);

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds;
            ds = qs.GetSettleRefundList(ViewState["Flistid"].ToString(), int.Parse(ViewState["query_type"].ToString()), start, max);
            
            DataTable dt = ds.Tables[0];

            
            dt.Columns.Add("listid",typeof(string));
            dt.Columns.Add("refund_id",typeof(string));
            dt.Columns.Add("modify_time",typeof(string));
            dt.Columns.Add("rp_fee",typeof(string));
            dt.Columns.Add("status",typeof(string));


            foreach(DataRow dr in dt.Rows)
            {
                dr["listid"] = dr["Ftransaction_id"].ToString();
                dr["refund_id"] = dr["Fdraw_id"].ToString();
                dr["modify_time"] = dr["Fmodify_time"].ToString();
                dr["rp_fee"] = MoneyTransfer.FenToYuan(dr["Frp_fee"].ToString());
                dr["status"] = getRefundStatusStr(int.Parse(dr["Fstatus"].ToString()), int.Parse(dr["Ffreeze_state"].ToString()));

            }
            this.DataGrid1.DataSource = dt.DefaultView;
            this.DataGrid1.DataBind();
        }

        private void checkData()
        {
            if(this.rtnList.Checked)
            {
                if(this.txtFlistid.Text.Trim().Length != 28)
                    throw new Exception("������28λ�����ţ�");
                else
                    ViewState["Flistid"] = this.txtFlistid.Text.Trim();

                //���ݶ�����ѯ
                ViewState["query_type"] = 1;
            }
            else if(this.rtnRefefundList.Checked)
            {
                if(this.txtRefefundList.Text.Trim().Length != 28)
                    throw new Exception("������28λ�˿����뵥�Ż򶳽ᵥ�ţ�");
                else
                    ViewState["Flistid"] = this.txtRefefundList.Text.Trim();

                //�����˿��ѯ
                ViewState["query_type"] = 2;
            }
            else
            {
                throw new Exception("δ֪�쳣");
            }
        }


        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                pager.RecordCount= 10000;
                checkData();
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
	}
}
