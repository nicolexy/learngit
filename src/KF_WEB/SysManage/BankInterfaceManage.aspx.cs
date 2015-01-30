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
using System.Web.Mail;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.SysManage
{
	/// <summary>
	/// SysBulletinManage 的摘要说明。
	/// </summary>
    public partial class BankInterfaceManage : System.Web.UI.Page
	{

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["sysid"] != null && Request.QueryString["sysid"].Trim() != "")
                {
                    ddlSysList.SelectedValue = Request.QueryString["sysid"].Trim();
                }
            }
            this.pager.RecordCount = 1000;
            BindData(1);
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
		}
		#endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        private void BindData(int index)
        {
            try
            {
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);
                string listtype = ddlSysList.SelectedValue;
                string outmsg = "";
                this.labQueryName.Text = "银行类型编码";

                string fbanktype = this.txtBankType.Text.Trim();
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = new DataSet();

                //业务类型
                //1 提现(默认)
                //2 向银行卡付款
                //3 还房贷
                //4 信用卡还款
                //5 代扣银行接口
                //6 银行支付接口
                if (fbanktype == "")
                    ds = qs.QueryBankBulletin(int.Parse(listtype), 0, 0, "",max,start);
                else
                    ds = qs.QueryBankBulletin(int.Parse(listtype), 0, int.Parse(fbanktype), "", max, start);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Datagrid2.DataSource = null;
                    Datagrid2.DataBind();
                    WebUtils.ShowMessage(this.Page, "没有读取到数据：" + PublicRes.GetErrorMsg(outmsg));
                    return;
                }
                ds.Tables[0].Columns.Add("FBank_TypeName", typeof(System.String));
                ds.Tables[0].Columns.Add("Fsysid", typeof(String));

                classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "banktype", "FBank_TypeName", "BANK_TYPE");//同FundQuery银行类型取值

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr.BeginEdit();
                    dr["Fsysid"] = listtype;
                //    dr["FBank_TypeName"] = classLibrary.setConfig.returnDicStr("BANK_TYPE", dr["banktype"].ToString());
                 
                    dr.EndEdit();
                }
                //排序
                DataTable dt = ds.Tables[0];
                DataView view = dt.DefaultView;
                view.Sort = "startime desc";
                dt = view.ToTable();
                DataSet dsResult = new DataSet();
                dsResult.Tables.Add(dt);

                Datagrid2.DataSource = dsResult.Tables[0].DefaultView;
                Datagrid2.DataBind();
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }


        protected void btadd_Click(object sender, System.EventArgs e)
        {
            string sysid = ddlSysList.SelectedValue;
            Response.Redirect("BankInterfaceManage_Detail.aspx?sysid=" + sysid + "&opertype=1");
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            BindData(1);
        }

	}
}
