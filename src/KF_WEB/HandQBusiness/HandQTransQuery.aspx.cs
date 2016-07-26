﻿using CFT.CSOMS.BLL.HandQModule;
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
    public partial class HandQTransQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                textBoxBeginDate.Text = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd 00:00:00");
                textBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                pager.PageSize = 10;
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
                ValidateDate();
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
            string Msg = "";
            string stime = ViewState["begindate"].ToString();
            string etime = ViewState["enddate"].ToString();
            if (TransType == "TransOut")
            {
                DataTable dt = new HandQService().HandQTansferQuery(uin, offset, limit, 1,stime,etime, out Msg);
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                DataGrid1.Caption = "转出";
            }
            else if (TransType == "TransIn")
            {
                DataTable dt = new HandQService().HandQTansferQuery(uin, offset, limit, 2,stime,etime, out Msg);
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
        private void ValidateDate()
        {
            DateTime begindate;
            DateTime enddate;

            if (!DateTime.TryParse(textBoxBeginDate.Text, out begindate) || !DateTime.TryParse(textBoxEndDate.Text, out enddate))
            {
                throw new Exception("日期输入有误！");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("终止日期小于起始日期，请重新输入！");
            }         

            var a = (enddate - begindate);
            if ((enddate - begindate).TotalDays > 30)
            {
                throw new Exception("查询日期时间跨度不能大于30天");
            }
            ViewState["begindate"] = begindate.ToString("yyyy-MM-dd HH:mm:ss");
            ViewState["enddate"] = enddate.ToString("yyyy-MM-dd HH:mm:ss");

        }
    }
}