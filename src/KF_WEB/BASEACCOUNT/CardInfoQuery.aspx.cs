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
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// SysBulletinManage 的摘要说明。
    /// </summary>
    public partial class CardInfoQuery : System.Web.UI.Page
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
                TextBoxBeginDate.Value = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

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
            string begindate = TextBoxBeginDate.Value.Trim();
            string enddate = TextBoxEndDate.Value.Trim();
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
                string nenddate = dtnewendate.ToString("yyyy-MM-dd 23:59:59");
                TextBoxEndDate.Value = nenddate;
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
                                foreach (string id in idList)
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
                            throw new Exception(begin + "至" + end + "时间段根据给定银行单号查询交易单号" + BankListId + "调用服务出错：" + eSoap);
                        }
                        catch (Exception eSys)
                        {
                            throw new Exception(begin + "至" + end + "时间段根据给定银行单号查询交易单号" + BankListId + "读取数据失败！" + eSys);
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
                throw new Exception("查询出错:" + es.Message.ToString());
            }
        }
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

            List<String> idlist = new List<string>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //查询出多个交易单号
                    idlist.Add(dr["Flistid"].ToString());
                }
                return idlist;
            }
            else
            {
                throw new LogicException("没有找到记录！");
            }
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
                    {
                        DataGrid1.DataSource = null;
                        DataGrid1.DataBind();
                        throw new Exception("数据库无此记录");
                    }
                    ds.Tables[0].Columns.Add("Fpaynum_str"); //交易金额
                    ds.Tables[0].Columns.Add("Fbuy_bank_type_str"); //银行类型
                    ds.Tables[0].Columns.Add("Fcard_tail"); //银行卡号
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "Fpaynum_str");//交易金额
                    DataSet dsCard = new DataSet();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        row["Fbuy_bank_type_str"] = Transfer.convertbankType(ds.Tables[0].Rows[0]["Fbuy_bank_type"].ToString());
                        string serialno = PublicRes.objectToString(ds.Tables[0], "Fbuy_bankid");//支付绑定序列号
                        string qqid = ds.Tables[0].Rows[0]["Fbuyid"].ToString();//买家账户号码
                        string uid = ds.Tables[0].Rows[0]["Fbuy_uid"].ToString();//买家内部帐号
                        dsCard = myService.GetBankCardBindList_New(qqid, "", "", uid, "", "", "", "", "", "", 0, false, 99, serialno, 0, 1);
                        if (dsCard == null || dsCard.Tables.Count < 1 || dsCard.Tables[0].Rows.Count < 1)
                            row["Fcard_tail"] = "";
                        else
                            row["Fcard_tail"] = dsCard.Tables[0].Rows[0]["Fcard_tail"];
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

                return dsAll;
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                throw new Exception("调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
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
                //DataSet dsOne;
                List<DataSet> listD = new List<DataSet>();
                for (int i = 0; i < iRows; i++)
                {
                    string r1 = res_dt.Rows[i][0].ToString().Trim();//财付通订单
                    string r2 = res_dt.Rows[i][1].ToString().Trim();//银行订单号
                    string r3 = res_dt.Rows[i][2].ToString().Trim();//开始日期
                    string r4 = res_dt.Rows[i][3].ToString().Trim();//结束日期

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
                        throw new Exception("银行订单号" + r2 + "查询日期输入有误！");
                    }
                    query(r1, r2, s_date, e_date);

                }

                ds = TradeInfo(4);//int iType总是4

                return ds;
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
                return null;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return null;
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
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString())); return;
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
                    sw.WriteLine("=\"" + dr["Flistid"].ToString() + "\"\t=\"" + dr["Fbank_listid"].ToString() + "\"\t=\"" + dr["Fcard_tail"] + "\"\t=\""
                        + dr["Fbuy_bank_type_str"] + "\"\t=\"" + dr["Fpaynum_str"] + "\"\t=\"" + dr["Fbuy_name"] + "\"");
                }
                sw.Close();
                Response.AddHeader("Content-Disposition", "attachment; filename=卡信息批量查询.xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
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

    }
}
