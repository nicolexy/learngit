using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class FCXGFreezeLog : System.Web.UI.Page
    {
        string op_uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = op_uid = Session["uid"] as string;
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
                pager.RecordCount = 1000;
            }
        }

        protected void btn_query_Click(object sender, EventArgs e)
        {
            pager.CurrentPageIndex = 1;
            QueryLog();
        }

        //分页控件
        protected void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            QueryLog();
        }

        private void QueryLog()
        {
            System.Data.DataTable dt = null;
            try
            {
                var param = new Dictionary<string, string>();

                var start_date_str = txt_start_date.Value.Trim();
                var end_date_str = txt_end_date.Value.Trim();
                var uid = txt_uid.Text.Trim();
                var uin = txt_uin.Text.Trim();
                var channel = list_channel.SelectedValue;
                var op_name = txt_op_name.Text.Trim();

                #region 添加条件

                if (start_date_str.Length > 0 || end_date_str.Length > 0)
                {
                    DateTime start_time, end_time;
                    if (DateTime.TryParse(start_date_str, out start_time) && DateTime.TryParse(end_date_str, out end_time))
                    {
                        if (end_time.Year != start_time.Year || (end_time - start_time).Days > 30)
                        {
                            WebUtils.ShowMessage(this.Page, "请输入不跨年并且在30天以内的日期"); return;
                        }
                        param.Add("start_time", start_time.ToString("yyyy-MM-dd"));
                        param.Add("end_time", end_time.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "请输入正确格式的开始时间和结束时间"); return;
                    }
                }


                if (uid.Length > 0)
                {
                    param.Add("uid", uid);
                }

                if (uin.Length > 0)
                {
                    param.Add("uin", uin);
                }

                if (op_name.Length > 0)
                {
                    param.Add("op_name", op_name);
                }

                if (channel.Length > 0)
                {
                    param.Add("channel", channel);
                }
                #endregion

                var bll = new FCXGWallet();
                var skip = pager.CurrentPageIndex * 10 - 10;
                dt = bll.QueryFreezeLog(param, skip, 10);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg(ex.Message));
            }
            dg_freezeLog.DataSource = dt;
            dg_freezeLog.DataBind();
        }
    }
}