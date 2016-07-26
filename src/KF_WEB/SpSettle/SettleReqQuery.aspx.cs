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
    /// SettleReqQuery ��ժҪ˵����
	/// </summary>
    public partial class SettleReqQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            // �ڴ˴������û������Գ�ʼ��ҳ��
            rq_tb2.Visible = false;
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

                if (listid.Length != 28)
                    throw new Exception("������28λ�����ţ�");
                string s_reqid = this.txtReqid.Text.Trim();

                BindInfo(listid, s_reqid);
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

        private void BindInfo(string szListid, string reqid)
        {
         
            SettleService service = new SettleService();
            DataTable dt = service.GetSettleReqList(szListid, reqid);
            if (dt != null )
            {
                ViewState["g_dt"] = dt;

                dt.Columns.Add("Ftotal_fee_str", typeof(string)); //֧�����
                dt.Columns.Add("Fsettle_fee_str", typeof(string));//���˽��
                dt.Columns.Add("Fstate_str", typeof(string)); //����״̬
                dt.Columns.Add("Fcurtype_str", typeof(string)); //����
                dt.Columns.Add("Flstate_str", typeof(string)); //��������״̬

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "����ǰ");
                ht1.Add("2", "���˳ɹ�");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "RMB");
                Hashtable ht3 = new Hashtable();
                ht3.Add("1", "����");
                ht3.Add("2", "����");

                classLibrary.setConfig.FenToYuan_Table(dt, "Ftotal_fee", "Ftotal_fee_str");
                classLibrary.setConfig.FenToYuan_Table(dt, "Fsettle_fee", "Fsettle_fee_str");

                classLibrary.setConfig.DbtypeToPageContent(dt, "Fstate", "Fstate_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Fcurtype", "Fcurtype_str", ht2);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Flstate", "Flstate_str", ht3);

                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else 
            {
                throw new Exception("û���ҵ�������"+szListid+"�ļ�¼��");
            }
            
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string listid = e.Item.Cells[0].Text.Trim(); //�Ƹ�ͨ������

            switch (e.CommandName)
            {
                case "REQDETAIL": //������ϸ
                    int rid = e.Item.ItemIndex;
                    rq_tb2.Visible = true;
                    GetReqDetail(rid);
                    break;
                case "SETDETAIL": //������ϸ
                    Response.Redirect("SettleReqDetail.aspx?listid=" + listid);
                    break;
            }
        }

        private void GetReqDetail(int rid) 
        {
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null) 
            {
                lb_c1.Text = g_dt.Rows[rid]["Fsettle_request_id"].ToString();//����������ˮ��
                lb_c2.Text = g_dt.Rows[rid]["Fsettle_list_id"].ToString();//�����ܵ�
                lb_c3.Text = g_dt.Rows[rid]["Fcoding"].ToString();//��������
                lb_c4.Text = g_dt.Rows[rid]["Flistid"].ToString();//�Ƹ�ͨ������
                lb_c5.Text = g_dt.Rows[rid]["Fpnr"].ToString();//PNR��
                lb_c6.Text = g_dt.Rows[rid]["Fcontact"].ToString();//��ϵ��
                lb_c7.Text = g_dt.Rows[rid]["Fpri_spid"].ToString();//��Ʊƽ̨ID
                lb_c8.Text = g_dt.Rows[rid]["Fflight_info"].ToString();//����
                lb_c9.Text = g_dt.Rows[rid]["Fphone"].ToString();//��ϵ�绰
                lb_c10.Text = g_dt.Rows[rid]["Fticket_num"].ToString();//��Ʊ����

                lb_c11.Text = g_dt.Rows[rid]["Fcurtype_str"].ToString();//����
                lb_c12.Text = g_dt.Rows[rid]["Fsettle_fee_str"].ToString();//�ܷ��˽��
                lb_c13.Text = g_dt.Rows[rid]["Ftotal_fee_str"].ToString();//����֧�����
                lb_c14.Text = g_dt.Rows[rid]["Fbus_type"].ToString();//ҵ�����
                lb_c15.Text = g_dt.Rows[rid]["Fbus_args"].ToString();//���˲���
                lb_c16.Text = g_dt.Rows[rid]["Fbus_desc"].ToString();//����ԭʼ����
                lb_c17.Text = g_dt.Rows[rid]["Fsp_bankurl"].ToString();//�̻��ص�URL
                lb_c18.Text = g_dt.Rows[rid]["Fstate_str"].ToString();//����״̬
                lb_c19.Text = g_dt.Rows[rid]["Flstate_str"].ToString();//��������״̬
                lb_c20.Text = g_dt.Rows[rid]["Fcreate_time"].ToString();//����ʱ��

                lb_c21.Text = g_dt.Rows[rid]["Fsettle_time"].ToString();//����ʱ��
                lb_c22.Text = g_dt.Rows[rid]["Fmodify_time"].ToString();//�޸�ʱ��
                lb_c23.Text = g_dt.Rows[rid]["Fagentid"].ToString();//��������Ϣ
            }
        }

        private void clearDT()
        {
            lb_c1.Text = "";
            lb_c2.Text = "";
            lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";
            lb_c8.Text = "";
            lb_c9.Text = "";

            lb_c10.Text = "";
            lb_c11.Text = "";
            lb_c12.Text = "";
            lb_c13.Text = "";
            lb_c14.Text = "";
            lb_c15.Text = "";
            lb_c16.Text = "";
            lb_c17.Text = "";
            lb_c18.Text = "";
            lb_c19.Text = "";

            lb_c20.Text = "";
            lb_c21.Text = "";
            lb_c22.Text = "";
            lb_c23.Text = "";
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
        }
		#endregion
	}
}
