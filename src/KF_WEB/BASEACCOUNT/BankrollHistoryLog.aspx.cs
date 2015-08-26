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
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
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

                string qqid = TextBox1_InputQQ.Text;
                Session["QQID"] = qqid;
                //账号类型:1,C账号;2,内部账号
                if (this.InternalID.Checked)
                {                   
                    Session["QQID"] = new AccountService().Uid2QQ(qqid);
                }
           
                GetUserInfo();
				//BindData(1,1);           //绑定数据

				iFramePath = "bankrollLog.aspx?type=QQID&BeginDate=" + begindate.ToString("yyyy-MM-dd 00:00:00") + "&EndDate=" + enddate.ToString("yyyy-MM-dd 23:59:59");
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"查询错误："+ ex.Message);
			}
		}

        private void GetUserInfo()
        {
            if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
            {
                WebUtils.ShowMessage(this.Page, "请输入账号!");
                return;
            }
            string qqid = TextBox1_InputQQ.Text;
            string type = "Uin";   //账号类型:Uin,C账号;Uid,内部账号
            if (this.InternalID.Checked)
            {
                type = "Uid";
            }
            try
            {
                //账户个人信息
                DataSet ds = new AccountService().GetPersonalInfo(qqid, type);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.Label1_Acc.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
                    this.labQQstate.Text = ds.Tables[0].Rows[0]["Fqqid_state"].ToString();
                    this.Label14_Ftruename.Text = ds.Tables[0].Rows[0]["Fname_str"].ToString();

                    this.labEmail.Text = ds.Tables[0].Rows[0]["Femail"].ToString();
                    this.labEmailState.Text = ds.Tables[0].Rows[0]["Femial_state"].ToString();
                    this.lbInnerID.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();

                    this.labMobile.Text = ds.Tables[0].Rows[0]["Fmobile"].ToString();
                    this.labMobileState.Text = ds.Tables[0].Rows[0]["Fmobile_state"].ToString();
                    this.lbLeftPay.Text = ds.Tables[0].Rows[0]["Fbpay_state_str"].ToString();

                    this.Label12_Fstate.Text = ds.Tables[0].Rows[0]["Fstate_str"].ToString();
                    this.Label13_Fuser_type.Text = ds.Tables[0].Rows[0]["Fuser_type_str"].ToString();

                    this.Label15_Useable.Text = ds.Tables[0].Rows[0]["Fuseable_fee"].ToString();
                    this.Label4_Freeze.Text = ds.Tables[0].Rows[0]["Ffreeze_fee"].ToString();

                    this.Label5_YestodayLeft.Text = ds.Tables[0].Rows[0]["Fyday_balance"].ToString();
                    this.Label3_LeftAcc.Text = ds.Tables[0].Rows[0]["Fbalance_str"].ToString();

                    this.Label2_Type.Text = ds.Tables[0].Rows[0]["Fcurtype_str"].ToString();
                    this.lblLoginTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();

                    this.Label16_Fapay.Text = ds.Tables[0].Rows[0]["Fapay"].ToString();
                    this.Label7_SingleMax.Text = ds.Tables[0].Rows[0]["Fquota_str"].ToString();

                    this.Label8_PerDayLmt.Text = ds.Tables[0].Rows[0]["Fquota_pay_str"].ToString();
                    this.lbFetchMoney.Text = ds.Tables[0].Rows[0]["Ffetch_str"].ToString();

                    this.lbSave.Text = ds.Tables[0].Rows[0]["Fsave_str"].ToString();
                    this.Label9_LastSaveDate.Text = ds.Tables[0].Rows[0]["Fsave_time"].ToString();

                    this.Label10_Drawing.Text = ds.Tables[0].Rows[0]["Ffetch_time"].ToString();
                    this.Label17_Flogin_ip.Text = ds.Tables[0].Rows[0]["Flogin_ip"].ToString();

                    this.Label6_LastModify.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
                    this.Label18_Attid.Text = ds.Tables[0].Rows[0]["Fpro_att"].ToString();

                    this.Label11_Remark.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
                }
            }
            catch { }
        }

		private void BindData(int istr,int imax)
		{
			Query_Service.Query_Service myService = new Query_Service.Query_Service();
			//myService.Finance_HeaderValue = classLibrary.setConfig.setFH(Session["uid"].ToString(),Request.UserHostAddress);    
			myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			DataSet ds = myService.GetUserAccount(Session["QQID"].ToString(),1,istr,imax);	

			if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1) 
			{
				throw new Exception("数据库无此记录");					
			}	

			this.Label1_Acc.Text			= ds.Tables[0].Rows[0]["Fqqid"].ToString();
			this.Label2_Type.Text			= classLibrary.setConfig.convertMoney_type(ds.Tables[0].Rows[0]["Fcurtype"].ToString());//tu.u_CurType;				   //"代金券";
			this.Label3_LeftAcc.Text		= classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fbalance"].ToString());//tu.u_Balance;				   //"3000";
			this.Label4_Freeze.Text			= classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fcon"].ToString());                  //"1000";
			this.Label5_YestodayLeft.Text	= ds.Tables[0].Rows[0]["Fyday_balance"].ToString();		   //"10";
			this.lblLoginTime.Text          = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
			this.Label6_LastModify.Text		= ds.Tables[0].Rows[0]["Fmodify_time"].ToString();		   //"2005-05-01";
			this.Label7_SingleMax.Text		= classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fquota"].ToString());		   //"2000";
			this.Label8_PerDayLmt.Text		= classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fquota_pay"].ToString());			//"5000";
			this.Label9_LastSaveDate.Text   = ds.Tables[0].Rows[0]["Fsave_time"].ToString();				//"2005-03-01";
			this.Label10_Drawing.Text		= ds.Tables[0].Rows[0]["Ffetch_time"].ToString();              //"2005-04-15";
			this.Label11_Remark.Text		= ds.Tables[0].Rows[0]["Fmemo"].ToString();					//"这个家伙很懒，什么都没有留下！";
			this.Label12_Fstate.Text        = classLibrary.setConfig.accountState(ds.Tables[0].Rows[0]["Fstate"].ToString());
			this.Label13_Fuser_type.Text    = classLibrary.setConfig.convertFuser_type(ds.Tables[0].Rows[0]["Fuser_type"].ToString());
			
			try
			{
				// 改动trueName的值为userInfo表的trueName
				this.Label14_Ftruename.Text     = ds.Tables[0].Rows[0]["UserRealName2"].ToString();
			}
			catch
			{
				this.Label14_Ftruename.Text     = ds.Tables[0].Rows[0]["Ftruename"].ToString();
			}
			
			this.Label15_Useable.Text       = classLibrary.setConfig.FenToYuan((long.Parse(ds.Tables[0].Rows[0]["Fbalance"].ToString())-long.Parse(ds.Tables[0].Rows[0]["Fcon"].ToString())).ToString());  //帐户余额减去冻结余额= 可用余额
			this.Label16_Fapay.Text         = ds.Tables[0].Rows[0]["Fapay"].ToString();
			this.Label17_Flogin_ip.Text     = ds.Tables[0].Rows[0]["Flogin_ip"].ToString();

			//furion 20061116 email登录修改
			this.labEmail.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Femail"]);
			this.labMobile.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fmobile"]);

			//2006-10-18 edwinyang 增加产品属性
			int nAttid =  int.Parse(ds.Tables[0].Rows[0]["Fatt_id"].ToString());
			this.Label18_Attid.Text			 = CheckBasicInfo(nAttid);
			this.lbInnerID.Text             = ds.Tables[0].Rows[0]["fuid"].ToString().Trim();
			this.lbFetchMoney.Text          = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Ffetch"].ToString().Trim());
			this.lbLeftPay.Text             = classLibrary.setConfig.convertBPAY(ds.Tables[0].Rows[0]["Fbpay_state"].ToString().Trim());
			this.lbSave.Text                = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fsave"].ToString().Trim());
			string fuid = ds.Tables[0].Rows[0]["fuid"].ToString().Trim();

			if(Label1_Acc.Text != "")
			{
				string testuid = myService.QQ2Uid(Label1_Acc.Text);

				if(testuid != null)
				{
					labQQstate.Text = "注册未关联";

					if(testuid.Trim() == fuid)
					{
						//再判断是否是已激活 furion 20070226
						string trueuid = myService.QQ2UidX(Label1_Acc.Text);

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
				string testuid = myService.QQ2Uid(labEmail.Text);

				if(testuid != null)
				{
					labEmailState.Text = "注册未关联";

					if(testuid.Trim() == fuid)
					{
						//再判断是否是已激活 furion 20070226
						string trueuid = myService.QQ2UidX(labEmail.Text);

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
				string testuid = myService.QQ2Uid(labMobile.Text);

				if(testuid != null)
				{
					labMobileState.Text = "注册未关联";

					if(testuid.Trim() == fuid)
					{
						//再判断是否是已激活 furion 20070226
						string trueuid = myService.QQ2UidX(labMobile.Text);

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
            //if (lbtnQueryOld.Text == "新版")
            //{
            //    lbtnQueryOld.Text = "旧版";
            //    iFramePath = "bankrollLog.aspx?isold=true&type=QQID&BeginDate=" + begindate.ToString("yyyy-MM-dd 00:00:00") + "&EndDate=" + enddate.ToString("yyyy-MM-dd 23:59:59");
            //}
            //else 
            //{
                lbtnQueryOld.Text = "新版";
                iFramePath = "bankrollLog.aspx?type=QQID&BeginDate=" + begindate.ToString("yyyy-MM-dd 00:00:00") + "&EndDate=" + enddate.ToString("yyyy-MM-dd 23:59:59");
            //}
        }

	}
}
