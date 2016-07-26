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
    /// SettleReqDetail 的摘要说明。
	/// </summary>
    public partial class SettleReqDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
            if (!IsPostBack) 
            {
                string listid = Request.QueryString["listid"];
                if (listid != null && !string.IsNullOrEmpty(listid.Trim()))
                {
                    try
                    {
                        if (listid.Length != 28)
                            throw new Exception("请输入28位订单号！");
                        listid = listid.Trim();
                        this.txtListid.Text = listid;
                        BindInfo(listid);
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
            }
            
		}

        protected void Button_qry_Click(object sender, System.EventArgs e) 
        {
            try
            {
                string listid = this.txtListid.Text.Trim();

                if (listid.Length != 28)
                    throw new Exception("请输入28位订单号！");
                

                BindInfo(listid);
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

        private void BindInfo(string szListid)
        {
       
            SettleService service = new SettleService();
            DataTable dt = service.GetSettleReqInfo(szListid);

            if (dt != null )
            {
                dt.Columns.Add("Fstate_str", typeof(string)); //分账状态
                dt.Columns.Add("Fsettle_num_str", typeof(string)); //分账金额

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "分账前");
                ht1.Add("2", "分账成功");

                classLibrary.setConfig.FenToYuan_Table(dt, "Fsettle_num", "Fsettle_num_str");

                classLibrary.setConfig.DbtypeToPageContent(dt, "Fstate", "Fstate_str", ht1);

                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else 
            {
                throw new Exception("没有找到订单号"+szListid+"的记录！");
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
