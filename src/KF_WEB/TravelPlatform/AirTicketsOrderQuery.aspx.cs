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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.TravelPlatform
{
    /// <summary>
    /// PayBusinessQuery 的摘要说明。
    /// </summary>
    public partial class AirTicketsOrderQuery : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Text = DateTime.Now.AddDays(-30).ToString("yyyy年MM月dd日");
                    TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            DateTime begindate;
            DateTime enddate;
            try
            {
                begindate = DateTime.Parse(TextBoxBeginDate.Text);
                enddate = DateTime.Parse(TextBoxEndDate.Text);
                if (begindate.CompareTo(enddate) > 0)
                {
                    WebUtils.ShowMessage(this.Page, "终止日期小于起始日期，请重新输入！");
                    return;
                }
            }
            catch
            {
                WebUtils.ShowMessage(this.Page, "日期输入有误！");
                return;
            }

            try
            {
                pager.RecordCount = 100000;
                BindData(1);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                //	this.dgList.Visible = false;
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                //	this.dgList.Visible = false;
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void BindData(int index)
        {
            try
            {
                pager.CurrentPageIndex = index;
                int limit = pager.PageSize;
                int page_id = index;
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                string sppreno = this.TextSppreno.Text.Trim();//票源订单号
                string ticketno = this.TextTicketno.Text.Trim();//票号
                string transaction_id = this.TextTransaction_id.Text.Trim();//财付通交易单号
                string passenger_name = this.TextPassenger_name.Text.Trim();//乘机人 姓名
                string cert_id = this.TextCert_id.Text.Trim();//乘机人 证件号码
                string name = this.TextName.Text.Trim();//联系人 姓名 
                string mobile = this.TextMobile.Text.Trim();//联系人 手机号码 
                string uin = this.TextUin.Text.Trim();//财付通账号
                string insur_no = this.TextInsur_no.Text.Trim();//保单号
                string start_time = DateTime.Parse(TextBoxBeginDate.Text).ToString("yyyy-MM-dd");//订购开始时间
                string end_time = DateTime.Parse(TextBoxEndDate.Text).ToString("yyyy-MM-dd");//订购结束时间
                string trade_type = this.ddlState.SelectedValue;//订单状态

                DataSet ds = qs.AirTicketsOrderQuery(sppreno, ticketno, transaction_id, passenger_name,
                 cert_id, name, mobile, uin, insur_no, start_time, end_time, trade_type, limit, page_id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("total_money_str", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "total_money", "total_money_str");
                    dgList.DataSource = ds.Tables[0].DefaultView;
                    dgList.DataBind();
                }
                else
                {
                    dgList.DataSource = null;
                    WebUtils.ShowMessage(this.Page, "没有找到记录！");
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }

        }
    }
}
