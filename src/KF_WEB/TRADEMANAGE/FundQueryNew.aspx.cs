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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.CSOMS.BLL.TradeModule;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// FundQuery 的摘要说明。
    /// </summary>
    public partial class FundQueryNew : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        public string begintime = DateTime.Now.ToString("yyyy-MM-dd");
        public string endtime = DateTime.Now.ToString("yyyy-MM-dd");

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //权限验证
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                // if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FundQuery")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            Label8.Text = "充值查询";
            if (Request.QueryString["fcurtype"] != null)//rowena 20100722  增加基金项目
            {
                Label8.Text = "基金充值查询";
            }


            //如果是充值单，需要判断是否为空，并且只能够为数字
            if (this.dpLst.SelectedValue == "czd")
            {
                this.revNumOnly.Enabled = true;
                this.rfvNullCheck.Enabled = true;
            }
            else //如果是QQID,不验证。为空时，默认为当天的全部用户的信息
            {
                this.revNumOnly.Enabled = false;
                this.rfvNullCheck.Enabled = false;
            }

            // 在此处放置用户代码以初始化页面

            if (!IsPostBack)
            {
                TextBoxBeginDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

                classLibrary.setConfig.GetAllBankList(ddlBankType);
                ddlBankType.Items.Insert(0, new ListItem("所有银行", "0000"));

                Table2.Visible = false;
            }


            //如果是从其他页面跳转获取充值单的详情，则跳转
            if (Request.QueryString["czID"] != null)
            {
                if (Request.QueryString["from"] != null && Request.QueryString["from"].ToString() == "bankquery")
                {
                    this.dpLst.SelectedValue = "BankBack";
                }
                if (Request.QueryString["from"] != null && Request.QueryString["from"].ToString() == "toBank")
                {
                    this.dpLst.SelectedValue = "toBank";
                }
                else
                {
                    this.dpLst.SelectedValue = "czd";
                }

                tbQQID.Text = Request.QueryString["czID"].ToString().Trim();
                //				this.CheckBox1.Checked=true;
                TextBoxBeginDate.Text = "2009-01-01 00:00:00";
                TextBoxEndDate.Text = "2009-01-01 00:00:00";
                if (Request.QueryString["checkdate"] != null)
                {
                    string date = Request.QueryString["checkdate"].Trim();
                    date = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6, 2);
                    TextBoxBeginDate.Text = DateTime.Parse(date).ToString("yyyy-MM-dd 00:00:00");
                    TextBoxEndDate.Text = DateTime.Parse(date).ToString("yyyy-MM-dd 23:59:59");

                }
                clickEvent();
            }
            else
            {
                //初始化时间

                if (this.dpLst.SelectedValue == "qq")
                {
                    string sID = this.tbQQID.Text.Trim();  //由于充值单查询一开始只支持ＱＱ号查询，所以是txbox是 QQID,后来加入充值单ID
                    //clickEvent(sID);	
                    //furion 自动查询完全是浪费时间。20061201
                }
                else
                {
                    Table2.Visible = false;
                }
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

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                Table2.Visible = true;
                pager.CurrentPageIndex = e.NewPageIndex;
                DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
                string newczdate = PublicRes.GetZWDicValue("OldOrderCZDataEndTime");
                DateTime dtnewcsdate = DateTime.Parse(newczdate);
                BindData(e.NewPageIndex);               
            }
            catch (LogicException lex)
            {
                LogError("TradeManage.FundQuery", "private void clickEvent(string ID),LogicException:", lex);
                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg(lex.Message.ToString()));
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogError("TradeManage.FundQuery", "private void clickEvent(string ID),SoapException:", eSoap);
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                LogError("TradeManage.FundQuery", "private void clickEvent(string ID),Exception:", eSys);
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + classLibrary.setConfig.replaceHtmlStr(eSys.Message));
            }
        }

        private void ValidateDate()
        {
            DateTime begindate;
            DateTime enddate;
            string u_ID = tbQQID.Text.Trim();
            try
            {
                begindate = DateTime.Parse(TextBoxBeginDate.Text);
                enddate = DateTime.Parse(TextBoxEndDate.Text);
            }
            catch
            {
                throw new Exception("日期输入有误！");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("终止日期小于起始日期，请重新输入！");
            }
            string newczdate = PublicRes.GetZWDicValue("OldOrderCZDataEndTime");
            if (newczdate == null || newczdate == "")
            {
                throw new Exception("未查询到OldOrderCZDataEndTime对应的配置值！");
            }

            if (this.dpLst.SelectedValue != "total")
            {
                if (string.IsNullOrEmpty(u_ID))
                    throw new Exception("不允许不输入单号或QQ号进行查询，请重新输入！");

                if (begindate.AddDays(15).CompareTo(enddate) < 0)
                    throw new Exception("选择时间段超过了十五天，请重新输入！");

                if (this.dpLst.SelectedValue.ToLower() == "tobank")
                {
                    if (begindate.Year != enddate.Year)
                        throw new Exception("给银行的订单号暂不支持跨年度查询，请重新输入！");
                }
            }
            else
            {
                if (begindate.AddDays(1).CompareTo(enddate) < 0)
                {
                    throw new Exception("选择时间段超过了一天，请重新输入！");
                }
            }
            DateTime dtnewcsdate = DateTime.Parse(newczdate);
            DateTime dtnewendate = dtnewcsdate.AddDays(-1);
            if (enddate.CompareTo(dtnewcsdate) >= 0 && begindate.CompareTo(dtnewcsdate) < 0)
            {
                string nenddate = dtnewendate.ToString("yyyy-MM-dd 23:59:59");
                TextBoxEndDate.Text = nenddate;
                throw new Exception("请以" + newczdate + "为开始日期或以" + nenddate + "结束日期!");
            }
            try
            {
                float tmp = float.Parse(tbFNum.Text.Trim());

                if (tmp > 2100000000)
                {
                    throw new Exception("金额最大为21000000元！不能超过该金额！");
                }
            }
            catch
            {
                throw new Exception("请输入正确的金额！");
            }

            ViewState["fnum"] = tbFNum.Text.Trim();
            ViewState["fnumMax"] = txbNumMax.Text.Trim();
            ViewState["fstate"] = ddlStateType.SelectedValue;

            ViewState["uid"] = classLibrary.setConfig.replaceSqlStr(u_ID);
            ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd HH:mm:ss"));
            begintime = begindate.ToString("yyyy-MM-dd");
            ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd HH:mm:ss"));
            endtime = enddate.ToString("yyyy-MM-dd");

            ViewState["sorttype"] = ddlSortType.SelectedValue;
            ViewState["querytype"] = this.dpLst.SelectedValue.Trim();

            //furion 20060324 增加银行查询条件
            ViewState["banktype"] = ddlBankType.SelectedValue;

            //furion 20050819 加入对SQL敏感字符的判断
            tbQQID.Text = classLibrary.setConfig.replaceSqlStr(u_ID);

        }

        protected void Button2_Click(object sender, System.EventArgs e)
        {
            //			//单击查询事件
            //			if(this.dpLst.SelectedValue == "czd")
            //			{
           // string sID = this.tbQQID.Text.Trim();  //由于充值单查询一开始只支持ＱＱ号查询，所以是txbox是 QQID,后来加入充值单ID
            clickEvent();
            //			}else if (this.dpLst.SelectedValue.ToLower() == "tobank") 	
        }

        /// <summary>
        /// 根据传入的ID进行充值单查询。
        /// </summary>
        /// <param name="ID"></param>
        private void clickEvent()
        {
            Table2.Visible = true;

            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);

            try
            {
                ValidateDate();

                if (this.dpLst.SelectedValue == "czd" || this.dpLst.SelectedValue == "toBank" || this.dpLst.SelectedValue == "BankBack")
                {
                    //默认指定下，不再查询笔数
                    pager.RecordCount = 9999;
                }
                else
                {
                    //			pager.RecordCount= GetCount(); 
                    pager.RecordCount = 999;
                }
                if (this.dpLst.SelectedValue == "total")
                {

                }
                else
                {
                    this.labCountNum.Text = "";
                    this.labAmount.Text = "";
                    this.labAmount.Visible = false;
                    this.labCountNum.Visible = false;
                    this.Label10.Visible = false;
                    this.Label9.Visible = false;

                }
            }
            catch (Exception err)
            {
                LogError("TradeManage.FundQuery", "private void clickEvent(string ID)", err);
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            Table2.Visible = true;
            try
            {
                BindData(1);
            }
            catch (LogicException lex)
            {
                LogError("TradeManage.FundQuery", "private void clickEvent(string ID),LogicException:", lex);
                string errStr = PublicRes.GetErrorMsg(lex.Message.ToString());
                WebUtils.ShowMessage(this.Page, errStr);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogError("TradeManage.FundQuery", "private void clickEvent(string ID),SoapException:", eSoap);
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                LogError("TradeManage.FundQuery", "private void clickEvent(string ID),Exception:", eSys);
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + classLibrary.setConfig.replaceHtmlStr(eSys.Message));
            }
        }

        private void BindData(int index)
        {
            string u_ID = ViewState["uid"].ToString(); ;
            DateTime begindate = (DateTime)ViewState["begindate"];
            DateTime enddate = (DateTime)ViewState["enddate"];

            string sorttype = ViewState["sorttype"].ToString();
            string queryType = ViewState["querytype"].ToString();

            begintime = begindate.ToString("yyyy-MM-dd");
            endtime = enddate.ToString("yyyy-MM-dd");

            float fnum = float.Parse(ViewState["fnum"].ToString());
            float fnumMax = float.Parse(ViewState["fnumMax"].ToString());

            int fstate = Int32.Parse(ViewState["fstate"].ToString());

            int max = pager.PageSize;
            int start = max * (index - 1) + 1;

            int newmax = pager.PageSize;
            int newstart = max * (index - 1);

            string banktype = ViewState["banktype"].ToString();


            int fcurtype = 1;

            if (Request.QueryString["fcurtype"] != null)//rowena 20100722  增加基金项目
            {
                fcurtype = int.Parse(Request.QueryString["fcurtype"].Trim());
            }
            DataSet ds = null;

            ds = new TradeService().GetBankRollListByListId(u_ID, queryType, fcurtype, begindate, enddate, fstate, fnum, fnumMax, banktype,  newstart, newmax);

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("FHistoryFlag", typeof(String));
                ds.Tables[0].Columns.Add("FNewNum", typeof(String));
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "FNum", "FNewNum");

                ds.Tables[0].Columns.Add("FbankName", typeof(String));
                classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Fbank_type", "FbankName", "BANK_TYPE");

                ds.Tables[0].Columns.Add("FStateName", typeof(String));
                classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Fsign", "FStateName", "TCLIST_SIGN");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr.BeginEdit();
                    dr["FHistoryFlag"] = "1";
                    dr.EndEdit();
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                throw new LogicException("没有找到记录！");
            }
        }


        //如果选择统计总数金额则隐藏输入框
        protected void dpLst_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.dpLst.SelectedValue == "total")
            {
                this.tbQQID.Visible = false;
            }
            else
            {
                this.tbQQID.Visible = true;
            }
        }
    }
}
