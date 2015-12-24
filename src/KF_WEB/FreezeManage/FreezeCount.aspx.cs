using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.FreezeManage
{
	/// <summary>
	/// FreezeCount 的摘要说明。
	/// </summary>
	public partial class FreezeCount : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox tbx_payAccount;
		protected System.Web.UI.WebControls.TextBox tbx_listNo;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			if(!IsPostBack)
			{
				this.tbx_beginDate.Value = DateTime.Now.AddMonths(-1).AddDays(1).ToString("yyyy-MM-dd");
                this.tbx_endDate.Value = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
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


		class DataStruct
		{
			public string handleUserName;
			public int unHandleNum;
			public int hangUpNum;
			public int Fin1Num;
			public int Fin2Num;
			public int DiscardNum;
			public int AddRecordNum;
            public int adddatanumsum;
			public int totalNum;
		}


		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			DateTime beginDate;
			DateTime endDate;
			string handleUserID = "";
			string handleResult = "";
            string handleType =string.Join(",", CheckBoxList1.Items.Cast<ListItem>().Where(u=>u.Selected).Select(u=>u.Value).ToArray());

			try
			{
                beginDate = DateTime.Parse(this.tbx_beginDate.Value);
                endDate = DateTime.Parse(this.tbx_endDate.Value);
			}
			catch
			{
				WebUtils.ShowMessage(this,"日期格式不正确");
				return;
			}

			if(beginDate.AddMonths(1) < endDate)
			{
				WebUtils.ShowMessage(this,"日期跨度大于1个月，请重新输入");
				return;
			}

			handleUserID = this.tbx_freezeHandleUserID.Text.Trim();

           
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			DataSet ds = qs.GetFreezeDiary("","",handleType,handleUserID,handleResult,"",beginDate.ToString("yyyy-MM-dd")
				,endDate.ToString("yyyy-MM-dd"),-1,20);

			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"查询结果为空");
				this.DataGrid_QueryResult.DataSource = null;
				this.DataGrid_QueryResult.DataBind();
				return;
			}
			
			DataSet ds2 = new DataSet();
			ds2.Tables.Add(new DataTable());
			ds2.Tables[0].Columns.Add("showTitle",typeof(string));
			ds2.Tables[0].Columns.Add("unHandle_Num",typeof(string));
			ds2.Tables[0].Columns.Add("handUp_Num",typeof(string));
			ds2.Tables[0].Columns.Add("Fin1_Num",typeof(string));
			ds2.Tables[0].Columns.Add("Fin2_Num",typeof(string));
			ds2.Tables[0].Columns.Add("AddRecordNum",typeof(string));
            ds2.Tables[0].Columns.Add("adddatanumsum", typeof(string));
			ds2.Tables[0].Columns.Add("Discard_Num",typeof(string));
			ds2.Tables[0].Columns.Add("TotalNum",typeof(string));

			ArrayList arKeys = new ArrayList();
			ArrayList arValues = new ArrayList();

			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				string handleUser = dr["FHandleUser"].ToString();
				int iType = int.Parse(dr["FHandleType"].ToString());
				DataStruct data = null
					;
				if(!arKeys.Contains(handleUser))
				{
					data = new DataStruct();
					data.handleUserName = handleUser;

					arKeys.Add(handleUser);
					arValues.Add(data);
				}
				else
				{
					data = (DataStruct)arValues[arKeys.IndexOf(handleUser)];
				}

                switch (iType)
                {
                    case 0:
                        { data.unHandleNum++; break; }
                    case 1:
                        { data.Fin1Num++; break; }
                    case 2:
                        { data.Fin2Num++; break; }
                    case 7:
                        { data.DiscardNum++; break; }
                    case 8:
                        { data.hangUpNum++; break; }
                    case 11:
                        { data.adddatanumsum++; break; }
                    case 100:
                        { data.AddRecordNum++; break; }
                    default:
                        { break; }
                }
				data.totalNum++;
			}

			for(int i=0;i<arValues.Count;i++)
			{
				DataStruct data = (DataStruct)arValues[i];

				ds2.Tables[0].Rows.Add(new object[]{ data.handleUserName,data.unHandleNum.ToString(),data.hangUpNum.ToString(),
				data.Fin1Num.ToString(),data.Fin2Num.ToString(),data.AddRecordNum.ToString(),data.adddatanumsum.ToString(),data.DiscardNum.ToString(),data.totalNum.ToString()});
			}

			this.DataGrid_QueryResult.DataSource = ds2;
			this.DataGrid_QueryResult.DataBind();
		}
	}
}
