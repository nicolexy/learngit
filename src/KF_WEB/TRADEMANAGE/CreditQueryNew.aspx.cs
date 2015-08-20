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


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// CreditQuery 的摘要说明。
	/// </summary>
	public partial class CreditQueryNew : System.Web.UI.Page
	{
        PickService pickservice = new PickService();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
			this.ddlDate.Attributes.Add("onclick", "CheckDate()");
			this.rbtFlistid.Attributes.Add("onclick", "CheckType()");
			this.rbtFspid.Attributes.Add("onclick", "CheckType()");

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				this.rbtFspid.Checked = true;
			}

			if(this.ddlDate.SelectedValue == "0")
			{
				this.TextBoxBeginDate.Enabled = true;
				this.TextBoxEndDate.Enabled = true;
				this.ButtonBeginDate.Enabled = true;
				this.ButtonEndDate.Enabled = true;
			}
			else
			{
				TextBoxBeginDate.Text = DateTime.Now.AddMonths(-3).ToString("yyyy年MM月dd日");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
				this.TextBoxBeginDate.Enabled = false;
				this.TextBoxEndDate.Enabled = false;
				this.ButtonBeginDate.Enabled = false;
				this.ButtonEndDate.Enabled = false;
			}

			if(this.rbtFspid.Checked)
			{
				this.divFspid.Style["display"] = "block";
				this.divFlistid.Style["display"] = "none";
			}
			else
			{
				this.divFspid.Style["display"] = "none";
				this.divFlistid.Style["display"] = "block";
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
			this.dgList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgList_ItemDataBound);
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(e.NewPageIndex);
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


		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
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
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + classLibrary.setConfig.replaceMStr(eSys.Message));
			}
		}


		private int GetCount()
		{
			if(this.rbtFspid.Checked)
			{
				DateTime begindate;
				DateTime enddate;

				if(this.ddlDate.SelectedValue == "0")
				{
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
				}
				else
				{
					begindate = DateTime.Today.AddMonths(-3);
					enddate = DateTime.Today;
				}

//				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
//				ds = qs.GetCreditQueryListForFaidCount(this.txtFspid.Text.Trim(),begindate,enddate);

				ViewState["begindate"] = begindate;
				ViewState["enddate"] = enddate;
			}
			else
			{
//				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
//				ds = qs.GetCreditQueryListCount(this.txtFspid.Text.Trim());
			}

			return 100;//Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
		}

		private void BindData(int index)
		{
			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

//			Finance_Header fh = new Finance_Header();
//			fh.UserIP = Request.UserHostAddress;
//			fh.UserName = Session["uid"].ToString();
//			fh.OperID = Int32.Parse(Session["OperID"].ToString());
//			fh.SzKey = Session["SzKey"].ToString();
//
//			qs.Finance_HeaderValue = fh;
			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

			DataSet ds;
            if (this.rbtFspid.Checked)
            {
                ds = pickservice.GetCreditQueryListForFaid(this.txtFspid.Text.Trim(), Convert.ToDateTime(ViewState["begindate"].ToString()), Convert.ToDateTime(ViewState["enddate"].ToString()), start, max);
                //ds = qs.GetCreditQueryListForFaid(this.txtFspid.Text.Trim(),Convert.ToDateTime(ViewState["begindate"].ToString()),Convert.ToDateTime(ViewState["enddate"].ToString()),start,max);
            }
            else
            {
                ds = pickservice.GetCreditQueryList(this.txtFlistid.Text.Trim(), start, max);
                //ds = qs.GetCreditQueryList(this.txtFlistid.Text.Trim(), start, max);
            }

			if(ds != null && ds.Tables.Count >0)
			{
				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

		private void dgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemIndex > -1)
			{
				if(e.Item.Cells[0].Text.Trim() != "")
					e.Item.Cells[0].Text = DateTime.Parse(e.Item.Cells[0].Text.Trim()).ToString("yyyy-MM-dd");
				if(e.Item.Cells[2].Text.Trim() == "3001")
					e.Item.Cells[3].Text = "兴业银行";
				if(e.Item.Cells[5].Text.Trim() != "")
					e.Item.Cells[5].Text = MoneyTransfer.FenToYuan(e.Item.Cells[5].Text.Trim());
				if(e.Item.Cells[6].Text.Trim() == "1")
					e.Item.Cells[6].Text = "成功";
				else if(e.Item.Cells[6].Text.Trim() == "2")
					e.Item.Cells[6].Text = "失败";
				else if(e.Item.Cells[6].Text.Trim() == "3" || e.Item.Cells[6].Text.Trim() == "4")
					e.Item.Cells[6].Text = "还款中";
				else
					e.Item.Cells[6].Text = "Unknow";
			}
		}
	}
}
