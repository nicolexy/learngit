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
using System.Configuration;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// PhoneBillQuery 的摘要说明。
	/// </summary>
	public partial class PhoneBillQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
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


		private void BindData(int index)
		{
			string strOutMsg = "";

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

			DataSet ds;
			if(rbtFlistID.Checked)
			{

				ds = qs.GetPhoneBillRecordByTransID(tbListID.Text.Trim(), out strOutMsg);
							
			}
			else
			{
				ds = qs.GetPhoneBillRecordByPhoneNumber(tbPhoneNo.Text.Trim(), out strOutMsg);
			}
			

				if(ds != null && ds.Tables.Count >0)
				{
					ds.Tables[0].Columns.Add("FTotalFee_yuan",typeof(String));
					ds.Tables[0].Columns.Add("FAmount_yuan",typeof(String));
					ds.Tables[0].Columns.Add("FComment_conv",typeof(String));
					ds.Tables[0].Columns.Add("Fstate_conv",typeof(String));
					ds.Tables[0].Columns.Add("FSpName_conv", typeof(String));
                    ds.Tables[0].Columns.Add("FUserStateName", typeof(String));//用户登录状态

					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						dr.BeginEdit();

						dr["FTotalFee_yuan"]	= MoneyTransfer.FenToYuan(dr["FTotalFee"].ToString());
						dr["FAmount_yuan"]		= MoneyTransfer.FenToYuan(dr["FAmount"].ToString());
                      
                        switch (dr["FUserState"].ToString())
                        {
                            case "0":
                                dr["FUserStateName"] = "匿名充值"; break;
                            case "1":
                                dr["FUserStateName"] = "QQ用户充值"; break;
                            case "2":
                                dr["FUserStateName"] = "QQ注册用户充值"; break;
                            case "3":
                                dr["FUserStateName"] = "EMAIL注册用户充值"; break;
                            default:
                                dr["FUserStateName"] = "未知"; break;
                        }

						switch (dr["FComment"].ToString().Substring(0,1))
						{
							case "1":
								dr["FComment_conv"] = "快充";break;
							case "2":
								dr["FComment_conv"] = "慢充(48小时)";break;
							case "3":
								dr["FComment_conv"] = "慢充(12小时)";break;
							case "4":
								dr["FComment_conv"] = "慢充(2小时)";break;
							default:
								dr["FComment_conv"] = "未知";break;
						}


						switch( dr["Fstate"].ToString())
						{
							case "1":
								dr["Fstate_conv"] = "预查询";break;
							case "2":
								dr["Fstate_conv"] = "预充值";break;
							case "3":
								dr["Fstate_conv"] = "充值请求发送成功";break;
							case "4":
								dr["Fstate_conv"] = "充值请求发送失败";break;
							case "13":
							case "14":
								dr["Fstate_conv"] = "充值成功";break;						
							case "23":
							case "24":
								dr["Fstate_conv"] = "已退款";break;		
							case "33":
							case "34":
								dr["Fstate_conv"] = "充值失败";break;
							default:
								dr["Fstate_conv"] = "状态位置:"+dr["Fstate"].ToString();
								break;
						}

						switch( dr["FSpName"].ToString() )
						{
							case "gy":
								dr["FSpName_conv"] = "高阳捷迅";break;
							case "jtd":
								dr["FSpName_conv"] = "捷通达";break;
							case "zt":
								dr["FSpName_conv"] = "中腾";break;
							case "of":
								dr["FSpName_conv"] = "欧飞";break;
							default:
								dr["FSpName_conv"] = dr["FSpName"].ToString();
								break;

						}

						dr.EndEdit();
					}



					dgList.DataSource = ds.Tables[0].DefaultView;
					dgList.DataBind();
				}
				else
				{
					throw new LogicException("没有找到记录！"+strOutMsg);
				}

		}

		private void dgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
// 			if(e.Item.ItemIndex > -1)
// 			{
// 				if(e.Item.Cells[0].Text.Trim() != "")
// 					e.Item.Cells[0].Text = DateTime.Parse(e.Item.Cells[0].Text.Trim()).ToString("yyyy-MM-dd");
// 				if(e.Item.Cells[2].Text.Trim() == "3001")
// 					e.Item.Cells[3].Text = "兴业银行";
// 				if(e.Item.Cells[5].Text.Trim() != "")
// 					e.Item.Cells[5].Text = MoneyTransfer.FenToYuan(e.Item.Cells[5].Text.Trim());
// 				if(e.Item.Cells[6].Text.Trim() == "1")
// 					e.Item.Cells[6].Text = "成功";
// 				else if(e.Item.Cells[6].Text.Trim() == "2")
// 					e.Item.Cells[6].Text = "失败";
// 				else if(e.Item.Cells[6].Text.Trim() == "3" || e.Item.Cells[6].Text.Trim() == "4")
// 					e.Item.Cells[6].Text = "还款中";
// 				else
// 					e.Item.Cells[6].Text = "Unknow";
//			}
		}

		protected void Textbox1_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
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
	}
}
