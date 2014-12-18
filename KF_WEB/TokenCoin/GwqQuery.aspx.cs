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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using SunLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.TokenCoin
{
	/// <summary>
    /// GwqQuery 的摘要说明。
	/// </summary>
    public partial class GwqQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
                Label_uid.Text = Session["uid"].ToString();

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

                Sun.EnumFillList(typeof(TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.TokenCoin.GwqState), false, "全部", ListState);
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
            this.btQuery.Click += new System.EventHandler(this.btQuery_Click);
            this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
		}
		#endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        private void btQuery_Click(object sender, System.EventArgs e)
        {
            string pno = TextBoxId.Text.ToString();
            string cftno = TextBoxSpid.Text.ToString();
            if (RadioID.Checked) 
            {
                if (string.IsNullOrEmpty(pno))
                {
                    WebUtils.ShowMessage(this.Page, "请输入批次号！");
                    return;
                }
            }
            if (RadioSpid.Checked)
            {
                if (string.IsNullOrEmpty(cftno))
                {
                    WebUtils.ShowMessage(this.Page, "请输入账号！");
                    return;
                }
            }

            try
            {
                pager.RecordCount = GetCount();

                BindData(1);
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

        private int GetCount()
        {
            string id = RadioSpid.Checked ?
                classLibrary.setConfig.replaceSqlStr(TextBoxSpid.Text) : TextBoxId.Text;
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.GetGwqCount_new(id, RadioID.Checked ? 1 : 0, ListState.SelectedValue == "" ? "" : "FState=" + ListState.SelectedValue);
        }

        private void BindData(int index)
        {
            pager.CurrentPageIndex = index;
            int max = pager.PageSize;
            int start = max * (index - 1) + 1;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            string id = RadioSpid.Checked ?
                classLibrary.setConfig.replaceSqlStr(TextBoxSpid.Text) : TextBoxId.Text;
            DataSet ds = qs.GetGwq_new(id, RadioID.Checked ? 1 : 0, ListState.SelectedValue == "" ? "" : "FState=" + ListState.SelectedValue, start, max);


            if (ds != null && ds.Tables.Count > 0)
            {
                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                throw new LogicException("没有找到记录！");
            }
        }

        private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex >= 0)
            {
                e.Item.Cells[6].Text = TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.TokenCoin.Define.FeeFormat(e.Item.Cells[5].Text, e.Item.Cells[6].Text);
                e.Item.Cells[5].Text = Sun.EnumGetName(typeof(TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.TokenCoin.CoinType), e.Item.Cells[5].Text);
                e.Item.Cells[9].Text = Sun.EnumGetName(typeof(TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.TokenCoin.GwqState), e.Item.Cells[9].Text);
            }
        }

	}
}
