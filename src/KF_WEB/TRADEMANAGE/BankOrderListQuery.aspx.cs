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
    /// SysBulletinManage 的摘要说明。
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
                    throw new Exception("数据库无此记录");
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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
                throw new Exception("日期输入有误！");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("终止日期小于起始日期，请重新输入！");
            }
            string newczdate = PublicRes.GetZWDicValue("OldOrderCZDataEndTime");
            if (newczdate == null || newczdate == "")
            {
                throw new Exception("未查询到OldOrderCZDataEndTime对应的配置值！");
            }


            if (begindate.AddDays(15).CompareTo(enddate) < 0)
            {
                throw new Exception("选择时间段超过了十五天，请重新输入！");
            }


            if (begindate.Year != enddate.Year)
                throw new Exception("给银行的订单号暂不支持跨年度查询，请重新输入！");


            DateTime dtnewcsdate = DateTime.Parse(newczdate);
            DateTime dtnewendate = dtnewcsdate.AddDays(-1);
            if (enddate.CompareTo(dtnewcsdate) >= 0 && begindate.CompareTo(dtnewcsdate) < 0)
            {
                string nenddate = dtnewendate.ToString("yyyy年MM月dd日 23:59:59");
                TextBoxEndDate.Text = nenddate;
                throw new Exception("请以" + newczdate + "为开始日期或以" + nenddate + "结束日期!");
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

            //furion 20060324 增加银行查询条件
            ViewState["banktype"] = "0000";

            //furion 20050819 加入对SQL敏感字符的判断
         //   txtBankListId.Text = classLibrary.setConfig.replaceSqlStr(u_ID);

        }

        protected int query(string UinListId, string BankListId, string begin, string end)
        {
            int iType = 0;
            try
            {
                if (UinListId == "" && BankListId == "")
                {
                    throw new Exception("财付通订单和银行订单号请至少输入一项！");
                }

                if (UinListId != "")//根据财付通订单
                {
                    listID.Add(UinListId); // 如果是交易单 就读取textBox中的值

                    iType = 4;
                }
                else
                {
                    if (BankListId != "")
                    {
                        ValidateDate(BankListId, begin, end);
                        //根据给定银行单号查询交易单号
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
                            throw new Exception(begin + "至" + end + "时间段根据给定银行单号" + BankListId + "查询交易单号出错" + errStr);
                        }
                        catch (SoapException eSoap) //捕获soap类异常
                        {
                            string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                            throw new Exception(begin + "至" + end + "时间段根据给定银行单号查询交易单号" + BankListId + "调用服务出错：" + errStr);
                        }
                        catch (Exception eSys)
                        {
                            string errStr = PublicRes.GetErrorMsg(eSys.Message.ToString());
                            throw new Exception(begin + "至" + end + "时间段根据给定银行单号查询交易单号" + BankListId + "读取数据失败:" + errStr);
                        }
                        // iType = 1;现在是交易单号，一样的查询
                        iType = 4;
                    }
                }

                //绑定交易单基础信息
                Session["ListID"] = listID;

                return iType;

            }
            catch (Exception es)
            {
                LogError("TradeManage.BankOrderListQuery", "protected int query(string UinListId, string BankListId, string begin, string end)", es);
                throw new Exception("查询出错:" + es.Message.ToString());
            }
        }

        /// <summary>
        /// 临时保存查询订单信息
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
                //furion 加入历史记录查询 20060522
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

            //参考充值记录查询，查不到记录就去查历史
            if (!isold && (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0))
            {
                ds = qs.GetFundList(u_ID, queryType, fcurtype, begindate, enddate, fstate, fnum, fnumMax, banktype, sorttype, true, start, max);
            }


            List<String> idlist = new List<string>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //查询出多个交易单号
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
                    Response.Redirect("../login.aspx?wh=1"); //重新登陆
                }

                //绑定交易资料信息
                Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                List<String> listID = (List<String>)Session["ListID"];//循环查询没有list元素

                List<DataSet> listD = new List<DataSet>();
                DataSet dsAll = new DataSet();
                DataSet ds = new DataSet();
                foreach (string id in listID)
                {
                    if (id.Length < 28)//交易单号小于28位的不显示
                        continue;

                    string selectStrSession = id;

                    DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());

                    DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());

                    int istr = 1;
                    int imax = 2;

                    ds = myService.GetPayList(selectStrSession, iType, beginTime, endTime, istr, imax);

                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                        continue;
                   
                    ds.Tables[0].Columns.Add("Fpaynum_str"); //交易金额
                    //ds.Tables[0].Columns.Add("Flstate_str"); //交易单的状态
                    ds.Tables[0].Columns.Add("CompanyName"); //商户名称
                    ds.Tables[0].Columns.Add("WWWAdress"); //商户网址
                    ds.Tables[0].Columns.Add("Fbuy_bank_type_str"); //银行类型
                    ds.Tables[0].Columns.Add("TradeState_str"); //交易状态
                    ds.Tables[0].Columns.Add("Faid"); //帐号

                    //ds.Tables[0].Columns.Add("tbegindate", typeof(string));
                    //ds.Tables[0].Columns.Add("tenddate", typeof(string));

                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "Fpaynum_str");//交易金额
                    DataSet buss_ds = new DataSet();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        row["Fbuy_bank_type_str"] = Transfer.convertbankType(ds.Tables[0].Rows[0]["Fbuy_bank_type"].ToString());
                      //  row["Flstate_str"] = classLibrary.setConfig.convertTradeState(ds.Tables[0].Rows[0]["Flstate"].ToString());
                        string spid = ds.Tables[0].Rows[0]["Flistid"].ToString().Substring(0, 10);//商户号为财付通订单号前十位
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

                        //查询交易状态
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
                                        row["TradeState_str"] = "转入退款";
                                    }
                                    else if (isCompelete)
                                    {
                                        row["TradeState_str"] = "交易完成";
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
                        dsAll = PublicRes.ToOneDataset(dsAll, listD[i]);//合并多个dataset
                    }
                }

                //检测是否遗漏批量搜索信息显示
                CheckDataShow(ref dsAll);

                return dsAll;
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                throw new Exception("调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {

                LogError("ForeignCurrencyPay.BankOrderListQuery", "BindInfo(typeid, listid, qry_time);", eSys);
                //WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                throw new Exception("读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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
                    string r1 = res_dt.Rows[i][0].ToString().Trim();//财付通订单
                    string r2 = res_dt.Rows[i][1].ToString().Trim();//银行订单号
                    string r3 = res_dt.Rows[i][2].ToString().Trim();//开始日期
                    string r4 = res_dt.Rows[i][3].ToString().Trim();//结束日期XZ 
                    if (string.IsNullOrEmpty(r1) && string.IsNullOrEmpty(r2)) break;

                    DateTime begindate = new DateTime(), enddate = new DateTime();
                    string s_date = ""; string e_date = "";
                    try
                    {

                        if (r3 != null && r3 != "")
                        {
                            begindate = DateTime.Parse(r3);
                            s_date = DateTime.Parse(r3).ToString("yyyy年MM月dd日 00:00:00");
                        }
                        if (r4 != null && r4 != "")
                        {
                            enddate = DateTime.Parse(r4);
                            e_date = enddate.ToString("yyyy年MM月dd日 23:59:59");
                        }
                    }
                    catch
                    {
                        throw new Exception("银行订单号"+r2 + "查询日期输入有误！");
                    }
                    query(r1, r2, s_date, e_date);

                }
                ds = TradeInfo(4);//int iType总是4

                return ds;
            }
            catch (Exception eSys)
            {
                throw new Exception("读取数据失败！" +eSys.Message.ToString());
            }
        }

        /// <summary>
        /// 检测所有订单信息是否查询显示，若没有显示则添加显示
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
                            //添加数据
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
                    WebUtils.ShowMessage(this.Page, "请选择上传文件！");
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
                        throw new Exception("数据库无此记录");
                    }
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();

                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "文件格式不正确，请选择xls格式文件上传。");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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
                    WebUtils.ShowMessage(this, "查询结果为空");
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
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("银行订单批量查询", Encoding.UTF8) + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(sw);
                Response.End();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "导出数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString())); return;
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
