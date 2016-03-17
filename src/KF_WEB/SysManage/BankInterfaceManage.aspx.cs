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
	/// SysBulletinManage 的摘要说明。
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

                PublicRes.GetDllDownList(ddlSysList, BankInterfaceName, "全部", "");
                PublicRes.GetDllDownList(ddlBullType, BullType, "全部", "");
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
                        this.ddl_bankname.Items.Add(new ListItem() { Text = "请选择", Value = "", Selected = true });
                    }
                }
            }

            this.btnOpen.Attributes["onClick"] = "if(!confirm('确定要提前放开正在维护中的公告吗？')) return false;";
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
                            WebUtils.ShowMessage(this.Page, "终止日期小于起始日期，请重新输入！");
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
                    WebUtils.ShowMessage(this.Page, "日期输入有误！");
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
                     WebUtils.ShowMessage(this.Page, "没有下一页");
                     return;
                 }

                ds = new SysManageService().QueryBankBulletin(listtype, 0, bank_type, "", ddlBullState.SelectedValue
                    , ddlBullType.SelectedValue, ViewState["begindate"].ToString(), ViewState["enddate"].ToString(),
                    ViewState["current_datetime"].ToString(), max, start,out total_num);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Datagrid2.DataSource = null;
                    Datagrid2.DataBind();
                    WebUtils.ShowMessage(this.Page, "没有记录");
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

                //排序
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
                LogError("SysManage.BankInterfaceManage", "private void BindData(int index)  读取数据异常:", eSys);
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        //修改、作废公告按钮限制
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
                //不允许修改：带*银行类型、不是例行维护公告、作废公告、过去时间段公告;
                if (banktype.Contains("*") || bull_type != "1" || endtime < DateTime.Now || bull_state=="2")
                {
                    lb.Visible = false;
                    check.Enabled = false;
                }

                //不允许作废：带*银行类型、不是例行维护公告、作废公告；只能作废未生效公告
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
                WebUtils.ShowMessage(this.Page, "请选定接口类型！");
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
                    WebUtils.ShowMessage(this.Page, "银行编码输入不能为空");
                    return;
                }
            }
            string sysid = ddlSysList.SelectedValue;
            Response.Redirect("BankInterfaceManage_Detail.aspx?sysid=" + sysid + "&opertype=1");
        }

        private void GetBankTypeList()
        {
            ArrayList bankTypeList = new ArrayList();
            string banktype = txtBankType.Text.Trim().Replace(" ", "").Replace("；", ";");
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
                        throw new Exception("银行编码有误！不全为数字");
                    }
                }
            }

            if (bankTypeList != null && bankTypeList.Count > 30)
                throw new Exception("银行编码超过30个！");

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
            var bankType = ViewState["bankType" + bank_code] as string[]; //缓存一下防止下次查找的时候还去查询接口
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
                    WebUtils.ShowMessage(this.Page, "请先选中需要操作的数据！"); return;
                }

                string outStr = "";
                #region 提交申请审批
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
                        bulletin.op_flag = "2";//修改
                        bulletin.returnUrl = "http://kf.cf.com/SysManage/BankInterfaceManage_Detail.aspx?Fbanktype=" + bulletin.banktype.Trim() + "&sysid=" + bulletin.businesstype + "&objid=" + objid + "&opertype=0";
                        bulletin.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        bulletin.updateuser = Session["uid"].ToString();
                        bankIntManDetail.CheckBulletin(this,bulletin, objid);
                    }
                    catch (Exception err)
                    {
                        string errOne = id + "|";
                        outStr += errOne;

                        LogError("SysManage.BankInterfaceManage", " protected void btOpen_Click(object sender, System.EventArgs e)id=" + outStr + ",公告开启申请审批异常:", err);
                    }
                }
                #endregion

                if (outStr != "")
                {
                    WebUtils.ShowMessage(this.Page, "公告开启申请异常：" + outStr);
                    return;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "全部开启申请已提交，请等待审批！");
                    return;
                }
            }
            catch (Exception err)
            {
                LogError("SysManage.BankInterfaceManage", " protected void btOpen_Click(object sender, System.EventArgs e)开启申请异常:", err);
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                WebUtils.ShowMessage(this.Page, "开启申请异常：" + errStr);
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
                    WebUtils.ShowMessage(this.Page, "请先选中需要操作的数据！"); return;
                }

                string outStr = "";
                #region 提交申请审批
                foreach (string id in listIds)
                {
                    try
                    {
                        commData.T_BANKBULLETIN_INFO bulletin = new commData.T_BANKBULLETIN_INFO();
                        BankInterfaceManage_Detail bankIntManDetail = new BankInterfaceManage_Detail();
                        string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                        bulletin = bankIntManDetail.GetBulletin("", id, "");
                        bulletin.IsNew = false;
                        bulletin.op_flag = "3";//作废
                        bulletin.returnUrl = "http://kf.cf.com/SysManage/BankInterfaceManage_Detail.aspx?Fbanktype=" + bulletin.banktype.Trim() + "&sysid=" + bulletin.businesstype + "&objid=" + objid + "&opertype=0";
                        bulletin.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        bulletin.updateuser = Session["uid"].ToString();
                        bankIntManDetail.CheckBulletin(this, bulletin, objid);
                    }
                    catch (Exception err)
                    {
                        string errOne = id + "|";
                        outStr += errOne;
                        LogHelper.LogInfo(errOne + "公告作废申请审批异常：" + err);
                    }
                }
                #endregion

                if (outStr != "")
                {
                    WebUtils.ShowMessage(this.Page, "公告作废申请异常：" + outStr);
                    return;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "全部作废申请已提交，请等待审批！");
                    return;
                }
            }
            catch (Exception err)
            {
                LogError("SysManage.BankInterfaceManage", "protected void btCancel_Click(object sender, System.EventArgs e)  作废申请异常:", err);
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                WebUtils.ShowMessage(this.Page, "作废申请异常：" + errStr);
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

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
        }
        #endregion
	}
}
