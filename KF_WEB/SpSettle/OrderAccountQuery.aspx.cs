using System;
using System.Collections;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
	/// <summary>
    /// OrderAccountQuery ��ժҪ˵����
	/// </summary>
    public partial class OrderAccountQuery : System.Web.UI.Page
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
                string spid = this.txtSpid.Text.Trim();
                string suin = this.txtUin.Text.Trim();
                if (spid == "" && suin == "")
                    throw new Exception("��ѯ��������Ϊ�գ�");

                BindInfo(spid, suin);
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

        private void BindInfo(string spid, string suin)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds;
            ds = qs.QueryTrueLimtList(spid, suin);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                dt.Columns.Add("Flstate_str", typeof(string)); //״̬

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "����");
                ht1.Add("2", "����");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Flstate", "Flstate_str", ht1);


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
