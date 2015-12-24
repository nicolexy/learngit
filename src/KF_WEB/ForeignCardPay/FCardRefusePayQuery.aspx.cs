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

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCardPay
{
    /// <summary>
    /// QueryYTTrade 的摘要说明。
    /// </summary>
    public partial class FCardRefusePayQuery : System.Web.UI.Page
    {
        public DateTime qbegindate, qenddate;
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
                this.btnQuery.Attributes.Add("onclick", "return CheckEmail();");
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
            string s_date = TextBoxBeginDate.Value;
            string e_date = TextBoxEndDate.Value;
            if (string.IsNullOrEmpty(s_date) || string.IsNullOrEmpty(e_date))
            {
                throw new Exception("请输入查询时间！");
            }

            try
            {
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
              
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
            if (spid == "" && spListID == "" && coding == "")
            {
                throw new Exception("请至少输入一个查询项！");
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
                this.pager.RecordCount = 1000;
                BindData(1);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
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

                int max = pager.PageSize;
                int start = max * (index - 1);
                TemplateControl temp = this;
                string ip = this.Page.Request.UserHostAddress;

                //根据订单查询拒付
                DataSet dsJF = new DataSet();
                dsJF = FCBLLService.QueryForeignCardRefusePayList(s_begindate, spid, coding, spListID, check_state.SelectedValue, sp_process_state.SelectedValue, s_begindate, s_enddate, start, max);
                if (dsJF == null || dsJF.Tables.Count < 1 || dsJF.Tables[0].Rows.Count < 1)
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    throw new Exception("数据库无此记录");
                }
                dsJF.Tables[0].Columns.Add("Fbank_req_refund_fee_str"); //查单金额
                dsJF.Tables[0].Columns.Add("Fbank_currency_freeze_fee_str"); //冻结金额
                dsJF.Tables[0].Columns.Add("Fbank_currency_refund_fee_str"); //拒付退款金额
                dsJF.Tables[0].Columns.Add("Fcheck_state_str"); //拒付状态
                dsJF.Tables[0].Columns.Add("Fsp_process_state_str"); //商户处理状态
                dsJF.Tables[0].Columns.Add("Frisk_process_state_str"); //风控处理状态
                classLibrary.setConfig.FenToYuan_Table(dsJF.Tables[0], "Fbank_req_refund_fee", "Fbank_req_refund_fee_str");
                classLibrary.setConfig.FenToYuan_Table(dsJF.Tables[0], "Fbank_currency_freeze_fee", "Fbank_currency_freeze_fee_str");
                classLibrary.setConfig.FenToYuan_Table(dsJF.Tables[0], "Fbank_currency_refund_fee", "Fbank_currency_refund_fee_str");
                foreach (DataRow row in dsJF.Tables[0].Rows)
                {
                    row["Fbank_req_refund_fee_str"] = row["Fbank_req_currency"].ToString() +" "+ row["Fbank_req_refund_fee_str"].ToString();
                    row["Fbank_currency_freeze_fee_str"] = row["Fbank_freeze_currency"].ToString() + " " + row["Fbank_currency_freeze_fee_str"].ToString();
                    row["Fbank_currency_refund_fee_str"] = row["Fbank_refund_currency"].ToString() + " " + row["Fbank_currency_refund_fee_str"].ToString();
                    string str = row["Fcheck_state"].ToString();//拒付状态
                    switch (str)
                    {
                        case "1":
                            row["Fcheck_state_str"] = "查单"; break;
                        case "2":
                            row["Fcheck_state_str"] = "拒付"; break;
                        case "3":
                            row["Fcheck_state_str"] = "预仲裁"; break;
                        case "4":
                            row["Fcheck_state_str"] = "仲裁"; break;
                        case "5":
                            row["Fcheck_state_str"] = "拒付退款"; break;
                        case "6":
                            row["Fcheck_state_str"] = "逾期解冻"; break;
                        default:
                            row["Fcheck_state_str"] = str; break;
                    }
                    str = row["Fsp_process_state"].ToString();//商户处理状态
                    switch (str)
                    {
                        case "1":
                            row["Fsp_process_state_str"] = "未处理"; break;
                        case "2":
                            row["Fsp_process_state_str"] = "已申诉"; break;
                        case "3":
                            row["Fsp_process_state_str"] = "同意拒付"; break;
                        case "4":
                            row["Fsp_process_state_str"] = "无需处理（已经退款）"; break;
                        case "5":
                            row["Fsp_process_state_str"] = "商户逾期转退款"; break;
                        default:
                            row["Fsp_process_state_str"] = str; break;
                    }
                    str = row["Frisk_process_state"].ToString();//风控处理状态
                    switch (str)
                    {
                        case "0":
                            row["Frisk_process_state_str"] = "未处理"; break;
                        case "1":
                            row["Frisk_process_state_str"] = "同意"; break;
                        case "2":
                            row["Frisk_process_state_str"] = "驳回"; break;
                        case "3":
                            row["Frisk_process_state_str"] = "驳回逾期转退款"; break;
                        default:
                            row["Frisk_process_state_str"] = str; break;
                    }

                }
                DataGrid1.DataSource = dsJF;
                DataGrid1.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }


    }
}