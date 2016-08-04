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
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// changeUserName_2 ��ժҪ˵����
    /// </summary>
    public partial class changeUserName_2 : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��		
            string szkey = Session["SzKey"].ToString();
            //int operid = Int32.Parse(Session["OperID"].ToString());

            //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

            if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

            this.Label1.Text = Session["OperID"].ToString();
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



        protected void Button_Update_Click(object sender, System.EventArgs e)
        {
            string url = "";
            try
            {
                //У���û���QQ�ź������Ƿ����
                string qqid = this.TX_QQID.Text.Trim();
                string name = this.txtOldName.Text.Trim();
                string newName = this.txbNewName.Text.Trim();
                string mail = this.txMail.Text.Trim();

                bool exeSign = CheckOldName(qqid, name);
                if (exeSign == false)
                {
                    WebUtils.ShowMessage(this.Page, "�Բ��� QQ�����ԭ���������� �����ύ���룡");
                    return;
                }

                //�ϴ���Ҫ��ͼƬ�������ض�Ӧ�������ϵĵ�ַ
                //����ļ�
                string alPath;
                HtmlInputFile inputFile = this.File1;

                alPath = PublicRes.upImage(inputFile, "Account");

                //string mail  = this.txMail.Text.Trim();
                string reason = this.txReason.Text.Trim();

                reason = classLibrary.setConfig.replaceSqlStr(reason);

                string fetchNo = "101" + DateTime.Now.ToString("yyyyMMddHHmmssff").ToString();  //101�޸�����
                string commTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();

                url = "fetchName.aspx?QQID=" + qqid + "&mail=" + mail + "&reason=" + TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.URLDecode(reason)
                    + "&oldName=" + TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.URLEncode(name) + "&newName="
                    + TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.URLEncode(newName)
                    + "&fetchNo=" + fetchNo + "&commTime=" + commTime + "&accPath=" + alPath + "&infoPath=" + alPath;

            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�޸������쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }

            Response.Redirect(url);
        }

        private bool CheckOldName(string qqid, string oldName)
        {
            string Msg = null;
            bool exeSign;

            //����service
            try
            {
                Finance_ManageService.Finance_Manage fs = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage();
                exeSign = fs.CheckOldName(qqid, oldName, out Msg);
                //�ж�ִ�н��
                if (exeSign == false)
                {
                    WebUtils.ShowMessage(this.Page, Msg);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                WebUtils.ShowMessage(this.Page, e.Message.ToString().Replace("'", "��"));
                return false;
            }

        }
    }

}
