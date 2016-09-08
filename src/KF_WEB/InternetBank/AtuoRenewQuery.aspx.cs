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

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
    /// <summary>
    /// SysBulletinManage 的摘要说明。
    /// </summary>
    public partial class AtuoRenewQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
               
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
        }
        #endregion

        private void BindData()
        {
            try
            {

                string outmsg = "";
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                DataSet ds = qs.AtuoRenewQuery(this.txtqqid.Text.Trim());

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                {
                    WebUtils.ShowMessage(this.Page, "没有读取到数据：" + PublicRes.GetErrorMsg(outmsg));
                    return;
                }
                DataTable dt = ds.Tables[0];
                dt.Columns.Add("Fmemo_str", typeof(String));
                dt.Columns.Add("Fbank_type_str", typeof(String));
                foreach (DataRow dr in dt.Rows)
                {
                    string Fmemo = dr["Fmemo"].ToString();
                    if (Fmemo == "")
                    {
                        dr["Fmemo_str"] = "老的余额自动续费";
                    }
                    else
                    {
                        string[] memo = Fmemo.Split('|');
                        string memoStr = "";
                        for (int i = 0; i < memo.Length; i++)
                        {
                            if (i != 0)
                                memoStr += "|";

                            string mType = memo[i];
                            if (InternetBankDictionary.AtuoRenewState.ContainsKey(mType))
                            {
                                memoStr += InternetBankDictionary.AtuoRenewState[mType];
                            }
                            else
                            {
                                memoStr += mType;
                            }
                        }
                        dr["Fmemo_str"] = memoStr;
                    }

                    if (dr["Fbank_type"].ToString() == "cft")
                        dr["Fbank_type_str"] = "财付通余额";
                    else
                    {
                        dr["Fbank_type_str"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
                    }
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        //解绑按钮
        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[9].FindControl("lbChange");
            string s_state = e.Item.Cells[8].Text.Trim();//批次状态
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (s_state == "2" || s_state == "3")
                {
                    lb.Visible = true;
                }
                lb.Attributes["onClick"] = "return confirm('确定要执行“解绑”操作吗？');";
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                string qqid = e.Item.Cells[5].Text.Trim(); //QQ
                string channel_id = e.Item.Cells[7].Text.Trim();
                if (e.CommandName == "CHANGE")
                {
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
                    qs.Finance_HeaderValue = fh;
                    qs.AtuoRenewUnbind(qqid, channel_id);
                    WebUtils.ShowMessage(this.Page, "解绑成功");
                    BindData();
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (this.txtqqid.Text.Trim() == "" || this.txtqqid.Text.Trim()==null)
            {
                WebUtils.ShowMessage(this.Page, "请输入财付通号码！");
                return;
            }
            BindData();
        }

    }
}
