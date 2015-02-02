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
	/// DomainApprove 的摘要说明。
	/// </summary>
	public partial class DomainApprove : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"DrawAndApprove")) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
                    if (!classLibrary.ClassLib.ValidateRight("DrawAndApprove", this)) Response.Redirect("../login.aspx?wh=1");

                    ViewState["uid"] = Session["uid"].ToString();
					ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
					ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");
					this.PanelDetail.Visible = false;
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
			this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

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

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.PanelDetail.Visible = false;
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

			int topCount = pager.PageSize;
            int notInCount = topCount * (index - 1);

            DateTime? ApplyTimeStart = null;
            if (!string.IsNullOrEmpty(TextBoxBeginDate.Text))
            {
                string strBeginTime = Convert.ToDateTime(TextBoxBeginDate.Text.Trim()).ToString("yyyy-MM-dd 00:00:00");
                ApplyTimeStart = Convert.ToDateTime(strBeginTime);
            }

            DateTime? ApplyTimeEnd = null;
            if (!string.IsNullOrEmpty(TextBoxEndDate.Text))
            {
                string strEndTime = Convert.ToDateTime(TextBoxEndDate.Text.Trim()).ToString("yyyy-MM-dd 23:59:59");
                ApplyTimeEnd = Convert.ToDateTime(strEndTime);
            }
           

            string Spid = txtSpid.Text.Trim();
            string CompanyName = txtCompanyName.Text.Trim();
            int? AmendState = null;
            if (ddlState.SelectedValue != "")
            {
                AmendState = int.Parse(ddlState.SelectedValue); 
            }

            string submitType = ddlSubmitType.SelectedValue;
            dgList.DataSource = null;
            dgList.DataBind();
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.GetSpidDomainQueryList(Spid, CompanyName, ApplyTimeStart, ApplyTimeEnd, AmendState, submitType, topCount, notInCount);

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("TypeStr",typeof(string));
				ds.Tables[0].Columns.Add("AmendStateStr",typeof(string));
				ds.Tables[0].Columns.Add("AmendTypeStr",typeof(string));

				if(this.ddlSubmitType.SelectedValue == "1")
				{
					ds.Tables[0].Columns.Add("AmendType",typeof(string));
					ds.Tables[0].Columns.Add("ContactEmail",typeof(string));
					ds.Tables[0].Columns.Add("OldEmail",typeof(string));
					ds.Tables[0].Columns.Add("NewEmail",typeof(string));
					ds.Tables[0].Columns.Add("OldCompanyname",typeof(string));
					ds.Tables[0].Columns.Add("NewCompanyname",typeof(string));
                    ds.Tables[0].Columns.Add("ElseImage", typeof(string));
				}
				else if(this.ddlSubmitType.SelectedValue == "2")
				{
					ds.Tables[0].Columns.Add("Type",typeof(string));
					ds.Tables[0].Columns.Add("WWWAdress",typeof(string));
					ds.Tables[0].Columns.Add("OldDomain",typeof(string));
					ds.Tables[0].Columns.Add("NewDomain",typeof(string));
					ds.Tables[0].Columns.Add("Memo",typeof(string));
					ds.Tables[0].Columns.Add("Reason",typeof(string));
				}
				else if(this.ddlSubmitType.SelectedValue == "3")
				{
					ds.Tables[0].Columns.Add("Type",typeof(string));
					ds.Tables[0].Columns.Add("WWWAdress",typeof(string));
					ds.Tables[0].Columns.Add("OldDomain",typeof(string));
					ds.Tables[0].Columns.Add("NewDomain",typeof(string));
					ds.Tables[0].Columns.Add("ContactEmail",typeof(string));
                    ds.Tables[0].Columns.Add("Memo", typeof(string));
                    ds.Tables[0].Columns.Add("Reason", typeof(string));
				}
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					if(this.ddlSubmitType.SelectedValue == "1")
					{
						dr["AmendType"] = "18";
						dr["AmendTypeStr"] = "域名";
						dr["TypeStr"] = TypeStr(dr["Type"].ToString());
						dr["ContactEmail"] = "";
						dr["OldEmail"] = "";
						dr["NewEmail"] = "";
						dr["OldCompanyname"] = "";
						dr["NewCompanyname"] = "";
                        dr["ElseImage"] = "";
					}
					else if(this.ddlSubmitType.SelectedValue == "2")
					{
						dr["AmendTypeStr"] = "邮箱";
						dr["Type"] = "";
						dr["TypeStr"] = "";
						dr["WWWAdress"] = "";
						dr["OldDomain"] = "";
						dr["NewDomain"] = "";
						dr["OldCompanyname"] = "";
						dr["NewCompanyname"] = "";
						dr["Memo"] = dr["ApplyResult"].ToString();
						dr["Reason"] = dr["DisagreeResult"].ToString();

					}
					else if(this.ddlSubmitType.SelectedValue == "3")
					{
						dr["AmendTypeStr"] = "商户名称";
						dr["Type"] = "";
						dr["TypeStr"] = "";
						dr["WWWAdress"] = "";
						dr["OldDomain"] = "";
						dr["NewDomain"] = "";
						dr["ContactEmail"] = "";
						dr["OldEmail"] = "";
						dr["NewEmail"] = "";
                        dr["Memo"] = dr["ApplyResult"].ToString();
                        dr["Reason"] = dr["DisagreeResult"].ToString();
					}
					dr["AmendStateStr"] = StateStr(dr["AmendState"].ToString());
				}
				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}


		#region 获得查询字符串
		private string GetfilterString()
		{
			string filter = " a.taskid=b.taskid ";
            string filterNew = " ";

            if (this.txtSpid.Text.Trim() != "")
            {
                filter += " and a.Spid='" + this.txtSpid.Text.Trim() + "' ";
                filterNew += " and Spid='" + this.txtSpid.Text.Trim() + "' ";
            }

            if (this.txtCompanyName.Text.Trim() != "")
            {
                filter += " and b.CompanyName like '%" + this.txtCompanyName.Text.Trim() + "%' ";
                filterNew += " and CompanyName.Contains('" + this.txtCompanyName.Text.Trim() + "') ";
            }

            if (this.TextBoxBeginDate.Text.Trim() != "")
            {
                filter += " and b.ApplyTime >='" + Convert.ToDateTime(this.TextBoxBeginDate.Text.Trim()).ToString("yyyy-MM-dd 00:00:00") + "' ";
                DateTime d1 = Convert.ToDateTime(TextBoxBeginDate.Text.Trim());
                filterNew += " and ApplyTime >=DateTime(" + d1.Year + "," + d1.Month + "," + d1.Day + ",00,00,00)";
            }

            if (this.TextBoxEndDate.Text.Trim() != "")
            {
                filter += " and b.ApplyTime <='" + Convert.ToDateTime(this.TextBoxEndDate.Text.Trim()).ToString("yyyy-MM-dd 23:59:59") + "' ";
                DateTime d1 = Convert.ToDateTime(TextBoxEndDate.Text.Trim());
                filterNew += " and ApplyTime <=DateTime(" + d1.Year + "," + d1.Month + "," + d1.Day + ",23,59,59)";
            }

            if (this.ddlState.SelectedValue != "")
            {
                filter += " and b.AmendState='" + this.ddlState.SelectedValue + "' ";
                filterNew += " and AmendState=" + this.ddlState.SelectedValue+" ";
            }

            return filterNew;
		}

		#endregion

		private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			try
			{
				if(e.CommandName == "Select")
				{
					this.PanelDetail.Visible = true;
					this.btnApprove.Visible = false;
					this.btnCancel.Visible = false;
					this.lblID.Text = e.Item.Cells[0].Text;
					this.lblSpid.Text = e.Item.Cells[1].Text;
					this.lblCompanyName.Text = e.Item.Cells[2].Text;
					this.lblAmendTypeStr.Text = e.Item.Cells[4].Text;
					this.lblState.Text = e.Item.Cells[16].Text;
					if(e.Item.Cells[3].Text == "18")
					{
						this.DomainPanel.Visible = true;
						this.EmailPanel.Visible = false;
						this.CompanyNamePanel.Visible = false;
						this.lblType.Text = e.Item.Cells[6].Text;
						this.lblWWWAdress.Text = e.Item.Cells[7].Text;
						this.lblOldDomain.Text = e.Item.Cells[8].Text;
						this.lblNewDomain.Text = e.Item.Cells[9].Text;
					}
					else if(e.Item.Cells[3].Text == "44")
					{
						this.DomainPanel.Visible = false;
						this.EmailPanel.Visible = true;
						this.CompanyNamePanel.Visible = false;
						this.lblEmail.Text = e.Item.Cells[10].Text;
						this.lblOldEmail.Text = e.Item.Cells[11].Text;
						this.lblNewEmail.Text = e.Item.Cells[12].Text;
						string imagestr = e.Item.Cells[22].Text.Trim();
                        string imagestr2 = e.Item.Cells[23].Text.Trim();
						string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

						if(!url.EndsWith("/"))
							url += "/";

						Image1.ImageUrl =  url + imagestr;
                        Image2.ImageUrl = url + imagestr2; //email审核图片
					}
					else if(e.Item.Cells[3].Text == "45")
					{
						this.DomainPanel.Visible = false;
						this.EmailPanel.Visible = false;
						this.CompanyNamePanel.Visible = true;
						this.lblOldCompanyName.Text = e.Item.Cells[13].Text;
						this.lblNewCompanyName.Text = e.Item.Cells[14].Text;
					}
					
					this.lblApplyTime.Text = e.Item.Cells[17].Text;
					this.lblCheckUser.Text = e.Item.Cells[18].Text;
					this.lblCheckTime.Text = e.Item.Cells[19].Text;
					this.lblMemo.Text = e.Item.Cells[20].Text;
                    this.txtReason.Text = e.Item.Cells[21].Text;

					if(e.Item.Cells[15].Text == "-3")
					{
						this.btnApprove.Visible = true;
						this.btnCancel.Visible = true;
					}
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + ex.Message.ToString());
			}
		}

		private string TypeStr(string Type)
		{
			if(Type == "1")
				return "新增";
			else if(Type == "2")
				return "修改";
			else if(Type == "3")
				return "删除";
			else
				return "Unknown";
		}

		private string StateStr(string State)
		{
			if(State == "-3")
				return "等待客服审批";
			else if(State == "0")
				return "等待资质审批";
			else if(State == "3")
				return "审批通过";
			else if(State == "4")
				return "审批不通过";
			else
				return "Unknown";
		}

		protected void btnApprove_Click(object sender, System.EventArgs e)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				if(DomainPanel.Visible)
					qs.ApproveSpidDomain(this.lblID.Text,ViewState["uid"].ToString(),true,"");
				else if(EmailPanel.Visible)
					qs.ApproveSpidEmail(this.lblID.Text,ViewState["uid"].ToString(),true,"");
				else if(CompanyNamePanel.Visible)
					qs.ApproveSpidCompanyName(this.lblID.Text,ViewState["uid"].ToString(),true,"");

				this.btnApprove.Visible = false;
				this.btnCancel.Visible = false;
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"审核失败！" + ex.Message.ToString());
			}
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				qs.ApproveSpidDomain(this.lblID.Text,ViewState["uid"].ToString(),false,this.txtReason.Text.Trim());
				this.btnApprove.Visible = false;
				this.btnCancel.Visible = false;
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"拒绝失败！" + ex.Message.ToString());
			}
		}


	}
}
