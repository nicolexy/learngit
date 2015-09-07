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
using CFT.CSOMS.BLL.SPOA;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// AgencyBusinessQuery 的摘要说明。
	/// </summary>
    public partial class ShouFuYiQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					this.divInfo.Visible = false;
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
			this.dgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgList_PageIndexChanged);

		}
		#endregion

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.divInfo.Visible = false;

			try
			{
				dgList.CurrentPageIndex = 0;
				BindData();
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				this.dgList.Visible = false;
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				this.dgList.Visible = false;
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message);
			}
		}

		private void BindData()
		{
			try
			{
                dgList.Visible = true;
                DataSet ds = new SPOAService().GetShouFuYiList(this.txtQQ.Text.Trim()); //修改为接口访问方式 yinhuang 2014/04/14
                if (ds == null||ds.Tables.Count==0||ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                {
                    dgList.Visible = false;
                    WebUtils.ShowMessage(this.Page, "没有记录");
                    return;
                }

				dgList.DataSource = ds.Tables[0].DefaultView;
				dgList.DataBind();
			}
			catch(Exception ex)
			{
				dgList.Visible = false;
				throw new LogicException(ex.Message); 
			}
		}

		private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
            
		}

		private void dgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.dgList.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}
	}
}
