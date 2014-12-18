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

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
	/// <summary>
    /// OrderRelationQuery 的摘要说明。
	/// </summary>
    public partial class OrderRelationQuery : System.Web.UI.Page
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
                string szlistid = this.txtSzlistid.Text.Trim();
                string sublistid = this.txtSublistid.Text.Trim();
                if (szlistid == "" && sublistid == "")
                    throw new Exception("查询条件不能为空！");

                BindInfo(szlistid, sublistid);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message.ToString());
            }
        }

        private void BindInfo(string sz_listid, string sub_listid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds;
            ds = qs.QueryRelationOrderList(sz_listid, sub_listid);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                dt.Columns.Add("Ftotal_fee_str", typeof(string)); //原单支付金额
                dt.Columns.Add("Fsub_total_fee_str", typeof(string)); //补差金额
                dt.Columns.Add("Fstate_str", typeof(string)); //交易状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "补差成功");
                ht1.Add("2", "补差失败");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ftotal_fee", "Ftotal_fee_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fsub_total_fee", "Fsub_total_fee_str");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fstate", "Fstate_str", ht1);


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
