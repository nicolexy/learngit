using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.ActivityModule;
using System.Collections;
using System.Web.UI.WebControls;

namespace TENCENT.OSS.CFT.KF.KF_Web.Activity
{
	/// <summary>
    /// LctActivityAdd 的摘要说明。
    /// 新活动添加
	/// </summary>
    public partial class LctActivityAdd : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
           
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    //QueryActivityData(1);
                    
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
            this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
		}
		#endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            QueryActivityData(e.NewPageIndex);
        }

        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[4].FindControl("lbDel");
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                lb.Attributes["onClick"] = "return confirm('确定要执行“删除”操作吗？');";
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim(); //ID

            switch (e.CommandName)
            {
                case "DEL": //删除
                    DelAct(fid);
                    break;
            }
        }

		private void ValidateDate()
		{
            string act_no = txtActNo.Text.Trim();
            string act_name = txtActName.Text.Trim();

            if (act_no == "")
            {
                throw new Exception("活动号不能为空！");
            }
            if (act_name == "")
            {
                throw new Exception("活动名称不能为空！");
            }
		}

        public void btnAdd_Click(object sender, System.EventArgs e)
		{
            try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
                BindData();
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

        public void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                QueryActivityData(1);
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

        private void DelAct(string fid) 
        {
            try
            {
                if (string.IsNullOrEmpty(fid)) 
                {
                    throw new Exception("请选择一条记录！");
                }
                new ActivityService().DelLctActivity(int.Parse(fid));
                pager.CurrentPageIndex = 1;
                QueryActivityData(1);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
                return;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
                return;
            }
        }

        private void BindData()
		{
            string act_no = txtActNo.Text.Trim();
            string act_name = txtActName.Text.Trim();

            new ActivityService().AddLctActivity(act_no, act_name, Session["uid"].ToString());
            WebUtils.ShowMessage(this.Page, "添加成功");
		}

        private void QueryActivityData(int index) 
        {
            int max = pager.PageSize;
            int start = max * (index - 1);

            this.pager.RecordCount = 1000;

            string act_no = txtActNo.Text.Trim();
            string act_name = txtActName.Text.Trim();

            DataTable dt = new ActivityService().QueryActivityList(act_no, act_name,start, max);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataGrid1.DataSource = dt.DefaultView;
                DataGrid1.DataBind();
            }
            else 
            {
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
            }
        }
	}
}