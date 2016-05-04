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
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using CFT.CSOMS.BLL.WechatPay;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Configuration;
using CFT.CSOMS.BLL.TransferMeaning;
using CFT.CSOMS.COMMLIB;
using CFT.Apollo.Logging;


namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
	/// <summary>
    /// CreditCardRefundQuery ��ժҪ˵����
	/// </summary>
    public partial class CreditCardRefundQuery : System.Web.UI.Page
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
            this.dd_queryDate.SelectedIndexChanged += new EventHandler(this.dd_queryDate_SelectedIndexChanged);
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
            DateTime begindate=new DateTime(), enddate=new DateTime();
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
                throw new Exception("������������");
            }

            if (this.dd_queryDate.SelectedValue == "2" && !string.IsNullOrEmpty(s_date) && !string.IsNullOrEmpty(e_date) && begindate.AddDays(3) < enddate)
            {
                throw new Exception("���ڼ������3�죬���������룡");
            }

            string no = this.cftNo.Text;
            if (string.IsNullOrEmpty(no)) 
            {
                throw new Exception("�������ѯ������");
            }

        }

        private void dd_queryDate_SelectedIndexChanged(object sender, EventArgs e) 
        {
            var d = this.dd_queryDate.SelectedIndex;
            if (d == 0)
            {
                this.showQueryDate.Visible = false;
            }
            else 
            {
                this.showQueryDate.Visible = true;
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
                clearDT();
                this.pager.RecordCount = 1000;
                BindData(1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fetch_no = e.Item.Cells[9].Text;
            string wx_no = e.Item.Cells[10].Text;
            //int curr_page = pager.CurrentPageIndex;
            //if (curr_page > 1)
            //{
            //    rid = this.pager.PageSize * curr_page + rid;
            //}
            GetDetail(wx_no,fetch_no);
        }

        private void GetDetail(string wx_no, string fetch_no)
        {
            //��Ҫע���ҳ���
            clearDT();
            var dt = new WechatPayService().QueryCreditCardRefund(fetch_no, "3", "", "", 0, 1);
            //var dt = new WechatPayService().QueryCreditCardRefundDetail(wx_no, fetch_no);
            if (dt != null)
            { //PublicRes.objectToString(dt, "", true)
                lb_c1.Text = PublicRes.objectToString(dt, "Fwx_fetch_no", true);
                lb_c2.Text = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(dt, "Fnum", true));

                var s_state = PublicRes.objectToString(dt, "Fstate", true);
                lb_c3.Text = ConvertState(s_state);

                if (s_state == "5")
                {
                    lb_c4.Text = "�ɹ�";
                }
                else if (s_state == "2" || s_state == "3" || s_state == "4")
                {
                    lb_c4.Text = "������";
                }
                else
                {
                    lb_c4.Text = "ʧ��    ʧ��ԭ��" + PublicRes.objectToString(dt, "Ffetch_memo", true);
                }

                lb_c5.Text = Transfer.convertbankType(PublicRes.objectToString(dt, "Fbank_type", true));
                lb_c6.Text = PublicRes.objectToString(dt, "Fcard_id", true);
                lb_c7.Text = PublicRes.objectToString(dt, "Fcard_name", true);
                lb_c8.Text = PublicRes.objectToString(dt, "Fopenid", true);
                lb_c9.Text = PublicRes.objectToString(dt, "Fpay_front_time", true);
                lb_c10.Text = PublicRes.objectToString(dt, "Fmodify_time", true);
                lb_c11.Text = PublicRes.objectToString(dt, "Fcft_fetch_no", true);
                try
                {
                    lb_Fstandby2.Text = MoneyTransfer.FenToYuan(PublicRes.objectToString(dt, "Fstandby2"));

                    var wxOrderdt = new WechatPayService().QueryTradeOrder(2, null, PublicRes.objectToString(dt, "Fsp_id", true), fetch_no);
                    if (wxOrderdt != null)
                    {
                        lb_wx_trans_id.Text = PublicRes.objectToString(wxOrderdt, "wx_trans_id", true);
                        lb_trade_id.Text = PublicRes.objectToString(wxOrderdt, "trade_no", true);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("΢�Ż����ѯ��ϸ�쳣��" + ex.ToString());
                }

            }
        }

        private string ConvertState(string state) 
        {
            string ret = "";
            switch (state) 
            {
                case "1": ret = "�ȴ�֧��"; break;
                case "2": ret = "֧�����"; break;
                case "3": ret = "B2Cת�����"; break;
                case "4": ret = "���ַ���"; break;
                case "5": ret = "���ֳɹ�"; break;
                case "6": ret = "����ʧ��"; break;
                case "7": ret = "��Ʊ"; break;
                case "8": ret = "�˿�"; break;
            }

            return ret;
        }

        private void clearDT()
        {
            lb_c1.Text = "";
            lb_c2.Text = "";
            lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";

            lb_c8.Text = "";
            lb_c9.Text = "";
            lb_c10.Text = "";
            lb_c11.Text = "";
        }

        private void BindData(int index)
        {
            clearDT();
            string s_begindate = "";
            string s_enddate = "";

            if (this.dd_queryDate.SelectedValue == "1")
            {
                DateTime d1 = DateTime.Now;
                DateTime d2 = d1.AddMonths(-1);
                s_begindate = d2.ToString("yyyy-MM-dd 00:00:00");
                s_enddate = d1.ToString("yyyy-MM-dd 23:59:59");
            }
            else
            {
                string s_stime = TextBoxBeginDate.Value;
                if (!string.IsNullOrEmpty(s_stime))
                {
                    DateTime begindate = DateTime.Parse(s_stime);
                    s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
                }
                string s_etime = TextBoxEndDate.Value;
                if (!string.IsNullOrEmpty(s_etime))
                {
                    DateTime enddate = DateTime.Parse(s_etime);
                    s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
                }
            }

            //string wxNo = "",bankNo="",refundNo="" , uin = "";
            //if (this.dd_queryType.SelectedValue == "1") 
            //{
            //    wxNo = this.cftNo.Text;
            //}
            //else if (this.dd_queryType.SelectedValue == "2") 
            //{
            //    bankNo = this.cftNo.Text;
            //}
            //else if (this.dd_queryType.SelectedValue == "3")
            //{
            //    refundNo = this.cftNo.Text;
            //}
            //else if (this.dd_queryType.SelectedValue == "4" 
            //    || this.dd_queryType.SelectedValue == "5" 
            //    || this.dd_queryType.SelectedValue == "6"
            //    ||this.dd_queryType.SelectedValue == "7"
            //    ||this.dd_queryType.SelectedValue == "8")
            //{
            //    Session["QQID"] = getQQID();
            //    uin = Session["QQID"].ToString();
            //}
            //else 
            //{
            //    throw new Exception("��ѯ��������ȷ��");
            //}

            int max = pager.PageSize;
            int start = max * (index - 1);

            string queryType = dd_queryType.SelectedValue;
            string No = cftNo.Text;
            var dt = new WechatPayService().QueryCreditCardRefund(No, queryType, s_begindate, s_enddate, start, max);
            if (dt != null && dt.Rows.Count > 0)
            {
                //ͨ��uidȡuin
                //ds.Tables[0].Columns.Add("Fqqid", typeof(string));
                dt.Columns.Add("Fnum_str", typeof(string));
                dt.Columns.Add("Fcard_id_str", typeof(string));
                dt.Columns.Add("Frefund_state_str", typeof(string));
                dt.Columns.Add("Fticket_str", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    //dr["Fqqid"] =  AccountService.Uid2QQ(dr["Fuid"].ToString());
                    dr["Fcard_id_str"] = classLibrary.setConfig.ConvertID(dr["Fcard_id"].ToString(), 4, 4);
                    if (dr["Fstate"].ToString() == "5")
                    {
                        dr["Frefund_state_str"] = "�ɹ�";
                    }
                    else if (dr["Fstate"].ToString() == "2" || dr["Fstate"].ToString() == "3" || dr["Fstate"].ToString() == "4") 
                    {
                        dr["Frefund_state_str"] = "������";
                    }
                    else 
                    {
                        dr["Frefund_state_str"] = "ʧ��";
                    }
                    if (dr["Fstate"].ToString() == "7")
                    {
                        dr["Fticket_str"] = "��";
                    }
                    else
                    {
                        dr["Fticket_str"] = "��";
                    }           
                }

                classLibrary.setConfig.FenToYuan_Table(dt, "Fnum", "Fnum_str");
                
                DataGrid1.DataSource = dt.DefaultView;
                DataGrid1.DataBind();
            }
            else 
            {
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
            }
            
        }

        protected void dd_queryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dd_queryType.SelectedValue == "2")
            {
                this.dd_queryDate.SelectedIndex = 1;
                this.dd_queryDate.Enabled = false;
                this.showQueryDate.Visible = true;
            }
            else
            {
                this.dd_queryDate.SelectedIndex = 0;
                this.dd_queryDate.Enabled = true;
                this.showQueryDate.Visible = false;
            }
        }
      
    }
	
}