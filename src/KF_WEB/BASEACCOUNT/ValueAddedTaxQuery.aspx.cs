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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// ValueAddedTaxQuery 的摘要说明。
	/// </summary>
	public partial class ValueAddedTaxQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

				if(!IsPostBack)
				{
                    if (!classLibrary.ClassLib.ValidateRight("DrawAndApprove", this)) Response.Redirect("../login.aspx?wh=1");
                    
                    btnAllModify.Attributes["onClick"]= "return confirm('确定允许全量信息修改吗？');";
                    //btnLittleModify.Attributes["onClick"]= "return confirm('确定收件人信息修改吗？');";
				}
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(pager.CurrentPageIndex);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + classLibrary.setConfig.replaceMStr(eSys.Message) );
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
			this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				pager.RecordCount = GetCount();
				BindData(1);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + classLibrary.setConfig.replaceMStr(eSys.Message));
			}
		}

		private int GetCount()
		{
			string filter = GetfilterString();
			ViewState["filter"] = filter;

			return 100;
		}

		private void BindData(int index)
		{
			string filter = ViewState["filter"].ToString();

			int TopCount = pager.PageSize;
			int NotInCount = TopCount * (index-1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.GetAllValueAddedTax(txtSpid.Text.Trim(), txtCompanyName.Text.Trim(), TopCount, NotInCount);

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("TaxInvoiceFlagStr",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr["TaxInvoiceFlagStr"] = GetFlag(dr["TaxInvoiceFlag"].ToString());
				}
				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

		private string GetFlag(string Flag)
		{
			if(Flag == "1")
				return "初次申请授权书待上传";
			else if(Flag == "2")
				return "初次申请审核中";
			else if(Flag == "3")
				return "全单修改审核中";
			else if(Flag == "4")
				return "审核通过";
			else if(Flag == "5")
				return "审核不通过";
			else if(Flag == "6")
				return "收件人信息修改中";
			else if(Flag == "7")
				return "收件人信息修改成功";
			else if(Flag == "8")
				return "收件人信息修改失败";
			else if(Flag == "9")
				return "待商户提交全单修改申请";
			else if(Flag == "10")
				return "全单修改授权书待上传";
            else if (Flag == "11")
                return "spoa审核";
			else 
				return Flag;
		}

		private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			try
			{
				BindDataDetail(e.Item.Cells[0].Text.Trim());
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + ex.Message.ToString());
			}
		}

		private void BindDataDetail(string Spid)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetOneValueAddedTax(Spid);
			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				this.PanelDetail.Visible = false;
				throw new Exception("数据库中没有找到该商户！");
			}

			this.PanelDetail.Visible = true;
			DataRow dr = ds.Tables[0].Rows[0];
			this.lblSpid.Text = dr["spid"].ToString();
			this.lblCompanyName.Text = dr["CompanyName"].ToString();
			this.lblTaxInvoiceFlag.Text = GetFlag(dr["TaxInvoiceFlag"].ToString());
			if(dr["TaxerType"].ToString() == "0")
				this.lblTaxerType.Text = "增值税一般纳税人";
			else if(dr["TaxerType"].ToString() == "1")
				this.lblTaxerType.Text = "不是增值税一般纳税人";
			else
				this.lblTaxerType.Text = dr["TaxerType"].ToString();

			if(dr["TaxInvoiceType"].ToString() == "0")
				this.lblTaxInvoiceType.Text = "增值税专用发票";
			else if(dr["TaxInvoiceType"].ToString() == "1")
				this.lblTaxInvoiceType.Text = "通用机打发票";
			else
				this.lblTaxInvoiceType.Text = dr["TaxInvoiceType"].ToString();
			this.lblTaxerCompanyName.Text = dr["TaxerCompanyName"].ToString();
			this.lblTaxerID.Text = dr["TaxerID"].ToString();
			this.lblTaxerBasebankName.Text = dr["TaxerBasebankName"].ToString();
            bool isRight = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);            
            //this.lblTaxerBaseBankAcct.Text = dr["TaxerBaseBankAcct"].ToString();
            this.lblTaxerBaseBankAcct.Text = classLibrary.setConfig.BankCardNoSubstring(dr["TaxerBaseBankAcct"].ToString(), isRight);
			this.lblTaxerReceiverName.Text = dr["TaxerReceiverName"].ToString();
			this.lblTaxerReceiverAddr.Text = dr["TaxerReceiverAddr"].ToString();
			this.lblTaxerReceiverPostalCode.Text = dr["TaxerReceiverPostalCode"].ToString();
			this.lblTaxerReceiverPhone.Text = dr["TaxerReceiverPhone"].ToString();

            this.lblTaxerCompanyAddress.Text = dr["TaxerCompanyAddress"].ToString();
            this.lblTaxerCompanyPhone.Text = dr["TaxerCompanyPhone"].ToString();
			
			if(dr["TaxerUserType"].ToString() == "1")
				this.lblTaxerUserType.Text = "个人";
			else if(dr["TaxerUserType"].ToString() == "2")
				this.lblTaxerUserType.Text = "公司";
			else
				this.lblTaxerUserType.Text = dr["TaxerUserType"].ToString();

			this.txtTaxInvoiceMemo.Text = dr["TaxInvoiceMemo"].ToString();

			//--申请状态: 0(或空值)-初始状态(未申请过) 1-初次申请授权书待上传 2-初次申请审核中 3-全单修改审核中 4-审核通过
			//5-审核不通过 6-收件人信息修改中 7-收件人信息修改成功 8-收件人信息修改失败 9-待商户提交全单修改申请 10-全单修改授权书待上传

			if(dr["TaxInvoiceFlag"].ToString().Trim() == "4" 
                || dr["TaxInvoiceFlag"].ToString().Trim() == "7" 
                || (dr["TaxInvoiceFlag"].ToString().Trim() == "5" && dr["TaxerType"].ToString() != "")
                || dr["TaxInvoiceFlag"].ToString().Trim() == "0"
                || dr["TaxInvoiceFlag"].ToString().Trim() == "" 
                )
			{
				this.btnAllModify.Visible = true;
                //this.btnLittleModify.Visible = true;
			}
			else
			{
				this.btnAllModify.Visible = false;
                //this.btnLittleModify.Visible = false;
			}
		}
		#region 获得查询字符串
		private string GetfilterString()
		{
			if(this.txtSpid.Text.Trim() == "" && this.txtCompanyName.Text.Trim() == "")
			{
				throw new Exception("请出入商户号或商户名称！");
			}

			string filter = " CompanyName like '%" + this.txtCompanyName.Text.Trim() + "%' ";
            string filterNew = " and CompanyName.Contains('" + this.txtCompanyName.Text.Trim() + "') ";

            if (this.txtSpid.Text.Trim() != "")
            {
                filter += " and Spid='" + this.txtSpid.Text.Trim() + "' ";
                filterNew += " and Spid='" + this.txtSpid.Text.Trim() + "' ";
            }
			return filterNew;
		}

		#endregion

		protected void btnAllModify_Click(object sender, System.EventArgs e)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				qs.ValueAddedTaxModify(this.lblSpid.Text,9);
				BindDataDetail(this.lblSpid.Text);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"操作失败！" + classLibrary.setConfig.replaceMStr(ex.Message) );
			}

		}

        //protected void btnLittleModify_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
        //        qs.ValueAddedTaxModify(this.lblSpid.Text,6);
        //        BindDataDetail(this.lblSpid.Text);
        //    }
        //    catch(Exception ex)
        //    {
        //        WebUtils.ShowMessage(this.Page,"操作失败！" + classLibrary.setConfig.replaceMStr(ex.Message) );
        //    }
        //}

	}
}
