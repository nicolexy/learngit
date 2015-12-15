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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// BankrollHistoryLog 的摘要说明。
	/// </summary>
	public partial class BankrollHistoryLog : System.Web.UI.Page
	{
		protected string iFramePath;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{

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

		}
		#endregion

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			try
			{
                if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
                {
                    WebUtils.ShowMessage(this.Page, "请输入账号!");
                    return;
                }
				DateTime begindate;
				DateTime enddate;
				try
				{
					begindate = DateTime.Parse(TextBoxBeginDate.Text);
					enddate = DateTime.Parse(TextBoxEndDate.Text);
				}
				catch
				{
					throw new Exception("日期输入有误！");
				}

				if(begindate.CompareTo(enddate) > 0)
				{
					throw new Exception("终止日期小于起始日期，请重新输入！");
				}

				if(begindate.AddMonths(1) < enddate)
				{
					throw new Exception("查询时间不能大于一个月，请重新输入！");
				}

                Session["QQID"] = getQQID();
                if ((this.InternalID.Checked) && (Session["QQID"] == null))//echo,如果查询条件为内部id，且对应QQ为空（此时有可能是注销的账户）
                {
                    BindDataCancel(1, 1);   //查询注销账户信息
                }
                else
                {
                    if (!(Session["QQID"] == null))
                    {
                        BindData(1, 1);           //绑定数据
                    }
                }

				iFramePath = "bankrollLog.aspx?type=QQID&BeginDate=" + begindate.ToString("yyyy-MM-dd 00:00:00") + "&EndDate=" + enddate.ToString("yyyy-MM-dd 23:59:59");
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"查询错误："+ ex.Message);
			}
		}
        
        protected string getQQID()
        {
            Session["fuid"] = "";
            if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
            {
                throw new Exception("请输入要查询的账号");
            }
            var id = this.TextBox1_InputQQ.Text.Trim();
            if (this.CFT.Checked)
            {
                return id;
            }
            else if (this.InternalID.Checked)
            {
                Session["fuid"] = id;  //注销帐号查询资金流水
                var qs = new Query_Service.Query_Service();
                return qs.Uid2QQ(id);
            }
            return id;
        }

        //注销账号信息查询
        private void BindDataCancel(int istr, int imax)
        {
            Query_Service.Query_Service myService = new Query_Service.Query_Service();
            myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = new DataSet();
            ds = new AccountService().GetUserAccountCancel(this.TextBox1_InputQQ.Text.Trim(), 1, istr, imax);

            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                throw new Exception("数据库无此记录");
            }
            else
            {
                Session["QQID"] = ds.Tables[0].Rows[0]["Fqqid"].ToString();
                string fuid = ds.Tables[0].Rows[0]["fuid"].ToString().Trim();

                this.Label1_Acc.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
                #region this.labQQstate
                if (Label1_Acc.Text != "")
                {
                    string testuid = new AccountService().QQ2Uid(Label1_Acc.Text);
                    if (testuid != null)
                    {
                        labQQstate.Text = "注册未关联";
                        if (testuid.Trim() == fuid)
                        {
                            //再判断是否是已激活 furion 20070226
                            string trueuid = new AccountService().QQ2UidX(Label1_Acc.Text);
                            if (trueuid != null)
                                labQQstate.Text = "已关联";
                            else
                                labQQstate.Text = "已关联未激活";
                        }
                    }
                    else
                        labQQstate.Text = "未注册";
                }
                else
                    labQQstate.Text = "";
                #endregion
                #region this.Label14_Ftruename
                // 2012/5/2 因为需要Q_USER_INFO获取准确的用户真实姓名而改动
                try
                {
                    this.Label14_Ftruename.Text = ds.Tables[0].Rows[0]["UserRealName2"].ToString();
                }
                catch
                {
                    this.Label14_Ftruename.Text = ds.Tables[0].Rows[0]["Ftruename"].ToString();
                }
                #endregion

                //furion 20061116 email登录修改
                this.labEmail.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Femail"]);
                #region this.labEmailState 
                if (labEmail.Text != "")
                {
                    string testuid = new AccountService().QQ2Uid(labEmail.Text);
                    if (testuid != null)
                    {
                        labEmailState.Text = "注册未关联";
                        if (testuid.Trim() == fuid)
                        {
                            //再判断是否是已激活 furion 20070226
                            string trueuid = new AccountService().QQ2UidX(labEmail.Text);
                            if (trueuid != null)
                                labEmailState.Text = "已关联";
                            else
                                labEmailState.Text = "已关联未激活";
                        }
                    }
                    else
                        labEmailState.Text = "未注册";
                }
                else
                    labEmailState.Text = "";
                #endregion
                this.lbInnerID.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();

                this.labMobile.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fmobile"]);
                #region this.labMobileState
                if (labMobile.Text != "")
                {
                    string testuid = new AccountService().QQ2Uid(labMobile.Text);
                    if (testuid != null)
                    {
                        labMobileState.Text = "注册未关联";
                        if (testuid.Trim() == fuid)
                        {
                            //再判断是否是已激活 furion 20070226
                            string trueuid = new AccountService().QQ2UidX(labMobile.Text);
                            if (trueuid != null)
                                labMobileState.Text = "已关联";
                            else
                                labMobileState.Text = "已关联未激活";
                        }
                    }
                    else
                        labMobileState.Text = "未注册";
                }
                else
                    labMobileState.Text = "";
                #endregion
                this.lbLeftPay.Text = Transfer.convertBPAY(ds.Tables[0].Rows[0]["Fbpay_state"].ToString().Trim());

                this.Label12_Fstate.Text = Transfer.accountState(ds.Tables[0].Rows[0]["Fstate"].ToString());
                this.Label13_Fuser_type.Text = Transfer.convertFuser_type(ds.Tables[0].Rows[0]["Fuser_type"].ToString());

                this.Label15_Useable.Text = classLibrary.setConfig.FenToYuan((long.Parse(ds.Tables[0].Rows[0]["Fbalance"].ToString()) - long.Parse(ds.Tables[0].Rows[0]["Fcon"].ToString())).ToString());  //帐户余额减去冻结余额= 可用余额
                #region this.Label4_Freeze
                string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //分账冻结金额
                string s_cron = PublicRes.objectToString(ds.Tables[0], "Fcon");
                long l_fzamt = 0, l_cron = 0;
                if (s_fz_amt != "")
                {
                    l_fzamt = long.Parse(s_fz_amt);
                }
                if (s_cron != "")
                {
                    l_cron = long.Parse(s_cron);
                }
                this.Label4_Freeze.Text = classLibrary.setConfig.FenToYuan(l_fzamt + l_cron);//冻结金额=分账冻结金额+冻结金额
                #endregion

                this.Label5_YestodayLeft.Text = ds.Tables[0].Rows[0]["Fyday_balance"].ToString();		   //"10";
                this.Label3_LeftAcc.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fbalance"].ToString());//tu.u_Balance;				   //"3000";

                this.Label2_Type.Text = Transfer.convertMoney_type(ds.Tables[0].Rows[0]["Fcurtype"].ToString());//tu.u_CurType;				   //"代金券";
                this.lblLoginTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();

                this.Label16_Fapay.Text = ds.Tables[0].Rows[0]["Fapay"].ToString();
                this.Label7_SingleMax.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fquota"].ToString());		   //"2000";

                this.Label8_PerDayLmt.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fquota_pay"].ToString());			//"5000";
                this.lbFetchMoney.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Ffetch"].ToString().Trim());

                this.lbSave.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fsave"].ToString().Trim());
                this.Label9_LastSaveDate.Text = ds.Tables[0].Rows[0]["Fsave_time"].ToString();				//"2005-03-01";

                this.Label10_Drawing.Text = ds.Tables[0].Rows[0]["Ffetch_time"].ToString();              //"2005-04-15";
                this.Label17_Flogin_ip.Text = ds.Tables[0].Rows[0]["Flogin_ip"].ToString();

                this.Label6_LastModify.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();		   //"2005-05-01";
                #region this.Label18_Attid
                //2006-10-18 edwinyang 增加产品属性
                int nAttid = int.Parse(ds.Tables[0].Rows[0]["Att_id"].ToString());
                this.Label18_Attid.Text = CheckBasicInfo(nAttid);
                #endregion

                this.Label11_Remark.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();					//"这个家伙很懒，什么都没有留下！";                
            }

            if (Session["QQID"] == null && Session["QQID"] == "")
            {
                return;
            }
        }

		private void BindData(int istr,int imax)
		{
			//Query_Service.Query_Service myService = new Query_Service.Query_Service();
			//myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			DataSet ds = new AccountService().GetUserAccount(Session["QQID"].ToString(),1,istr,imax);	

			if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1) 
			{
				throw new Exception("数据库无此记录");					
			}	

			this.Label1_Acc.Text			= PublicRes.objectToString(ds.Tables[0], "Fqqid");
            this.Label2_Type.Text           = Transfer.convertMoney_type(PublicRes.objectToString(ds.Tables[0], "Fcurtype"));//tu.u_CurType;				   //"代金券";
			this.Label3_LeftAcc.Text		= classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fbalance"));//tu.u_Balance;				   //"3000";
			this.Label4_Freeze.Text			= classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fcon"));                  //"1000";
            this.Label5_YestodayLeft.Text = PublicRes.objectToString(ds.Tables[0], "Fyday_balance"); 		   //"10";
			this.lblLoginTime.Text          = PublicRes.objectToString(ds.Tables[0],"Fcreate_time");
			this.Label6_LastModify.Text		= PublicRes.objectToString(ds.Tables[0],"Fmodify_time");		   //"2005-05-01";
			this.Label7_SingleMax.Text		= classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fquota"));		   //"2000";
			this.Label8_PerDayLmt.Text		= classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fquota_pay"));			//"5000";
			this.Label9_LastSaveDate.Text   = PublicRes.objectToString(ds.Tables[0],"Fsave_time");				//"2005-03-01";
			this.Label10_Drawing.Text		= PublicRes.objectToString(ds.Tables[0],"Ffetch_time");              //"2005-04-15";
			this.Label11_Remark.Text		= PublicRes.objectToString(ds.Tables[0],"Fmemo");					//"这个家伙很懒，什么都没有留下！";
            this.Label12_Fstate.Text        = Transfer.accountState(PublicRes.objectToString(ds.Tables[0],"Fstate"));
            this.Label13_Fuser_type.Text    = Transfer.convertFuser_type(PublicRes.objectToString(ds.Tables[0],"Fuser_type"));
			
			try
			{
				// 改动trueName的值为userInfo表的trueName
				this.Label14_Ftruename.Text     = PublicRes.objectToString(ds.Tables[0],"UserRealName2");
			}
			catch
			{
				this.Label14_Ftruename.Text     = PublicRes.objectToString(ds.Tables[0],"Ftruename");
			}
			
			this.Label15_Useable.Text       = classLibrary.setConfig.FenToYuan((long.Parse(PublicRes.objectToString(ds.Tables[0],"Fbalance"))-long.Parse(PublicRes.objectToString(ds.Tables[0],"Fcon"))).ToString());  //帐户余额减去冻结余额= 可用余额
			this.Label16_Fapay.Text         = PublicRes.objectToString(ds.Tables[0],"Fapay");
			this.Label17_Flogin_ip.Text     = PublicRes.objectToString(ds.Tables[0],"Flogin_ip");

			//furion 20061116 email登录修改
			this.labEmail.Text = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0],"Femail"));
            this.labMobile.Text = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0], "Fmobile"));

			//2006-10-18 edwinyang 增加产品属性
            int nAttid = int.Parse(PublicRes.objectToString(ds.Tables[0], "Att_id"));
			this.Label18_Attid.Text			 = CheckBasicInfo(nAttid);
			this.lbInnerID.Text             = PublicRes.objectToString(ds.Tables[0],"fuid").Trim();
			this.lbFetchMoney.Text          = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Ffetch").Trim());
            this.lbLeftPay.Text             = Transfer.convertBPAY(PublicRes.objectToString(ds.Tables[0],"Fbpay_state").Trim());
			this.lbSave.Text                = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fsave").Trim());
			string fuid = PublicRes.objectToString(ds.Tables[0],"fuid").Trim();

			if(Label1_Acc.Text != "")
			{
                string testuid = new AccountService().QQ2Uid(Label1_Acc.Text);

				if(testuid != null)
				{
					labQQstate.Text = "注册未关联";

					if(testuid.Trim() == fuid)
					{
						//再判断是否是已激活 furion 20070226
                        string trueuid = new AccountService().QQ2UidX(Label1_Acc.Text);

						if(trueuid != null)
							labQQstate.Text = "已关联";
						else
							labQQstate.Text = "已关联未激活";
					}
				}
				else
					labQQstate.Text = "未注册";
			}
			else
				labQQstate.Text = "";

			if(labEmail.Text != "")
			{
                string testuid = new AccountService().QQ2Uid(labEmail.Text);

				if(testuid != null)
				{
					labEmailState.Text = "注册未关联";

					if(testuid.Trim() == fuid)
					{
						//再判断是否是已激活 furion 20070226
                        string trueuid = new AccountService().QQ2UidX(labEmail.Text);

						if(trueuid != null)
							labEmailState.Text = "已关联";
						else
							labEmailState.Text = "已关联未激活";
					}
				}
				else
					labEmailState.Text = "未注册";
			}
			else
				labEmailState.Text = "";

			if(labMobile.Text != "")
			{
                string testuid = new AccountService().QQ2Uid(labMobile.Text);

				if(testuid != null)
				{
					labMobileState.Text = "注册未关联";

					if(testuid.Trim() == fuid)
					{
						//再判断是否是已激活 furion 20070226
                        string trueuid = new AccountService().QQ2UidX(labMobile.Text);

						if(trueuid != null)
							labMobileState.Text = "已关联";
						else
							labMobileState.Text = "已关联未激活";
					}
				}
				else
					labMobileState.Text = "未注册";
			}
			else
				labMobileState.Text = "";

		}

		private string CheckBasicInfo(int nAttid)
		{
			DataSet ds = PermitPara.QueryDicAccName();

			if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				return "";
			}

			DataTable dt = ds.Tables[0];

			foreach(DataRow dr in dt.Rows)
			{
				if (nAttid == int.Parse(dr["Value"].ToString().Trim()))
				{
					return dr["Text"].ToString().Trim();
				}
			}
			return "";
		}

        protected void lbtnQueryOld_Click(object sender, EventArgs e)
        {
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            lbtnQueryOld.Text = "新版";
            iFramePath = "bankrollLog.aspx?type=QQID&BeginDate=" + begindate.ToString("yyyy-MM-dd 00:00:00") + "&EndDate=" + enddate.ToString("yyyy-MM-dd 23:59:59");
        }

	}
}
