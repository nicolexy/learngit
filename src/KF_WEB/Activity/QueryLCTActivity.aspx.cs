using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.ActivityModule;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.KF_Web.Activity
{
    /// <summary>
    /// QueryLCTActivity 的摘要说明。
    /// </summary>
    public partial class QueryLCTActivity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
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
            DateTime begindate, enddate;

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
            string cft_no = txtCftNo.Text.Trim();

            if (cft_no == "")
            {
                throw new Exception("微信支付账号不能为空！");
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
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()) + ", stacktrace" + eSys.StackTrace);
            }
        }

        private void BindData(int index)
        {
            clearDT();
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

            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = null;

            ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, s_enddate);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["g_dt"] = ds.Tables[0];
                //理财通活动
                if (act_id.Equals("lct"))
                {
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                    DataGrid2.DataSource = null;
                    DataGrid2.DataBind();
                }
                //用户翻倍收益卡活动
                else if (act_id.Equals("userfbsyk"))
                {
                    DataGrid2.DataSource = ds.Tables[0].DefaultView;
                    DataGrid2.DataBind();
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                }
            }
            else
            {
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
                DataGrid2.DataSource = null;
                DataGrid2.DataBind();
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            int rid = e.Item.ItemIndex;
            GetDetail(rid);
        }

        private void GetDetail(int rid)
        {
            try{
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null && g_dt.Rows.Count > 0)
            {
                lb_ActId.Text = g_dt.Rows[rid]["FActionId"].ToString();//活动号
                lb_ActName.Text = g_dt.Rows[rid]["FActName"].ToString();//活动名称
                lb_TransId.Text = g_dt.Rows[rid]["FPriTransId"].ToString();//申购单号
                lb_State.Text = g_dt.Rows[rid]["FGiveStateStr"].ToString();//赠送状态
                lb_BatchId.Text = g_dt.Rows[rid]["FPrizeDesc"].ToString();//批次号
                lb_Spname.Text = g_dt.Rows[rid]["FspnameStr"].ToString();//申购基金
                lb_SendTicketTime.Text = g_dt.Rows[rid]["FPrizeModifyTime"].ToString();//发券时间
                lb_StartDate.Text = g_dt.Rows[rid]["FStartDate"].ToString();//第一个收益日期
                lb_CreateTime.Text = g_dt.Rows[rid]["FPrizeTime"].ToString();//奖品创建时间
                lb_ExpireTime.Text = g_dt.Rows[rid]["FPrizeExpiredTime"].ToString();//奖品失效时间
                lb_GivePosId.Text = g_dt.Rows[rid]["FGivePosId"].ToString();//赠送流水
                lb_Openid.Text = g_dt.Rows[rid]["FUin"].ToString();//openid
                lb_ErrorInfo.Text = g_dt.Rows[rid]["FErrInfo"].ToString();//错误信息
                //马里奥活动时候需要显示渠道号
                if (g_dt.Rows[rid]["FActionId"].ToString().Equals("20035"))
                {
                    //通过Uin找到UID，在通过UID找到FChannel_id
                    var uid = new Query_Service.Query_Service().Uid2QQ(g_dt.Rows[rid]["FUin"].ToString());
                    lb_ChannelId.Text = new ActivityService().GetChannelIDByFUid(uid);//渠道号
                }
                else
                {
                    lb_ChannelId.Text = string.Empty;
                }
                lb_FActType.Text = g_dt.Rows[rid]["FActTypeStr"].ToString();//活动分类
                lb_FPrizeType.Text = g_dt.Rows[rid]["FPrizeTypeStr"].ToString();//奖品类型
                lb_FPrizeName.Text = g_dt.Rows[rid]["FPrizeName"].ToString();//奖品名称
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()) + ", stacktrace" + eSys.StackTrace);
            }
       
        }

        private void clearDT()
        {
            lb_ActId.Text = "";
            lb_TransId.Text = "";
            lb_State.Text = "";
            lb_BatchId.Text = "";
            lb_Spname.Text = "";
            lb_SendTicketTime.Text = "";
            lb_StartDate.Text = "";
            lb_CreateTime.Text = "";
            lb_ExpireTime.Text = "";
            lb_GivePosId.Text = "";
            lb_Openid.Text = "";
            lb_ErrorInfo.Text = "";
            lb_ChannelId.Text = "";
            lb_FActType.Text = "";
            lb_FPrizeType.Text = "";
            lb_FPrizeName.Text = "";
        }
    }
}