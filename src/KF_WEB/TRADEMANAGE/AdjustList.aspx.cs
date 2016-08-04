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
	/// AdjustList 的摘要说明。
	/// </summary>
	public partial class AdjustList : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
                TextBoxBeginDate.Text = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).ToString("yyyy-MM-dd");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
		}

        private void CheckData()
        {
            ViewState["IsrtnList"] = true;
            
            ViewState["queryType"] = 0;
            ViewState["Flistid"] = ""; 
            ViewState["Fspid"] = "";
            ViewState["begindate"] = "";
            ViewState["enddate"] = "";

            if(this.rtnList.Checked)
            {
                if(this.txtFlistid.Text.Trim().Length != 28)
                    throw new Exception("请输入28位订单号！");
                else
                    ViewState["Flistid"] = this.txtFlistid.Text.Trim();

                ViewState["queryType"] = 1;
            }
            else if(this.rtbSpid.Checked)
            {
                ViewState["IsrtnList"] = false;

                DateTime begindate;
                DateTime enddate;
                try
                {
                    begindate = DateTime.Parse(TextBoxBeginDate.Text);
                    enddate = DateTime.Parse(TextBoxEndDate.Text);
                    ViewState["begindate"] = begindate;
                    ViewState["enddate"] = enddate;
                }
                catch
                {
                    throw new Exception("日期输入有误！");
                }

                if(begindate.CompareTo(enddate) > 0)
                {
                    throw new Exception("终止日期小于起始日期，请重新输入！");
                }

                if(begindate.Year != enddate.Year || begindate.Month != enddate.Month)
                {
                    throw new Exception("不允许跨月查询！");
                }

                if(this.txtFspid.Text.Trim() == "")
                    throw new Exception("请输入商户号！");
                else
                    ViewState["Fspid"] = this.txtFspid.Text.Trim();

                ViewState["queryType"] = 3;
            }
            else if(this.rtbSpList.Checked)
            {
                if(this.txtSpList.Text.Trim().Length == 0)
                    throw new Exception("请输入商户订单号！");
                else
                    ViewState["Flistid"] = this.txtSpList.Text.Trim();
                ViewState["queryType"] = 2;
            }
            else
                throw new Exception("请选择一种查询方式！");
        }


        private string getTypeStr(int iType)
        {
            string szType;
            switch(iType)
            {
                case 1:
                    szType = "调入";
                    break;
                case 2:
                    szType = "调出";
                    break;
                default :
                    szType = string.Format("未定义类型：%d", iType);
                    break;
            }
            return szType;
        }

        private string getStatusStr(int iStatus)
        {
            string szStatus;
            switch(iStatus)
            {
                case 1:
                    szStatus = "初始状态";
                    break;
                case 2:
                    szStatus = "待审批";
                    break;
                case 3:
                    szStatus = "处理中";
                    break;
                case 4:
                    szStatus = "成功";
                    break;
                case 5:
                    szStatus = "失败";
                    break;
                default :
                    szStatus = string.Format("未定义类型：%d", iStatus);
                    break;
            }
            return szStatus;
        }

        private void BindData(int index)
        {
            int max = pager.PageSize;
            int start = max * (index-1);


            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds;
            //ds = qs.GetAirAdjustList(int.Parse(ViewState["queryType"].ToString()), 
            //                            ViewState["Flistid"].ToString(), 
            //                            ViewState["Fspid"].ToString(),
            //                            ViewState["begindate"].ToString(),
            //                            ViewState["enddate"].ToString(),
            //                            start,
            //                            max);
            SettleService service = new SettleService();
            DataTable dt = service.GetAirAdjustList(int.Parse(ViewState["queryType"].ToString()),
                                       ViewState["Flistid"].ToString(),
                                      ViewState["Fspid"].ToString(),
                                     ViewState["begindate"].ToString(),
                                     ViewState["enddate"].ToString(),
                                      start,
                                     max);

 
            dt.Columns.Add("listid",typeof(string));
            dt.Columns.Add("spListid",typeof(string));
            dt.Columns.Add("TimeStr",typeof(string));
            dt.Columns.Add("total",typeof(string));
            dt.Columns.Add("type",typeof(string));
            dt.Columns.Add("status",typeof(string));


            foreach(DataRow dr in dt.Rows)
            {
                dr["listid"] = dr["Ftransaction_id"].ToString();
                //dr["spListid"] = dr["Fsp_bill_no"].ToString();
                dr["TimeStr"] = dr["Fadjust_time"].ToString();
                dr["total"] =  MoneyTransfer.FenToYuan(dr["Fnum"].ToString());
                dr["type"] = getTypeStr(int.Parse(dr["Ftype"].ToString()));
                dr["status"] = getStatusStr(int.Parse(dr["Fstatus"].ToString()));

                //对订单号进行隐藏处理 yinhuang
                string spid = dr["Ftransaction_id"].ToString().Substring(0, 10);
                if (!PublicRes.isWhiteOfSeparate(spid, Session["uid"].ToString())) 
                {
                    dr["spListid"] = classLibrary.setConfig.ConvertID(dr["Fsp_bill_no"].ToString(), 0, 4);
                }
                else
                {
                    dr["spListid"] = dr["Fsp_bill_no"].ToString();
                }
            }
            this.DataGrid1.DataSource = dt.DefaultView;
            this.DataGrid1.DataBind();

        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                CheckData();
            }
            catch(Exception err)
            {
                WebUtils.ShowMessage(this.Page,err.Message);
                return;
            }

            try
            {
                pager.RecordCount= 10000;
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
