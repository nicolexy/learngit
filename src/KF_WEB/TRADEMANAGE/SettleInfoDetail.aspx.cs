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

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SettleInfoDetail 的摘要说明。
	/// </summary>
	public partial class SettleInfoDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            // 在此处放置用户代码以初始化页面
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            string listid = Request.QueryString["listid"];
            if (listid != null && !string.IsNullOrEmpty(listid.Trim())) 
            {
                try
                {
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

        private string getRole(int iRole)
        {
            string szRole;
            switch(iRole)
            {
                case 1:
                    szRole = "供应商";
                    break;
                case 2:
                    szRole = "平台方";
                    break;
                case 3:
                    szRole = "财付通";
                    break;
                case 4:
                    szRole = "独立分润方";
                    break;
                case 5:
                    szRole = "销售商";
                    break;
                default :
                    szRole = string.Format("未知角色：{0}", iRole);
                    break;
            }
            return szRole;
        }

        private string getStatus(int iStatus)
        {
            string szStatus;
            switch(iStatus)
            {
                case 0:
                    szStatus = "未分账";
                    break;
                case 2:
                    szStatus = "已分账";
                    break;
                case 6:
                    szStatus = "已经分账回退";
                    break;
                default :
                    szStatus = string.Format("未知状态：{0}", iStatus);
                    break;
            }
            return szStatus;
        }
        private void BindInfo(string szListid)
        {
            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds;
            //ds = qs.GetSettleInfoListDetail(szListid);

            SettleService service = new SettleService();
            DataTable  dt = service.GetSettleInfoListDetail(szListid);

            if (dt != null  )
            {
                dt.Columns.Add("uin", typeof(string));
                dt.Columns.Add("role", typeof(string));
                dt.Columns.Add("status", typeof(string));
                dt.Columns.Add("settle_in", typeof(string));
                dt.Columns.Add("actual_in", typeof(string));
                dt.Columns.Add("refundfee", typeof(string));
                dt.Columns.Add("freeze", typeof(string));
                dt.Columns.Add("modify_time", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    dr["uin"] = dr["Factor"].ToString();
                    dr["role"] = getRole(int.Parse(dr["Frole"].ToString()));
                    dr["status"] = getStatus(int.Parse(dr["Fstate"].ToString()));
                    dr["settle_in"] = MoneyTransfer.FenToYuan(dr["Fpre_fee"].ToString());
                    dr["actual_in"] = MoneyTransfer.FenToYuan(dr["Fact_fee"].ToString());
                    dr["refundfee"] = MoneyTransfer.FenToYuan(dr["Frefund_fee"].ToString());
                    dr["freeze"] = MoneyTransfer.FenToYuan(dr["Ffreeze_fee"].ToString());
                    dr["modify_time"] = dr["Fmodify_time"].ToString();
                }
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
