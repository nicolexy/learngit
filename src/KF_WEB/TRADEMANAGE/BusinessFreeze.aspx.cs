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
using CFT.CSOMS.BLL.SPOA;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// BusinessFreeze 的摘要说明。
	/// </summary>
	public partial class BusinessFreeze : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					this.btnFreeze.Attributes["onClick"] = "if(!confirm('确定要提交吗？')) return false;"; 
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
		}
		#endregion

		protected void btnFreeze_Click(object sender, System.EventArgs e)
		{
            //Table2.Visible = false;
            try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = TENCENT.OSS.CFT.KF.Common.AllUserRight.GetServiceID("InfoCenter") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "执行了[商户冻结]操作,操作对象[" + this.txtFspid.Text.Trim()
					+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				if(this.txtFspid.Text.Trim() == "")
				{
					throw new Exception("请输入商户号!");
				}
				if(this.txtReason.Text.Trim() == "")
				{
					throw new Exception("请输入原因!");
				}
                if (!this.cbxFreeze.Checked && !this.cbxPay.Checked && !this.cbxAccLose.Checked && !this.cbxCloseAgent.Checked)
				{
					throw new Exception("请选择操作选项!");
				}

				bool IsFreeze = false;//暂停结算
				bool IsFreezePay = false;//关闭支付
                bool IsAccLoss = false; //账号挂失
                bool IsCloseAgent = false; //关闭中介
				if(this.cbxFreeze.Checked)
					IsFreeze = true;
				if(this.cbxPay.Checked)
					IsFreezePay = true;
                if (this.cbxAccLose.Checked)
                    IsAccLoss = true;
                if (this.cbxCloseAgent.Checked)
                    IsCloseAgent = true;

				string log = classLibrary.SensitivePowerOperaLib.MakeLog("edit",Session["uid"].ToString().Trim(),"[商户冻结]",
					this.txtFspid.Text.Trim(),Session["uid"].ToString(),IsFreeze.ToString(),IsFreezePay.ToString(),
					this.txtReason.Text.Trim());

				if(!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("InfoCenter",log,this))
				{
					
				}
                if (IsFreeze)
                {
                    //暂停结算，直接调spoa接口
                    //SPOA_Service.SPOA_Service spoaService = new TENCENT.OSS.CFT.KF.KF_Web.SPOA_Service.SPOA_Service();
                    //string spoa_ret = spoaService.FreezeSP(this.txtFspid.Text.Trim(), this.txtReason.Text.Trim(), Session["uid"].ToString());
                   
                    string spoa_ret = new SPOAService().FreezeSpid(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                    
                    if (string.IsNullOrEmpty(spoa_ret)|| spoa_ret=="0")
                    {
                        
                    }
                    else 
                    {
                        throw new Exception("暂停结算错误："+spoa_ret);
                    }
                }
               
                if (IsFreezePay)
                {
                    //关闭支付，直接调spoa接口
                    //SPOA_Service.SPOA_Service spoaService = new TENCENT.OSS.CFT.KF.KF_Web.SPOA_Service.SPOA_Service();
                    //string spoa_ret = spoaService.ClosePay(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                  
                    string spoa_ret = new SPOAService().ClosePay(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                    if (spoa_ret == "0")
                    {
                        
                    }
                    else
                    {
                        throw new Exception("关闭支付错误：" + spoa_ret);
                    }
                }

                if (IsAccLoss)
                {
                    //账号挂失，直接调spoa接口
                    //SPOA_Service.SPOA_Service spoaService = new TENCENT.OSS.CFT.KF.KF_Web.SPOA_Service.SPOA_Service();
                    //string spoa_ret = spoaService.LostOfSpid(this.txtFspid.Text.Trim(),Session["uid"].ToString(), this.txtReason.Text.Trim());
                  
                    string spoa_ret = new SPOAService().LostOfSpid(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                    if (spoa_ret == "0")
                    {
                        
                    }
                    else
                    {
                        throw new Exception("账号挂失错误：" + spoa_ret);
                    }
                }

                if (IsCloseAgent)
                {
                    //关闭中介，直接调spoa接口
                    //SPOA_Service.SPOA_Service spoaService = new TENCENT.OSS.CFT.KF.KF_Web.SPOA_Service.SPOA_Service();
                    //string spoa_ret = spoaService.CloseAgency(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                  
                    string spoa_ret = new SPOAService().CloseAgency(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                    if (spoa_ret == "0")
                    {
                        
                    }
                    else
                    {
                        throw new Exception("关闭中介错误：" + spoa_ret);
                    }
                }

                WebUtils.ShowMessage(this.Page, "操作成功");
			}
			catch(SoapException eSoap)
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,eSys.Message);
			}
		}
   
        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            RemaindDiv.Visible = false;
            Table2.Visible = true;
            try
            {
                string spid = this.txtFspid.Text;
                if (string.IsNullOrEmpty(spid))
                {
                    throw new Exception("请输入商户号!");
                }
                
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.QueryBussFreezeList(spid.Trim(),"12,41,42,51,52",""); //暂停结算、关闭支付、关闭退款、账号挂失、关闭中介
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
                {
                    ds.Tables[0].Columns.Add("amendstate_str", typeof(String));//状态
                    ds.Tables[0].Columns.Add("amendtype_str", typeof(String));//状态

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("0", "等待资质审批");
                    ht1.Add("1", "等待领导审批");
                    ht1.Add("-1", "等待风控审批");
                    ht1.Add("11","等待生效中");
	                ht1.Add("2","等待结算审批");
	                ht1.Add("-2","等待产品审核");
	                ht1.Add("3","审批通过");
	                ht1.Add("-3","等待客服审批");
	                ht1.Add("4","审批作废");
	                ht1.Add("7","申请被打回");
	                ht1.Add("8","等待中小商户组审核");
	                ht1.Add("9","等待指定人审核");

                    Hashtable ht2 = new Hashtable();
                    ht2.Add("12", "暂停结算");
                    ht2.Add("41", "关闭支付");
                    ht2.Add("42", "关闭退款");
                    ht2.Add("51", "账号挂失");
                    ht2.Add("52", "关闭中介");

                    
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "amendstate", "amendstate_str", ht1);
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "amendtype", "amendtype_str", ht2);

                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
            }
            catch (SoapException eSoap)
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message);
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) 
        {
            string s = e.Item.Cells[7].Text;
            txtApplyResult.Text = s;
        }
	}
}
