using CFT.CSOMS.BLL.TravelModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Schema;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;

namespace TENCENT.OSS.CFT.KF.KF_Web.TravelPlatform
{
    /// <summary>
    /// PayBusinessQuery 的摘要说明。
    /// </summary>
    public partial class AirTicketsOrderQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        public bool HideDivDisplay = false;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

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
            this.dgList.ItemCommand += dgList_ItemCommand;
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

        void dgList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                var ds = ViewState["Cache"] as DataSet;
                HideDivDisplay = true;
                var orow = ds.Tables[0].Rows[e.Item.ItemIndex];

                #region 数据绑定
                var dataView = ds.Tables[0].AsEnumerable().Where(u => u["listid"] == orow["listid"]).AsDataView();
                Flights_Repeater.DataSource = dataView;
                Flights_Repeater.DataBind();
                OrderInfo_Repeater.DataSource = dataView;
                OrderInfo_Repeater.DataBind();
                Passengers_Repeater.DataSource = ds.Tables["passengers"].AsEnumerable().Where(u => u["listid"].ToString().Trim() == orow["listid"].ToString().Trim()).AsDataView();
                Passengers_Repeater.DataBind();
                #endregion

                #region 联系人信息
                var row = ds.Tables["contact"].Select("listid='" + orow["listid"] + "'")[0];
                contact_email.InnerText = row["email"].ToString();
                contact_mobile.InnerText = row["mobile"].ToString();
                contact_name.InnerText = row["name"].ToString();
                contact_telephone.InnerText = row["telephone"].ToString();
                #endregion

                var mailtype = row["mailtype"].ToString();
                Journey.InnerText = mailtype == "" ? "不要行程单" : mailtype == "1" ? "邮寄获取" : mailtype == "2" ? "自助获取行程单" : "其他";

                #region 备注
                var peopleNum = Convert.ToInt16(orow["adult_num"]) + Convert.ToInt16(orow["child_num"]);
                var totalMoney = orow["total_money_str"];
                var airporttax_money = orow["airport_tax_money_str"];
                var fuel_tax_money = orow["fuel_tax_money_str"];
                var ticket_money = orow["ticket_money_str"];
                var insurance_money = orow["insurance_money_str"];
                remark.InnerText = string.Format("共{0}人乘机，本次订单合计金额：{1}元（其中机票{2}元，机建{3}元，燃油费{4}元，保险费{5}元）", peopleNum, totalMoney, ticket_money, airporttax_money, fuel_tax_money, insurance_money);
                showHint.Visible = !(orow["insurance_orderid"].ToString() == "无");
                #endregion
            }
        }

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

        private void BindData(int index)
        {
            int queryType;
            AirTickets bll = new AirTickets();
            try
            {
                var start_time = DateTime.Parse(TextBoxBeginDate.Value); //订购开始时间
                var end_time = DateTime.Parse(TextBoxEndDate.Value);     //订购结束时间
                var trade_type = this.ddlState.SelectedValue;           //订单状态     
                var uin = this.TextUin.Text.Trim();
                var wd = bll.GetKeyWordAndType(out queryType, TextSppreno, TextTicketno, TextTransaction_id, TextPassenger_name, TextCert_id, TextMobile, TextInsur_no, TextName);
                DataSet ds;
                if (string.IsNullOrEmpty(wd))
                {
                    if (uin == "")
                    {
                        WebUtils.ShowMessage(this.Page, "最少输入一个关键信息,进行查询");
                        return;
                    }
                    ds = bll.AirTicketsOrderQueryByUin(uin, trade_type, start_time, end_time, "kf", pager.PageSize, index);
                }
                else
                {
                    ds = bll.AirTicketsOrderQuery(queryType, wd, trade_type, uin, start_time, end_time, "kf", pager.PageSize, index);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dgList.DataSource = ds.Tables[0].DefaultView;
                    foreach (DataRow item in ds.Tables["passengers"].Rows)
                    {
                        var cert_id = item["cert_id"].ToString();
                        var cert_type = item["cert_type"].ToString();
                        if (cert_type == "NI") //身份证
                        {
                            item["cert_id"] = setConfig.ConvertID(cert_id, 10, 4);
                        }
                        else if (cert_type == "PP") //护照
                        {
                            item["cert_id"] = setConfig.ConvertID(cert_id, 4, 2);
                        }
                    }
                    dgList.DataBind();
                    ViewState["Cache"] = ds;
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
                WebUtils.ShowMessage(this.Page, eSys.ToString());
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }

        }

    }
}
