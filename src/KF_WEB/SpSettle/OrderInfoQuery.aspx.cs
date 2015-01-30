using System;
using System.Collections;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
	/// <summary>
    /// OrderInfoQuery 的摘要说明。
	/// </summary>
    public partial class OrderInfoQuery : System.Web.UI.Page
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            // 在此处放置用户代码以初始化页面
            rq_tb2.Visible = false;
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
		}

        protected void Button_qry_Click(object sender, System.EventArgs e) 
        {
            try
            {
                string merge_listid = this.txtMergeListid.Text.Trim();
                string listid = this.txtListid.Text.Trim();

                if (merge_listid == "" && listid == "")
                    throw new Exception("请输入查询条件！");

                BindInfo(merge_listid, listid);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message.ToString());
            }
        }

        private void BindInfo(string merge_listid, string listid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds;
            ds = qs.QuerySubOrderList(merge_listid, listid);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                ViewState["g_dt"] = dt;

                dt.Columns.Add("Ftype_str", typeof(string)); //业务类型
                dt.Columns.Add("Fpay_type_str", typeof(string));//支付类型
                dt.Columns.Add("Fcurtype_str", typeof(string)); //币种
                dt.Columns.Add("Ftrade_state_str", typeof(string)); //交易状态
                dt.Columns.Add("Frefund_state_str", typeof(string)); //退款状态
                dt.Columns.Add("Flstate_str", typeof(string)); //订单状态
                dt.Columns.Add("Fchannel_id_str", typeof(string)); //渠道号

                dt.Columns.Add("Fprice_str", typeof(string)); //产品价格
                dt.Columns.Add("Fcarriage_str", typeof(string)); //物流费用
                dt.Columns.Add("Fpaynum_str", typeof(string)); //实际支付费用
                dt.Columns.Add("Ffact_str", typeof(string)); //总支付费用
                dt.Columns.Add("Fprocedure_str", typeof(string)); //交易手续费
                dt.Columns.Add("Fcash_str", typeof(string)); //现金支付金额
                dt.Columns.Add("Ftoken_str", typeof(string)); //代金券支付金额
                dt.Columns.Add("Ffee3_str", typeof(string)); //其它费用
                dt.Columns.Add("Fpaybuy_str", typeof(string)); //退还给买家金额
                dt.Columns.Add("Fpaysale_str", typeof(string)); //退还给卖家金额

                dt.Columns.Add("Fbuy_bank_type_str", typeof(string)); //买家开户行
                dt.Columns.Add("Fsale_bank_type_str", typeof(string)); //卖家开户行

                foreach (DataRow dr in ds.Tables[0].Rows) 
                {
                    dr["Fbuy_bank_type_str"] = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(dr["Fbuy_bank_type"].ToString());
                    dr["Fsale_bank_type_str"] = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(dr["Fsale_bank_type"].ToString());
                }
                
                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "补差支付");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Ftype", "Ftype_str", ht1);
                ht1.Clear();

                ht1.Add("1", "银行卡支付");
                ht1.Add("2", "财付通支付");
                ht1.Add("3", "一点通支付");
                ht1.Add("4", "信用卡支付");
                ht1.Add("5", "委托代扣");
                ht1.Add("6", "混合支付");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fpay_type", "Fpay_type_str", ht1);
                ht1.Clear();

                ht1.Add("1", "RMB");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcurtype", "Fcurtype_str", ht1);
                ht1.Clear();

                ht1.Add("1", "等待买家支付");
                ht1.Add("2", "支付成功/等待卖家发货");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Ftrade_state", "Ftrade_state_str", ht1);
                ht1.Clear();

                ht1.Add("0", "初始状态");
                ht1.Add("1", "退款请求");
                ht1.Add("2", "退款成功");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Frefund_state", "Frefund_state_str", ht1);
                ht1.Clear();

                ht1.Add("1", "锁定");
                ht1.Add("2", "正常");
                ht1.Add("3", "作废");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Flstate", "Flstate_str", ht1);
                ht1.Clear();

                ht1.Add("1", "财付通");
                ht1.Add("2", "拍拍网");
                ht1.Add("3", "客服端小钱包");
                ht1.Add("4", "手机支付");
                ht1.Add("5", "第三方");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fchannel_id", "Fchannel_id_str", ht1);
                ht1.Clear();

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fprice", "Fprice_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fcarriage", "Fcarriage_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "Fpaynum_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ffact", "Ffact_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fprocedure", "Fprocedure_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fcash", "Fcash_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ftoken", "Ftoken_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ffee3", "Ffee3_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaybuy", "Fpaybuy_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaysale", "Fpaysale_str");

                
                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else 
            {
                throw new Exception("没有记录！");
            }
            
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string listid = e.Item.Cells[0].Text.Trim(); //财付通订单号

            switch (e.CommandName)
            {
                case "DETAIL": //请求明细
                    int rid = e.Item.ItemIndex;
                    rq_tb2.Visible = true;
                    GetReqDetail(rid);
                    break;
            }
        }

        private void GetReqDetail(int rid) 
        {
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null) 
            {
                lb_c1.Text = g_dt.Rows[rid]["Fmerge_listid"].ToString();//原交易单
                lb_c2.Text = g_dt.Rows[rid]["Ftype_str"].ToString();//业务类型
                lb_c3.Text = g_dt.Rows[rid]["Flistid"].ToString();//补差支付单
                lb_c4.Text = g_dt.Rows[rid]["Fcoding"].ToString();//订单编码
                lb_c5.Text = g_dt.Rows[rid]["Fspid"].ToString();//商户号
                lb_c6.Text = g_dt.Rows[rid]["Fbank_listid"].ToString();//给银行的订单号
                lb_c7.Text = g_dt.Rows[rid]["Fbank_backid"].ToString();//银行返回的订单号
                lb_c8.Text = g_dt.Rows[rid]["Fpay_type_str"].ToString();//支付类型
                lb_c9.Text = g_dt.Rows[rid]["Fbuy_uid"].ToString();//买家内部ID
                lb_c10.Text = g_dt.Rows[rid]["Fbuyid"].ToString();//买家账户号

                lb_c11.Text = g_dt.Rows[rid]["Fbuy_name"].ToString();//付款方名称
                lb_c12.Text = g_dt.Rows[rid]["Fbuy_bank_type_str"].ToString();//买家开户行
                lb_c13.Text = g_dt.Rows[rid]["Fsale_uid"].ToString();//卖家内部ID
                lb_c14.Text = g_dt.Rows[rid]["Fsaleid"].ToString();//卖家账户号
                lb_c15.Text = g_dt.Rows[rid]["Fsale_name"].ToString();//卖家的名称
                lb_c16.Text = g_dt.Rows[rid]["Fsale_bank_type_str"].ToString();//卖家开户行
                lb_c17.Text = g_dt.Rows[rid]["Fcurtype_str"].ToString();//币种
                lb_c18.Text = g_dt.Rows[rid]["Ftrade_state_str"].ToString();//交易状态
                lb_c19.Text = g_dt.Rows[rid]["Frefund_state_str"].ToString();//退款/物流状态
                lb_c20.Text = g_dt.Rows[rid]["Flstate_str"].ToString();//订单状态

                lb_c21.Text = g_dt.Rows[rid]["Fprice_str"].ToString();//产品价格
                lb_c22.Text = g_dt.Rows[rid]["Fcarriage_str"].ToString();//物流费用
                lb_c23.Text = g_dt.Rows[rid]["Fpaynum_str"].ToString();//实际支付费用
                lb_c24.Text = g_dt.Rows[rid]["Ffact_str"].ToString();//总支付费用
                lb_c25.Text = g_dt.Rows[rid]["Fprocedure_str"].ToString();//交易手续费
                lb_c26.Text = g_dt.Rows[rid]["Fservice"].ToString();//服务费率
                lb_c27.Text = g_dt.Rows[rid]["Fcash_str"].ToString();//现金支付金额
                lb_c28.Text = g_dt.Rows[rid]["Ftoken_str"].ToString();//代金券支付金额
                lb_c29.Text = g_dt.Rows[rid]["Ffee3_str"].ToString();//其它费用

                lb_c30.Text = g_dt.Rows[rid]["Fcreate_time"].ToString();//订单创建时间
                lb_c31.Text = g_dt.Rows[rid]["Fpay_time"].ToString();//买家付款时间
                lb_c32.Text = g_dt.Rows[rid]["Fip"].ToString();//最后修改交易单的IP
                lb_c33.Text = g_dt.Rows[rid]["Fmemo"].ToString();//交易说明
                lb_c34.Text = g_dt.Rows[rid]["Fexplain"].ToString();//备注
                lb_c35.Text = g_dt.Rows[rid]["Fmodify_time"].ToString();//修改时间
                lb_c36.Text = g_dt.Rows[rid]["Fchannel_id_str"].ToString();//渠道
                lb_c37.Text = g_dt.Rows[rid]["Fpaybuy_str"].ToString();//退还给买家的金额
                lb_c38.Text = g_dt.Rows[rid]["Fpaysale_str"].ToString();//退还给卖家的金额
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
            lb_c23.Text = "";
            lb_c24.Text = "";
            lb_c25.Text = "";
            lb_c26.Text = "";
            lb_c27.Text = "";
            lb_c28.Text = "";
            lb_c29.Text = "";

            lb_c30.Text = "";
            lb_c31.Text = "";
            lb_c32.Text = "";
            lb_c33.Text = "";
            lb_c34.Text = "";
            lb_c35.Text = "";
            lb_c36.Text = "";
            lb_c37.Text = "";
            lb_c38.Text = "";
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
        }
		#endregion
	}
}
