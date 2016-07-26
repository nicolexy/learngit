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
using Tencent.DotNet.Common.UI;
using System.Text.RegularExpressions;
using System.IO;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// TradeMigration ��ժҪ˵����
    /// </summary>
    public class TradeMigration : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected System.Web.UI.WebControls.Label Label_uid;
        protected System.Web.UI.WebControls.Button btMigration;
        protected System.Web.UI.WebControls.Label labErrMsg;
        protected System.Web.UI.WebControls.Label lblUId;
        protected System.Web.UI.WebControls.TextBox txtTradeId;
        protected System.Web.UI.WebControls.FileUpload File1;
        //protected System.Web.UI.WebControls.TextBox txtTradeId;sayidԭ����������tradeId����һ�е�order�滻�˾Ͷ���
        protected System.Web.UI.WebControls.RegularExpressionValidator rfvNum;

        private void Page_Load(object sender, System.EventArgs e)
        {
           
            try
            {
                lblUId.Text = Session["uid"].ToString();
                string sr = Session["SzKey"].ToString();
                if (!ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch  //���û�е�½����û��Ȩ�޾�����
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
            this.btMigration.Click += new System.EventHandler(this.btMigration_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void btMigration_Click(object sender, System.EventArgs e)
        {

            string tradeId = txtTradeId.Text.Trim();
            if (!SunLibraryEX.StringEx.IsNumber(tradeId) || !SunLibraryEX.StringEx.MatchLength(tradeId, 0, 32))
            {
                labErrMsg.Text = "���׵������󣬽��׵��ű���ΪС�ڵ���32λ���֣�";
                WebUtils.ShowMessage(this.Page, labErrMsg.Text);
                return;
            }
            string msg = "";
            if (!MigrationCheck(tradeId, out  msg))
            {
                labErrMsg.Text = "�����׵�Ǩ������ʧ�ܣ�ʧ����Ϣ����:<br>" + msg;
                WebUtils.ShowMessage(this.Page, "�����׵�Ǩ������ʧ�ܣ�ʧ����Ϣ���£�" + PublicRes.GetErrorMsg(msg));
                return;
            }
            else
            {
                labErrMsg.Text = "�����׵�Ǩ����������ɹ�!";
                WebUtils.ShowMessage(this.Page, labErrMsg.Text);

            }
        }

        private void BatchMigrate(out string msgWrong)
        {
            msgWrong = "";
            try
            {
                string path = Server.MapPath("~/") + "PLFile" + "\\TradeMigration.xls";
                File1.PostedFile.SaveAs(path);

                DataSet res_ds = PublicRes.readXls(path,"F1");
                DataTable res_dt = res_ds.Tables[0];
                int iColums = res_dt.Columns.Count;
                int iRows = res_dt.Rows.Count;

                 
                for (int i = 0; i < iRows; i++)
                {
                    string r1 = res_dt.Rows[i][0].ToString().Trim();//���׵���
                    try
                    {
                       
                        if (!SunLibraryEX.StringEx.IsNumber(r1) || !SunLibraryEX.StringEx.MatchLength(r1, 0, 32))
                        {
                            msgWrong += "  " + r1 + ": ���׵������󣬽��׵��ű���ΪС�ڵ���32λ���֣�" + "</br></br>";
                            continue;
                        }

                        string msgErr = "";
                        if (!MigrationCheck(r1, out  msgErr))
                        {
                            msgWrong += "  " + r1 + ": ʧ����Ϣ����:" + msgErr + "</br></br>";
                            continue;
                        }
                    }
                    catch(Exception ex)
                    {
                        msgWrong += "  " + r1 + ": ����Ǩ���쳣:" + PublicRes.GetErrorMsg(ex.Message) + "</br></br>";
                        continue;
                    }
                }
            }
            catch (Exception eSys)
            {
                msgWrong+="����Ǩ���쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString());
            }
        }

        public void btnBatch_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!File1.HasFile)
                {
                    WebUtils.ShowMessage(this.Page, "��ѡ���ϴ��ļ���");
                    return;
                }
                if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
                {
                     string msgWrong="";
                     BatchMigrate(out msgWrong);
                     if (msgWrong != "")
                     {
                         labErrMsg.Text = "����Ǩ������ʧ�ܣ�ʧ����Ϣ����:<br>" + msgWrong;
                         WebUtils.ShowMessage(this.Page, "�����׵�Ǩ������ʧ�ܣ�ʧ����ϸ��Ϣ��ҳ�棡");
                         return;
                     }
                     else
                     {
                         labErrMsg.Text = "����Ǩ����������ɹ�!";
                         WebUtils.ShowMessage(this.Page, labErrMsg.Text);
                         return;
                     }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "�ļ���ʽ����ȷ����ѡ��xls��ʽ�ļ��ϴ���");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); return;
            }
        }

        //����soapͷ��Ϣ
        private ZWCheck_Service.Finance_Header SetWebServiceHeader(TemplateControl page)
        {

            ZWCheck_Service.Finance_Header header = new ZWCheck_Service.Finance_Header();
            //header.SrcUrl = page.Page.Request.Url.ToString();
            header.UserIP = page.Page.Request.UserHostAddress;
            header.UserName = (page.Page.Session["uid"] == null) ? "" : page.Page.Session["uid"].ToString();
            //header.SessionID = page.Page.Session.SessionID;
            header.SzKey = (page.Page.Session["SzKey"] == null) ? "" : page.Page.Session["SzKey"].ToString();
            header.OperID = (page.Page.Session["OperID"] == null) ? 0 : Int32.Parse(page.Page.Session["OperID"].ToString());
            header.RightString = (page.Page.Session["SzKey"] == null) ? "" : page.Page.Session["SzKey"].ToString();
            return header;
        }
        private bool MigrationCheck(string tradeId, out string msg)
        {
            msg = "";
            try
            {
                //������ϵͳ�ӿ�
                ZWCheck_Service.Check_Service checkService = new ZWCheck_Service.Check_Service();
                ZWCheck_Service.Param[] parameters = new ZWCheck_Service.Param[1];
                parameters[0] = new ZWCheck_Service.Param();
                parameters[0].ParamName = "MsgId";
                parameters[0].ParamValue = tradeId;
                checkService.Finance_HeaderValue = SetWebServiceHeader(this);
                checkService.StartCheck(tradeId, "TradeMigration", "���׵�Ǩ������", "0", parameters);

                return true;
            }
            catch (Exception ex)
            {
                msg += PublicRes.GetErrorMsg(ex.Message);
                return false;
            }

        }
    }
}
