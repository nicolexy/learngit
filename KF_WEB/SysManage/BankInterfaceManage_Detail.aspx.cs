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

namespace TENCENT.OSS.CFT.KF.KF_Web.SysManage
{
	/// <summary>
	/// SysBulletinManage_Detail 的摘要说明。
	/// </summary>
    public partial class BankInterfaceManage_Detail : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lbTitle;
		protected System.Web.UI.WebControls.DropDownList ddlSysList;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		protected System.Web.UI.WebControls.Button btnNew;
		protected System.Web.UI.WebControls.Button btnIssue;
        private string sysid, bulletinId, uctype;
        private static int nTime=0;//添加时间段
        private int max = 5;//公告最大条数
        private static bool queryed = false;//是否已查询过数据库有多少条记录
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				//Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
            if (!IsPostBack)
            {
                iStartTime.Attributes.Add("onclick", "openBankModeBegin()");
                iEndTime.Attributes.Add("onclick", "openBankModeEnd()");
                iStartTime1.Attributes.Add("onclick", "openBankModeBegin1()");
                iEndTime1.Attributes.Add("onclick", "openBankModeEnd1()");
                iStartTime2.Attributes.Add("onclick", "openBankModeBegin2()");
                iEndTime2.Attributes.Add("onclick", "openBankModeEnd2()");
                iStartTime3.Attributes.Add("onclick", "openBankModeBegin3()");
                iEndTime3.Attributes.Add("onclick", "openBankModeEnd3()");
                iStartTime4.Attributes.Add("onclick", "openBankModeBegin4()");
                iEndTime4.Attributes.Add("onclick", "openBankModeEnd4()");
                this.btInterfaceAdd.Attributes.Add("onclick", "return confirm(\"你确认要进行新增操作申请吗？\");");
                this.btInterfaceUpdate.Attributes.Add("onclick", "return confirm(\"你确认要进行修改操作申请吗？\");");

                //初始化static
                nTime=0;
                queryed = false;

                sysid = Request.QueryString["sysid"].Trim();
                ViewState["sysid"] = sysid;

                if (Request.QueryString["sysid"] != null && Request.QueryString["sysid"].Trim() != "")
                {
                    #region
                    tbStartTime.Enabled = true;
                    iStartTime.Visible = true;
                    tbEndTime.Enabled = true;
                    iEndTime.Visible = true;

                 
                    Table4.Visible = true;

                    //DropDownListShow();//根据radio显示时间

                  //  setConfig.GetAllBankList(ddlQueryBankTypeInterface);

                    //银行下拉列表
                    setConfig.GetAllBankListFromDic(ddlQueryBankTypeInterface);
                    ddlQueryBankTypeInterface.Items.Insert(0, new ListItem("所有银行", ""));


                    this.tbStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                    if (Request.QueryString["opertype"] != null && Request.QueryString["opertype"].Trim() != "")
                    {
                        string opertype = Request.QueryString["opertype"].Trim();
                        if (opertype == "2")//编辑或查询
                        {
                            this.btInterfaceAdd.Visible = false;
                            this.btInterfaceUpdate.Visible = true;
                            this.btInterfaceBack.Visible = true;
                            hlInterfaceBack.Visible = false;
                        }
                        else if (opertype == "1")//新增
                        {
                            this.btInterfaceAdd.Visible = true;
                            this.btInterfaceUpdate.Visible = false;
                            this.btInterfaceBack.Visible = true;
                            hlInterfaceBack.Visible = false;
                            this.InterfaceOpen.Visible = false;

                        }
                        else//账务系统过来查看
                        {
                            this.btInterfaceAdd.Visible = false;
                            this.btInterfaceUpdate.Visible = false;
                            this.btInterfaceBack.Visible = false;
                            hlInterfaceBack.Visible = true;
                            if (Request.QueryString["objid"] != null && Request.QueryString["objid"].Trim() != "")
                            {
                                string objid = Request.QueryString["objid"].Trim();
                                ViewState["objid"] = objid;
                                BindBankInterface("", objid);
                            }

                        }
                    }

                    #endregion
                }
                else
                {
                    //出错了，不能不带参数时来
                    Response.Redirect("../login.aspx?wh=1");
                }

                if (Request.QueryString["bulletinId"] != null && Request.QueryString["bulletinId"].Trim() != "")//编辑查看
                {
                    bulletinId = Request.QueryString["bulletinId"].Trim();
                    ViewState["bulletinId"] = bulletinId;
                }
                else//新增
                {
                    bulletinId = "";
                    ViewState["bulletinId"] = bulletinId;
                }

                if (bulletinId == "")
                {

                    labTitle.Text = "银行接口新增";


                    if (ViewState["objid"] != null)
                    {
                        labTitle.Text = "查看银行接口";
                    }
                }
                else
                {
                    labTitle.Text = "银行接口修改";
                    BindBankInterface(bulletinId, "");
                }
            }
            else
            {
                bulletinId = ViewState["bulletinId"].ToString();
                sysid = ViewState["sysid"].ToString();
            }

            if (ViewState["sysid"].ToString() != "6")
            {
                this.TR1.Visible = false;
            }
		}


        protected void BindBankInterface(string bulletinId, string objid)
        {
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                string msg = "";
                Query_Service.T_BANKBULLETIN_INFO bankbulletin = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_BANKBULLETIN_INFO();
                DataSet ds = new DataSet();
                if (bulletinId != null && bulletinId != "")
                {
                    ds = qs.QueryBankBulletin(int.Parse(ViewState["sysid"].ToString()), 0, 0, bulletinId,5,0);//通过接口返回记录
                    bankbulletin = qs.TurnBankBulletinClass(ds);//将记录转换成公告类
                }
                if (objid != null && objid != "")
                {
                    bankbulletin = qs.QueryBankBulletinByObjid(objid, "BankBulletinForType", out msg);
                }
                if (bankbulletin == null)
                {
                    WebUtils.ShowMessage(this.Page, "查询出错！");
                    return;
                }

                this.ddlQueryBankTypeInterface.SelectedValue = bankbulletin.banktype.Trim();
                this.tbInterfaceMainText.Text = bankbulletin.maintext.Trim().Replace("<br/>", "\r\n");
                this.tbStartTime.Text = bankbulletin.startime.Trim();
                this.tbEndTime.Text = bankbulletin.endtime.Trim();
           //     this.tbalwtime.Text = bankbulletin.Falwtime.Trim();//有效时间段
                this.tbcreateuserInterface.Text = bankbulletin.createuser.Trim();
                this.InterfaceOpen.Visible = true;
                this.tbbusinetype.Text = bankbulletin.businesstype.Trim();
                this.TextTCMainText.Text = bankbulletin.popuptext.Trim();
                this.tbbulletin_id.Text = bankbulletin.bulletin_id.Trim();
                if (bankbulletin.closetype.Trim() == "1")
                {
                  //  this.cbForbid.Checked = true;
                    this.ForbidRadio.SelectedValue = "1";
                    tcTextId.Visible = false;
                }
                else if (bankbulletin.closetype.Trim() == "2")
                {
                    this.ForbidRadio.SelectedValue = "2";
                    tcTextId.Visible = true;
                }
                else if (bankbulletin.closetype.Trim() == "3")
                {
                    this.ForbidRadio.SelectedValue = "3";
                    tcTextId.Visible = false;
                }
                else
                {
                    throw new Exception("关闭类型" + bankbulletin.closetype.Trim() + "不可知！");
                }
                this.closedRadio.SelectedValue = bankbulletin.op_support_flag.Trim();//子类型
                this.tbTitle.Text = bankbulletin.title;//标题
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
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
            this.InterfaceOpen.CheckedChanged += new System.EventHandler(this.InterfaceOpen_CheckedChanged);
          //  this.cbForbid.CheckedChanged += new System.EventHandler(this.cbForbid_CheckedChanged);
		}
		#endregion

        protected void btInterfaceAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                string sysid = ViewState["sysid"].ToString();
                if (this.tbInterfaceMainText.Text.Trim().Length > 50)
                {
                    WebUtils.ShowMessage(this.Page, "正文超过50字符,请重新输入！");
                    return;
                }
                if (this.TextTCMainText.Text.Trim().Length > 80)
                {
                    WebUtils.ShowMessage(this.Page, "弹层正文超过80字符,请重新输入！");
                    return;
                }

                Query_Service.T_BANKBULLETIN_INFO bankbulletin = new Query_Service.T_BANKBULLETIN_INFO();
                bankbulletin.IsNew = true;
                bankbulletin.IsOPen = false;
                bankbulletin.banktype = this.ddlQueryBankTypeInterface.SelectedValue.Trim();
                bankbulletin.businesstype = sysid;
                if (sysid == "6")//支付有子类型：1.全部关闭  2.签约  3.支付  4.退款
                {
                    bankbulletin.op_support_flag = this.closedRadio.SelectedValue;
                }
                else
                    bankbulletin.op_support_flag = "1";

                //是否影响接口
                //if (this.cbForbid.Checked == true)
                //    bankbulletin.closetype = "1";
                //else
                //    bankbulletin.closetype = "2";

                if (this.ForbidRadio.SelectedValue == "1")
                    bankbulletin.closetype = "1";
                else if (this.ForbidRadio.SelectedValue == "2")
                    bankbulletin.closetype = "2";
                else if (this.ForbidRadio.SelectedValue == "3")
                    bankbulletin.closetype = "3";

                bankbulletin.title = tbTitle.Text.Trim() ;
                bankbulletin.maintext = this.tbInterfaceMainText.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                bankbulletin.popuptext = TextTCMainText.Text.Trim();
                bankbulletin.createuser = Session["uid"].ToString();
                bankbulletin.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                bankbulletin.updateuser = Session["uid"].ToString();
                bankbulletin.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (bankbulletin.closetype == "2")//软关闭，必须有弹层，标题及弹层必须有一个，所以只确保弹层即可
                {
                    if (bankbulletin.popuptext == "" || bankbulletin.popuptext == null)
                    {
                        WebUtils.ShowMessage(this.Page, "软关闭必须输入弹层正文！");
                        return;
                    }
                }
                else
                {
                    bankbulletin.popuptext = "";//硬关闭可不输入弹层
                }

                //构造审批数据
                string bankName = "";
                try
                {
                    bankName = bankbulletin.maintext.Substring(0, bankbulletin.maintext.IndexOf("银行"));//取银行名称
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "请按模板填写公告！");
                    return;
                }

                string[] startTime = new string[nTime + 1];
                string[] endTime = new string[nTime + 1];

                //获取时间数组
                for (int i = 0; i <= nTime; i++)//nTime最大4
                {
                    #region
                    try
                    {
                        if (i == 0)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 1)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime1.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime1.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 2)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime2.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime2.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 3)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime3.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime3.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 4)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime4.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime4.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    catch
                    {
                        WebUtils.ShowMessage(this.Page, "请输入正确的日期");
                        return;
                    }
                    #endregion

                }

                string msg = "";
                if (IsRepeatedSelf(startTime, endTime, out msg))
                { //判断新增时间段自身重复
                    WebUtils.ShowMessage(this.Page, msg);
                    return;
                }

                //if (IsRepeated("", startTime, endTime, out msg))//新增的时候数据库不可能有与bulletin_id一样的记录
                //{ //判断数据库是否存在重复时间段的公告
                //    WebUtils.ShowMessage(this.Page, msg);
                //    return;
                //}

                string objid = "";
                //审批
                #region
                for (int i = 0; i <= nTime; i++)//nTime最大4
                {
                    try
                    {
                        objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage(); ;//每个审批单
                        bankbulletin.returnUrl = "http://kf.cf.com/SysManage/BankInterfaceManage_Detail.aspx?Fbanktype=" + this.ddlQueryBankTypeInterface.SelectedValue.Trim() + "&sysid=" + sysid + "&objid=" + objid + "&opertype=0";
                        bankbulletin.bulletin_id = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage();//每个银行公告ID
                        bankbulletin.startime = startTime[i];
                        bankbulletin.endtime = endTime[i];

                        if (i != 0)
                        {
                            bankbulletin.maintext = bankName + "银行系统维护中，预计" + DateTime.Parse(bankbulletin.endtime).ToString("MM月dd日HH:mm") + "恢复。";
                            if (bankbulletin.closetype == "2")
                            {
                                bankbulletin.popuptext = DateTime.Parse(bankbulletin.startime).ToString("MM月dd日HH:mm") + "至" + DateTime.Parse(bankbulletin.endtime).ToString("MM月dd日HH:mm") +
                                    "因" + bankName + "银行系统维护，此期间操作的付款将延迟到" + DateTime.Parse(bankbulletin.endtime).AddDays(1).ToString("MM月dd日") + "到账";
                            }
                            else
                            {
                                bankbulletin.popuptext = "";
                            }
                        }


                        string[,] param = GetBulletin(bankbulletin);//获得单条公告

                        Check_WebService.Param[] pa = ToParamArray(param);

                        string memo = "新增银行接口维护信息,银行类型:" + this.ddlQueryBankTypeInterface.SelectedItem.Text.Trim(); ;
                        PublicRes.CreateCheckService(this).StartCheck(objid,
                            "BankBulletinForType", memo, "0", pa);
                        PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "BankBulletinForType", "新增银行接口维护申请审批", 1, this.ddlQueryBankTypeInterface.SelectedValue.Trim(), "");
                    }
                    catch
                    {
                        WebUtils.ShowMessage(this.Page, "该维护时间段" + bankbulletin.startime + "--" + bankbulletin.endtime + "失败，请查询后再继续新增或修改！");
                        return;
                    }
                }
                #endregion

                nTime = 0;
                queryed = false;//需要重新查询数据库确定存在几条公告

                TimesHide();

                WebUtils.ShowMessage(this.Page, "全部新增银行接口维护申请已提交，请等待审批！");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void btInterfaceUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                string sysid = ViewState["sysid"].ToString();
                if (this.tbInterfaceMainText.Text.Trim().Length > 50)
                {
                    WebUtils.ShowMessage(this.Page, "正文超过50字符,请重新输入！");
                    return;
                }
                if (this.TextTCMainText.Text.Trim().Length > 80)
                {
                    WebUtils.ShowMessage(this.Page, "弹层正文超过80字符,请重新输入！");
                    return;
                }

                Query_Service.T_BANKBULLETIN_INFO bankbulletin = new Query_Service.T_BANKBULLETIN_INFO();
                bankbulletin.bulletin_id = this.tbbulletin_id.Text.Trim();
                bankbulletin.IsNew = false;
                bankbulletin.IsOPen = this.InterfaceOpen.Checked;
                bankbulletin.banktype = this.ddlQueryBankTypeInterface.SelectedValue.Trim();
                bankbulletin.businesstype = sysid;
                if (sysid == "6")//支付有子类型：1.全部关闭  2.签约  3.支付  4.退款
                {
                    bankbulletin.op_support_flag = this.closedRadio.SelectedValue;
                }
                else
                    bankbulletin.op_support_flag = "1";
                //是否影响接口
                //if (this.cbForbid.Checked == true)
                //    bankbulletin.closetype = "1";
                //else
                //    bankbulletin.closetype = "2";
                if (this.ForbidRadio.SelectedValue == "1")
                    bankbulletin.closetype = "1";
                else if (this.ForbidRadio.SelectedValue == "2")
                    bankbulletin.closetype = "2";
                else if (this.ForbidRadio.SelectedValue == "3")
                    bankbulletin.closetype = "3";

                bankbulletin.title = tbTitle.Text.Trim();
                bankbulletin.maintext = this.tbInterfaceMainText.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                bankbulletin.popuptext = TextTCMainText.Text.Trim();
                bankbulletin.createuser = Session["uid"].ToString();
                bankbulletin.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                bankbulletin.updateuser = Session["uid"].ToString();
                bankbulletin.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (bankbulletin.closetype == "2")//软关闭，必须有弹层，标题及弹层必须有一个，所以只确保弹层即可
                {
                    if (bankbulletin.popuptext == "" || bankbulletin.popuptext == null)
                    {
                        WebUtils.ShowMessage(this.Page, "软关闭必须输入弹层正文！");
                        return;
                    }
                }

                //审批
                string bankName = "";
                try
                {
                    bankName = bankbulletin.maintext.Substring(0, bankbulletin.maintext.IndexOf("银行"));
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "请按模板填写公告！");
                    return;
                }

                if (this.InterfaceOpen.Checked)//若立即启用则只能修改本条公告
                {
                    nTime = 0;

                }

                string[] startTime = new string[nTime + 1];
                string[] endTime = new string[nTime + 1];

                for (int i = 0; i <= nTime; i++)//nTime最大4
                {
                    #region
                    try
                    {
                        if (i == 0)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 1)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime1.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime1.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 2)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime2.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime2.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 3)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime3.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime3.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (i == 4)
                        {
                            startTime[i] = DateTime.Parse(this.tbStartTime4.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            endTime[i] = DateTime.Parse(this.tbEndTime4.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    catch
                    {
                        WebUtils.ShowMessage(this.Page, "请输入正确的日期");
                        return;
                    }
                    #endregion
                }

                string msg = "";
                if (IsRepeatedSelf(startTime, endTime, out msg))
                { //判断新增时间段自身重复
                    WebUtils.ShowMessage(this.Page, msg);
                    return;
                }

                //if (IsRepeated(bankbulletin.bulletin_id, startTime, endTime, out msg))//新增的时候数据库不可能有与bulletin_id一样的记录
                //{ //判断数据库是否存在重复时间段的公告
                //    WebUtils.ShowMessage(this.Page, msg);
                //    return;
                //}
                string objid = "";
                //审批
                #region
                for (int i = 0; i <= nTime; i++)//nTime最大4
                {
                    try
                    {
                        bankbulletin.startime = startTime[i];
                        bankbulletin.endtime = endTime[i];
                        if (this.InterfaceOpen.Checked)//若立即启用则结束时间为当前时间
                            bankbulletin.endtime = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");

                        if (i != 0)//i=0为修改，其他为新增
                        {
                            bankbulletin.IsNew = true;
                            bankbulletin.bulletin_id = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage();
                            bankbulletin.maintext = bankName + "银行系统维护中，预计" + DateTime.Parse(bankbulletin.endtime).ToString("MM月dd日HH:mm") + "恢复。";
                            if (bankbulletin.closetype == "2")
                            {
                                bankbulletin.popuptext = DateTime.Parse(bankbulletin.startime).ToString("MM月dd日HH:mm") + "至" + DateTime.Parse(bankbulletin.endtime).ToString("MM月dd日HH:mm") +
                                    "因" + bankName + "银行系统维护，此期间操作的付款将延迟到" + DateTime.Parse(bankbulletin.endtime).AddDays(1).ToString("MM月dd日") + "到账";
                            }
                            else
                            {
                                bankbulletin.popuptext = "";
                            }
                            bankbulletin.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();//每个审批单
                        bankbulletin.returnUrl = "http://kf.cf.com/SysManage/BankInterfaceManage_Detail.aspx?Fbanktype=" + this.ddlQueryBankTypeInterface.SelectedValue.Trim() + "&sysid=" + sysid + "&objid=" + objid + "&opertype=0";
                       

                        string[,] param = GetBulletin(bankbulletin);//获得单条公告

                        Check_WebService.Param[] pa = ToParamArray(param);

                        string memo = "新增银行接口维护信息,银行类型:" + this.ddlQueryBankTypeInterface.SelectedItem.Text.Trim(); ;

                        PublicRes.CreateCheckService(this).StartCheck(objid,
                            "BankBulletinForType", memo, "0", pa);
                        PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "BankBulletinForType", "新增银行接口维护申请审批", 1, this.ddlQueryBankTypeInterface.SelectedValue.Trim(), "");
                    }
                    catch(Exception err)
                    {
                        WebUtils.ShowMessage(this.Page, "该维护时间段" + bankbulletin.startime + "--" + bankbulletin.endtime + "失败，请查询后再继续新增或修改！错误：" + err.Message);
                        return;
                    }
                }
                #endregion
                nTime = 0;
                queryed = false;//需要重新查询数据库确定存在几条公告
                TimesHide();
                WebUtils.ShowMessage(this.Page, "全部修改或新增银行接口维护申请已提交，请等待审批！");

            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        //判断数据库是否存在重复时间段的公告
        protected bool IsRepeated(string bulletin_id,string[] startTime, string[] endTime, out string msg)
        {
            msg = "";
            bool index = false;
            try
            {
                int len = startTime.Length;
                if (startTime.Length != endTime.Length)
                {
                    msg = "判断数据库是否存在重复时间段的公告出错！";
                }

                QueryBankBulletinOfBusiness(int.Parse(ViewState["sysid"].ToString()), int.Parse(this.closedRadio.SelectedValue), int.Parse(this.ddlQueryBankTypeInterface.SelectedValue.Trim()));

                DataTable g_dt = (DataTable)ViewState["g_dt"];

                if (g_dt != null)
                {
                   
                    //加判断
                    for (int i = 0; i <= nTime; i++)//nTime最大4
                    {
                        DateTime start = DateTime.Parse(startTime[i]);//新增公告开始时间
                        DateTime end = DateTime.Parse(endTime[i]);//新增公告结束时间
                       
                        foreach (DataRow dr in g_dt.Rows)
                        {
                            string dbBulletin_id = dr["bulletin_id"].ToString();
                            if (bulletin_id == dbBulletin_id)//不与被修改的那条记录做比较
                            {
                                continue;
                            }
                            DateTime dbStart = DateTime.Parse(dr["startime"].ToString());
                            DateTime dbEnd = DateTime.Parse(dr["endtime"].ToString());
                            if (end <= dbStart || dbEnd <= start)
                            {
                            }
                            else
                            {
                                msg = "与数据库公告时间段" + dr["startime"].ToString() + "--" + dr["endtime"].ToString() + "重复！";
                                index = true;
                                return index;
                            }
                        }
                    }
                }
                return index;
            }
            catch (Exception e)
            {
                msg = "判断数据库是否存在重复时间段的公告出错！";
                throw new Exception(msg + e.Message);
            }
        }

        //判断新增时间段自身重复
        protected bool IsRepeatedSelf(string[] startTime, string[] endTime, out string msg)
        {
            bool index = false;
            msg = "";
            try
            {
                int len = startTime.Length;
                if (startTime.Length != endTime.Length)
                {
                    msg = "判断新增时间段自身重复出错！";
                }
                for (int i = 0; i < len; i++)
                {
                    DateTime start = DateTime.Parse(startTime[i]);
                    DateTime end = DateTime.Parse(endTime[i]);
                    for (int j = 0; j < len; j++)
                    {
                        DateTime start2 = DateTime.Parse(startTime[j]);
                        DateTime end2 = DateTime.Parse(endTime[j]);
                        if (i == j)
                            continue;
                        else
                        {
                            if (end <= start2 || end2 <= start)
                            {
                            }
                            else
                            {
                                msg = "新增时间段" + start.ToString("yyyy-MM-dd HH:mm:ss") + "--" + end.ToString("yyyy-MM-dd HH:mm:ss") + "与"
                                    + start2.ToString("yyyy-MM-dd HH:mm:ss") + "--" + end2.ToString("yyyy-MM-dd HH:mm:ss") + "重复！";
                                index = true;
                                return index;
                            }
                        }
                    }
                }
                return index;
            }
            catch (Exception e)
            {
                msg = "判断新增时间段自身是否重复出错！";
                throw new Exception(msg+e.Message);
            }
        }
        
        //新增完银行公告隐藏增加的时间段
        private void TimesHide()
        {
            this.StartTime1.Visible = false;
            this.EndTime1.Visible = false;
            this.StartTime2.Visible = false;
            this.EndTime2.Visible = false;
            this.StartTime3.Visible = false;
            this.EndTime3.Visible = false;
            this.StartTime4.Visible = false;
            this.EndTime4.Visible = false;
        }

        protected void btInterfaceBack_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("BankInterfaceManage.aspx?sysid=" + sysid);
        }

        protected void ddlQueryBankTypeInterface_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.lbbanktypeInterface.Text = this.ddlQueryBankTypeInterface.SelectedValue.Trim();
        }

        private void InterfaceOpen_CheckedChanged(object sender, System.EventArgs e)
		{
            if (InterfaceOpen.Checked)
			{
                tbStartTime.Enabled = false;
                iStartTime.Visible = false;
                tbEndTime.Enabled = false;
                iEndTime.Visible = false;
                this.TRTimeSet.Visible = false;
                this.StartTime1.Visible = false;
                this.EndTime1.Visible = false;
                this.StartTime2.Visible = false;
                this.EndTime2.Visible = false;
                this.StartTime3.Visible = false;
                this.EndTime3.Visible = false; 
                this.StartTime4.Visible = false;
                this.EndTime4.Visible = false;
                nTime = 0;
			}
			else
			{
                tbStartTime.Enabled = true;
                iStartTime.Visible = true;
                tbEndTime.Enabled = true;
                iEndTime.Visible = true;
                this.TRTimeSet.Visible = true;
			}
		}

        ////弹层正文控制
        //private void cbForbid_CheckedChanged(object sender, System.EventArgs e)
        //{
        //    if (cbForbid.Checked)
        //    {
        //      //  TextTCMainText.Text = "";//避免连续新增时会提交上条公告的信息
        //        tcTextId.Visible = false;
        //    }
        //    else
        //        tcTextId.Visible = true;
        //}

        protected void closedRadio_SelectedIndexChanged(object sender, System.EventArgs e)
        {
           
        }
        protected void ForbidRadio_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.ForbidRadio.SelectedValue == "2")
            {
                tcTextId.Visible = true;
            }
            else
            {
                //  TextTCMainText.Text = "";//避免连续新增时会提交上条公告的信息
                tcTextId.Visible = false;
            }
        }

        protected void btAddTime_Click(object sender, System.EventArgs e)
        {
            int dbN = 0;
            if (!queryed)//若还没查询过数据库记录，则先查询
            {
                QueryBankBulletinOfBusiness(int.Parse(ViewState["sysid"].ToString()), int.Parse(this.closedRadio.SelectedValue), int.Parse(this.ddlQueryBankTypeInterface.SelectedValue.Trim()));
            }
           
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt == null)//记录数为0
                dbN = 0;
            else
                dbN = g_dt.Rows.Count;

            if (max - dbN - nTime > 0)
            {
                nTime++;//每点一次按钮时间段加一次i
                if (nTime == 1)
                {
                    StartTime1.Visible = true;
                    EndTime1.Visible = true;
                    this.tbStartTime1.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime1.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                }
                if (nTime == 2)
                {
                    StartTime2.Visible = true;
                    EndTime2.Visible = true;
                    this.tbStartTime2.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime2.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                }
                if (nTime == 3)
                {
                    StartTime3.Visible = true;
                    EndTime3.Visible = true;
                    this.tbStartTime3.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime3.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                }
                if (nTime == 4)
                {
                    StartTime4.Visible = true;
                    EndTime4.Visible = true;
                    this.tbStartTime4.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    this.tbEndTime4.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                }
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "不能再增加啦！");
                return;
            }

        }

        //公告
        protected string[,] GetBulletin(Query_Service.T_BANKBULLETIN_INFO bankbulletin)
        {
            string[,] param = new string[,] { { "IsNew", Convert.ToString(bankbulletin.IsNew) }, 
            { "IsOPen", Convert.ToString(bankbulletin.IsOPen) },
            { "bulletin_id", Convert.ToString(bankbulletin.bulletin_id) },
            { "banktype", Convert.ToString(bankbulletin.banktype) },
            { "businesstype", Convert.ToString(bankbulletin.businesstype) },
            { "op_support_flag", Convert.ToString(bankbulletin.op_support_flag) },
            { "closetype", Convert.ToString(bankbulletin.closetype) },
            { "title", Convert.ToString(bankbulletin.title) },
            { "maintext", Convert.ToString(bankbulletin.maintext) },
            { "popuptext", Convert.ToString(bankbulletin.popuptext) },
            { "startime", Convert.ToString(bankbulletin.startime) },
            { "endtime", Convert.ToString(bankbulletin.endtime) },
            { "createuser", Convert.ToString(bankbulletin.createuser) },
            { "createtime", Convert.ToString(bankbulletin.createtime) },
            { "updateuser", Convert.ToString(bankbulletin.updateuser) },
            //{ "Falwtime", Convert.ToString(bankbulletin.Falwtime) },
            { "returnUrl", Convert.ToString(bankbulletin.returnUrl) }};
            return param;
        }

        //获取某银行的业务子类型，限制公告条数及时间段重复
        protected void QueryBankBulletinOfBusiness(int businesstype, int op_flag, int banktype)
        {
            Query_Service.Query_Service qs = new Query_Service.Query_Service();
            Query_Service.T_BANKBULLETIN_INFO bankbulletin = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_BANKBULLETIN_INFO();
            DataSet ds = qs.QueryBankBulletin(businesstype, op_flag, banktype, "",8,0);//通过接口返回所有子类型记录，一般不超过5条
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["g_dt"] = ds.Tables[0];
                queryed = true;
            }
        }

        //protected void InterfaceLong_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    if (!InterfaceOpen.Checked)
        //        DropDownListShow();
        //}

        //private void DropDownListShow()
        //{
        //    if (this.InterfaceLong.SelectedValue == "1")
        //    {
        //        this.alwtime.Visible = true;
        //        this.StartTime.Visible = false;
        //        this.EndTime.Visible = false;
        //    }
        //    else
        //    {
        //        this.alwtime.Visible = false;
        //        this.StartTime.Visible = true;
        //        this.EndTime.Visible = true;
        //    }
        //}

        private static Check_WebService.Param[] ToParamArray(string[,] param)
        {
            Check_WebService.Param[] pa = new Check_WebService.Param[param.GetLength(0)];
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i] = new Check_WebService.Param();
                pa[i].ParamName = param[i, 0];
                pa[i].ParamValue = Convert.ToString(param[i, 1]);
                pa[i].ParamFlag = "";
            }
            return pa;
        }
	}
}