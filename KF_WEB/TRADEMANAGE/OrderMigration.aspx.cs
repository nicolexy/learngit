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
	/// OrderMigration ��ժҪ˵����
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

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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
                labErrMsg.Text="���������󣬶����ű���ΪС�ڵ���32λ���֣�";
                WebUtils.ShowMessage(this.Page,labErrMsg.Text);
                return;
            }
            string msg = "";
            if(!MigrationCheck(orderId,out  msg))
            {
                labErrMsg.Text="���𶩵�Ǩ������ʧ�ܣ�ʧ����Ϣ����:<br>"+msg;
                WebUtils.ShowMessage(this.Page,"���𶩵�Ǩ������ʧ�ܣ�ʧ����Ϣ���£�"+PublicRes.GetErrorMsg(msg));
                return ;
            }
            else
            {
                labErrMsg.Text="���𶩵�Ǩ����������ɹ�!";
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

                PublicRes.CreateCheckService(this).StartCheck(orderId, "OrderMigration", "����Ǩ������", "0", parameters);

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
