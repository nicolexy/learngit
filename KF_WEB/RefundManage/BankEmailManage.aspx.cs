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
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using CFT.CSOMS.BLL.RefundModule;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
    /// QueryYTTrade ��ժҪ˵����
	/// </summary>
    public partial class BankEmailManage : System.Web.UI.Page
	{
        protected BankEmailService sysService = new BankEmailService();
        protected void Page_Load(object sender, System.EventArgs e)
		{

			try
			{

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                    this.pager.RecordCount = 1000;
                    this.pagerContacts.RecordCount = 1000;
                    BindData(1);
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
            this.DataGridGroup.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGridGroup_ItemCommand);
            this.pagerContacts.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePagerContacts);
            this.DataGridContacts.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGridContacts_ItemCommand);
		}
		#endregion

        private void BindData(int index)
        {
            try
            {
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);
                DataSet ds= new DataSet();
                ds = sysService.QueryAllContactsGroup(start, max);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    DataGridGroup.DataSource = null;
                    DataGridGroup.DataBind();
                    throw new Exception("���ݿ��޷����¼");
                }

                DataGridGroup.DataSource = ds;
                DataGridGroup.DataBind();
                TableContacts.Visible = false;
                TableAddOne.Visible = false;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�����ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        public void btnAddGroup_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.groupName.Text.Trim()))
                {
                    WebUtils.ShowMessage(this.Page, "������������"); return;
                }
                sysService.AddContactsGroup(this.groupName.Text.Trim(), Session["uid"].ToString());
                BindData(1);
                WebUtils.ShowMessage(this.Page, "��ӷ���ɹ���");
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        private void DataGridGroup_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim();
            string groupName = e.Item.Cells[1].Text.Trim();
            ViewState["groupId"] = fid;
            ViewState["groupName"] = groupName;

            switch (e.CommandName)
            {
                case "DeleteGroup": //ɾ�����������¼
                    DeleteGroup(fid);
                    break;
                case "QueryOneGroup": //�鿴��������ϵ��
                    QueryOneGroup(fid);
                    break;
            }
        }

        private void DeleteGroup(string fid)
        {

            try
            {
                sysService.DelContactsGroup(fid,Session["uid"].ToString());
                BindData(1);
                WebUtils.ShowMessage(this.Page, "ɾ������ɹ���");
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "ɾ������ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        private void QueryOneGroup(string fid,int index=1)
        {

            try
            {
                TableContacts.Visible = true;
                TableAddOne.Visible = false;
                this.conGroupName.Text = ViewState["groupName"].ToString();
                this.pagerContacts.CurrentPageIndex = index;
                int max = pagerContacts.PageSize;
                int start = max * (index - 1);
                DataSet ds = new DataSet();
                ds = sysService.QueryOneGroupContacts(fid,start, max);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    DataGridContacts.DataSource = null;
                    DataGridContacts.DataBind();
                    throw new Exception("���ݿ��޸����Ա��¼");
                }

                DataGridContacts.DataSource = ds;
                DataGridContacts.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ���Ա����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        public void btnAddContacts_Click(object sender, System.EventArgs e)
        {
            this.TableAddOne.Visible = true;
            this.txtName.Text = "";
            this.txtEmail.Text = "";
        }

        private void DataGridContacts_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim();

            switch (e.CommandName)
            {
                case "DeleteContacts": //ɾ��������Ա��¼
                    DeleteContacts(fid);
                    break;
            }
        }

        private void DeleteContacts(string fid)
        {

            try
            {
                sysService.DelContacts(fid, Session["uid"].ToString());
                QueryOneGroup(ViewState["groupId"].ToString());
                WebUtils.ShowMessage(this.Page, "ɾ����Ա�ɹ���");
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "ɾ����Աʧ�ܣ�"+ PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        public void btnAddOne_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtName.Text.Trim()) || string.IsNullOrEmpty(this.txtEmail.Text.Trim()))
                {
                    WebUtils.ShowMessage(this.Page, "���������Ƽ��ʼ���ַ��"); return;
                }
                sysService.AddContacts(ViewState["groupId"].ToString(), this.txtName.Text.Trim(), this.txtEmail.Text.Trim(), Session["uid"].ToString());
                WebUtils.ShowMessage(this.Page, "����³�Ա�ɹ���");
                this.TableAddOne.Visible = false;
                QueryOneGroup(ViewState["groupId"].ToString());
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "����³�Աʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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
                    string path = Server.MapPath("~/") + "PLFile" + "\\bankemail.xls";
                    File1.PostedFile.SaveAs(path);

                    DataSet res_ds = PublicRes.readXls(path);
                    DataTable res_dt = res_ds.Tables[0];
                    int iColums = res_dt.Columns.Count;
                    int iRows = res_dt.Rows.Count;
                    for (int i = 0; i < iRows; i++)
                    {
                        string r1 = res_dt.Rows[i][0].ToString().Trim();//����
                        string r2 = res_dt.Rows[i][1].ToString().Trim();//email
                        if (string.IsNullOrEmpty(r1) || string.IsNullOrEmpty(r2))
                        {
                            //���ƻ����ʼ���ַΪ�գ���������������
                           continue;
                        }
                        sysService.AddContacts(ViewState["groupId"].ToString(), r1.Trim(), r2.Trim(), Session["uid"].ToString());
                    }
                    WebUtils.ShowMessage(this.Page, "���������³�Ա�ɹ���");
                    this.TableAddOne.Visible = false;
                    QueryOneGroup(ViewState["groupId"].ToString());
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

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        public void ChangePagerContacts(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pagerContacts.CurrentPageIndex = e.NewPageIndex;
            QueryOneGroup(ViewState["groupId"].ToString(), e.NewPageIndex);
        }

	}
}