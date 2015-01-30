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
	public partial class SysBulletinManage_Detail : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lbTitle;
		protected System.Web.UI.WebControls.DropDownList ddlSysList;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		protected System.Web.UI.WebControls.Button btnNew;
		protected System.Web.UI.WebControls.Button btnIssue;
        private string sysid, fid, uctype;
	
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
			if(!IsPostBack)
			{
                ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
                ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
                ibstarttime.Attributes.Add("onclick", "openBankModeBegin()");
                ibendtime.Attributes.Add("onclick", "openBankModeEnd()");
                iInterfaceStartTime.Attributes.Add("onclick", "openBankInterfaceModeBegin()");
                iInterfaceEndTime.Attributes.Add("onclick", "openBankInterfaceModeEnd()");
                this.btadd.Attributes.Add("onclick", "return confirm(\"你确认要进行新增操作申请吗？\");");
                this.btupdate.Attributes.Add("onclick", "return confirm(\"你确认要进行修改操作申请吗？\");");

                ib_ucstarttime.Attributes.Add("onclick", "openUCModeBegin()");
                ib_ucendtime.Attributes.Add("onclick", "openUCModeEnd()");
                this.bt_ucupdate.Attributes.Add("onclick", "return confirm(\"你确认要进行修改操作申请吗？\");");

                sysid = Request.QueryString["sysid"].Trim();
                ViewState["sysid"] = sysid;

                if (Request.QueryString["sysid"] != null && Request.QueryString["sysid"].Trim() != "")
                {
                    #region
                    
                    #region
                    if (sysid == "8")
                    {
						tbstarttime.Enabled=true;
						ibstarttime.Visible=true;
						tbendtime.Enabled=true;
						ibendtime.Visible=true;

                        this.Table1.Visible = false;
                        this.Table2.Visible = true;
                        Table3.Visible = false;
                        Table4.Visible = false;

                        //银行下拉列表
                        setConfig.GetAllBankListFromDic(ddlQueryBankTypeInterface);
                        ddlQueryBankTypeInterface.Items.Insert(0, new ListItem("所有银行", ""));

                        this.tbstarttime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                        this.tbendtime.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                        if (Request.QueryString["opertype"] != null && Request.QueryString["opertype"].Trim() != "")
                        {
                            string opertype = Request.QueryString["opertype"].Trim();
                            if (opertype == "2")
                            {
                                this.btadd.Visible = false;
                                this.btupdate.Visible = true;
                                this.btback.Visible = true;
                                hlBack.Visible = false;
                            }
                            else if (opertype == "1")
                            {
                                this.btadd.Visible = true;
                                this.btupdate.Visible = false;
                                this.btback.Visible = true;
                                hlBack.Visible = false;
								this.cbopen.Visible=false;
                            }
                            else
                            {
                                this.btadd.Visible = false;
                                this.btupdate.Visible = false;
                                this.btback.Visible = false;
                                hlBack.Visible = true;
                                if (Request.QueryString["objid"] != null && Request.QueryString["objid"].Trim() != "")
                                {
                                    string objid = Request.QueryString["objid"].Trim();
                                    ViewState["objid"] = objid;
                                    BindBankBulletin("", objid);
                                }

                            }
                        }

                    }
                    #endregion
                    #region
                    else if (sysid == "21"||sysid == "22"||sysid == "23"||sysid == "24")
                    {
                        tbInterfaceStartTime.Enabled = true;
                        iInterfaceStartTime.Visible = true;
                        tbInterfaceEndTime.Enabled = true;
                        iInterfaceEndTime.Visible = true;

                        this.Table1.Visible = false;
                        this.Table2.Visible = false;
                        Table3.Visible = false;
                        Table4.Visible = true;

                        //DropDownListShow();//根据radio显示时间

                        //银行下拉列表
                        setConfig.GetAllBankListFromDic(ddlQueryBankTypeInterface);
                        ddlQueryBankTypeInterface.Items.Insert(0, new ListItem("所有银行", ""));
                        this.tbInterfaceStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                        this.tbInterfaceEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                        if (Request.QueryString["opertype"] != null && Request.QueryString["opertype"].Trim() != "")
                        {
                            string opertype = Request.QueryString["opertype"].Trim();
                            if (opertype == "2")
                            {
                                this.btInterfaceAdd.Visible = false;
                                this.btInterfaceUpdate.Visible = true;
                                this.btInterfaceBack.Visible = true;
                                hlInterfaceBack.Visible = false;
                            }
                            else if (opertype == "1")
                            {
                                this.btInterfaceAdd.Visible = true;
                                this.btInterfaceUpdate.Visible = false;
                                this.btInterfaceBack.Visible = true;
                                hlInterfaceBack.Visible = false;
                                this.InterfaceOpen.Visible = false;
                            }
                            else
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

                    }
                    #endregion
                    #region
                    else if (sysid == "7") //生活缴费
                    {

                        this.Table1.Visible = false;
                        this.Table2.Visible = false;
                        Table3.Visible = true;
                        Table4.Visible = false;

                        this.tb_ucstarttime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                        this.tb_ucendtime.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                        if (Request.QueryString["opertype"] != null && Request.QueryString["opertype"].Trim() != "")
                        {
                            string opertype = Request.QueryString["opertype"].Trim();
                            if (opertype == "2")
                            {
                                this.bt_ucupdate.Visible = true;
                                this.bt_ucback.Visible = true;
                                hl_ucback.Visible = false;
                            }
                            else
                            {
                                this.bt_ucupdate.Visible = false;
                                this.bt_ucback.Visible = false;
                                hl_ucback.Visible = true;
                                if (Request.QueryString["objid"] != null && Request.QueryString["objid"].Trim() != "")
                                {
                                    string objid = Request.QueryString["objid"].Trim();
                                    ViewState["objid"] = objid;
                                    BindUCBulletin("", "", objid); //审批时的查看。
                                }

                            }
                        }
                       
                        if (Request.QueryString["uctype"] != null && Request.QueryString["uctype"].Trim() != "")
                        {
                            uctype = Request.QueryString["uctype"];
                        }
                        else
                            uctype = "8";

                        ViewState["uctype"] = uctype;

                        if (uctype == "1")
                            lbuctype.Text = "每日";
                        else if (uctype == "2")
                            lbuctype.Text = "每周";
                        else if (uctype == "4")
                            lbuctype.Text = "每月";
                        else if (uctype == "8")
                            lbuctype.Text = "维护";
                    } 
                    #endregion
                    else
                    {
                        if (sysid == "14" || sysid == "11" || sysid == "12" || sysid == "13" || sysid == "51" || sysid == "52" || sysid == "53" || sysid == "54" || sysid == "55" || sysid == "56")
                        {
                            Isred.Visible = false;
                            Isnew.Visible = false;
                        }
                        this.Table1.Visible = true;
                        this.Table2.Visible = false;
                        Table3.Visible = false;
                        Table4.Visible = false;
                    }
                    #endregion
                }
                else
                {
                    //出错了，不能不带参数时来
                    Response.Redirect("../login.aspx?wh=1");
                }

                if (Request.QueryString["fid"] != null && Request.QueryString["fid"].Trim() != "")
                {
                    fid = Request.QueryString["fid"].Trim();
                    ViewState["fid"] = fid;
                }
                else
                {
                    fid = "";
                    ViewState["fid"] = fid;
                }

                if (fid == "")
                {
                    if (sysid == "51" || sysid == "52" || sysid == "53" || sysid == "54" || sysid == "55" || sysid == "56")
                    {
                        labTitle.Text = "常见问题新增";
                    }
                    else if (sysid == "21" || sysid == "22" || sysid == "23" || sysid == "24")
                    {
                        labTitle.Text = "银行接口新增";
                    }
                    else
                        labTitle.Text = "新增公告";

                    if (ViewState["objid"] != null)
                    {
                        labTitle.Text = "查看公告";
                    }
                }
                else
                {
                    if (sysid == "51" || sysid == "52" || sysid == "53" || sysid == "54" || sysid == "55" || sysid == "56")
                    {
                        labTitle.Text = "常见问题修改";
                    }
                    else if (sysid == "21" || sysid == "22" || sysid == "23" || sysid == "24")
                    {
                        labTitle.Text = "银行接口修改";
                    }
                    else
                        labTitle.Text = "修改公告";

                    if (ViewState["sysid"].ToString() == "8")
                    {
                        BindBankBulletin(fid, "");
                    }
                    else if (sysid == "21" || sysid == "22" || sysid == "23" || sysid == "24")
                    {
                        BindBankInterface(fid, "");
                    }
                    else if (ViewState["sysid"].ToString() == "7")
                    {
                        BindUCBulletin(fid, uctype, "");
                    }
                    else
                    {

                        BindData(fid);
                    }
                }
            }
            else
            {
                fid = ViewState["fid"].ToString();
                sysid = ViewState["sysid"].ToString();
            }
		}

        private void BindData(string fid)
		{
           
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
            qs.Finance_HeaderValue = fh;

			string msg = "";
			Query_Service.SysBulletinClass sc = qs.GetOneSysBulletin(fid,out msg);
			if(sc != null)
			{
				//绑定开始
				tbTitle.Text = sc.FTitle;
				tbIssueTime.Text = DateTime.Parse(sc.FissueTime).ToString("yyyy年MM月dd日");

				if(sc.FLastTime != "")
					tbLastTime.Text = DateTime.Parse(sc.FLastTime).ToString("yyyy年MM月dd日");

				tbUrl.Text = sc.FUrl;
				tbUserID.Text = sc.FuserId;
				rbIsnew.SelectedIndex = sc.FIsNew - 1;
				rbIsred.SelectedValue = sc.FIsRed.ToString();
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"读取失败：" + msg);
			}
		}

        private void BindBankBulletin(string fid, string objid)
        {
            Query_Service.Query_Service qs = new Query_Service.Query_Service();
            string msg = "";
            Query_Service.T_BANKBULLETIN_INFO_ALL bankbulletin = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_BANKBULLETIN_INFO_ALL();

            if (fid != null && fid != "")
            {
                bankbulletin = qs.QueryBankBulletinById(fid, out msg);
            }
            if (objid != null && objid != "")
            {
                bankbulletin = qs.QueryBankBulletinInfoByObjid(objid, "ConfigBankBulletin", out msg);
            }

            this.ddlQueryBankType.SelectedValue = bankbulletin.Fbanktype.Trim();
            this.tbbanktitle.Text = bankbulletin.Ftitle.Trim();
            this.tbmaintext.Text = bankbulletin.Fmaintext.Trim().Replace("<br/>", "\r\n");
            this.tbstarttime.Text = bankbulletin.Fstartime.Trim();
            this.tbendtime.Text = bankbulletin.Fendtime.Trim();
            this.tbcreateuser.Text = bankbulletin.Fcreateuser.Trim();
		    this.cbopen.Visible=true;
        }

        private void BindBankInterface(string fid, string objid)
        {
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                string msg = "";
                Query_Service.T_BANKBULLETIN_TYPE_ALL bankbulletin = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_BANKBULLETIN_TYPE_ALL();

                if (fid != null && fid != "")
                {
                    bankbulletin = qs.QueryBankInterfaceById(fid, out msg);
                }
                if (objid != null && objid != "")
                {
                    bankbulletin = qs.QueryBankInterfaceByObjid(objid, "BankBulletinForType", out msg);
                }
             
                this.ddlQueryBankTypeInterface.SelectedValue = bankbulletin.Fbanktype.Trim();
                this.tbInterfaceMainText.Text = bankbulletin.Fmaintext.Trim().Replace("<br/>", "\r\n");
                this.tbInterfaceStartTime.Text = bankbulletin.Fstartime.Trim();
                this.tbInterfaceEndTime.Text = bankbulletin.Fendtime.Trim();
           //     this.tbalwtime.Text = bankbulletin.Falwtime.Trim();//有效时间段
                this.tbcreateuserInterface.Text = bankbulletin.Fcreateuser.Trim();
                this.InterfaceOpen.Visible = true;
                this.tbarea.Text = bankbulletin.Farea.Trim();
                this.tbcity.Text = bankbulletin.Fcity.Trim();
                this.tbbusinetype.Text = bankbulletin.Fbusinetype.Trim();
                this.TextTCMainText.Text = bankbulletin.Ftctext.Trim();
                this.tbFid.Text = bankbulletin.Fid.Trim();
                if (bankbulletin.Isaffectinterface.Trim() == "1")
                {
                    this.cbForbid.Checked = true;
                    tcTextId.Visible = false;
                }
                else
                {
                    this.cbForbid.Checked = false;
                    tcTextId.Visible = true;
                }
                //if (bankbulletin.Falwaysworkstate.Trim() == "1")
                //    this.InterfaceLong.SelectedValue = "1";
                //else
                //    this.InterfaceLong.SelectedValue = "0";
              
                //DropDownListShow();//根据radio显示时间

               

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

        private void BindUCBulletin(string fid, string uctype, string objid)
        {
            Query_Service.Query_Service qs = new Query_Service.Query_Service();
            string msg = "";
            Query_Service.T_PUCNEWSERVICE_INFO bankbulletin = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_PUCNEWSERVICE_INFO();
            if (fid != null && fid != "")
            {
                bankbulletin = qs.QueryUCBulletinById(fid, uctype, out msg);
            }
            if (objid != null && objid != "")
            {
                bankbulletin = qs.QueryUCBulletinInfoByObjid(objid, "ConfigUCBulletin", out msg);
            }

            this.tbServicecode.Text = bankbulletin.Fservicecode.Trim();
            this.tb_uc_text.Text = bankbulletin.Ftips.Trim().Replace("<br/>", "\r\n");
            this.tb_ucstarttime.Text = bankbulletin.Fstartime.Trim();
            this.tb_ucendtime.Text = bankbulletin.Fendtime.Trim();
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
			this.cbopen.CheckedChanged += new System.EventHandler(this.cbopen_CheckedChanged);
            this.InterfaceOpen.CheckedChanged += new System.EventHandler(this.InterfaceOpen_CheckedChanged);
            this.cbForbid.CheckedChanged += new System.EventHandler(this.cbForbid_CheckedChanged);
		}
		#endregion

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
            qs.Finance_HeaderValue = fh;

			string msg = "";
			Query_Service.SysBulletinClass sc = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.SysBulletinClass();
			sc.FSysID = Int32.Parse(sysid);
			sc.FIsNew = Int32.Parse(rbIsnew.SelectedValue);
			sc.FTitle = classLibrary.setConfig.replaceMStr(tbTitle.Text.Trim());
			sc.FIsRed = Int32.Parse(rbIsred.SelectedValue);

			if(sc.FTitle == "")
			{
				WebUtils.ShowMessage(this.Page,"请输入标题");
				return;
			}

            if (sc.FTitle.Length>16)
            {
                WebUtils.ShowMessage(this.Page, "标题长度超过16字符");
                return;
            }

			sc.FUrl = classLibrary.setConfig.replaceMStr(tbUrl.Text.Trim());
			if(sc.FUrl == "")
			{
				WebUtils.ShowMessage(this.Page,"请输入链接地址");
				return;
			}

			sc.FuserId = classLibrary.setConfig.replaceMStr(tbUserID.Text.Trim());
			if(sc.FuserId == "")
			{
				WebUtils.ShowMessage(this.Page,"请输入发布人");
				return;
			}

			try
			{
				sc.FissueTime = DateTime.Parse(tbIssueTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
				if(tbLastTime.Text.Trim() != "")
				{
					sc.FLastTime = DateTime.Parse(tbLastTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
				}
				else
					sc.FLastTime = "";
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"请输入正确的日期");
				return ;
			}

			if(!sc.FUrl.ToLower().StartsWith("http"))
				sc.FUrl = "http://" + sc.FUrl;

			//修改
			if(fid != "")
			{
				//修改。
				sc.FID = Int32.Parse(fid);	
				if(qs.ChangeOneSysBulletin(sc,Request.UserHostAddress,out msg))
				{
					WebUtils.ShowMessage(this.Page,"修改成功");
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"修改失败：" + msg);
				}
			}
			else
			{
				//新增
				sc.FID = 0;

				if(qs.AddOneSysBulletin(sc,Request.UserHostAddress,out msg))
				{
					WebUtils.ShowMessage(this.Page,"新增成功");
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"新增失败：" + msg);
				}
			}
		}

        protected void btnBack_Click(object sender, System.EventArgs e)
        {
            if (sysid == "51" || sysid == "52" || sysid == "53" || sysid == "54" || sysid == "55" || sysid == "56")
                Response.Redirect("QuestionManage.aspx?sysid=" + sysid);
            else
                Response.Redirect("SysBulletinManage.aspx?sysid=" + sysid);
        }

        protected void btadd_Click(object sender, System.EventArgs e)
        {
            string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            Query_Service.T_BANKBULLETIN_INFO_ALL bankbulletin = new Query_Service.T_BANKBULLETIN_INFO_ALL();
            bankbulletin.IsNew = true;
			bankbulletin.IsOPen=false;
            bankbulletin.Fbanktype = this.ddlQueryBankType.SelectedValue.Trim();
            bankbulletin.Ftitle = this.tbbanktitle.Text.Trim(); ;
            bankbulletin.Fmaintext = this.tbmaintext.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            bankbulletin.Fstartime = this.tbstarttime.Text.Trim();
            bankbulletin.Fendtime = this.tbendtime.Text.Trim();
            bankbulletin.Fcreateuser = Session["uid"].ToString();
            bankbulletin.Fcreatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            bankbulletin.Fupdateuser = Session["uid"].ToString();
            bankbulletin.returnUrl = "/SysManage/SysBulletinManage_Detail.aspx?Fbanktype=" + this.ddlQueryBankType.SelectedValue.Trim() + "&sysid=8&objid=" + objid + "&opertype=0";

            string[,] param = new string[,] { { "IsNew", Convert.ToString(bankbulletin.IsNew) }, 
            { "IsOPen", Convert.ToString(bankbulletin.IsOPen) },
            { "Fbanktype", Convert.ToString(bankbulletin.Fbanktype) },
            { "Ftitle", Convert.ToString(bankbulletin.Ftitle) },
            { "Fmaintext", Convert.ToString(bankbulletin.Fmaintext) },
            { "Fstartime", Convert.ToString(bankbulletin.Fstartime) },
            { "Fendtime", Convert.ToString(bankbulletin.Fendtime) },
            { "Fcreateuser", Convert.ToString(bankbulletin.Fcreateuser) },
            { "Fcreatetime", Convert.ToString(bankbulletin.Fcreatetime) },
            { "Fupdateuser", Convert.ToString(bankbulletin.Fupdateuser) },
            { "returnUrl", Convert.ToString(bankbulletin.returnUrl) },};

            Check_WebService.Param[] pa=ToParamArray(param);


            string memo = "新增银行维护公告信息,银行类型:" + this.ddlQueryBankType.SelectedItem.Text.Trim(); ;
            PublicRes.CreateCheckService(this).StartCheck(objid,
                "ConfigBankBulletin", memo, "0", pa);
            PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "ConfigBankBulletin", "新增银行维护公告申请审批", 1, this.ddlQueryBankType.SelectedValue.Trim(), "");
            WebUtils.ShowMessage(this.Page, "新增银行维护公告申请已提交，请等待审批！");
        }

        protected void btupdate_Click(object sender, System.EventArgs e)
        {
            string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string fbanktype = this.ddlQueryBankType.SelectedValue.Trim();
            Query_Service.T_BANKBULLETIN_INFO_ALL bankbulletin = new Query_Service.T_BANKBULLETIN_INFO_ALL();
            bankbulletin.IsNew = false;
			bankbulletin.IsOPen=this.cbopen.Checked;
            bankbulletin.Fbanktype = fbanktype;
            bankbulletin.Ftitle = this.tbbanktitle.Text.Trim(); ;
            bankbulletin.Fmaintext = this.tbmaintext.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            bankbulletin.Fupdateuser = Session["uid"].ToString();
            bankbulletin.Fstartime = this.tbstarttime.Text.Trim();
            bankbulletin.Fendtime = this.tbendtime.Text.Trim();
            bankbulletin.Fcreateuser = this.tbcreateuser.Text.Trim();
            bankbulletin.returnUrl = "/SysManage/SysBulletinManage_Detail.aspx?Fbanktype=" + fbanktype + "&sysid=8&objid=" + objid + "&opertype=0";

            string[,] param = new string[,] { { "IsNew", Convert.ToString(bankbulletin.IsNew) }, 
            { "IsOPen", Convert.ToString(bankbulletin.IsOPen) },
            { "Fbanktype", Convert.ToString(bankbulletin.Fbanktype) },
            { "Ftitle", Convert.ToString(bankbulletin.Ftitle) },
            { "Fmaintext", Convert.ToString(bankbulletin.Fmaintext) },
            { "Fstartime", Convert.ToString(bankbulletin.Fstartime) },
            { "Fendtime", Convert.ToString(bankbulletin.Fendtime) },
            { "Fcreateuser", Convert.ToString(bankbulletin.Fcreateuser) },
            { "Fcreatetime", Convert.ToString(bankbulletin.Fcreatetime) },
            { "Fupdateuser", Convert.ToString(bankbulletin.Fupdateuser) },
            { "returnUrl", Convert.ToString(bankbulletin.returnUrl) },};

            Check_WebService.Param[] pa = ToParamArray(param);

            string memo = "银行维护公告信息,银行类型:" + this.ddlQueryBankType.SelectedItem.Text.Trim(); ;
            PublicRes.CreateCheckService(this).StartCheck(objid,
                "ConfigBankBulletin", memo, "0", pa);
            PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "ConfigBankBulletin", "修改银行维护公告申请审批", 1, fbanktype, "");
            WebUtils.ShowMessage(this.Page, "修改银行公告申请已提交，请等待审批！");
        }

        protected void btback_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("SysBulletinManage.aspx?sysid=" + sysid);
        }

        protected void btInterfaceAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                string sysid = ViewState["sysid"].ToString();
                if (this.tbInterfaceMainText.Text.Trim().Length > 30)
                {
                    WebUtils.ShowMessage(this.Page, "正文超过30字符,请重新输入！");
                    return;
                }
                if (this.TextTCMainText.Text.Trim().Length > 60)
                {
                    WebUtils.ShowMessage(this.Page, "弹层正文超过60字符,请重新输入！");
                    return;
                }

                Query_Service.T_BANKBULLETIN_TYPE_ALL bankbulletin = new Query_Service.T_BANKBULLETIN_TYPE_ALL();
                bankbulletin.Fid = "";
                bankbulletin.IsNew = true;
                bankbulletin.IsOPen = false;
                bankbulletin.Ftitle = "";
                bankbulletin.Fbanktype = this.ddlQueryBankTypeInterface.SelectedValue.Trim();
                bankbulletin.Fmaintext = this.tbInterfaceMainText.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                try
                {
                    bankbulletin.Fstartime = DateTime.Parse(this.tbInterfaceStartTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                    bankbulletin.Fendtime = DateTime.Parse(this.tbInterfaceEndTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "请输入正确的日期");
                    return;
                }
                bankbulletin.Fcreateuser = Session["uid"].ToString();
                bankbulletin.Fcreatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                bankbulletin.Fupdateuser = Session["uid"].ToString();
                bankbulletin.returnUrl = "http://kf.cf.com/SysManage/SysBulletinManage_Detail.aspx?Fbanktype=" + this.ddlQueryBankTypeInterface.SelectedValue.Trim() + "&sysid=" + sysid + "&objid=" + objid + "&opertype=0";
                bankbulletin.Farea = this.tbarea.Text.Trim();
                bankbulletin.Fcity = this.tbcity.Text.Trim();
                //    bankbulletin.Falwaysworkstate = this.InterfaceLong.SelectedValue;
                //是否影响接口
                if (this.cbForbid.Checked == true)
                    bankbulletin.Isaffectinterface = "1";
                else
                    bankbulletin.Isaffectinterface = "0";
                //业务类型
                string busineType = "";
                if (sysid == "21")
                    busineType = "0";
                else if (sysid == "22")
                    busineType = "1";
                else if (sysid == "23")
                    busineType = "2";
                else if (sysid == "24")
                    busineType = "3";
                bankbulletin.Fbusinetype = busineType;
                //if (this.InterfaceLong.SelectedValue == "1")//校验时间格式
                //{
                //    checkEverydayTime(tbalwtime.Text.Trim());
                //}
                //bankbulletin.Falwtime = tbalwtime.Text.Trim();

                bankbulletin.Ftctext = TextTCMainText.Text.Trim();

                //判断数据库是否存在重复时间段的公告
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                string msg = "";
                if (qs.IsRepeatedTime(bankbulletin.Fid, bankbulletin.Fbanktype, bankbulletin.Fbusinetype, bankbulletin.Fstartime, bankbulletin.Fendtime, out msg))
                {
                    WebUtils.ShowMessage(this.Page, msg);
                    return;
                }

                string[,] param = new string[,] { { "IsNew", Convert.ToString(bankbulletin.IsNew) }, 
            { "IsOPen", Convert.ToString(bankbulletin.IsOPen) },
            { "Fbanktype", Convert.ToString(bankbulletin.Fbanktype) },
            { "Fmaintext", Convert.ToString(bankbulletin.Fmaintext) },
            { "Fstartime", Convert.ToString(bankbulletin.Fstartime) },
            { "Fendtime", Convert.ToString(bankbulletin.Fendtime) },
            { "Fcreateuser", Convert.ToString(bankbulletin.Fcreateuser) },
            { "Fcreatetime", Convert.ToString(bankbulletin.Fcreatetime) },
            { "Fupdateuser", Convert.ToString(bankbulletin.Fupdateuser) },
            { "returnUrl", Convert.ToString(bankbulletin.returnUrl) },
            { "Farea", Convert.ToString(bankbulletin.Farea) },
            { "Fcity", Convert.ToString(bankbulletin.Fcity) },
            { "Fbusinetype", Convert.ToString(bankbulletin.Fbusinetype) },
            //{ "Falwaysworkstate", Convert.ToString(bankbulletin.Falwaysworkstate) },
            { "Isaffectinterface", Convert.ToString(bankbulletin.Isaffectinterface) },
            //{ "Falwtime", Convert.ToString(bankbulletin.Falwtime) },
            { "Ftctext", Convert.ToString(bankbulletin.Ftctext) },
            { "Fid", Convert.ToString(bankbulletin.Fid) }};

                Check_WebService.Param[] pa = ToParamArray(param);


                string memo = "新增银行接口维护信息,银行类型:" + this.ddlQueryBankTypeInterface.SelectedItem.Text.Trim(); ;
                PublicRes.CreateCheckService(this).StartCheck(objid,
                    "BankBulletinForType", memo, "0", pa);
                PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "BankBulletinForType", "新增银行接口维护申请审批", 1, this.ddlQueryBankTypeInterface.SelectedValue.Trim(), "");
                WebUtils.ShowMessage(this.Page, "新增银行接口维护申请已提交，请等待审批！");
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
                string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                string fbanktype = this.ddlQueryBankTypeInterface.SelectedValue.Trim();
                if (this.tbInterfaceMainText.Text.Trim().Length > 30)
                {
                    WebUtils.ShowMessage(this.Page, "正文超过30字符,请重新输入！");
                    return;
                }
                if (this.TextTCMainText.Text.Trim().Length > 60)
                {
                    WebUtils.ShowMessage(this.Page, "弹层正文超过60字符,请重新输入！");
                    return;
                }
                Query_Service.T_BANKBULLETIN_TYPE_ALL bankbulletin = new Query_Service.T_BANKBULLETIN_TYPE_ALL();
                bankbulletin.Fid = this.tbFid.Text.Trim();
                bankbulletin.IsNew = false;
                bankbulletin.IsOPen = this.InterfaceOpen.Checked;
                bankbulletin.Ftitle = "";
                bankbulletin.Fbanktype = fbanktype;
                bankbulletin.Fmaintext = this.tbInterfaceMainText.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
                bankbulletin.Fupdateuser = Session["uid"].ToString();
                try
                {
                    bankbulletin.Fstartime = DateTime.Parse(this.tbInterfaceStartTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                    bankbulletin.Fendtime = DateTime.Parse(this.tbInterfaceEndTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "请输入正确的日期");
                    return;
                }
                bankbulletin.Fcreateuser = this.tbcreateuserInterface.Text.Trim();
                bankbulletin.returnUrl = "http://kf.cf.com/SysManage/SysBulletinManage_Detail.aspx?Fbanktype=" + fbanktype + "&sysid=" + sysid + "&objid=" + objid + "&opertype=0";
                bankbulletin.Farea = this.tbarea.Text.Trim();
                bankbulletin.Fcity = this.tbcity.Text.Trim();
                bankbulletin.Fbusinetype = this.tbbusinetype.Text.Trim();
                //bankbulletin.Falwaysworkstate = this.InterfaceLong.SelectedValue;
                if (this.cbForbid.Checked == true)
                    bankbulletin.Isaffectinterface = "1";
                else
                    bankbulletin.Isaffectinterface = "0";
                //if (this.InterfaceLong.SelectedValue == "1")//校验时间格式
                //{
                //    checkEverydayTime(tbalwtime.Text.Trim());
                //}
                //bankbulletin.Falwtime = tbalwtime.Text.Trim();
                bankbulletin.Ftctext = TextTCMainText.Text.Trim();

                //判断数据库是否存在重复时间段的公告
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                string msg = "";
                if (qs.IsRepeatedTime(bankbulletin.Fid,bankbulletin.Fbanktype, bankbulletin.Fbusinetype, bankbulletin.Fstartime, bankbulletin.Fendtime, out msg))
                {
                    WebUtils.ShowMessage(this.Page, msg);
                    return;
                }

                string[,] param = new string[,] { { "IsNew", Convert.ToString(bankbulletin.IsNew) }, 
            { "IsOPen", Convert.ToString(bankbulletin.IsOPen) },
            { "Fbanktype", Convert.ToString(bankbulletin.Fbanktype) },
            { "Fmaintext", Convert.ToString(bankbulletin.Fmaintext) },
            { "Fstartime", Convert.ToString(bankbulletin.Fstartime) },
            { "Fendtime", Convert.ToString(bankbulletin.Fendtime) },
            { "Fcreateuser", Convert.ToString(bankbulletin.Fcreateuser) },
            { "Fcreatetime", Convert.ToString(bankbulletin.Fcreatetime) },
            { "Fupdateuser", Convert.ToString(bankbulletin.Fupdateuser) },
            { "returnUrl", Convert.ToString(bankbulletin.returnUrl) },
            { "Farea", Convert.ToString(bankbulletin.Farea) },
            { "Fcity", Convert.ToString(bankbulletin.Fcity) },
            { "Fbusinetype", Convert.ToString(bankbulletin.Fbusinetype) },
            //{ "Falwaysworkstate", Convert.ToString(bankbulletin.Falwaysworkstate) },
            { "Isaffectinterface", Convert.ToString(bankbulletin.Isaffectinterface) },
            //{ "Falwtime", Convert.ToString(bankbulletin.Falwtime) },
            { "Ftctext", Convert.ToString(bankbulletin.Ftctext) },
            { "Fid", Convert.ToString(bankbulletin.Fid)}};

                Check_WebService.Param[] pa = ToParamArray(param);

                string memo = "银行接口维护信息,银行类型:" + this.ddlQueryBankTypeInterface.SelectedItem.Text.Trim(); ;
                PublicRes.CreateCheckService(this).StartCheck(objid,
                    "BankBulletinForType", memo, "0", pa);
                PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "BankBulletinForType", "修改银行接口维护申请审批", 1, fbanktype, "");
                WebUtils.ShowMessage(this.Page, "修改银行接口维护申请已提交，请等待审批！");
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

        protected void btInterfaceBack_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("SysBulletinManage.aspx?sysid=" + sysid);
        }

        protected void ddlQueryBankType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.lbbanktype.Text = this.ddlQueryBankType.SelectedValue.Trim();
        }

        protected void ddlQueryBankTypeInterface_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.lbbanktypeInterface.Text = this.ddlQueryBankTypeInterface.SelectedValue.Trim();
        }

        protected void bt_ucback_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("SysBulletinManage.aspx?sysid=" + sysid);
        }

        protected void bt_ucupdate_Click(object sender, System.EventArgs e)
        {
            string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string Fservicecode = ViewState["fid"].ToString();
            string Fuctype = ViewState["uctype"].ToString();

            Query_Service.T_PUCNEWSERVICE_INFO bankbulletin = new Query_Service.T_PUCNEWSERVICE_INFO();
            bankbulletin.IsNew = false;

            bankbulletin.Fservicecode = Fservicecode;
            bankbulletin.Fuctype = Fuctype;

            bankbulletin.Ftips = this.tb_uc_text.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");


            try
            {
                bankbulletin.Fstartime = DateTime.Parse(tb_ucstarttime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                bankbulletin.Fendtime = DateTime.Parse(tb_ucendtime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                WebUtils.ShowMessage(this.Page, "请输入正确的日期");
                return;
            }

            bankbulletin.returnUrl = "/SysManage/SysBulletinManage_Detail.aspx?sysid=7&objid=" + objid + "&opertype=0";

            string[,] param = new string[,] { { "IsNew", Convert.ToString(bankbulletin.IsNew) }, 
            { "Fservicecode", Convert.ToString(bankbulletin.Fservicecode) },
            { "Fuctype", Convert.ToString(bankbulletin.Fuctype) },
            { "Ftips", Convert.ToString(bankbulletin.Ftips) },
            { "Fstartime", Convert.ToString(bankbulletin.Fstartime) },
            { "Fendtime", Convert.ToString(bankbulletin.Fendtime) },
            { "returnUrl", Convert.ToString(bankbulletin.returnUrl) }};

            Check_WebService.Param[] pa = ToParamArray(param);

            string memo = "生活缴费维护公告信息,业务代码:" + Fservicecode;
            PublicRes.CreateCheckService(this).StartCheck(objid,
                "ConfigUCBulletin", memo, "0", pa);
            PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "ConfigUCBulletin", "修改生活缴费维护公告申请审批", 1, Fservicecode, "");
            WebUtils.ShowMessage(this.Page, "修改生活缴费公告申请已提交，请等待审批！");
        }
        private void InterfaceOpen_CheckedChanged(object sender, System.EventArgs e)
		{
            if (InterfaceOpen.Checked)
			{
                tbInterfaceStartTime.Enabled = false;
                iInterfaceStartTime.Visible = false;
                tbInterfaceEndTime.Enabled = false;
                iInterfaceEndTime.Visible = false;
                //tbalwtime.Enabled = false;
			}
			else
			{
                tbInterfaceStartTime.Enabled = true;
                iInterfaceStartTime.Visible = true;
                tbInterfaceEndTime.Enabled = true;
                iInterfaceEndTime.Visible = true;
                //tbalwtime.Enabled= true;
			}
		}

        //弹层正文控制
        private void cbForbid_CheckedChanged(object sender, System.EventArgs e)
        {
            if (cbForbid.Checked)
            {
              //  TextTCMainText.Text = "";//避免连续新增时会提交上条公告的信息
                tcTextId.Visible = false;
            }
            else
                tcTextId.Visible = true;
        }

        private void cbopen_CheckedChanged(object sender, System.EventArgs e)
        {
            if (cbopen.Checked)
            {
                tbstarttime.Enabled = false;
                ibstarttime.Visible = false;
                tbendtime.Enabled = false;
                ibendtime.Visible = false;
            }
            else
            {
                tbstarttime.Enabled = true;
                ibstarttime.Visible = true;
                tbendtime.Enabled = true;
                ibendtime.Visible = true;
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

        private void checkEverydayTime(string alwtime)
        {
            DateTime dt;
            string[] time = alwtime.Split('-');
            if (time.Length != 2)
                throw new Exception("格式有误，请按00:00:00-00:00:00格式输入！");
            bool flag1 = DateTime.TryParse(time[0], out dt);
            bool flag2 = DateTime.TryParse(time[1], out dt);
            if ((!flag1) || (!flag2) || time[0].Length != 8 || time[1].Length != 8)
            {
                throw new Exception("时间输入有误，且请按00:00:00-00:00:00格式输入！");
            }
        }

	}
}