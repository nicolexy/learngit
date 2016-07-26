using CFT.CSOMS.BLL.SPOA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
    public partial class CertBlackList : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        MerchantService merchantService = new MerchantService();
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            this.pager.PageSize = 10;
            this.pager.RecordCount = 100000;
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Bind(1);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "查询失败：" + ex.Message);
            }
        }

        private void Bind(int pageindex)
        {
            string spid = txtSpid.Text;
            string SDate = txtStartDate.Text;
            string edate = txtEndDate.Text;
            int start = (pageindex - 1) * pager.PageSize;
            DataSet ds = merchantService.QueryCertNoticeBlackList(spid, SDate, edate, start, pager.PageSize);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataGrid1.DataSource = dt.DefaultView;
                DataGrid1.DataBind();
            }
        }
        private void Delete(string fid)
        {
            merchantService.DeleteCertNoticeBlackList(fid, Session["uid"].ToString());
            Bind(pager.CurrentPageIndex);
        }
        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                pager.CurrentPageIndex = e.NewPageIndex;
                Bind(pager.CurrentPageIndex);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string spid = txtSpid_add.Text.Trim();
                if (spid == "") 
                {
                    throw new Exception("请输入商户号！");
                }
                string memo = txtFMemo.Text.Trim();
                SPOAService spoaservice = new SPOAService();
                DataSet ds = spoaservice.GetOneValueAddedTax(spid);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
                {
                    throw new Exception("该商户号不存在！");
                }

                string companyname = ds.Tables[0].Rows[0]["companyname"].ToString(); ;
                merchantService.InsertCertNoticeBlackList(spid, companyname, memo, Session["uid"].ToString());
                TbEdit.Visible = false;
                Bind(pager.CurrentPageIndex);
                WebUtils.ShowMessage(this.Page, "保存成功");
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "新增失败：" + ex.Message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            TbEdit.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            txtSpid_add.Text = "";
            txtFMemo.Text = "";
            TbEdit.Visible = true;
        }
        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                string fid = e.Item.Cells[0].Text.Trim(); //ID

                switch (e.CommandName)
                {
                    case "DEL": //删除
                        Delete(fid);
                        break;
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "删除失败：" + ex.Message);
            }
        }
    }
}