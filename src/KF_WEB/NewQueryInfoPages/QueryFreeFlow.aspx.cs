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
using CFT.CSOMS.BLL.UserAppealModule;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryFreeFlow 的摘要说明。
	/// </summary>
    public partial class QueryFreeFlow : System.Web.UI.Page
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
			//this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		private void ValidateDate()
		{
			
            string ccftno = cftNo.Text.ToString();
            if (ccftno == "")
            {
                throw new Exception("请输入查询项！");
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
            string s_cftno = cftNo.Text.Trim();

            lb_c1.Text = s_cftno;   
            DataSet ds = null;

            //vip项目专用DB业务下线
            //会员账号基本信息查询接口取消 
            //public DataSet QueryCFTMember(string account)
            //20150122 v_swuzhang
            ////1.查询会员
            //ds = new VIPService().QueryCFTMember(s_cftno);
            //if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count != 1)
            //{
            //    //会员不存在
            //    lb_c2.Text = "否";
            //}
            //else
            //{
            //    DataRow row = ds.Tables[0].Rows[0];
            //    //lb_c1.Text = row["Fuin"].ToString();
            //    lb_c2.Text = "是";//是否为QQ会员
            //    lb_c3.Text = row["Fvip_exp_date"].ToString();//会员过期时间
            //}


            //2.查询实名认证
            bool stateMsg = false;
            DataSet authenState = new UserAppealService().GetUserAuthenState(s_cftno, "", 0, out stateMsg);
            if (stateMsg)
            {
                lb_c4.Text = "是";
            }
            else
            {
                lb_c4.Text = "否";
            }

            //3.免费流量
            ds = new VIPService().GetFreeFlowInfo(s_cftno);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                lb_c6.Text = classLibrary.setConfig.FenToYuan(dt.Rows[0]["free_amount"].ToString());//免费流量
            }

            //4.白名单、大额用户
            ds = new AccountService().GetUserTypeInfo(s_cftno, 3, 1, 0, 1, 1); //1转账白名单
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                string ys = dt.Rows[0]["eip_user"].ToString();
                if (ys == "Y")
                {
                    lb_c5.Text = "是";
                }
                else if (ys == "N")
                {
                    lb_c5.Text = "否";
                }
                else
                {
                    lb_c5.Text = "";//是否为白名单
                }

            }
            else
            {
                lb_c5.Text = "否";
            }

            ds = new AccountService().GetUserTypeInfo(s_cftno, 5, 1, 0, 1, 1); //0提现大额用户
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                string ys = dt.Rows[0]["eip_user"].ToString();
                if (ys == "Y")
                {
                    lb_c7.Text = "是";
                }
                else if (ys == "N")
                {
                    lb_c7.Text = "否";
                }
                else
                {
                    lb_c7.Text = "";//是否为大额用户
                }

            }
            else
            {
                lb_c7.Text = "否";
            }
        }

        private void clearDetailTB() {
            lb_c1.Text = "";
            //lb_c2.Text = "";
            //lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";
        }
	}
}