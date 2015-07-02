using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace CFT.CSOMS.DAL.Infrastructure
{
    #region 基类

    /// <summary>
    /// 交易单表的类形式
    /// </summary>
    public class T_PAY_LIST
    {
        public string u_ListID;
        public string u_Coding;					//订单编码
        public string u_SPID;					//机构代码名称（发起者）
        public string u_Bank_ListID;			//给银行的订单号
        public string u_Pay_Type;					//支付类型
        public string u_BuyID;					//买家帐户号码
        public string u_Buy_Name;				//付款方名称
        public string u_Buy_Bank_Type;				//买家开户行
        public string u_Buy_BankID;				//买家的银行帐号
        public string u_SaleID;					//卖家帐户号码
        public string u_Sale_Name;				//卖家的名称
        public string u_CurType;					//币种代码
        public string u_State;						//交易状态
        public string u_LState;					//交易单状态
        public string u_Price;						//产品的价格
        public string u_Carriage;					//物流费用
        public string u_PayNum;					//应支付的总价格
        public string u_Fact;						//实际支付费用
        public string u_Procedure;					//交易（服务）手续费
        public string u_Service;					//服务费率
        public string u_Cash;						//现金支付金额
        public string u_Create_Time_C2C;		//定单创建时间（c2c）
        public string u_Create_Time;			//定单创建时间（本地系统时间）
        public string u_Bargain_Time;			//买家付款时间(bank)
        public string u_Receive_Time_C2C;		//打款给卖家时间(c2c)
        public string u_Receive_Time;			//打款给卖家时间（本地）
        public string u_IP;						//最后修改交易单的IP
        public string u_Memo;					//交易说明
        public string u_Explain;				//备注（后台人员操作记录）
        public string u_Modify_Time;			//最后修改时间（c2c）/ 本地系统时间

        public string u_Pay_Time;
    }

    /// <summary>
    /// 腾讯付款记录表的类形式
    /// </summary>
    public class T_TCBANKPAY_LIST
    {
        public string u_ListID;
        public string u_Tde_ID;
        public string u_Bank_List;
        public string u_Bankid;
        public string u_State;
        public string u_Type;
        public string u_Subject;
        public string u_Num;
        public string u_Sign;
        public string u_Bank_Acc;
        public string u_Bank_Type;
        public string u_Curtype;
        public string u_Aid;
        public string u_ABankid;
        public string u_aName;
        public string u_Prove;
        public string u_IP;
        public string u_Memo;
        public string u_Modify_Time;
        public string u_Pay_Front_Time;
        public string u_Pay_Time;
        public string u_Uid;
    }

    /// <summary>
    /// 退款单表的类形式
    /// </summary>
    public class T_REFUND
    {
        public string u_ListID;
        public string u_RListID;				//退款单的ID号
        public string u_Create_Time;
        public string u_SPID;				//机构代码名称（发起者）
        public string u_PayType;				//退款方式
        public string u_BuyID;				//买家帐户号码
        public string u_Buy_Name;			//买家名称
        public string u_Buy_Bank_Type;			//买家银行的类型
        public string u_Buy_BankID;			//买家的银行帐号
        public string u_SaleID;				//卖家帐户号码
        public string u_Sale_Name;			//卖家的名称
        public string u_Sale_Bank_Type;		//卖家银行的类型
        public string u_Sale_BankID;		//卖家的银行帐号
        public string u_State;					//退款的状态（接口）
        public string u_LState;				//退款单的状态
        public string u_PayBuy;				//退还给买家的金额
        public string u_PaySale;				//退还给卖家的金额
        public string u_Procedure;				//交易手续费
        public string u_Bargain_Time;		//C2C请求退款日期
        public string u_OK_Time;			//成交日期（退款日期）
        public string u_IP;					//最后修改的IP地址
        public string u_Memo;				//退款说明
        public string u_Explain;			//操作人员备注
        public string u_Modify_Time;		//最后修改时间
    }

    /// <summary>
    /// 用户帐户表的类形式表示
    /// </summary>
    public class T_USER
    {
        public string u_QQID;
        public string u_CurType;				//币种类型
        public string u_TrueName;				//真实姓名
        public string u_Balance;				//帐户余额
        public string u_Con;					//冻结金额
        public string u_Yday_Balance;			//昨日余额
        public string u_Quota;					//单笔交易限额
        public string u_APay;					//当日已付金额
        public string u_Quota_Pay;				//单日支付限额
        public string u_Save_Time;				//最近存款日期
        public string u_Fetch_Time;				//最近提款日期
        public string u_Login_IP;				//最后登录/修改的IP地址
        public string u_Modify_Time_C2C;		//最后【登录/修改】时间(c2c)
        public string u_State;					//帐户状态
        public string u_Memo;					//用户备注
        public string u_Modify_Time;			//最后修改时间（本地）
        public string u_User_Type;				//用户类型。

        public T_USER()
        {
            //需不需要初始化呢？
        }
    }

    #endregion

    #region	 受控资金信息查询

    public class QeuryUserControledFinInfoClass : Query_BaseForNET
    {
        public QeuryUserControledFinInfoClass(string fuid, string beginDateStr, string endDateStr, string cur_type, int iNumStart, int iNumMax)
        {
            this.ICEcommand = "FINANCE_QUERY_USER_CONTROLED";
            this.ICESql = "uid=" + fuid;

            if (beginDateStr != null && beginDateStr.Trim() != "" && endDateStr != null && endDateStr.Trim() != "")
            {
                this.ICESql += "&createTime_begin=" + beginDateStr + "&createTime_end=" + endDateStr;
            }

            if (cur_type != null && cur_type.Trim() != "")
            {
                this.ICESql += "&cur_type=" + cur_type;
            }

            this.ICESql += "&strlimit=limit " + iNumStart + "," + iNumMax;
        }
    }
    #endregion

    #region 邮储汇款查询类

    public class RemitQueryClass : Query_BaseForNET
    {
        public RemitQueryClass(string batchid, string tranType, string dataType, string remitType, string tranState, string spid, string remitRec, string listID)
        {
            string strWhere = " where 1=1 ";

            if (batchid != null && batchid != "")
            {
                strWhere += " and Fbatchid='" + batchid + "'";
            }

            if (spid != null && spid != "")
            {
                strWhere += " and Fspid='" + spid + "'";
            }

            if (tranType != "99")
            {
                strWhere += " and Ftran_type=" + tranType;
            }

            if (dataType != "99")
            {
                strWhere += " and Fdata_type=" + dataType;
            }

            if (remitType != "99")
            {
                strWhere += " and Fremit_type=" + remitType;
            }

            if (tranState != "99")
            {
                strWhere += " and Ftran_state='" + tranState + "'";
            }

            if (listID != null && listID.Trim() != "")
            {
                strWhere += " and Flistid='" + listID.Trim() + "' ";
            }

            if (remitRec != null && remitRec.Trim() != "")
            {
                strWhere += " and Fremit_rec='" + remitRec.Trim() + "' ";
            }

            fstrSql = "select * from c2c_zwdb.t_remit_order " + strWhere;
            fstrSql_count = "select count(*) from c2c_zwdb.t_remit_order " + strWhere;
        }
    }

    public class QueryRemitStateInfo : Query_BaseForNET
    {
        public QueryRemitStateInfo(string Ford_date, string Ford_ssn)
        {
            string strWhere = " where Ford_date='" + Ford_date + "' and Ford_ssn='" + Ford_ssn + "'";

            fstrSql = "select * from c2c_db_remit.t_remit_list " + strWhere;
        }
    }

    #endregion

    #region 腾讯付款记录表的查询处理

    /// <summary>
    /// 腾讯付款记录表的查询类
    /// </summary>
    public class Q_TCBANKPAY_LIST : Query_BaseForNET
    {
        private string f_strID;
        public Q_TCBANKPAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd)
        {
            f_strID = strID;
            if (iIDType == 0)
            {
                string currtable = "";
                string othertable = "";
                PickQueryClass.GetPayListTableFromTime(dtBegin, out currtable, out othertable);

                //**furion提现单改造20120216
                string whereStr = " where " + PublicRes.GetSqlFromQQ(strID, "fuid") + " and Fcurtype=1 and fpay_front_time_acc between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fbankid!=4 ";//去掉提现表中银行退单

                fstrSql = "";

                DateTime tmpDate = dtBegin;
                while (tmpDate <= dtEnd)
                {
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");

                    fstrSql = fstrSql + " select " + PickQueryClass.GetTcPayListNewFields() + ",10000 as Total from " + TableName + whereStr + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }

                string TableName1 = "c2c_db.t_tcpay_list";
                fstrSql = fstrSql + " select " + PickQueryClass.GetTcPayListOldFields() + ",10000 as Total from " + TableName1 + whereStr + " ";

                fstrSql += " Order by fpay_front_time_acc DESC";

            }
            else
            {
                string currtable = "";
                string othertable = "";
                PickQueryClass.GetPayListTableFromID(f_strID, out currtable, out othertable);

                //**furion提现单改造20120216
                //furion V30_FURION改动 need
                //				fstrSql = "Select * from c2c_db.t_tcpay_list where flistid=(select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "') Order by fpay_front_time DESC";
                fstrSql = "Select " + PickQueryClass.GetTcPayListOldFields() + " from c2c_db.t_tcpay_list where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                fstrSql += " union all Select " + PickQueryClass.GetTcPayListNewFields() + " from " + currtable + " where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                fstrSql += " union all Select " + PickQueryClass.GetTcPayListNewFields() + " from " + othertable + " where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                fstrSql += " Order by fpay_front_time DESC";
            }
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_TCBANKPAY_LIST[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJB"));
            //T_TCBANKPAY_LIST result = new T_TCBANKPAY_LIST();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);

                if (dt.Rows.Count > 0)
                {
                    T_TCBANKPAY_LIST[] result = new T_TCBANKPAY_LIST[dt.Rows.Count];  //多行数据时，用数组存放
                    int i = 0;
                    foreach (DataRow dr in dt.Rows) //？
                    {
                        result[i] = new T_TCBANKPAY_LIST();               //数组的一行存放一个类，多维数组来存放多个类

                        result[i].u_Tde_ID = QueryInfo.GetString(dr["Ftde_id"]);
                        result[i].u_ListID = QueryInfo.GetString(dr["Flistid"]);
                        result[i].u_Bank_List = QueryInfo.GetString(dr["Fbank_list"]);
                        result[i].u_Bankid = QueryInfo.GetString(dr["Fbankid"]);
                        result[i].u_State = QueryInfo.GetInt(dr["Fstate"]);
                        result[i].u_Type = QueryInfo.GetInt(dr["Ftype"]);
                        result[i].u_Subject = QueryInfo.GetInt(dr["Fsubject"]);
                        result[i].u_Num = QueryInfo.GetInt(dr["Fnum"]);
                        result[i].u_Sign = QueryInfo.GetInt(dr["Fsign"]);
                        result[i].u_Bank_Acc = QueryInfo.GetString(dr["Fbank_acc"]);
                        result[i].u_Bank_Type = QueryInfo.GetInt(dr["Fbank_type"]);
                        result[i].u_Curtype = QueryInfo.GetInt(dr["Fcurtype"]);
                        result[i].u_Aid = QueryInfo.GetString(dr["Faid"]);
                        result[i].u_ABankid = QueryInfo.GetString(dr["Fabankid"]);
                        result[i].u_aName = QueryInfo.GetString(dr["Faname"]);
                        result[i].u_Prove = QueryInfo.GetString(dr["Fprove"]);
                        result[i].u_IP = QueryInfo.GetString(dr["Fip"]);
                        result[i].u_Memo = QueryInfo.GetString(dr["Fmemo"]);
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["Fmodify_time"]);
                        result[i].u_Pay_Front_Time = QueryInfo.GetDateTime(dr["Fpay_front_time"]);
                        result[i].u_Pay_Time = QueryInfo.GetDateTime(dr["Fpay_time"]);
                        result[i].u_Uid = QueryInfo.GetString(dr["Fuid"]);

                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("没有查找到相应的记录");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }

        public static string GetBankName(string banktype)
        {
            switch (banktype)
            {
                #region
                case "1001":
                    return "招商银行";

                case "1002":
                    return "中国工商银行";

                case "1003":
                    return "中国建设银行";

                case "1004":
                    return "上海浦东发展银行";

                case "1005":
                    return "中国农业银行";

                case "1006":
                    return "中国民生银行";

                case "1007":
                    return "农行国际卡";

                case "1008":
                    return "深圳发展银行";

                case "1009":
                    return "兴业银行";

                case "1010":
                    return "深圳平安银行";

                case "1011":
                    return "中国邮政储蓄银行";

                case "1020":
                    return "中国交通银行";

                case "1021":
                    return "中信实业银行";

                case "1022":
                    return "中国光大银行";

                case "1023":
                    return "农村合作信用社";

                case "1024":
                    return "上海银行";

                case "1025":
                    return "华夏银行";

                case "1026":
                    return "中国银行";

                case "1027":
                    return "广东发展银行";

                case "1028":
                    return "广东银联";

                case "1099":
                    return "其他银行";

                case "1030":
                    return "工行B2B";
                case "1031":
                    return "招行大额";
                case "1032":
                    return "北京银行";
                case "1033":
                    return "网汇通";
                case "1034":
                    return "建行大额";
                case "1037":
                    return "工行大额";
                case "1038":
                    return "招行基础业务";

                case "1039":
                    return "工行直付";

                case "1040":
                    return "建行B2B";
                case "1041":
                    return "民生借记卡";
                case "1042":
                    return "招行B2B";

                case "2001":
                    return "招行一点通";
                case "2002":
                    return "工行一点通";
                case "3001":
                    return "兴业信用卡";
                case "3002":
                    return "中行信用卡";

                #endregion

                case "9999":
                    return "汇总银行";

                case "0000":
                    return "所有银行";
                default:
                    return "";
            }
        }
    }

    #endregion

    #region 提现查询类
    public class PickQueryClass : Query_BaseForNET
    {
        public PickQueryClass(string u_ID)
        {

            //**furion提现单改造20120216
            fstrSql = "select " + GetTcPayListOldFields() + " from c2c_db.t_tcpay_list where  Flistid='" + u_ID + "'";

            string currtable = "";
            string othertable = "";
            GetPayListTableFromID(u_ID, out currtable, out othertable);

            fstrSql += " union all select " + GetTcPayListNewFields() + " from " + currtable + " where  Flistid='" + u_ID + "'";
            fstrSql += " union all select " + GetTcPayListNewFields() + " from " + othertable + " where  Flistid='" + u_ID + "'";

            //fstrSql_count = "select count(*) from c2c_db.t_tcpay_list where Flistid='" + u_ID + "'";
            fstrSql_count = "select 1";
        }

        // 2012/5/29 添加是否解密银行卡参数
        public PickQueryClass(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype, int idtype, string cashtype, bool isSecret)
        {

            string strGroup = "";
            string strWhere = "";

            if (u_ID != null && u_ID.Trim() != "")
            {
                if (idtype == 0)
                {
                    //furion 20051101 以后查询全以内部ID开始.
                    string uid = PublicRes.ConvertToFuid(u_ID);

                    //strWhere += " where faid='" + u_ID + "' and Fsubject=14 ";
                    //strWhere += " where Fuid=" + uid + " and Fsubject=14 ";
                    strWhere += " where Fuid=" + uid + " ";
                }
                else if (idtype == 1)
                {
                    if (isSecret)
                    {
                        string bankID = BankLib.BankIOX.GetCreditEncode(u_ID, BankLib.BankIOX.fxykconn);
                        strWhere += " where Fabankid='" + bankID + "' ";
                    }
                    else
                    {
                        strWhere += " where Fabankid='" + u_ID + "' ";
                    }
                }
                else if (idtype == 2)
                {
                    strWhere += "  where Flistid='" + u_ID + "' ";
                }
            }

            if (fstate != 0)
            {
                if (strWhere != "")
                {
                    strWhere += " and FSign=" + fstate.ToString() + " ";
                }
                else
                {
                    strWhere += " where FSign=" + fstate.ToString() + " ";
                }
            }

            long num = (long)Math.Round(fnum * 100, 0);
            if (strWhere != "")
            {
                strWhere += " and Fnum>=" + num.ToString() + " ";
            }
            else
            {
                strWhere += " where Fnum>=" + num.ToString() + " ";
            }

            if (banktype != "0000")
            {
                strWhere += " and Fabank_type=" + banktype + " ";
            }
            if (cashtype != "0000")
            {
                strWhere += " and Fbankid=" + cashtype + " ";
            }

            string test = u_BeginTime.ToString("yyyy-MM-dd");
            if (test != "1940-01-01")
            {
                strWhere += " and Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                //从效率考虑,只取开始时间和结束时间所在月的历史记录,其它不取.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime;
                //while(tmpDate <= u_EndTime.AddMonths(1))
                while (tmpDate <= u_EndTime)
                {
                    //string TableName = "c2c_db_tcpay.t_tcpay_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + "( select " + GetTcPayListNewFields() + " from " + TableName + strWhere + ") union all ";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }

            }
            //**furion提现单改造20120216
            string TableName1 = "c2c_db.t_tcpay_list";
            strGroup = strGroup + "( select " + GetTcPayListOldFields() + " from " + TableName1 + strWhere + " )";

            string strorder = "";
            if (sorttype != null && sorttype.Trim() != "")
            {
                if (sorttype.Trim() == "1")
                {
                    strorder = " order by Fpay_front_time_acc asc ";
                }
                else if (sorttype.Trim() == "2")
                {
                    strorder = " order by Fpay_front_time_acc desc ";
                }
                else if (sorttype.Trim() == "3")
                {
                    strorder = " order by Fnum asc ";
                }
                else if (sorttype.Trim() == "4")
                {
                    strorder = " order by Fnum desc ";
                }
            }

            fstrSql = strGroup + strorder;
            fstrSql_count = "select 10000";//" select count(*) from ( " + strGroup + ") a ";

        }

        public PickQueryClass(string listid, DateTime u_BeginTime, DateTime u_EndTime, bool oldflag)
        {
            string strGroup = "";
            string strWhere = "";

            string test = u_BeginTime.ToString("yyyy-MM-dd");

            if (listid != null && listid.Trim() != "")
            {
                strWhere += " where Flistid='" + listid + "' ";

                string currtable = "";
                string othertable = "";
                GetPayListTableFromID(listid, out currtable, out othertable);

                strGroup += "  select " + GetTcPayListNewFields() + " from " + currtable + " where  Flistid='" + listid + "' union all ";
                strGroup += "  select " + GetTcPayListNewFields() + " from " + othertable + " where Flistid='" + listid + "' union all ";
                test = "1940-01-01"; //不再使用里面的循环体构造strgroup
            }
            else
            {
                //strWhere += " where Fsubject=14 ";
            }

            if (test != "1940-01-01")
            {
                if (strWhere == "")
                {
                    strWhere = " where Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
                else
                {
                    strWhere += " and Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

                //从效率考虑,只取开始时间和结束时间所在月的历史记录,其它不取.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime;
                //while(tmpDate <= u_EndTime.AddMonths(1))
                while (tmpDate <= u_EndTime)
                {
                    //string TableName = "c2c_db_tcpay.t_tcpay_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select " + GetTcPayListNewFields() + " from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }

            }
            //**furion提现单改造20120216
            string TableName1 = "c2c_db.t_tcpay_list";
            strGroup = strGroup + " select " + GetTcPayListOldFields() + " from " + TableName1 + strWhere + " ";


            fstrSql = strGroup;
            fstrSql_count = "select 10000";//" select count(*) from ( " + strGroup + ")  a ";
        }

        public static string GetTcPayListNewFields()
        {
            return " Ftde_id,Flistid,Fdraw_id,Fsp_batch,Fsp_serial,Fsp_operator,Fspid,Fbankid,Fproduct,Fbusiness_type,Ftype,Fsubject,Fnum,"
                + "Fcharge,Fcharge_payer,Fcharge_recv_uid,Fstate,Fsign,Fbank_list,Fbank_acc,Fbank_type,Fcurtype,Fuid,Faid,Faname,Fabank_type,"
                + "Fabankid,Fprove,Fip,Fmemo,Fbank_memo,Fresult,Frefund_ticket_flag,Frefund_ticket_list,Fpay_front_time,Fpay_front_time_acc,"
                + "Fpay_time,Fpay_time_acc,Fmodify_time,Fbank_name,Farea,Fcity,Facc_name,Fuser_type,Fstandby1,Fstandby2,Fstandby3,Fstandby4,Fstandby5,Fstandby6 ";
        }

        public static string GetTcPayListOldFields()
        {

            return " Ftde_id,Flistid,'' as Fdraw_id,'' as Fsp_batch,'' as Fsp_serial,'' as Fsp_operator,Fspid,Fbankid,0 as Fproduct,0 as Fbusiness_type,Ftype,Fsubject,Fnum,"
                + "0 as Fcharge,0 as Fcharge_payer,0 as Fcharge_recv_uid,Fstate,Fsign,Fbank_list,Fbank_acc,Fbank_type,Fcurtype,Fuid,Faid,Faname,Fabank_type,"
                + "Fabankid,Fprove,Fip,Fmemo,'' as Fbank_mem,'' as Fresult,1 as Frefund_ticket_flag,'' as Frefund_ticket_list,Fpay_front_time,Fpay_front_time_acc,"
                + "Fpay_time,Fpay_time_acc,Fmodify_time,Fbank_name,Farea,Fcity,Facc_name,Fuser_type,0 as Fstandby1,0 as Fstandby2,'' as Fstandby3,'' as Fstandby4,'' as Fstandby5,'' as Fstandby6 ";
        }

        public static void GetPayListTableFromTime(DateTime datetime, out string currtable, out string othertable)
        {
            currtable = "c2c_db.t_tcpay_list_" + datetime.ToString("yyyyMM");

            if (datetime.Day > 15)
            {
                othertable = "c2c_db.t_tcpay_list_" + datetime.AddMonths(1).ToString("yyyyMM");
            }
            else
            {
                othertable = "c2c_db.t_tcpay_list_" + datetime.AddMonths(-1).ToString("yyyyMM");
            }
        }

        public static void GetPayListTableFromID(string listid, out string currtable, out string othertable)
        {
            //1、3位系统ID+YYYYMMDD+10位流水号；
            //2、3位系统ID+10位商户号+YYYYMMDD+7位序列号

            string strdate = "";
            if (listid.Length == 21)
            {
                strdate = listid.Substring(3, 4) + "-" + listid.Substring(7, 2) + "-" + listid.Substring(9, 2);
            }
            else if (listid.Length == 28)
            {
                strdate = listid.Substring(13, 4) + "-" + listid.Substring(17, 2) + "-" + listid.Substring(19, 2);
            }

            DateTime dt = DateTime.Now;
            try
            {
                dt = DateTime.Parse(strdate);
            }
            catch
            {
                dt = DateTime.Now;
            }

            GetPayListTableFromTime(dt, out currtable, out othertable);
        }
    }

    #endregion

    #region 退款单表的查询处理
    /// <summary>
    /// 退款单表的查询类
    /// </summary>
    public class Q_REFUND : Query_BaseForNET
    {
        private string f_strID;
        public Q_REFUND(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd)
        {
            f_strID = strID;

            if (iIDType == 0 || iIDType == 1)
            {
                string fuid = PublicRes.ConvertToFuid(strID);
                if (fuid == null)
                    fuid = "0";

                string whereStr = " where fbuy_uid=" + fuid + " ";
                if (iIDType == 1)
                {
                    whereStr = " where fsale_uid=" + fuid + " ";
                }

                if (dtBegin.ToString("yyyy-MM-dd") != "1900-01-01" && dtEnd.ToString("yyyy-MM-dd") != "4000-01-01")
                {
                    whereStr += "and fcreate_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

                string count = "10000";
                fstrSql = "select *," + count + " as total from c2c_db.t_refund_list " + whereStr + " Order by Fcreate_time DESC";
            }
            else  //客服系统未使用
            {
                fstrSql = "Select * from c2c_db.t_refund_list where flistid='" + f_strID + "'";
            }
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_REFUND GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            T_REFUND result = new T_REFUND();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    result.u_Bargain_Time = QueryInfo.GetDateTime(dr["fbargain_time"]);
                    result.u_Buy_Bank_Type = QueryInfo.GetInt(dr["fbuy_bank_type"]);
                    result.u_Buy_BankID = QueryInfo.GetString(dr["fbuy_bankid"]);
                    result.u_Buy_Name = QueryInfo.GetString(dr["fbuy_name"]);
                    result.u_BuyID = QueryInfo.GetString(dr["fbuyid"]);
                    result.u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                    result.u_Explain = QueryInfo.GetString(dr["fexplain"]);
                    result.u_IP = QueryInfo.GetString(dr["fip"]);
                    result.u_LState = QueryInfo.GetInt(dr["flstate"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_OK_Time = QueryInfo.GetDateTime(dr["fok_time"]);
                    result.u_PayBuy = QueryInfo.GetInt(dr["fpaybuy"]);
                    result.u_PaySale = QueryInfo.GetInt(dr["fpaysale"]);
                    result.u_PayType = QueryInfo.GetInt(dr["fpaytype"]);
                    result.u_Procedure = QueryInfo.GetInt(dr["fprocedure"]);
                    result.u_RListID = QueryInfo.GetInt(dr["frlistid"]);
                    result.u_Sale_Bank_Type = QueryInfo.GetInt(dr["fsale_bank_type"]);
                    result.u_Sale_BankID = QueryInfo.GetString(dr["fsale_bankid"]);
                    result.u_Sale_Name = QueryInfo.GetString(dr["fsale_name"]);
                    result.u_SaleID = QueryInfo.GetString(dr["fsaleid"]);
                    result.u_SPID = QueryInfo.GetString(dr["fspid"]);
                    result.u_State = QueryInfo.GetInt(dr["fsatate"]);

                    result.u_ListID = QueryInfo.GetString(dr["flistid"]);
                }
                else
                {
                    throw new Exception("没有查找到相应的记录");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion

    #region 实名认证处理类

    public class UserClassClass : Query_BaseForNET
    {

        public static bool HandleParameterX(DataSet ds)
        {
            try
            {
                ds.Tables[0].Columns.Add("Fcre_typeName", typeof(String));
                ds.Tables[0].Columns.Add("FpickstateName", typeof(String));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    string tmp = dr["Fcre_type"].ToString();
                    if (tmp == "1")
                    {
                        dr["Fcre_typeName"] = "身份证";
                    }
                    else if (tmp == "2")
                    {
                        dr["Fcre_typeName"] = "护照";
                    }
                    else if (tmp == "3")
                    {
                        dr["Fcre_typeName"] = "军官证";
                    }
                    else if (tmp == "100")
                    {
                        dr["Fcre_typeName"] = "对公鉴权";
                    }
                    else
                    {
                        dr["Fcre_typeName"] = "未定义";
                    }

                    tmp = dr["Fpickstate"].ToString();
                    if (tmp == "0")
                    {
                        dr["FpickstateName"] = "未处理";
                    }
                    else if (tmp == "1")
                    {
                        dr["FpickstateName"] = "已领单";
                    }
                    else if (tmp == "2")
                    {
                        dr["FpickstateName"] = "认证成功";
                    }
                    else if (tmp == "3")
                    {
                        dr["FpickstateName"] = "认证失败";
                    }
                    else
                    {
                        dr["FpickstateName"] = "未定义" + tmp;
                    }

                }
                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
        }

        private static bool SendAppealMail(string email, bool issucc, string param1, out string msg)
        {
            msg = "";

            if (PublicRes.IgnoreLimitCheck)
                return true;

            if (email == null || email.Trim() == "")
            {
                //furion 20060902 是否支持不发邮件取决于这里.要么返回真,要么返回假
                return true;
            }

            string filename = ConfigurationManager.AppSettings["ServicePath"].Trim();
            if (!filename.EndsWith("\\"))
                filename += "\\";

            string title = ""; if (issucc) filename += "UserClassYes.htm"; else filename += "UserClassNo.htm";

            StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("GB2312"));
            try
            {
                string content = sr.ReadToEnd();

                content = String.Format(content, param1);

                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                newMail.SendMail(email, "", title, content, true, null);

                return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                sr.Close();
            }
        }



        private static string getCgiString(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
            return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&");
        }

        #region 暂时注释
        //public static bool CommitAppeal(Finance_Header fh, string[] result, string dbstr, out string msg)
        //{
        //    msg = "";
        //    bool flag = true;

        //    MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
        //    try
        //    {
        //        if (result.Length == 0)
        //        {
        //            return true;
        //        }

        //        da.OpenConn();
        //        string strSql = "";
        //        foreach (string strresult in result)
        //        {
        //            //成功，失败，都需要发邮件，异常，转后台时不用发邮件。
        //            //另外，失败时发邮件失败
        //            string[] split = strresult.Split(';');
        //            if (split.Length != 3)
        //                continue;

        //            long id = long.Parse(split[0].Trim());
        //            int index = Int32.Parse(split[1].Trim());
        //            string idcard = split[2].Trim().ToUpper();

        //            //if(index == 0 && idcard.Length != 5)
        //            if (index == -1 && idcard.Length != 5)
        //                continue;

        //            //新加判断
        //            if (idcard.Length == 5)
        //                index = 0;
        //            else
        //                index += 1;

        //            int emailflag = 0; //0不用发邮件，1发成功邮件，2发失败邮件。
        //            string memo = "";

        //            strSql = " select Fuid from authen_process_db.t_authening_info where Flist_id=" + id;
        //            string fuin = da.GetOneResult(strSql);

        //            if (fuin == null || fuin.Trim() == "")
        //            {
        //                msg += "记录{" + id + "}的帐号读取有错;";
        //                flag = false;
        //                continue;
        //            }

        //            //三条分支，0判断后五位是否一样，1直接拒绝，2转后台处理。
        //            if (index == 1)
        //            {
        //                strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='提交证件不合格。',Fpickstate=3"
        //                    + " where Flist_id=" + id + " and Fpickstate=1";
        //                emailflag = 2;
        //                memo = "提交证件不合格。";
        //            }
        //            else if (index == 0)
        //            {                
        //                string Msg = "";
        //                strSql = "uid=" + fuin;
        //                string allidcard = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcreid", out Msg);

        //                if (allidcard == null || allidcard.Trim() == "")
        //                {
        //                    msg += "记录{" + id + "}的帐号读取有错;" + Msg;
        //                    flag = false;
        //                    continue;
        //                }

        //                if (allidcard.EndsWith(idcard))
        //                {

        //                    //这里才会成功
        //                    strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='证件合格。',Fpickstate=2"
        //                        + " where Flist_id=" + id + " and Fpickstate=1";


        //                    emailflag = 1;
        //                    memo = "证件合格。";

        //                }
        //                else //furion 20071106 新加需求，如果是输入帐号有错这种，变成异常转后台。
        //                {
        //                    //失败。
        //                    strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='提交证件不合格。"
        //                        + idcard + "',Fpickstate=3"
        //                        + " where Flist_id=" + id + " and Fpickstate=1";

        //                    emailflag = 2;

        //                }//if allidcard
        //            }//if index		

        //            //调用接口,如果失败跳过,不发邮件.
        //            string strInfoSql = "select Fqqid,Fpath,Fstandby3 from authen_process_db.t_authening_info where Flist_id=" + id;
        //            DataTable dtinfo = da.GetTable(strInfoSql);

        //            if (dtinfo == null || dtinfo.Rows.Count != 1)
        //            {
        //                msg += "记录{" + id + "}的信息读取有错;";
        //                flag = false;
        //                continue;
        //            }

        //            if (dtinfo.Rows[0]["Fstandby3"].ToString() != "")  //走mandy的新流程   opr_state：0未定义，1确认，2驳回
        //            {
        //                string inmsg = "uid=" + fuin;
        //                inmsg += "&opr_state=" + emailflag;
        //                inmsg += "&memo=" + memo;
        //                inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
        //                inmsg += "&operator=" + fh.UserName;

        //                string reply;
        //                short sresult;

        //                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_cre_check_service", inmsg, true, out reply, out sresult, out msg))
        //                {
        //                    if (sresult != 0)
        //                    {
        //                        msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
        //                        flag = false;
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        if (reply.StartsWith("result=0"))
        //                        {

        //                        }
        //                        else
        //                        {
        //                            msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
        //                            flag = false;
        //                            continue;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
        //                    flag = false;
        //                    continue;
        //                }
        //            }
        //            else   //走老流程
        //            {
        //                string inmsg = "uin=" + dtinfo.Rows[0]["Fqqid"].ToString().Trim();
        //                inmsg += "&opr_state=" + emailflag;
        //                inmsg += "&opr_type=1";
        //                inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
        //                inmsg += "&operator=" + fh.UserName;

        //                string reply;
        //                short sresult;

        //                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_creinfo_service", inmsg, true, out reply, out sresult, out msg))
        //                {
        //                    if (sresult != 0)
        //                    {
        //                        msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
        //                        flag = false;
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        if (reply.StartsWith("result=0"))
        //                        {

        //                        }
        //                        else
        //                        {
        //                            msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
        //                            flag = false;
        //                            continue;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
        //                    flag = false;
        //                    continue;
        //                }
        //            }

        //            int iresult = da.ExecSqlNum(strSql);
        //            if (iresult != 1)
        //            {
        //                msg += "更新记录{" + id + "}时未成功;";
        //                flag = false;
        //                continue;
        //            }

        //            //发送邮件根据成功或失败，发送邮件，如果发送失败，就转后台处理。
        //            //if(emailflag > 0)
        //            if (emailflag < 0) //不再发邮件
        //            {
        //                //取得此审批的各需要信息.
        //                strSql = " select Fuid,Ftruename from authen_process_db.t_authening_info where Flist_id=" + id;
        //                DataSet ds = da.dsGetTotalData(strSql);

        //                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
        //                {
        //                    string username = ds.Tables[0].Rows[0]["Ftruename"].ToString();
        //                    //查询出来email

        //                    string Msg = "";
        //                    strSql = "uid=" + fuin;
        //                    string email = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Femail", out Msg);

        //                    if (email == null || email.Trim() == "")
        //                        continue;

        //                    bool resultfalg = false;
        //                    if (emailflag == 1)
        //                        resultfalg = true;

        //                    string tmpmsg = "";

        //                    if (!SendAppealMail(email, resultfalg, username, out tmpmsg))
        //                    {
        //                        msg += "发送邮件失败：" + tmpmsg;
        //                        flag = false;
        //                        continue;
        //                    }
        //                }
        //            } 

        //        }

        //        return true;

        //    }
        //    finally
        //    {
        //        da.Dispose();
        //    }
        //}
        #endregion

        public static void InputUserClasslNumber(string User, string Type, string OperationType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                string sql = "select count(1) from db_appeal.t_tenpay_appeal_kf_total where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                if (da.GetOneResult(sql) == "1")
                {
                    if (Type == "Success")
                        sql = "update db_appeal.t_tenpay_appeal_kf_total set UserClassSuccessNum = UserClassSuccessNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                    else if (Type == "Fail")
                        sql = "update db_appeal.t_tenpay_appeal_kf_total set UserClassFailNum = UserClassFailNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                    else if (Type == "Other")
                        sql = "update db_appeal.t_tenpay_appeal_kf_total set UserClassOtherNum = UserClassOtherNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                    else
                    {
                        throw new Exception("没有这种类型!");
                    }
                }
                else
                {
                    if (Type == "Success")
                        sql = "insert into db_appeal.t_tenpay_appeal_kf_total(User,OperationDay,UserClassSuccessNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Fail")
                        sql = "insert into db_appeal.t_tenpay_appeal_kf_total(User,OperationDay,UserClassFailNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Other")
                        sql = "insert into db_appeal.t_tenpay_appeal_kf_total(User,OperationDay,UserClassOtherNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else
                    {
                        throw new Exception("没有这种类型!");
                    }
                }

                da.ExecSqlNum(sql);
            }
            catch
            {
                throw new Exception("记录处理统计失败！");
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool UserClassConfirm(int flist_id, string dbstr, string user, out string msg)
        {
            msg = "";
            int emailflag = 1; //0不用发邮件，1发成功邮件，2发失败邮件。

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
            try
            {
                // 2012/4/18 改动sql，允许对认证失败的记录进行二次实名认证。
                da.OpenConn();

                string strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='证件合格。',Fpickstate=2"
                    + " where Flist_id=" + flist_id.ToString() + " and (Fpickstate=0 or Fpickstate=1 or Fpickstate=3)";

                //调用接口,如果失败跳过,不发邮件.
                string strInfoSql = "select Fuid,Fqqid,Fpath,Fstandby3 from authen_process_db.t_authening_info where Flist_id=" + flist_id.ToString();
                DataTable dtinfo = da.GetTable(strInfoSql);

                if (dtinfo == null || dtinfo.Rows.Count != 1)
                {
                    msg += "记录{" + flist_id.ToString() + "}的信息读取有错;";
                    return false;
                }

                if (dtinfo.Rows[0]["Fstandby3"].ToString() != "")  //走mandy的新流程   opr_state：0未定义，1确认，2驳回
                {
                    string inmsg = "uid=" + dtinfo.Rows[0]["Fuid"].ToString();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&memo=证件合格。";
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_cre_check_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }
                else
                {
                    string inmsg = "uin=" + dtinfo.Rows[0]["Fqqid"].ToString().Trim();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&opr_type=1";
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_creinfo_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                InputUserClasslNumber(user, "Other", "appeal");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }

                int iresult = da.ExecSqlNum(strSql);
                if (iresult != 1)
                {
                    msg += "更新记录{" + flist_id.ToString() + "}时未成功;";
                    return false;
                }

                InputUserClasslNumber(user, "Success", "appeal");
                return true;
            }
            finally
            {
                da.Dispose();
            }
        }


        public static bool UserClassCancel(int flist_id, string reason, string OtherReason, string dbstr, string user, out string msg)
        {
            msg = "";
            int emailflag = 2; //0不用发邮件，1发成功邮件，2发失败邮件。

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
            try
            {
                da.OpenConn();
                string strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='" + reason + OtherReason + "',Fpickstate=3"
                    + " where Flist_id=" + flist_id.ToString() + " and (Fpickstate=0 or Fpickstate=1)";

                //调用接口,如果失败跳过,不发邮件.
                string strInfoSql = "select Fuid,Fqqid,Fpath,Fstandby3 from authen_process_db.t_authening_info where Flist_id=" + flist_id.ToString();
                DataTable dtinfo = da.GetTable(strInfoSql);

                if (dtinfo == null || dtinfo.Rows.Count != 1)
                {
                    msg += "记录{" + flist_id.ToString() + "}的信息读取有错;";
                    return false;
                }

                if (dtinfo.Rows[0]["Fstandby3"].ToString() != "")  //走mandy的新流程   opr_state：0未定义，1确认，2驳回
                {
                    string inmsg = "uid=" + dtinfo.Rows[0]["Fuid"].ToString();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&memo=" + reason + OtherReason;
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_cre_check_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }
                else
                {
                    string inmsg = "uin=" + dtinfo.Rows[0]["Fqqid"].ToString().Trim();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&opr_type=1";
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_creinfo_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg;
                        return false;
                    }
                }

                int iresult = da.ExecSqlNum(strSql);
                if (iresult != 1)
                {
                    msg += "更新记录{" + flist_id.ToString() + "}时未成功;";
                    return false;
                }

                InputUserClasslNumber(user, "Fail", "appeal");

                return true;
            }
            finally
            {
                da.Dispose();
            }
        }

        public UserClassClass(string u_BeginTime, string u_EndTime, string fuin, int fstate, string QQType, int SortType)
        {

            string strWhere = " where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 ";

            if (fuin != null && fuin != "")
            {
                strWhere += " and Fqqid = '" + fuin + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {
                strWhere += " and Fmodify_time between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                strWhere += " and Fpickstate=" + fstate + "  ";
            }

            //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            //用户等级及vip标识(Fstandby1)
            //非cft会员及等级： 0-6
            //cft会员及等级： 100-106
            //vip会员及等级： 200-206
            //连续1个月不做任务的普通会员（同非会员）：400-406
            //0是默认值
            if (QQType == "0")
            {
                strWhere += " and (Fstandby1 < 100 or Fstandby1 >= 300) ";
            }
            else if (QQType == "1")
            {
                strWhere += " and Fstandby1>=100 and Fstandby1<200 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and Fstandby1>=200 and Fstandby1<300 ";
            }

            if (SortType != 99)
            {
                if (SortType == 0)   //排序：时间小到大
                    strWhere += " order by Fcreate_time asc ";
                if (SortType == 1)   //排序：时间大到小
                    strWhere += " order by Fcreate_time desc ";
            }

            fstrSql = "select Fpickstate,Fpickuser,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Flist_id,Fcreate_time from authen_process_db.t_authening_info "
                + strWhere;
            fstrSql_count = "select count(1) from authen_process_db.t_authening_info " + strWhere;
        }

        public UserClassClass(string fuin, string Flag)
        {
            fstrSql = "select Fpickstate,Fpickuser,Fcard_stat,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Flist_id,Fcreate_time,Fcre_stat from authen_process_db.t_authening_info where Fqqid = '" + fuin + "' and Fauthen_type=1 " +
                "order by Fmodify_time ";
            fstrSql_count = "select count(1) from authen_process_db.t_authening_info where Fqqid = '" + fuin + "' and Fauthen_type=1 ";
        }

        public UserClassClass(int flist_id)
        {
            fstrSql = "select Fpickstate,Fpickuser,Fcard_stat,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Flist_id,Fcreate_time from authen_process_db.t_authening_info where flist_id=" + flist_id.ToString();
            fstrSql_count = "select count(1) from authen_process_db.t_authening_info where flist_id=" + flist_id.ToString();
        }

        public static DataSet GetLockList(DateTime BeginDate, DateTime EndDate, int fstate, string username, int Count)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("RU"));
            da.OpenConn();

            DataSet ds = new DataSet();

            try
            {
                string strWhere = "where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 " +
                    "and Fmodify_time between '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "' ";

                if (fstate == 99)
                    fstate = 0;

                if (fstate != 0 && fstate != 1)
                {
                    throw new Exception("改状态不允许批量领单!");
                }
                strWhere += " and Fpickstate=" + fstate + "  ";

                if (username == null || username == "")
                {
                    throw new Exception("批量领单人不允许为空!");
                }

                string strSql = "select Fuid,Flist_id,Fpickstate,Fpickuser,Fcard_stat,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Fcreate_time from authen_process_db.t_authening_info "
                    + strWhere + "  order by Fmodify_time limit " + Count;

                ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string WhereStr = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WhereStr += "'" + dr["Flist_id"].ToString().Trim() + "',";
                    }
                    WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

                    strSql = " update authen_process_db.t_authening_info set FPickUser='" + username + "',FPickTime=now(),Fpickstate=1 where Flist_id in(" + WhereStr + ")";

                    da.ExecSql(strSql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

            return ds;

        }



        public static DataSet GetDeleteList(string Fqqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("RU"));
            da.OpenConn();

            try
            {
                string strSql = "select Fauthen_operator,Fqqid,Fmemo,Fmodify_time from authen_process_db.t_authening_info " +
                    "where Fqqid='" + Fqqid + "' and Fmemo='客服删除' order by Fmodify_time desc limit 0,100";

                return da.dsGetTotalData(strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

        }

    }

    #endregion

    #region 用户商家工具按钮表查询处理

    /// <summary>
    /// 用户商家工具按钮表的查询类处理
    /// </summary>
    public class Q_BUTTONINFO : Query_BaseForNET
    {
        private string f_strID;
        public Q_BUTTONINFO(string strID, int istr, int imax)
        {
            f_strID = strID;

            string tname = PublicRes.GetTableName("t_button_info", f_strID);
            string sWhereStr = " where " + PublicRes.GetSqlFromQQ(strID, "fowner_uin");
            string count = PublicRes.ExecuteOne("select count(*) from " + tname + sWhereStr, "ZJB");

            fstrSql = "select *," + count + " as total from " + tname
                + sWhereStr + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
        }
    }

    #endregion

    #region 用户账户表查询处理

    /// <summary>
    /// 用户帐户表的查询类处理
    /// </summary>
    public class Q_USER : Query_BaseForNET
    {
        private string f_strID;
        public string FlagForTable;
        public Q_USER(string fuid, int fcurtype)
        {

            if (fuid == null)
                fuid = "0";

            // TODO: 1客户信息资料外移
            //先把email和mobile从t_user_info中取出,再放入此SQL中.
            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
            string femail = "";
            string fmobile = "";
            string fatt_id = "";
            string ftrueName = "";
            string fz_amt = ""; //分账冻结金额 

            //ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            ICEAccess ice = ICEAccessFactory.GetICEAccess("ICEConnectionString");
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                string errMsg = "";
                string strSql = "uid=" + fuid;
                fatt_id = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERATT, "Fatt_id", out errMsg);

                fatt_id = QueryInfo.GetString(fatt_id);

                DataTable dt_userInfo = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);
                if (dt_userInfo != null && dt_userInfo.Rows.Count == 1)
                {
                    femail = dt_userInfo.Rows[0]["Femail"].ToString();
                    fmobile = dt_userInfo.Rows[0]["Fmobile"].ToString();
                    ftrueName = dt_userInfo.Rows[0]["FtrueName"].ToString();

                    string fusertype = QueryInfo.GetString(dt_userInfo.Rows[0]["Fuser_type"]);
                    if (fusertype == "2")//公司类型
                    {
                        ftrueName = dt_userInfo.Rows[0]["Fcompany_name"].ToString();
                    }
                }

                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=" + fcurtype + "&");

                string strResp = "";
                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);
                if (dt == null || dt.Rows.Count == 0)
                    throw new LogicException("调用ICE查询T_user无记录" + strResp);

                ice.CloseConn();

                da.OpenConn();
                string sql = "select * from app_platform.t_account_freeze where Fuid = '" + fuid + "'";
                DataTable dt2 = da.GetTable(sql);
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    fz_amt = dt2.Rows[0]["Famount"].ToString();
                }

                //用dt里的一条记录组合出select语句。
                string strtmp = " select ";
                foreach (DataColumn dc in dt.Columns)
                {
                    string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                    strtmp += " '" + valuetmp + "' as " + dc.ColumnName + ",";
                }

                fstrSql = strtmp + "'" + femail + "' as Femail,'" + fmobile + "' as Fmobile, '" + fatt_id + "' as Att_id, '" + ftrueName + "' as UserRealName2, '" + fz_amt + "' as Ffz_amt ";
            }
            finally
            {
                ice.Dispose();
                da.Dispose();
            }
        }

        //为子帐户专用
        public Q_USER(string strID, string Fcurtype)
        {
            f_strID = strID;

            string fuid = PublicRes.ConvertToFuid(strID);
            if (fuid == null)
                fuid = "0";

            // TODO: 1客户信息资料外移

            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));

            //ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            ICEAccess ice = ICEAccessFactory.GetICEAccess("ICEConnectionString");
            string femail = "";
            string fmobile = "";
            try
            {
                string errMsg = "";
                string strSql = "uid=" + fuid;
                femail = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Femail", out errMsg);
                fmobile = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fmobile", out errMsg);

                femail = QueryInfo.GetString(femail);
                fmobile = QueryInfo.GetString(fmobile);
            }
            finally
            {
                //da_zl.Dispose();
            }

            ice.OpenConn();
            string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
            strwhere += ICEAccess.URLEncode("fcurtype=" + Fcurtype + "&");

            string strResp = "";
            DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);
            if (dt == null || dt.Rows.Count == 0)
                throw new LogicException("调用ICE查询T_user无记录" + strResp);

            ice.CloseConn();

            //用dt里的一条记录组合出select语句。
            string strtmp = " select ";
            foreach (DataColumn dc in dt.Columns)
            {
                string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                strtmp += " '" + valuetmp + "' as " + dc.ColumnName + ",";
            }

            fstrSql = strtmp + "'" + femail + "' as Femail,'" + fmobile + "' as Fmobile ";

        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_USER GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            T_USER result = new T_USER();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.u_APay = QueryInfo.GetInt(dr["fapay"]);
                    result.u_Balance = QueryInfo.GetInt(dr["fBalance"]);
                    result.u_Con = QueryInfo.GetInt(dr["fcon"]);
                    result.u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                    result.u_Fetch_Time = QueryInfo.GetDateTime(dr["ffetch_time"]);
                    result.u_Login_IP = QueryInfo.GetString(dr["flogin_ip"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_Modify_Time_C2C = QueryInfo.GetDateTime(dr["fmodify_time_c2c"]);
                    result.u_Quota = QueryInfo.GetInt(dr["fquota"]);
                    result.u_Quota_Pay = QueryInfo.GetInt(dr["fquota_pay"]);
                    result.u_Save_Time = QueryInfo.GetDateTime(dr["fsave_time"]);
                    result.u_State = QueryInfo.GetInt(dr["fstate"]);
                    result.u_TrueName = QueryInfo.GetString(dr["ftruename"]);
                    result.u_Yday_Balance = QueryInfo.GetInt(dr["fyday_balance"]);
                    result.u_User_Type = QueryInfo.GetInt(dr["fuser_type"]);

                    result.u_QQID = QueryInfo.GetString(dr["fqqid"]);
                }
                else
                {
                    throw new Exception("没有查找到相应的记录");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion

}
