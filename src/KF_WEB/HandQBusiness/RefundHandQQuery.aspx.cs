using System;
using System.Data;
using System.Linq;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.COMMLIB;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.HandQModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.HandQBusiness
{
    public partial class RefundHandQQuery : System.Web.UI.Page
    {
        private const int m_nPageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                textBoxBeginDate.Text = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-01 00:00:00");
                textBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                this.DatagridList.Visible = false;
                divInfo.Visible = false;
            }
           
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateDate();
                this.pager.RecordCount = 1000;
                string type = ddlType.SelectedValue;
                this.DatagridList.Visible = false;
                divInfo.Visible = false;
                if (type == "1" || type == "2")
                {
                    BindData(1);
                }
                else
                {
                    BindDetail(txtInput.Text.Trim(), ddlType.SelectedValue);
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void ValidateDate()
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                showMsg("输入不能为空");
                return;
            }

            string type=ddlType.SelectedValue;
            if (type == "1" || type == "2")
            {
                DateTime begindate;
                DateTime enddate;

                try
                {
                    begindate = DateTime.Parse(textBoxBeginDate.Text);
                    enddate = DateTime.Parse(textBoxEndDate.Text);
                }
                catch
                {
                    throw new Exception("日期输入有误！");
                }

                if (begindate.CompareTo(enddate) > 0)
                {
                    throw new Exception("终止日期小于起始日期，请重新输入！");
                }

                if (begindate.AddMonths(2) > enddate)
                {
                    throw new Exception("日期间隔大于3个月份，请重新输入！");
                }
                ViewState["begindate"] =begindate.ToString("yyyy-MM-dd 00:00:00");
                ViewState["enddate"] = enddate.ToString("yyyy-MM-dd 23:59:59");
            }

        }

        private void BindData(int index)
        {
            try
            {
                divInfo.Visible = false;
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);

                DataSet ds = new HandQService().RefundHandQQuery(txtInput.Text.Trim(), ddlType.SelectedValue,
                    ViewState["begindate"].ToString(), ViewState["enddate"].ToString(),start,max);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DatagridList.DataSource = ds.Tables[0].DefaultView;
                    DatagridList.DataBind();
                    this.DatagridList.Visible = true;
                  
                }
                else
                {
                    DatagridList.DataSource = null;
                    DatagridList.DataBind();
                    throw new Exception("没有找到记录！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(PublicRes.GetErrorMsg(ex.Message.ToString()));
            }
        }

        public void BindDetail(string acc,string type)
        {
            try
            {
                Clear();
                divInfo.Visible = true;
                DataSet ds = new HandQService().RefundHandQDetailQuery(acc.Trim(), type);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    this.lbopenid.Text = dr["openid"].ToString();
                    this.lbcard_id.Text = dr["card_id"].ToString();
                    this.lbwx_fetch_no.Text = dr["wx_fetch_no"].ToString();
                    this.lbcard_name.Text = dr["card_name"].ToString();
                    this.lbnum.Text = dr["num_str"].ToString();
                    this.lbbank_name.Text = dr["bank_name"].ToString();
                    this.lbcreate_time.Text = dr["create_time"].ToString();
                    this.lbstate.Text = dr["state_str"].ToString();
                    this.lbmodify_time.Text = dr["modify_time"].ToString();
                    this.lbcft_fetch_no.Text = dr["cft_fetch_no"].ToString();
                    this.lbtrade_no.Text = dr["trade_no"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(PublicRes.GetErrorMsg(ex.Message.ToString()));
            }
        }

        protected void gvReceiveHQList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    var commandArgs = e.CommandArgument.ToString();

                    if (commandArgs=="")
                    {
                        throw new Exception("详情参数错误");
                    }
                    BindDetail(commandArgs,"4");
                }
                
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        public void Clear(){
            this.lbopenid.Text = "";
            this.lbcard_id.Text = "";
            this.lbwx_fetch_no.Text = "";
            this.lbcard_name.Text = "";
            this.lbnum.Text = "";
            this.lbbank_name.Text = "";
            this.lbcreate_time.Text = "";
            this.lbstate.Text = "";
            this.lbmodify_time.Text = "";
            this.lbcft_fetch_no.Text = "";
            this.lbtrade_no.Text = "";
        }

        protected void PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                int currentPage = e.NewPageIndex;
                BindData(currentPage);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void showMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }
    }
}