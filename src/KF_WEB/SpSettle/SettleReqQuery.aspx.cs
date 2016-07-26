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
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
	/// <summary>
    /// SettleReqQuery 的摘要说明。
	/// </summary>
    public partial class SettleReqQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
                string listid = this.txtListid.Text.Trim();

                if (listid.Length != 28)
                    throw new Exception("请输入28位订单号！");
                string s_reqid = this.txtReqid.Text.Trim();

                BindInfo(listid, s_reqid);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private void BindInfo(string szListid, string reqid)
        {
         
            SettleService service = new SettleService();
            DataTable dt = service.GetSettleReqList(szListid, reqid);
            if (dt != null )
            {
                ViewState["g_dt"] = dt;

                dt.Columns.Add("Ftotal_fee_str", typeof(string)); //支付金额
                dt.Columns.Add("Fsettle_fee_str", typeof(string));//分账金额
                dt.Columns.Add("Fstate_str", typeof(string)); //分账状态
                dt.Columns.Add("Fcurtype_str", typeof(string)); //币种
                dt.Columns.Add("Flstate_str", typeof(string)); //订单物理状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "分账前");
                ht1.Add("2", "分账成功");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "RMB");
                Hashtable ht3 = new Hashtable();
                ht3.Add("1", "正常");
                ht3.Add("2", "作废");

                classLibrary.setConfig.FenToYuan_Table(dt, "Ftotal_fee", "Ftotal_fee_str");
                classLibrary.setConfig.FenToYuan_Table(dt, "Fsettle_fee", "Fsettle_fee_str");

                classLibrary.setConfig.DbtypeToPageContent(dt, "Fstate", "Fstate_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Fcurtype", "Fcurtype_str", ht2);
                classLibrary.setConfig.DbtypeToPageContent(dt, "Flstate", "Flstate_str", ht3);

                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else 
            {
                throw new Exception("没有找到订单号"+szListid+"的记录！");
            }
            
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string listid = e.Item.Cells[0].Text.Trim(); //财付通订单号

            switch (e.CommandName)
            {
                case "REQDETAIL": //请求明细
                    int rid = e.Item.ItemIndex;
                    rq_tb2.Visible = true;
                    GetReqDetail(rid);
                    break;
                case "SETDETAIL": //分账明细
                    Response.Redirect("SettleReqDetail.aspx?listid=" + listid);
                    break;
            }
        }

        private void GetReqDetail(int rid) 
        {
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null) 
            {
                lb_c1.Text = g_dt.Rows[rid]["Fsettle_request_id"].ToString();//分账请求流水号
                lb_c2.Text = g_dt.Rows[rid]["Fsettle_list_id"].ToString();//分账总单
                lb_c3.Text = g_dt.Rows[rid]["Fcoding"].ToString();//订单编码
                lb_c4.Text = g_dt.Rows[rid]["Flistid"].ToString();//财付通订单号
                lb_c5.Text = g_dt.Rows[rid]["Fpnr"].ToString();//PNR码
                lb_c6.Text = g_dt.Rows[rid]["Fcontact"].ToString();//联系人
                lb_c7.Text = g_dt.Rows[rid]["Fpri_spid"].ToString();//机票平台ID
                lb_c8.Text = g_dt.Rows[rid]["Fflight_info"].ToString();//航程
                lb_c9.Text = g_dt.Rows[rid]["Fphone"].ToString();//联系电话
                lb_c10.Text = g_dt.Rows[rid]["Fticket_num"].ToString();//机票张数

                lb_c11.Text = g_dt.Rows[rid]["Fcurtype_str"].ToString();//币种
                lb_c12.Text = g_dt.Rows[rid]["Fsettle_fee_str"].ToString();//总分账金额
                lb_c13.Text = g_dt.Rows[rid]["Ftotal_fee_str"].ToString();//订单支付金额
                lb_c14.Text = g_dt.Rows[rid]["Fbus_type"].ToString();//业务规则
                lb_c15.Text = g_dt.Rows[rid]["Fbus_args"].ToString();//分账参数
                lb_c16.Text = g_dt.Rows[rid]["Fbus_desc"].ToString();//分账原始描述
                lb_c17.Text = g_dt.Rows[rid]["Fsp_bankurl"].ToString();//商户回调URL
                lb_c18.Text = g_dt.Rows[rid]["Fstate_str"].ToString();//分账状态
                lb_c19.Text = g_dt.Rows[rid]["Flstate_str"].ToString();//订单物理状态
                lb_c20.Text = g_dt.Rows[rid]["Fcreate_time"].ToString();//创建时间

                lb_c21.Text = g_dt.Rows[rid]["Fsettle_time"].ToString();//分账时间
                lb_c22.Text = g_dt.Rows[rid]["Fmodify_time"].ToString();//修改时间
                lb_c23.Text = g_dt.Rows[rid]["Fagentid"].ToString();//代理商信息
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
