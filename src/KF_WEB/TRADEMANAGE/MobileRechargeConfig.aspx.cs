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
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// Summary description for MobileRechargeConfig.
	/// </summary>
	public partial class MobileRechargeConfig : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!classLibrary.ClassLib.ValidateRight("MobileConfig",this)) Response.Redirect("../login.aspx?wh=1");
			renderTable();
		}

		void renderTable()
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			DataSet ds = qs.QueryMobilProduct();
			if(ds == null || ds.Tables.Count == 0)
			{
				return;
			}
			DataTable newDT = new DataTable();
			newDT.Columns.Add("operator", typeof(string));
			newDT.Columns.Add("parValue", typeof(string));
			newDT.Columns.Add("solution", typeof(string));
			string cardType = string.Empty;
			string cardValue = string.Empty;
			int rowIndex = -1;
			foreach(DataRow dataRow in ds.Tables[0].Rows)
			{
				string typeS = dataRow["Fcard_type"].ToString();
				string valueS = dataRow["Fcard_value"].ToString();
				string supplierName =  hashName[dataRow["Fsupplier_id"].ToString()].ToString();
				string min = dataRow["Fmin_ref"].ToString();
				string max = dataRow["Fmax_ref"].ToString(); 
				string percentage = getPercentage(min, max);
				if(typeS == cardType && valueS == cardValue)
				{
					if(rowIndex > -1)
					{
						newDT.Rows[rowIndex]["solution"] = newDT.Rows[rowIndex]["solution"].ToString()+", "+supplierName+": "+percentage;
					}
				}
				else
				{
					DataRow row = newDT.NewRow();
					row["operator"] = typeS;
					row["parValue"] = valueS;
					row["solution"] = supplierName+": "+percentage;
					newDT.Rows.Add(row);
					rowIndex++;
					cardType = typeS;
					cardValue = valueS;
				}
			}
			
			foreach(DataRow dataRow in newDT.Rows)
			{
				HtmlTableRow row = new HtmlTableRow();
				HtmlTableCell cellOperator = new HtmlTableCell();
				cellOperator.InnerText = getOperatorName(dataRow["operator"].ToString());
				HtmlTableCell cellParValue = new HtmlTableCell();
				cellParValue.InnerText = classLibrary.setConfig.FenToYuan(dataRow["parValue"].ToString());
				HtmlTableCell cellSolution = new HtmlTableCell();		
				cellSolution.InnerText = dataRow["solution"].ToString();;
				HtmlTableCell cellModify = new HtmlTableCell();
				cellModify.InnerText = "更改方案";
				cellModify.Attributes.Add("style","cursor:pointer;text-decoration:underline");
				cellModify.Attributes.Add("onclick",string.Format("modify('{0}','{1}','{2}')", dataRow["operator"].ToString(), 
					dataRow["parValue"].ToString(), cellOperator.InnerText + cellParValue.InnerText));
				row.Cells.Add(cellOperator);
				row.Cells.Add(cellParValue);
				row.Cells.Add(cellSolution);
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

		string getOperatorName(string num)
		{
			if(num == "1")
			{
				return "移动";
			}
			else if(num == "2")
			{
				return "联通";
			}
			else if(num == "3")
			{
				return "电信";
			}
			else return num;
		}


		string getPercentage(string min, string max)
		{
			int minN = int.Parse(min);
			int maxN = int.Parse(max);
			return Convert.ToString(maxN - minN + 1)+"%";
		}

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
