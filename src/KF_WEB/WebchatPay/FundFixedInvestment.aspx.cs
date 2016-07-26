using CFT.CSOMS.BLL.CFTAccountModule;
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
    public partial class FundFixedInvestment : System.Web.UI.Page
    {
       public string iframeSRC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                pager1.RecordCount = 10000;
                pager2.RecordCount = 10000;
                pager1.PageSize = 5;
                pager2.PageSize = 5;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string uin = this.txt_user.Text.Trim();
                if (string.IsNullOrEmpty(uin))
                {
                    throw new Exception("微信财付通账号不能为空！");
                }
                pager1.CurrentPageIndex = 1;
                pager2.CurrentPageIndex = 1;
                pager2.Visible = false;
                setDagaGrid();
                string USERTYPE = RadioButtonList_SelectValue("USERTYPE");
                uin = AccountService.GetQQID(USERTYPE, uin);
                ViewState["uin"] = uin;
                string uid = new AccountService().QQ2Uid(uin);
                ViewState["uid"] = uid;

#if DEBUG
                ViewState["uid"] = "299708515";
                ViewState["uin"] = "442632198";
#endif

                string PROJECT = RadioButtonList_SelectValue("PROJECT");
                ViewState["PROJECT"] = PROJECT;
                Bind();
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

        private void Bind()
        {
           string PROJECT = ViewState["PROJECT"].ToString();
           int limit = pager1.PageSize;
           int offset = (pager1.CurrentPageIndex - 1) * limit;
            //定投
            if (PROJECT == "DT")
            {
                pager1.Visible = true;
                string uid = ViewState["uid"].ToString();
                FundService service = new FundService();
                DataTable dt = service.Get_DT_fundBuyPlan(uid, offset, limit);
                classLibrary.setConfig.GetColumnValueFromDic(dt, "Fbank_type", "Fbank_type", "BANK_TYPE");
                dg_DT_fundBuyPlan.DataSource = dt;
                dg_DT_fundBuyPlan.DataBind();
            }
            else if (PROJECT == "HFD")
            {
                pager1.Visible = true;
                string uin = ViewState["uin"].ToString();
                FundService service = new FundService();
                DataTable dt = service.Get_HFD_FundFetchPlan(uin, offset, limit);
                classLibrary.setConfig.GetColumnValueFromDic(dt, "Fbank_type", "Fbank_type", "BANK_TYPE");
                dg_HFD_FundFetchPlan.DataSource = dt;
                dg_HFD_FundFetchPlan.DataBind();
            }
            else if (PROJECT == "DreamProject")
            {
                iframeSRC = "FundDreamProject.aspx?uin=" + ViewState["uin"].ToString();
                DataBind();
            }
            else if (PROJECT == "PayBackCredit")
            {
                iframeSRC = "FundPayBackCredit.aspx?uin=" + ViewState["uin"].ToString();
                DataBind();
            }
        }
        public void ChangePage1(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                pager1.CurrentPageIndex = e.NewPageIndex;
                Bind();
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
        public void ChangePage2(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                pager2.CurrentPageIndex = e.NewPageIndex;
                string PROJECT = ViewState["PROJECT"].ToString();
                string plan_id = ViewState["plan_id"].ToString();

                if (PROJECT == "DT")
                {
                    GetPlanBuyOrder(plan_id);
                }
                else if (PROJECT == "HFD")
                {
                    GetPlanFetchOrder(plan_id);
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
        #region 定投
        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                string plan_id = e.Item.Cells[0].Text.Trim(); //ID 
                ViewState["plan_id"] = plan_id;
                if (e.CommandName == "other")
                {
                    GetfundBuyPlanByPlanid(plan_id);
                }
                else if (e.CommandName == "KKrecord")
                {
                    //pager2.Visible = true;
                    GetPlanBuyOrder(plan_id);
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

        private void GetfundBuyPlanByPlanid(string plan_id)
        {
            string uid = ViewState["uid"].ToString();
            FundService service = new FundService();
            DataTable dt = service.Get_DT_fundBuyPlanByPlanid(uid, plan_id);
            dg_fundBuyPlanByPlanid.DataSource = dt;
            dg_fundBuyPlanByPlanid.DataBind();
        }
        private void GetPlanBuyOrder(string plan_id)
        {
            int limit = pager2.PageSize;
            int offset = (pager2.CurrentPageIndex - 1) * limit;
            string uid = ViewState["uid"].ToString();
            FundService service = new FundService();
            DataTable dt = service.Get_DT_PlanBuyOrder(uid, plan_id, offset, limit);
            classLibrary.setConfig.GetColumnValueFromDic(dt, "Fbank_type", "Fbank_type", "BANK_TYPE");
            if (dt != null && dt.Rows.Count > 0) 
            {
                pager2.Visible = true;
            }

            dg_PlanBuyOrder.DataSource = dt;
            dg_PlanBuyOrder.DataBind();
        }
      
        #endregion

        #region 定赎
        protected void DataGrid4_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                string plan_id = e.Item.Cells[0].Text.Trim(); //ID
                ViewState["plan_id"] = plan_id;
                if (e.CommandName == "other")
                {
                    GetFundFetchPlan(plan_id);
                }
                else if (e.CommandName == "KKrecord")
                {
                    //pager2.Visible = true;
                    GetPlanFetchOrder(plan_id);
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

        private void GetFundFetchPlan(string plan_id)
        {
            string uin = ViewState["uin"].ToString();
            FundService service = new FundService();
            DataTable dt = service.Get_HFD_FundFetchPlanByPlanid(uin, plan_id);
            dg_FundFetchPlanByPlanid.DataSource = dt;
            dg_FundFetchPlanByPlanid.DataBind();
        }
        private void GetPlanFetchOrder(string plan_id)
        {
            int limit = pager2.PageSize;
            int offset = (pager2.CurrentPageIndex - 1) * limit;
            string uin = ViewState["uin"].ToString();
            FundService service = new FundService();
            DataTable dt = service.Get_HFD_PlanFetchOrder(uin, plan_id, offset, limit);
            classLibrary.setConfig.GetColumnValueFromDic(dt, "Fbank_type", "Fbank_type", "BANK_TYPE");
            if (dt != null && dt.Rows.Count > 0)
            {
                pager2.Visible = true;
            }
            dg_PlanFetchOrder.DataSource = dt;
            dg_PlanFetchOrder.DataBind();
        }
        #endregion

        private void setDagaGrid()
        {
            dg_DT_fundBuyPlan.DataSource = null;
            dg_fundBuyPlanByPlanid.DataSource = null;
            dg_PlanBuyOrder.DataSource = null;
            dg_DT_fundBuyPlan.DataBind();
            dg_fundBuyPlanByPlanid.DataBind();
            dg_PlanBuyOrder.DataBind();

            dg_HFD_FundFetchPlan.DataSource = null;
            dg_FundFetchPlanByPlanid.DataSource = null;
            dg_PlanFetchOrder.DataSource = null;
            dg_HFD_FundFetchPlan.DataBind();
            dg_FundFetchPlanByPlanid.DataBind();
            dg_PlanFetchOrder.DataBind();
        }

        private string RadioButtonList_SelectValue(string GroupName)
        {
            string value = "";
            foreach (object control in this.formMain.Controls)
            {
                if (control.GetType() == typeof(RadioButton))
                {
                    var radio = control as RadioButton;
                    if (radio.GroupName == GroupName && radio.Checked == true)
                    {
                        value = radio.ID;
                    }
                }
            }
            return value;
        }

        protected void dg_PlanFetchOrder_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            ////spid1:fee1&spid2:fee2;
            //string Fredeem_info = e.Item.Cells[13].Text;
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //foreach (var item in Fredeem_info.Split('&'))
            //{
            //    string[] c = item.Split(':');
            //    if (c.Length == 2)
            //    {
            //        dic.Add(c[0], c[1]);
            //    }
            //}

            //object obj = e.Item.Cells[13].FindControl("tb_Fredeem_info");
            //if (obj != null)
            //{
            //    Table tb_info = obj as Table;
                
            //    foreach (string item in dic.Keys) 
            //    {
            //        TableRow tr = new TableRow();
            //        tr.Cells.Add(new TableCell());
            //    }

            //}
        }
    }
}