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

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
	/// <summary>
    /// AdjustQuery ��ժҪ˵����
	/// </summary>
    public partial class AdjustQuery : System.Web.UI.Page
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
                string listid = this.txtListid.Text.Trim();
                string orderid = this.txtOrderid.Text.Trim();
                string spid = this.txtSpid.Text.Trim();
                if (listid == "" && orderid == "" && spid == "") 
                {
                    throw new Exception("�������ѯ������");
                }
                string adjust_time = TextBoxBeginDate.Value.Trim();
                if (spid != "" && adjust_time == "") 
                {
                    throw new Exception("��ѡ��������ڣ�");
                }
                if (adjust_time != "") 
                {
                    DateTime begindate = DateTime.Parse(adjust_time);
                    adjust_time = begindate.ToString("yyyy-MM-dd");
                }
                
                BindInfo(listid, orderid, spid, adjust_time);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        private void BindInfo(string szListid, string orderid, string spid, string adjust_time)
        {
            SettleService service = new SettleService();
            DataTable dt = service.QueryAdjustList(szListid, orderid, spid, adjust_time);

            if (dt != null  )
            {
                dt.Columns.Add("Fnum_str", typeof(string)); //���ʽ��
                dt.Columns.Add("Ftype_str", typeof(string));//��������
                dt.Columns.Add("Fstatus_str", typeof(string)); //����״̬

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "����");
                ht1.Add("2", "����");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "��ʼ״̬");
                ht2.Add("2", "������");
                ht2.Add("3", "������");
                ht2.Add("4", "����ɹ�");
                ht2.Add("5", "����ʧ��");

                classLibrary.setConfig.FenToYuan_Table(dt, "Fnum", "Fnum_str");

                classLibrary.setConfig.DbtypeToPageContent(dt, "Ftype", "Ftype_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Fstatus", "Fstatus_str", ht2);

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
