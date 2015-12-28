using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.ActivityModule;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.KF_Web.Activity
{
    /// <summary>
    /// QueryLCTActivity ��ժҪ˵����
    /// </summary>
    public partial class QueryLCTActivity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

                }

            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
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
                string s_date = TextBoxBeginDate.Value;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Value;
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
            catch
            {
                throw new Exception("������������");
            }
            string cft_no = txtCftNo.Text.Trim();

            if (cft_no == "")
            {
                throw new Exception("΢��֧���˺Ų���Ϊ�գ�");
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
                this.pager.RecordCount = 1000;
                BindData(1);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()) + ", stacktrace" + eSys.StackTrace);
            }
        }

        private void BindData(int index)
        {
            clearDT();
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

            string cft_no = txtCftNo.Text.Trim();
            string act_id = ddlActId.SelectedValue;

            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = null;

            ds = new ActivityService().QueryActivity(start, max, act_id, cft_no, s_begindate, s_enddate);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["g_dt"] = ds.Tables[0];
                //���ͨ�
                if (act_id.Equals("lct"))
                {
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                    DataGrid2.DataSource = null;
                    DataGrid2.DataBind();
                }
                //�û��������濨�
                else if (act_id.Equals("userfbsyk"))
                {
                    DataGrid2.DataSource = ds.Tables[0].DefaultView;
                    DataGrid2.DataBind();
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                }
            }
            else
            {
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
                DataGrid2.DataSource = null;
                DataGrid2.DataBind();
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            int rid = e.Item.ItemIndex;
            GetDetail(rid);
        }

        private void GetDetail(int rid)
        {
            try{
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null && g_dt.Rows.Count > 0)
            {
                lb_ActId.Text = g_dt.Rows[rid]["FActionId"].ToString();//���
                lb_ActName.Text = g_dt.Rows[rid]["FActName"].ToString();//�����
                lb_TransId.Text = g_dt.Rows[rid]["FPriTransId"].ToString();//�깺����
                lb_State.Text = g_dt.Rows[rid]["FGiveStateStr"].ToString();//����״̬
                lb_BatchId.Text = g_dt.Rows[rid]["FPrizeDesc"].ToString();//���κ�
                lb_Spname.Text = g_dt.Rows[rid]["FspnameStr"].ToString();//�깺����
                lb_SendTicketTime.Text = g_dt.Rows[rid]["FPrizeModifyTime"].ToString();//��ȯʱ��
                lb_StartDate.Text = g_dt.Rows[rid]["FStartDate"].ToString();//��һ����������
                lb_CreateTime.Text = g_dt.Rows[rid]["FPrizeTime"].ToString();//��Ʒ����ʱ��
                lb_ExpireTime.Text = g_dt.Rows[rid]["FPrizeExpiredTime"].ToString();//��ƷʧЧʱ��
                lb_GivePosId.Text = g_dt.Rows[rid]["FGivePosId"].ToString();//������ˮ
                lb_Openid.Text = g_dt.Rows[rid]["FUin"].ToString();//openid
                lb_ErrorInfo.Text = g_dt.Rows[rid]["FErrInfo"].ToString();//������Ϣ
                //����»ʱ����Ҫ��ʾ������
                if (g_dt.Rows[rid]["FActionId"].ToString().Equals("20035"))
                {
                    //ͨ��Uin�ҵ�UID����ͨ��UID�ҵ�FChannel_id
                    var uid = new Query_Service.Query_Service().Uid2QQ(g_dt.Rows[rid]["FUin"].ToString());
                    lb_ChannelId.Text = new ActivityService().GetChannelIDByFUid(uid);//������
                }
                else
                {
                    lb_ChannelId.Text = string.Empty;
                }
                lb_FActType.Text = g_dt.Rows[rid]["FActTypeStr"].ToString();//�����
                lb_FPrizeType.Text = g_dt.Rows[rid]["FPrizeTypeStr"].ToString();//��Ʒ����
                lb_FPrizeName.Text = g_dt.Rows[rid]["FPrizeName"].ToString();//��Ʒ����
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()) + ", stacktrace" + eSys.StackTrace);
            }
       
        }

        private void clearDT()
        {
            lb_ActId.Text = "";
            lb_TransId.Text = "";
            lb_State.Text = "";
            lb_BatchId.Text = "";
            lb_Spname.Text = "";
            lb_SendTicketTime.Text = "";
            lb_StartDate.Text = "";
            lb_CreateTime.Text = "";
            lb_ExpireTime.Text = "";
            lb_GivePosId.Text = "";
            lb_Openid.Text = "";
            lb_ErrorInfo.Text = "";
            lb_ChannelId.Text = "";
            lb_FActType.Text = "";
            lb_FPrizeType.Text = "";
            lb_FPrizeName.Text = "";
        }
    }
}