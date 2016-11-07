using CFT.Apollo.Common.Configuration;
using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.BankCheckSystem;
using CFT.CSOMS.BLL.WechatPay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BankCheckSystem
{
    public partial class BankCheckUsersAdd :  TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                RequestBankInfo();
                bindRgiht();
            }
        }

        protected void bindRgiht()
        {
            radio_Fauth_level.Items.Clear();
            Dictionary<string, string> dic = new BankCheckSystemService().GetRights();
            foreach (var item in dic)
            {
                ListItem listitem = new ListItem(item.Value, item.Key);
                radio_Fauth_level.Items.Add(listitem);
            }
        }

        private void RequestBankInfo()
        {
            try
            {
                Hashtable ht = new FastPayService().RequestBankInfo();
                ArrayList akeys = new ArrayList(ht.Keys);
                akeys.Sort(new TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BankCardQueryNew.PolyphoneIComparer());
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                WebUtils.ShowMessage(this.Page, "数据验证失败！");
                return;
            }
            try
            {
                upImage(up_Fuser_id_no_url);

                string Fuser_login_account = txt_Fuser_bind_email.Text.Trim();
                string Fbank_id = ddl_Fbank_id.SelectedValue;
                string Fuser_bind_email = txt_Fuser_bind_email.Text.Trim();
                string Fuser_name = txt_Fuser_name.Text.Trim();
                string Fuser_id_no = txt_Fuser_id_no.Text.Trim();
                string Fcontact_address = txt_Fcontact_address.Text.Trim();
                string Fcontact_name = txt_Fcontact_name.Text.Trim();
                string Fcontact_tel = txt_Fcontact_tel.Text.Trim();
                string Fcontact_mobile = txt_Fcontact_mobile.Text.Trim();
                string Fuser_id_no_url = ViewState["alPath"] != null ? ViewState["alPath"].ToString() : "";
                string Fcontact_qq = txt_Fcontact_qq.Text.Trim();
                string Fremark = txt_Fremark.Text.Trim();
                List<string> Fauth_levels = new Func<List<string>>(() =>
                {
                    List<string> list = new List<string>();
                    foreach (ListItem item in radio_Fauth_level.Items)
                    {
                        if (item.Selected)
                            list.Add(item.Value);
                    }
                    return list;
                })();

                string PWD;
                if (!new BankCheckSystemService().UsersAdd(out PWD, Fuser_login_account, Fbank_id, Fuser_bind_email, Fuser_name, Fuser_id_no,
                    Fcontact_address, Fcontact_name, Fcontact_tel, Fcontact_mobile, Fuser_id_no_url, Fcontact_qq, Fremark, Fauth_levels))
                {
                    WebUtils.ShowMessage(this.Page, "添加失败！");
                }

                if (!new BankCheckSystemService().InsertRecords(Fuser_bind_email, ((int)OperationType.创建帐号).ToString(), "", Session["uid"].ToString()))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("插入操作记录失败"));
                }
                string str_params = "username={0}&initial={1}";
                str_params = string.Format(str_params, Fuser_name, PWD);
//#if DEBUG
//                Fuser_bind_email = "2081146365@qq.com";
//#endif
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(Fuser_bind_email, "2592", str_params);
                WebUtils.ShowMessage(this.Page, "添加成功！");
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
                LogHelper.LogInfo("银行查单系统新增账号失败：" + ex.ToString());
            }
        }


        protected void upImage(HtmlInputFile file)
        {
            //上传需要的图片，并返回对应服务器上的地址
            //存放文件
            // string s1 = File1.Value;
            try
            {
                if (file.Value == "")
                {
                    return;
                }
                string szTypeName = Path.GetExtension(file.Value);
                string upStr = null;

                if (szTypeName.ToLower() != ".jpg" && szTypeName.ToLower() != ".gif" && szTypeName.ToLower() != ".bmp" && szTypeName.ToLower() != ".png")
                {
                    throw new Exception("上传的文件不正确，必须为jpg,gif,bmp,png");
                }

                string fileName = "s1" + DateTime.Now.ToString("yyyyMMddHHmmss") + szTypeName;
                upStr = "uploadfile/" + DateTime.Now.ToString("yyyyMMdd") + "/CSOMS/RedemptionFund";

                string targetPath = Path.Combine(Server.MapPath(Request.ApplicationPath), upStr.Replace("/", "\\"));
                PublicRes.CreateDirectory(targetPath);

                file.PostedFile.SaveAs(Path.Combine(targetPath, fileName));

                if (AppSettings.Get<bool>("FPSenabled", false))
                {
                    //前期 先保留 老的  图片存储方式
                    var result = commLib.FPSFileHelper.UploadFile(file.PostedFile.InputStream, "RedemptionFund/" + fileName);
                    ViewState["alPath"] = ViewState["kfPath"] = result.url;
                }
                else
                {
                    ViewState["kfPath"] = "/" + upStr + "/" + fileName;
                    ViewState["alPath"] = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString() + ViewState["kfPath"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("上传图片失败！" + ex.ToString());
            }
        }
    }
}