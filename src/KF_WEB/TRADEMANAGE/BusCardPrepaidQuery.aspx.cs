using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CFT.CSOMS.BLL.TradeModule;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    public partial class BusCardPrepaidQuery : System.Web.UI.Page
    {
        static DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.textBoxEndDate.Text = DateTime.Today.ToShortDateString();
                this.textBoxBeginDate.Text = DateTime.Today.AddMonths(-1).ToShortDateString();
            }
        }
        protected void buttonQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxBeginDate.Text) || string.IsNullOrEmpty(textBoxEndDate.Text))
            {
                WebUtils.ShowMessage(this.Page, "请输入查询起止日期");
                return;
            }
            string uin = textBoxAccountID.Text.Trim();
            if (string.IsNullOrEmpty(uin))
            {
                WebUtils.ShowMessage(this.Page, "必须输入财付通账号");
                return;
            }
            string listid = textBoxOrderID.Text.Trim();
            string cardid = textBoxCardNum.Text.Trim();
            string beginDate = DateTime.Parse(textBoxBeginDate.Text).ToString("yyyyMMdd");
            string endDate = DateTime.Parse(textBoxEndDate.Text).ToString("yyyyMMdd");
            string errMsg = "";
            DataSet ds = new TradeService().QueryBusCardPrepaid(beginDate,endDate,80,uin,listid,cardid,out errMsg);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows != null)
            {
                dt = ds.Tables[0];
                this.gridViewQueryResult.DataSource = dt;
                this.gridViewQueryResult.DataBind();
            }
            else
            {
                WebUtils.ShowMessage(this.Page, errMsg);
                return;
            }
        }
        protected void gridViewQueryResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewQueryResult.DataSource = dt;
            gridViewQueryResult.PageIndex = e.NewPageIndex;
            gridViewQueryResult.DataBind();
        }
    }
}