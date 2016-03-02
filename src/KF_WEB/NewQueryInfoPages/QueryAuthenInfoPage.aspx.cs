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
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;

using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.UserAppealModule;


namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryAuthenInfoPage 的摘要说明。
	/// </summary>
	public partial class QueryAuthenInfoPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button btn_submit_acc;
		protected System.Web.UI.WebControls.TextBox TextBox1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				if(Session["OperID"] != null)
					this.lb_operatorID.Text = Session["OperID"].ToString();
			}

            if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
			
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

		
		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			string acc = this.tbx_acc.Text.ToString();
			string bankID = this.tbx_bacc.Text.ToString();
			int bankType = 0;

            if (acc == "" && bankID == "")
			{
				WebUtils.ShowMessage(this,"请输入查询条件");
				return;
			}
			
			try
			{
				this.Clear();
                this.lb_queryAcc.Text = acc;

				string opLog = SensitivePowerOperaLib.MakeLog("get","","",acc,bankID.ToString(),bankType.ToString());

                SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", opLog, this);

				if(bankID != null && bankID.Trim() != "")
					bankType = classLibrary.getData.GetBankCodeFromBankName(this.ddl_bankType.SelectedValue);
								
                bool stateMsg = false;
                DataSet ds = new UserAppealService().GetUserAuthenState(acc, bankID, bankType, out stateMsg);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					//ShowMsg("查询结果为空");
					WebUtils.ShowMessage(this,"查询结果为空");
					return;
				}

				DataRow dr = ds.Tables[0].Rows[0];

				if(dr["queryType"].ToString() == "1")
				{
                    //增加列，进行显示处理 yinhuang 2013.6.14
                    ds.Tables[0].Columns.Add("authenStateName", typeof(string));//身份认证状态
                    ds.Tables[0].Columns.Add("authenTypeName", typeof(string));//认证方式
                    ds.Tables[0].Columns.Add("creTypeName", typeof(string));//证件类型
                    ds.Tables[0].Columns.Add("Fcard_stat_Name", typeof(string));//银行打款状态

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("1", "已认证");
                    ht1.Add("2", "打款中");
                    ht1.Add("3", "打款结束认证中");
                    ht1.Add("4", "打款失败");
                    ht1.Add("5", "多次金额确认失败");
                    ht1.Add("9", "uin打款锁定");
                    ht1.Add("10", "没用");

                    Hashtable ht2 = new Hashtable();
                    ht2.Add("1", "身份证");
                    ht2.Add("2", "护照");
                    ht2.Add("3", "军官证");

                    Hashtable ht3 = new Hashtable();
                    ht3.Add("1", "已认证");
                    ht3.Add("2", "待认证");
                    ht3.Add("3", "确认错误等待更改信息");
                    ht3.Add("4", "修改注册信息失败");
                    ht3.Add("9", "uin客服审核锁定");
                    ht3.Add("10", "作废");
                    ht3.Add("0", "身份信息未审核");

                    Hashtable ht4 = new Hashtable();
                    ht4.Add("1", "客服认证");
                    ht4.Add("2", "工行认证");
                    ht4.Add("3", "paipai认证");
                    ht4.Add("4", "授权认证");
                    ht4.Add("5", "公安部实名");
                    ht4.Add("6", "一点通实名认证");

                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcard_stat", "Fcard_stat_Name", ht1);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcre_type", "creTypeName", ht2);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcre_stat", "authenStateName", ht3);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fauthen_type", "authenTypeName", ht4);

					this.lb_c1.Text = dr["authenTypeName"].ToString();
					this.lb_c2.Text = setConfig.replaceHtmlStr(dr["Fcard_stat_Name"].ToString());
					this.Label3.Text = dr["authenStateName"].ToString();
					this.lb_c3.Text = dr["creTypeName"].ToString();
					this.lb_c4.Text = dr["Fcre_id"].ToString();
					this.lb_c5.Text = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
					this.lb_c6.Text = dr["Fbank_id"].ToString();
					this.lb_c7.Text = dr["Ffirst_authen_id"].ToString();
					this.Label4.Text = dr["Fqqid"].ToString();
					this.lb_c10.Text = setConfig.replaceHtmlStr(dr["Ftried_times"].ToString());
					this.lb_c11.Text = setConfig.replaceHtmlStr(dr["Fcre_change_times"].ToString());
					this.lb_c12.Text = setConfig.replaceHtmlStr(dr["Fcard_changed_times"].ToString());

					this.lb_userStatue.Text = "用户实名认证中";
				}
				else if(dr["queryType"].ToString() == "2")
				{
                    ds.Tables[0].Columns.Add("authenTypeName", typeof(string));//认证方式
                    ds.Tables[0].Columns.Add("creTypeName", typeof(string));//证件类型

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("1", "打款认证+客服审核");
                    ht1.Add("2", "工行认证");
                    ht1.Add("3", "paipai认证");
                    ht1.Add("4", "授权认证");
                    ht1.Add("5", "公安部实名");
                    ht1.Add("6", "一点通实名认证");
                    ht1.Add("7", "重号置实名");
                    ht1.Add("8", "重号置实名认证");
                    ht1.Add("9", "客服审核");
                    ht1.Add("10", "认证未通过");
                    ht1.Add("11", "打款认证+公安部鉴权");

                    Hashtable ht2 = new Hashtable();
                    ht2.Add("1", "身份证");
                    ht2.Add("2", "护照");
                    ht2.Add("3", "军官证");
                    ht2.Add("100", "对公鉴权");

                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fvalue", "authenTypeName", ht1);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fstandby1", "creTypeName", ht2);

                    this.lb_c15.Text = setConfig.replaceHtmlStr(dr["authenTypeName"].ToString());
					this.lb_c16.Text = setConfig.replaceHtmlStr(dr["creTypeName"].ToString());
                    var creid = setConfig.replaceHtmlStr(dr["Fattach"].ToString());
                    this.lb_c17.Text = setConfig.ConvertID(creid, creid.Length - 6, 3);
					this.lb_c18.Text = setConfig.replaceHtmlStr(dr["Fcreate_time"].ToString());
					this.lb_c19.Text = setConfig.replaceHtmlStr(dr["Fmodify_time"].ToString());
					this.lb_c20.Text = setConfig.replaceHtmlStr(dr["Fstandby3"].ToString());

					this.lb_userStatue.Text = "用户已通过实名认证";
				}

				//this.div_detail.Style.Add("display","inline");
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
				//WebUtils.ShowMessage(this,setConfig.replaceHtmlStr(ex.Message));
			}
		}

		private void Clear()
		{
            this.lb_queryAcc.Text = "";
            this.lb_c1.Text = "";
			this.lb_c2.Text = "";
			this.lb_c3.Text = "";
			this.lb_c4.Text = "";
			this.lb_c5.Text = "";
			this.lb_c6.Text = "";
			this.lb_c7.Text = "";
			this.lb_c10.Text = "";
			this.lb_c11.Text = "";
			this.lb_c12.Text = "";
			this.Label3.Text = "";
			this.Label4.Text = "";

			this.lb_c15.Text = "";
			this.lb_c16.Text = "";
			this.lb_c17.Text = "";
			this.lb_c18.Text = "";
			this.lb_c19.Text = "";
			this.lb_c20.Text = "";
		}

	}
}
