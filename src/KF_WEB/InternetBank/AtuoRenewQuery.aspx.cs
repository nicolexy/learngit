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
    /// SysBulletinManage ��ժҪ˵����
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
                    WebUtils.ShowMessage(this.Page, "û�ж�ȡ�����ݣ�" + PublicRes.GetErrorMsg(outmsg));
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
                        dr["Fmemo_str"] = "�ϵ�����Զ�����";
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
                        dr["Fbank_type_str"] = "�Ƹ�ͨ���";
                    else
                    {
                        dr["Fbank_type_str"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
                    }
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        //���ť
        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[9].FindControl("lbChange");
            string s_state = e.Item.Cells[8].Text.Trim();//����״̬
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (s_state == "2" || s_state == "3")
                {
                    lb.Visible = true;
                }
                lb.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С���󡱲�����');";
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
                    WebUtils.ShowMessage(this.Page, "���ɹ�");
                    BindData();
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (this.txtqqid.Text.Trim() == "" || this.txtqqid.Text.Trim()==null)
            {
                WebUtils.ShowMessage(this.Page, "������Ƹ�ͨ���룡");
                return;
            }
            BindData();
        }

    }
}
