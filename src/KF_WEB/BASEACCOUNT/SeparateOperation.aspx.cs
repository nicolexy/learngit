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
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// UserClassQuery 的摘要说明。
	/// </summary>
	public partial class SeparateOperation : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{


		protected System.Web.UI.WebControls.DataGrid dgListDetail;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
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

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				pager.RecordCount= 1000;//GetCount(); 
				BindData(1);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}

		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}
		private void BindData(int index)
		{
			string flist = this.TextBoxFlistid.Text.Trim();
			int max = pager.PageSize;
			int start = max * (index-1);
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetSeparateOperationList(flist,start,max);

			if(ds != null && ds.Tables.Count > 0)
			{
				ds.Tables[0].Columns.Add("subjectStr",typeof(string));
				ds.Tables[0].Columns.Add("paynumInt",typeof(string));
				ds.Tables[0].Columns.Add("paynumOut",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
                    dr["subjectStr"] = Transfer.convertSubject(dr["subject"].ToString());
					if(dr["type"].ToString().Trim() == "1")
					{
						dr["paynumInt"] = MoneyTransfer.FenToYuan(dr["paynum"].ToString());
						dr["paynumOut"] = "0";
					}
					else if(dr["type"].ToString().Trim() == "2")
					{
						dr["paynumInt"] = "0";
						dr["paynumOut"] = MoneyTransfer.FenToYuan(dr["paynum"].ToString());
					}
					else
					{
						dr["paynumInt"] = "Unknown";
						dr["paynumOut"] = "Unknown";
					}
                    string spid = dr["listid"].ToString().Substring(0, 10);
                    if (!PublicRes.isWhiteOfSeparate(spid, Session["uid"].ToString())) 
                    {
                        //不在白名单
                        string s_fromid = dr["fromId"].ToString();
                        int idx = s_fromid.IndexOf("@");
                        dr.BeginEdit();
                        if (idx > -1)
                        {
                            //email
                            string s_st = s_fromid.Substring(0, idx);
                            string s_et = s_fromid.Substring(idx+1);
                            dr["fromId"] = classLibrary.setConfig.ConvertID(s_st, 0, 2) + "@" + s_et;
                        }
                        else { 
                            //数字，保留最后2位
                            dr["fromId"] = classLibrary.setConfig.ConvertID(s_fromid, 0, 2);
                        }
                        string s_fromname = dr["fromName"].ToString();
                        dr["fromName"] = classLibrary.setConfig.ConvertID(s_fromname, 0, 2);
                        dr.EndEdit();
                    }
				}
				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！"); 
			}
		}

	}
}
