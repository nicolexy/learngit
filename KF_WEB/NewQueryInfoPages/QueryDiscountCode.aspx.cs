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
using System.Xml;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryDiscountCode 的摘要说明。
	/// </summary>
    public partial class QueryDiscountCode : System.Web.UI.Page
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
            string ccftno = cftNo.Text.ToString();
            string cdkno = cdkNo.Text.ToString();

            if (this.rbCftno.Checked)
            {
                if (string.IsNullOrEmpty(ccftno))
                {
                    throw new Exception("请输入账号！");
                }
            }
            if (this.rbCdkno.Checked)
            {
                if (string.IsNullOrEmpty(cdkno))
                {
                    throw new Exception("请输入批次号！");
                }
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
                this.pager.RecordCount = 1000;
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

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            int rid = e.Item.ItemIndex;
            
            GetDetail(rid);
        }

        private void GetDetail(int rid)
        {
            //需要注意分页情况
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null)
            {
                lb_c1.Text = g_dt.Rows[rid]["Fbillno"].ToString();//打折密码对应提现单
                lb_c2.Text = g_dt.Rows[rid]["Fstartdate"].ToString();//发放时间
                lb_c3.Text = g_dt.Rows[rid]["Fuserbillno"].ToString();//自还部分提现单
                lb_c4.Text = g_dt.Rows[rid]["Fmodifytime"].ToString();//使用时间
                lb_c5.Text = g_dt.Rows[rid]["Fuserpay_str"].ToString();//自还金额
                lb_c6.Text = g_dt.Rows[rid]["Fvaliddate"].ToString();//有效期
                lb_c7.Text = "";//是否允许赠送
                lb_c8.Text = g_dt.Rows[rid]["Fstate_str"].ToString();//业务状态
                lb_c9.Text = "";//备注
                
                
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
            
        }

        private void BindData(int index)
		{
            string s_cftno = "";
            string s_cdkno = "";

            if (this.rbCftno.Checked)
            {
                s_cftno = cftNo.Text.ToString().Trim();

            }
            else if (this.rbCdkno.Checked)
            {
                s_cdkno = cdkNo.Text.ToString().Trim();
            }
            string s_status = ddlStatus.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryDiscountCode(s_cftno, s_cdkno, s_status, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
                ViewState["g_dt"] = ds.Tables[0];

                ds.Tables[0].Columns.Add("Fcdk_name", typeof(String));//打折密码名称
                ds.Tables[0].Columns.Add("Fcdkeytype_str", typeof(String));//类型
                ds.Tables[0].Columns.Add("Fstate_str", typeof(String));//状态
                ds.Tables[0].Columns.Add("Fbankid_wh", typeof(String));//卡尾号

                ds.Tables[0].Columns.Add("Famount_str", typeof(String));//面值
                ds.Tables[0].Columns.Add("Ffeelimit_str", typeof(String));//最低还款额
                ds.Tables[0].Columns.Add("Fuserpay_str", typeof(String));//自还金额

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "锁定");
                ht1.Add("2", "已使用");
                ht1.Add("3", "已作废");
                ht1.Add("4", "补单成功");
                ht1.Add("999", "黑名单");
                ht1.Add("0", "未使用");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string cdkeytype_str = "";
                    string s_cdkeyType = dr["Fcdkeytype"].ToString();
                    if (!string.IsNullOrEmpty(s_cdkeyType))
                    {
                        int key = Int32.Parse(s_cdkeyType);
                        if (key < 10000)
                        {
                            cdkeytype_str = "信用卡还款打折密码";
                        }
                        else if (key > 10000)
                        {
                            cdkeytype_str = "还房贷打折密码";
                        }
                        else
                        {
                            cdkeytype_str = "未知类型：" + key;
                        }
                    }
                    else {
                        cdkeytype_str = "未知类型：" + s_cdkeyType;
                    }
                    
                    //卡尾号
                    string s_bankid = dr["Fbankid"].ToString();
                    if (s_bankid.Length > 4) {
                        s_bankid = s_bankid.Substring(s_bankid.Length-4, 4);
                    }
                  
                    dr.BeginEdit();
                    dr["Fcdkeytype_str"] = cdkeytype_str;
                    dr["Fbankid_wh"] = s_bankid;
                    dr.EndEdit();
                }

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fstate", "Fstate_str", ht1);
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Famount", "Famount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ffeelimit", "Ffeelimit_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fuserpay", "Fuserpay_str");

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
                DataGrid1.DataSource = null;
				throw new LogicException("没有找到记录！");
			}
		}

	}
}