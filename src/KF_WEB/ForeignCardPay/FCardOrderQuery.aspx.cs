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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Xml;
using CFT.CSOMS.BLL.ForeignCardModule;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCardPay
{
    /// <summary>
    /// QueryYTTrade 的摘要说明。
    /// </summary>
    public partial class FCardOrderQuery : System.Web.UI.Page
    {
        protected ForeignCardService FCBLLService = new ForeignCardService();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                }
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        private void ValidateDate()
        {
            DateTime begindate = new DateTime(), enddate = new DateTime();

            try
            {
                string s_date = TextBoxBeginDate.Value;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Value;
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
            catch
            {
                throw new Exception("日期输入有误！");
            }
            if (begindate.Year != enddate.Year)
            {
                throw new Exception("请不要跨年查询！");
            }
            string spid = txtspid.Text.ToString();
            string spListID = txtspListID.Text.ToString();
            string coding = txtCoding.Text.ToString();
            string email = txtEmail.Text.ToString();
            if (string.IsNullOrEmpty(spid))
            {
                throw new Exception("请输入商户编号！");
            }
        }

        public void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                ButtonExport.Visible = false;
                clearDT();
                clearDTUser();
                this.tableOrder.Visible = false;
                this.tableUser.Visible = false;
                this.pager.RecordCount = 1000;
                BindData(1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void BindData(int index)
        {
            try
            {
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);
                DataSet dsOrder = QueryData(start,max);
                if (dsOrder == null || dsOrder.Tables.Count < 1 || dsOrder.Tables[0].Rows.Count < 1)
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    throw new Exception("数据库无此记录");
                }
                ButtonExport.Visible = true;
                ViewState["g_dt"] = dsOrder;
                DataGrid1.DataSource = dsOrder;
                DataGrid1.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }

        }

        private DataSet QueryData(int start, int max)
        {
            try
            {
                string s_stime = TextBoxBeginDate.Value;
                string s_begindate = "";
                if (s_stime != null && s_stime != "")
                {
                    DateTime begindate = DateTime.Parse(s_stime);
                    s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
                }
                string s_etime = TextBoxEndDate.Value;
                string s_enddate = "";
                if (s_etime != null && s_etime != "")
                {
                    DateTime enddate = DateTime.Parse(s_etime);
                    s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
                }

                string spid = txtspid.Text.ToString();
                string coding = txtCoding.Text.ToString();
                string spListID = txtspListID.Text.ToString();
                string email = txtEmail.Text.ToString();

                TemplateControl temp = this;
                string ip = this.Page.Request.UserHostAddress;

                DataSet dsOrder = new DataSet();
                string uid = ""; 
                if (!string.IsNullOrEmpty(spid))
                {//查询商户的内部ID
                    string msg = string.Empty;
                    Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    myService.Finance_HeaderValue = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.setFH(Session["uid"].ToString(), Request.UserHostAddress);
                    if(!myService.GetMerchantMidUid(spid, out uid, out msg))
                        throw new Exception(msg);
                }

                dsOrder = FCBLLService.QueryForeignCardOrderList(uid,spid, coding, spListID, email, s_begindate, s_enddate, ddl_state.SelectedValue, start, max);

                if (dsOrder == null || dsOrder.Tables.Count < 1 || dsOrder.Tables[0].Rows.Count < 1)
                    return null;

                dsOrder.Tables[0].Columns.Add("Fpaynum_str"); //币种+交易金额
                dsOrder.Tables[0].Columns.Add("Fpaynum_str_excel"); //用于excel导出去掉</br>
                dsOrder.Tables[0].Columns.Add("Ftrade_state_str"); //交易状态
                dsOrder.Tables[0].Columns.Add("FTK_str"); //是否退款
                dsOrder.Tables[0].Columns.Add("FZF_str"); //是否拒付
                dsOrder.Tables[0].Columns.Add("Frisk_level_str"); //是否拒付

                foreach (DataRow row in dsOrder.Tables[0].Rows)
                {
                    row["Fpaynum_str"] = row["Fsp_currency"].ToString() + " " + MoneyTransfer.FenToYuan(PublicRes.GetString(row["Fsp_currency_fee"]))
                        +"</br>"+ row["Fbank_currency"].ToString() + " " + MoneyTransfer.FenToYuan(PublicRes.GetString(row["Fbank_currency_fee"]));
                    row["Fpaynum_str_excel"] = row["Fpaynum_str"].ToString().Replace("</br>", " ");
                    string tmp = row["Ftrade_state"].ToString();
                    switch (tmp)
                    {
                        case "1":
                            row["Ftrade_state_str"] = "等待买家支付"; break;
                        case "2":
                            row["Ftrade_state_str"] = " 支付成功"; break;
                        case "3":
                            row["Ftrade_state_str"] = "待处理"; break;
                        case "4":
                            row["Ftrade_state_str"] = " 退款"; break;
                        default:
                            row["Ftrade_state_str"] = "未知"; break;
                    }
                    if (tmp == "4")
                        row["FTK_str"] = "是";
                    else
                        row["FTK_str"] = "否";
                    int n = Convert.ToInt32(row["Fcb_state"].ToString());
                    if (n>0)
                        row["FZF_str"] = "是";
                    else
                        row["FZF_str"] = "否";

                    tmp = row["Frisk_level"].ToString();
                    switch (tmp)
                    {
                        case "1":
                            row["Frisk_level_str"] = "通过"; break;
                        case "2":
                            row["Frisk_level_str"] = "拒绝"; break;
                        case "3":
                            row["Frisk_level_str"] = "风险待定"; break;
                        case "0":
                            row["Frisk_level_str"] = " 未定义"; break;
                        default:
                            row["Frisk_level_str"] = "未知"; break;
                    }
                }
                return dsOrder;
            }
            catch (Exception eSys)
            {
                throw new Exception(PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }

        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                int rid = e.Item.ItemIndex;
                GetDetail(rid);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void GetDetail(int rid)
        {
            try
            {
                this.tableOrder.Visible = true;
                this.tableUser.Visible = true;
                clearDT();
                clearDTUser();
                DataSet ds = (DataSet)ViewState["g_dt"];
                DataTable g_dt = ds.Tables[0];
                if (g_dt != null)
                {
                    lb_c1.Text = g_dt.Rows[rid]["Fspid"].ToString();//商户编号
                    lb_c2.Text = g_dt.Rows[rid]["Fcoding"].ToString();//商家订单号
                    lb_c3.Text = g_dt.Rows[rid]["Ftransaction_id"].ToString();//财付通订单号
                    lb_c4.Text = g_dt.Rows[rid]["Fbank_listid"].ToString();//给银行订单号
                    lb_c5.Text = g_dt.Rows[rid]["Fauth_code"].ToString();//银行授权号
                    lb_c6.Text = g_dt.Rows[rid]["Fmodify_time"].ToString();//交易时间
                    lb_c7.Text = g_dt.Rows[rid]["Fpaynum_str"].ToString(); //交易金额
                    lb_c8.Text = g_dt.Rows[rid]["Fuser_card_type"].ToString(); //交易卡种
                    lb_c9.Text = g_dt.Rows[rid]["Fuser_card_info"].ToString(); //卡号
                    lb_c10.Text = g_dt.Rows[rid]["Ftrade_state_str"].ToString();//交易状态
                    lb_c11.Text = g_dt.Rows[rid]["Frisk_level_str"].ToString();//风控状态
                    //拒付退款金额
                    lb_c12.Text = g_dt.Rows[rid]["Fbank_cb_currency"].ToString() + " " + MoneyTransfer.FenToYuan(PublicRes.GetString(g_dt.Rows[rid]["Fbank_cb_fee"].ToString()));
                    lb_c13.Text = g_dt.Rows[rid]["Fsp_currency_refund"].ToString() + " " + MoneyTransfer.FenToYuan(PublicRes.GetString(g_dt.Rows[rid]["Fsp_currency_refund_fee"].ToString()))
                      + "/" + g_dt.Rows[rid]["Fbank_refund_currency"].ToString() + " " + MoneyTransfer.FenToYuan(PublicRes.GetString(g_dt.Rows[rid]["Fbank_refund_fee"].ToString()));
                    string str = g_dt.Rows[rid]["Fcb_state"].ToString();//拒付状态
                    switch (str)
                    {
                        case "0":
                            lb_c14.Text = "正常"; break;
                        case "1":
                            lb_c14.Text = "查单"; break;
                        case "2":
                            lb_c14.Text = "拒付调查"; break;
                        case "3":
                            lb_c14.Text = "预仲裁"; break;
                        case "4":
                            lb_c14.Text = "仲裁"; break;
                        case "5":
                            lb_c14.Text = "拒付退款"; break;
                        case "6":
                            lb_c14.Text = "逾期解冻"; break;
                        default:
                            lb_c14.Text ="未知"+ str; break;
                    }

                    bb_c1.Text = g_dt.Rows[rid]["Ffirst_name"].ToString() + " " + g_dt.Rows[rid]["Fsecond_name"].ToString(); //持卡人姓名
                    bb_c2.Text = g_dt.Rows[rid]["Femail"].ToString();//邮箱
                    bb_c3.Text = g_dt.Rows[rid]["Fphone"].ToString();//电话
                    bb_c4.Text = g_dt.Rows[rid]["Fip"].ToString();//IP来源
                    bb_c5.Text = g_dt.Rows[rid]["Fbilling_address"].ToString();//国家/地区
                    bb_c6.Text = g_dt.Rows[rid]["Fzip_code"].ToString();//邮编
                }
            }
            catch (Exception eSys)
            {
                throw new Exception(PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        public void Export_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                ExportData();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void ExportData()
        {
            //DataSet dsOrder = QueryData(0, 1000);
            DataSet dsOrder = (DataSet)ViewState["g_dt"];
            if (dsOrder == null || dsOrder.Tables.Count < 1 || dsOrder.Tables[0].Rows.Count < 1)
            {
                throw new Exception("数据库无此记录");
            }

            StringWriter sw = new StringWriter();
            string excelHeader = DataGrid1.Columns[0].HeaderText;
            for (int i = 1; i < DataGrid1.Columns.Count; i++)
            {
                excelHeader += "\t" + DataGrid1.Columns[i].HeaderText;
            }
            sw.WriteLine(excelHeader);

            foreach (DataRow dr in dsOrder.Tables[0].Rows)
            {
                sw.WriteLine("=\"" + dr["Fspid"].ToString() + "\"\t=\"" + dr["Fcoding"].ToString() + "\"\t=\"" + dr["Ftransaction_id"]
                    + "\"\t" + dr["Fmodify_time"] + "\t=\"" + dr["Fpaynum_str_excel"] + "\"\t=\"" 
                    + dr["Ftrade_state_str"] + "\"\t=\"" + dr["Frisk_level_str"] + "\"\t=\"" +
                    dr["FTK_str"] + "\"\t=\"" + dr["FZF_str"] + "\"\t=\"" + dr["Fuser_card_type"]
                + "\"");

            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=订单查询.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();
        }
        private void clearDT()
        {
            lb_c1.Text = "";
            lb_c2.Text = "";
            lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";

            lb_c8.Text = "";
            lb_c9.Text = "";
            lb_c10.Text = "";
            lb_c11.Text = "";
            lb_c12.Text = "";
            lb_c13.Text = "";
            lb_c14.Text = "";
        }

        private void clearDTUser()
        {
            bb_c1.Text = "";
            bb_c2.Text = "";
            bb_c3.Text = "";
            bb_c4.Text = "";
            bb_c5.Text = "";
            bb_c6.Text = "";
            bb_c7.Text = "";
            bb_c8.Text = "";
        }

    }
}