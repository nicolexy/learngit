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
using Tencent.DotNet.Common.UI;
using System.Text;
using TENCENT.OSS.CFT.KF.Common;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
    /// DepositAccountQuery ��ժҪ˵����
	/// </summary>
    public partial class DepositAccountQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected System.Web.UI.WebControls.Button btnQuery;
        protected System.Web.UI.WebControls.TextBox txtSPId;
        protected System.Web.UI.WebControls.Label lblOperatorId;
        protected System.Web.UI.WebControls.Repeater rptAccountList;
        protected System.Web.UI.WebControls.TextBox txtDepositAccountId;


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
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion


        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //���µ�½
                Response.End();
            }

            this.lblOperatorId.Text = Session["uid"].ToString();
        }


        private void BindInfo()
        {
            string msg = string.Empty;

            Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            myService.Finance_HeaderValue = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.setFH(Session["uid"].ToString(), Request.UserHostAddress);

            try
            {
                string uid = "0";
                if (txtSPId.Text.Trim().Length <= 0)
                {
                    WebUtils.ShowMessage(this.Page, "�̻���Ϊ������");
                    return;
                }

                string insureUIn = null;
                if (txtDepositAccountId.Text.Trim().Length > 0)
                    insureUIn = txtDepositAccountId.Text.Trim();

                myService.GetMerchantMidUid(txtSPId.Text.Trim(), out uid, out msg);
                //��ѯ��֤���˻�
                DataSet ds = myService.GetInsureAccount(uid, insureUIn, out msg);

                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    WebUtils.ShowMessage(this.Page, "��ѯ������¼");
                    return;
                }

                ds.Tables[0].Columns.Add("balance");
                ds.Tables[0].Columns.Add("ratio");
                ds.Tables[0].Columns.Add("Finsure_type_str");
                ds.Tables[0].Columns.Add("Finsure_add_type_str");

                //ѭ����ѯÿ����¼��Ӧ�ı�֤���˻����
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        dr["Finsure_conf_amount"] = MoneyTransfer.FenToYuan(dr["Finsure_conf_amount"].ToString());
                        dr["balance"] = MoneyTransfer.FenToYuan(myService.GetUserBalanceByUId(dr["Finsure_uid"].ToString(), 1, out msg));

                        if (dr["Finsure_conf_amount"].ToString() == string.Empty || decimal.Parse(dr["Finsure_conf_amount"].ToString()) <= 0)
                        {
                            dr["ratio"] = "--";
                        }
                        else
                        {
                            dr["ratio"] = string.Format("{0:f}%", decimal.Parse(dr["balance"].ToString()) * 100 / decimal.Parse(dr["Finsure_conf_amount"].ToString()));
                        }

                        dr["Finsure_type_str"] = ConvertInsureType(dr["Finsure_type"]);
                        dr["Finsure_add_type_str"] = ConvertAddType(dr["Finsure_add_type"]);
                    }
                    catch
                    {
                        dr["Finsure_conf_amount"] = "--";
                        dr["balance"] = "--";
                        dr["ratio"] = "--";
                        dr["Finsure_type_str"] = "--";
                        dr["Finsure_add_type_str"] = "--";
                    }
                }


                this.rptAccountList.DataSource = ds.Tables[0];
                this.rptAccountList.DataBind();

            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }


        private void btnQuery_Click(object sender, System.EventArgs e)
        {
            BindInfo();
        }


        public string ConvertInsureType(object insureTypeCode)
        {
            string strResult = "--";

            try
            {
                switch (int.Parse(insureTypeCode.ToString()))
                {
                    case 1:
                        strResult = "�̻��̶���֤��";
                        break;
                    case 2:
                        strResult = "�⿨ѭ����֤��";
                        break;
                    case 3:
                        strResult = "���ձ�֤��";
                        break;
                    case 101:
                        strResult = "�⿨�����˻�";
                        break;
                }
            }
            catch { throw; }

            return strResult;
        }


        public string ConvertAddType(object addTypeCode)
        {
            StringBuilder sbResult = new StringBuilder();

            try
            {
                int addtype = int.Parse(addTypeCode.ToString());
                if ((addtype & 1) > 0) sbResult.Append(" �����ڿ� ");
                if ((addtype & 2) > 0) sbResult.Append(" ��ֵ���� ");
            }
            catch
            {
                sbResult.Append("--");
            }

            return sbResult.ToString();
        }

	}
}



