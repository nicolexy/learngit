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
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.DKModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
    /// <summary>
    /// QueryDKLimitPage ��ժҪ˵����
    /// </summary>
    public partial class QueryDKLimitPage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (!IsPostBack)
            {
                classLibrary.setConfig.GetAllBankList_DK(ddlBank_Type);

                //tbx_bankID.Text = "6222350040762419";
                //ddlBank_Type.Items.Add(new ListItem("����","3007"));
            }
        }

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.DataGrid_QueryResult.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid_QueryResult_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.pager_PageChanged);

        }
        #endregion

        protected void btn_serach_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (tbx_bankID.Text.Trim() == "")
                    return;
                if (ddlBank_Type.SelectedValue == null || ddlBank_Type.SelectedValue == "")
                    return;

                ViewState["bankaccno"] = tbx_bankID.Text.Trim();
                ViewState["banktype"] = ddlBank_Type.SelectedValue.Trim();

                BindData(1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.Message.ToString()));
            }
        }

        private void BindData(int index)
        {
            string bankaccno = ViewState["bankaccno"].ToString();
            string banktype = ViewState["banktype"].ToString();

            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            //DataSet ds = qs.GetDKLimit_List(banktype,bankaccno,(index - 1) * this.pager.PageSize,this.pager.PageSize);
            DataTable dt = new DKService().GetDKLimit_List(banktype, bankaccno, "", (index - 1) * this.pager.PageSize, this.pager.PageSize);
            if (dt == null || dt.Rows.Count == 0)
            {
                WebUtils.ShowMessage(this, "��ѯ���Ϊ��");
                return;
            }

            DataGrid_QueryResult.DataSource = dt.DefaultView;
            DataGrid_QueryResult.DataBind();
        }

        private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            int index = e.NewPageIndex;
            BindData(index);
        }

        private void DataGrid_QueryResult_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName == "detail")
            {
                try
                {
                    string banktype = e.Item.Cells[1].Text;
                    string bankaccno = e.Item.Cells[2].Text;
                    string servicecode = e.Item.Cells[3].Text;

                    //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    //qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                    //DataSet ds = qs.GetDKLimit_Detail(banktype,bankaccno,servicecode);
                    DataTable dt = new DKService().GetDKLimit_List(banktype, bankaccno, servicecode, 0, 1);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return;
                    }
                    else
                    {
                        DataRow dr = dt.Rows[0];

                        lb_c1.Text = dr["Fonce_data"].ToString();
                        lb_c3.Text = dr["Fday_sum_data"].ToString();
                        lb_c4.Text = dr["Fday_use_data"].ToString();

                        lb_c5.Text = dr["Fweek_sum_data"].ToString();
                        lb_c6.Text = dr["Fweek_use_data"].ToString();

                        lb_c7.Text = dr["Fmonth_sum_data"].ToString();
                        lb_c8.Text = dr["Fmonth_use_data"].ToString();

                        lb_c9.Text = dr["Fquarter_sum_data"].ToString();
                        lb_c10.Text = dr["Fquarter_use_data"].ToString();

                        lb_c11.Text = dr["Fyear_sum_data"].ToString();
                        lb_c12.Text = dr["Fyear_use_data"].ToString();

                        lb_c13.Text = dr["Fday_sum_count"].ToString();
                        lb_c14.Text = dr["Fday_use_count"].ToString();

                        lb_c15.Text = dr["Fweek_sum_count"].ToString();
                        lb_c16.Text = dr["Fweek_use_count"].ToString();

                        lb_c17.Text = dr["Fmonth_sum_count"].ToString();
                        lb_c18.Text = dr["Fmonth_use_count"].ToString();

                        lb_c19.Text = dr["Fquarter_sum_count"].ToString();
                        lb_c20.Text = dr["Fquarter_use_count"].ToString();

                        lb_c23.Text = dr["Fyear_sum_count"].ToString();
                        lb_c24.Text = dr["Fyear_use_count"].ToString();
                    }
                }
                catch (Exception eSys)
                {
                    WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.Message.ToString()));
                }
            }
        }
    }
}
