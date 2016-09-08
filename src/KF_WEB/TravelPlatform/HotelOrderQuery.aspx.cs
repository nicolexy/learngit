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
    public partial class HotelOrderQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Value = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                    TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
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
                begindate = DateTime.Parse(TextBoxBeginDate.Value);
                enddate = DateTime.Parse(TextBoxEndDate.Value);
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

        protected void BindData(int index)
        {
            try
            {
                pager.CurrentPageIndex = index;
                int limit = pager.PageSize;
                int page_id = index;
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                string uin = this.TextUin.Text.Trim();//财付通账号
                string hotelName = this.TextHotelName.Text.Trim();//保单号
                string start_time = DateTime.Parse(TextBoxBeginDate.Value).ToString("yyyy-MM-dd");//订购开始时间
                string end_time = DateTime.Parse(TextBoxEndDate.Value).ToString("yyyy-MM-dd");//订购结束时间
                DataSet ds = qs.HotelOrderQuery(uin, hotelName, start_time, end_time, limit, page_id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("PayAmt_str", typeof(String));
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "PayAmt", "PayAmt_str");
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
