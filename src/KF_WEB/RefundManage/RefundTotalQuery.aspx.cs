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
using TENCENT.OSS.C2C.Finance.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Data.OleDb;
using TENCENT.OSS.C2C.Finance.BankLib;
using System.Reflection;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// RefundTotalQuery 的摘要说明。
	/// </summary>
	public partial class RefundTotalQuery : System.Web.UI.Page
	{
	    protected Hashtable ht;
        //当次实际需要使用的ht;
		protected string[,] ar;
		
	
		private void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                string uid = Session["uid"].ToString();
                string szkey = Session["szkey"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch  //如果没有登陆或者没有权限就跳出
            {
                Response.Redirect("../login.aspx?wh=1");
            }

			if(!IsPostBack)
			{	
				TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
				Table2.Visible = false;		
				BindBankType(ddlrefund_bank);

				try
				{
					if(Request.QueryString["batchid"]!=null&&Request.QueryString["refundids"]!=null)
					{
						ViewState["batchid"] = Request.QueryString["batchid"].ToString();
						ViewState["refundids"] = Request.QueryString["refundids"].ToString();
						BindDataByIds();
						SearchPanel.Visible = false;
						
						

					}
				}
				catch
				{
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
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            //this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region 按钮事件

		private void btnQuery_Click(object sender, System.EventArgs e)
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
				pager.RecordCount= GetCount(); 
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

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}
		#endregion

		#region 本地方法
		void BindBankType(DropDownList DropDownList1)
		{
			classLibrary.setConfig.GetAllBankList(ddlrefund_bank);
			ddlrefund_bank.Items.Insert(0,new ListItem("所有银行","0000"));
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
			
			if(txFoldid.Text.Trim()=="")//退款单为空
			{
				if(begindate.AddDays(1).CompareTo(enddate) < 0)
				{
					throw new Exception("选择时间段超过了两天，请重新输入！");
				}

				if(Convert.ToDateTime(begindate).Year!=Convert.ToDateTime(enddate).Year)
				{
					throw new Exception("暂不支持跨年查询，请重新选择起止日期！");
				}
			}
						
			ViewState["spid"] = tbSPID.Text.Trim();
			ViewState["begindate"] = begindate.ToString("yyyy-MM-dd 00:00:00");
			ViewState["enddate"] = enddate.ToString("yyyy-MM-dd 23:59:59");
			ViewState["refundtype"] = ddlrefund_type.SelectedValue;
			ViewState["refundbank"] = ddlrefund_bank.SelectedValue;
			ViewState["refundpath"] = ddlrefund_path.SelectedValue;
			ViewState["refundstate"] = ddlrefund_state.SelectedValue;
			ViewState["returnstate"] = ddlreturn_state.SelectedValue;
			ViewState["RefundID"] =tbRefundID.Text.Trim();
			ViewState["Banklist"]=tbBank_list.Text.Trim();
			ViewState["oldid"]=this.txFoldid.Text.Trim();
		}


		private void BindData(int index)
		{
			string spid = ViewState["spid"].ToString();
			string begindate = ViewState["begindate"].ToString();
			string enddate = ViewState["enddate"].ToString();
			int refundtype = Int32.Parse(ViewState["refundtype"].ToString());
			string banktype = ViewState["refundbank"].ToString();
			int refundpath = Int32.Parse(ViewState["refundpath"].ToString());
			int refundstate = Int32.Parse(ViewState["refundstate"].ToString());
			int returnstate = Int32.Parse(ViewState["returnstate"].ToString());
			string RefundID = ViewState["RefundID"].ToString();
			string Banklist = ViewState["Banklist"].ToString();
			string oldid = ViewState["oldid"].ToString();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

            FINANCE_RefundSERVICE.Query_Service qs = new FINANCE_RefundSERVICE.Query_Service();

            FINANCE_RefundSERVICE.Finance_Header fh = setConfig.FsetRefundFH(this);
			qs.Finance_HeaderValue = fh;
			
			DataSet ds = qs.GetRefundTotalList(spid,begindate,enddate,refundtype,banktype,refundpath,refundstate,returnstate,RefundID,Banklist,false,oldid,start,max);
			

			if(ds != null && ds.Tables.Count >0)
			{			
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fbanktype","Fbank_typeName","BANK_TYPE");

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new Exception("没有找到记录！");
			}
		}


		private void BindDataByIds()
		{
			this.pager.PageSize=100;
            FINANCE_RefundSERVICE.Query_Service qs = new FINANCE_RefundSERVICE.Query_Service();

            FINANCE_RefundSERVICE.Finance_Header fh = setConfig.FsetRefundFH(this);
			
			qs.Finance_HeaderValue = fh;
			
			DataSet ds = qs.GetRefundTotalByIds(ViewState["batchid"].ToString(),ViewState["refundids"].ToString());

			try
			{
				int count=1;
				string refundids=ViewState["refundids"].ToString();
				if(refundids.IndexOf("|")>0)
				{
					string[] items=refundids.Split('|');
					count=items.Length;
				}
				Table2.Visible = true;
				Table1.Visible = false;
				
				pager.RecordCount= count; 

				if(ds != null && ds.Tables.Count >0)
				{			
					classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fbanktype","Fbank_typeName","BANK_TYPE");

					DataGrid1.DataSource = ds.Tables[0].DefaultView;
					DataGrid1.DataBind();
				}
				else
				{
					throw new Exception("没有找到记录！");
				}
				
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
			string spid = ViewState["spid"].ToString();
			string begindate = ViewState["begindate"].ToString();
			string enddate = ViewState["enddate"].ToString();
			int refundtype = Int32.Parse(ViewState["refundtype"].ToString());
			string banktype = ViewState["refundbank"].ToString();
			int refundpath = Int32.Parse(ViewState["refundpath"].ToString());
			int refundstate = Int32.Parse(ViewState["refundstate"].ToString());
			int returnstate = Int32.Parse(ViewState["returnstate"].ToString());
			string RefundID = ViewState["RefundID"].ToString();
			string Banklist = ViewState["Banklist"].ToString();
			string oldid = ViewState["oldid"].ToString();

            FINANCE_RefundSERVICE.Query_Service qs = new FINANCE_RefundSERVICE.Query_Service();
			return qs.GetRefundTotalCount(spid,begindate,enddate,refundtype,banktype,refundpath,refundstate,returnstate,RefundID,Banklist,oldid);
		}


        //public bool Table2Excel(DataTable dt, string ExcelPath, out string msg)
        //{
        //    msg = "";
        //    try
        //    {
        //        if(System.IO.File.Exists(ExcelPath))
        //            System.IO.File.Delete(ExcelPath);

        //        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" 
        //            +"Data Source=" + ExcelPath
        //            +";Extended Properties='Excel 8.0'"; //;HDR=Yes;IMEX=1

				
        //        OleDbConnection conn = new OleDbConnection(strConn);
        //        try
        //        {
        //            string tablename = "生成内容";
        //            if(dt!=null&&dt.Rows.Count>0)
        //            {
        //                string bankname=dt.Rows[0]["Fbank_typeName"].ToString();
        //                if(bankname!="")
        //                {
        //                    tablename=bankname;
        //                }
        //            }

        //            string strCreateTableSQL = @" CREATE TABLE ";
        //            strCreateTableSQL +=tablename;
        //            strCreateTableSQL += @" ( ";
        //            strCreateTableSQL += @" 交易单号 VARCHAR(32), ";
        //            strCreateTableSQL += @" 订单号 VARCHAR(32), ";
        //            strCreateTableSQL += @" 交易日期 VARCHAR(32), ";
        //            strCreateTableSQL += @" 交易金额 VARCHAR(32), ";
        //            strCreateTableSQL += @" 退款金额 VARCHAR(20) ";
        //            strCreateTableSQL += @" ) ";

        //            OleDbCommand command = new OleDbCommand(strCreateTableSQL,conn);
        //            conn.Open();

        //            command.ExecuteNonQuery();

        //            tablename = BankIO.GetFirstSheetName(conn);
        //            string InsertSql = "INSERT INTO " + tablename + "( 交易单号,订单号,交易日期,交易金额,退款金额 ) "+
        //                "values('{0}','{1}','{2}','{3}','{4}' )";

        //            string strSql="";
        //            long sumramt=0;
        //            foreach(DataRow drs in dt.Rows)
        //            {						
        //                string FRefundID=drs["FPaylistid"].ToString();

        //                string FPay_time="";
        //                if(drs["FPay_time"]!=null&&drs["FPay_time"].ToString()!="")
        //                {
        //                    FPay_time=Convert.ToDateTime(drs["FPay_time"].ToString()).ToString("yyyy年MM月dd日");
        //                }

        //                string Fbank_listid="";
        //                if(drs["Fbank_listid"]!=null)
        //                {
        //                    Fbank_listid=drs["Fbank_listid"].ToString();
        //                }


        //                string FamtName="";
        //                if(drs["FamtName"]!=null)
        //                {
        //                    FamtName=drs["FamtName"].ToString();
        //                }

        //                string FreturnamtName="";
        //                if(drs["FreturnamtName"]!=null)
        //                {
        //                    FreturnamtName=drs["FreturnamtName"].ToString();
        //                    sumramt+=long.Parse(drs["Freturnamt"].ToString());
        //                }

        //                strSql = String.Format(InsertSql,FRefundID,Fbank_listid,FPay_time,FamtName,FreturnamtName);

        //                command.CommandText = strSql;
        //                command.ExecuteNonQuery();
        //            }
				

        //            return true;
        //        }
        //        finally
        //        {
        //            conn.Close();
        //            conn.Dispose();
        //        }
        //    }
        //    catch(Exception err)
        //    {
        //        msg = err.Message;
        //        return false;
        //    }
        //}
		#endregion
	}
}
