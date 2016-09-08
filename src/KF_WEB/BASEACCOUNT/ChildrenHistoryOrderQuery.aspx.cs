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
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
    /// ChildrenHistoryOrderQuery 的摘要说明。
	/// </summary>
    public partial class ChildrenHistoryOrderQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        public DateTime qbegindate, qenddate;

        protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("PayManagement", this)) Response.Redirect("../login.aspx?wh=1");

                    string sbegindate = Request.QueryString["qbegindate"];
                    if (sbegindate != null && sbegindate != "")
                    {
                        qbegindate = DateTime.Parse(sbegindate);
                        TextBoxBeginDate.Value = qbegindate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        qbegindate = DateTime.Now.AddDays(-30);
                        TextBoxBeginDate.Value = qbegindate.ToString("yyyy-MM-dd");
                    }
                    string senddate = Request.QueryString["qenddate"];
                    if (senddate != null && senddate != "")
                    {
                        qenddate = DateTime.Parse(senddate);
                        TextBoxEndDate.Value = qenddate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        qenddate = DateTime.Now;
                        TextBoxEndDate.Value = qenddate.ToString("yyyy-MM-dd");
                    }
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
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
            DateTime begindate, enddate;

			try
			{
                begindate = DateTime.Parse(TextBoxBeginDate.Value);
                enddate = DateTime.Parse(TextBoxEndDate.Value);
			}
			catch
			{
				throw new Exception("日期输入有误！");
			}
            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("终止日期小于起始日期，请重新输入！");
            }
            TimeSpan span = enddate.Subtract(begindate);
            int dayDiff = span.Days;
            if (dayDiff > 30) {
                throw new Exception("只能查询一个月区间的历史记录！");
            }

            string cft_no = tbCft.Text;
            if (cft_no == "") {
                throw new Exception("财付通账号不能为空！");
            }

		}

        public void btnQuery_Click(object sender, System.EventArgs e)
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
                this.pager.RecordCount = 1000;
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

        private void BindData(int index)
		{
            string s_stime = TextBoxBeginDate.Value;
            DateTime begindate = DateTime.Parse(s_stime);
            string s_etime = TextBoxEndDate.Value;
            DateTime enddate = DateTime.Parse(s_etime);

            string cft_no = tbCft.Text.ToString().Trim();
            string no_type = ddlType.SelectedValue.ToString();

            int max = pager.PageSize;
            int start = 1 + max * (index - 1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.GetChildrenBankRollList(cft_no, begindate, enddate, no_type, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("Faction_type_str", typeof(String)); //动作类型
                ds.Tables[0].Columns.Add("Fcurtype_str", typeof(String)); //币种类型
                ds.Tables[0].Columns.Add("Ftype_str", typeof(String)); //交易类型
                ds.Tables[0].Columns.Add("Fsubject_str", typeof(String)); //类别/科目
                ds.Tables[0].Columns.Add("Fpaynum_str", typeof(String)); //金额
                ds.Tables[0].Columns.Add("Fbalance_str", typeof(String)); //账户余额

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "Fpaynum_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fbalance", "Fbalance_str");

                foreach (DataRow dr in ds.Tables[0].Rows) 
                {
                    string s_actiontype = Transfer.convertActionType(dr["Faction_type"].ToString());
                    string s_curtype = Transfer.convertMoney_type(dr["Fcurtype"].ToString());
                    string s_type = Transfer.convertTradeType(dr["Ftype"].ToString());
                    string s_subject = Transfer.convertSubject(dr["Fsubject"].ToString());

                    dr.BeginEdit();
                    dr["Faction_type_str"] = s_actiontype;
                    dr["Fcurtype_str"] = s_curtype;
                    dr["Ftype_str"] = s_type;
                    dr["Fsubject_str"] = s_subject;
                    dr.EndEdit();
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

	}
}