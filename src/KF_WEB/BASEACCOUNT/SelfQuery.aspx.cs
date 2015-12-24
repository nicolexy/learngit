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
using System.Collections.Generic;
using CFT.CSOMS.BLL.SPOA;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// SelfQuery 的摘要说明。
	/// </summary>
	public partial class SelfQuery : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataGrid dgListFlist;
	
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

                    ListItem li = new ListItem("全部","");
                    
					BoxCpStatus.Items.Clear();
					Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
					DataSet ds = qs.GetSelfTypeList();
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						BoxCpStatus.Items.Add(new ListItem(dr["DictName"].ToString(),dr["DictID"].ToString()));
					}

					ddlKFList.Items.Clear();
					ds = qs.GetSelfKFList();
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						ddlKFList.Items.Add(new ListItem(dr["KFCheckUser"].ToString(),dr["KFCheckUser"].ToString()));
					}
                    
                    ddlMerType.Items.Clear();
                    ds = new SPOAService().GetSelfQuerySPType(); 
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ddlMerType.Items.Add(new ListItem(dr["text"].ToString(), dr["value"].ToString()));
                    }

					BoxCpStatus.Items.Add(li);
					ddlKFList.Items.Add(li);
                    ddlMerType.Items.Add(li);
					li.Selected = true;
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
				pager.RecordCount = GetCount(); 
				lblResultCount.Text = pager.RecordCount.ToString();
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

            string SPID = (BoxCpNumber.Text.Trim() == "") ? "" : BoxCpNumber.Text.Trim();
            int? DraftFlag = null;
            string CompanyName = (BoxCpName.Text.Trim() == "") ? "" : BoxCpName.Text.Trim();
            int? Flag = null;
            if (BoxCpStatus.SelectedValue != "")
            {
                Flag = int.Parse(BoxCpStatus.SelectedValue);
            }
            string WWWAdress = (boxWWWAddress.Text.Trim() == "") ? "" : boxWWWAddress.Text.Trim();
            string Appid = (txtAppid.Text.Trim() == "") ? "" : txtAppid.Text.Trim();
            DateTime? ApplyTimeStart = null;
            if (!string.IsNullOrEmpty(TextBoxBeginDate.Value))
            {
                string strBeginTime = Convert.ToDateTime(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00");
                ApplyTimeStart = Convert.ToDateTime(strBeginTime);
            }

            DateTime? ApplyTimeEnd = null;
            if (!string.IsNullOrEmpty(TextBoxEndDate.Value))
            {
                string strEndTime = Convert.ToDateTime(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59");
                ApplyTimeEnd = Convert.ToDateTime(strEndTime);
            }


            string BankUserName = (tbBankName.Text.Trim() == "") ? "" : tbBankName.Text.Trim();
            string KFCheckUser = (ddlKFList.SelectedValue == "") ? "" : ddlKFList.SelectedValue;
            string SuggestUser = (tbSuggestUser.Text.Trim() == "") ? "" : tbSuggestUser.Text.Trim();
            string MerType = (ddlMerType.SelectedValue == "") ? "" : ddlMerType.SelectedValue;
            //(BoxCpNumber.Text.Trim(), 0, BoxCpName.Text.Trim(), BoxCpStatus.SelectedValue, boxWWWAddress.Text.Trim(), txtAppid.Text.Trim(), beginTime, endTime, tbBankName.Text.Trim(), ddlKFList.SelectedValue, tbSuggestUser.Text.Trim()," ddlMerType.SelectedValue");
            return new SPOAService().GetSelfQueryListCount(SPID, DraftFlag, CompanyName, Flag, WWWAdress, Appid, ApplyTimeStart, ApplyTimeEnd, BankUserName, KFCheckUser, SuggestUser, MerType);
		}

		private void BindData(int index)
		{
			string filter = ViewState["filter"].ToString();
            string[] filter1 = GetfilterArrString();  //修改为数组方式查询 yinhuang 2014/05/12
			int TopCount = pager.PageSize;
			int NotInCount = TopCount * (index-1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

            string SPID = (BoxCpNumber.Text.Trim() == "") ? "" : BoxCpNumber.Text.Trim();
            int? DraftFlag = null;
            string CompanyName = (BoxCpName.Text.Trim() == "") ? "" : BoxCpName.Text.Trim();
            int? Flag = null;
            if (BoxCpStatus.SelectedValue != "")
            {
                Flag = int.Parse(BoxCpStatus.SelectedValue);
            }
            string WWWAdress = (boxWWWAddress.Text.Trim() == "") ? "" : boxWWWAddress.Text.Trim();
            string Appid = (txtAppid.Text.Trim() == "") ? "": txtAppid.Text.Trim();
            DateTime? ApplyTimeStart = null;
            if (!string.IsNullOrEmpty(TextBoxBeginDate.Value))
            {
                string strBeginTime = Convert.ToDateTime(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00");
                ApplyTimeStart = Convert.ToDateTime(strBeginTime);
            }

            DateTime? ApplyTimeEnd = null;
            if (!string.IsNullOrEmpty(TextBoxEndDate.Value))
            {
                string strEndTime = Convert.ToDateTime(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59");
                ApplyTimeEnd = Convert.ToDateTime(strEndTime);
            }
            
            
            string BankUserName = (tbBankName.Text.Trim() == "") ? "" : tbBankName.Text.Trim();
            string KFCheckUser = (ddlKFList.SelectedValue == "") ? "" : ddlKFList.SelectedValue;
            string SuggestUser = (tbSuggestUser.Text.Trim() == "") ? "" : tbSuggestUser.Text.Trim();
            string MerType = (ddlMerType.SelectedValue == "") ? "" : ddlMerType.SelectedValue;
            DataSet ds = new SPOAService().GetSelfQueryList(SPID, DraftFlag, CompanyName, Flag, WWWAdress, Appid, ApplyTimeStart, ApplyTimeEnd, BankUserName, KFCheckUser, SuggestUser, MerType, TopCount, NotInCount);

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


		#region 获得查询字符串
		protected string GetfilterString()
		{
			string filter = " DraftFlag=0 ";
            string filterNew = " and DraftFlag=0 ";

			if(BoxCpName.Text.Trim() != "")
			{
				filter += " and CompanyName like '%" + BoxCpName.Text.Trim() + "%' ";
                filterNew += " and CompanyName.Contains('" + BoxCpName.Text.Trim() + "') ";
			}

			if(BoxCpNumber.Text.Trim() != "")
			{
				filter += " and SPID='" + BoxCpNumber.Text.Trim() + "' ";
                filterNew += " and SPID='" + BoxCpNumber.Text.Trim() + "' ";
			}

			if(BoxCpStatus.SelectedValue != "")
			{
				filter += " and Flag=" + BoxCpStatus.SelectedValue + " ";
                filterNew += " and Flag=" + BoxCpStatus.SelectedValue + " ";
			}

			if(boxWWWAddress.Text.Trim() != "")
			{
				filter += " and WWWAdress like '%" + boxWWWAddress.Text.Trim() + "%' ";
                filterNew += " and WWWAdress.Contains('" + boxWWWAddress.Text.Trim() + "') ";
			}

            if (txtAppid.Text.Trim() != "")
            {
                filter += " and appid='" + txtAppid.Text.Trim() + "' ";
                filterNew += " and appid='" + txtAppid.Text.Trim() + "' ";
            }

            if (TextBoxBeginDate.Value.Trim() != "")
			{
                filter += " and ApplyTime >='" + Convert.ToDateTime(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00") + "' ";
                //filterNew += " and ApplyTime.Value.ToString() >='" + Convert.ToDateTime(TextBoxBeginDate.Text.Trim()).ToString("yyyy-MM-dd 00:00:00") + "' ";
                DateTime d1 = Convert.ToDateTime(TextBoxBeginDate.Value.Trim());
                filterNew += " and ApplyTime >=DateTime("+d1.Year+","+d1.Month+","+d1.Day+",00,00,00)";
			}

            if (TextBoxEndDate.Value.Trim() != "")
			{
                filter += " and ApplyTime <='" + Convert.ToDateTime(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59") + "' ";
                //filterNew += " and ApplyTime.Value.ToString() <='" + Convert.ToDateTime(TextBoxEndDate.Text.Trim()).ToString("yyyy-MM-dd 23:59:59") + "' ";
                DateTime d1 = Convert.ToDateTime(TextBoxEndDate.Value.Trim());
                filterNew += " and ApplyTime <=DateTime(" + d1.Year + "," + d1.Month + "," + d1.Day + ",23,59,59)";
			}

			if(tbBankName.Text.Trim() != "")
			{
				filter += " and BankUserName like '%" + tbBankName.Text.Trim() + "%' ";
                filterNew += " and BankUserName.Contains('" + tbBankName.Text.Trim() + "') ";
			}

			if(ddlKFList.SelectedValue != "")
			{
				filter += " and KFCheckUser = '" + ddlKFList.SelectedValue + "' ";
                filterNew += " and KFCheckUser = '" + ddlKFList.SelectedValue + "' ";
			}

			if(tbSuggestUser.Text.Trim() != "")
			{
				filter += " and SuggestUser = '" + tbSuggestUser.Text.Trim() + "' ";
                filterNew += " and SuggestUser = '" + tbSuggestUser.Text.Trim() + "' ";
			}

            /*
			if(ddlMerType.SelectedValue!="9")
			{
				if(ddlMerType.SelectedValue!="2")
				{
					filter += " and datafrom=" + ddlMerType.SelectedValue + " and Fagentid is null";
                    filterNew += " and datafrom=" + ddlMerType.SelectedValue + " and Fagentid = null";

				}
				else
				{
					filter += " and datafrom=0  and Fagentid is not null";
                    filterNew += " and datafrom=0  and Fagentid != null";
				}
			}
            */
            //修改商户类型查询条件，从spoa接口获取 2014/08/19 yinhuang
            if (ddlMerType.SelectedValue != "") 
            {
                filter += " and " + ddlMerType.SelectedValue;
                filterNew += " and " + ddlMerType.SelectedValue;
            }

            return filterNew;			
		}

        protected string[] GetfilterArrString()
        {
            List<string> temp = new List<string>();
            string filter = " DraftFlag=0 ";
            temp.Add("DraftFlag=(0)");

            if (BoxCpName.Text.Trim() != "")
            {
                filter += " and CompanyName like '%" + BoxCpName.Text.Trim() + "%' ";
                temp.Add("CompanyName like (" + BoxCpName.Text.Trim() + ")");
            }

            if (BoxCpNumber.Text.Trim() != "")
            {
                filter += " and SPID='" + BoxCpNumber.Text.Trim() + "' ";
                temp.Add("SPID=(" + BoxCpNumber.Text.Trim() + ")");
            }

            if (BoxCpStatus.SelectedValue != "")
            {
                filter += " and Flag=" + BoxCpStatus.SelectedValue + " ";
                temp.Add("Flag=(" + BoxCpStatus.Text.Trim() + ")");
            }

            if (boxWWWAddress.Text.Trim() != "")
            {
                filter += " and WWWAdress like '%" + boxWWWAddress.Text.Trim() + "%' ";
                temp.Add("WWWAdress like (" + boxWWWAddress.Text.Trim() + ")");
            }

            if (TextBoxBeginDate.Value.Trim() != "")
            {
                filter += " and ApplyTime >='" + Convert.ToDateTime(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00") + "' ";
                temp.Add("ApplyTime >= (" + Convert.ToDateTime(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00") + ")");
            }

            if (TextBoxEndDate.Value.Trim() != "")
            {
                filter += " and ApplyTime <='" + Convert.ToDateTime(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59") + "' ";
                temp.Add("ApplyTime <= (" + Convert.ToDateTime(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59") + ")");
            }

            if (tbBankName.Text.Trim() != "")
            {
                filter += " and BankUserName like '%" + tbBankName.Text.Trim() + "%' ";
                temp.Add("BankUserName like (" + tbBankName.Text.Trim() + ")");
            }

            if (ddlKFList.SelectedValue != "")
            {
                filter += " and KFCheckUser = '" + ddlKFList.SelectedValue + "' ";
                temp.Add("KFCheckUser=(" + ddlKFList.SelectedValue + ")");
            }

            if (tbSuggestUser.Text.Trim() != "")
            {
                filter += " and SuggestUser = '" + tbSuggestUser.Text.Trim() + "' ";
                temp.Add("SuggestUser=(" + tbSuggestUser.Text.Trim() + ")");
            }

            if (ddlMerType.SelectedValue != "9")
            {
                if (ddlMerType.SelectedValue != "2")
                {
                    filter += " and datafrom=" + ddlMerType.SelectedValue + " and Fagentid is null";
                    temp.Add("datafrom=(" + ddlMerType.SelectedValue + ")");
                    temp.Add("Fagentid is null");

                }
                else
                {
                    filter += " and datafrom=0  and Fagentid is not null";
                    temp.Add("datafrom=(0)");
                    temp.Add("Fagentid is not null");
                }
            }
            string[] filterArr = new string[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                filterArr[i] = temp[i];
            }

            return filterArr;
		}
		#endregion

		private void dgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemIndex > -1)
			{
				if(e.Item.Cells[10].Text.Trim() != "-2")
					e.Item.Cells[13].Text = "";
			}
		}

		private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName == "Select")
			{
				try
				{
					string UserID = Session["uid"].ToString();
					if(UserID == "")
					{
						throw new Exception("请重新登陆!");
					}
					else
					{
						Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
						qs.CheckTicket(e.Item.Cells[0].Text,UserID);
						WebUtils.ShowMessage(this.Page,"领单成功");
						btnSearch_Click(null,null);
					}
				}
				catch(Exception ex)
				{
					WebUtils.ShowMessage(this.Page,ex.Message);
				}
			}
		}
	}
}
