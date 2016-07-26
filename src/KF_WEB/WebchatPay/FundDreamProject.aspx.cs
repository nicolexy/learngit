using CFT.CSOMS.BLL.FundModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class FundDreamProject : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    AspNetPager1.RecordCount = 10000;
                    AspNetPager1.PageSize = 5;

                    AspNetPager2.RecordCount = 10000;
                    AspNetPager2.PageSize = 5;
                   
                    AspNetPager3.RecordCount = 10000;
                    AspNetPager3.PageSize = 5;

                    AspNetPager1.Visible = false;
                    AspNetPager2.Visible = false;
                    AspNetPager3.Visible = false;

                    string uin = Request.QueryString["uin"];
                    ViewState["uin"] = uin;
                    DreamProject_Plan();
                } 
                
            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }
        private void DreamProject_Plan()
        {
            AspNetPager1.Visible = false;
            dg1.Caption = "梦想计划（计划列表）";
            dg1.DataSource = null;
            dg1.DataBind();


            int limit = AspNetPager1.PageSize;
            int offset = (AspNetPager1.CurrentPageIndex - 1) * limit;
            string uin = ViewState["uin"].ToString();
            DataTable dt = new FundService().Get_DreamProject_Plan(uin, offset, limit);
            AspNetPager1.Visible = true;
            dg1.DataSource = dt;
            dg1.DataBind();
        }

        /// <summary>
        /// 梦想交易单列表
        /// </summary>
        private void DreamProject_Trans(string plan_id)
        {
            AspNetPager2.Visible = false;
            DataGrid2.Caption = "梦想计划（交易单列表）";
            DataGrid2.DataSource = null;
            DataGrid2.DataBind();


            int limit = AspNetPager2.PageSize;
            int offset = (AspNetPager2.CurrentPageIndex - 1) * limit;
            string uin = ViewState["uin"].ToString();
            string tradeId = new FundService().GetTradeIdByUIN(uin);
            if (string.IsNullOrEmpty(tradeId))
                throw new Exception("查询不到用户的TradeId，请确认当前用户是否有注册基金账户");
            DataTable dt = new FundService().Get_DreamProject_trans(plan_id, tradeId, offset, limit);
            AspNetPager2.Visible = true;
            DataGrid2.DataSource = dt;
            DataGrid2.DataBind();
        }

        /// <summary>
        /// 资产列表
        /// </summary>
        private void DreamProject_asset(string plan_id)
        {
            AspNetPager3.Visible = false;
            DataGrid3.Caption = "梦想计划（资产详情）";
            DataGrid3.DataSource = null;
            DataGrid3.DataBind();


            int limit = AspNetPager2.PageSize;
            int offset = (AspNetPager2.CurrentPageIndex - 1) * limit;
            string uin = ViewState["uin"].ToString();
            string tradeId = new FundService().GetTradeIdByUIN(uin);
            if (string.IsNullOrEmpty(tradeId))
                throw new Exception("查询不到用户的TradeId，请确认当前用户是否有注册基金账户");
#if DEBUG
            plan_id = "201603300000140677";
            tradeId = "201509140369301000";
#endif
            DataTable dt = new FundService().Get_DreamProject_asset(plan_id, tradeId, offset, limit);
            AspNetPager3.Visible = true;
            DataGrid3.DataSource = dt;
            DataGrid3.DataBind();
        }

        protected void dg1_ItemCommand1(object source, DataGridCommandEventArgs e)
        {
            try
            {
                string plan_id = e.Item.Cells[0].Text.Trim(); //ID 

                if (e.CommandName == "Command_trans")
                {
                    DreamProject_Trans(plan_id);
                }
                else 
                {
                    DreamProject_asset(plan_id);
                }
            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }

        public void OnChangePage1(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                AspNetPager1.CurrentPageIndex = e.NewPageIndex;
                DreamProject_Plan();

            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }
        public void OnChangePage2(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                AspNetPager1.CurrentPageIndex = e.NewPageIndex;
                DreamProject_Plan();

            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }
        public void OnChangePage3(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                AspNetPager1.CurrentPageIndex = e.NewPageIndex;
                DreamProject_Plan();

            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }
    }
}