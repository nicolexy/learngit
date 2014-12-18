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


namespace TENCENT.OSS.CFT.KF.KF_Web.settleRefund
{
	/// <summary>
	/// settleRefund 的摘要说明。
	/// </summary>
	public partial class settleRefund : System.Web.UI.Page
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

            if(!IsPostBack)
            {
                this.rtnList.Checked = true;
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.pager_PageChanged);

        }
		#endregion


        public void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);        
        }

        private string getRefundStatusStr(int iStatus, int iFreezeFstatus)
        {
            string strStatus;
            switch(iStatus)
            {
                case 4:
                    strStatus="退款成功";
                    break;
                case 5:
                    strStatus="分账回退失败";
                    break;
                case 6:
                    strStatus="垫付退款成功";
                    break;
                case 12:
                    strStatus="分账回退中";
                    break;
                case 13:
                    strStatus="分帐回退成功";
                    break;
                case 14:
                    strStatus="分账回退成功，b2c退款中";
                    break;
                case 15:
                    strStatus="b2c退款成功，分账回退中";
                    break;
                case 16:
                    strStatus="垫付退款中";
                    break;
                case 17:
                    strStatus="垫付退款成功，分账回退失败";
                    break;
                case 18:
                    strStatus="分账回退成功，b2c退款失败";
                    break;
                case 19:
                    strStatus="垫付退款失败";
                    break;
                case 20:
                switch(iFreezeFstatus)
                {
                    case 0:
                        strStatus="初始状态";
                        break;
                    case 1:
                        strStatus="冻结申请中";
                        break;
                    case 2:
                        strStatus="冻结成功";
                        break;
                    case 3:
                        strStatus="冻结失败";
                        break;
                    case 4:
                        strStatus="解冻处理中";
                        break;
                    case 5:
                        strStatus="解冻成功";
                        break;
                    default:
                        strStatus = string.Format("冻结解冻处理状态：%d", iFreezeFstatus);
                        break;
                }
                    break;
                default:
                    strStatus = string.Format("%d", iStatus);
                    break;

            }
            return strStatus;
        }

        private void BindData(int index)
        {
            int max = pager.PageSize;
            int start = max * (index-1);

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds;
            ds = qs.GetSettleRefundList(ViewState["Flistid"].ToString(), int.Parse(ViewState["query_type"].ToString()), start, max);
            
            DataTable dt = ds.Tables[0];

            
            dt.Columns.Add("listid",typeof(string));
            dt.Columns.Add("refund_id",typeof(string));
            dt.Columns.Add("modify_time",typeof(string));
            dt.Columns.Add("rp_fee",typeof(string));
            dt.Columns.Add("status",typeof(string));


            foreach(DataRow dr in dt.Rows)
            {
                dr["listid"] = dr["Ftransaction_id"].ToString();
                dr["refund_id"] = dr["Fdraw_id"].ToString();
                dr["modify_time"] = dr["Fmodify_time"].ToString();
                dr["rp_fee"] = MoneyTransfer.FenToYuan(dr["Frp_fee"].ToString());
                dr["status"] = getRefundStatusStr(int.Parse(dr["Fstatus"].ToString()), int.Parse(dr["Ffreeze_state"].ToString()));

            }
            this.DataGrid1.DataSource = dt.DefaultView;
            this.DataGrid1.DataBind();
        }

        private void checkData()
        {
            if(this.rtnList.Checked)
            {
                if(this.txtFlistid.Text.Trim().Length != 28)
                    throw new Exception("请输入28位订单号！");
                else
                    ViewState["Flistid"] = this.txtFlistid.Text.Trim();

                //根据订单查询
                ViewState["query_type"] = 1;
            }
            else if(this.rtnRefefundList.Checked)
            {
                if(this.txtRefefundList.Text.Trim().Length != 28)
                    throw new Exception("请输入28位退款申请单号或冻结单号！");
                else
                    ViewState["Flistid"] = this.txtRefefundList.Text.Trim();

                //根据退款单查询
                ViewState["query_type"] = 2;
            }
            else
            {
                throw new Exception("未知异常");
            }
        }


        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                pager.RecordCount= 10000;
                checkData();
                BindData(1);
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
	}
}
