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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using System.Reflection;
using Wuqi.Webdiyer;
using System.Configuration;
using CFT.CSOMS.BLL.TradeModule;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// TradeLog 的摘要说明。
    /// </summary>
    public partial class ManJianUsingQuery : System.Web.UI.Page
    {
        protected System.Data.DataSet DS_TradeLog;
        protected System.Data.DataTable dataTable1;
        protected System.Data.DataColumn Fuid;
        protected System.Data.DataColumn FListID;
        protected System.Data.DataColumn dataColumn1;
        protected System.Data.DataColumn dataColumn2;
        DateTime dt;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            // 在此处放置用户代码以初始化页面

            if (!IsPostBack)
            {
                string user = "buy";
                ViewState["user"] = user;
                DateTime now = DateTime.Now;
                DateTime d1 = new DateTime(now.Year, now.Month, 1);
                TextBoxBeginDate.Text = d1.ToString("yyyy年MM月dd日");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                BindBankType(ddlBankType);
            }
        }

        public void Item_DataBound(Object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||

                e.Item.ItemType == ListItemType.AlternatingItem)

                e.Item.Cells[1].Text = "<nobr>" + e.Item.Cells[1].Text + "</nobr>";
        }

        private void ValidateDate()
        {
            DateTime begindate;
            DateTime enddate;
            try
            {
                begindate = DateTime.Parse(TextBoxBeginDate.Text);
                enddate = DateTime.Parse(TextBoxEndDate.Text);
                ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
                ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
                ViewState["banktype"] = ddlBankType.SelectedValue;
            }
            catch
            {
                throw new Exception("日期输入有误！");
            }
            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("终止日期小于起始日期，请重新输入！");
            }
        }

        void BindBankType(DropDownList ddl)
        {
            ddl.Items.Add(new ListItem("所有银行", ""));
            ddl.Items.Add(new ListItem("招商银行", "1001"));
            ddl.Items.Add(new ListItem("工商银行", "1002"));
            ddl.Items.Add(new ListItem("工行信用卡", "1050"));
            ddl.Items.Add(new ListItem("建设银行", "1003"));
            ddl.Items.Add(new ListItem("浦发银行", "1004"));
            ddl.Items.Add(new ListItem("农业银行", "1005"));
            ddl.Items.Add(new ListItem("民生银行", "1006"));
            ddl.Items.Add(new ListItem("农行国际卡", "1007"));
            ddl.Items.Add(new ListItem("深圳发展银行", "1008"));
            ddl.Items.Add(new ListItem("兴业银行", "1009"));
            ddl.Items.Add(new ListItem("深圳商业银行", "1010"));
            ddl.Items.Add(new ListItem("中国交通银行", "1020"));
            ddl.Items.Add(new ListItem("中信实业银行", "1021"));
            ddl.Items.Add(new ListItem("中国光大银行", "1022"));
            ddl.Items.Add(new ListItem("农村合作信用社", "1023"));
            ddl.Items.Add(new ListItem("上海银行", "1024"));
            ddl.Items.Add(new ListItem("华夏银行", "1025"));
            ddl.Items.Add(new ListItem("中国银行", "1026"));
            ddl.Items.Add(new ListItem("广东发展银行", "1027"));
            ddl.Items.Add(new ListItem("广东银联", "1028"));
            ddl.Items.Add(new ListItem("建行B2B", "1040"));
            ddl.Items.Add(new ListItem("民生借记卡", "1041"));
            ddl.Items.Add(new ListItem("招行B2B", "1042"));
            ddl.Items.Add(new ListItem("中信银行", "1044"));
            ddl.Items.Add(new ListItem("其他银行", "1099"));
        }

        private DateTime SetTime(string aa)  //时间转换函数
        {

            //初始化时间	

            dt = new DateTime(int.Parse(aa.Substring(0, 4)), int.Parse(aa.Substring(4, 2)), int.Parse(aa.Substring(6, 2)), int.Parse(aa.Substring(8, 2)), int.Parse(aa.Substring(10, 2)), int.Parse(aa.Substring(12, 2)));

            return dt;

        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
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
            if (this.txtQQ.Text.Trim() == "" || this.txtQQ.Text.Trim() == null)
            {
                WebUtils.ShowMessage(this.Page, "请输入财付通账号！");
                return;
            }
            try
            {
                BindData(0, 1);
                //  this.DataGrid2.Visible = true;
                //  Page.DataBind();
            }
            catch (Exception eSys)
            {
                this.DataGrid2.Visible = false;
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message);
            }
        }


        private void BindData(int i, int pageIndex)
        {

            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
                Response.Redirect("../login.aspx?wh=1");
            }

            else
            {
                try
                {
                    string selectStr = this.txtQQ.Text.Trim();
                    DateTime beginTime = (DateTime)ViewState["begindate"];
                    DateTime endTime = (DateTime)ViewState["enddate"];
                    string banktype = ViewState["banktype"].ToString();
                    int pageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());
                    int istr = 1 + pageSize * (pageIndex - 1);   //1表示第一页
                    int imax = pageSize;
                    #region old
                    //Query_Service.Query_Service myService = new Query_Service.Query_Service();
                    //myService.Finance_HeaderValue = classLibrary.setConfig.setFH(Session["uid"].ToString(), Request.UserHostAddress);
                    //DS_TradeLog = myService.GetManJianUsingList(selectStr, i, beginTime, endTime, banktype, istr, imax);
                    #endregion
                    DS_TradeLog = new TradeService().GetManJianUsingList(selectStr, i, beginTime, endTime, banktype, istr, imax);
                    int total;
                    if (DS_TradeLog != null && DS_TradeLog.Tables.Count != 0 && DS_TradeLog.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow row in DS_TradeLog.Tables[0].Rows)
                        {
                            bool isC2C = false;
                            int type = 0;
                            if (int.TryParse(row["Ftrade_type"].ToString(), out type))
                            {
                                if (type == 1)
                                {
                                    isC2C = true;
                                }
                            }
                            if (isC2C)
                            {
                                row["Fstate"] = 100;
                            }
                        }
                        total = Int32.Parse(DS_TradeLog.Tables[0].Rows[0]["total"].ToString().Trim());
                        pager.RecordCount = total;
                        pager.PageSize = pageSize;
                        pager.CustomInfoText = "记录总数：<font color=\"blue\"><b>" + pager.RecordCount.ToString() + "</b></font>";
                        pager.CustomInfoText += " 总页数：<font color=\"blue\"><b>" + pager.PageCount.ToString() + "</b></font>";
                        pager.CustomInfoText += " 当前页：<font color=\"red\"><b>" + pager.CurrentPageIndex.ToString() + "</b></font>";
                        this.DataGrid2.Visible = true;
                        Page.DataBind();
                    }
                    else
                    {
                        this.DataGrid2.Visible = false;
                        WebUtils.ShowMessage(this.Page, "没有查询到记录！");
                    }
                }
                catch (SoapException eSoap) //捕获soap类
                {
                    string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                    WebUtils.ShowMessage(this.Page, "查询错误，详细：" + str);
                }
                catch (Exception e)
                {
                    Response.Write("<font color=red>查询错误，详细：" + e.Message + "</font>");
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
            this.DS_TradeLog = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.Fuid = new System.Data.DataColumn();
            this.FListID = new System.Data.DataColumn();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DS_TradeLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();

            // 
            // DS_TradeLog
            // 
            this.DS_TradeLog.DataSetName = "NewDataSet";
            this.DS_TradeLog.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.DS_TradeLog.Tables.AddRange(new System.Data.DataTable[] { this.dataTable1 });
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] { this.Fuid, this.FListID, this.dataColumn1, this.dataColumn2 });
            this.dataTable1.TableName = "Table1";
            // 
            // Fuid
            // 
            this.Fuid.ColumnName = "Fuid";
            // 
            // FListID
            // 
            this.FListID.ColumnName = "FlistID";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "Column1";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "Column2";
            ((System.ComponentModel.ISupportInitialize)(this.DS_TradeLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
        }

        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(0, pager.CurrentPageIndex);
            Page.DataBind();
        }

    }

}

