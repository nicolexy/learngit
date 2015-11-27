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
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryYTFreeze 的摘要说明。
	/// </summary>
    public partial class QueryZHFreeze : System.Web.UI.Page
	{
        public DateTime qbegindate,qenddate;

        protected void Page_Load(object sender, System.EventArgs e)
		{
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    string sbegindate = Request.QueryString["qbegindate"];
                    if (sbegindate != null && sbegindate != "")
                    {
                        qbegindate = DateTime.Parse(sbegindate);
                        TextBoxBeginDate.Text = qbegindate.ToString("yyyy年MM月dd日");
                    }
                    else
                    {
                        qbegindate = DateTime.Now;
                        TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                    }
                    string senddate = Request.QueryString["qenddate"];
                    if (senddate != null && senddate != "")
                    {
                        qenddate = DateTime.Parse(senddate);
                        TextBoxEndDate.Text = qenddate.ToString("yyyy年MM月dd日");
                    }
                    else
                    {
                        qenddate = DateTime.Now;
                        TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                    }

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
			DateTime begindate,enddate;

			try
			{
                string s_date = TextBoxBeginDate.Text;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Text;
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
			}
			catch
			{
				throw new Exception("日期输入有误！");
			}

            string ccftno = cftNo.Text.ToString();
            string cytno = ytNo.Text.ToString();
            string ctradeno = tradeNo.Text.ToString();
         //   string cbuss_orderno = bussOrderNo.Text.ToString();
            if (ccftno == "" && cytno == "" && ctradeno == "")
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
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
                clearDT();
                this.pager.RecordCount = 1000;
                BindData(1);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {       
            int rid = e.Item.ItemIndex;     
            GetDetail(rid);      
        }

        private void GetDetail(int rid) 
        {
            //需要注意分页情况
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null)
            {
                lb_c1.Text = g_dt.Rows[rid]["Freal_name"].ToString();//真实姓名
                lb_c2.Text = g_dt.Rows[rid]["Fyt_no"].ToString();//运通账号
                lb_c3.Text = g_dt.Rows[rid]["Fcft_uin_str"].ToString();//财付通账号
                lb_c4.Text = g_dt.Rows[rid]["Fcreate_time"].ToString();//冻结时间
                lb_c5.Text = g_dt.Rows[rid]["Flistid"].ToString();//交易单号
                lb_c6.Text = g_dt.Rows[rid]["Ftid"].ToString();//商家订单号
                lb_c7.Text = g_dt.Rows[rid]["Fca_info"].ToString();//商家名称
                lb_c8.Text = "";//商品名称
                string curname = g_dt.Rows[rid]["Ftran_curtype"].ToString();
                if (curname != null && curname != "")
                {
                    curname = PublicRes.GetCurName(curname);
                }
                if (curname == "BIF" || curname == "CLP" || curname == "KMF" || curname == "DJF" || curname == "GNF" || curname == "JPY"
                    || curname == "KRW" || curname == "MGF" || curname == "VUV" || curname == "PYG" || curname == "RWF" || curname == "VND" || curname == "UYI"
                    || curname == "XAF" || curname == "XOF" || curname == "XPF" || curname == "BYR")
                {
                    lb_c9.Text = g_dt.Rows[rid]["Ftran_amt"].ToString() + "    " + curname;//原始订单金额
                }
                else {
                    lb_c9.Text = g_dt.Rows[rid]["Ftran_amt_str"].ToString() + "    " + curname;//原始订单金额
                }

                lb_c10.Text = g_dt.Rows[rid]["Frecon_amt_str"].ToString()+"    USD";//运通授权金额
                lb_c11.Text = g_dt.Rows[rid]["Ffreeze_amt_rmb_str"].ToString() + "    CNY";//初始冻结金额
                lb_c12.Text = g_dt.Rows[rid]["CurrFreeMoney_str"].ToString() + "    CNY";//当前冻结金额
                lb_c13.Text = g_dt.Rows[rid]["Fstatus_str"].ToString();//订单状态
                lb_c14.Text = g_dt.Rows[rid]["LastUnfreeTime"].ToString();//最近一次解冻时间
                lb_c15.Text = g_dt.Rows[rid]["unFreeTimes"].ToString();//解冻次数
                lb_c16.Text = "余额支付";//支付方式
                lb_c17.Text = g_dt.Rows[rid]["unFreeMoney_str"].ToString()+"元";//累计解冻金额
                lb_c18.Text = g_dt.Rows[rid]["FPay_str"].ToString() + "元";//累计请款金额
                lb_c19.Text = g_dt.Rows[rid]["LastUnfreeTime"].ToString();//最近请款时间
                lb_c20.Text = g_dt.Rows[rid]["Fmodify_time"].ToString();//最近更新时间
                lb_c21.Text = g_dt.Rows[rid]["Fapp_code"].ToString();//appcode
                lb_c22.Text = g_dt.Rows[rid]["FselfUnfree_str"].ToString();//自助申请提前解冻
            }
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

            lb_c15.Text = "";
            lb_c16.Text = "";
            lb_c17.Text = "";
            lb_c18.Text = "";
            lb_c19.Text = "";
            lb_c20.Text = "";
            lb_c21.Text = "";
            lb_c22.Text = "";
        }

        private void BindData(int index)
		{
            string s_stime = TextBoxBeginDate.Text;
            string s_begindate = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
            }
            string s_etime = TextBoxEndDate.Text;
            string s_enddate = "";
            if (s_etime != null && s_etime != "")
            {
                DateTime enddate = DateTime.Parse(s_etime);
                s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
            }

            string s_cftno = cftNo.Text.ToString();
            string s_ytno = ytNo.Text.ToString();
            string s_tradeno = tradeNo.Text.ToString();
          //  string s_bussorderno = bussOrderNo.Text.ToString();

            int max = pager.PageSize;
			int start = max * (index-1);

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = qs.GetYTFreezeList(s_cftno, s_ytno, s_tradeno, "", s_begindate, s_enddate, start, max, "2");
            if(ds != null && ds.Tables.Count >0)
            {
                ViewState["g_dt"] = ds.Tables[0];
                ds.Tables[0].Columns.Add("Ffreeze_amt_rmb_str", typeof(String));
                ds.Tables[0].Columns.Add("Ftran_amt_str", typeof(String));
                ds.Tables[0].Columns.Add("unFreeMoney_str", typeof(String));
                ds.Tables[0].Columns.Add("Frecon_amt_str", typeof(String));
                ds.Tables[0].Columns.Add("FPay_str", typeof(String));
                ds.Tables[0].Columns.Add("CurrFreeMoney_str", typeof(String));
                ds.Tables[0].Columns.Add("Fstatus_str", typeof(String));//状态

                ds.Tables[0].Columns.Add("Fcft_uin_str", typeof(String));//财付通账号
                ds.Tables[0].Columns.Add("Freal_name", typeof(String));//真实姓名
                ds.Tables[0].Columns.Add("Fyt_no", typeof(String));//运通账号
                ds.Tables[0].Columns.Add("FselfUnfree_str", typeof(String));//自助申请提前解冻
                
                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "初始化");
                ht1.Add("2", "授权成功");
                ht1.Add("3", "授权失败");
                ht1.Add("4", "授权撤销");
                ht1.Add("5", "失效");
                ht1.Add("6", "请款成功");
                ht1.Add("7", "部分请款成功");

                string yt_no = "";
                //通过财付通账号查询运通账号
                if (s_ytno != null && s_ytno != "")
                {
                    yt_no = s_ytno;
                }
            

                //通过uid获得真实姓名
                string qqid = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string FselfUnfree = dr["FselfUnfree"].ToString();
                    if (FselfUnfree == "1")
                    {
                        dr["FselfUnfree_str"] = "是";
                    }
                    else
                    {
                        dr["FselfUnfree_str"] = "否";
                    }

                    string uid = dr["Fuid"].ToString();
                    if (s_cftno != null && s_cftno != "")
                    {
                        //通过财付通账号去查询信息
                        qqid = s_cftno;
                    }
                    else if (uid != null && uid != "")
                    {
                        qqid = qs.Uid2QQ(uid);
                    }

                    DataSet ds_u = new DataSet();
                    if (qqid != null && qqid != "")
                    {

                        ds_u = new AccountService().GetUserInfo(qqid, 1, 1);
                    }
                    string true_name = "";
                    if (ds_u != null && ds_u.Tables.Count > 0)
                    {
                        //个人信息
                        true_name = ds_u.Tables[0].Rows[0]["Ftruename"].ToString();//姓名

                    }
                    if (yt_no == "") {
                        DataSet ht = qs.GetYTInfoList(qqid, "", "","2");
                        if (ht != null && ht.Tables.Count > 0)
                        {
                            yt_no = ht.Tables[0].Rows[0]["card_id"].ToString();
                        }
                    }
                    dr.BeginEdit();
                    dr["Freal_name"] = true_name;
                    dr["Fcft_uin_str"] = qqid;
                    dr["Fyt_no"] = classLibrary.setConfig.ConvertID(yt_no,6,5);
                    dr.EndEdit();
                }
                //获得真实姓名end...

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fstate", "Fstatus_str", ht1);
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ffreeze_amt_rmb", "Ffreeze_amt_rmb_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ftran_amt", "Ftran_amt_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "unFreeMoney", "unFreeMoney_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Frecon_amt", "Frecon_amt_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "FPay", "FPay_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "CurrFreeMoney", "CurrFreeMoney_str");

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                DataGrid1.DataSource = null;
                throw new LogicException("没有找到记录！");
            }
        }

	}
}