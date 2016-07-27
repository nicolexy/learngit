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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.BankCardBindModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// BankCardUnbind 的摘要说明。
    /// </summary>
    public partial class BankCardUnbindNew : PageBase
    {
        protected Wuqi.Webdiyer.AspNetPager Aspnetpager1;
        bool isRight_SensitiveRole = false;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                isRight_SensitiveRole = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                if (!ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                this.rbtn_bkt_JJK.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
                this.rbtn_bkt_XYK.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
                this.rbtn_bkt_ALL.CheckedChanged += new EventHandler(rbtns_CheckedChanged);

                this.rbtn_all.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
                this.rbtn_fastPay.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
                this.rbtn_ydt.CheckedChanged += new EventHandler(rbtns_CheckedChanged);

                if (!IsPostBack)
                {
                    if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().Trim() == "edit")
                    {
                        ShowEdit();
                    }

                    btnUnbind.Attributes["onClick"] = "if(!confirm('确定要解除绑定吗？')) return false;";

                    this.rbtn_all.Checked = true;
                    this.rbtn_bkt_ALL.Checked = true;

                    this.ddl_BankType.Items.Clear();
                    this.ddl_BankType.Items.Add(new ListItem("全部", ""));
                    AddAllBankType();

                    this.pager.RecordCount = 1000;
                    this.pager.PageSize = 5;
                }

                this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
            }
            catch(Exception ex)
            {
                LogError("BaseAccount.BankCardUnbindNew", "protected void Page_Load(object sender, System.EventArgs e)", ex);
                Response.Redirect("../login.aspx?wh=1");
            }
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        private void ShowEdit()
        {
            this.PanelList.Visible = false;
            this.PanelMod.Visible = true;            
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            // 20130809 数据库标记：FBDIndex=1绑定表 FBDIndex=2 临时绑定表

            DataSet ds = new BankCardBindService().GetBankCardBindList_New("", "", "", Request.QueryString["Fuid"].ToString(), "",
                "", "", "", "", 99, Request.QueryString["Fbind_serialno"].ToString(), Session["uid"].ToString(), 0, "", 0, 1);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
            {
                WebUtils.ShowMessage(this, "没有查找到相应的记录");
                return;
            }
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];

                //支付限额lxl
                ds.Tables[0].Columns.Add("Fonce_quota_str", typeof(String));
                ds.Tables[0].Columns.Add("Fday_quota_str", typeof(String));
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fonce_quota", "Fonce_quota_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fday_quota", "Fday_quota_str");
                this.lblonce_quota.Text = ds.Tables[0].Rows[0]["Fonce_quota_str"].ToString();
                this.lblday_quota.Text = ds.Tables[0].Rows[0]["Fday_quota_str"].ToString();
                //小额免短通知lxl
                if (ds.Tables[0].Rows[0]["sms_flag"].ToString() == "1")
                {
                    this.lbli_character2.Text = "已开启";
                }
                else if (ds.Tables[0].Rows[0]["sms_flag"].ToString() == "0")
                {
                    this.lbli_character2.Text = "已关闭";
                }

                this.lblFuin.Text = ds.Tables[0].Rows[0]["Fuin"].ToString();
                string Fbank_type = ds.Tables[0].Rows[0]["Fbank_type"].ToString();

                this.lblFbank_type.Text = classLibrary.getData.GetBankNameFromBankCode(Fbank_type);
                lblFbankType.Text = Fbank_type; //增加bank_type yinhuang 2013.7.18
                this.lblFbind_serialno.Text = ds.Tables[0].Rows[0]["Fbind_serialno"].ToString();
                this.lblFprotocol_no.Text = ds.Tables[0].Rows[0]["Fprotocol_no"].ToString();
                string Fbank_status = ds.Tables[0].Rows[0]["Fbank_status"].ToString();

                if (Fbank_status == "0")
                    this.lblFbank_status.Text = "未定义";
                else if (Fbank_status == "1")
                    this.lblFbank_status.Text = "预绑定状态(未激活)";
                else if (Fbank_status == "2")
                    this.lblFbank_status.Text = "绑定确认(正常)";
                else if (Fbank_status == "3")
                    this.lblFbank_status.Text = "解除绑定";
                else
                    this.lblFbank_status.Text = "Unknown";

                string cardTail = ds.Tables[0].Rows[0]["Fcard_tail"].ToString();
                if (cardTail != "" && cardTail.Length > 4)
                {
                    this.lblFcard_tail.Text = cardTail.Substring(cardTail.Length - 4, 4);
                }
                this.lblFcard_tail_db.Text = cardTail;

                this.lblFtruename.Text = classLibrary.setConfig.ConvertName(ds.Tables[0].Rows[0]["Ftruename"].ToString(), isRight_SensitiveRole);
                string Fbind_type = ds.Tables[0].Rows[0]["Fbind_type"].ToString();

                if (Fbind_type == "0")
                    this.lblFbind_type.Text = "未知类型";
                else if (Fbind_type == "1")
                    this.lblFbind_type.Text = "普通借记卡关联";
                else if (Fbind_type == "2")
                    this.lblFbind_type.Text = "银行联名卡关联";
                else if (Fbind_type == "3")
                    this.lblFbind_type.Text = "信用卡关联";
                else if (Fbind_type == "4")
                    this.lblFbind_type.Text = "内部绑定";
                else if (Fbind_type == "20")
                    this.lblFbank_type.Text = "普通信用卡关联";
                else
                    this.lblFbind_type.Text = "Unknown";

                string Fbind_status = ds.Tables[0].Rows[0]["Fbind_status"].ToString();

                if (Fbind_status == "0")
                    this.lblFbind_status.Text = "未定义";
                else if (Fbind_status == "1")
                    this.lblFbind_status.Text = "初始状态";
                else if (Fbind_status == "2")
                    this.lblFbind_status.Text = "开启";
                else if (Fbind_status == "3")
                    this.lblFbind_status.Text = "关闭";
                else if (Fbind_status == "4")
                    this.lblFbind_status.Text = "解除";
                else if (Fbind_status == "5")
                    this.lblFbind_status.Text = "银行已激活，用户未激活";
                else
                    this.lblFbind_status.Text = "Unknown";

                string Fbind_flag = ds.Tables[0].Rows[0]["Fbind_flag"].ToString();

                if (Fbind_flag == "0")
                    this.lblFbind_flag.Text = "未知";
                else if (Fbind_flag == "1")
                    this.lblFbind_flag.Text = "有效";
                else if (Fbind_flag == "2")
                    this.lblFbind_flag.Text = "无效";
                else
                    this.lblFbind_flag.Text = "Unknown";

                this.lblFbank_id.Text = ds.Tables[0].Rows[0]["Fbank_id"].ToString();
                this.lblFindex.Text = ds.Tables[0].Rows[0]["Findex"].ToString();
                this.lblFuid.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();
                this.txtFmemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();

                try
                {
                    
                    if (ds.Tables[0].Rows[0]["Fmobilephone"].ToString() != "")
                        this.lblPhone.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds.Tables[0].Rows[0]["Fmobilephone"].ToString(), isRight_SensitiveRole);
                    else
                        this.lblPhone.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds.Tables[0].Rows[0]["Ftelephone"].ToString(), isRight_SensitiveRole); 

                    this.lblUid.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();

                    switch (ds.Tables[0].Rows[0]["Fcre_type"].ToString())
                    {
                        case "1":
                            {
                                this.lblcreType.Text = "身份证"; break;
                            }
                        case "2":
                            {
                                this.lblcreType.Text = "护照"; break;
                            }
                        case "3":
                            {
                                this.lblcreType.Text = "军官证"; break;
                            }
                        default:
                            {
                                this.lblcreType.Text = "未知"; break;
                            }
                    }
                   
                    if (this.lblcreType.Text == "身份证")
                    {
                        this.lblCreID.Text = classLibrary.setConfig.IDCardNoSubstring(ds.Tables[0].Rows[0]["Fcre_id"].ToString(), isRight_SensitiveRole);
                    }
                    else
                    {
                        this.lblCreID.Text = ds.Tables[0].Rows[0]["Fcre_id"].ToString();
                    }
                    this.lblCreateTime.Text = dr["Fcreate_time"].ToString();
                    this.lblbindTimeLocal.Text = dr["Fbind_time_local"].ToString();
                    this.lblbindTimeBank.Text = dr["Fbind_time_bank"].ToString();
                    this.lblUnbindTimeLocal.Text = dr["Funchain_time_local"].ToString();
                    this.lblUnbindTimeBank.Text = dr["Funchain_time_bank"].ToString();
                }
                catch (Exception ex)
                {
                    LogError("BaseAccount.BankCardUnbindNew", "private void ShowEdit()", ex);
                    WebUtils.ShowMessage(this, ex.Message + ", stacktrace" + ex.StackTrace);

                }

                if (Fbind_flag == "2" && Fbind_status == "4" && Fbank_status == "3")
                    this.btnUnbind.Enabled = false;
                else
                    this.btnUnbind.Enabled = true;
            }
        }

        protected void btnUnbind_Click(object sender, System.EventArgs e)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                //20130809 如果为邮储一点通、建行一点通、工行一点通，它们的协议号存在了bankID字段，不能走统一的解绑方式
                //解绑时调用绑定服务解绑,使用uid 卡尾号
                //使用卡尾号，参数有限长，若超过4位，则使用uid  Fbind_serialno解绑

                //20140425 lxl 问题：存在一个用户一个银行卡绑定了两次的情况，查询出两条绑定记录，但是解绑不了
                //需要用特殊解绑方式的uid  Fbind_serialno来解绑
                //因此要将“调用特殊服务解绑”适用于所有银行，不能解绑的情况下就用该种解绑。
                //本次将UnbindBankCardSpecial接口做调整，适用需求
                if (CheckBoxUnbind.Checked == true)
                {
                    DataSet ds = qs.UnbindBankCardSpecial(this.lblFbankType.Text, this.lblFuin.Text, this.lblFcard_tail_db.Text, this.lblFbind_serialno.Text, this.lblFprotocol_no.Text);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        string res_info = dt.Rows[0]["res_info"].ToString();
                        if (res_info != null && res_info == "ok")
                        {
                            WebUtils.ShowMessage(this.Page, "调用特殊服务解绑成功");
                        }
                    }
                }
                else
                {
                    //2013/7/18 yinhuang 原来是修改表来解绑，现在使用接口来解绑
                    qs.UnbindBankCard(this.lblFbankType.Text, this.lblFuin.Text, this.lblFprotocol_no.Text);
                    WebUtils.ShowMessage(this.Page, "解绑成功");
                }

                this.btnUnbind.Enabled = false;
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogError("BaseAccount.BankCardUnbindNew", "protected void btnUnbind_Click(object sender, System.EventArgs e).SoapException调用服务出错:", eSoap);
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception ex)
            {
                LogError("BaseAccount.BankCardUnbindNew", "protected void btnUnbind_Click(object sender, System.EventArgs e).异常:", ex);
                WebUtils.ShowMessage(this.Page, ex.Message + ", stacktrace" + ex.StackTrace);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("BankCardUnbindNew.aspx");
        }

        //同步
        protected void btnSynchron_Click(object sender, System.EventArgs e)
        {
            try
            {
                var infos = new BankCardBindService().SyncBankCardBind(this.lblFbankType.Text, this.lblFcard_tail_db.Text, this.lblFbank_id.Text, Session["uid"].ToString());

                //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //qs.SynchronBankCardBind(this.lblFbankType.Text, this.lblFcard_tail_db.Text, this.lblFbank_id.Text);
                WebUtils.ShowMessage(this.Page, "同步成功");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogError("BaseAccount.BankCardUnbindNew", "protected void btnSynchron_Click(object sender, System.EventArgs e).SoapException调用服务出错:", eSoap);
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception ex)
            {
                LogError("BaseAccount.BankCardUnbindNew", "protected void btnSynchron_Click(object sender, System.EventArgs e).同步异常:", ex);
                WebUtils.ShowMessage(this.Page, ex.Message + ", stacktrace" + ex.StackTrace);
            }
        }

        private void BindData_New(int index)
        {
            this.pager.CurrentPageIndex = index;
            try
            {
                DataSet ds = null;

                int queryType = 0;

                if (this.rbtn_ydt.Checked)
                {
                    queryType = 1;
                }
                else if (this.rbtn_fastPay.Checked)
                {
                    queryType = 2;
                }

                string beginDateStr = this.tbx_beginDate.Value.Trim();
                string endDateStr = this.tbx_endDate.Value.Trim();

                try
                {
                    if (beginDateStr != "")
                        beginDateStr = DateTime.Parse(beginDateStr).ToString("yyyy-MM-dd HH:mm:ss");

                    if (endDateStr != "")
                        endDateStr = DateTime.Parse(endDateStr).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "请输入正确的日期格式");
                    return;
                }
                ds = new BankCardBindService().GetBankCardBindList_New(this.txtQQ.Text.Trim(), this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.tbx_uid.Text.Trim(), this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(), this.tbx_phone.Text.Trim(), beginDateStr,
                    endDateStr, int.Parse(this.ddl_bindStatue.SelectedValue), "",
                    Session["uid"].ToString(), queryType, this.ddl_creType.SelectedValue, (index - 1) * this.pager.PageSize, this.pager.PageSize);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception("没有查找到相应的记录！");
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("Fbank_statusStr", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Ftruename"] = classLibrary.setConfig.ConvertName(dr["Ftruename"].ToString(), isRight_SensitiveRole);
                        if (dr["Fbank_status"].ToString() == "0")
                            dr["Fbank_statusStr"] = "未定义";
                        else if (dr["Fbank_status"].ToString() == "1")
                            dr["Fbank_statusStr"] = "预绑定状态(未激活)";
                        else if (dr["Fbank_status"].ToString() == "2")
                            dr["Fbank_statusStr"] = "绑定确认(正常)";
                        else if (dr["Fbank_status"].ToString() == "3")
                            dr["Fbank_statusStr"] = "解除绑定";
                        else
                            dr["Fbank_statusStr"] = "Unknown";
                    }
                    this.dgList.DataSource = dt.DefaultView;
                    dgList.DataBind();
                    Session["qqid"] = this.dgList.Items[0].Cells[3].Text.Trim();
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogError("BaseAccount.BankCardUnbindNew", "private void BindData_New(int index).SoapException异常:", eSoap);
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception eSys)
            {
                LogError("BaseAccount.BankCardUnbindNew", "private void BindData_New(int index).SoapException异常:", eSys);
                string message = eSys.Message;
                string frozen = "res_info_origin=user is frozen";
                string notcomplete = "res_info_origin=userinfo is not complete";
                if (message.Contains(frozen))
                {
                    WebUtils.ShowMessage(this.Page, "冻结账户请使用其他条件查询!");
                }
                else if (message.Contains(notcomplete))
                {
                    WebUtils.ShowMessage(this.Page, "简化注册账户请使用其他条件查询!");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.Message.ToString()) + ", stacktrace" + eSys.StackTrace);
                }
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            BindData_New(1);
        }

        private void rbtns_CheckedChanged(object sender, EventArgs e)
        {
            getData.BankClass[] bkInfoList = null;
            this.ddl_BankType.Items.Clear();

            this.ddl_BankType.Items.Add(new ListItem("全部", ""));

            if (this.rbtn_fastPay.Checked)
            {
                if (this.rbtn_bkt_JJK.Checked)
                {
                    bkInfoList = getData.GetFPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_XYK.Checked)
                {
                    bkInfoList = getData.GetFPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_ALL.Checked)
                {
                    bkInfoList = getData.GetFPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }

                    bkInfoList = getData.GetFPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
            }
            else if (this.rbtn_ydt.Checked)
            {
                if (this.rbtn_bkt_JJK.Checked)
                {
                    bkInfoList = getData.GetOPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_XYK.Checked)
                {
                    bkInfoList = getData.GetOPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_ALL.Checked)
                {
                    bkInfoList = getData.GetOPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }

                    bkInfoList = getData.GetOPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
            }
            else if (this.rbtn_all.Checked)
            {
                if (this.rbtn_bkt_JJK.Checked)
                {
                    bkInfoList = getData.GetOPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }

                    bkInfoList = getData.GetFPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_XYK.Checked)
                {
                    bkInfoList = getData.GetOPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }

                    bkInfoList = getData.GetFPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_ALL.Checked)
                {
                    AddAllBankType();
                }
            }
        }

        private void AddAllBankType()
        {
            getData.BankClass[] bkInfoList = null;

            bkInfoList = getData.GetOPBankList(1);
            for (int i = 0; i < bkInfoList.Length; i++)
            {
                this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
            }

            bkInfoList = getData.GetOPBankList(2);
            for (int i = 0; i < bkInfoList.Length; i++)
            {
                this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
            }

            bkInfoList = getData.GetFPBankList(1);
            for (int i = 0; i < bkInfoList.Length; i++)
            {
                this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
            }

            bkInfoList = getData.GetFPBankList(2);
            for (int i = 0; i < bkInfoList.Length; i++)
            {
                this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
            }
        }

        private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            this.BindData_New(e.NewPageIndex);
        }      
    }
}
