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
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.VIPAccount
{
	/// <summary>
	/// Summary description for BaseInformationQuery.
	/// </summary>
	public partial class BaseInformationQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			account.Text = string.Empty;
			firstTime.Text = string.Empty;
			yidiantongTime.Text = string.Empty;
			certificationTime.Text = string.Empty;
			level.Text = string.Empty;
			expiration.Text = string.Empty;
			balance.Text = string.Empty;
			vipType.Text = string.Empty;
			lastMissionTime.Text = string.Empty;
			realNameTime.Text = string.Empty;
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				string accountInput = txtQQ.Text.Trim();
                DataSet ds = new VIPService().QueryCFTMember(accountInput);
				if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count != 1)
				{
					account.Text = "该帐号不存在！";
				}
				else
				{
					DataRow row = ds.Tables[0].Rows[0];
					account.Text = row["Fuin"].ToString();
					expiration.Text = row["Fvip_exp_date"].ToString();
					DateTime timeStamp = new DateTime(1900,1,1,0,0,0);	//时间戳为1900年
					balance.Text = row["Fvalue"].ToString();

					if(row["Flast_trans_time"] is DBNull)
					{
					}
					else
					{
						lastMissionTime.Text = timeStamp.AddSeconds(Convert.ToDouble(row["Flast_trans_time"])).ToString();
					}
					
					if(row["Fvipflag"]  is DBNull)
					{}
					else
					{
						int vip = Convert.ToInt32(row["Fvipflag"]);
						if(vip == (int)VipType.Vip)
						{
							vipType.Text = "VIP";
							level.Text = getLevel(row["Fvalue"].ToString());
						}
						else if(vip == (int)VipType.SVip)
						{
							vipType.Text = "SVIP";
							level.Text = getLevel(row["Fvalue"].ToString());
						}
						else if(vip == (int)VipType.Degrade)
						{
							vipType.Text = "降级用户";
							level.Text = "0";
						}
						else if(vip == (int)VipType.Inactive)
						{
							vipType.Text = "未激活用户";
							level.Text = "0";
						}
					}
				
					DataSet advancedInfo = qs.QueryCFTMemberAdvanced(accountInput);
					if(advancedInfo == null || advancedInfo.Tables.Count < 1 || advancedInfo.Tables[0].Rows.Count < 1)
					{
						return;
					}
					else
					{
						foreach(DataRow infoRow in advancedInfo.Tables[0].Rows)
						{
							int Fvalue = 0;
							try
							{
								Fvalue = Convert.ToInt32(infoRow["Finfos"]);
							}
							catch{}
							if(Fvalue != 1)
							{
								continue;
							}
							string Ftype = infoRow["Ftype"].ToString().Trim();
							string date = Convert.ToDateTime(infoRow["fmod_time"]).ToString();
							if(Ftype == "firstvip")
							{
								firstTime.Text = date;
							}
							else if(Ftype == "oneclick")
							{
								yidiantongTime.Text = date;
							}
							else if(Ftype == "authname")
							{
								realNameTime.Text = date;
							}
							else if(Ftype == "cert")
							{
								certificationTime.Text = date;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				loger.err("BaseInformationQuery", ex.Message);
				Response.Write("<script language=javascript>alert('查询记录出错')</script>");
			}
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
			this.btnSearch.Click+=new EventHandler(btnSearch_Click);
		}
		#endregion

		private string getLevel(string growthValue)
		{
			if(growthValue == null || growthValue == "")
			{
				return "0";
			}
			try
			{
				long growth = Convert.ToInt64(growthValue);
				if(growth <= 199)
					return "0";
				else if(200 <= growth && growth <= 1499)
					return "1";
				else if(1500 <= growth && growth <= 6999)
					return "2";
				else if(7000 <= growth && growth <= 17999)
					return "3";
				else if(18000 <= growth && growth <= 49999)
					return "4";
				else if(50000 <= growth && growth <= 119999)
					return "5";
				else if(120000 <= growth && growth <= 2147483647)
					return "6";
				return "0";
			}
			catch
			{
				return "0";
			}
		}
	}

	public enum VipType
	{
		Inactive = 0, //未激活用户
		Vip = 1,		//VIP
		SVip = 2,		//SVIP
		Degrade = 4		//降级用户
	}
}
