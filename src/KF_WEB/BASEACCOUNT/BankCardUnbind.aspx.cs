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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// BankCardUnbind 的摘要说明。
    /// </summary>
    public partial class BankCardUnbind : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
                    this.pager.PageSize = 10;

                    this.pager1.RecordCount = 1000;
                    this.pager1.PageSize = 10;
                }

                this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
                this.pager1.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager1_PageChanged);
                this.Datagrid1.ItemCommand += new DataGridCommandEventHandler(Datagrid1_ItemCommand);
            }
            catch
            {
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
            DataSet ds = qs.GetBankCardBind(Request.QueryString["Fuid"].ToString(), Request.QueryString["Findex"].ToString(), Request.QueryString["FBDIndex"].ToString());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
            {
                //throw new Exception("没有查找到相应的记录！");
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
                    this.lblCreID.Text = classLibrary.setConfig.ConvertCreID(ds.Tables[0].Rows[0]["Fcre_id"].ToString());
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
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message + ", stacktrace" + ex.StackTrace);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("BankCardUnbind.aspx");
        }

        //同步
        protected void btnSynchron_Click(object sender, System.EventArgs e)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.SynchronBankCardBind(this.lblFbankType.Text, this.lblFcard_tail_db.Text, this.lblFbank_id.Text);
                WebUtils.ShowMessage(this.Page, "同步成功");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message + ", stacktrace" + ex.StackTrace);
            }
        }

        private void BindData_UIN(int index)
        {
            this.pager1.CurrentPageIndex = index;
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                DataSet ds1 = qs.GetBankCardBindList_UIN(this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(), this.tbx_phone.Text.Trim(),
                    int.Parse(this.ddl_bindStatue.SelectedValue), this.pager1.PageSize * (index - 1), this.pager1.PageSize);
                //继续查实时绑定库表，为了查当天记录lxl
                DataSet ds2 = qs.GetBankCardBindList_UIN_2(this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(), this.tbx_phone.Text.Trim(),
                    int.Parse(this.ddl_bindStatue.SelectedValue), this.pager1.PageSize * (index - 1), this.pager1.PageSize);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.Tables[0].Columns.Add("fuin", typeof(string));
                ds.Tables[0].Columns.Add("Fbank_typeStr", typeof(string));
                ds.Tables[0].Columns.Add("fcre_id", typeof(string));
                ds.Tables[0].Columns.Add("fbank_id", typeof(string));
                ds.Tables[0].Columns.Add("fcard_tail", typeof(string));//20131017 lxl 卡号后四位
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        DataRow drResult = dt.NewRow();
                        drResult["fuin"] = dr["fuin"].ToString();
                        drResult["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
                        drResult["fcre_id"] = setConfig.ConvertCreID(dr["fcre_id"].ToString());
                        drResult["fbank_id"] = setConfig.ConvertCreID(dr["fbank_id"].ToString());
                        drResult["fcard_tail"] = dr["fcard_tail"].ToString();
                        dt.Rows.Add(drResult);
                    }
                }
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds2.Tables[0].Rows)
                    {
                        DataRow drResult = dt.NewRow();
                        drResult["fuin"] = dr["fuin"].ToString();
                        drResult["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
                        drResult["fcre_id"] = "";
                        drResult["fbank_id"] = dr["fcard_id"].ToString();
                        drResult["fcard_tail"] = dr["fcard_tail"].ToString();
                        dt.Rows.Add(drResult);
                    }
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.Datagrid1.DataSource = ds;
                    this.Datagrid1.DataBind();

                    BindData(ds.Tables[0].Rows[0]["fuin"].ToString(), 1);
                }
                else
                {
                    WebUtils.ShowMessage(this, "查询一点通账户列表为空");
                    this.dgList.DataSource = null;
                    this.dgList.DataBind();
                    return;
                }
            }
            catch (System.Exception ex)
            {
                WebUtils.ShowMessage(this, ex.Message);
            }
        }


        private void BindData(string qqid, int index)
        {
            this.pager.CurrentPageIndex = index;
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

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
                        DateTime.Parse(beginDateStr);

                    if (endDateStr != "")
                        DateTime.Parse(endDateStr);
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "请输入正确的日期格式");
                    return;
                }

                DataSet ds = qs.GetBankCardBindList_2(qqid.Trim(), this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(),
                    this.tbx_phone.Text.Trim(), beginDateStr, endDateStr, queryType,
                    int.Parse(this.ddl_bindStatue.SelectedValue), (index - 1) * this.pager.PageSize, this.pager.PageSize);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.dgList.DataSource = null;
                    this.dgList.DataBind();
                    WebUtils.ShowMessage(this, "查询一点通信息为空");
                    return;
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("Fbank_typeStr", typeof(string));
                    dt.Columns.Add("Fbank_statusStr", typeof(string));
                    dt.Columns.Add("Fxyzf_typeStr", typeof(string)); //信用支付类型

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!(dr["Fi_character4"] is DBNull))
                        {
                            if (dr["Fi_character4"].ToString() == "33")
                            {
                                dr["Fxyzf_typeStr"] = "是";
                            }
                            else
                            {
                                dr["Fxyzf_typeStr"] = "否";
                            }
                        }
                        else
                        {
                            dr["Fxyzf_typeStr"] = "否";
                        }

                        dr["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());                        
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
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString() + ", stacktrace" + eSys.StackTrace);
            }
        }

        private void BindData(int index)
        {
            this.pager.CurrentPageIndex = index;
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

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
                        DateTime.Parse(beginDateStr);

                    if (endDateStr != "")
                        DateTime.Parse(endDateStr);
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "请输入正确的日期格式");
                    return;
                }

                ds = qs.GetBankCardBindList_New(this.txtQQ.Text.Trim(), this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.tbx_uid.Text.Trim(), this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(),
                    this.tbx_phone.Text.Trim(), beginDateStr, endDateStr, queryType, this.cbx_showAbout.Checked,
                    int.Parse(this.ddl_bindStatue.SelectedValue), "", (index - 1) * this.pager.PageSize, this.pager.PageSize);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception("没有查找到相应的记录！");
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("Fbank_typeStr", typeof(string));
                    dt.Columns.Add("Fbank_statusStr", typeof(string));
                    dt.Columns.Add("Fxyzf_typeStr", typeof(string)); //信用支付类型

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!(dr["Fi_character4"] is DBNull))
                        {
                            if (dr["Fi_character4"].ToString() == "33")
                            {
                                dr["Fxyzf_typeStr"] = "是";
                            }
                            else
                            {
                                dr["Fxyzf_typeStr"] = "否";
                            }
                        }
                        else
                        {
                            dr["Fxyzf_typeStr"] = "否";
                        }

                        dr["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
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
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString() + ", stacktrace" + eSys.StackTrace);
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (this.tbx_uid.Text.Trim() != "" || this.txtQQ.Text.Trim() != "")
            {
                BindData(1);
            }
            else
            {
                BindData_UIN(1);
            }
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
            //20130828 lxl 往回翻页有问题修复
            string qqid = Session["qqid"].ToString();

            this.BindData(qqid, e.NewPageIndex);
        }

        private void pager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            this.BindData_UIN(e.NewPageIndex);
        }

        private void Datagrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "query")
            {
                this.BindData(e.Item.Cells[0].Text.Trim(), 1);
            }
        }
    }
}
