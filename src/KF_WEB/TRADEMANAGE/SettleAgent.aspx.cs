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
namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SettleAgent 的摘要说明。
	/// </summary>
	public partial class SettleAgent : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected System.Web.UI.WebControls.Label Label3;
        protected System.Web.UI.WebControls.TextBox TextBoxBeginDate;
        protected System.Web.UI.WebControls.ImageButton ButtonBeginDate;
        protected System.Web.UI.WebControls.Label Label4;
        protected System.Web.UI.WebControls.TextBox TextBoxEndDate;
        protected System.Web.UI.WebControls.ImageButton ButtonEndDate;
        protected System.Web.UI.WebControls.TextBox txtFspid;


        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                // if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

        }
        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }


        private void BindData(int index)
        {
            int max = pager.PageSize;
            int start = max * (index-1);

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds;
            ds = qs.GetSpAgentRelation(this.txtSpid.Text.Trim(), this.txtAgentid.Text.Trim().ToString(), start, max);
            
            DataTable dt = ds.Tables[0];
            
            dt.Columns.Add("spid",typeof(string));
            dt.Columns.Add("spid_name",typeof(string));
            dt.Columns.Add("agentid",typeof(string));
            dt.Columns.Add("agentid_name",typeof(string));
            dt.Columns.Add("uin",typeof(string));
            dt.Columns.Add("time",typeof(string));

            foreach(DataRow dr in dt.Rows)
            {
                dr["spid"] = dr["Fspid"].ToString();
                dr["spid_name"] = dr["Fsp_name"].ToString();
                //dr["agentid"] = dr["Fagentid"].ToString();
                //dr["agentid_name"] = dr["Fagent_name"].ToString();
                //dr["uin"] = dr["Fstandby3"].ToString();
                dr["time"] = dr["Fcreate_time"].ToString();

                if (!PublicRes.isWhiteOfSeparate(dr["Fspid"].ToString(), Session["uid"].ToString()))
                {
                    dr["agentid_name"] = classLibrary.setConfig.ConvertID(dr["Fagent_name"].ToString(), 0, 2);
                    string s_agentid = dr["Fagentid"].ToString();
                    int idx = s_agentid.IndexOf("@");
                    if (idx > -1)
                    {
                        string s_st = s_agentid.Substring(0, idx);
                        string s_et = s_agentid.Substring(idx + 1);
                        dr["agentid"] = classLibrary.setConfig.ConvertID(s_st, 0, 2) + "@" + s_et;
                    }
                    else {
                        dr["agentid"] = classLibrary.setConfig.ConvertID(s_agentid, 0, 2);
                    }

                    string s_uin = dr["Fstandby3"].ToString();
                    idx = s_uin.IndexOf("@");
                    if (idx > -1)
                    {
                        string s_st = s_uin.Substring(0, idx);
                        string s_et = s_uin.Substring(idx + 1);
                        dr["uin"] = classLibrary.setConfig.ConvertID(s_st, 0, 2) + "@" + s_et;
                    }
                    else
                    {
                        dr["uin"] = classLibrary.setConfig.ConvertID(s_uin, 0, 2);
                    }
                }
                else {
                    dr["agentid"] = dr["Fagentid"].ToString();
                    dr["agentid_name"] = dr["Fagent_name"].ToString();
                    dr["uin"] = dr["Fstandby3"].ToString();
                }
            }
            this.DataGrid1.DataSource = dt.DefaultView;
            this.DataGrid1.DataBind();
        }

        private void checkData()
        {
            if(this.txtSpid.Text.Trim().Length != 0 )
            {
                if(this.txtSpid.Text.Trim().Length != 10)
                    throw new Exception("请输入10位商户号！");
                else
                    ViewState["spid"] = this.txtSpid.Text.Trim();
            }
            if(this.txtAgentid.Text.Trim().Length != 0 )
            {
                if(this.txtAgentid.Text.Trim().Length != 10)
                    throw new Exception("请输入10位代理商户号！");
                else
                    ViewState["agentid"] = this.txtAgentid.Text.Trim();
            }

            if(this.txtSpid.Text.Trim().Length == 0 && this.txtAgentid.Text.Trim().Length == 0)
            {
                throw new Exception("请输入商户号或者代理商");
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


        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            pager.RecordCount= 10000;
            try
            {
                checkData();
            }
            catch(Exception err)
            {
                WebUtils.ShowMessage(this.Page,err.Message);
                return;
            }

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
                WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
            }
        
        }

	}
}
