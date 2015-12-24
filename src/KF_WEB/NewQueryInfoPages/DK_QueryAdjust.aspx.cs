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
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// DK_QueryAdjust 的摘要说明。
	/// </summary>
	public partial class DK_QueryAdjust : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                if (!Page.IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("DKAdjust", this)) Response.Redirect("../login.aspx?wh=1");
                    
                    this.TextBoxBeginDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    this.TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

                    classLibrary.setConfig.GetAllBankList(ddlbanktype);
                    ddlbanktype.Items.Insert(0, new ListItem("所有银行", "0000"));
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

		}
		#endregion

		protected void btQuery_Click(object sender, System.EventArgs e)
		{
			//本版本增加审批，文件夹，新页面，ICE配置，数据库。
			//按参数查询数据。
            string starttime = DateTime.Parse(TextBoxBeginDate.Value.Trim()).ToString("yyyy-MM-dd 00:00:00");
            string endtime = DateTime.Parse(TextBoxEndDate.Value.Trim()).ToString("yyyy-MM-dd 23:59:59");
			string banktype = ddlbanktype.SelectedValue;
			string spid = txbMerchant.Text.Trim();
			string coding = txbOrder.Text.Trim();
			string bank_list = txbMoney.Text.Trim();
			string bankaccno = txtBankOrder.Text.Trim();
			string uname = Textbox1.Text.Trim();

			Query_Service.Query_Service fs = new Query_Service.Query_Service();

			DataSet ds = fs.DK_QueryCheckDetailInfo(starttime,endtime,banktype,spid,coding,bank_list,bankaccno,uname);

			if(ds != null && ds.Tables.Count>0 && ds.Tables[0] != null)
			{
				ds.Tables[0].Columns.Add("Fbank_typename");
					ds.Tables[0].Columns.Add("Fpaynumname");
					ds.Tables[0].Columns.Add("Fendstatename");
					ds.Tables[0].Columns.Add("Fcheckstatename");

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();
					dr["Fbank_typeName"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
					dr["Fpaynumname"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());

					if(dr["Fendstate"].ToString() == "9")
					{
						dr["Fendstatename"] = "调整失败";
					}
					else
						dr["Fendstatename"] = "调整成功";

					string tmp = dr["Fcheckstate"].ToString();
					if(tmp == "0")
					{
						dr["Fcheckstatename"] = "待发起审批";
					}
					else if(tmp == "1")
					{
						dr["Fcheckstatename"] = "已发起审批";
					}
					else if(tmp == "2")
					{
						dr["Fcheckstatename"] = "审批已执行";
					}
					else if(tmp == "3")
					{
						dr["Fcheckstatename"] = "审批已撤消";
					}
						
					dr.EndEdit();
				}
				

				dgInfo.DataSource = ds.Tables[0].DefaultView;
				dgInfo.DataBind();
			}
		}
	}
}
