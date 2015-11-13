using CFT.CSOMS.BLL.TradeModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class FreezeFinQuery2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = Session["uid"] as string;
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
                pager.RecordCount = 1000;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var sDateStr = txt_sDate.Value.Trim();
            var eDateStr = txt_eDate.Value.Trim();
            var uin = txt_uin.Text.Trim();
            var moeny = txt_moeny.Text.Trim();
            if (uin.Length < 1)
            {
                WebUtils.ShowMessage(this, "账号不可以为空"); return;
            }

            if (sDateStr.Length < 1 || eDateStr.Length < 1)
            {
                WebUtils.ShowMessage(this, "请输入日期"); return;
            }
            DateTime sDate, eDate;
            if (!DateTime.TryParse(sDateStr, out sDate) || !DateTime.TryParse(eDateStr, out eDate))
            {
                WebUtils.ShowMessage(this, "请输入正确的日期"); return;
            }
            if (moeny.Length > 0)
            {
                try
                {
                    moeny = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.YuanToFen(moeny);
                }
                catch
                {
                    WebUtils.ShowMessage(this, "请输入正确的金额"); return;
                }
            }
            ViewState["uin"] = uin;
            ViewState["sDate"] = sDate;
            ViewState["eDate"] = eDate;
            ViewState["moeny"] = moeny;
            BindDataGrid(uin, sDate, eDate, moeny, 1);
        }

        private void BindDataGrid(string uin, DateTime sDate, DateTime eDate, string moeny, int index)
        {
            try
            {
                var ref_param = ViewState["ref_param"] as string ?? "";
                var imax = pager.PageSize;
                var istr = pager.PageSize * (index - 1);
                var ds = new TradeService().GetBankRollList(uin, "", sDate, eDate, "3", istr, imax, ref  ref_param);
                ViewState["ref_param"] = ref_param;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var dt = ds.Tables[0];
                    if (!string.IsNullOrEmpty(moeny))
                    {
                        var arr = dt.Select("Fcon = '" + moeny + "'");
                        dt = dt.Clone();
                        foreach (var item in arr)
                        {
                            dt.ImportRow(item);
                        }
                    }
                    #region 值处理
                    dt.Columns.Add("Fcon_str");
                    dt.Columns.Add("Fpaynum_str");
                    dt.Columns.Add("Fsubject_str");

                    foreach (DataRow item in dt.Rows)
                    {
                        item["Fsubject_str"] = setConfig.convertSubject((string)item["Fsubject"]);
                    }

                    setConfig.FenToYuan_Table(dt, "Fcon", "Fcon_str");
                    setConfig.FenToYuan_Table(dt, "Fpaynum", "Fpaynum_str");
                    #endregion
                    dg_FundFlow.DataSource = ViewState["CacheData"] = dt;
                    dg_FundFlow.DataBind();
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, "查询出错" + PublicRes.GetErrorMsg(ex.Message));
            }
        }

        protected void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            var uin = (string)ViewState["uin"];
            var sDate = (DateTime)ViewState["sDate"];
            var eDate = (DateTime)ViewState["eDate"];
            var moeny = (string)ViewState["moeny"];
            BindDataGrid(uin, sDate, eDate, moeny, e.NewPageIndex);
        }

        protected void dg_passwordLog_DataBinding(object sender, EventArgs e)
        {
            var dg = sender as DataGrid;
            var dt = dg.DataSource as DataTable;
            if (dt != null)
            {
                pager.Visible = true;
                pager.RecordCount = 1000;
            }
        }

        protected void dg_FundFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dt = ViewState["CacheData"] as DataTable;
            var index = ((DataGrid)sender).SelectedIndex;
            var row = dt.Rows[index];
            lb_Fapplyid.InnerText = row["Fapplyid"] as string;
            lb_Fbkid.InnerText = row["Fbkid"] as string;
            lb_Flistid.InnerText = row["Flistid"] as string;
            lb_Fqqid.InnerText = row["Fqqid"] as string;
            lb_Fsubject.InnerText = row["Fsubject_str"] as string;
            lb_Fcreate_time.InnerText = row["Fcreate_time"] as string;
            lb_Fcon_str.InnerText = row["Fcon_str"] as string;
            lb_Fpaynum_str.InnerText = row["Fpaynum_str"] as string;
            //lb_Fbank_type.InnerText = row["Fbank_type"] as string;
            lb_Fmodify_time.InnerText = row["Fmodify_time"] as string;
            lb_Fmemo.InnerText = row["Fmemo"] as string;
        }
    }
}