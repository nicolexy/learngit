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
using CFT.CSOMS.BLL.SysManageModule;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Text;

namespace TENCENT.OSS.CFT.KF.KF_Web.SysManage
{
	/// <summary>
    /// QueryYTTrade ��ժҪ˵����
	/// </summary>
    public partial class BulletinSendMail : System.Web.UI.Page
	{
        private string title;
        private static List<string> listGroupId = new List<string>();
        Hashtable htGroup = new Hashtable();
        protected SysManageService sysService = new SysManageService();
        protected void Page_Load(object sender, System.EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                    this.pager.RecordCount = 1000;
                    this.pagerContacts.RecordCount = 1000;
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            TableGroup.Visible = false;
            TableContacts.Visible = false;
            ViewState["title"] = Session["title"].ToString();
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
		}
		#endregion


        private void BindDataGroup(int index)
        {
            try
            {
                TableGroup.Visible = true;
                TableContacts.Visible = false;
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
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�����ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        protected void btnContacts_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbmaintext.Text.Trim()) || string.IsNullOrEmpty(this.tbdate.Text.Trim()))
            {
                WebUtils.ShowMessage(this.Page, "�������ʼ����ļ����ڣ�"); return;
            }
            BindDataGroup(1);
            this.btnSendMail.Visible = true;
        }


        protected void btnSubmitGroup_Click(object sender, System.EventArgs e)
        {
            try
            {
                listGroupId.Clear();
                System.Web.UI.WebControls.CheckBox CBox;
                string groupId = "";
                string groupName = "";
                foreach (DataGridItem DgItem in DataGridGroup.Items)
                {
                    CBox = (CheckBox)DgItem.FindControl("selGroup");
                    if (CBox.Checked)
                    {
                        groupId = ((HtmlInputHidden)DgItem.FindControl("selectGroupId")).Value;
                        groupName = ((HtmlInputHidden)DgItem.FindControl("selectGroupName")).Value;
                        htGroup.Add(groupId, groupName);
                    }
                }

                StringBuilder group = new StringBuilder();
                foreach (DictionaryEntry de in htGroup)
                {
                    group.Append(de.Value.ToString());//�����ʼ���ȫ������
                    group.Append(";");
                    listGroupId.Add(de.Key.ToString());//�����ʼ���ȫ����id
                }

                this.tbGroup.Text = group.ToString();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "����ʼ�����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        //�����ʼ�
        protected void btnSendMail_Click(object sender, System.EventArgs e)
        {
            try
            {
                string maintext = this.tbmaintext.Text.Trim();
                string date = this.tbdate.Text.Trim();

                string emailMsg ="<html><head><title></title></head><body><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:1310px;line-height:22px; margin-left:10px; table-layout:fixed;\" align='left' ID='Table1'><tr><td style='width:1000px;'><p style='padding:10px 0;margin:0;'> "
                + "�𾴵ĲƸ�ͨ�û���<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;���ã�<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}<br><br><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + "�Ƹ�֧ͨ���Ƽ����޹�˾<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                +"{1}</p></td></tr></table></body></html>";
                emailMsg = string.Format(emailMsg, maintext, date);
                StringBuilder emailList = new StringBuilder();
                foreach (string id in listGroupId)
                {
                    DataSet ds = new DataSet();
                    ds = sysService.QueryOneGroupContacts(id, 0, 1000);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string bccMail = row["Femail"].ToString();//�����ܼ������ַ
                            CommMailSend.SendInternalMailCanSecret("", "", bccMail, ViewState["title"].ToString(), emailMsg.ToString(), true, null);
                        }
                    }
                }

                WebUtils.ShowMessage(this.Page, "�����ʼ��ɹ���");
                this.btnSendMail.Visible = false;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�����ʼ�ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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
                case "QueryOneGroup": //�鿴��������ϵ��
                    QueryOneGroup(fid);
                    break;
            }
        }


        private void QueryOneGroup(string fid,int index=1)
        {

            try
            {
                TableGroup.Visible = true;
                TableContacts.Visible = true;
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
                }

                DataGridContacts.DataSource = ds;
                DataGridContacts.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ���Ա����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindDataGroup(e.NewPageIndex);
        }

        public void ChangePagerContacts(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pagerContacts.CurrentPageIndex = e.NewPageIndex;
            QueryOneGroup(ViewState["groupId"].ToString(), e.NewPageIndex);
        }

	}
}