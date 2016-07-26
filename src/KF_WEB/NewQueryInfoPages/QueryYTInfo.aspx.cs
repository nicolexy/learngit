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
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryYTInfo ��ժҪ˵����
	/// </summary>
    public partial class QueryYTInfo : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            //radioListOrder.Attributes.Add("onclick", "showRadioClick()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Value = "";
                }

                if (this.rbYtno.Checked)
                {
                    detailTB.Visible = true;
                    certTB.Visible = false;
                }
                else
                {
                    certTB.Visible = true;
                    detailTB.Visible = false;
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
			//this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

	
		private void ValidateDate()
		{
			DateTime begindate;

			try
			{
                string s_date = TextBoxBeginDate.Value;
                if (s_date != null && s_date != "") {
                    begindate = DateTime.Parse(s_date);
                }
			}
			catch
			{
				throw new Exception("������������");
			}
            string ccftno = cftNo.Text.ToString();
            string cytno = ytNo.Text.ToString();
            if (ccftno == "" && cytno == "")
            {
                throw new Exception("����������һ����ѯ�");
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
                clearDetailTB();
				BindData();
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

        private void BindData()
		{
            bool isRight = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
            string s_time = TextBoxBeginDate.Value;
            string s_begindate = "";
            if (s_time != null && s_time != "") {
                DateTime begindate = DateTime.Parse(s_time);
                s_begindate = begindate.ToString("yyMM");
            }
            
            string s_cftno = cftNo.Text.ToString();
            string s_ytno = ytNo.Text.ToString();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ht = qs.GetYTInfoList(s_cftno, s_ytno, s_begindate,"1");

            if (ht != null && ht.Tables.Count>0)
            {
                DataTable dt = ht.Tables[0];
                string day_limit = classLibrary.setConfig.FenToYuan(dt.Rows[0]["day_sum_limit"].ToString());
                string single_limit = classLibrary.setConfig.FenToYuan(dt.Rows[0]["per_tran_limit"].ToString());
                string no_opt_limit = classLibrary.setConfig.FenToYuan(dt.Rows[0]["no_otp_per_tran_limit"].ToString());

                //lb_c1.Text = s_cftno;//�Ƹ�ͨ�˺�
                lb_c2.Text = dt.Rows[0]["create_time"].ToString();//�״ο�ͨʱ��
                lb_c3.Text = classLibrary.setConfig.ConvertID(dt.Rows[0]["card_id"].ToString(),6,5);//��ͨ�˺�
                string status = dt.Rows[0]["status"].ToString();//״̬
                string s_status = "";
                if (status == "1") {
                    s_status = "����";
                }
                else if (status == "2") {
                    s_status = "����";
                }
                else if (status == "3")
                {
                    s_status = "����";
                }
                else if (status == "4")
                {
                    s_status = "����";
                }
                lb_c4.Text = s_status;
                lb_c10.Text = dt.Rows[0]["address_num"].ToString();//�˵���ַ����

                status = dt.Rows[0]["otp_status"].ToString();//״̬
                s_status = "";
                if (status == "0")
                {
                    s_status = "��ʼ��";
                }
                else if (status == "1")
                {
                    s_status = "��ǩЭ��";
                }
                else if (status == "2")
                {
                    s_status = "���Э��";
                }
                else if (status == "3")
                {
                    s_status = "����";
                }

                lb_c11.Text = s_status;//�ް�ȫ����ɫͨ��
                lb_c12.Text = single_limit;//��������֧���޶�
                lb_c13.Text = no_opt_limit;//�ް�ȫ���̼ҵ�������޶�
                //lb_c14.Text = dt.Rows[0]["last_update_time"].ToString();//���һ�θ���ʱ��
                lb_c15.Text = day_limit;//����֧���޶�
                //lb_c16.Text = dt.Rows[0]["last_login_ip"].ToString();//���һ�ε�¼IP
                lb_c17.Text = "";//�������
                lb_c18.Text = "";//������

                string qqid = "";
                string uid = dt.Rows[0]["uid"].ToString();
                //string uid = "";
                if (s_cftno != null && s_cftno != "")
                {
                    //ͨ���Ƹ�ͨ�˺�ȥ��ѯ��Ϣ
                    qqid = s_cftno;
                }
                else if (uid != null && uid != "")
                {
                    qqid = qs.Uid2QQ(uid);
                }

                lb_c1.Text = qqid;//�Ƹ�ͨ�˺�
                DataSet ds = new DataSet();
                DataSet ds_acc = new DataSet();
                if (qqid != null && qqid != "")
                {

                    ds = new AccountService().GetUserInfo(qqid, 0, 1, 1);
                    ds_acc = new AccountService().GetUserAccount(qqid, 1, 1, 1);
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    //������Ϣ

                    lb_c5.Text = classLibrary.setConfig.ConvertName(ds.Tables[0].Rows[0]["Ftruename"].ToString(), isRight);   //����
                    string s_cretype = PublicRes.GetString(ds.Tables[0].Rows[0]["Fcre_type"]);//֤������
                    if (s_cretype == "1")
                    {
                        lb_c6.Text = "���֤";
                    }
                    else {
                        lb_c6.Text = "";
                    }
                    
                    lb_c7.Text = classLibrary.setConfig.ConvertTelephoneNumber(PublicRes.GetString(ds.Tables[0].Rows[0]["Fmobile"]), isRight);//�ֻ���
                    if (lb_c6.Text == "���֤")
                    {
                        lb_c8.Text = classLibrary.setConfig.IDCardNoSubstring(ds.Tables[0].Rows[0]["Fcreid"].ToString(), isRight);//֤������
                    }
                    else 
                    {
                        lb_c8.Text = ds.Tables[0].Rows[0]["Fcreid"].ToString();// classLibrary.setConfig.ConvertCreID(ds.Tables[0].Rows[0]["Fcreid"].ToString());//֤������
                    }
                   
                    lb_c9.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Femail"]);//email
                }
                if (ds_acc != null && ds_acc.Tables.Count > 0) 
                {
                    lb_c18.Text = classLibrary.setConfig.FenToYuan(ds_acc.Tables[0].Rows[0]["Fcon"].ToString());//������
                    lb_c17.Text = classLibrary.setConfig.FenToYuan((long.Parse(ds_acc.Tables[0].Rows[0]["Fbalance"].ToString()) - long.Parse(ds_acc.Tables[0].Rows[0]["Fcon"].ToString())).ToString());
                }
            }
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

        private void clearDetailTB() {
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
        }

        private void clearCertTB()
        {
            lb_c19.Text = "";
            lb_c20.Text = "";
        }

        public void btnCert_Click(object sender, System.EventArgs e)
        {

            try
            {
                string cert_no = certNo.Text.ToString();
                if (cert_no == "") 
                {
                    WebUtils.ShowMessage(this.Page, "���������֤�ţ�");
                    return;
                }
                clearCertTB();
                BindData2();
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

        private void BindData2()
        {
            string s_certno = certNo.Text.ToString();

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ht = qs.GetCertNum(s_certno,"1");

            if (ht != null && ht.Tables.Count > 0)
            {
                DataTable dt = ht.Tables[0];

                bool isRight = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
                lb_c19.Text = classLibrary.setConfig.IDCardNoSubstring(s_certno, isRight);
                lb_c20.Text = dt.Rows[0]["num"].ToString();
            }
            else
            {
                throw new LogicException("û���ҵ���¼��");
            }
        }

	}
}