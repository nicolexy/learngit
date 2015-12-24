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
using CFT.CSOMS.BLL.WechatPay;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
    /// Summary description for BankBillNoQuery.
	/// </summary>
    public partial class BankBillNoQuery : System.Web.UI.Page
	{
		
		string strBeginDate = "",strEndDate = "";

		protected void Page_Load(object sender, System.EventArgs e)
		{
          
			if(!IsPostBack)
			{
                try
                {
                    if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                }
                catch
                {
                    Response.Redirect("../login.aspx?wh=1");
                }

				this.tbx_beginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
			}
		}


		protected void btn_query_Click(object sender, System.EventArgs e)
        {
            if (!validate())
            {
                return;
            }
            this.pager.RecordCount = 1000;
			BindData(1);
		}
		

		private bool validate()
		{
			if(tbx_acc.Text.Trim() == string.Empty)
			{
				ShowMsg("请输入用户账号！");
				return false;
			}

            if (this.tbx_beginDate.Text.Trim() == string.Empty)
            {
                ShowMsg("请输入日期！");
                return false;
            }

            if (this.tbx_bankType.Text.Trim() == string.Empty)
            {
                ShowMsg("请输入银行类型！");
                return false;
            }

            try
            {
                strBeginDate = DateTime.Parse(this.tbx_beginDate.Text).ToString("yyyyMMdd");
                return true;
            }
            catch
            {
                ShowMsg("日期格式不正确！");
                return false; ;
            }
		}


		private void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}


		private void BindData(int index)
		{
            try
            {
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);

                DataSet ds = new FastPayService().QueryBankBillNoList(this.tbx_acc.Text.Trim(), this.tbx_bankType.Text.Trim(), strBeginDate.Trim(), int.Parse(this.ddlBiuType.SelectedValue), start, max);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.ShowMsg("查询记录为空!");
                }
                this.DataGrid_QueryResult.DataSource = ds;
                this.DataGrid_QueryResult.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "查询异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			pager.RecordCount = 1000;
			pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(ChangePage);
		}
		#endregion
	}
}
