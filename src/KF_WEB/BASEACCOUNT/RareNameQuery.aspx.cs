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
using System.Web.Mail;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// SysBulletinManage ��ժҪ˵����
    /// </summary>
    public partial class RareNameQuery : System.Web.UI.Page
    {
        private string add = "1";
        private string change = "3";
        private string delete = "2";

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
               
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

        private void BindData()
        {
            try
            {
                string outmsg = "";
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                DataSet ds = qs.RareNameQuery(this.txtCardNo.Text.Trim());

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                {
                    WebUtils.ShowMessage(this.Page, "û�ж�ȡ�����ݣ�" + PublicRes.GetErrorMsg(outmsg));
                    return;
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }

        }
        private void operation(string op_type)
        {
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                Query_Service.T_RareName_INFO rareName = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_RareName_INFO();
                rareName.op_type = op_type;
                rareName.card_no = this.card_no.Text.Trim();
                rareName.account_name = this.account_name.Text.Trim();
                rareName.user_type = this.ddlUserType.SelectedValue;
                rareName.modify_type = this.ddlModifyType.SelectedValue;
                rareName.card_state = this.ddlCardState.SelectedValue;
                rareName.modify_username = Session["uid"].ToString();
                qs.AddOneRareName(rareName);
                this.tableDetail.Visible = false;
                BindData();
                WebUtils.ShowMessage(this.Page, "�����ɹ���");
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }

        }

        private void Show(DataGridItem item)
        {
            try
            {
                this.tableDetail.Visible = true;
                this.Button4.Text = "�༭";
                this.card_no.Text = item.Cells[4].Text.Trim();
                this.account_name.Text = item.Cells[5].Text.Trim();
                this.ddlUserType.SelectedValue = item.Cells[0].Text.Trim();
                this.ddlModifyType.SelectedValue = item.Cells[2].Text.Trim();
                this.ddlCardState.SelectedValue = item.Cells[1].Text.Trim();
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void Delete(DataGridItem item)
        {
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                Query_Service.T_RareName_INFO rareName = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_RareName_INFO();
                rareName.op_type = delete;
                rareName.card_no = item.Cells[4].Text.Trim();
                rareName.account_name = item.Cells[5].Text.Trim();
                rareName.user_type = item.Cells[0].Text.Trim();
                rareName.modify_type = item.Cells[2].Text.Trim();
                rareName.card_state = item.Cells[1].Text.Trim();
                rareName.modify_username = Session["uid"].ToString();
                qs.AddOneRareName(rareName);
                BindData();
                WebUtils.ShowMessage(this.Page, "ɾ���ɹ���");
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "ɾ��ʧ�ܣ�" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "ɾ��ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

       
        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim();

            switch (e.CommandName)
            {
                case "CHANGE": //�޸�
                    Session["op_type"] = change;
                    Show(e.Item);
                    break;
                case "DEL": //ɾ��������¼
                    Session["op_type"] = delete;
                    Delete(e.Item);
                    break;
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            BindData();
        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            this.tableDetail.Visible = true;
            this.Button4.Text = "����";
            this.card_no.Text = "";
            this.account_name.Text = "";
            Session["op_type"] = add;
        }

        protected void operation_Click(object sender, System.EventArgs e)
        {
            this.tableDetail.Visible = true;
           // ��������(1,������3�޸�)
            operation(Session["op_type"].ToString());
        }

    }
}
