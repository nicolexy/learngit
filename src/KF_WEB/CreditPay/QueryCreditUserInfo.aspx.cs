using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.KF_Web.CreditPay
{
	/// <summary>
    /// QueryCreditUserInfo 的摘要说明。yinhuang 2013/11/25
	/// </summary>
    public partial class QueryCreditUserInfo : System.Web.UI.Page
	{

        protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    

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
		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			//BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
            string ccftno = cftNo.Text.ToString();
            
            if (ccftno == "")
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
                clearDT();
                BindData(1);
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

        private void clearDT() 
        {
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

        private void BindData(int index)
		{
            string s_cftno = cftNo.Text.ToString();
            
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryCreditUserInfo(s_cftno);
            
            if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("baseline_total_str", typeof(String));//总额度
                ds.Tables[0].Columns.Add("baseline_rest_str", typeof(String));//可用额度
                ds.Tables[0].Columns.Add("baseline_used_str", typeof(String));//已用额度
                ds.Tables[0].Columns.Add("tmpline_total_str", typeof(String));//临时额度
                ds.Tables[0].Columns.Add("return_balance_str", typeof(String));//溢出资金

                ds.Tables[0].Columns.Add("bank_acc_status_str", typeof(String));//账户状态
                ds.Tables[0].Columns.Add("bank_creline_status_str", typeof(String));//额度状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("10", "正常");
                ht1.Add("22", "逾期");
                ht1.Add("23", "逾期");
                ht1.Add("24", "逾期");
                ht1.Add("25", "逾期");
                ht1.Add("81", "呆账-人工核销");
                ht1.Add("89", "呆账-已完全收回");
                ht1.Add("90", "已结清");
                ht1.Add("1N", "未预授信");

                Hashtable ht2 = new Hashtable();
                ht2.Add("10", "正常");
                ht2.Add("21", "暂时停用(客户申请)");
                ht2.Add("22", "冻结(逾期，系统自动设定)");
                ht2.Add("23", "冻结(内部人员人工设定)");
                ht2.Add("24", "冻结(中途审核，系统自动设定)");
                ht2.Add("81", "已过激活期限，已失效");
                ht2.Add("82", "已过使用期限，已失效");
                ht2.Add("90", "已注销");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "baseline_total", "baseline_total_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "baseline_rest", "baseline_rest_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "baseline_used", "baseline_used_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "tmpline_total", "tmpline_total_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "return_balance", "return_balance_str");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "bank_acc_status", "bank_acc_status_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "bank_creline_status", "bank_creline_status_str", ht2);

                DataRow dr = ds.Tables[0].Rows[0];
                lb_c1.Text = dr["Fqqid"].ToString();  //账号
                lb_c2.Text = dr["Ftruename"].ToString();  //姓名
                if (!(dr["Fcredit"] is DBNull))
                {
                    if (dr["Fcredit"].ToString() != "") 
                    {
                        lb_c3.Text = classLibrary.setConfig.ConvertID(dr["Fcredit"].ToString(), 4, 2); //身份证号
                    }
                }

                lb_c4.Text = dr["Fmobile"].ToString();  //手机号码
                lb_c5.Text = dr["Femail"].ToString();  //邮箱

                if (!(dr["Fcredit_result"] is DBNull)) 
                {
                    if (dr["Fcredit_result"].ToString() == "Y")
                    {
                        lb_c6.Text = "征信成功";  //征信结果
                    }
                    else 
                    {
                        lb_c6.Text = "征信失败";  //征信结果
                    }
                }

                
                lb_c7.Text = dr["baseline_total_str"].ToString();  //总额度

              
                if (!(dr["activeFlag"] is DBNull)) 
                {
                    string s = dr["activeFlag"].ToString();
                    if (string.IsNullOrEmpty(s) || s == "2")
                    {
                        lb_c8.Text = "未激活";
                    }
                    else 
                    {
                        lb_c8.Text = "已激活";
                    }
                }

                lb_c9.Text = dr["baseline_rest_str"].ToString();  //可用额度
                lb_c10.Text = dr["activate_date"].ToString();  //激活日期
                lb_c11.Text = dr["baseline_used_str"].ToString();  //已用额度
                lb_c12.Text = dr["bank_acc_status_str"].ToString();  //账户状态
                lb_c13.Text = dr["tmpline_total_str"].ToString();  //临时额度

                if (!(dr["Fret_date"] is DBNull))
                {
                    string s_date = dr["Fret_date"].ToString();
                    if (string.IsNullOrEmpty(s_date) || s_date == "0")
                    {
                        lb_c14.Text = s_date;
                    }
                    else 
                    {
                        int billdate = int.Parse(dr["Fret_date"].ToString()) - 7;
                        lb_c14.Text = billdate.ToString();//账单日
                    }
                }

                lb_c15.Text = dr["Fline_expdate"].ToString();  //额度有效期
                lb_c16.Text = dr["Fret_date"].ToString();  //还款日
                lb_c17.Text = dr["bank_creline_status_str"].ToString();  //额度状态
                lb_c18.Text = dr["return_balance_str"].ToString();
            }
            else
            {
                throw new LogicException("没有找到记录！");
            }
        }

	}
}