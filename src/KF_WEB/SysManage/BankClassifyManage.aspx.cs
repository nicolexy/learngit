using CFT.CSOMS.BLL.SysManageModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.SysManage
{
    public partial class BankClassifyManage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected static bool IsAdd = false;
        Dictionary<string, string> BankBusinessType = BankClassifyService.BankBusinessType;
        Dictionary<string, string> BankUseStatus = BankClassifyService.BankUseStatus;
        Dictionary<string, string> BankCardType = BankClassifyService.BankCardType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classLibrary.ClassLib.ValidateRight("BankClassifyInfo", this))
                Response.Redirect("../login.aspx?wh=1");

            if (!IsPostBack)
            {
                int totalNum = 0;
                DataSet ds = new BankClassifyService().QueryBankBusiInfo(1, "", "", 0, 0, 0, 1000, out totalNum);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = ds.Tables[0].DefaultView;
                        dv.Sort = "bank_name ASC";
                        ds.Tables.RemoveAt(0);
                        ds.Tables.Add(dv.ToTable());
                        this.ddl_bankname.DataSource = ds.Tables[0];
                        this.ddl_au_bankname.DataSource = ds.Tables[0];
                    }
                }

                this.ddl_bankname.DataTextField = "bank_name";
                this.ddl_bankname.DataValueField = "bank_code";
                this.ddl_bankname.DataBind();
                this.ddl_bankname.Items.Insert(0, new ListItem("请选择", ""));

                PublicRes.GetDllDownList(ddl_busi_type, BankBusinessType, "请选择", "0");
                PublicRes.GetDllDownList(ddl_use_status, BankUseStatus, "请选择", "0");
                ddl_use_status.SelectedValue = "2"; //接口 默认查询2-正在使用 

                #region

                this.ddl_au_bankname.DataTextField = "bank_name";
                this.ddl_au_bankname.DataValueField = "bank_code";
                this.ddl_au_bankname.DataBind();
                this.ddl_au_bankname.Items.Insert(0, new ListItem("请选择", ""));
                
                PublicRes.GetDllDownList(ddl_au_busi_type, BankBusinessType, "请选择", "0");
                PublicRes.GetDllDownList(ddl_au_card_type, BankCardType, "请选择", "0");
                PublicRes.GetDllDownList(ddl_au_use_status, BankUseStatus, "请选择", "0");

                #endregion
            }
        }

        protected void btn_query_Click(object sender, EventArgs e)
        {
            BindData(1);
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            if (!authentication()) return;
            IsAdd = true;
            this.table_action.Visible = true;
            this.tbx_au_bank_type.Enabled = true;
            this.ddl_au_busi_type.Enabled = true;
            this.lab_action_title.Text = "银行分类信息新增";
            this.tbx_au_bank_type.Text = "";
            this.ddl_au_bankname.SelectedIndex = 0;
            this.ddl_au_busi_type.SelectedIndex = 0;
            this.ddl_au_card_type.SelectedIndex = 0;
            this.ddl_au_use_status.SelectedIndex = 0;
        }

        //增、改
        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string bank_type = this.tbx_au_bank_type.Text;
            string bank_name = this.ddl_au_bankname.SelectedItem.Text;
            string bank_code = this.ddl_au_bankname.SelectedValue;
            int busi_type = Convert.ToInt32(this.ddl_au_busi_type.SelectedValue);
            int card_type = Convert.ToInt32(this.ddl_au_card_type.SelectedValue);
            int use_status = Convert.ToInt32(this.ddl_au_use_status.SelectedValue);

            bool flag = false;
            if (IsAdd)
            {
                flag  = new BankClassifyService().OpBankBusiInfo(1, bank_type, busi_type, bank_code, bank_name, card_type, use_status);
            }
            else
            {
                flag = new BankClassifyService().OpBankBusiInfo(3, bank_type, busi_type, bank_code, bank_name, card_type, use_status);
            }
            if (flag)
            {
                refurbish_binddata();
                this.table_action.Visible = false;
                WebUtils.ShowMessage(this.Page, "银行分类信息" + (IsAdd ? "新增" : "修改") + "成功。");
            }
            else
                WebUtils.ShowMessage(this.Page, "银行分类信息" + (IsAdd ? "新增" : "修改") + "失败。");
        }

        protected void refurbish_binddata()
        {
            string bank_code = ddl_bankname.SelectedValue;
            //string bank_code = this.txt_bankcode.Value;
            int business_type = Convert.ToInt32(ddl_busi_type.SelectedValue);
            if (!string.IsNullOrEmpty(bank_code) || business_type > 0)
                BindData(pager.CurrentPageIndex);
        }

        protected void btn_back_Click(object sender, EventArgs e)
        {
            this.table_action.Visible = false;
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        private void BindData(int index)
        {
            //string bank_code = this.txt_bankcode.Value;
            string bank_code = ddl_bankname.SelectedValue;
            int business_type = Convert.ToInt32(ddl_busi_type.SelectedValue);
            int use_status = Convert.ToInt32(ddl_use_status.SelectedValue);
            if (string.IsNullOrEmpty(bank_code) && business_type <= 0)
            {
                WebUtils.ShowMessage(this.Page, "开户行名和银行业务类型，至少选择一项！");
                return;
            }

            int totalNum = 0;
            pager.CurrentPageIndex = index;

            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new BankClassifyService().QueryBankBusiInfo(0, bank_code, "", business_type, use_status, start, max, out totalNum);

            pager.RecordCount = totalNum;

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

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
            {
                if (BankBusinessType !=null && BankBusinessType.ContainsKey(e.Item.Cells[4].Text))
                    e.Item.Cells[4].Text = BankBusinessType[e.Item.Cells[4].Text];

                if (BankCardType!=null && BankCardType.ContainsKey(e.Item.Cells[5].Text))
                    e.Item.Cells[5].Text = BankCardType[e.Item.Cells[5].Text];

                if (BankUseStatus != null && BankUseStatus.ContainsKey(e.Item.Cells[6].Text))
                    e.Item.Cells[6].Text = BankUseStatus[e.Item.Cells[6].Text];
            }

            object objDel = e.Item.Cells[10].FindControl("lbDel");
            if (objDel != null)
            {
                LinkButton lb2 = (LinkButton)objDel;
                lb2.Attributes["onClick"] = "return confirm('你确定要删除该条银行分类信息吗？删除后将无法恢复！');";
            }
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string bank_code = e.Item.Cells[0].Text;
            string bank_type = e.Item.Cells[1].Text;
            int business_type = int.Parse(e.Item.Cells[7].Text);
            int card_type = int.Parse(e.Item.Cells[8].Text);
            int use_status = int.Parse(e.Item.Cells[9].Text);

            switch (e.CommandName)
            {
                case "CHANGE": //编辑
                    if (!authentication()) return;
                    this.table_action.Visible = true;
                    this.lab_action_title.Text = "银行分类信息修改";

                    this.tbx_au_bank_type.Enabled = false;
                    this.ddl_au_busi_type.Enabled = false;

                    this.tbx_au_bank_type.Text = bank_type;
                    this.ddl_au_bankname.SelectedValue = bank_code;
                    this.ddl_au_busi_type.SelectedValue = business_type.ToString();
                    this.ddl_au_card_type.SelectedValue = card_type.ToString();
                    this.ddl_au_use_status.SelectedValue = use_status.ToString();
                    IsAdd = false;
                    break;
                case "DEL": //删除
                    if (!authentication()) return;
                    bool flag = new BankClassifyService().OpBankBusiInfo(2, bank_type, business_type, "", "", 0, 0);
                    if (flag)
                        refurbish_binddata();
                    else
                        WebUtils.ShowMessage(this.Page, "银行分类信息删除失败。");
                    break;
            }
        }

        protected bool authentication()
        {
            bool result = false;
            if (!classLibrary.ClassLib.ValidateRight("BankClassifyInfo", this))
            {
                WebUtils.ShowMessage(this.Page, "没有权限！");
            }
            else
                result = true;
            return result;
        }


        protected void btn_input_excel_Click(object sender, EventArgs e)
        {
            if (!authentication()) return;
            try
            {
                int succ = 0, fail = 0;
                string inputFail = string.Empty;

                string path = Server.MapPath("~/") + "PLFile" + "\\refund.xls";
                File1.PostedFile.SaveAs(path);
                DataSet res_ds = PublicRes.readXls(path);
                DataTable res_dt = res_ds.Tables[0];

                int iColums = res_dt.Columns.Count;
                int iRows = res_dt.Rows.Count;
                for (int i = 0; i < res_dt.Rows.Count; i++)
                {
                    string bank_type = res_dt.Rows[i][0].ToString();
                    int busi_type = Convert.ToInt32(res_dt.Rows[i][1]);
                    string bank_code = res_dt.Rows[i][2].ToString();
                    string bank_name = res_dt.Rows[i][3].ToString();
                    int card_type = Convert.ToInt32(res_dt.Rows[i][4]);
                    int use_status = Convert.ToInt32(res_dt.Rows[i][5]);

                    bool flag = new BankClassifyService().OpBankBusiInfo(1, bank_type, busi_type, bank_code, bank_name, card_type, use_status);
                    if (flag)
                        succ++;
                    else
                    {
                        inputFail += string.Format("{0},{1},{2},{3},{4},{5}；", bank_type, busi_type, bank_code, bank_name, card_type, use_status);
                        fail++;
                    }
                }
                WebUtils.ShowMessage(this.Page,
                    string.Format("银行分类信息导入;总数：{0};成功数：{1};失败数：{2}{3}", 
                    iRows, succ, fail, (fail > 0 ? ";失败数据：" + inputFail : "")));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "银行分类信息异常：" + ex.Message);
            }
        }
    }
}