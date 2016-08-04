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
using System.Xml;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using System.Text;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
	/// <summary>
    /// QueryYTTrade ��ժҪ˵����
	/// </summary>
    public partial class FCAInfoQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
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
           

		}
		#endregion


		private void ValidateDate()
		{
            string spid = txtspid.Text.ToString();
            if (spid == "")
            {
                throw new Exception("�������̻���ţ�");
            }
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }
            clearDT();
            GetDetail();
        }


        private void GetDetail()
        {
            string spid = txtspid.Text.ToString();
            try
            {
                clearDT();
                ForeignCurrencyService FCBLLService = new ForeignCurrencyService();
                TemplateControl temp = this;
                string ip = this.Page.Request.UserHostAddress;
                DataSet ds = FCBLLService.MerInfoQuery(spid, "", ip);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    string name = dt.Rows[0]["mer_name"].ToString(); //�̻�����
                    lb_c1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(name));
                    lb_c2.Text = dt.Rows[0]["address"].ToString();//�̻���ַ
                    lb_c3.Text = dt.Rows[0]["sp_web"].ToString();//��վ����
                    lb_c4.Text = dt.Rows[0]["postal_code"].ToString();//�������
                    lb_c5.Text = dt.Rows[0]["area"].ToString();//��������
                    lb_c6.Text = dt.Rows[0]["phone"].ToString();//��ϵ�绰
                    lb_c7.Text = dt.Rows[0]["email"].ToString();//Email
                    lb_c8.Text = dt.Rows[0]["mobile"].ToString();//��ϵ�ֻ�
                    string tmp=dt.Rows[0]["mer_type"].ToString();//�̻�����
                    switch (tmp)
                    {
                        case "1":
                            lb_c15.Text = "����"; break;
                        case "2":
                            lb_c15.Text = " ʵ��"; break;
                        case "3":
                            lb_c15.Text = " ����"; break;
                    }
                    lb_c19.Text = tmp;
                    lb_c10.Text = dt.Rows[0]["boss_name"].ToString();//��������
                    tmp = dt.Rows[0]["boss_cre_type"].ToString();//���˴���֤������
                    tmp = PublicRes.GetCreType(tmp);
                    lb_c11.Text = tmp;
                    lb_c12.Text = dt.Rows[0]["boss_cre_no"].ToString();//����֤������
                    lb_c13.Text = dt.Rows[0]["mer_no"].ToString();//�̻�Ӫҵִ�ձ��
                    //��������ҪҪ�Ľӿ��еģ�������ǽӿڷ��ص�
                    lb_c14.Text = dt.Rows[0]["uid"].ToString();//�̻��ڲ�id
                    tmp = dt.Rows[0]["inner_sp"].ToString();//�Ƿ����ڲ��̻�
                    switch (tmp)
                    {
                        case "1":
                            lb_c15.Text = "��˾�ڲ��̻�";break;
                        case "2":
                            lb_c15.Text = " �ⲿ�̻�";break;
                    }
                    lb_c16.Text = dt.Rows[0]["company_name"].ToString();//��˾����
                    lb_c17.Text = dt.Rows[0]["country"].ToString();//����
                    lb_c18.Text = dt.Rows[0]["city"].ToString();//����
                    lb_c19.Text = dt.Rows[0]["memo"].ToString();//˵��
                    lb_c20.Text = dt.Rows[0]["inner_memo"].ToString();//�ڲ��޸ı�ע
                    lb_c21.Text = dt.Rows[0]["modify_user"].ToString();//����Ա
                    lb_c22.Text = dt.Rows[0]["bd_user"].ToString();//�̻���չbd
                    lb_c23.Text = dt.Rows[0]["modify_ip"].ToString();//IP��ַ
                    lb_c24.Text = dt.Rows[0]["create_time"].ToString();//����ʱ��
                    lb_c25.Text = dt.Rows[0]["uid"].ToString();//�޸�ʱ��
                    tmp = dt.Rows[0]["state"].ToString();//�̻�״̬
                    switch (tmp)
                    {
                        case "1":
                            lb_c26.Text = "��ʼ״̬"; break;
                        case "2":
                            lb_c26.Text = " ע�����"; break;
                    }
                    tmp = dt.Rows[0]["lstate"].ToString();//����״̬
                    switch (tmp)
                    {
                        case "1":
                            lb_c27.Text = "���� "; break;
                        case "2":
                            lb_c27.Text = " ����"; break;
                        case "3":
                            lb_c27.Text = " ����"; break;
                    }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "û�в�ѯ����¼��");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.Message));
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
            lb_c24.Text = "";
            lb_c25.Text = "";
            lb_c26.Text = "";
            lb_c27.Text = "";
            lb_c28.Text = "";
            lb_c29.Text = "";
            lb_c30.Text = "";


        }

  

	}
}