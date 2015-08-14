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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// FundCardQuery_Detail ��ժҪ˵����
    /// </summary>
    public partial class FundCardQuery_DetailNew : System.Web.UI.Page
    {
        TradeService service = new TradeService();
        int pagesize = 10;
        private void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            try
            {
               // string sr = Session["key"].ToString();

                Label_uid.Text = Session["uid"].ToString();

                pager.RecordCount = 1000;
                pager.PageSize = pagesize;
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            if (!IsPostBack)
            {

                string tmp = Request.QueryString["flistID"];
                if (tmp != null && tmp.Trim() != "")
                {
                    this.TextBox1_ListID.Text = tmp;

                    try
                    {
                        BandData(1);
                    }
                    catch (LogicException err)
                    {
                        WebUtils.ShowMessage(this.Page, err.Message);
                    }
                }
                ShowPlan(1);
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
            this.btsearch.Click += new System.EventHandler(this.btsearch_Click);
            this.DGData.SelectedIndexChanged += new System.EventHandler(this.DGData_SelectedIndexChanged);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        void ShowPlan(int type)
        {
            if (type == 1)
            {
                this.Paneltitel.Visible = true;
                this.PanelList.Visible = true;
                this.PanelDetail.Visible = false;
            }
            if (type == 2)
            {
                this.Paneltitel.Visible = false;
                this.PanelList.Visible = false;
                this.PanelDetail.Visible = true;
            }
        }

        private void BandData(int index)
        {
            string Flistid = this.TextBox1_ListID.Text.Trim();
            string fsupplylist = this.TextBox_Fsupply_list.Text.Trim();
            string fcarrdid = this.Textbox_Fcard_id.Text.Trim();


            if (Flistid == "" && fsupplylist == "" && fcarrdid == "")
            {
                WebUtils.ShowMessage(this.Page, "��ѯ��������������һ����");

                return;
            }

            //Query_Service.Query_Service qs = new Query_Service.Query_Service();

            DataSet ds = service.GetFundCardListDetail(Flistid, fsupplylist, fcarrdid, (index - 1) * pagesize, pagesize);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("FStateName", typeof(System.String));
                ds.Tables[0].Columns.Add("FSignName", typeof(System.String));
                ds.Tables[0].Columns.Add("FNumYuan", typeof(System.String));
                ds.Tables[0].Columns.Add("FCardtypeName", typeof(System.String));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr.BeginEdit();
                    string tmp = PublicRes.GetInt(dr["Fstate"]);
                    if (tmp == "1")
                    {
                        tmp = "����ǰ";
                    }
                    if (tmp == "2")
                    {
                        tmp = "�����";
                    }
                    dr["FStateName"] = tmp;
                    tmp = PublicRes.GetInt(dr["Fsign"]);
                    if (tmp == "1")
                    {
                        tmp = "�����ɹ�";
                    }
                    if (tmp == "2")
                    {
                        tmp = "����ʧ��";
                    }
                    if (tmp == "3")
                    {
                        tmp = "��ʼ��";
                    }

                    dr["FSignName"] = tmp;
                    string fum = PublicRes.GetString(dr["Fnum"]);
                    if (fum == "" || fum == null)
                    {
                        fum = "0";
                    }
                    long itmp = long.Parse(fum);

                    double ltmp = MoneyTransfer.FenToYuan(itmp);

                    dr["FNumYuan"] = ltmp.ToString();
                    tmp = PublicRes.GetInt(dr["Fcard_type"]);
                    if (tmp == "1")
                    {
                        tmp = "�ƶ���";
                    }
                    if (tmp == "2")
                    {
                        tmp = "��ͨ��";
                    }
                    dr["FCardtypeName"] = tmp;
                    dr.EndEdit();
                }
                this.DGData.DataSource = ds.Tables[0].DefaultView;
                this.DGData.DataBind();

            }
            else
            {
                WebUtils.ShowMessage(this.Page, "û���ҵ���¼��");
            }
        }

        private void btsearch_Click(object sender, System.EventArgs e)
        {
            ShowPlan(1);
            BandData(1);
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BandData(pager.CurrentPageIndex);
        }

        //
        private void DGData_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            string flistid = DGData.SelectedItem.Cells[0].Text.Trim();
            if (flistid != "")
            {
                ShowPlan(2);

                DataSet ds = service.GetFundCardListDetail(flistid, "", "", 0, 1);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    labFListID.Text = PublicRes.GetString(dr["FListID"]);
                    string fum = PublicRes.GetString(dr["Fnum"]);
                    if (fum == "" || fum == null)
                    {
                        fum = "0";
                    }
                    long itmp = long.Parse(fum);

                    double ltmp = MoneyTransfer.FenToYuan(itmp);

                    labFNum.Text = ltmp.ToString();

                    string tmp = PublicRes.GetInt(dr["Fstate"]);
                    if (tmp == "1")
                    {
                        labFState.Text = "����ǰ";
                    }
                    if (tmp == "2")
                    {
                        labFState.Text = "�����";
                    }

                    tmp = PublicRes.GetInt(dr["Fsign"]);
                    if (tmp == "1")
                    {
                        labFSign.Text = "�����ɹ�";
                    }
                    if (tmp == "2")
                    {
                        labFSign.Text = "����ʧ��";
                    }
                    if (tmp == "3")
                    {
                        labFSign.Text = "��ʼ��";
                    }


                    labFsupply_list.Text = PublicRes.GetString(dr["Fsupply_list"]);
                    labFsp_back_prove.Text = PublicRes.GetString(dr["Fsp_back_prove"]);
                    LabFcard_id.Text = PublicRes.GetString(dr["Fcard_id"]);
                    tmp = PublicRes.GetInt(dr["Fcard_type"]);
                    if (tmp == "1")
                    {
                        LabFcard_type.Text = "�ƶ���";
                    }
                    if (tmp == "2")
                    {
                        LabFcard_type.Text = "��ͨ��";
                    }

                    labFsupply_id.Text = PublicRes.GetString(dr["Fsupply_id"]);
                    labFuin.Text = PublicRes.GetString(dr["Fuin"]);
                    labFuser_name.Text = PublicRes.GetString(dr["Fuser_name"]);
                    labFpay_front_time.Text = PublicRes.GetDateTime(dr["Fpay_front_time"]);
                    labFsp_time.Text = PublicRes.GetDateTime(dr["Fsp_time"]);
                    labFmodify_time.Text = PublicRes.GetDateTime(dr["FModify_time"]);
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "û���ҵ���¼��");
                }
            }
        }
    }
}
