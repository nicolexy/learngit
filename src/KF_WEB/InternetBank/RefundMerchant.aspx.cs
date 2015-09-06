using CFT.CSOMS.BLL.InternetBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
    public partial class RefundMerchant : System.Web.UI.Page
    {
        protected static bool IsAdd = false;
        protected static int Old_Frefund_id = 0;
        protected static int temp_fid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lab_operator.Text = Session["uid"].ToString();

                if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
                    Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            if (!IsPostBack)
            {
                this.tbx_beginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.tbx_endDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btn_query_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }
            BindData(1);
        }

        private void ValidateDate()
        {
            DateTime begindate;
            DateTime enddate;

            try
            {
                begindate = DateTime.Parse(tbx_beginDate.Text);
                enddate = DateTime.Parse(tbx_endDate.Text);
            }
            catch
            {
                throw new Exception("日期输入有误！");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("终止日期小于起始日期，请重新输入！");
            }

        }

        private void BindData(int index)
        {
            int Frefund_id = 0;
            int.TryParse(this.tbx_query_refund.Text.Trim(), out Frefund_id);
            string sTime = DateTime.Parse(tbx_beginDate.Text).ToString("yyyy-MM-dd 00:00:00");
            string eTime = DateTime.Parse(tbx_endDate.Text).ToString("yyyy-MM-dd 23:59:59");


            pager.RecordCount = new InternetBankService().GetRefundCount(Frefund_id, sTime, eTime);
            pager.CurrentPageIndex = index;


            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new InternetBankService().GetRefundByFrefundId(Frefund_id, sTime, eTime,start,max);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
            }
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            IsAdd = true;
            this.table_action.Visible = true;
            this.lab_action_title.Text = "新增商户";
            this.tbx_refund_number.Text = "";
            this.tbx_refund_name.Text = "";
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            int Frefund_id = 0;
            int.TryParse(this.tbx_refund_number.Text.Trim(), out Frefund_id);
            string Frefund_name = this.tbx_refund_name.Text.Trim();
            string Fcreate_by = this.lab_operator.Text;
            if (Frefund_id == 0 || string.IsNullOrEmpty(Frefund_name))
            {
                WebUtils.ShowMessage(this.Page, "商户号码+商户名称不能为空！");
                return;
            }
            if (IsAdd)
            {
                int flag = new InternetBankService().AddRefundList(Frefund_id, Frefund_name, Fcreate_by);
                switch (flag)
                {
                    case -1:
                        WebUtils.ShowMessage(this.Page, "退款商户录入失败！");
                        break;
                    case 0:
                        WebUtils.ShowMessage(this.Page, "退款商户录入成功。");
                        BindData(1);
                        break;
                    case 1:
                        WebUtils.ShowMessage(this.Page, Frefund_id + "商户号码已存在！");
                        break;
                }
            }
            else
            {
                int flag = new InternetBankService().EditRefundByFid(Frefund_id,Old_Frefund_id, Frefund_name, Fcreate_by, temp_fid);
                switch (flag)
                {
                    case -1:
                        WebUtils.ShowMessage(this.Page, "修改退款商户信息失败！");
                        break;
                    case 0:
                        WebUtils.ShowMessage(this.Page, "修改退款商户信息成功。");
                        this.table_action.Visible = false;
                        BindData(1);
                        break;
                    case 1:
                        WebUtils.ShowMessage(this.Page, Frefund_id + "商户号码已存在！");
                        break;
                }                    
            }
        }

        protected void btn_back_Click(object sender, EventArgs e)
        {
            this.table_action.Visible = false;
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int fid = Convert.ToInt32(e.Item.Cells[0].Text.Trim()); //ID
            int Frefund_id = int.Parse(e.Item.Cells[1].Text);
            string Frefund_name = e.Item.Cells[2].Text;

            switch (e.CommandName)
            {
                case "CHANGE": //编辑
                    temp_fid = fid;
                    EditRefund(Frefund_id, Frefund_name);
                    Old_Frefund_id = Frefund_id;
                    IsAdd = false;
                    break;
                case "DEL": //删除
                    DelRefund(fid);
                    break;
            }
        }

        private void DelRefund(int fid)
        {
            if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
            {
                //权限判断
                WebUtils.ShowMessage(this.Page, "没有权限！");
                return;
            }
            try
            {
                bool flag = new InternetBankService().DelRefundByFid(fid);
                if (flag)
                {
                    WebUtils.ShowMessage(this.Page, "删除退款商户信息成功。");
                    BindData(1);
                }
                else
                    WebUtils.ShowMessage(this.Page, "删除退款商户信息失败！");
            }
            catch (Exception e)
            {
                WebUtils.ShowMessage(this.Page, e.Message);
            }
        }

        private void EditRefund(int Frefund_id, string Frefund_name)
        {
            if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
            {
                //权限判断
                WebUtils.ShowMessage(this.Page, "没有权限！");
                return;
            }
            this.table_action.Visible = true;
            this.tbx_refund_number.Text = Frefund_id.ToString();
            this.tbx_refund_name.Text = Frefund_name;
        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[6].FindControl("lbChange");
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                lb.Attributes["onClick"] = "return confirm('确定要执行“修改”操作吗？');";
            }
            object obj2 = e.Item.Cells[6].FindControl("lbDel");
            if (obj2 != null)
            {
                LinkButton lb2 = (LinkButton)obj2;
                lb2.Attributes["onClick"] = "return confirm('确定要执行“删除”操作吗？');";
            }
        }
    }
}