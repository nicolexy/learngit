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
	/// PayBusinessQuery 的摘要说明。
	/// </summary>
    public partial class CGIInfoQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");


				if(!IsPostBack)
				{
                    TextBoxStartTime.Text = DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd");
                    TextBoxEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    this.cgiTR.Visible = false;
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindDataStat(e.NewPageIndex);
        }
        #endregion

        private void ValidateDate()
        {
            txtSpid.Text.ToString();
            if (string.IsNullOrEmpty(txtSpid.Text.ToString().Trim()))
            {
                throw new Exception("请输入商户号！");
            }
            string begin = TextBoxStartTime.Text.Trim();
            string end = TextBoxEndTime.Text.Trim();
            HandleTime(begin, end, "startTime", "endTime");
        }

        public void HandleTime(string begin, string end, string viewTimeNameStart, string viewTimeNameEnd)
        {
            if (string.IsNullOrEmpty(begin.Trim()) || string.IsNullOrEmpty(end.Trim()))
            {
                throw new Exception("请输入时间！");
            }

            DateTime begindate = new DateTime();
            DateTime enddate = new DateTime();
            try
            {
                begindate = DateTime.Parse(begin);
                enddate = DateTime.Parse(end);
            }
            catch
            {
                throw new Exception("日期输入有误！");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("终止日期小于起始日期，请重新输入！");
            }

            if (begindate.Year != enddate.Year)
            {
                throw new Exception("仅支持相同年份数据查询！");
            }
           

            ViewState[viewTimeNameStart] = begindate.ToString("yyyy-MM-dd 00:00:00");
            ViewState[viewTimeNameEnd] = enddate.ToString("yyyy-MM-dd 23:59:59");
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
		{
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

			try
			{
                this.cgiTR.Visible = true;
                this.pager.RecordCount = 1000;
				dgList.CurrentPageIndex = 0;
                BindData();
                BindDataStat(1);
               
			}
			catch(Exception eSys)
			{
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message));
			}
		}

        private void BindData()
        {
            DataTable dt = new MerchantService().QueryCertExpiredInfo(this.txtSpid.Text.Trim(),
                                    ViewState["startTime"].ToString(), ViewState["endTime"].ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                this.Datagrid1.Visible = true;
                Datagrid1.DataSource = dt.DefaultView;
                Datagrid1.DataBind();
            }
            else
            {
                Datagrid1.DataSource = null;
              //  throw new LogicException("没有找到记录！");
            }
        }

        private void BindDataStat(int index)
		{
            this.pager.CurrentPageIndex = index;
            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new MerchantService().QueryCgiInfo(this.txtSpid.Text.Trim(),
                         ViewState["startTime"].ToString(), ViewState["endTime"].ToString(),start,  max);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.dgList.Visible = true;
                dgList.DataSource = ds.Tables[0].DefaultView;
                dgList.DataBind();
            }
            else
            {
                dgList.DataSource = null;
               // throw new LogicException("没有找到记录！");
            }
		}

	}
}
