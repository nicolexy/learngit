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
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryYTInfo 的摘要说明。
	/// </summary>
    public partial class QueryYTInfo : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            //radioListOrder.Attributes.Add("onclick", "showRadioClick()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Value = "";
                }

                if (this.rbYtno.Checked)
                {
                    detailTB.Visible = true;
                    certTB.Visible = false;
                }
                else
                {
                    certTB.Visible = true;
                    detailTB.Visible = false;
                } 
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
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
			//this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

	
		private void ValidateDate()
		{
			DateTime begindate;

			try
			{
                string s_date = TextBoxBeginDate.Value;
                if (s_date != null && s_date != "") {
                    begindate = DateTime.Parse(s_date);
                }
			}
			catch
			{
				throw new Exception("日期输入有误！");
			}
            string ccftno = cftNo.Text.ToString();
            string cytno = ytNo.Text.ToString();
            if (ccftno == "" && cytno == "")
            {
                throw new Exception("请至少输入一个查询项！");
            }
            
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}
            
			try
			{
                clearDetailTB();
				BindData();
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

        private void BindData()
		{
            bool isRight = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
            string s_time = TextBoxBeginDate.Value;
            string s_begindate = "";
            if (s_time != null && s_time != "") {
                DateTime begindate = DateTime.Parse(s_time);
                s_begindate = begindate.ToString("yyMM");
            }
            
            string s_cftno = cftNo.Text.ToString();
            string s_ytno = ytNo.Text.ToString();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ht = qs.GetYTInfoList(s_cftno, s_ytno, s_begindate,"1");

            if (ht != null && ht.Tables.Count>0)
            {
                DataTable dt = ht.Tables[0];
                string day_limit = classLibrary.setConfig.FenToYuan(dt.Rows[0]["day_sum_limit"].ToString());
                string single_limit = classLibrary.setConfig.FenToYuan(dt.Rows[0]["per_tran_limit"].ToString());
                string no_opt_limit = classLibrary.setConfig.FenToYuan(dt.Rows[0]["no_otp_per_tran_limit"].ToString());

                //lb_c1.Text = s_cftno;//财付通账号
                lb_c2.Text = dt.Rows[0]["create_time"].ToString();//首次开通时间
                lb_c3.Text = classLibrary.setConfig.ConvertID(dt.Rows[0]["card_id"].ToString(),6,5);//运通账号
                string status = dt.Rows[0]["status"].ToString();//状态
                string s_status = "";
                if (status == "1") {
                    s_status = "正常";
                }
                else if (status == "2") {
                    s_status = "销户";
                }
                else if (status == "3")
                {
                    s_status = "冻结";
                }
                else if (status == "4")
                {
                    s_status = "过期";
                }
                lb_c4.Text = s_status;
                lb_c10.Text = dt.Rows[0]["address_num"].ToString();//账单地址个数

                status = dt.Rows[0]["otp_status"].ToString();//状态
                s_status = "";
                if (status == "0")
                {
                    s_status = "初始化";
                }
                else if (status == "1")
                {
                    s_status = "已签协议";
                }
                else if (status == "2")
                {
                    s_status = "解除协议";
                }
                else if (status == "3")
                {
                    s_status = "冻结";
                }

                lb_c11.Text = s_status;//无安全码绿色通道
                lb_c12.Text = single_limit;//单笔设置支付限额
                lb_c13.Text = no_opt_limit;//无安全码商家单笔最大限额
                //lb_c14.Text = dt.Rows[0]["last_update_time"].ToString();//最近一次更新时间
                lb_c15.Text = day_limit;//单日支付限额
                //lb_c16.Text = dt.Rows[0]["last_login_ip"].ToString();//最后一次登录IP
                lb_c17.Text = "";//可用余额
                lb_c18.Text = "";//冻结金额

                string qqid = "";
                string uid = dt.Rows[0]["uid"].ToString();
                //string uid = "";
                if (s_cftno != null && s_cftno != "")
                {
                    //通过财付通账号去查询信息
                    qqid = s_cftno;
                }
                else if (uid != null && uid != "")
                {
                    qqid = qs.Uid2QQ(uid);
                }

                lb_c1.Text = qqid;//财付通账号
                DataSet ds = new DataSet();
                DataSet ds_acc = new DataSet();
                if (qqid != null && qqid != "")
                {

                    ds = new AccountService().GetUserInfo(qqid, 0, 1, 1);
                    ds_acc = new AccountService().GetUserAccount(qqid, 1, 1, 1);
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    //个人信息

                    lb_c5.Text = classLibrary.setConfig.ConvertName(ds.Tables[0].Rows[0]["Ftruename"].ToString(), isRight);   //姓名
                    string s_cretype = PublicRes.GetString(ds.Tables[0].Rows[0]["Fcre_type"]);//证件类型
                    if (s_cretype == "1")
                    {
                        lb_c6.Text = "身份证";
                    }
                    else {
                        lb_c6.Text = "";
                    }
                    
                    lb_c7.Text = classLibrary.setConfig.ConvertTelephoneNumber(PublicRes.GetString(ds.Tables[0].Rows[0]["Fmobile"]), isRight);//手机号
                    if (lb_c6.Text == "身份证")
                    {
                        lb_c8.Text = classLibrary.setConfig.IDCardNoSubstring(ds.Tables[0].Rows[0]["Fcreid"].ToString(), isRight);//证件号码
                    }
                    else 
                    {
                        lb_c8.Text = ds.Tables[0].Rows[0]["Fcreid"].ToString();// classLibrary.setConfig.ConvertCreID(ds.Tables[0].Rows[0]["Fcreid"].ToString());//证件号码
                    }
                   
                    lb_c9.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Femail"]);//email
                }
                if (ds_acc != null && ds_acc.Tables.Count > 0) 
                {
                    lb_c18.Text = classLibrary.setConfig.FenToYuan(ds_acc.Tables[0].Rows[0]["Fcon"].ToString());//冻结金额
                    lb_c17.Text = classLibrary.setConfig.FenToYuan((long.Parse(ds_acc.Tables[0].Rows[0]["Fbalance"].ToString()) - long.Parse(ds_acc.Tables[0].Rows[0]["Fcon"].ToString())).ToString());
                }
            }
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

        private void clearDetailTB() {
            lb_c1.Text = "";
            lb_c2.Text = "";
            lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";
            lb_c8.Text = "";
            lb_c9.Text = "";
            lb_c10.Text = "";

            lb_c11.Text = "";
            lb_c12.Text = "";
            lb_c13.Text = "";
            lb_c14.Text = "";
            lb_c15.Text = "";
            lb_c16.Text = "";
            lb_c17.Text = "";
            lb_c18.Text = "";
        }

        private void clearCertTB()
        {
            lb_c19.Text = "";
            lb_c20.Text = "";
        }

        public void btnCert_Click(object sender, System.EventArgs e)
        {

            try
            {
                string cert_no = certNo.Text.ToString();
                if (cert_no == "") 
                {
                    WebUtils.ShowMessage(this.Page, "请输入身份证号！");
                    return;
                }
                clearCertTB();
                BindData2();
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private void BindData2()
        {
            string s_certno = certNo.Text.ToString();

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ht = qs.GetCertNum(s_certno,"1");

            if (ht != null && ht.Tables.Count > 0)
            {
                DataTable dt = ht.Tables[0];

                bool isRight = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
                lb_c19.Text = classLibrary.setConfig.IDCardNoSubstring(s_certno, isRight);
                lb_c20.Text = dt.Rows[0]["num"].ToString();
            }
            else
            {
                throw new LogicException("没有找到记录！");
            }
        }

	}
}