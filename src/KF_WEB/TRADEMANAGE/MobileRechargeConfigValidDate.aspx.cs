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

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// Summary description for MobileRechargeConfigValidDate.
	/// </summary>
	public partial class MobileRechargeConfigValidDate : System.Web.UI.Page
	{
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!classLibrary.ClassLib.ValidateRight("MobileConfig",this)) Response.Redirect("../login.aspx?wh=1");
			renderTable();
		}

		void renderTable()
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			DataSet ds = qs.QuerySupplierValidDate();
			if(ds == null || ds.Tables.Count == 0)
			{
				return;
			}
			foreach(DataRow dataRow in ds.Tables[0].Rows)
			{
				HtmlTableRow row = new HtmlTableRow();
				string supplierID = dataRow["Fsupplier_id"].ToString();
				string startTime = dataRow["Fstart_time"].ToString();
				string endTime = dataRow["Fend_time"].ToString();
				string name = string.Empty;
				if(hashName.ContainsKey(supplierID))
				{
					name = hashName[supplierID].ToString();
				}
				HtmlTableCell cellName = new HtmlTableCell();
				cellName.InnerText = name;
				HtmlTableCell cellStart = new HtmlTableCell();
				cellStart.InnerText = startTime;
				HtmlTableCell cellEnd = new HtmlTableCell();		
				cellEnd.InnerText = endTime;
				HtmlTableCell cellModify = new HtmlTableCell();
				cellModify.InnerText = "ÐÞ¸Ä";
				cellModify.Attributes.Add("style","cursor:pointer;text-decoration:underline");
				cellModify.Attributes.Add("onclick",string.Format("modify('{0}','{1}','{2}')", supplierID, startTime, endTime));
				row.Cells.Add(cellName);
				row.Cells.Add(cellStart);
				row.Cells.Add(cellEnd);
				row.Cells.Add(cellModify);
				contentTable.Rows.Add(row);
			}
		}


		Hashtable hashName
		{
			get
			{
				if(_hashName == null)
				{
					_hashName = new Hashtable();
					Query_Service.Query_Service qs = new Query_Service.Query_Service();
					DataSet ds = qs.QuerySupplierName();
					if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
					{
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							string supplierID = row["Fsupply_id"].ToString();
							if(supplierID != "" && !_hashName.ContainsKey(supplierID))
							{
								_hashName.Add(supplierID,row["Fsupply_name"].ToString());
							}
						}
					}
				}
				return _hashName;
			}
		}


		Hashtable _hashName;

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
