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
	/// RefundTotalQuery ��ժҪ˵����
	/// </summary>
	public partial class RefundTotalQuery : System.Web.UI.Page
	{
	    protected Hashtable ht;
        //����ʵ����Ҫʹ�õ�ht;
		protected string[,] ar;
		
	
		private void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                string uid = Session["uid"].ToString();
                string szkey = Session["szkey"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch  //���û�е�½����û��Ȩ�޾�����
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

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            //this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region ��ť�¼�

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
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
			}
		
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}
		#endregion

		#region ���ط���
		void BindBankType(DropDownList DropDownList1)
		{
			classLibrary.setConfig.GetAllBankList(ddlrefund_bank);
			ddlrefund_bank.Items.Insert(0,new ListItem("��������","0000"));
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
				throw new Exception("������������");
			}
			
			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
			}
			
			if(txFoldid.Text.Trim()=="")//�˿Ϊ��
			{
				if(begindate.AddDays(1).CompareTo(enddate) < 0)
				{
					throw new Exception("ѡ��ʱ��γ��������죬���������룡");
				}

				if(Convert.ToDateTime(begindate).Year!=Convert.ToDateTime(enddate).Year)
				{
					throw new Exception("�ݲ�֧�ֿ����ѯ��������ѡ����ֹ���ڣ�");
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
				throw new Exception("û���ҵ���¼��");
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
					throw new Exception("û���ҵ���¼��");
				}
				
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
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
        //            string tablename = "��������";
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
        //            strCreateTableSQL += @" ���׵��� VARCHAR(32), ";
        //            strCreateTableSQL += @" ������ VARCHAR(32), ";
        //            strCreateTableSQL += @" �������� VARCHAR(32), ";
        //            strCreateTableSQL += @" ���׽�� VARCHAR(32), ";
        //            strCreateTableSQL += @" �˿��� VARCHAR(20) ";
        //            strCreateTableSQL += @" ) ";

        //            OleDbCommand command = new OleDbCommand(strCreateTableSQL,conn);
        //            conn.Open();

        //            command.ExecuteNonQuery();

        //            tablename = BankIO.GetFirstSheetName(conn);
        //            string InsertSql = "INSERT INTO " + tablename + "( ���׵���,������,��������,���׽��,�˿��� ) "+
        //                "values('{0}','{1}','{2}','{3}','{4}' )";

        //            string strSql="";
        //            long sumramt=0;
        //            foreach(DataRow drs in dt.Rows)
        //            {						
        //                string FRefundID=drs["FPaylistid"].ToString();

        //                string FPay_time="";
        //                if(drs["FPay_time"]!=null&&drs["FPay_time"].ToString()!="")
        //                {
        //                    FPay_time=Convert.ToDateTime(drs["FPay_time"].ToString()).ToString("yyyy��MM��dd��");
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
