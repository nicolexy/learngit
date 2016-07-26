using CFT.CSOMS.BLL.ForeignCurrencyModule;
using CFT.CSOMS.COMMLIB;
using commLib.Entity.HKWallet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class RealNameCheck : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        string ip = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pager1.RecordCount = 10000;
                pager1.PageSize = 5;
                pager1.Visible = false;
            }

            ip = this.Page.Request.UserHostAddress.ToString();
            if (ip == "::1")
                ip = "127.0.0.1";
        }

        protected void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager1.CurrentPageIndex = e.NewPageIndex;
            try
            {
                bind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }

        protected void btn_Query_Click(object sender, EventArgs e)
        {
            try
            {
                bind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }
        private void bind()
        {
            var bll = new FCXGWallet();
            string input = txt_input_id.Text.Trim();
            string uin = "";

            if (!string.IsNullOrEmpty(input))
            {
                if (checkUid.Checked)
                {
                    string queryuid = input;
                    var dt_user = bll.QueryUserInfo(queryuid, ip);
                    if (dt_user != null && dt_user.Columns.Count > 0 && dt_user.Columns.Contains("uin"))
                    {
                        uin = dt_user.Rows[0]["uin"].ToString();
                    }
                }
                else if (checkWeChatId.Checked)
                {
                    uin = WeChatHelper.GetFCXGOpenIdFromWeChatName(input, Request.UserHostAddress) + "@wx.hkg";
                }
                else if (checkUin.Checked)
                {
                    uin = input;
                }
            }

            string type = ddl_type.SelectedValue.Trim();
            string state = ddl_state.SelectedValue.Trim();
            DateTime? start_time = null;
            DateTime? end_time = null;
            if (!string.IsNullOrEmpty(txtstime.Text.Trim()))
            {
                start_time = Convert.ToDateTime(txtstime.Text.Trim());
            }
            if (!string.IsNullOrEmpty(txtetime.Text.Trim()))
            {
                end_time = Convert.ToDateTime(txtetime.Text.Trim());
            }

            int limit = pager1.PageSize;
            int offset = limit * (pager1.CurrentPageIndex - 1);
            //DataTable dt = new FCXGWallet().QueryRealNameInfo2(uin, type, state, start_time, end_time, ip, offset, limit);
            //ViewState["dt"] = dt;
            //DataGrid1.DataSource = dt;
            //DataGrid1.DataBind();

            List<HKWalletRealNameCheckModel> list = new FCXGWallet().New_QueryRealNameInfo2(uin, type, state, start_time, end_time, ip, offset, limit);
            DataGrid1.DataSource = list;
            DataGrid1.DataBind();

            pager1.Visible = true;
            panDetail.Visible = false;
            ViewState["list"] = list;
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                //DataTable dt = ViewState["dt"] as DataTable;
                //string approval_id = e.Item.Cells[0].Text.Trim();
                //DataRow dr = dt.Select(" approval_id='" + approval_id + "'")[0];
                //lbl_uin.Text = dr["uin"].ToString();
                //lbl_name.Text = dr["name"].ToString();
                //lbl_birthday.Text = dr["birthday"].ToString();
                //lbl_country.Text = dr["country"].ToString();
                //lbl_cre_type.Text = dr["cre_type"].ToString();
                //lbl_cre_id.Text = dr["cre_id"].ToString();
                //lbl_approval_id.Text = dr["approval_id"].ToString();

                //string GetImageFromKf2Url = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString();

                //img_photo_path1.ImageUrl = GetImageFromKf2Url + "/" + dr["photo_path1"].ToString();
                //img_photo_path2.ImageUrl = GetImageFromKf2Url + "/" + dr["photo_path2"].ToString();
                //txt_memo.Text = dr["memo"].ToString();

                //string state = dr["state"].ToString();
                //if (state != "待审核")
                //{
                //    btn_Pass.Visible = false;
                //    btn_Refuse.Visible = false;
                //}

                //panDetail.Visible = true;
                List<HKWalletRealNameCheckModel> list = ViewState["list"] as List<HKWalletRealNameCheckModel>;
                string approval_id = e.Item.Cells[0].Text.Trim();
                HKWalletRealNameCheckModel RealNameModel = list.FirstOrDefault(p => p.approval_id == approval_id);

                lbl_uin.Text = RealNameModel.uin;
                lbl_name.Text = RealNameModel.name;
                lbl_birthday.Text = RealNameModel.birthday;
                lbl_country.Text = RealNameModel.country;
                lbl_cre_type.Text = RealNameModel.cre_type_str;
                lbl_cre_id.Text = RealNameModel.cre_id;
                lbl_approval_id.Text = RealNameModel.approval_id;
                string GetImageFromKf2Url = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString();

                if (string.IsNullOrEmpty(RealNameModel.photo_path1))
                {
                    img_photo_path1.Visible = false;
                }
                else
                {
                    img_photo_path1.ImageUrl = GetImageFromKf2Url + "/waibi/" + RealNameModel.photo_path1;
                }
                if (string.IsNullOrEmpty(RealNameModel.photo_path2))
                {
                    img_photo_path2.Visible = false;
                }
                else
                {
                    img_photo_path2.ImageUrl = GetImageFromKf2Url + "/waibi/" + RealNameModel.photo_path2;
                }

#if DEBUG
                img_photo_path1.ImageUrl = "http://kf2.cf.com/waibi/20160629/600000086_20160629_165139_1.jpg";
                img_photo_path2.ImageUrl = "http://kf2.cf.com/waibi/20160629/600000086_20160629_165139_1.jpg";
#endif
                //txt_memo.Text = RealNameModel.memo;
                ddlmemo.SelectedValue = RealNameModel.memo;
                string state = RealNameModel.state_str;
                if (state != "待审核")
                {
                    btn_Pass.Visible = false;
                    btn_Refuse.Visible = false;
                }
                panDetail.Visible = true;
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }

        protected void btn_Pass_Click(object sender, EventArgs e)
        {
            try
            {
                string checktype = (sender as Button).Text;

                string uin = lbl_uin.Text.Trim();
                string approval_id = lbl_approval_id.Text.Trim();
                //审批状态(2: 审核通过 3: 客服审核不通过 4: 机器审核不通过) 
                string state = "";
                if (checktype == "通 过")
                {
                    state = "2";
                }
                else 
                {
                    state = "3";
                }
                //state = "1";
                string memo = ddlmemo.SelectedValue;
                if (Session["uid"] == null) 
                {
                    Response.Redirect("../login.aspx?wh=1");
                }

                string Operator = Session["uid"].ToString();

#if DEBUG
                Operator = "v_yqyqguo";
#endif
                if ("0" == new FCXGWallet().checkRealName(Operator, uin, approval_id, state, memo, ip)) 
                {
                    WebUtils.ShowMessage(this.Page, "审核成功！");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "审核失败！" + HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

       
    }
}