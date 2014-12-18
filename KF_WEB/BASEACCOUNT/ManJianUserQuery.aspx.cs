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
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// PayBusinessQuery 的摘要说明。
	/// </summary>
    public partial class ManJianUserQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
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
            this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
			this.dgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgList_PageIndexChanged);

		}
		#endregion

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
            if (this.txtQQ.Text.Trim()=="")
            {
                WebUtils.ShowMessage(this.Page, "请输入财付通账号" );
                return;
            }
			try
			{
				dgList.CurrentPageIndex = 0;
				BindData();
			}
			catch(Exception eSys)
			{
				this.dgList.Visible = false;
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

        private void BindData()
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                DataSet ds = qs.ManJianUserList(this.txtQQ.Text.Trim());
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询结果为空");
                    return;
                }
                dgList.Visible = true;
                this.dgList.DataSource = ds;
                this.dgList.DataBind();
            }
            catch (SoapException eSoap) //捕获soap类
            {
                dgList.Visible = false;
                string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "查询错误，详细：" + str);
                return;
            }
            catch (Exception e)
            {
               WebUtils.ShowMessage(this.Page, "查询错误，详细：" + e.Message.ToString());
               return;
            }
        }

        private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                try
                {
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    string qq = e.Item.Cells[0].Text;
                    string cre_id = e.Item.Cells[1].Text;
                    string bank_type = e.Item.Cells[2].Text;
                    string mobile = e.Item.Cells[3].Text;
                    DataSet ds=qs.ManJianAddOne(qq, cre_id, bank_type, mobile);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        WebUtils.ShowMessage(this, "增加一次失败！");
                        return;
                    }
                    WebUtils.ShowMessage(this.Page, "增加成功！");
                }
                catch (SoapException eSoap) //捕获soap类
                {
                    dgList.Visible = false;
                    string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                    WebUtils.ShowMessage(this.Page, "增加一次失败！，详细：" + str);
                    return;
                }
            }
        }


		private void dgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.dgList.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}
	}
}
