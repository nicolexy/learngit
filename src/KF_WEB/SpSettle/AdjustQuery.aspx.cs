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
    /// AdjustQuery 的摘要说明。
	/// </summary>
    public partial class AdjustQuery : System.Web.UI.Page
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            // 在此处放置用户代码以初始化页面
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

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
                string listid = this.txtListid.Text.Trim();
                string orderid = this.txtOrderid.Text.Trim();
                string spid = this.txtSpid.Text.Trim();
                if (listid == "" && orderid == "" && spid == "") 
                {
                    throw new Exception("请输入查询条件！");
                }
                string adjust_time = TextBoxBeginDate.Value.Trim();
                if (spid != "" && adjust_time == "") 
                {
                    throw new Exception("请选择调帐日期！");
                }
                if (adjust_time != "") 
                {
                    DateTime begindate = DateTime.Parse(adjust_time);
                    adjust_time = begindate.ToString("yyyy-MM-dd");
                }
                
                BindInfo(listid, orderid, spid, adjust_time);
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

        private void BindInfo(string szListid, string orderid, string spid, string adjust_time)
        {
            SettleService service = new SettleService();
            DataTable dt = service.QueryAdjustList(szListid, orderid, spid, adjust_time);

            if (dt != null  )
            {
                dt.Columns.Add("Fnum_str", typeof(string)); //调帐金额
                dt.Columns.Add("Ftype_str", typeof(string));//调帐类型
                dt.Columns.Add("Fstatus_str", typeof(string)); //调帐状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "调入");
                ht1.Add("2", "调出");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "初始状态");
                ht2.Add("2", "待审批");
                ht2.Add("3", "处理中");
                ht2.Add("4", "处理成功");
                ht2.Add("5", "处理失败");

                classLibrary.setConfig.FenToYuan_Table(dt, "Fnum", "Fnum_str");

                classLibrary.setConfig.DbtypeToPageContent(dt, "Ftype", "Ftype_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Fstatus", "Fstatus_str", ht2);

                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else 
            {
                throw new Exception("没有找到记录！");
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
