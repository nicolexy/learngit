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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// RefundQuery 的摘要说明。
	/// </summary>
	public partial class RefundQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 

			if(Session["uid"] == null)
				Response.Redirect("../login.aspx?wh=1");

			if (!IsPostBack)
			{
				try
				{
					Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
					object[] al = qs.GetAllRefundBank();
                    ddlBankTypeInit(al);
                    
					string fbatchid = Request["batchid"];
					if(fbatchid != null && fbatchid.Trim().Length == 13)
					{
						ViewState["batchid"] = fbatchid.Trim();

						TextBoxBeginDate.Text = fbatchid.Substring(0,4) + "-" + fbatchid.Substring(4,2) + "-" + fbatchid.Substring(6,2);
						ddlBankType.SelectedValue = fbatchid.Substring(8,4);

						//	pager.RecordCount= 1000;
						//	BindData(1);
					}
					else
					{
						ViewState["batchid"] = DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "9999R";

						TextBoxBeginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
						ddlBankType.SelectedValue = "9999";
					}
				}
				catch(Exception ex)
				{
					WebUtils.ShowMessage(this.Page,"初始化出错：" + ex.Message);
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
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

        /// <summary>
        /// 初始化ddlBankType item数据
        /// 按银行排序
        /// </summary>
        public void ddlBankTypeInit(object[] al)
        {
            Hashtable bankTypeName = new Hashtable();
            ddlBankType.Items.Clear();
            if (al != null)
            {
                foreach (Object obj in al)
                {
                    string banktype = obj.ToString();
                    string bankname = classLibrary.setConfig.convertbankType(banktype);
                    if (!bankTypeName.Contains(bankname))
                        bankTypeName.Add(bankname, banktype);
                }
            }

            ArrayList akeys = new ArrayList(bankTypeName.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                ddlBankType.Items.Add(new ListItem(k.ToString(), bankTypeName[k].ToString()));
            }

        }

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
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message);
			}
		}


		private int GetCount()
		{
			string batchid = ViewState["batchid"].ToString();
			int ifromtype = Int32.Parse(ddlFromType.SelectedValue);
			int irefundtype = Int32.Parse(ddlRefundType.SelectedValue);
			int irefundstate = Int32.Parse(ddlRefundState.SelectedValue);
			int ireturnstate = Int32.Parse(ddlReturnState.SelectedValue);
			string listid = tbListID.Text.Trim();
            string Fbank_listid = tbFbank_listid.Text.Trim();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.GetRefundListCount(batchid, ifromtype, irefundtype, irefundstate, ireturnstate, listid, Fbank_listid);
		}

		private void BindData(int index)
		{
			string batchid = ViewState["batchid"].ToString();
			int ifromtype = Int32.Parse(ddlFromType.SelectedValue);
			int irefundtype = Int32.Parse(ddlRefundType.SelectedValue);
			int irefundstate = Int32.Parse(ddlRefundState.SelectedValue);
			int ireturnstate = Int32.Parse(ddlReturnState.SelectedValue);
			string listid = tbListID.Text.Trim();
            string Fbank_listid = tbFbank_listid.Text.Trim();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

            DataSet ds = qs.GetRefundList(batchid, ifromtype, irefundtype, irefundstate, ireturnstate, listid, Fbank_listid, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("FreturnamtName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Freturnamt","FreturnamtName");

				ds.Tables[0].Columns.Add("FamtName",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Famt","FamtName");

				ds.Tables[0].Columns.Add("FstateName",typeof(String));
				ds.Tables[0].Columns.Add("FreturnStateName",typeof(String));

				ds.Tables[0].Columns.Add("FrefundPathName",typeof(String));
				
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();

					string tmp = dr["Fstate"].ToString();
					if(tmp == "0") 
						tmp = "初始状态";
					else if(tmp == "1") 
						tmp = "退单流程中";
					else if(tmp == "2") 
						tmp = "退单成功";
					else if(tmp == "3") 
						tmp = "退单失败";
					else if(tmp == "4") 
						tmp = "退单状态未定";
					else if(tmp == "5") 
						tmp = "手工退单中";
					else if(tmp == "6") 
						tmp = "申请手工退单";
					else if(tmp == "7") 
						tmp = "申请转入代发";

					
					dr["FstateName"] = tmp;

					tmp = dr["FreturnState"].ToString();
					if(tmp == "1") 
						tmp = "回导前";
					else if(tmp == "2") 
						tmp = "回导后";					

					dr["FreturnStateName"] = tmp;

					tmp = dr["FrefundPath"].ToString();
					if(tmp == "1") 
						tmp = "网银退单";
					else if(tmp == "2") 
						tmp = "接口退单";
					else if(tmp == "3") 
						tmp = "人工授权";
					else if(tmp == "4") 
						tmp = "转帐退单";
					else if(tmp == "5") 
						tmp = "转入代发";
					else if(tmp == "6") 
						tmp = "付款退款";
					
					dr["FrefundPathName"] = tmp;

					dr.EndEdit();
				}

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}


        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            labErrMsg.Text = "";

            string date = DateTime.Parse(TextBoxBeginDate.Text).ToString("yyyyMMdd");
            string bank = ddlBankType.SelectedValue;
            
            //场次：1 2 3 4 5，对应字母：R T U W Y
            //默认选择第一场
            string batchid = date + bank + "R";
            if (ddlFbatchid.SelectedValue != "0")
            {
                batchid = date + bank + ddlFbatchid.SelectedValue;
            }
            ViewState["batchid"] = batchid;

            pager.RecordCount = 1000;//GetCount(); 

            BindData(1);

        }

	}
}
