using CFT.CSOMS.BLL.HandQModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.HandQBusiness
{
    public partial class HandQTransQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pager.PageSize = 20;
                pager.RecordCount = 10000;
            }
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

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string TransType = RadioButtonList_SelectValue("TransType");
                string uin = txt_user.Text;//527123000
                ViewState["TransType"] = TransType;
                ViewState["uin"] = uin;
                pager.CurrentPageIndex = 1;
                Bind();
            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message.ToString()));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }

        private void Bind() 
        {
            string TransType = ViewState["TransType"].ToString();
            string uin = ViewState["uin"].ToString();
            int limit = pager.PageSize;
            int offset = (pager.CurrentPageIndex - 1) * limit;

            if (TransType == "TransOut")
            {
                DataTable dt = new HandQService().HandQPayerTrans(uin, offset, limit);
                if (!dt.Columns.Contains("Fsave_listid"))
                {
                    dt.Columns.Add("Fsave_listid", typeof(string));
                }
                if (!dt.Columns.Contains("Ftransfer_listid"))
                {
                    dt.Columns.Add("Ftransfer_listid", typeof(string));
                }
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                DataGrid1.Caption = "转出";
            }
            else if (TransType == "TransIn")
            {
                DataTable dt = new HandQService().HandQSellerTrans(uin, offset, limit);
                if (!dt.Columns.Contains("Fsave_listid"))
                {
                    dt.Columns.Add("Fsave_listid", typeof(string));
                }
                if (!dt.Columns.Contains("Ftransfer_listid"))
                {
                    dt.Columns.Add("Ftransfer_listid", typeof(string));
                }
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                DataGrid1.Caption = "转入";
            }
        }
        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                pager.CurrentPageIndex = e.NewPageIndex;
                Bind();
            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message.ToString()));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }
    }
}