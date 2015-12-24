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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// batPay ��ժҪ˵����
    /// </summary>
    public partial class batPay : System.Web.UI.Page
    {

        private string WeekIndex = "0";


        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"TradeLogQuery")) Response.Redirect("../login.aspx?wh=1");
                if (!classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                WeekIndex = Request.QueryString["WeekIndex"];
                try
                {
                    DateTime dt = DateTime.Parse(WeekIndex);
                }
                catch
                {
                    WeekIndex = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }

                TextBoxBeginDate.Value = WeekIndex;
            }
            else
            {
                WeekIndex = TextBoxBeginDate.Value.Trim();
            }

            try
            {
                InitGrid();
            }
            catch
            {
                WebUtils.ShowMessage(this.Page, "��ʾ����ʱ���������ԡ�");
            }
        }

        private void InitGrid()
        {
            DataSet ds = new DataSet();
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                ds = qs.BatPay_InitGrid(WeekIndex);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʱ����:" + ex.Message);
                return;
            }
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ShowBatPay(ds.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "չʾ����ʱ����:" + ex.Message);
                return;
            }
        }


        private void ShowBatPay(DataTable dt)
        {
            DateTime BeginDate = DateTime.Parse(WeekIndex);
            string strDateUse = BeginDate.ToString("yyyyMMdd");

            dt.Columns.Add("Detail", typeof(string));

            if (dt.Rows.Count == 0)
            {
                if (CanVisible(strDateUse))
                {
                    DataRow dr = dt.NewRow();
                    dr["FBatchID"] = strDateUse + "1001";

                    string strDate = BeginDate.ToString("yyyy��MM��dd��");
                    dr["FDate"] = strDate;
                    dr["FStatusName"] = "��δ���ܸ�������";

                    if (!CanStartTask(strDateUse + "1001"))
                    {
                        dr["FStatusName"] = "��δ�����ݿ��գ����������ڴ��������";
                    }
                    dr["FUrl"] = "BatchID=" + strDateUse + "1001";

                    dr["FBankID"] = "��������";

                    dt.Rows.Add(dr);
                }
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int iStatus = Int32.Parse(dr["FStatus"].ToString());
                    dr.BeginEdit();
                    dr["FStatusName"] = GetStatusName(iStatus, dr["FBatchID"].ToString());
                    if (iStatus == 5 && (dr["FPayCount"] == null || dr["FPayCount"].ToString() == "0"))
                    {
                        dr["FStatusName"] = "�������޸����¼";
                    }
                    else
                    {
                        dr["Detail"] = "��ϸ";
                    }

                    dr["FUrl"] = "BatchID=" + dr["FBatchID"].ToString();

                    dr["FBankID"] = GetBankName(dr["FBankType"].ToString());

                    string tmp = dr["FDate"].ToString();

                    tmp = tmp.Substring(0, 4) + "��" + tmp.Substring(4, 2) + "��" + tmp.Substring(6, 2) + "��";
                    dr["FDate"] = tmp;
                    dr.EndEdit();
                }
            }

            dt.DefaultView.Sort = "FBankType";
            DataGrid1.DataSource = dt.DefaultView;
            DataGrid1.DataBind();
        }

        public string GetBankName(string strBankID)
        {
            DataTable dt;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            dt = qs.BatPay_GetBank().Tables[0];

            if (dt == null)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʱ���������ԡ�");
                return "";
            }

            foreach (DataRow dr in dt.Rows)
            {
                string Fbank_type = dr["Fbank_type"].ToString();
                string FFlag_name = dr["FFlag2_2"].ToString();
                if (strBankID != null && strBankID == Fbank_type)
                {
                    return FFlag_name;
                }
            }
            return "�޴�����";     
        }

        public static void GetAllPayBankList(DropDownList ddl)
        {
            DataTable dt;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            dt = qs.BatPay_GetBank().Tables[0];

            if (dt != null)
            {

                ddl.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    string Fbank_type = dr["Fbank_type"].ToString();
                    string FFlag_name = dr["FFlag2_2"].ToString();

                    ddl.Items.Add(new ListItem(FFlag_name, Fbank_type));
                }

            }
        }

        private static bool CanVisible(string strDate)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.BatPay_CanVisible(strDate);
        }

        public static string GetStatusName(int iStatus, string strBatchID)
        {
            string[] strArray = new string[30];
            strArray[0] = "���Ի��ܸ�������";
            strArray[1] = "���������ѻ���";
            strArray[2] = "�������������";
            strArray[3] = "��������������";
            strArray[4] = "���������";
            strArray[5] = "����������Ч";

            strArray[6] = "���������";
            strArray[9] = "<FONT color=red>������������ʧ��</FONT>";  //FBatchNoExp
            strArray[10] = "�����Чִ�н��"; //FBatchNoImp  FStatusDes
            strArray[11] = "<FONT color=red>�����������ɺ��дҵ��ϵͳʧ��</FONT>";

            strArray[20] = "�����������ύ����";
            strArray[21] = "�������������";
            strArray[22] = "������һ���ϴ��ɹ�";
            strArray[23] = "�����������ϴ��ɹ�";
            strArray[24] = "����������";
            strArray[25] = "������������";

            strArray[7] = "ת�������������";

            string tmp = "";
            if (strBatchID != "0" && iStatus > 8 && iStatus < 12) tmp = GetErrorMsg(strBatchID, iStatus);

            if (tmp != "")
                return strArray[iStatus] + "��" + tmp;
            else
                return strArray[iStatus];

        }

        private static string GetErrorMsg(string strBatchID, int iStatus)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.BatPay_GetErrorMsg(strBatchID, iStatus);
        }

        public static bool CanStartTask(string strBatchID)
        {
            if (CheckSnapFinish(strBatchID))
            {
                if (CheckFinish11(strBatchID))
                {
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    return qs.BatPay_SixCheck(strBatchID);
                }
            }
            return false;
        }

        private static bool CheckSnapFinish(string strBatchID)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.BatPay_CheckSnapFinish(strBatchID);
        }

        private static bool CheckFinish11(string strBatchID)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.BatPay_CheckFinish11(strBatchID);
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
        }
        #endregion

    }
}
