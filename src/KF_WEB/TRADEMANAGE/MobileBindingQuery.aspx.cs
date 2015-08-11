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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.MobileModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// MobileBindingQuery 的摘要说明。
    /// </summary>
    public partial class MobileBindingQuery : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                //ViewState["QQ"] = this.txbQQ.Text.Trim();
                BindData(1);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + classLibrary.setConfig.replaceMStr(ex.Message));
            }
        }

        private void BindData(int index)
        {
            DataSet ds = new MobileService().GetMsgNotify(txbQQ.Text.Trim());

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string Fstatus = Convert.ToString(Convert.ToInt32(dr["Fstatus"]), 2);
                    if (Fstatus.Substring(30, 1).ToString() == "0" && Fstatus.Substring(25, 1).ToString() == "0" && Fstatus.Substring(24, 1).ToString() == "0")
                    {
                        dr["Unbind"] = "";
                    }
                    else
                    {
                        if (ClassLib.ValidateRight("DeleteCrt", this))
                            dr["Unbind"] = "解绑";
                        else
                            dr["Unbind"] = "";
                    }
                }

                DataGrid1.DataSource = ds;
                DataGrid1.DataBind();
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "没有找到记录");
            }
        }

        protected void btnUpdateBindInfo_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (new MobileService().UpDateBindInfo(this.txbQQ.Text.Trim()))
                {
                    WebUtils.ShowMessage(this.Page, "更新成功!");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "更新失败!");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, "更新失败" + ex.Message);
            }
        }

        public void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                try
                {
                    string Msg = "";
                    if (new MobileService().UnbindMsgNotify(e.Item.Cells[0].Text.Trim(), out Msg))
                    {
                        BindData(1);
                        WebUtils.ShowMessage(this.Page, "解绑成功");
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, Msg);
                    }
                }
                catch (Exception ex)
                {
                    WebUtils.ShowMessage(this.Page, ex.Message);
                }
            }
        }

    }
}
