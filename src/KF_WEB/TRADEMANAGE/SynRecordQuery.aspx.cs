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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SynRecordQuery 的摘要说明。
	/// </summary>
	public partial class SynRecordQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label12;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

				Table2.Visible = false;				
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
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{ 
			DateTime begindate;
			DateTime enddate;

			try
			{
				begindate = DateTime.Parse(TextBoxBeginDate.Text);
				enddate = DateTime.Parse(TextBoxEndDate.Text);
			}
			catch
			{
				throw new Exception("日期输入有误！");
			}

			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("终止日期小于起始日期，请重新输入！");
			}

			if(begindate.AddDays(8).CompareTo(enddate) < 0)
			{
				throw new Exception("选择时间段超过了八天，请重新输入！");
			}


			ViewState["Ftransaction_id"] = tbTransactionID.Text.Trim();
			ViewState["Fpay_status"] = ddlPay_Status.SelectedValue.Trim();

			ViewState["Fsyn_status"] = ddlSynStatus.SelectedValue.Trim();
			ViewState["Fsyn_type"] = ddlSynType.SelectedValue.Trim();
			ViewState["Fpay_type"] = ddlPayType.SelectedValue.Trim();

			ViewState["Fsp_id"] = tbSPID.Text.Trim();
			ViewState["Fsp_billno"] = tbSPBillno.Text.Trim();
			ViewState["Fpurchaser_uin"] = tbPurchaser.Text.Trim();
			ViewState["Fbargainor_uin"] = tbBargainor.Text.Trim();

			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

			ViewState["synresult"] = ddlSynResult.SelectedValue.Trim();

			if(CheckBox1.Checked)
				ViewState["flag"] = 1;
			else
				ViewState["flag"] = 0;
		}

		protected void Button2_Click(object sender, System.EventArgs e)
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
				Table2.Visible = true;
                pager.RecordCount = 2000;//GetCount(); 
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

		private int GetCount()
		{
			string begindate = ViewState["begindate"].ToString();
			string enddate = ViewState["enddate"].ToString();

			string transid = ViewState["Ftransaction_id"].ToString();
			int paystatus = Int32.Parse(ViewState["Fpay_status"].ToString());

			int synstatus = Int32.Parse(ViewState["Fsyn_status"].ToString());
			int syntype = Int32.Parse(ViewState["Fsyn_type"].ToString());
			int paytype = Int32.Parse(ViewState["Fpay_type"].ToString());
			int synresult = Int32.Parse(ViewState["synresult"].ToString());

			string spid = ViewState["Fsp_id"].ToString();
			string spbillno = ViewState["Fsp_billno"].ToString();
			string purchaser = ViewState["Fpurchaser_uin"].ToString();
			string bargainor = ViewState["Fbargainor_uin"].ToString();

			int flag = Int32.Parse(ViewState["flag"].ToString());

			//Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return 1000;//qs.GetSynRecordQueryCount(transid,begindate,enddate,paystatus,synstatus,syntype,paytype,spid,spbillno,purchaser,bargainor,flag,synresult);
		}

		public string GetTransUrl(string listID)
		{
			string returnUrl = "";

			if(listID.Length == 21)
			{
				returnUrl = "FundQuery.aspx?czID=" + listID;
			}
			else if(listID.Length == 28)
			{
				returnUrl = "TradeLogQuery.aspx?id=" + listID;
			}

			return returnUrl;
		}

        private void showMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

		private void BindData(int index)
		{
			string begindate = ViewState["begindate"].ToString();
			string enddate = ViewState["enddate"].ToString();

			string transid = ViewState["Ftransaction_id"].ToString();
			int paystatus = Int32.Parse(ViewState["Fpay_status"].ToString());

			int synstatus = Int32.Parse(ViewState["Fsyn_status"].ToString());
			int syntype = Int32.Parse(ViewState["Fsyn_type"].ToString());
			int paytype = Int32.Parse(ViewState["Fpay_type"].ToString());
			int synresult = Int32.Parse(ViewState["synresult"].ToString());

			string spid = ViewState["Fsp_id"].ToString();
			string spbillno = ViewState["Fsp_billno"].ToString();
			string purchaser = ViewState["Fpurchaser_uin"].ToString();
			string bargainor = ViewState["Fbargainor_uin"].ToString();

			int flag = Int32.Parse(ViewState["flag"].ToString());
			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

			DataSet ds = qs.GetSynRecordQueryList(transid,begindate,enddate,paystatus,synstatus,
				syntype,paytype,spid,spbillno,purchaser,bargainor,flag, synresult,start,max);

			if(ds != null && ds.Tables.Count >0)
			{
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fbank_type","Fbank_typeName","BANK_TYPE");

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
                showMsg("没有找到记录");
			}
		}

        protected void btnSelectItem_Click(object sender, EventArgs e)
        {

            int count = DataGrid1.Items.Count;
            if (count <= 0)
            {
                showMsg("没有可选的数据项");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                System.Web.UI.Control obj = DataGrid1.Items[i].Cells[10].FindControl("CheckBox2");
                if (obj != null && obj.Visible)
                {
                    CheckBox cb = (CheckBox)obj;
                    if (selbtnID.Text == "全选")
                    {
                        cb.Checked = true;
                    }
                    else
                    {
                        cb.Checked = false;
                    }
                }
            }

            if (selbtnID.Text == "全选")
            {
                selbtnID.Text = "取消全选";
            }
            else
            {
                selbtnID.Text = "全选";
            }

        }

        protected void btnBatchSyn_Click(object sender, System.EventArgs e)
        {
            try
            {
                string listIds = "";
                int itemCounts = 0;
                string msg = "";
                bool getFlag = GetCheckData(out listIds, out itemCounts, out msg);
                if (!getFlag)
                {
                    WebUtils.ShowMessage(this.Page, msg);

                }
                else
                {
                    if (listIds != "" && listIds.EndsWith("|"))
                    {
                        listIds = listIds.Substring(0, listIds.Length - 1);
                    }
                    else
                    {

                        WebUtils.ShowMessage(this.Page, "请先选中需要操作的数据！");
                        BindData(pager.CurrentPageIndex);
                        return;
                    }
                    Query_Service.Query_Service qs = new Query_Service.Query_Service();
                    string errMsg = "";
                    if (!qs.BatchSynPayState(listIds, itemCounts, out errMsg))
                    {
                        WebUtils.ShowMessage(this.Page, errMsg);
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "批量同步操作成功！");
                    }
                }

                BindData(pager.CurrentPageIndex);

            }
            catch (Exception err)
            {
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                WebUtils.ShowMessage(this.Page, "同步支付状态失败：" + errStr);
            }
        }

        

        private bool GetCheckData(out string listIds, out int itemCounts, out string msg)
        {
            listIds = "";
            itemCounts = 0;
            msg = "";
            try
            {
                int count = DataGrid1.Items.Count;

                for (int i = 0; i < count; i++)
                {
                    System.Web.UI.Control obj = DataGrid1.Items[i].Cells[10].FindControl("CheckBox2");
                    if (obj != null && obj.Visible)
                    {
                        CheckBox cb = (CheckBox)obj;
                        if (cb.Checked)
                        {
                            string listid = DataGrid1.Items[i].Cells[11].Text.Trim();

                            listIds += (listid + "|");
                            itemCounts++;
                        }
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
	}
}
