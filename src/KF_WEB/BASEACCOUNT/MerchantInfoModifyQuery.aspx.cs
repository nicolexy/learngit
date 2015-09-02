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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Collections.Generic;
using CFT.CSOMS.BLL.SPOA;
using System.Xml;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// SelfQuery 的摘要说明。
	/// </summary>
	public partial class MerchantInfoModifyQuery : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataGrid dgListFlist;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

				if(!IsPostBack)
				{
                    //下拉列表
                    GetAllModTypeList(ddlModType);
                    ddlModType.Items.Insert(0, new ListItem("全部", ""));
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
		//	this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
            this.dgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgList_PageIndexChanged);
		}
		#endregion

        private void dgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            this.dgList.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        public static void GetAllModTypeList(System.Web.UI.WebControls.DropDownList ddl)
        {
            ddl.Items.Clear();
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic = BaseDictionary.ModType;

            foreach (KeyValuePair<string, string> a in BaseDictionary.ModType)
            {
                    ListItem li = new ListItem(a.Value.ToString(),a.Key.ToString());
                    ddl.Items.Add(li);
            }
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

		private void BindData(int index)
		{
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
            qs.Finance_HeaderValue = fh;
            string spid = this.txtspid.Text.Trim();
            string accountC = this.txtAccountC.Text.Trim();
            DataSet ds = new SPOAService().QueryAmendMspInfo(spid, ddlModType.SelectedValue, accountC);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
            {
                dgList.DataSource = ds.Tables[0].DefaultView;
                dgList.DataBind();
            }
            else
            {
                dgList.DataSource = null;
                dgList.DataBind();
                throw new LogicException("没有找到记录！");
            }
		}

	}
}
