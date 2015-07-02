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
using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.SysManageModule;
using System.Collections.Generic;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace TENCENT.OSS.CFT.KF.KF_Web.SysManage
{
	/// <summary>
	/// SysBulletinManage_Detail 的摘要说明。
	/// </summary>
    public partial class BankInterfaceManage_Detail : System.Web.UI.Page
	{
        private string sysid, bulletinId, uctype;
        private static int nTime=0;//添加时间段
        SysManageService bll = new SysManageService();
        Dictionary<string, string> BankInterfaceName = SysManageService.BankInterfaceName;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				string szkey = Session["SzKey"].ToString();
				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

            try
            {
                if (!IsPostBack)
                {
                    #region onclick添加
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
                    # endregion

                    sysid = Request.QueryString["sysid"].Trim();
                    ViewState["sysid"] = sysid;

                    if (Request.QueryString["sysid"] != null && Request.QueryString["sysid"].Trim() != "")
                    {
                        #region 初始化页面和参数
                        tbStartTime.Enabled = true;
                        iStartTime.Visible = true;
                        tbEndTime.Enabled = true;
                        iEndTime.Visible = true;

                        //银行下拉列表
                        //setConfig.GetAllBankListFromDic(ddlQueryBankTypeInterface);
                        //ddlQueryBankTypeInterface.Items.Insert(0, new ListItem("所有银行", ""));
                        this.tbStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                        this.tbEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                        string instanceText = BankInterfaceName[sysid].ToString();//接口名称

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

                        if (Request.QueryString["opertype"] != null && Request.QueryString["opertype"].Trim() != "")
                        {
                            string opertype = Request.QueryString["opertype"].Trim();
                            if (opertype == "2")//编辑
                            {
                                btAddTime.Visible = false;
                                this.btInterfaceAdd.Visible = false;
                                this.btInterfaceUpdate.Visible = true;
                                this.btInterfaceBack.Visible = true;
                                hlInterfaceBack.Visible = false;
                                labTitle.Text = instanceText + "修改";
                                BindBankInterface(bulletinId, "");
                            }
                            else if (opertype == "1")//新增
                            {
                                if (Session["BankTypeList"] != null)
                                {
                                    ArrayList BankTypeList = (ArrayList)Session["BankTypeList"];
                                    if (BankTypeList == null || BankTypeList.Count < 0)
                                    {
                                        WebUtils.ShowMessage(this.Page, "新增公告请输入银行编码！");
                                        return;
                                    }
                                    BindBank(BankTypeList);
                                    Session.Remove("BankTypeList");
                                }
                                else
                                {
                                    WebUtils.ShowMessage(this.Page, "新增公告请输入银行编码！");
                                    return;
                                }

                                this.btInterfaceAdd.Visible = true;
                                this.btInterfaceUpdate.Visible = false;
                                this.btInterfaceBack.Visible = true;
                                hlInterfaceBack.Visible = false;
                                this.InterfaceOpen.Visible = false;
                                labTitle.Text = instanceText + "新增";
                            }
                            else
                            {
                                this.btInterfaceAdd.Visible = false;
                                this.btInterfaceUpdate.Visible = false;
                                this.btInterfaceBack.Visible = false;
                                hlInterfaceBack.Visible = true;
                                labTitle.Text = "查看" + instanceText;
                                if (Request.QueryString["objid"] != null && Request.QueryString["objid"].Trim() != "")
                                {//账务系统过来查看含参数objid
                                    string objid = Request.QueryString["objid"].Trim();
                                    ViewState["objid"] = objid;
                                    BindBankInterface("", objid);
                                }
                                else//客服系统查看无objid参数
                                    BindBankInterface(bulletinId, "");
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //出错了，不能不带参数时来
                        Response.Redirect("../login.aspx?wh=1");
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
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page,PublicRes.GetErrorMsg(ex.Message));
                return;
            }
		}

        protected void BindBankInterface(string bulletinId, string objid)
        {
            try
            {
                commData.T_BANKBULLETIN_INFO bankbulletin = new commData.T_BANKBULLETIN_INFO();
                bankbulletin = GetBulletin(ViewState["sysid"].ToString(),bulletinId, objid);

                if (bankbulletin == null)
                {
                    WebUtils.ShowMessage(this.Page, "查询出错！");
                    return;
                }
                ArrayList bankTypeList = new ArrayList();
                bankTypeList.Add(bankbulletin.banktype.Trim());
                BindBank(bankTypeList);

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

        //查询公告详情
        public commData.T_BANKBULLETIN_INFO GetBulletin(string sysid, string bulletinId, string objid)
        {
            commData.T_BANKBULLETIN_INFO bankbulletin = new commData.T_BANKBULLETIN_INFO();
            DataSet ds = new DataSet();
            if (bulletinId != null && bulletinId != "")
            {
                int total_num=0; 
                ds = bll.QueryBankBulletin(sysid, 0, 0, bulletinId,
                    "", "", "", "", "", 5, 0,out total_num);//通过接口返回记录
                bankbulletin = bll.TurnBankBulletinClass(ds);//将记录转换成公告类
            }
            else if (objid != null && objid != "")
            {
                bankbulletin = bll.QueryBankBulletinByObjid(objid, "BankBulletinForType");
            }
            return bankbulletin;
        }

        //绑定银行列表
        private void BindBank(ArrayList bankTypeList)
        {
            Hashtable bank = BankIO.GetBankHashTable();
            List<Bank> listBank = new List<Bank>();
          
            foreach (string s in bankTypeList)
            {
                Bank bankOne = new Bank();
                bankOne.banktype = s;
                try
                {
                    bankOne.banktype_str = bank[bankOne.banktype].ToString();
                }
                catch
                {
                    bankOne.banktype_str = "未知" + bankOne.banktype;
                    //WebUtils.ShowMessage(this.Page, bankOne.banktype+"：未查到银行类型!");
                    //return;
                }
                listBank.Add(bankOne);
            }
           
            if (listBank != null && listBank.Count != 0)
            {
                DatagridBank.DataSource = listBank;
                ViewState["bankType"] = listBank;
            }
            else
            {
                ViewState["bankType"] = null;
                WebUtils.ShowMessage(this.Page, "银行类型有误!");
                DatagridBank.DataSource = null;
            }
            DatagridBank.DataBind();
        }

        protected void btInterfaceAdd_Click(object sender, System.EventArgs e)
        {
            this.InterfaceOpen.Enabled = false;
            OperateBankInterFace(true);
            this.btInterfaceAdd.Visible = false;
        }

        protected void btInterfaceUpdate_Click(object sender, System.EventArgs e)
        {
            btAddTime.Enabled = false;
            OperateBankInterFace(false);
            this.btInterfaceUpdate.Visible = false;
        }

        /// 新增或修改公告
        /// <param name="isAdd">标记是否新增：true新增，false修改</param>
        protected void OperateBankInterFace(bool isAdd)
        {
            try
            {
                //银行类型限制
                List<Bank> bankList = BankTypeLimit();

                string sysid = ViewState["sysid"].ToString();

                #region 输入限制
                int n = DateTime.Now.ToString("MM月dd日HH:mm").Length;
                if (this.tbInterfaceMainText.Text.Trim().Length > 71)//80-11+7
                {
                    WebUtils.ShowMessage(this.Page, "正文超过50字符,请重新输入！");
                    return;
                }
                if (this.TextTCMainText.Text.Trim().Length > 53)//80-11*2+8+7
                {
                    WebUtils.ShowMessage(this.Page, "弹层正文超过80字符,请重新输入！");
                    return;
                }

                string popuptext = "";
                string TCMain = TextTCMainText.Text.Trim().Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                //软关闭，必须有弹层，标题及弹层必须有一个，所以只确保弹层即可
                if (this.ForbidRadio.SelectedValue == "2")
                {
                    if (TCMain == "" || TCMain == null)
                    {
                        WebUtils.ShowMessage(this.Page, "软关闭必须输入弹层正文！");
                        return;
                    }
                    popuptext = TCMain;
                }
                #endregion

                //获取时间数组
                string[] startTime;
                string[] endTime;
                GetTime(out startTime, out endTime);

                #region 公告相同参数部分
                commData.T_BANKBULLETIN_INFO bankbulletin = new commData.T_BANKBULLETIN_INFO();
                bankbulletin.IsOPen = this.InterfaceOpen.Checked;
            //    bankbulletin.title = tbTitle.Text.Trim();
                bankbulletin.createuser = Session["uid"].ToString();
                bankbulletin.updateuser = Session["uid"].ToString();
                bankbulletin.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                bankbulletin.businesstype = sysid;
                if (sysid == "6")//支付有子类型：1.全部关闭  2.签约  3.支付  4.退款
                {
                    bankbulletin.op_support_flag = this.closedRadio.SelectedValue;
                }
                else
                    bankbulletin.op_support_flag = "1";

                if (this.ForbidRadio.SelectedValue == "1")
                    bankbulletin.closetype = "1";
                else if (this.ForbidRadio.SelectedValue == "2")
                    bankbulletin.closetype = "2";
                else if (this.ForbidRadio.SelectedValue == "3")
                    bankbulletin.closetype = "3";
                #endregion

                #region 提出公告申请
                string objid = "";
                string maintext = this.tbInterfaceMainText.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                string title = this.tbTitle.Text.Trim().Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                string outStr = "";
                for (int i = 0; i <= nTime; i++)//nTime最大4
                {
                    try
                    {
                        bankbulletin.startime = startTime[i];
                        bankbulletin.endtime = endTime[i];

                        if (this.InterfaceOpen.Checked)//若立即启用则结束时间为当前时间+5min
                            bankbulletin.endtime = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");

                        bankbulletin.maintext = maintext.Replace("endtime", DateTime.Parse(bankbulletin.endtime).ToString("MM月dd日HH:mm"));
                        bankbulletin.popuptext = popuptext.Replace("startime", DateTime.Parse(bankbulletin.startime).ToString("MM月dd日HH:mm"))
                            .Replace("endtime", DateTime.Parse(bankbulletin.endtime).ToString("MM月dd日HH:mm"));

                        foreach (Bank b in bankList)
                        {
                            if (!isAdd)//被修改的那条公告id为旧的，不是新增
                            {
                                bankbulletin.bulletin_id = this.tbbulletin_id.Text.Trim();
                                bankbulletin.IsNew = false;
                                bankbulletin.createtime = this.tbcreatetime.Text.Trim();
                            }
                            else
                            {
                                bankbulletin.bulletin_id = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage();//每个银行公告ID
                                bankbulletin.IsNew = true;
                                bankbulletin.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }

                            bankbulletin.title = title.Replace("bankName",b.banktype_str.Trim());
                            objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage(); //每个审批单
                            bankbulletin.banktype = b.banktype.Trim();
                            bankbulletin.returnUrl = "http://kf.cf.com/SysManage/BankInterfaceManage_Detail.aspx?Fbanktype=" + b.banktype.Trim() + "&sysid=" + sysid + "&objid=" + objid + "&opertype=0";

                            CheckBulletin(this, bankbulletin, objid);
                        }

                    }
                    catch (Exception err)
                    {
                        string errOne = bankbulletin.banktype + ":" + bankbulletin.startime + "--" + bankbulletin.endtime + "|";
                        outStr += errOne;
                        LogHelper.LogInfo(errOne + " 维护异常：" + err);
                    }
                }
                #endregion

                if (outStr != "")
                {
                    WebUtils.ShowMessage(this.Page, "接口维护异常：" + outStr);
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "全部银行接口维护申请已提交，请等待审批！");
                }
                nTime = 0;
                TimesHide();
                return;
            }
            catch (Exception err) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                WebUtils.ShowMessage(this.Page, "银行接口维护异常：" + errStr);
            }
        }

        //待操作银行类型限制
        private List<Bank> BankTypeLimit()
        {
            List<Bank> bankList = (List<Bank>)ViewState["bankType"];
            string limitstr = "";
            if (bankList != null)
            {
                foreach (Bank b in bankList)
                {
                    if (b.banktype_str.Contains("*"))
                        limitstr += b.banktype + ";";
                }
            }
            else
                throw new Exception("未选定银行类型！");

            if (limitstr!="")
            {
                throw new Exception(limitstr+" 已下线,请剔除该银行编码再操作！");
            }
            return bankList;
        }

        //获取公告时间段
        private void GetTime(out string[] startTime, out string[] endTime)
        {
            startTime = new string[nTime + 1];
            endTime = new string[nTime + 1];

            for (int i = 0; i <= nTime; i++)//nTime最大4
            {
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
                    throw new Exception("请输入正确的日期");
                }
            }

            string msg = "";
            //判断新增时间段自身重复
            IsRepeatedSelf(startTime, endTime);
        }

        /// <summary>
        /// 发起公告审批
        /// </summary>
        /// <param name="bankbulletin">公告类</param>
        /// <param name="objid">objid</param>
        public void CheckBulletin(System.Web.UI.Page page,commData.T_BANKBULLETIN_INFO bankbulletin, string objid)
        {
            string[,] param = GetBulletin(bankbulletin);//获得单条公告

            Check_WebService.Param[] pa = ToParamArray(param);

            string memo = "新增银行接口维护信息,银行类型:" + bankbulletin.banktype.Trim();

            PublicRes.CreateCheckService(page).StartCheck(objid,
                "BankBulletinForType", memo, "0", pa);
        }

        //判断新增时间段自身重复
        protected bool IsRepeatedSelf(string[] startTime, string[] endTime)
        {
            bool index = false;
            try
            {
                int len = startTime.Length;
                if (startTime.Length != endTime.Length)
                {
                    throw new Exception("判断新增时间段自身重复出错！");
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
                                throw new Exception("新增时间段" + start.ToString("yyyy-MM-dd HH:mm:ss") + "--" + end.ToString("yyyy-MM-dd HH:mm:ss") + "与"
                                    + start2.ToString("yyyy-MM-dd HH:mm:ss") + "--" + end2.ToString("yyyy-MM-dd HH:mm:ss") + "重复！");
                        }
                    }
                }
                return index;
            }
            catch (Exception e)
            {
                throw new Exception("判断新增时间段自身是否重复出错:" + e.Message);
            }
        }
        
        //新增完银行公告,隐藏增加的时间段
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

        //立即开启按钮控制是否显示时间段
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

        protected void ForbidRadio_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.ForbidRadio.SelectedValue == "2")
            {
                tcTextId.Visible = true;
            }
            else
            {
                tcTextId.Visible = false;
            }
        }

        protected void btAddTime_Click(object sender, System.EventArgs e)
        {
            nTime++;//每点一次按钮时间段加一次i
            if (nTime == 1)
            {
                StartTime1.Visible = true;
                EndTime1.Visible = true;
                this.tbStartTime1.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                this.tbEndTime1.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            }
            else if (nTime == 2)
            {
                StartTime2.Visible = true;
                EndTime2.Visible = true;
                this.tbStartTime2.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                this.tbEndTime2.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            }
            else if (nTime == 3)
            {
                StartTime3.Visible = true;
                EndTime3.Visible = true;
                this.tbStartTime3.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                this.tbEndTime3.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            }
            else if (nTime == 4)
            {
                StartTime4.Visible = true;
                EndTime4.Visible = true;
                this.tbStartTime4.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                this.tbEndTime4.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                btAddTime.Enabled = false;
            }
        }

        //公告
        protected string[,] GetBulletin(commData.T_BANKBULLETIN_INFO bankbulletin)
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

        protected void btInterfaceBack_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("BankInterfaceManage.aspx?sysid=" + sysid);
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
	}

     [Serializable]
    class Bank
    {
        public string banktype { get; set; }
        public string banktype_str { get; set; }
    }

}
