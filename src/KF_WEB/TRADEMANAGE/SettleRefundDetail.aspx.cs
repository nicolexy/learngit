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
	/// SettleRefundDetail 的摘要说明。
	/// </summary>
	public partial class SettleRefundDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }




            try
            {
                string refund_id = Request.QueryString["refund_id"];
                string listid = Request.QueryString["listid"];

                if(refund_id == null || listid == null)
                {
                    throw new Exception("请求参数错误");
                }

                BindInfo(refund_id, listid);
            }
            catch(LogicException err)
            {
                WebUtils.ShowMessage(this.Page,err.Message);
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

        private string getListStatus(int iStatus)
        {
            string szStatus;
            switch (iStatus)
            {
                case 1:
                    szStatus = "处理中";
                    break;
                case 2:
                    szStatus = "成功";
                    break;
                case 3:
                    szStatus = "失败";
                    break;
                default:
                    szStatus = string.Format("未知状态：%d", iStatus);
                    break;

            }
            return szStatus;
        }

        private string getListType(int iType)
        {
            string szType;
            switch (iType)
            {
                case 1:
                    szType = "分账退款";
                    break;
                case 2:
                    szType = "冻结";
                    break;
                case 3:
                    szType = "解冻";
                    break;
                default:
                    szType = string.Format("未知类型：%d", iType);
                    break;

            }
            return szType;
        }

        private void BindInfo(string refund_id, string listid)
        {
            LabelListId.Text = listid;
            labelRefundId.Text = refund_id;

            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds;
            //ds = qs.GetSettleRefundListDetail(refund_id, listid);
            SettleService service = new SettleService();

             DataTable dt = service.GetSettleRefundListDetail(refund_id, listid);
             if (dt == null) throw new Exception("没有查到数据");
 
            
            dt.Columns.Add("uin",typeof(string));
            dt.Columns.Add("status",typeof(string));
            dt.Columns.Add("type",typeof(string));
            dt.Columns.Add("refund_fee",typeof(string));
            dt.Columns.Add("modify_time",typeof(string));
            dt.Columns.Add("bus_args",typeof(string));

            foreach(DataRow dr in dt.Rows)
            {
                dr["uin"] = dr["Fuin"].ToString();
                dr["status"] = getListStatus(int.Parse(dr["Fstate"].ToString()));
                dr["type"] = getListType(int.Parse(dr["Foper_type"].ToString()));
                dr["refund_fee"] = MoneyTransfer.FenToYuan(dr["Famount"].ToString());
                dr["modify_time"] = dr["Fmodify_time"].ToString();
                dr["bus_args"] = dr["Frfd_bus_args"].ToString();
            }
            this.DataGrid1.DataSource = dt.DefaultView;
            this.DataGrid1.DataBind();
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
