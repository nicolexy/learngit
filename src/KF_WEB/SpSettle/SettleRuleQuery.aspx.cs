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
    /// SettleRuleQuery ��ժҪ˵����
	/// </summary>
    public partial class SettleRuleQuery : System.Web.UI.Page
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            // �ڴ˴������û������Գ�ʼ��ҳ��
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());
                BalanceRollPager.RecordCount = 10000;
                BalanceRollPager.CurrentPageIndex = 1;
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
                string spid = this.txtSpid.Text.Trim();
                if (spid == "")
                    throw new Exception("�̻��Ų���Ϊ�գ�");

                BindInfo(spid, 0, 10);
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

        private void BindInfo(string spid, int offset, int limit)
        {
            SettleService settleService = new SettleService();
            DataTable dt = settleService.QuerySettleRuleList(spid, offset, limit);
            if (dt != null)
            {
                dt.Columns.Add("Fcharge_method_str", typeof(string)); //�շѷ�ʽ
                dt.Columns.Add("Fstate_str", typeof(string)); //��¼״̬
                dt.Columns.Add("Fsettle_type_str", typeof(string)); //ҵ������

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "ԭ����ʱ��ȡ");
                ht1.Add("2", "��֧����");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "����");
                ht2.Add("2", "����");
                Hashtable ht3 = new Hashtable();
                ht3.Add("0", "Ĭ��");
                ht3.Add("1", "ί�д���");

                classLibrary.setConfig.DbtypeToPageContent(dt, "Fcharge_method", "Fcharge_method_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Fstate", "Fstate_str", ht2);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Fsettle_type", "Fsettle_type_str", ht3);

                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else
            {
                throw new Exception("û���ҵ���¼��");
            }
        }

        protected void BalanceRollPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                this.BalanceRollPager.CurrentPageIndex = e.NewPageIndex;

                BindInfo(this.txtSpid.Text.Trim(), e.NewPageIndex,10);
               
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
