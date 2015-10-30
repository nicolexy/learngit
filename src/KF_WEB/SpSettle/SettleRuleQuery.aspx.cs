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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
	/// <summary>
    /// SettleRuleQuery 的摘要说明。
	/// </summary>
    public partial class SettleRuleQuery : System.Web.UI.Page
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            // 在此处放置用户代码以初始化页面
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());
                BalanceRollPager.RecordCount = 10000;
                BalanceRollPager.CurrentPageIndex = 1;
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
		}

        protected void Button_qry_Click(object sender, System.EventArgs e) 
        {
            try
            {
                string spid = this.txtSpid.Text.Trim();
                if (spid == "")
                    throw new Exception("商户号不能为空！");

                BindInfo(spid, 0, 10);
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

        private void BindInfo(string spid, int offset, int limit)
        {
            SettleService settleService = new SettleService();
            DataTable dt = settleService.QuerySettleRuleList(spid, offset, limit);
            if (dt != null)
            {
                dt.Columns.Add("Fcharge_method_str", typeof(string)); //收费方式
                dt.Columns.Add("Fstate_str", typeof(string)); //记录状态
                dt.Columns.Add("Fsettle_type_str", typeof(string)); //业务类型

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "原分账时收取");
                ht1.Add("2", "收支分离");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "正常");
                ht2.Add("2", "作废");
                Hashtable ht3 = new Hashtable();
                ht3.Add("0", "默认");
                ht3.Add("1", "委托代扣");

                classLibrary.setConfig.DbtypeToPageContent(dt, "Fcharge_method", "Fcharge_method_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Fstate", "Fstate_str", ht2);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Fsettle_type", "Fsettle_type_str", ht3);

                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else
            {
                throw new Exception("没有找到记录！");
            }
        }

        protected void BalanceRollPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                this.BalanceRollPager.CurrentPageIndex = e.NewPageIndex;

                BindInfo(this.txtSpid.Text.Trim(), e.NewPageIndex,10);
               
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
	}
}
