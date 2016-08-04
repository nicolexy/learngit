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
	/// Summary description for MobileRechargeConfigEdit.
	/// </summary>
	public partial class MobileRechargeConfigEdit : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!classLibrary.ClassLib.ValidateRight("MobileConfig",this)) Response.Redirect("../login.aspx?wh=1");
			if(!Page.IsPostBack)
			{
				Session["cardtype"] = Request["cardtype"];
				Session["cardvalue"] = Request["cardvalue"];
				this.title.InnerText = Request["title"];
			}
			renderTable();
		}

		void renderTable()
		{
			foreach(DictionaryEntry en in hashName)
			{
				HtmlTableRow row = new HtmlTableRow();
				HtmlTableCell cellName = new HtmlTableCell();
				cellName.InnerText = en.Value.ToString();
				HtmlInputText text = new HtmlInputText();
				text.ID = en.Key.ToString();
				if(hashValue.ContainsKey(text.ID))
				{
					text.Value = hashValue[text.ID].ToString();
				}
				HtmlTableCell cellInput = new HtmlTableCell();
				cellInput.Controls.Add(text);
				HtmlTableCell cellPercentage = new HtmlTableCell();
				cellPercentage.InnerText = "%";
				row.Cells.Add(cellName);
				row.Cells.Add(cellInput);
				row.Cells.Add(cellPercentage);
				contentTable.Rows.Add(row);
			}
		}

		public void modifyBtn_Click(object sender, System.EventArgs e)
		{
			Hashtable ha = new Hashtable();
			foreach(DictionaryEntry en in hashName)
			{
				try
				{
					string percentage = Request[en.Key.ToString()];
					if(percentage.Trim() == "")continue;
					int percent = int.Parse(percentage);
					if(percent <= 0)
					{
						ShowMsg("请输入正整数");
						return;
					}
					ha.Add(en.Key, percent);
				}
				catch
				{
					ShowMsg("请输入正整数");
					return;
				}
			}
			if(!validate100(ha))
			{
				ShowMsg("百分比总和不为100%");
				return;
			}
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			qs.DeleteMobileProduct(Session["cardtype"].ToString(), Session["cardvalue"].ToString());
			int sum = 0;
			foreach(DictionaryEntry en in ha)
			{
				int percent = Convert.ToInt32(en.Value);
				string supplierID = en.Key.ToString();
				qs.UpdateMobileProduct(Session["cardtype"].ToString(), Session["cardvalue"].ToString(), sum.ToString(), 
					Convert.ToString(sum + percent - 1), supplierID);
				sum = sum + percent;
			}
			_hashValue = null;
			Response.Write("<script language=javascript>window.parent.cancel();alert('更新成功');window.parent.location.reload();</script>");
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}


		bool validate100(Hashtable hash)
		{
			int sum = 0;
			foreach(DictionaryEntry en in hash)
			{
				sum += Convert.ToInt32(en.Value);
			}
			if(sum == 100)
			{
				return true;
			}
			else 
			{
				return false;
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

		Hashtable hashValue
		{
			get
			{
				if(_hashValue == null)
				{
					_hashValue = new Hashtable();
					Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
					DataSet ds = qs.QueryOneMobilProduct(Session["cardtype"].ToString(), Session["cardvalue"].ToString());
					if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
					{
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							string supplierID = row["Fsupplier_id"].ToString();
							if(supplierID != "" && !_hashValue.ContainsKey(supplierID))
							{
								try
								{
									int min = Convert.ToInt32(row["Fmin_ref"].ToString());
									int max = Convert.ToInt32(row["Fmax_ref"].ToString());
									_hashValue.Add(supplierID, max - min + 1);
								}
								catch
								{
									continue;
								}
							}
						}
					}
				}
				return _hashValue;
			}
		}


		Hashtable _hashValue;

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
