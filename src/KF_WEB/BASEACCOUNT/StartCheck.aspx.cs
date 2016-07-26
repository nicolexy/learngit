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
using TENCENT.OSS.C2C.Finance.DataAccess;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// StartCheck 的摘要说明。
    /// </summary>
    public partial class StartCheck : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected System.Web.UI.WebControls.LinkButton lkUnchecked;
        protected System.Web.UI.WebControls.DataGrid dgCheck;
        protected System.Data.DataSet dscheck;
        protected System.Data.DataTable dtcheck;
        protected System.Web.UI.WebControls.Label lbTask;
        protected System.Web.UI.WebControls.LinkButton lkChecked;

        public string iFramePath, iFrameHeight;
        public string signShow, exedSign, exeShow, sign;
        protected System.Web.UI.WebControls.DropDownList dlCheckType;
        protected System.Web.UI.WebControls.DataGrid dgCheckLog;
        protected System.Web.UI.WebControls.Label Label2;
        protected System.Web.UI.WebControls.LinkButton lkShow;
        protected Wuqi.Webdiyer.AspNetPager AspNetPager1;
        protected Wuqi.Webdiyer.AspNetPager pager;
        protected System.Web.UI.WebControls.Label lbInfo;
        protected System.Data.DataTable dtLog;

        private void Page_Load(object sender, System.EventArgs e)
        {
         
            // 在此处放置用户代码以初始化页面
            try
            {

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
                   
            if (!Page.IsPostBack)
            {
                exeShow = "false";  //是否执行

                //初始化审批分类
                Hashtable ht = getCheckType();
                
                foreach (string s in ht.Keys)
                {
                    dlCheckType.Items.Add(new ListItem(ht[s].ToString(), s));
                }
                this.dlCheckType.SelectedValue = "0";
                dlCheckType.DataBind();
                BindInfo();
            }
        }

        private static Hashtable getCheckType()
        {
            Check_Service cs = new Check_Service();
            DataSet ds = cs.getCheckType();
            Hashtable ht = new Hashtable();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    ht.Add(dr["ftypeid"].ToString(), dr["ftypeName"].ToString());
                }
            }
            return ht;
        }

        public void BindUncheck(int pageIndex)
        {
            //判断是否登陆
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //重新登陆
            }

            int iStr, iMax;
            iMax = pager.PageSize; //每页显示
            iStr = (pageIndex - 1) * iMax + 1;  //初始索引
            pager.RecordCount = Int32.Parse(ViewState["uncheckNum"].ToString());  //记录条数

            //数据绑定处理
            Check_Service cs = new Check_Service();
            this.dtcheck = cs.GetStartCheckData(this.dlCheckType.SelectedValue, Session["uid"].ToString().Trim().ToLower(), iStr, iMax, "");

            if (dtcheck.Rows.Count == 0 || dtcheck == null)
            {
                dgCheck.Visible = false;
                this.lbTask.Visible = true;
                this.lbTask.Text = "当前没有待审批任务！";
            }
            else
            {
                dgCheck.Visible = true;
                this.lbTask.Visible = false;
            }
            iFrameHeight = "10";

            ViewState["sign"] = "uncheck";
            dgCheck.DataBind();
        }

        public void BindChecked(int pageIndex)
        {

            //判断是否登陆
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //重新登陆
            }

            int iStr, iMax;

            iMax = pager.PageSize; //每页显示
            iStr = (pageIndex - 1) * iMax + 1;  //初始索引

            pager.RecordCount = Int32.Parse(ViewState["checkedNum"].ToString());  //记录条数

            //数据绑定处理
            Check_Service cs = new Check_Service();
            this.dtcheck = cs.GetFinishCheckData(this.dlCheckType.SelectedValue, Session["uid"].ToString().Trim().ToLower(), iStr, iMax, "");

            if (dtcheck.Rows.Count == 0 || dtcheck == null)
            {
                dgCheck.Visible = false;
                this.lbTask.Visible = true;
                this.lbTask.Text = "当前没有已审批任务！";
            }
            else
            {
                //邦定数据的时候，读取状态
                exedSign = dtcheck.Rows[0]["Fstate"].ToString().Trim();

                dgCheck.Visible = true;
                this.lbTask.Visible = false;
            }
            iFrameHeight = "10";
            ViewState["sign"] = "checked";
            dgCheck.DataBind();
        }

        public void BindInfo()
        {
            int uncheckNum, checkedNum;
            string selectType = this.dlCheckType.SelectedValue;
            string uid = Session["uid"].ToString();

            Check_Service cs = new Check_Service();
       
            uncheckNum =cs.GetStartCheckCount(selectType, uid);
            checkedNum = cs.GetFinishCheckCount(selectType, uid);
            ViewState["uncheckNum"] = uncheckNum;
            ViewState["checkedNum"] = checkedNum;
            this.lbInfo.Text = "今天是：" + DateTime.Now.ToString("yyyy年MM月dd日") + " 您当前有" + this.dlCheckType.SelectedItem.Text + uncheckNum.ToString() + "件待审批," + checkedNum.ToString() + "件通过审批," + cs.GetToDoNum(selectType, Session["uid"].ToString().Trim()) + "件待执行。";
            iFrameHeight = "0";

            //绑定相关信息
            this.lkShow.Visible = false;
            ViewState["signShow"] = true;

            if (ViewState["sign"] == null)
                ViewState["sign"] = "uncheck";

            if (ViewState["sign"].ToString() == "uncheck")
                BindUncheck(1); //默认绑定第一页
            else if (ViewState["sign"].ToString() == "checked")
                BindChecked(1); //默认绑定第一页

            if (dtcheck.Rows.Count == 0 || dtcheck == null)
            {
                dgCheck.Visible = false;
                this.lbTask.Visible = true;
                this.lbTask.Text = "当前没有任务！";
                iFrameHeight = "10";
            }
        }

        /// <summary>
        /// 跳转获取详细信息，并且保存id值，供其它使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void detailCheck(Object sender, CommandEventArgs e)  //oncommand
        {

            //每次点击，刷新服务器，需要重新绑定
            if (ViewState["sign"].ToString() == "uncheck")
                BindUncheck(pager.CurrentPageIndex);
            else
                BindChecked(pager.CurrentPageIndex);

            //置收缩/展开状态为true;
            //			lkShow.Visible= true;
            this.lkShow.Text = "- 收缩";
            ViewState["signShow"] = true;

            string id = e.CommandArgument.ToString();
            ViewState["id"] = id;
            iFramePath = "detailCheck.aspx?id=" + id + "&type=" + e.CommandName + "&sign=" + ViewState["sign"].ToString() + "&right=false";
            if (ViewState["sign"].ToString() == "uncheck")
            {
                iFrameHeight = "140";  //设置Iframe高度
            }
            else
            {
                iFrameHeight = "140";
            }

            //绑定日志
            dtLog = GetCheckLog(1, 10);

            sign = ViewState["sign"].ToString();  //审批过还是未审批的标识

            dgCheck.DataBind();
            dgCheckLog.DataBind();
        }

        public void detailPage(Object sender, CommandEventArgs e)
        {
            string type = e.CommandName.ToString().Trim();
            string objID = e.CommandArgument.ToString().Trim();

            if (type == "batchpay")
                Response.Redirect("../BatchPay/ShowDetail.Aspx?BatchID=" + objID + "&pos=check ");
            else
                Response.Redirect("../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=" + objID + "&pos=check");
        }


        public void executeTask(Object sender, CommandEventArgs e)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //重新登陆
            }

            try
            {
                string checkID = e.CommandArgument.ToString().Trim();

                Check_WebService.Check_Service cw = new Check_WebService.Check_Service();
                Check_WebService.Finance_Header fh = new Check_WebService.Finance_Header();
                fh.UserName = Session["uid"].ToString();
                fh.UserPassword = Session["pwd"].ToString();
                fh.UserIP = Request.UserHostAddress;
                cw.Finance_HeaderValue = fh;

                cw.ExecuteCheck(e.CommandArgument.ToString());
                Response.Write("<script>alert('执行成功！');</script>");
                //				Response.Write("<script language=javascript>window.parent.location='DoCheck.Aspx'</script>");   //跳转到详细的任务界面
            }
            catch (Exception emsg)
            {
                //				Response.Write("<script>alert('执行失败！');</script>");
                WebUtils.ShowMessage(this.Page, "执行失败，详细：" + emsg.Message.ToString().Replace("'", "’"));
            }
            finally
            {
                //执行已审批的刷新
                lkShow.Visible = false;
                dtLog = null;
                dgCheckLog.DataBind();
                this.lkChecked.ForeColor = Color.Red;
                this.lkUnchecked.ForeColor = Color.Black;
                BindChecked(1);
                Page.DataBind();
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
            this.dscheck = new System.Data.DataSet();
            this.dtcheck = new System.Data.DataTable();
            this.dtLog = new System.Data.DataTable();
            ((System.ComponentModel.ISupportInitialize)(this.dscheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtcheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtLog)).BeginInit();
            this.lkUnchecked.Click += new System.EventHandler(this.lkUnchecked_Click);
            this.lkChecked.Click += new System.EventHandler(this.lkChecked_Click);
            this.dlCheckType.SelectedIndexChanged += new System.EventHandler(this.dlCheckType_SelectedIndexChanged);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
            // 
            // dscheck
            // 
            this.dscheck.DataSetName = "NewDataSet";
            this.dscheck.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.dscheck.Tables.AddRange(new System.Data.DataTable[] {
																		 this.dtcheck,
																		 this.dtLog});
            // 
            // dtcheck
            // 
            this.dtcheck.TableName = "dtcheck";
            // 
            // dtLog
            // 
            this.dtLog.TableName = "dtLog";
            this.Load += new System.EventHandler(this.Page_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dscheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtcheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtLog)).EndInit();

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;

            if (ViewState["sign"].ToString() == "uncheck")
                BindUncheck(pager.CurrentPageIndex);
            else
                BindChecked(pager.CurrentPageIndex);

            sign = ViewState["sign"].ToString();  //审批过还是未审批的标识
            dgCheck.DataBind();
        }

        private void lkUnchecked_Click(object sender, System.EventArgs e)
        {
            exeShow = "false"; //是否显示要执行的按钮 重要！！

            lkShow.Visible = false;

            //首先清空log的数据
            dtLog = null;
            dgCheckLog.DataBind();

            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时！ 请重新登陆。");
                Response.Redirect("../login.aspx?wh=1");
            }

            this.lkChecked.ForeColor = Color.Black;
            this.lkUnchecked.ForeColor = Color.Red;

            BindUncheck(1); //默认绑定第一页 

            sign = ViewState["sign"].ToString();  //审批过还是未审批的标识

        }

        private void lkChecked_Click(object sender, System.EventArgs e)
        {
            lkCheckedClick();
        }

        public void lkCheckedClick()
        {
            exeShow = "true"; //是否显示要执行的按钮 重要！！

            lkShow.Visible = false;

            //首先清空log的数据
            dtLog = null;
            dgCheckLog.DataBind();

            this.lkChecked.ForeColor = Color.Red;
            this.lkUnchecked.ForeColor = Color.Black;

            BindChecked(1);

            sign = ViewState["sign"].ToString();  //审批过还是未审批的标识
        }

        /// <summary>
        /// 读取审批日志
        /// </summary>
        /// <returns></returns>
        private DataTable GetCheckLog(int iStartIndex, int iRecordCount)
        {
            DataTable dt = null;
            if (ViewState["id"] != null)
            {
                try
                {
                    Check_Service cs = new Check_Service();
                    dt = cs.GetCheckLog(ViewState["id"].ToString(),iStartIndex, iRecordCount);
                        return dt;
                }
                catch (Exception ex)
                {
                    WebUtils.ShowMessage(this.Page, ex.Message.ToString());
                    return dt;

                }
               
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "超时！ 请重新登陆。");
                return dt;
            }

        }    

        private void lkCheckLog_Click(object sender, System.EventArgs e)
        {
            //			dtLog = GetCheckLog(1,6);
        }

        private void LinkButton2_Click(object sender, System.EventArgs e)
        {
            if ((bool)ViewState["signShow"] == true)
            {
                this.lkShow.Text = "+ 展开";
                //收缩，并置状态为false
                dtcheck = null;
                dgCheck.DataBind();
                iFramePath = "detailCheck.aspx?id=" + ViewState["id"] + "&type=" + this.dlCheckType.SelectedValue + "&sign=" + ViewState["sign"].ToString() + "&right=false";
                ViewState["signShow"] = false;
            }
            else
            {
                if (ViewState["sign"].ToString() == "uncheck")
                    BindUncheck(pager.CurrentPageIndex);
                else
                    BindChecked(pager.CurrentPageIndex);

                this.lkShow.Text = "- 收缩";
                //展开，并置状态为true
                iFramePath = "detailCheck.aspx?id=" + ViewState["id"] + "&type=" + this.dlCheckType.SelectedValue + "&sign=" + ViewState["sign"].ToString() + "&right=false";
                ViewState["signShow"] = true;
            }
        }


        public string covert(string str)
        {
            if (str == "<font color = red>审批完成</font>")
            {
                exedSign = "No";
                return "点击执行";
            }
            else if (str == "已执行")
            {
                exedSign = "Yes";
                return "已执行";
            }
            else
            {
                return str;
            }
        }

       

        private void dlCheckType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindInfo();
        }
    }
}
