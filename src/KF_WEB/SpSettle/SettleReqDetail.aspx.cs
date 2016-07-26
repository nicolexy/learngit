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
    /// SettleReqDetail ��ժҪ˵����
	/// </summary>
    public partial class SettleReqDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
            if (!IsPostBack) 
            {
                string listid = Request.QueryString["listid"];
                if (listid != null && !string.IsNullOrEmpty(listid.Trim()))
                {
                    try
                    {
                        if (listid.Length != 28)
                            throw new Exception("������28λ�����ţ�");
                        listid = listid.Trim();
                        this.txtListid.Text = listid;
                        BindInfo(listid);
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
            }
            
		}

        protected void Button_qry_Click(object sender, System.EventArgs e) 
        {
            try
            {
                string listid = this.txtListid.Text.Trim();

                if (listid.Length != 28)
                    throw new Exception("������28λ�����ţ�");
                

                BindInfo(listid);
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

        private void BindInfo(string szListid)
        {
       
            SettleService service = new SettleService();
            DataTable dt = service.GetSettleReqInfo(szListid);

            if (dt != null )
            {
                dt.Columns.Add("Fstate_str", typeof(string)); //����״̬
                dt.Columns.Add("Fsettle_num_str", typeof(string)); //���˽��

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "����ǰ");
                ht1.Add("2", "���˳ɹ�");

                classLibrary.setConfig.FenToYuan_Table(dt, "Fsettle_num", "Fsettle_num_str");

                classLibrary.setConfig.DbtypeToPageContent(dt, "Fstate", "Fstate_str", ht1);

                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else 
            {
                throw new Exception("û���ҵ�������"+szListid+"�ļ�¼��");
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
