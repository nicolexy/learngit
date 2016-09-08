using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// BusinessFreeze ��ժҪ˵����
	/// </summary>
	public partial class BusinessResume : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					this.btnFreeze.Attributes["onClick"] = "if(!confirm('ȷ��Ҫ�ָ����̻���')) return false;"; 
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
		}
		#endregion

		protected void btnFreeze_Click(object sender, System.EventArgs e)
		{
            try 
            {              
                btnFreeze_Ex();
            }
            catch (SoapException eSoap)
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message);
            }
		}

        private void btnFreeze_Ex() 
        {
            try
            {
                string strszkey = Session["SzKey"].ToString().Trim();
                int ioperid = Int32.Parse(Session["OperID"].ToString());
                int iserviceid = TENCENT.OSS.CFT.KF.Common.AllUserRight.GetServiceID("InfoCenter");
                string struserdata = Session["uid"].ToString().Trim();
                string content = struserdata + "ִ����[�̻��ָ�]����,��������[" + this.txtFspid.Text.Trim()
                    + "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Common.AllUserRight.UpdateSession(strszkey, ioperid, PublicRes.GROUPID, iserviceid, struserdata, content);

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            try
            {
                if (this.txtFspid.Text.Trim() == "")
                {
                    throw new Exception("�������̻���!");
                }
                if (this.txtReason.Text.Trim() == "")
                {
                    throw new Exception("������ԭ��!");
                }
                if (!this.cbxOpenFreeze.Checked && !this.cbxOpenPay.Checked && !this.cbxOpenAgent.Checked)
                {
                    throw new Exception("��ѡ�����ѡ��!");
                }
                
                string log = SensitivePowerOperaLib.MakeLog("edit", Session["uid"].ToString().Trim(), "[�̻��ָ�]",
                    this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());

                if (!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", log, this))
                {

                }

                bool IsOpenFreeze = false;//�ָ�����͹�ʧ
                bool IsOpenPay = false; //��֧ͨ��
                bool IsOpenAgent = false; //��ͨ�н�
                if (this.cbxOpenFreeze.Checked)
                    IsOpenFreeze = true;
                if (this.cbxOpenPay.Checked)
                    IsOpenPay = true;
                if (this.cbxOpenAgent.Checked)
                    IsOpenAgent = true;

                //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //qs.BusinessResume(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());

                if (IsOpenFreeze)
                {
                    //�ָ�����͹�ʧ����spoa�ӿ�
                    SPOA_Service.SPOA_Service spoaService = new TENCENT.OSS.CFT.KF.KF_Web.SPOA_Service.SPOA_Service();
                    string spoa_ret = spoaService.RestoreOfSpid(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                    if (spoa_ret == "0")
                    {

                    }
                    else
                    {
                        throw new Exception("�ָ�����͹�ʧ����" + spoa_ret);
                    }
                }
                if (IsOpenPay)
                {
                    //��֧ͨ������spoa�ӿ�
                    SPOA_Service.SPOA_Service spoaService = new TENCENT.OSS.CFT.KF.KF_Web.SPOA_Service.SPOA_Service();
                    string spoa_ret = spoaService.OpenPay(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                    if (spoa_ret == "0")
                    {

                    }
                    else
                    {
                        throw new Exception("��֧ͨ������" + spoa_ret);
                    }
                }
                if (IsOpenAgent)
                {
                    //��ͨ�н飬��spoa�ӿ�
                    SPOA_Service.SPOA_Service spoaService = new TENCENT.OSS.CFT.KF.KF_Web.SPOA_Service.SPOA_Service();
                    string spoa_ret = spoaService.OpenAgency(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                    if (spoa_ret == "0")
                    {

                    }
                    else
                    {
                        throw new Exception("��ͨ�н����" + spoa_ret);
                    }
                }

                WebUtils.ShowMessage(this.Page, "�����ɹ�");
            }
            catch (SoapException eSoap)
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                throw new Exception(errStr);
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message);
            }
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            RemaindDiv.Visible = false;
            Table2.Visible = true;
            try
            {
                string spid = this.txtFspid.Text;
                if (string.IsNullOrEmpty(spid))
                {
                    throw new Exception("�������̻���!");
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.QueryBussFreezeList(spid.Trim(), "13",""); //�̻��ָ�
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("amendstate_str", typeof(String));//״̬
                    ds.Tables[0].Columns.Add("amendtype_str", typeof(String));//״̬

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("0", "�ȴ���������");
                    ht1.Add("1", "�ȴ��쵼����");
                    ht1.Add("-1", "�ȴ��������");
                    ht1.Add("11", "�ȴ���Ч��");
                    ht1.Add("2", "�ȴ���������");
                    ht1.Add("-2", "�ȴ���Ʒ���");
                    ht1.Add("3", "����ͨ��");
                    ht1.Add("-3", "�ȴ��ͷ�����");
                    ht1.Add("4", "��������");
                    ht1.Add("7", "���뱻���");
                    ht1.Add("8", "�ȴ���С�̻������");
                    ht1.Add("9", "�ȴ�ָ�������");

                    Hashtable ht2 = new Hashtable();
                    ht2.Add("13", "�̻��ָ�");

                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "amendstate", "amendstate_str", ht1);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "amendtype", "amendtype_str", ht2);

                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
            }
            catch (SoapException eSoap)
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message);
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string s = e.Item.Cells[7].Text;
            txtApplyResult.Text = s;
        }

        private string QueryResumeMsg() 
        {
            string msg = "";
            try
            {
                string spid = this.txtFspid.Text;
                if (string.IsNullOrEmpty(spid))
                {
                    throw new Exception("�������̻���!");
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.QueryBussFreezeList(spid.Trim(), "12","");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    msg += "�̻�����";
                }
                ds = qs.QueryBussFreezeList(spid.Trim(), "41", "");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    msg += "��֧������";
                }
                ds = qs.QueryBussFreezeList(spid.Trim(), "52", "");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    msg += "���н鹦��";
                }
            }
            catch (SoapException eSoap)
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                throw new Exception(errStr);
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message);
            }
            return msg;
        }
	}
}
