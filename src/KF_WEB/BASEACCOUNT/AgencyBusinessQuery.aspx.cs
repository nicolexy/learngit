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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// AgencyBusinessQuery 的摘要说明。
    /// </summary>
    public partial class AgencyBusinessQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                if (!IsPostBack)
                {
                    this.divInfo.Visible = false;
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
            this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
            //this.dgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgList_PageIndexChanged);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

        }
        #endregion

        private void ValidateDate()
        {
            string s_qq = txtQQ.Text.ToString();
            string spwww = txtNetAddress.Text.ToString();
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.divInfo.Visible = false;

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
                this.pager.RecordCount = 1000;
                BindData(1);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                this.dgList.Visible = false;
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception eSys)
            {
                this.dgList.Visible = false;
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message + ", stacktrace" + eSys.StackTrace);
            }
        }

        private void BindData(int index)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            string qqid = this.txtQQ.Text.Trim();
            string addr = this.txtNetAddress.Text.Trim();

            DataSet ds = null;

            //增加分页
            int max = pager.PageSize;
            int start = max * (index - 1);

            //通过用户账号查询
            ds = qs.GetAgencyBusinessList(qqid, addr, start, max);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dgList.DataSource = ds.Tables[0].DefaultView;
                dgList.DataBind();
            }
            else
            {
                dgList.DataSource = null;
                dgList.DataBind();
            }
        }

        private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                try
                {
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    DataSet ds = null;
                    string sflag = e.Item.Cells[1].Text;

                    ds = qs.GetAgencyBusinessInfo(e.Item.Cells[0].Text);

                    if (ds != null && ds.Tables[0].Rows.Count == 1)
                    {
                        this.divInfo.Visible = true;

                        this.lblNo.Text = ds.Tables[0].Rows[0]["Fid"].ToString();
                        this.lblQQ.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
                        this.lblURL.Text = ds.Tables[0].Rows[0]["Fdomain"].ToString();
                        this.lblPhone.Text = ds.Tables[0].Rows[0]["Ftel"].ToString();
                        this.lblMobile.Text = ds.Tables[0].Rows[0]["FMobile"].ToString();
                        this.lblEmail.Text = ds.Tables[0].Rows[0]["Femail"].ToString();
                        this.lblTradeType.Text = ds.Tables[0].Rows[0]["TradeName"].ToString();
                        this.lblName.Text = ds.Tables[0].Rows[0]["FName"].ToString();
                        this.lblAddress.Text = ds.Tables[0].Rows[0]["Faddress"].ToString();
                        this.lblAddressNo.Text = ds.Tables[0].Rows[0]["Fpostcode"].ToString();
                        this.lblRemark.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
                        this.lblCreateTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
                        this.lblModifyTime.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
                        this.lblApprovePersonID.Text = ds.Tables[0].Rows[0]["Fcheck_id"].ToString();
                        this.lblApprovePerson.Text = ds.Tables[0].Rows[0]["Fcheck_user"].ToString();
                        this.lblApproveTime.Text = ds.Tables[0].Rows[0]["Fcheck_time"].ToString();
                        this.lblType.Text = ds.Tables[0].Rows[0]["DictName"].ToString();
                        this.lblCommendatory.Text = ds.Tables[0].Rows[0]["Fsuggester"].ToString();
                        this.lblOperateRemark.Text = ds.Tables[0].Rows[0]["Fop_memo"].ToString();
                    }
                    else
                    {
                        throw new Exception("数据读取失败");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ", stacktrace" + ex.StackTrace);
                }
            }
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }
    }
}
