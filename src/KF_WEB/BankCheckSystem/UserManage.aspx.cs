using CFT.CSOMS.BLL.BankCheckSystem;
using CFT.CSOMS.BLL.WechatPay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BankCheckSystem
{
    public partial class UserManage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("BankCheck", this))
                    Response.Redirect("../login.aspx?wh=1");

                Aspnetpager1.RecordCount = 10000;
                Aspnetpager1.PageSize = 10;
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                RequestBankInfo();
            }
           
        }
        private void RequestBankInfo()
        {
            try
            {
                Hashtable ht = new FastPayService().RequestBankInfo();
                ArrayList akeys = new ArrayList(ht.Keys);
                akeys.Sort(new TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BankCardQueryNew.PolyphoneIComparer());
                ddl_Fbank_id.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));
                foreach (string k in akeys)
                {
                    ddl_Fbank_id.Items.Add(new System.Web.UI.WebControls.ListItem(k.ToString(), ht[k].ToString()));
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "请求RequestBankInfo出错：原因{0}" + ex.ToString());
            }
        }

        private DataTable GetBankType(string binkidField, string bankNameField, DataTable dt)
        {
            Hashtable ht = new FastPayService().RequestBankInfo();
            if (dt == null) return null;
            dt.Columns.Add(bankNameField, typeof(string));
            foreach (DataRow dr in dt.Rows)
            {
                string binkid = dr[binkidField].ToString().Trim();
                //binkid = binkid.Split('|')[0];
                if (ht.ContainsValue(binkid))
                {
                    dr[bankNameField] = new Func<string>(() =>
                    {
                        foreach (var item in ht.Keys)
                        {
                            if (ht[item].ToString() == binkid)
                            {
                                return item.ToString();
                            }
                        }
                        return "未知：" + binkid;
                    })();
                }
                else
                {
                    dr[bankNameField] = "未知：" + binkid;
                }
            }
            return dt;
        }

        protected void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {                
                Bind(1);
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        private void Bind(int index)
        {
            string userBindEmail = txt_userBindEmail.Text.Trim();
            string bankId = ddl_Fbank_id.SelectedValue;
            string userName = txt_Fuser_name.Text.Trim();
            string userIdNo = txt_Fuser_id_no.Text.Trim();
            int limit = Aspnetpager1.PageSize;
            int offset = (index - 1) * limit;
            DataTable dt = new BankCheckSystemService().GetUserinfo2(userBindEmail, bankId, userName, userIdNo, offset, limit);
            dt = GetBankType("Fbank_id", "Fbank_id_str", dt);
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();

        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "detail")
                {
                    tbDetail.Visible = true;
                    string userId = e.Item.Cells[0].Text.Trim();
                    string userbindemail = e.Item.Cells[1].Text.Trim();

                    DataTable dt = new BankCheckSystemService().GetUserinfo2(userbindemail, "", "", "", 0, 1);
                    dt = GetBankType("Fbank_id", "Fbank_id_str",dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lbl_Fuser_bind_email.Text = dt.Rows[0]["Fuser_bind_email"].ToString();
                        lbl_Fbank_id_str.Text = dt.Rows[0]["Fbank_id_str"].ToString();
                        lbl_Fuser_name.Text = dt.Rows[0]["Fuser_name"].ToString();
                        lbl_Fuser_id_no.Text = dt.Rows[0]["Fuser_id_no"].ToString();
                        lbl_Fcontact_tel.Text = dt.Rows[0]["Fcontact_tel"].ToString();
                        txt_Fcontact_name.Text = dt.Rows[0]["Fcontact_name"].ToString();
                        txt_Fcontact_mobile.Text = dt.Rows[0]["Fcontact_mobile"].ToString();
                        txt_Fcontact_qq.Text = dt.Rows[0]["Fcontact_qq"].ToString();
                        txt_Fcontact_email.Text = dt.Rows[0]["Fcontact_email"].ToString();
                        txt_Fcontact_manager.Text = dt.Rows[0]["Fcontact_manager"].ToString();
                        txt_Fremark.Text = dt.Rows[0]["Fremark"].ToString();

                        btnSave.CommandArgument = dt.Rows[0]["Fuser_id"].ToString();
                    }
                    bindRgiht(userId);
                }
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        protected void bindRgiht(string userId)
        {
            DataTable dt = new BankCheckSystemService().GetUserAuthRelation(userId);
            Dictionary<string, string> dic = new BankCheckSystemService().GetRights();
            chk_right.Items.Clear();
            foreach (var item in dic)
            {
                ListItem listitem = new ListItem(item.Value, item.Key);
                chk_right.Items.Add(listitem);
                if (dt != null)
                {
                    if (dt.Select(" Fauth_level ='" + item.Key + "'").Count() > 0)
                    {
                        listitem.Selected = true;
                    }
                }
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string Fuser_bind_email = lbl_Fuser_bind_email.Text.Trim();
                string Fcontact_name = txt_Fcontact_name.Text.Trim();
                string Fcontact_mobile = txt_Fcontact_mobile.Text.Trim();
                string Fcontact_qq = txt_Fcontact_qq.Text.Trim();
                string Fcontact_email = txt_Fcontact_email.Text.Trim();
                string Fcontact_manager = txt_Fcontact_manager.Text.Trim();
                string Fremark = txt_Fremark.Text.Trim();
                //修改用户信息
                if (!new BankCheckSystemService().EditUsersInfo(Fuser_bind_email, Fcontact_name, Fcontact_mobile, Fcontact_qq, Fcontact_email, Fcontact_manager, Fremark))
                {
                    WebUtils.ShowMessage(this.Page, "修改用户信息失败！");
                    return;
                }

                string userId = btnSave.CommandArgument;
                List<string> rights = new Func<List<string>>(() =>
                {
                    List<string> list = new List<string>();
                    foreach (ListItem item in chk_right.Items)
                    {
                        if (item.Selected)
                            list.Add(item.Value);
                    }
                    return list;
                })();
                //修改权限
                if (!new BankCheckSystemService().EditAuthRelation(userId, rights))
                {
                    WebUtils.ShowMessage(this.Page, "修改权限失败！");
                    return;
                }
                if (!new BankCheckSystemService().InsertRecords(Fuser_bind_email, ((int)OperationType.修改用户基本信息).ToString(), "", Session["uid"].ToString()))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("插入操作记录失败"));
                    return;
                }
                tbDetail.Visible = false;
                Bind(Aspnetpager1.CurrentPageIndex);
                WebUtils.ShowMessage(this.Page, "修改成功！");
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }
      
        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            Aspnetpager1.CurrentPageIndex = e.NewPageIndex;
            Bind(e.NewPageIndex);

        }
    }
}