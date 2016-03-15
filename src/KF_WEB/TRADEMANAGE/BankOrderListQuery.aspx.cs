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
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using CFT.CSOMS.BLL.TransferMeaning;
using System.Linq;
using CFT.Apollo.Logging;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// SysBulletinManage ��ժҪ˵����
    /// </summary>
    public partial class BankOrderListQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        public string begintime = DateTime.Now.ToString("yyyy-MM-dd");
        public string endtime = DateTime.Now.ToString("yyyy-MM-dd");
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
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

        }
        #endregion

        public List<String> listID = new List<string>();
        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            string UinListId = txtUinListId.Text.Trim();
            string BankListId = txtBankListId.Text.Trim();
            string begindate = TextBoxBeginDate.Text.Trim();
            string enddate = TextBoxEndDate.Text.Trim();
            bind(UinListId, BankListId, begindate, enddate);
           
        }
        private void bind(string UinListId, string BankListId, string begindate, string enddate)
        {
            try
            {
                int iType = query(UinListId, BankListId, begindate, enddate);
                DataSet ds = TradeInfo(iType);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    throw new Exception("���ݿ��޴˼�¼");
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }
        }
        private void ValidateDate(string BankListId, string begin, string end)
        {
            DateTime begindate;
            DateTime enddate;
            string u_ID = BankListId.Trim();
            try
            {
                begindate = DateTime.Parse(begin.Trim());
                enddate = DateTime.Parse(end.Trim());
            }
            catch
            {
                throw new Exception("������������");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
            }
            string newczdate = PublicRes.GetZWDicValue("OldOrderCZDataEndTime");
            if (newczdate == null || newczdate == "")
            {
                throw new Exception("δ��ѯ��OldOrderCZDataEndTime��Ӧ������ֵ��");
            }


            if (begindate.AddDays(15).CompareTo(enddate) < 0)
            {
                throw new Exception("ѡ��ʱ��γ�����ʮ���죬���������룡");
            }


            if (begindate.Year != enddate.Year)
                throw new Exception("�����еĶ������ݲ�֧�ֿ���Ȳ�ѯ�����������룡");


            DateTime dtnewcsdate = DateTime.Parse(newczdate);
            DateTime dtnewendate = dtnewcsdate.AddDays(-1);
            if (enddate.CompareTo(dtnewcsdate) >= 0 && begindate.CompareTo(dtnewcsdate) < 0)
            {
                string nenddate = dtnewendate.ToString("yyyy��MM��dd�� 23:59:59");
                TextBoxEndDate.Text = nenddate;
                throw new Exception("����" + newczdate + "Ϊ��ʼ���ڻ���" + nenddate + "��������!");
            }


            ViewState["fnum"] = "0.00";
            ViewState["fnumMax"] = "20000000.00";
            ViewState["fstate"] = "0";

            ViewState["uid"] = classLibrary.setConfig.replaceSqlStr(u_ID);
            ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd HH:mm:ss"));
            begintime = begindate.ToString("yyyy-MM-dd");
            ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd HH:mm:ss"));
            endtime = enddate.ToString("yyyy-MM-dd");

            ViewState["sorttype"] = "1";
            ViewState["querytype"] = "toBank";

            //furion 20060324 �������в�ѯ����
            ViewState["banktype"] = "0000";

            //furion 20050819 �����SQL�����ַ����ж�
         //   txtBankListId.Text = classLibrary.setConfig.replaceSqlStr(u_ID);

        }

        protected int query(string UinListId, string BankListId, string begin, string end)
        {
            int iType = 0;
            try
            {
                if (UinListId == "" && BankListId == "")
                {
                    throw new Exception("�Ƹ�ͨ���������ж���������������һ�");
                }

                if (UinListId != "")//���ݲƸ�ͨ����
                {
                    listID.Add(UinListId); // ����ǽ��׵� �Ͷ�ȡtextBox�е�ֵ

                    iType = 4;
                }
                else
                {
                    if (BankListId != "")
                    {
                        ValidateDate(BankListId, begin, end);
                        //���ݸ������е��Ų�ѯ���׵���
                        Query_Service.Query_Service qs = new Query_Service.Query_Service();
                        DateTime begindate = DateTime.Parse(begin);
                        DateTime enddate = DateTime.Parse(end);
                        List<String> idList = new List<string>();
                        try
                        {
                            if (qs.IsNewOrderCZData(enddate))
                            {
                                idList = BindDataListId(1, false);

                            }
                            else
                            {
                                idList = BindDataListId(1, true);

                            }
                            if (idList != null && idList.Count > 0)
                            {
                                foreach(string id in idList)
                                    listID.Add(id); 
                            }
                        }
                        catch (LogicException lex)
                        {
                            string errStr = PublicRes.GetErrorMsg(lex.Message.ToString());
                            throw new Exception(begin + "��" + end + "ʱ��θ��ݸ������е���" + BankListId + "��ѯ���׵��ų���" + errStr);
                        }
                        catch (SoapException eSoap) //����soap���쳣
                        {
                            string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                            throw new Exception(begin + "��" + end + "ʱ��θ��ݸ������е��Ų�ѯ���׵���" + BankListId + "���÷������" + errStr);
                        }
                        catch (Exception eSys)
                        {
                            string errStr = PublicRes.GetErrorMsg(eSys.Message.ToString());
                            throw new Exception(begin + "��" + end + "ʱ��θ��ݸ������е��Ų�ѯ���׵���" + BankListId + "��ȡ����ʧ��:" + errStr);
                        }
                        // iType = 1;�����ǽ��׵��ţ�һ���Ĳ�ѯ
                        iType = 4;
                    }
                }

                //�󶨽��׵�������Ϣ
                Session["ListID"] = listID;

                return iType;

            }
            catch (Exception es)
            {
                LogError("TradeManage.BankOrderListQuery", "protected int query(string UinListId, string BankListId, string begin, string end)", es);
                throw new Exception("��ѯ����:" + es.Message.ToString());
            }
        }

        /// <summary>
        /// ��ʱ�����ѯ������Ϣ
        /// </summary>
        DataTable tmpListData = null;

        private List<String> BindDataListId(int index, bool isold)
        {
            string u_ID = ViewState["uid"].ToString();
            DateTime begindate = (DateTime)ViewState["begindate"];
            DateTime enddate = (DateTime)ViewState["enddate"];

            string sorttype = ViewState["sorttype"].ToString();
            string queryType = ViewState["querytype"].ToString();

            begintime = begindate.ToString("yyyy-MM-dd");
            endtime = enddate.ToString("yyyy-MM-dd");

            float fnum = float.Parse(ViewState["fnum"].ToString());
            float fnumMax = float.Parse(ViewState["fnumMax"].ToString());

            int fstate = Int32.Parse(ViewState["fstate"].ToString());

            int max = 100;
            int start = max * (index - 1) + 1;

            int newmax = 100;
            int newstart = max * (index - 1);

            string banktype = ViewState["banktype"].ToString();


            int fcurtype = 1;

            DataSet ds = null;
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
            qs.Finance_HeaderValue = fh;
            if (isold)
            {
                //furion ������ʷ��¼��ѯ 20060522
                bool isHistory = true;

                ds = qs.GetFundList(u_ID, queryType, fcurtype, begindate, enddate, fstate, fnum, fnumMax, banktype, sorttype, isHistory, start, max);

            }
            else
            {
                ds = qs.GetBankRollListByListId(u_ID, queryType, fcurtype, begindate, enddate, fstate, fnum, fnumMax, banktype, sorttype, newstart, newmax);

            }
            if ((queryType == "toBank") && !u_ID.ToUpper().StartsWith("CFT"))
            {
                DataTable cftDetail = new DataTable();
                if (ds != null && ds.Tables.Count == 1)
                {
                    cftDetail = ds.Tables[0];
                }

                for (int i = 1; i < 9; i++)
                {
                    string newUID = "CFT0" + i.ToString() + u_ID;
                    DataSet tmpDS = null;
                    if (isold)
                    {
                        tmpDS = qs.GetFundList(newUID, queryType, fcurtype, begindate, enddate, fstate, fnum, fnumMax, banktype, sorttype, true, start, max);
                    }
                    else
                    {
                        tmpDS = qs.GetBankRollListByListId(newUID, queryType, fcurtype, begindate, enddate, fstate, fnum, fnumMax, banktype, sorttype, newstart, newmax);
                    }
                    DataTable tmpDetail = null;
                    if (tmpDS != null && tmpDS.Tables.Count == 1)
                    {
                        tmpDetail = tmpDS.Tables[0];
                    }
                    if (tmpDetail != null && tmpDetail.Rows.Count > 0)
                    {
                        if (cftDetail == null || cftDetail.Rows.Count == 0)
                        {
                            ds = tmpDS;
                            cftDetail = ds.Tables[0];

                        }
                        else
                        {
                            foreach (DataRow dr2 in tmpDetail.Rows)
                            {
                                cftDetail.ImportRow(dr2);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //�ο���ֵ��¼��ѯ���鲻����¼��ȥ����ʷ
            if (!isold && (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0))
            {
                ds = qs.GetFundList(u_ID, queryType, fcurtype, begindate, enddate, fstate, fnum, fnumMax, banktype, sorttype, true, start, max);
            }


            List<String> idlist = new List<string>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //��ѯ��������׵���
                    idlist.Add(dr["Flistid"].ToString());
                }

                if (tmpListData == null)
                {
                    tmpListData = ds.Tables[0];
                }
                else {
                    tmpListData.Merge(ds.Tables[0]);
                }

            }


            return idlist;
        }

        private DataSet TradeInfo(int iType)
        {
            try
            {
                if (Session["uid"] == null)
                {
                    Response.Redirect("../login.aspx?wh=1"); //���µ�½
                }

                //�󶨽���������Ϣ
                Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                List<String> listID = (List<String>)Session["ListID"];//ѭ����ѯû��listԪ��

                List<DataSet> listD = new List<DataSet>();
                DataSet dsAll = new DataSet();
                DataSet ds = new DataSet();
                foreach (string id in listID)
                {
                    if (id.Length < 28)//���׵���С��28λ�Ĳ���ʾ
                        continue;

                    string selectStrSession = id;

                    DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());

                    DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());

                    int istr = 1;
                    int imax = 2;

                    ds = myService.GetPayList(selectStrSession, iType, beginTime, endTime, istr, imax);

                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                        continue;
                   
                    ds.Tables[0].Columns.Add("Fpaynum_str"); //���׽��
                    //ds.Tables[0].Columns.Add("Flstate_str"); //���׵���״̬
                    ds.Tables[0].Columns.Add("CompanyName"); //�̻�����
                    ds.Tables[0].Columns.Add("WWWAdress"); //�̻���ַ
                    ds.Tables[0].Columns.Add("Fbuy_bank_type_str"); //��������
                    ds.Tables[0].Columns.Add("TradeState_str"); //����״̬
                    ds.Tables[0].Columns.Add("Faid"); //�ʺ�

                    //ds.Tables[0].Columns.Add("tbegindate", typeof(string));
                    //ds.Tables[0].Columns.Add("tenddate", typeof(string));

                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "Fpaynum_str");//���׽��
                    DataSet buss_ds = new DataSet();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        row["Fbuy_bank_type_str"] = Transfer.convertbankType(ds.Tables[0].Rows[0]["Fbuy_bank_type"].ToString());
                      //  row["Flstate_str"] = classLibrary.setConfig.convertTradeState(ds.Tables[0].Rows[0]["Flstate"].ToString());
                        string spid = ds.Tables[0].Rows[0]["Flistid"].ToString().Substring(0, 10);//�̻���Ϊ�Ƹ�ͨ������ǰʮλ
                        buss_ds = myService.GetPayBusinessList("", spid.Trim(), "", "");
                        if (buss_ds == null || buss_ds.Tables.Count < 1 || buss_ds.Tables[0].Rows.Count < 1)
                        {
                            row["CompanyName"] = "";
                            row["WWWAdress"] = "";
                        }
                        else
                        {
                            row["CompanyName"] = buss_ds.Tables[0].Rows[0]["CompanyName"].ToString();
                            row["WWWAdress"] = buss_ds.Tables[0].Rows[0]["WWWAdress"].ToString();

                        }
                        string memo = ds.Tables[0].Rows[0]["Fmemo"].ToString();

                        //��ѯ����״̬
                        bool isC2C = false;
                        int type = 0;
                        if (int.TryParse(ds.Tables[0].Rows[0]["Ftrade_type"].ToString(), out type))
                        {
                            if (type == 1)
                            {
                                isC2C = true;
                            }
                        }
                        if (ds.Tables[0].Rows[0]["Flistid"].ToString() != "")
                        {
                            var ID = ds.Tables[0].Rows[0]["Flistid"].ToString();
                            Query_Service.Query_Service qs = new Query_Service.Query_Service();
                            DataSet dsState = qs.GetQueryListDetail(ID);

                            if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                            {
                                dsState.Tables[0].Columns.Add("Ftrade_stateName");
                                classLibrary.setConfig.GetColumnValueFromDic(dsState.Tables[0], "Ftrade_state", "Ftrade_stateName", "PAY_STATE");
                                row["TradeState_str"] = dsState.Tables[0].Rows[0]["Ftrade_stateName"].ToString();
                                if (isC2C)
                                {
                                    myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                                    var dsList = myService.GetBankRollList_withID(DateTime.Now.AddDays(-PublicRes.PersonInfoDayCount), DateTime.Now.AddDays(1), ID, 1, 50);
                                    bool isRefund = false;
                                    bool isCompelete = false;
                                    if (dsList != null && dsList.Tables.Count > 0 && dsList.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dsList.Tables[0].Rows)
                                        {
                                            var state = dr["Fsubject"].ToString();
                                            int stateNum = 0;
                                            if (int.TryParse(state, out stateNum))
                                            {
                                                if (stateNum == 5 || stateNum == 6)
                                                {
                                                    isRefund = true;
                                                }
                                                else if (stateNum == 3 || stateNum == 4 || stateNum == 8)
                                                {
                                                    isCompelete = true;
                                                }
                                            }
                                        }
                                    }

                                    if (isRefund)
                                    {
                                        row["TradeState_str"] = "ת���˿�";
                                    }
                                    else if (isCompelete)
                                    {
                                        row["TradeState_str"] = "�������";
                                    }
                                }
                            }
                        }


                    }
                    listD.Add(ds);

                }

                if (listD != null && listD.Count > 0)
                {
                    dsAll = listD[0];
                    for (int i = 1; i < listD.Count; i++)
                    {
                        dsAll = PublicRes.ToOneDataset(dsAll, listD[i]);//�ϲ����dataset
                    }
                }

                //����Ƿ���©����������Ϣ��ʾ
                CheckDataShow(ref dsAll);

                return dsAll;
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                throw new Exception("���÷������" + errStr);
            }
            catch (Exception eSys)
            {

                LogError("ForeignCurrencyPay.BankOrderListQuery", "BindInfo(typeid, listid, qry_time);", eSys);
                //WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                throw new Exception("��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private DataSet SearchMore()
        {
            try
            {
                string path = Server.MapPath("~/") + "PLFile" + "\\refund.xls";
                File1.PostedFile.SaveAs(path);

                DataSet res_ds = PublicRes.readXls(path);
                DataTable res_dt = res_ds.Tables[0];
                int iColums = res_dt.Columns.Count;
                int iRows = res_dt.Rows.Count;
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = new DataSet();
                //DataSet dsOne = new DataSet();
                List<DataSet> listD = new List<DataSet>();
                for (int i = 0; i < iRows; i++)
                {
                    string r1 = res_dt.Rows[i][0].ToString().Trim();//�Ƹ�ͨ����
                    string r2 = res_dt.Rows[i][1].ToString().Trim();//���ж�����
                    string r3 = res_dt.Rows[i][2].ToString().Trim();//��ʼ����
                    string r4 = res_dt.Rows[i][3].ToString().Trim();//��������XZ 
                    if (string.IsNullOrEmpty(r1) && string.IsNullOrEmpty(r2)) break;

                    DateTime begindate = new DateTime(), enddate = new DateTime();
                    string s_date = ""; string e_date = "";
                    try
                    {

                        if (r3 != null && r3 != "")
                        {
                            begindate = DateTime.Parse(r3);
                            s_date = DateTime.Parse(r3).ToString("yyyy��MM��dd�� 00:00:00");
                        }
                        if (r4 != null && r4 != "")
                        {
                            enddate = DateTime.Parse(r4);
                            e_date = enddate.ToString("yyyy��MM��dd�� 23:59:59");
                        }
                    }
                    catch
                    {
                        throw new Exception("���ж�����"+r2 + "��ѯ������������");
                    }
                    query(r1, r2, s_date, e_date);

                }
                ds = TradeInfo(4);//int iType����4

                return ds;
            }
            catch (Exception eSys)
            {
                throw new Exception("��ȡ����ʧ�ܣ�" +eSys.Message.ToString());
            }
        }

        /// <summary>
        /// ������ж�����Ϣ�Ƿ��ѯ��ʾ����û����ʾ�������ʾ
        /// v_swuzhang
        /// 2016-03-10
        /// </summary>
        /// <param name="ds"></param>
        private void CheckDataShow(ref DataSet dsc)
        {
            try
            {
                if (tmpListData != null && tmpListData.Rows.Count > 0 && dsc != null && dsc.Tables.Count > 0)
                {
                    LogHelper.LogInfo("BankOrderListQuery : tmpListData.Count:" + tmpListData.Rows.Count);

                    tmpListData.Columns.Add("FNewNum", typeof(String));
                    tmpListData.Columns.Add("FStateName", typeof(String));
                    setConfig.FenToYuan_Table(tmpListData, "FNum", "FNewNum");
                    classLibrary.setConfig.GetColumnValueFromDic(tmpListData, "Fsign", "FStateName", "TCLIST_SIGN");

                    tmpListData.Columns.Add("FbankName", typeof(String));
                    classLibrary.setConfig.GetColumnValueFromDic(tmpListData, "Fbank_type", "FbankName", "BANK_TYPE");

                    for (int i = 0; i < tmpListData.Rows.Count; i++)
                    {
                        var rowdata = tmpListData.Rows[i];

                        LogHelper.LogInfo("BankOrderListQuery : tmpListData.Fbank_listid=" + rowdata["Fbank_list"] + ",Flistid=" + rowdata["Flistid"]);

                        var tmpData = dsc.Tables[0].Select("Fbank_list =" + rowdata["Fbank_list"]);
                        if (tmpData == null || tmpData.Length <= 0)
                        {
                            //�������
                            DataRow dr = dsc.Tables[0].NewRow();
                            dr["Fbank_listid"] = rowdata["Fbank_list"];
                            dr["Flistid"] = rowdata["Flistid"];
                            dr["Faid"] = rowdata["Faid"];
                            dr["Fpay_time"] = rowdata["Fpay_time"];
                            dr["Fpaynum_str"] = rowdata["FNewNum"];
                            dr["TradeState_str"] = rowdata["FStateName"];
                            dr["Fbuy_bank_type_str"] = rowdata["FbankName"];

                            dsc.Tables[0].Rows.Add(dr);
                        }
                    }
                }
            }
            catch(Exception ef) {
                LogError("BankOrderListQuery ", " private void CheckDataShow(ref DataSet dsc)", ef);
            }
        }

        public void btnSearchMore_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!File1.HasFile)
                {
                    WebUtils.ShowMessage(this.Page, "��ѡ���ϴ��ļ���");
                    return;
                }
                if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
                {
                    DataSet ds = SearchMore();
                    ViewState["gt_more"] = ds;
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    {
                        DataGrid1.DataSource = null;
                        DataGrid1.DataBind();
                        throw new Exception("���ݿ��޴˼�¼");
                    }
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();

                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "�ļ���ʽ����ȷ����ѡ��xls��ʽ�ļ��ϴ���");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }
        }

        private void BindDataOutExcel()
        {
            try
            {

                DataSet ds = (DataSet)ViewState["gt_more"];
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "��ѯ���Ϊ��");
                    return;
                }

                DataTable dt = ds.Tables[0];
                StringWriter sw = new StringWriter();
                string excelHeader = DataGrid1.Columns[0].HeaderText;
                for (int i = 1; i < DataGrid1.Columns.Count; i++)
                {
                    excelHeader += "\t" + DataGrid1.Columns[i].HeaderText;
                }
                sw.WriteLine(excelHeader);
                foreach (DataRow dr in dt.Rows)
                {
                    sw.WriteLine("=\"" + dr["Fbank_listid"].ToString() + "\"\t=\"" + dr["Flistid"].ToString() + "\"\t=\"" + dr["Fpaynum_str"] + "\"\t=\""
                        + dr["TradeState_str"] + "\"\t=\"" + dr["CompanyName"] + "\"\t=\"" + dr["WWWAdress"] + "\"\t=\"" + dr["Fbuy_bank_type_str"] + "\"\t=\"" + dr["Fmemo"] + "\"");
                }
                sw.Close();
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("���ж���������ѯ", Encoding.UTF8) + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(sw);
                Response.End();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); return;
            }
        }

        protected void btn_outExcel_Click(object sender, System.EventArgs e)
        {
            BindDataOutExcel();

        }

        public  string ConvertDateToString(object objval)
        {
            string retVal = string.Empty;

            if (objval == null) {
                return retVal;
            }

            DateTime dtime = DateTime.Now;
            if (DateTime.TryParse(objval.ToString(), out dtime)) {
                retVal= dtime.ToString("yyyyMMddHHmmss");
            }

            return retVal;
        }

    }
}
