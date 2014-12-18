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
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// OrderMigration 的摘要说明。
	/// </summary>
    public class OrderMigration : System.Web.UI.Page
	{
        protected System.Web.UI.WebControls.Label Label_uid;
        protected System.Web.UI.WebControls.TextBox txtOrderId;
        protected System.Web.UI.WebControls.Button btMigration;
        protected System.Web.UI.WebControls.Label labErrMsg;
        protected System.Web.UI.WebControls.Label lblUId;
        protected System.Web.UI.WebControls.RegularExpressionValidator rfvNum;
    
		private void Page_Load(object sender, System.EventArgs e)
		{
           lblUId.Text = Session["uid"].ToString();
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
            this.btMigration.Click += new System.EventHandler(this.btMigration_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion

        private void btMigration_Click(object sender, System.EventArgs e)
        {

            string orderId = txtOrderId.Text.Trim();
            if (!SunLibraryEX.StringEx.IsNumber(orderId) || !SunLibraryEX.StringEx.MatchLength(orderId, 0, 32))
            {
                labErrMsg.Text="订单号有误，订单号必须为小于等于32位数字！";
                WebUtils.ShowMessage(this.Page,labErrMsg.Text);
                return;
            }
            string msg = "";
            if(!MigrationCheck(orderId,out  msg))
            {
                labErrMsg.Text="提起订单迁移审批失败，失败信息如下:<br>"+msg;
                WebUtils.ShowMessage(this.Page,"提起订单迁移审批失败，失败信息如下："+PublicRes.GetErrorMsg(msg));
                return ;
            }
            else
            {
                labErrMsg.Text="提起订单迁移审批申请成功!";
                WebUtils.ShowMessage(this.Page,labErrMsg.Text);
							
            }
        }

        private bool MigrationCheck(string orderId,out string msg)
        {
            msg="";
            try
            {
                Check_WebService.Param[] parameters = new Check_WebService.Param[1];
                parameters[0] = new Check_WebService.Param();
                parameters[0].ParamName = "MsgId";
                parameters[0].ParamValue = commLib.GenID.GenOrderMigrationMSGId(orderId);

                PublicRes.CreateCheckService(this).StartCheck(orderId, "OrderMigration", "订单迁移申请", "0", parameters);

                return true;
            }
            catch(Exception ex)
            {
                msg+=ex.Message;
                return false;
            }

        }
	}
}
