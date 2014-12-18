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

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
	/// <summary>
    /// OrderRelationQuery ��ժҪ˵����
	/// </summary>
    public partial class OrderRelationQuery : System.Web.UI.Page
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            // �ڴ˴������û������Գ�ʼ��ҳ��
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
		}

        protected void Button_qry_Click(object sender, System.EventArgs e) 
        {
            try
            {
                string szlistid = this.txtSzlistid.Text.Trim();
                string sublistid = this.txtSublistid.Text.Trim();
                if (szlistid == "" && sublistid == "")
                    throw new Exception("��ѯ��������Ϊ�գ�");

                BindInfo(szlistid, sublistid);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message.ToString());
            }
        }

        private void BindInfo(string sz_listid, string sub_listid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds;
            ds = qs.QueryRelationOrderList(sz_listid, sub_listid);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                dt.Columns.Add("Ftotal_fee_str", typeof(string)); //ԭ��֧�����
                dt.Columns.Add("Fsub_total_fee_str", typeof(string)); //������
                dt.Columns.Add("Fstate_str", typeof(string)); //����״̬

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "����ɹ�");
                ht1.Add("2", "����ʧ��");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ftotal_fee", "Ftotal_fee_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fsub_total_fee", "Fsub_total_fee_str");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fstate", "Fstate_str", ht1);


                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else 
            {
                throw new Exception("û���ҵ���¼��");
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
        }
		#endregion
	}
}
