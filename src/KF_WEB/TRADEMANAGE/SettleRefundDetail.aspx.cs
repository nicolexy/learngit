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
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SettleRefundDetail ��ժҪ˵����
	/// </summary>
	public partial class SettleRefundDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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




            try
            {
                string refund_id = Request.QueryString["refund_id"];
                string listid = Request.QueryString["listid"];

                if(refund_id == null || listid == null)
                {
                    throw new Exception("�����������");
                }

                BindInfo(refund_id, listid);
            }
            catch(LogicException err)
            {
                WebUtils.ShowMessage(this.Page,err.Message);
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

        private string getListStatus(int iStatus)
        {
            string szStatus;
            switch (iStatus)
            {
                case 1:
                    szStatus = "������";
                    break;
                case 2:
                    szStatus = "�ɹ�";
                    break;
                case 3:
                    szStatus = "ʧ��";
                    break;
                default:
                    szStatus = string.Format("δ֪״̬��%d", iStatus);
                    break;

            }
            return szStatus;
        }

        private string getListType(int iType)
        {
            string szType;
            switch (iType)
            {
                case 1:
                    szType = "�����˿�";
                    break;
                case 2:
                    szType = "����";
                    break;
                case 3:
                    szType = "�ⶳ";
                    break;
                default:
                    szType = string.Format("δ֪���ͣ�%d", iType);
                    break;

            }
            return szType;
        }

        private void BindInfo(string refund_id, string listid)
        {
            LabelListId.Text = listid;
            labelRefundId.Text = refund_id;

            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds;
            //ds = qs.GetSettleRefundListDetail(refund_id, listid);
            SettleService service = new SettleService();

             DataTable dt = service.GetSettleRefundListDetail(refund_id, listid);
             if (dt == null) throw new Exception("û�в鵽����");
 
            
            dt.Columns.Add("uin",typeof(string));
            dt.Columns.Add("status",typeof(string));
            dt.Columns.Add("type",typeof(string));
            dt.Columns.Add("refund_fee",typeof(string));
            dt.Columns.Add("modify_time",typeof(string));
            dt.Columns.Add("bus_args",typeof(string));

            foreach(DataRow dr in dt.Rows)
            {
                dr["uin"] = dr["Fuin"].ToString();
                dr["status"] = getListStatus(int.Parse(dr["Fstate"].ToString()));
                dr["type"] = getListType(int.Parse(dr["Foper_type"].ToString()));
                dr["refund_fee"] = MoneyTransfer.FenToYuan(dr["Famount"].ToString());
                dr["modify_time"] = dr["Fmodify_time"].ToString();
                dr["bus_args"] = dr["Frfd_bus_args"].ToString();
            }
            this.DataGrid1.DataSource = dt.DefaultView;
            this.DataGrid1.DataBind();
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

        }
		#endregion
	}
}
