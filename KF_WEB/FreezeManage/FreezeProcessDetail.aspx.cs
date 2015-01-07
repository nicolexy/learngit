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

using System.IO;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using Tencent.DotNet.Common.UI;
using System.Configuration;
using CFT.CSOMS.BLL.FreezeModule;
using CFT.CSOMS.BLL.UserAppealModule;
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Web.FreezeManage
{
	/// <summary>
    /// FreezeProcessDetail 的摘要说明。
	/// </summary>
    public partial class FreezeProcessDetail : System.Web.UI.Page
	{

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

            cbBt_sfz1.Attributes.Add("onclick", "cbxSfzz()");
            cbBt_sfz2.Attributes.Add("onclick", "cbxSfzf()");
            cbBt_yhk1.Attributes.Add("onclick", "cbxYhkzp()");
            cbBt_zjly1.Attributes.Add("onclick", "cbxZjlyjt()");
            cbBt_zjly2.Attributes.Add("onclick", "cbxZjlyjt()");
            cbBt_zjly3.Attributes.Add("onclick", "cbxZjlyjt()");
            cbBt_zjly4.Attributes.Add("onclick", "cbxZjlyjt()");
            cbBt_zjly5.Attributes.Add("onclick", "cbxZjlyjt()");
            cbBt_zjly6.Attributes.Add("onclick", "cbxZjlyjt()");
            cbBt_zjly7.Attributes.Add("onclick", "cbxZjlyjt()");

            cbBt_qtzp1.Attributes.Add("onclick", "cbxBcqtzjzp()");
            cbBt_qtzp2.Attributes.Add("onclick", "cbxBcqtzjzp()");
            cbBt_qtzp3.Attributes.Add("onclick", "cbxBcqtzjzp()");
            cbBt_qtzp_zdy.Attributes.Add("onclick", "cbxBcqtzjzp()");
            cbBt_scbs1.Attributes.Add("onclick", "cbxBcsfzsczp()");
            cbBt_scbs2.Attributes.Add("onclick", "cbxBcsfzsczp()");
            cbBt_scbs_zdy.Attributes.Add("onclick", "cbxBcsfzsczp()");
            cbBt_hjzm1.Attributes.Add("onclick", "cbxBchjzmzp()");
            cbBt_hjzm2.Attributes.Add("onclick", "cbxBchjzmzp()");
            cbBt_hjzm_zdy.Attributes.Add("onclick", "cbxBchjzmzp()");
            cbBt_bczl1.Attributes.Add("onclick", "cbxBcjljtzp()");
            cbBt_bczl2.Attributes.Add("onclick", "cbxBcjljtzp()");
            cbBt_bczl_zdy.Attributes.Add("onclick", "cbxBcjljtzp()");

			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
				ViewState["FID"] = Request.QueryString["fid"].ToString().Trim();

				ViewState["FFreezeID"] = Request.QueryString["ffreeze_id"].ToString().Trim();

                ViewState["FSubmitDate"] = Request.QueryString["fsubmit_date"].ToString().Trim();

				SetAllBtnVisible(false);

				BindData(1);

				string[] fastReplyBuff = getData.GetFreezeFastReplay(this,false);

				if(fastReplyBuff != null)
				{
					this.ddl_fastReply1.Items.Clear();
					foreach(string str in fastReplyBuff)
					{
						if(str != null && str.Trim().Length > 0)
						{
							this.ddl_fastReply1.Items.Add(str);
						}
						else
						{
							
						}
					}
				}
			}
            
			this.ddl_fastReply1.SelectedIndexChanged += new EventHandler(ddl_fastReply1_SelectedIndexChanged);
		}



		private void SetAllBtnVisible(bool canSee)
		{
			this.btn_Del.Visible = canSee;
			this.btn_Finish1.Visible = canSee;
			this.btn_Finish2.Visible = canSee;
			this.btn_hangUp.Visible = canSee;
			
		}


		public void BindData(int iIndex)
		{
            try
            {
                SetAllBtnVisible(false);

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
                qs.Finance_HeaderValue = fh;

                this.tbx_payAccount.Text = ViewState["FFreezeID"].ToString();


                DataSet ds2 = qs.GetCFTUserAppealDetail_New(ViewState["FID"].ToString(), ViewState["FSubmitDate"].ToString());

                if (ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询用户申诉资料结果为空");
                    return;
                }
                else
                {
                    DataRow dr2 = ds2.Tables[0].Rows[0];

                    int ftype = int.Parse(dr2["Ftype"].ToString());

                    string str = dr2["FState"].ToString();

                    ViewState["Fstate"] = dr2["FState"].ToString();
                    ViewState["isFreezeListHas"] = dr2["isFreezeListHas"].ToString();

                    //this.tbx_cerNO.Text = dr2["cre_id"].ToString();    //用户提交证件号码
                    //this.tbx_userSubBindMobile.Text = dr2["contact_no"].ToString();//用户提交绑定手机
                    //this.tbx_lastAddr.Text = dr2["FIp"].ToString();//最后一次使用的地址
                    //this.tbx_bindMobile.Text = dr2["mobile"].ToString();//绑定手机
                    this.tbx_email.Text = dr2["FEmail"].ToString();//联系邮箱
                    //this.tbx_phoneNo.Text = dr2["contact_no"].ToString();//联系电话
                    //this.tbx_DC.Text = dr2["DC_timeaddress"].ToString();//近期安装数字证书
                    //this.tbx_userQA.Text = dr2["reason"].ToString();

                    this.tbx_cerNO.Text = dr2["FcreId"].ToString();    //用户提交证件号码
                    ViewState["TjCreId"] = dr2["FcreId"].ToString();
                    ViewState["TjCreType"] = dr2["FCreType"].ToString(); //证件类型
                    this.tbx_phoneNo.Text = dr2["FReservedMobile"].ToString();//联系电话
                    this.tbx_freezeReason.Text = dr2["FreezeReason"].ToString();//冻结原因

                    //以下为添加的展示字段
                    this.tbx_subUserName.Text = PublicRes.GetString(dr2["FOldName"].ToString());//用户提交姓名
                    this.lblstandard_score.Text = dr2["FStandardScore"].ToString();//免审核标准分
                    this.lblrisk_result.Text = dr2["risk_result"].ToString();//风控标记
                    if (dr2["FClearPps"].ToString() == "1" && dr2["FType"].ToString() == "1")//是否清空密保资料
                    {
                        this.clear_pps.Text = "清除";
                    }
                    else if (dr2["FType"].ToString() == "1" && dr2["FClearPps"].ToString() != "1")
                    {
                        this.clear_pps.Text = "不清除";
                    }
                    else
                    {
                        this.clear_pps.Text = "";
                    }
                    this.lblscore.Text = dr2["FAppealScore"].ToString();//实际得分
                    bool isAuthen = new UserAppealService().GetUserAuthenState(dr2["Fuin"].ToString(), "", 0);//实名认证
                    if (isAuthen)
                        this.lbauthenState.Text = "是";
                    else
                        this.lbauthenState.Text = "否";
                    this.lbldetail_score.Text = dr2["detail_score"].ToString();//得分明细


                    this.tbx_userQA.Text = dr2["FAppealReason"].ToString();  //用户描述

                    string img_cgi = ConfigurationManager.AppSettings["GetAppealImageCgi"].ToString();

                    //int ftype = int.Parse(dr2["Ftype"].ToString());

                    #region 查询账户基本信息
                    DataSet dsuser = qs.GetAppealUserInfo(dr2["Fuin"].ToString());
                    if (dsuser == null || dsuser.Tables.Count == 0 || dsuser.Tables[0].Rows.Count == 0)
                    {
                        //return;
                    }
                    else
                    {
                        DataRow dr3 = dsuser.Tables[0].Rows[0];

                        this.tbx_userName.Text = PublicRes.GetString(dr3["Ftruename"]);
                        this.tbx_regCreNO.Text = PublicRes.GetString(dr3["Fcreid"]);
                        ViewState["OldCreId"] = PublicRes.GetString(dr3["Fcreid"]);
                        this.tbx_restFin.Text = classLibrary.setConfig.FenToYuan((double.Parse(dr3["FBalance"].ToString()) - double.Parse(dr3["Fcon"].ToString())).ToString());
                    }
                    #endregion

                    #region 图片
                    if (!(dr2["FCreImg1"] is DBNull))
                    {
                        if (dr2["FCreImg1"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                this.Image1.ImageUrl = dr2["FCreImg1"].ToString();  //身份证正面
                            }
                            else
                            {
                                this.Image1.ImageUrl = img_cgi + dr2["FCreImg1"].ToString();  //身份证正面
                            }
                        }
                    }
                    if (!(dr2["FCreImg2"] is DBNull))
                    {
                        if (dr2["FCreImg2"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                this.Image2.ImageUrl = dr2["FCreImg2"].ToString();  //身份证反面
                            }
                            else
                            {
                                this.Image2.ImageUrl = img_cgi + dr2["FCreImg2"].ToString();  //身份证反面
                            }
                        }
                    }
                    if (!(dr2["FOtherImage1"] is DBNull))
                    {
                        if (dr2["FOtherImage1"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                this.Image3.ImageUrl = dr2["FOtherImage1"].ToString();  //银行卡照片
                            }
                            else
                            {
                                this.Image3.ImageUrl = img_cgi + dr2["FOtherImage1"].ToString();  //银行卡照片
                            }
                        }
                    }
                    if (!(dr2["FProveBanlanceImage"] is DBNull))
                    {
                        if (dr2["FProveBanlanceImage"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                this.Image4.ImageUrl = dr2["FProveBanlanceImage"].ToString();  //资金来源截图
                            }
                            else
                            {
                                this.Image4.ImageUrl = img_cgi + dr2["FProveBanlanceImage"].ToString();  //资金来源截图
                            }
                        }
                    }
                    #endregion

                    #region//其它图片
                    if (!(dr2["FOtherImage2"] is DBNull))
                    {
                        if (dr2["FOtherImage2"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                this.img_qtzp1.ImageUrl = dr2["FOtherImage2"].ToString();  //补充其他证件照片
                            }
                            else
                            {
                                this.img_qtzp1.ImageUrl = img_cgi + dr2["FOtherImage2"].ToString();  //补充其他证件照片
                            }
                        }
                    }
                    if (!(dr2["FOtherImage3"] is DBNull))
                    {
                        if (dr2["FOtherImage3"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                this.img_scbs1.ImageUrl = dr2["FOtherImage3"].ToString();  //补充的手持身份证半身照
                            }
                            else
                            {
                                this.img_scbs1.ImageUrl = img_cgi + dr2["FOtherImage3"].ToString();  //补充的手持身份证半身照
                            }
                        }
                    }
                    if (!(dr2["FOtherImage4"] is DBNull))
                    {
                        if (dr2["FOtherImage4"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                this.img_hjzm1.ImageUrl = dr2["FOtherImage4"].ToString();  //补充户籍证明照片
                            }
                            else
                            {
                                this.img_hjzm1.ImageUrl = img_cgi + dr2["FOtherImage4"].ToString();  //补充户籍证明照片
                            }
                        }
                    }
                    if (!(dr2["FOtherImage5"] is DBNull))
                    {
                        if (dr2["FOtherImage5"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                this.img_zljt1.ImageUrl = dr2["FOtherImage5"].ToString();  //补充资料截图
                            }
                            else
                            {
                                this.img_zljt1.ImageUrl = img_cgi + dr2["FOtherImage5"].ToString();  //补充资料截图
                            }
                        }
                    }
                    #endregion

                    #region  自定义标题、内容
                    //自定义标题1
                    if (!(dr2["Fsup_desc1"] is DBNull))
                    {
                        tbx_bcqtzjzp_zdy.Text = dr2["Fsup_desc1"].ToString();
                    }
                    //自定义标题2
                    if (!(dr2["Fsup_desc2"] is DBNull))
                    {
                        tbx_bcsfzsczp_zdy.Text = dr2["Fsup_desc2"].ToString();
                    }
                    //自定义标题3
                    if (!(dr2["Fsup_desc3"] is DBNull))
                    {
                        tbx_bchjzmzp_zdy.Text = dr2["Fsup_desc3"].ToString();
                    }
                    //自定义标题4
                    if (!(dr2["Fsup_desc4"] is DBNull))
                    {
                        tbx_bcjljtzp_zdy.Text = dr2["Fsup_desc4"].ToString();
                    }

                    //自定义内容1
                    if (!(dr2["Fsup_tips1"] is DBNull))
                    {
                        tbx_qtzp_zdy.Text = dr2["Fsup_tips1"].ToString();
                    }
                    //自定义内容2
                    if (!(dr2["Fsup_tips2"] is DBNull))
                    {
                        tbx_scbs_zdy.Text = dr2["Fsup_tips2"].ToString();
                    }
                    //自定义内容3
                    if (!(dr2["Fsup_tips3"] is DBNull))
                    {
                        tbx_hjzm_zdy.Text = dr2["Fsup_tips3"].ToString();
                    }
                    //自定义内容4
                    if (!(dr2["Fsup_tips4"] is DBNull))
                    {
                        tbx_bczl_zdy.Text = dr2["Fsup_tips4"].ToString();
                    }
                    #endregion
                }


                if (ViewState["isFreezeListHas"].ToString() == "1")
                {//冻结单中存在冻结记录
                    if (ViewState["Fstate"].ToString() == "1")
                    {
                        // 已结单
                        this.btn_Del.Visible = true;
                    }
                    else if (ViewState["Fstate"].ToString() == "7")
                    {
                        // 已作废
                    }
                    else
                    {
                        // 挂起或者未处理
                        SetAllBtnVisible(true);
                    }
                }
                else
                {
                    // 如果该用户不处于冻结状态，则只允许对这个申诉进行作废处理
                    if (ViewState["Fstate"].ToString() != "7")
                    {
                        this.btn_Del.Visible = true;
                    }

                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "查询异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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
            //this.cbBt_sfz1.CheckedChanged
		}
		#endregion

		//挂起
        protected void btn_hangUp_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);
            string userDesc = classLibrary.setConfig.replaceHtmlStr(this.tbx_userQA.Text);

			try
			{
                if (qs.CreateFreezeDiary_NEW(ViewState["FID"].ToString(), 8, Session["uid"].ToString()
                    , handleResult, "", ViewState["FFreezeID"].ToString(), this.tbx_phoneNo.Text.Trim(), ViewState["FSubmitDate"].ToString(), 0, userDesc,"","","","","","","",""))
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

        //结单解冻
		protected void btn_Finish1_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);
            string userDesc = classLibrary.setConfig.replaceHtmlStr(this.tbx_userQA.Text);

			try
			{
				// 结单的话，将FSourceType设置为结单状态 ,需要发送tips和手机短信
                if (qs.CreateFreezeDiary_NEW(ViewState["FID"].ToString(), 1, Session["uid"].ToString()
                    , handleResult, "", ViewState["FFreezeID"].ToString(), this.tbx_phoneNo.Text.Trim(), ViewState["FSubmitDate"].ToString(), 0, userDesc, "", "", "", "", "", "", "", ""))
				{
					//Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
                    Response.Write("<script language=javascript>window.location='../BaseAccount/freezeBankAcc.aspx?id=false&uid=" + ViewState["FFreezeID"].ToString() + "&iswechat=false&type=per'</script>");
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

        //补充资料
		protected void btn_Finish2_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

            var ret = verifyCheckBox();
            if (ret != "0") 
            {
                return;
            }
            
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);

			try
			{
				//如果客服回复不为空，则用户描述必勾选
                this.cbBt_yhms1.Checked = true;
                #region 掩码勾选
                //如果是补填，需要传补填些什么资料 资金来源等
                Int32 bt = 0;
                if (this.cbBt_sfzz.Checked) 
                {
                    //身份证正面
                    bt = bt | 0x00000001;
                }
                if (this.cbBt_sfz1.Checked)
                {
                    //注册财付通身份证正面
                    bt = bt | 0x00000002;
                }
                if (this.cbBt_sfzf.Checked)
                {
                    //身份证反面
                    bt = bt | 0x00000004;
                }
                if (this.cbBt_sfz2.Checked)
                {
                    //注册财付通身份证反面
                    bt = bt | 0x00000008;
                }
                if (this.cbBt_yhkzp.Checked)
                {
                    //银行卡
                    bt = bt | 0x00000800;
                }
                if (this.cbBt_yhk1.Checked)
                {
                    //银行卡照片或扫描
                    bt = bt | 0x00001000;
                }
                //资金截图
                if (this.cbBt_zjlyjt.Checked)
                {
                    bt = bt | 0x00010000;
                }
                if (this.cbBt_zjly1.Checked)
                {
                    bt = bt | 0x00020000;
                }
                if (this.cbBt_zjly2.Checked)
                {
                    bt = bt | 0x00040000;
                }
                if (this.cbBt_zjly3.Checked)
                {
                    bt = bt | 0x00080000;
                }
                if (this.cbBt_zjly4.Checked)
                {
                    bt = bt | 0x00100000;
                }
                if (this.cbBt_zjly5.Checked)
                {
                    bt = bt | 0x00200000;
                }
                if (this.cbBt_zjly6.Checked)
                {
                    bt = bt | 0x00400000;
                }
                if (this.cbBt_zjly7.Checked)
                {
                    bt = bt | 0x00800000;
                }

                /*
                //增加自定义内容后取消 yinhuang 14/8/8
                //补充其他证件照
                if (this.cbBt_bcqtzjzp.Checked)
                {
                    bt = bt | 0x00000010;
                }
                if (this.cbBt_qtzp1.Checked)
                {
                    bt = bt | 0x00000020;
                }
                if (this.cbBt_qtzp2.Checked)
                {
                    bt = bt | 0x00000040;
                }
                if (this.cbBt_qtzp3.Checked)
                {
                    bt = bt | 0x00000080;
                }
                //补充的手持身份证半身照
                if (this.cbBt_bcsfzsczp.Checked)
                {
                    bt = bt | 0x00000100;
                }
                if (this.cbBt_scbs1.Checked)
                {
                    bt = bt | 0x00000200;
                }
                if (this.cbBt_scbs2.Checked)
                {
                    bt = bt | 0x00000400;
                }
                //补充户籍证明照片
                if (this.cbBt_bchjzmzp.Checked)
                {
                    bt = bt | 0x00002000;
                }
                if (this.cbBt_hjzm1.Checked)
                {
                    bt = bt | 0x00004000;
                }
                if (this.cbBt_hjzm2.Checked)
                {
                    bt = bt | 0x00008000;
                }
                //补充资料截图
                if (this.cbBt_bcjljtzp.Checked)
                {
                    bt = bt | 0x01000000;
                }
                if (this.cbBt_bczl1.Checked)
                {
                    bt = bt | 0x02000000;
                }
                if (this.cbBt_bczl2.Checked)
                {
                    bt = bt | 0x04000000;
                }

                */
                //用户描述
                if (this.cbBt_yhms1.Checked)
                {
                    bt = bt | 0x08000000;
                }

                #endregion

                if (bt == 0) 
                {
                    WebUtils.ShowMessage(this, "请勾选补填信息！");
                    return;
                }

                //自定义标题1
                var zdyBt1 = "";
                if (cbBt_bcqtzjzp.Checked) 
                {
                    zdyBt1 = tbx_bcqtzjzp_zdy.Text;
                }
                //自定义标题2
                var zdyBt2 = "";
                if (cbBt_bcsfzsczp.Checked)
                {
                    zdyBt2 = tbx_bcsfzsczp_zdy.Text;
                }
                //自定义标题3
                var zdyBt3 = "";
                if (cbBt_bchjzmzp.Checked)
                {
                    zdyBt3 = tbx_bchjzmzp_zdy.Text;
                }
                //自定义标题4
                var zdyBt4 = "";
                if (cbBt_bcjljtzp.Checked)
                {
                    zdyBt4 = tbx_bcjljtzp_zdy.Text;
                }
                //自定义内容1
                var zdyCont1 = "";
                if (cbBt_qtzp1.Checked)
                {
                    zdyCont1 = cbBt_qtzp1.Text;
                }
                else if (cbBt_qtzp2.Checked)
                {
                    zdyCont1 = cbBt_qtzp2.Text;
                }
                else if (cbBt_qtzp3.Checked)
                {
                    zdyCont1 = cbBt_qtzp3.Text;
                }
                else if (cbBt_qtzp_zdy.Checked)
                {
                    zdyCont1 = tbx_qtzp_zdy.Text;
                }
                //自定义内容2
                var zdyCont2 = "";
                if (cbBt_scbs1.Checked)
                {
                    zdyCont2 = cbBt_scbs1.Text;
                }
                else if (cbBt_scbs2.Checked)
                {
                    zdyCont2 = cbBt_scbs2.Text;
                }
                else if (cbBt_scbs_zdy.Checked)
                {
                    zdyCont2 = tbx_scbs_zdy.Text;
                }
                //自定义内容3
                var zdyCont3 = "";
                if (cbBt_hjzm1.Checked)
                {
                    zdyCont3 = cbBt_hjzm1.Text;
                }
                else if (cbBt_hjzm2.Checked)
                {
                    zdyCont3 = cbBt_hjzm2.Text;
                }
                if (cbBt_hjzm_zdy.Checked)
                {
                    zdyCont3 = tbx_hjzm_zdy.Text;
                }
                //自定义内容4
                var zdyCont4 = "";
                if (cbBt_bczl1.Checked)
                {
                    zdyCont4 = cbBt_bczl1.Text;
                }
                else if (cbBt_bczl2.Checked)
                {
                    zdyCont4 = cbBt_bczl2.Text;
                }
                if (cbBt_bczl_zdy.Checked)
                {
                    zdyCont4 = tbx_bczl_zdy.Text;
                }

                string userDesc = classLibrary.setConfig.replaceHtmlStr(this.tbx_userQA.Text);

                // 结单的话，将FSourceType设置为结单状态
                if (qs.CreateFreezeDiary_NEW(ViewState["FID"].ToString(), 2, Session["uid"].ToString(), handleResult, "", ViewState["FFreezeID"].ToString(), this.tbx_phoneNo.Text.Trim(), ViewState["FSubmitDate"].ToString(), bt, userDesc
                    , zdyBt1, zdyBt2, zdyBt3, zdyBt4, zdyCont1, zdyCont2, zdyCont3, zdyCont4))
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

        //作废
		protected void btn_Del_Click(object sender, System.EventArgs e)
		{
            if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string userDesc = classLibrary.setConfig.replaceHtmlStr(this.tbx_userQA.Text);
			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);

			try
			{
				// 作废的话，将FSourceType设置为作废状态
                if (qs.CreateFreezeDiary_NEW(ViewState["FID"].ToString(), 7, Session["uid"].ToString(), handleResult, "", ViewState["FFreezeID"].ToString(), this.tbx_phoneNo.Text.Trim(), ViewState["FSubmitDate"].ToString(), 0, userDesc, "", "", "", "", "", "", "", ""))
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

        //补充处理结果
		protected void btn_addRecord_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_handleResult.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入补充的处理结果");
				return;
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string handleResult = classLibrary.setConfig.replaceHtmlStr(this.tbx_handleResult.Text);
            string userDesc = classLibrary.setConfig.replaceHtmlStr(this.tbx_userQA.Text);

			try
			{
                if (qs.CreateFreezeDiary_NEW(ViewState["FID"].ToString(), 100, Session["uid"].ToString()
                    , handleResult, "", ViewState["FFreezeID"].ToString(), this.tbx_phoneNo.Text.Trim(), ViewState["FSubmitDate"].ToString(), 0, userDesc, "", "", "", "", "", "", "", ""))
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=s",false);
				}
				else
				{
					Response.Redirect("FreezeDiary.aspx?FFreezeListID=" + ViewState["FID"].ToString() + "&state=f",false);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

        //同步身份证号
        protected void btn_synCreid_Click(object sender, System.EventArgs e) 
        {
            try 
            {
                bool ret = new FreezeService().SyncCreid(ViewState["FFreezeID"].ToString(), ViewState["OldCreId"].ToString(), ViewState["TjCreId"].ToString(), int.Parse(ViewState["TjCreType"].ToString()), Session["uid"].ToString(), Request.UserHostAddress);
                if (ret)
                {
                    WebUtils.ShowMessage(this, "同步身份证号成功！");
                }
                else 
                {
                    WebUtils.ShowMessage(this, "同步身份证号失败！");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, ex.Message);
                return;
            }
        }

		private void ddl_fastReply1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string strFastReply = this.ddl_fastReply1.SelectedValue;

			this.tbx_handleResult.Text += (strFastReply);
		}

        private string verifyCheckBox() 
        {
            string ret = "0";
            //判断checkbox勾选而子选项未勾选
            //身份证正面
            if (cbBt_sfzz.Checked && !cbBt_sfz1.Checked)
            {
                WebUtils.ShowMessage(this, "请勾选身份证正面照片子项！");
                return "1";
            }
            if (!cbBt_sfzz.Checked && cbBt_sfz1.Checked)
            {
                WebUtils.ShowMessage(this, "请勾选身份证正面照片！");
                return "2";
            }
            //身份证反面
            if (cbBt_sfzf.Checked && !cbBt_sfz2.Checked)
            {
                WebUtils.ShowMessage(this, "请勾选身份证反面照片子项！");
                return "3";
            }
            if (!cbBt_sfzf.Checked && cbBt_sfz2.Checked)
            {
                WebUtils.ShowMessage(this, "请勾选身份证反面照片！");
                return "4";
            }
            //银行卡照片
            if (cbBt_yhkzp.Checked && !cbBt_yhk1.Checked)
            {
                WebUtils.ShowMessage(this, "请勾选银行卡照片子项！");
                return "5";
            }
            if (!cbBt_yhkzp.Checked && cbBt_yhk1.Checked)
            {
                WebUtils.ShowMessage(this, "请勾选银行卡照片！");
                return "6";
            }
            //资金来源截图
            if (cbBt_zjlyjt.Checked)
            {
                int i = 0;
                if (cbBt_zjly1.Checked)
                {
                    i++;
                }
                if (cbBt_zjly2.Checked)
                {
                    i++;
                }
                if (cbBt_zjly3.Checked)
                {
                    i++;
                }
                if (cbBt_zjly4.Checked)
                {
                    i++;
                }
                if (cbBt_zjly5.Checked)
                {
                    i++;
                }
                if (cbBt_zjly6.Checked)
                {
                    i++;
                }
                if (cbBt_zjly7.Checked)
                {
                    i++;
                }
                if (i == 0)
                {
                    WebUtils.ShowMessage(this, "请勾选资金来源截图子项！");
                    return "7";
                }
            }
            else 
            {
                int i = 0;
                if (cbBt_zjly1.Checked)
                {
                    i++;
                }
                if (cbBt_zjly2.Checked)
                {
                    i++;
                }
                if (cbBt_zjly3.Checked)
                {
                    i++;
                }
                if (cbBt_zjly4.Checked)
                {
                    i++;
                }
                if (cbBt_zjly5.Checked)
                {
                    i++;
                }
                if (cbBt_zjly6.Checked)
                {
                    i++;
                }
                if (cbBt_zjly7.Checked)
                {
                    i++;
                }
                if (i > 0)
                {
                    WebUtils.ShowMessage(this, "请勾选资金来源截图！");
                    return "8";
                }
            }
            //补充其他照片
            if (cbBt_bcqtzjzp.Checked)
            {
                var zdy = tbx_bcqtzjzp_zdy.Text;
                if (zdy.Trim() == "") 
                {
                    WebUtils.ShowMessage(this, "请输入自定义1内容！");
                    return "9";
                }

                int i = 0;
                if (cbBt_qtzp1.Checked)
                {
                    i++;
                }
                if (cbBt_qtzp2.Checked)
                {
                    i++;
                }
                if (cbBt_qtzp3.Checked)
                {
                    i++;
                }
                if (cbBt_qtzp_zdy.Checked)
                {
                    i++;
                }
                if (i == 0)
                {
                    WebUtils.ShowMessage(this, "请勾选"+zdy+"子项！");
                    return "9";
                }
            }
            else
            {
                int i = 0;
                if (cbBt_qtzp1.Checked)
                {
                    i++;
                }
                if (cbBt_qtzp2.Checked)
                {
                    i++;
                }
                if (cbBt_qtzp3.Checked)
                {
                    i++;
                }
                if (cbBt_qtzp_zdy.Checked)
                {
                    i++;
                }
                if (i > 0)
                {
                    WebUtils.ShowMessage(this, "请勾选自定义1！");
                    return "10";
                }
            }
            if (cbBt_qtzp_zdy.Checked && tbx_qtzp_zdy.Text == "") 
            {
                WebUtils.ShowMessage(this, "请输入自定义1子项内容！");
                return "10";
            }

            //补充手持身份证半身照
            if (cbBt_bcsfzsczp.Checked)
            {
                var zdy = tbx_bcsfzsczp_zdy.Text;
                if (zdy.Trim() == "")
                {
                    WebUtils.ShowMessage(this, "请输入自定义2内容！");
                    return "11";
                }
                int i = 0;
                if (cbBt_scbs1.Checked)
                {
                    i++;
                }
                if (cbBt_scbs2.Checked)
                {
                    i++;
                }
                if (cbBt_scbs_zdy.Checked)
                {
                    i++;
                }
                if (i == 0)
                {
                    WebUtils.ShowMessage(this, "请勾选"+zdy+"子项！");
                    return "11";
                }
            }
            else
            {
                int i = 0;
                if (cbBt_scbs1.Checked)
                {
                    i++;
                }
                if (cbBt_scbs2.Checked)
                {
                    i++;
                }
                if (cbBt_scbs_zdy.Checked)
                {
                    i++;
                }
                if (i > 0)
                {
                    WebUtils.ShowMessage(this, "请勾选自定义2！");
                    return "12";
                }
            }
            if (cbBt_scbs_zdy.Checked && tbx_scbs_zdy.Text == "")
            {
                WebUtils.ShowMessage(this, "请输入自定义2子项内容！");
                return "12";
            }

            //补充户籍证明照片
            if (cbBt_bchjzmzp.Checked)
            {
                var zdy = tbx_bchjzmzp_zdy.Text;
                if (zdy.Trim() == "")
                {
                    WebUtils.ShowMessage(this, "请输入自定义3内容！");
                    return "13";
                }
                int i = 0;
                if (cbBt_hjzm1.Checked)
                {
                    i++;
                }
                if (cbBt_hjzm2.Checked)
                {
                    i++;
                }
                if (cbBt_hjzm_zdy.Checked)
                {
                    i++;
                }
                if (i == 0)
                {
                    WebUtils.ShowMessage(this, "请勾选"+zdy+"子项！");
                    return "13";
                }
            }
            else
            {
                int i = 0;
                if (cbBt_hjzm1.Checked)
                {
                    i++;
                }
                if (cbBt_hjzm2.Checked)
                {
                    i++;
                }
                if (cbBt_hjzm_zdy.Checked)
                {
                    i++;
                }
                if (i > 0)
                {
                    WebUtils.ShowMessage(this, "请勾选自定义3！");
                    return "14";
                }
            }
            if (cbBt_hjzm_zdy.Checked && tbx_hjzm_zdy.Text == "")
            {
                WebUtils.ShowMessage(this, "请输入自定义3子项内容！");
                return "14";
            }

            //补充资料截图
            if (cbBt_bcjljtzp.Checked)
            {
                var zdy = tbx_bcjljtzp_zdy.Text;
                if (zdy.Trim() == "")
                {
                    WebUtils.ShowMessage(this, "请输入自定义4内容！");
                    return "15";
                }
                int i = 0;
                if (cbBt_bczl1.Checked)
                {
                    i++;
                }
                if (cbBt_bczl2.Checked)
                {
                    i++;
                }
                if (cbBt_bczl_zdy.Checked)
                {
                    i++;
                }
                if (i == 0)
                {
                    WebUtils.ShowMessage(this, "请勾选"+zdy+"子项！");
                    return "15";
                }
            }
            else
            {
                int i = 0;
                if (cbBt_bczl1.Checked)
                {
                    i++;
                }
                if (cbBt_bczl2.Checked)
                {
                    i++;
                }
                if (cbBt_bczl_zdy.Checked)
                {
                    i++;
                }
                if (i > 0)
                {
                    WebUtils.ShowMessage(this, "请勾选自定义4！");
                    return "16";
                }
            }
            if (cbBt_bczl_zdy.Checked && tbx_bczl_zdy.Text == "")
            {
                WebUtils.ShowMessage(this, "请输入自定义4子项内容！");
                return "16";
            }

            return ret;
        }

		protected void btn_manageFastReply_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("./FastReplyManagePage.aspx",false);
		}
	}
}
