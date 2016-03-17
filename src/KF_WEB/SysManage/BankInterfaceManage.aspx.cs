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
using CFT.CSOMS.BLL.SysManageModule;
using System.Collections.Generic;
using SunLibraryEX;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.Apollo.Logging;
using System.Linq;

namespace TENCENT.OSS.CFT.KF.KF_Web.SysManage
{
	/// <summary>
	/// SysBulletinManage ��ժҪ˵����
	/// </summary>
    public partial class BankInterfaceManage : PageBase
	{
        SysManageService bll = new SysManageService();
        Dictionary<string, string> BankInterfaceName = SysManageService.BankInterfaceName;
        Dictionary<string, string> BullType = SysManageService.BullType;
        Dictionary<string, string> BullState = SysManageService.BullState;
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
                if (Request.QueryString["sysid"] != null && Request.QueryString["sysid"].Trim() != "")
                {
                    ddlSysList.SelectedValue = Request.QueryString["sysid"].Trim();
                }

                PublicRes.GetDllDownList(ddlSysList, BankInterfaceName, "ȫ��", "");
                PublicRes.GetDllDownList(ddlBullType, BullType, "ȫ��", "");
                PublicRes.GetDropdownlist(BullState,ddlBullState);
                ddlSysList.SelectedValue = "1";

                textBoxBeginDate.Text ="";
                textBoxEndDate.Text ="";
                this.pager.RecordCount = 1000;

                int totalNum = 0;
                DataSet ds = new BankClassifyService().QueryBankBusiInfo(1, "", "", 0, 0, 0, 1000, out totalNum);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = ds.Tables[0].DefaultView;
                        dv.Sort = "bank_name ASC"; 
                        this.ddl_bankname.DataSource = dv.ToTable();
                        this.ddl_bankname.DataBind();
                        this.ddl_bankname.Items.Add(new ListItem() { Text = "��ѡ��", Value = "", Selected = true });
                    }
                }
            }

            this.btnOpen.Attributes["onClick"] = "if(!confirm('ȷ��Ҫ��ǰ�ſ�����ά���еĹ�����')) return false;";
        }

        private void ValidateDate()
        {
                DateTime begindate;
                DateTime enddate;
                try
                {
                    if (!string.IsNullOrEmpty(textBoxBeginDate.Text.Trim()) && !string.IsNullOrEmpty(textBoxEndDate.Text.Trim()))
                    {
                        begindate = DateTime.Parse(textBoxBeginDate.Text);
                        enddate = DateTime.Parse(textBoxEndDate.Text);

                        if (begindate.CompareTo(enddate) > 0)
                        {
                            WebUtils.ShowMessage(this.Page, "��ֹ����С����ʼ���ڣ����������룡");
                            return;
                        }
                        ViewState["begindate"] = begindate.ToString("yyyy-MM-dd 00:00:00");
                        ViewState["enddate"] = enddate.ToString("yyyy-MM-dd 23:59:59");
                    }
                    else
                    {
                        ViewState["begindate"] = "";
                        ViewState["enddate"] = "";
                    }
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "������������");
                    return;
                }
                ViewState["current_datetime"] = "";
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        private void BindData(int index)
        {
            try
            {
                GetBankTypeList();
            }
            catch (Exception ex)
            {
                LogError("SysManage.BankInterfaceManage", "private void BindData(int index)", ex);
                WebUtils.ShowMessage(this.Page, ex.Message);
                return;
            }
            try
            {
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);
                string listtype = ddlSysList.SelectedValue;

                DataSet ds = new DataSet();

                int bank_type = 0;
                if (Session["BankTypeList"] != null)
                {
                    ArrayList BankTypeList = (ArrayList)Session["BankTypeList"];
                    if (BankTypeList != null && BankTypeList.Count > 0)
                        bank_type = int.Parse(BankTypeList[0].ToString().Trim());
                }
                 int total_num=0;
                 if (index > 1 && ViewState["total_num"] != null && (int)ViewState["total_num"] <= start)
                 {
                     WebUtils.ShowMessage(this.Page, "û����һҳ");
                     return;
                 }

                ds = new SysManageService().QueryBankBulletin(listtype, 0, bank_type, "", ddlBullState.SelectedValue
                    , ddlBullType.SelectedValue, ViewState["begindate"].ToString(), ViewState["enddate"].ToString(),
                    ViewState["current_datetime"].ToString(), max, start,out total_num);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Datagrid2.DataSource = null;
                    Datagrid2.DataBind();
                    WebUtils.ShowMessage(this.Page, "û�м�¼");
                    return;
                }

                ViewState["total_num"] = total_num;
                ds.Tables[0].Columns.Add("URLUpdate", typeof(System.String));
                ds.Tables[0].Columns.Add("URLQuery", typeof(String));
                string url = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr.BeginEdit();
                    url = String.Format("BankInterfaceManage_Detail.aspx?sysid={0}&bulletinId={1}", listtype, dr["bulletin_id"].ToString());
                    dr["URLUpdate"] = url + "&opertype=2";
                    dr["URLQuery"] = url + "&opertype=0";
                    dr.EndEdit();
                }

                //����
                DataTable dt = ds.Tables[0];
                DataView view = dt.DefaultView;
                view.Sort = "startime desc";
                dt = view.ToTable();
                DataSet dsResult = new DataSet();
                dsResult.Tables.Add(dt);

                Datagrid2.DataSource = dsResult.Tables[0].DefaultView;
                Datagrid2.DataBind();
            }
            catch (Exception eSys)
            {
                LogError("SysManage.BankInterfaceManage", "private void BindData(int index)  ��ȡ�����쳣:", eSys);
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        //�޸ġ����Ϲ��水ť����
        public void dg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj1 = e.Item.Cells[16].FindControl("btupdate");
            object obj2 = e.Item.Cells[17].FindControl("CheckBox2");
            object objcancel = e.Item.Cells[18].FindControl("CheckBoxCancel");
            if (obj1 != null)
            {
                string banktype = e.Item.Cells[4].Text.Trim();
                string bull_type = e.Item.Cells[1].Text.Trim();
                DateTime endtime = DateTime.Parse(e.Item.Cells[9].Text.Trim());
                 DateTime startime = DateTime.Parse(e.Item.Cells[8].Text.Trim());
                 string bull_state = e.Item.Cells[2].Text.Trim();
                LinkButton lb = (LinkButton)obj1;
                CheckBox check = (CheckBox)obj2;
                CheckBox checkCancel = (CheckBox)objcancel;
                //�������޸ģ���*�������͡���������ά�����桢���Ϲ��桢��ȥʱ��ι���;
                if (banktype.Contains("*") || bull_type != "1" || endtime < DateTime.Now || bull_state=="2")
                {
                    lb.Visible = false;
                    check.Enabled = false;
                }

                //���������ϣ���*�������͡���������ά�����桢���Ϲ��棻ֻ������δ��Ч����
                if (banktype.Contains("*") || bull_type != "1" || !(endtime > DateTime.Now && startime > DateTime.Now) || bull_state == "2")
                {
                    checkCancel.Enabled = false;
                }
            }
        }
     
        protected void btadd_Click(object sender, System.EventArgs e)
        {
            if (ddlSysList.SelectedValue == "")
            {
                WebUtils.ShowMessage(this.Page, "��ѡ���ӿ����ͣ�");
                return;
            }

            try
            {
                GetBankTypeList();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
                return;
            }

            if (Session["BankTypeList"] != null)
            {
                ArrayList BankTypeList = (ArrayList)Session["BankTypeList"];
                if (BankTypeList == null || BankTypeList.Count == 0)
                {
                    WebUtils.ShowMessage(this.Page, "���б������벻��Ϊ��");
                    return;
                }
            }
            string sysid = ddlSysList.SelectedValue;
            Response.Redirect("BankInterfaceManage_Detail.aspx?sysid=" + sysid + "&opertype=1");
        }

        private void GetBankTypeList()
        {
            ArrayList bankTypeList = new ArrayList();
            string banktype = txtBankType.Text.Trim().Replace(" ", "").Replace("��", ";");
            string[] arrBank = banktype.Split(';');

            foreach (string str in arrBank)
            {
                if (string.IsNullOrEmpty(str.Trim()))
                    continue;
                if (!bankTypeList.Contains(str.Trim()))
                {
                    if (StringEx.IsNumber(str.Trim()))
                        bankTypeList.Add(str.Trim());
                    else
                    {
                        throw new Exception("���б������󣡲�ȫΪ����");
                    }
                }
            }

            if (bankTypeList != null && bankTypeList.Count > 30)
                throw new Exception("���б��볬��30����");

            if (!string.IsNullOrEmpty( ddl_bankname.SelectedItem.Value))
            { 
                var bankTypes= GetbankTypeByBankName(ddl_bankname.SelectedItem.Value);
                if (bankTypes != null && bankTypes.Length > 0)
                {
                    foreach (var item in bankTypes)
                    {
                        if (!bankTypeList.Contains(item))
                        {
                            bankTypeList.Add(item);
                        }
                    }
                }
            }

            Session["BankTypeList"] = bankTypeList;
        }

        private string[] GetbankTypeByBankName(string bank_code)
        {
            var bankType = ViewState["bankType" + bank_code] as string[]; //����һ�·�ֹ�´β��ҵ�ʱ��ȥ��ѯ�ӿ�
            if (bankType == null)
            {
                var bll = new BankClassifyService();
                var totalNum = 0;
                var limit=10;
                DataSet ds = bll.QueryBankBusiInfo(0, bank_code, "", 0, 0, 0, limit, out totalNum);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var dt = ds.Tables[0];
                    var pagecout = (int)Math.Ceiling(totalNum * 1.0 / limit);
                    for (int i = 1; i < pagecout; i++)
                    {
                        var offset = i * limit;
                        DataSet ds1 = bll.QueryBankBusiInfo(0, bank_code, "", 0, 2, offset, limit, out totalNum);
                        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            dt.Merge(ds1.Tables[0]);
                        }
                    }
                    bankType = dt.AsEnumerable().Select(u => (string)u["bank_type"]).ToArray();
                    ViewState["bankType" + bank_code] = bankType;
                }
            }
            return bankType;
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            this.pager.CurrentPageIndex = 1;
            ValidateDate();
            BindData(1);
        }

        protected void btOpen_Click(object sender, System.EventArgs e)
        {
            try
            {
                int itemCounts = 0;
                ArrayList listIds = PublicRes.GetCheckData(Datagrid2, 17, 0, "CheckBox2", out itemCounts);

                if (listIds == null || listIds.Count == 0)
                {
                    WebUtils.ShowMessage(this.Page, "����ѡ����Ҫ���������ݣ�"); return;
                }

                string outStr = "";
                #region �ύ��������
                foreach (string id in listIds)
                {
                    try
                    {
                        commData.T_BANKBULLETIN_INFO bulletin = new commData.T_BANKBULLETIN_INFO();
                        BankInterfaceManage_Detail bankIntManDetail = new BankInterfaceManage_Detail();
                        string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                        bulletin = bankIntManDetail.GetBulletin("", id, "");
                        bulletin.endtime = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
                        bulletin.IsNew = false;
                        bulletin.IsOPen = true;
                        bulletin.op_flag = "2";//�޸�
                        bulletin.returnUrl = "http://kf.cf.com/SysManage/BankInterfaceManage_Detail.aspx?Fbanktype=" + bulletin.banktype.Trim() + "&sysid=" + bulletin.businesstype + "&objid=" + objid + "&opertype=0";
                        bulletin.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        bulletin.updateuser = Session["uid"].ToString();
                        bankIntManDetail.CheckBulletin(this,bulletin, objid);
                    }
                    catch (Exception err)
                    {
                        string errOne = id + "|";
                        outStr += errOne;

                        LogError("SysManage.BankInterfaceManage", " protected void btOpen_Click(object sender, System.EventArgs e)id=" + outStr + ",���濪�����������쳣:", err);
                    }
                }
                #endregion

                if (outStr != "")
                {
                    WebUtils.ShowMessage(this.Page, "���濪�������쳣��" + outStr);
                    return;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "ȫ�������������ύ����ȴ�������");
                    return;
                }
            }
            catch (Exception err)
            {
                LogError("SysManage.BankInterfaceManage", " protected void btOpen_Click(object sender, System.EventArgs e)���������쳣:", err);
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���������쳣��" + errStr);
            }
        }

        protected void btCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                int itemCounts = 0;
                ArrayList listIds = PublicRes.GetCheckData(Datagrid2, 18, 0, "CheckBoxCancel", out itemCounts);

                if (listIds == null || listIds.Count == 0)
                {
                    WebUtils.ShowMessage(this.Page, "����ѡ����Ҫ���������ݣ�"); return;
                }

                string outStr = "";
                #region �ύ��������
                foreach (string id in listIds)
                {
                    try
                    {
                        commData.T_BANKBULLETIN_INFO bulletin = new commData.T_BANKBULLETIN_INFO();
                        BankInterfaceManage_Detail bankIntManDetail = new BankInterfaceManage_Detail();
                        string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                        bulletin = bankIntManDetail.GetBulletin("", id, "");
                        bulletin.IsNew = false;
                        bulletin.op_flag = "3";//����
                        bulletin.returnUrl = "http://kf.cf.com/SysManage/BankInterfaceManage_Detail.aspx?Fbanktype=" + bulletin.banktype.Trim() + "&sysid=" + bulletin.businesstype + "&objid=" + objid + "&opertype=0";
                        bulletin.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        bulletin.updateuser = Session["uid"].ToString();
                        bankIntManDetail.CheckBulletin(this, bulletin, objid);
                    }
                    catch (Exception err)
                    {
                        string errOne = id + "|";
                        outStr += errOne;
                        LogHelper.LogInfo(errOne + "�����������������쳣��" + err);
                    }
                }
                #endregion

                if (outStr != "")
                {
                    WebUtils.ShowMessage(this.Page, "�������������쳣��" + outStr);
                    return;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "ȫ�������������ύ����ȴ�������");
                    return;
                }
            }
            catch (Exception err)
            {
                LogError("SysManage.BankInterfaceManage", "protected void btCancel_Click(object sender, System.EventArgs e)  ���������쳣:", err);
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���������쳣��" + errStr);
            }
        }

        protected void btCurrent_Click(object sender, System.EventArgs e)
        {
            this.pager.CurrentPageIndex = 1;
            ViewState["begindate"] = "";
            ViewState["enddate"] = "";
            ViewState["current_datetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            BindData(1);
        }
      
        protected void OnCheckBox_CheckedSelect(object sender, System.EventArgs e)
        {
            PublicRes.CheckAll(sender, Datagrid2, 17, "CheckBox2");
        }

        protected void OnCheckBoxCancel_CheckedSelect(object sender, System.EventArgs e)
        {
            PublicRes.CheckAll(sender, Datagrid2, 18, "CheckBoxCancel");
        }

        private void showMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
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
        }
        #endregion
	}
}
