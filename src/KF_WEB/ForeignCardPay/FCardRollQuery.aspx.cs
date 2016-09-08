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
using System.Xml;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using System.IO;
using CFT.CSOMS.BLL.ForeignCardModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCardPay
{
    /// <summary>
    /// QueryYTTrade 的摘要说明。
    /// </summary>
    public partial class FCardRollQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        public DateTime qbegindate, qenddate;
        protected ForeignCardService FCBLLService = new ForeignCardService();
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("PayManagement", this)) Response.Redirect("../login.aspx?wh=1");
                }
                this.btnQuery.Attributes.Add("onclick", "return CheckEmail();");
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
            try
            {
                pager.CurrentPageIndex = e.NewPageIndex;
                BindData(e.NewPageIndex);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private void ValidateDate()
        {
            DateTime begindate = new DateTime(), enddate = new DateTime();
            string s_date = TextBoxBeginDate.Value;
            string e_date = TextBoxEndDate.Value;
            try
            {
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
            catch
            {
                throw new Exception("日期输入有误！");
            }
            string spid = txtspid.Text.ToString();

            if (spid == "")
            {
                throw new Exception("请输入商户编号！");
            }
            if (string.IsNullOrEmpty(s_date) || string.IsNullOrEmpty(e_date))
            {
                throw new Exception("请输入日期！");
            }
            if (begindate.Year != enddate.Year)
            {
                throw new Exception("请不要跨年查询！");
            }
        }

        public void btnQuery_Click(object sender, System.EventArgs e)
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
                ButtonExport.Visible = false;
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
                this.pager.RecordCount = 1000;
                BindData(1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private void BindData(int index)
        {
            try
            {
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);
                string s_stime = TextBoxBeginDate.Value;
                string s_begindate = "";
                if (s_stime != null && s_stime != "")
                {
                    DateTime begindate = DateTime.Parse(s_stime);
                    s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
                }
                string s_etime = TextBoxEndDate.Value;
                string s_enddate = "";
                if (s_etime != null && s_etime != "")
                {
                    DateTime enddate = DateTime.Parse(s_etime);
                    s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
                }
             
                string spid = txtspid.Text.ToString().Trim();
                ViewState["s_begindate"] = s_begindate.ToString();
                ViewState["s_enddate"] = s_enddate.ToString();
                ViewState["spid"] = spid.ToString();

                DataSet ds = QueryData(spid, s_begindate, s_enddate, max, start);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    ButtonExport.Visible = false;
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    throw new Exception("数据库无此记录");
                }
                ViewState["g_dt"] = ds;
                DataGrid1.DataSource = ds;
                DataGrid1.DataBind();
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message.ToString());
            }
        }

        private DataSet QueryData(string spid,string s_etime,string s_enddate, int max, int start)
        {
            try
            {
                DataSet ds = new DataSet();
                //查询商户流水
                ds = FCBLLService.GetForeignCardRollList(spid, s_etime, s_enddate, start, max);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    ButtonExport.Visible = false;
                    return null;
                }

                ButtonExport.Visible = true;
                return ds;
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message.ToString());
            }
        }

        public void Export_Click(object sender, System.EventArgs e)
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
                ExportData();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "导出数据失败！" + eSys.Message.ToString());
            }
        }

        private void ExportData()
        {
            DataSet ds = (DataSet)ViewState["g_dt"];
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                throw new Exception("数据库无此记录");
            }

            StringWriter sw = new StringWriter();

            string excelHeader = DataGrid1.Columns[0].HeaderText;
            for (int i = 1; i < DataGrid1.Columns.Count; i++)
            {
                excelHeader += "\t" + DataGrid1.Columns[i].HeaderText;
            }
            sw.WriteLine(excelHeader);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sw.WriteLine("=\"" + dr["FListid"].ToString() 
                    + "\"\t=\"" + dr["Faction_type_str"].ToString() 
                    + "\"\t=\"" + dr["Fcurtype_str"]
                    + "\"\t" + dr["Ftype_str"] 
                    + "\t=\"" + dr["Fsubject_str"] 
                    + "\"\t=\"" + dr["FpaynumStr"] 
                    + "\"\t=\""+ dr["FbalanceStr"] 
                    + "\"\t=\"" + dr["FconnumStr"] 
                    + "\"\t=\"" + dr["FconStr"]
                    + "\"\t=\"" + dr["Fmemo"]
                    + "\"\t=\"" + dr["Fmodify_time_acc"]
                    + "\"\t=\"" + dr["Fmodify_time"]
                    + "\"\t=\"" + dr["Fexplain"] + "\"");
            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=外卡账户流水查询.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();
        }
    }
}