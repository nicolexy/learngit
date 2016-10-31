using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.TransferMeaning;
using CFT.CSOMS.COMMLIB;
using SunLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace TENCENT.OSS.CFT.KF.KF_Web
{
    public partial class KFWebTest : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

    private  string ComputeSha256(string str)
    {
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            HMACSHA256 sha2561 = new HMACSHA256();

            byte[] sha256Byte1 = sha2561.ComputeHash(bytes);

            var sb1 = new StringBuilder();
            foreach (byte b1 in sha256Byte1)
            {
                sb1.AppendFormat("{0:x2}", b1);
            }
             sb1.ToString();

            using (SHA256Managed sha256 = new SHA256Managed())
            {
                byte[] sha256Byte = sha256.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (byte b in sha256Byte)
                {
                    sb.AppendFormat("{0:x2}", b);
                }
                return sb.ToString();
            }

            
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //var timestamp = "1477446576";
            //var xRioSeq = "220c0e0a:0157feada5e2:020381";
            //var staffId = "79723";
            //var staffName = "v_swuzhang";
            //var xExtData = string.Empty;
            //var signature = "727BA923DA0763370F4A1EB5A005EC79A63E4AD631AD39E79DB1B2BCDA6F0A51";
            //var tokenId = "v20vsc036sujn68s0eds7jm0913bs51v";

            //DateTimeOffset expiresAtOffset = DateTimeOffset.Now;
            //var localTimeStamp = expiresAtOffset.ToUniversalTime().UtcDateTime;
            //var localSignStr = string.Format("{0}{1}{2},{3},{4},{5}{0}", timestamp, tokenId, xRioSeq, staffId, staffName, xExtData);
            //var localSignature = ComputeSha256(localSignStr);

            LogHelper.LogInfo(" public partial class KFWebTest : TENCENT.OSS.CFT.KF.KF_Web.PageBase  ");
            if (Request["wechatname"] != null)
            {
                LogHelper.LogInfo(" KFWebTest.aspx  request key ：" + Request["wechatname"].ToString());


                LogHelper.LogInfo(" KFWebTest.aspx  wechatname ：" + Request["wechatname"].ToString());
                WeChatInfo(Request["wechatname"].ToString());
            }

            if (Request["dbkey"] != null)
            {
                LogHelper.LogInfo(" KFWebTest.aspx  request key ：" + Request["dbkey"].ToString());

                if (Request["gofunc"] != null && Request["gofunc"].ToLower() == "checkdbconn")
                {
                    LogHelper.LogInfo(" KFWebTest.aspx  dbkey ：" + Request["dbkey"].ToString());
                    CheckDBConn(Request["dbkey"].ToString());
                }
                else
                {
                    LogHelper.LogInfo(" KFWebTest.aspx  dbkey ：" + Request["dbkey"].ToString());
                    GetDBConnStr(Request["dbkey"].ToString());
                }
            }

            if (Request["reqtype"] != null)
            {
                var reqtype = Request["reqtype"].ToString().ToLower();
                LogHelper.LogInfo(" KFWebTest.aspx  request reqtype ：" + reqtype);

                switch (reqtype)
                {
                    case "sendmail":
                        var actiontype = Request["actiontype"] != null ? Request["actiontype"].ToString() : "11914";
                        LogHelper.LogInfo(" KFWebTest.aspx  request actiontype ：" + actiontype);
                        SendEmail(actiontype);
                        break;
                    case "updateuserinfoattr":
                        var qqid = Request["qqid"] != null ? Request["qqid"].ToString() : "380885873";
                        LogHelper.LogInfo(" KFWebTest.aspx  request qqid ：" + qqid);
                        UpdateUserInfoAttr(qqid);
                        break;
                    case "bow":
                        var listid = Request["listid"] != null ? Request["listid"].ToString() : "1259051901321610225267518331";
                        LogHelper.LogInfo(" KFWebTest.aspx  request listid ：" + listid);
                        BindList(listid);
                        break;
                    case "bowold":
                        var olistid = Request["listid"] != null ? Request["listid"].ToString() : "1259051901321610225267518331";
                        LogHelper.LogInfo(" KFWebTest.aspx  request listid ：" + olistid);
                        if (Request["ser"] != null)
                        {
                            BindOldList(olistid, true);
                        }
                        else
                        {
                            BindOldList(olistid);
                        }
                        break;
                    case "tdetoid":
                        var tdeid = Request["listid"] != null ? Request["listid"].ToString() : "1259051901321610225267518331";
                        LogHelper.LogInfo(" KFWebTest.aspx  request listid ：" + tdeid);
                        TdeToID(tdeid);
                        break;
                    case "rechargebind":
                        var rechargeid = Request["rechargeid"] != null ? Request["rechargeid"].ToString() : "110321610276540307000";
                        var banklistid = Request["banklistid"] != null ? Request["banklistid"].ToString() : "201609285461406";
                        var banktype = Request["banktype"] != null ? Request["banktype"].ToString() : "0";
                        var curtype = Request["curtype"] != null ? Request["curtype"].ToString() : "0";
                        LogHelper.LogInfo(" KFWebTest.aspx  request rechargebind ：" + rechargeid);

                        RechargeBindData(rechargeid, banklistid, int.Parse(banktype), int.Parse(curtype));
                        break;
                    case "rechargebindold":
                        var rechargeido = Request["rechargeid"] != null ? Request["rechargeid"].ToString() : "110321610276540307000";
                        var banklistido = Request["banklistid"] != null ? Request["banklistid"].ToString() : "201609285461406";
                        var banktypeo = Request["banktype"] != null ? Request["banktype"].ToString() : "0";
                        var curtypeo = Request["curtype"] != null ? Request["curtype"].ToString() : "0";

                        //queryType= "czd", DateTime? startDate=null, DateTime? endDate = null
                        var queryType = Request["querytype"] != null ? Request["querytype"].ToString() : "czd";
                        var startDate = Request["startdate"] != null ? DateTime.Parse(Request["startdate"].ToString()) : DateTime.Now.AddDays(-3);
                        var endDate = Request["enddate"] != null ? DateTime.Parse(Request["enddate"].ToString()): DateTime.Now;
                        LogHelper.LogInfo(" KFWebTest.aspx  request rechargebindold ：" + rechargeido);

                        RechargeBindDataOld(rechargeido, banklistido, banktypeo, int.Parse(curtypeo),queryType,startDate,endDate);
                        break;
                }
            }
        }

        /// <summary>
        /// 充值单
        /// </summary>
        /// <param name="recharge"></param>
        private void RechargeBindData(string rechargeid,string banklistid="",int banktype=0,int curtype=0)
        {
            try
            {
                RechargeService recharge = new RechargeService();
                var rechargeData = recharge.GetRechargeTable(rechargeid, banklistid, banktype, curtype);

                this.GridView1.DataSource = rechargeData;
                this.GridView1.DataBind();
            }
            catch (Exception ef) {
                var strVal = string.Format(" TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TradeLogQuery  调用新接口 Apollo.Bow ：new RechargeService().GetRechargeTable(),rechargeid={0},rechargeid={1},rechargeid={2},rechargeid={3},new PickService().TdeToID,异常：{4}",rechargeid,banklistid,banktype,curtype,ef.ToString());
                this.Label1.Text = strVal;

                LogHelper.LogError(strVal);
            }
        }

        private void RechargeBindDataOld(string rechargeid, string banklistid = "", string banktype = "", int curtype = 0,string queryType= "czd", DateTime? startDate=null, DateTime? endDate = null)
        {
            /*
             * queryType:
             * qq 按帐号
             czd 充值单号
             toBank 给银行的订单号
             BankBack 银行返回订单号
             * 
             */

            try
            {
                var rechargeData = new TradeService().GetBankRollListByListId(rechargeid, queryType, curtype, startDate.Value,endDate.Value , 0, 0, 20000000, banktype, 0, 20);

                this.GridView1.DataSource = rechargeData;
                this.GridView1.DataBind();
            }
            catch (Exception ef)
            {
                var strVal = string.Format(" TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TradeLogQuery  调用新接口 Apollo.Bow ：new RechargeService().GetRechargeTable(),rechargeid={0},rechargeid={1},rechargeid={2},rechargeid={3},new PickService().TdeToID,异常：{4}", rechargeid, banklistid, banktype, curtype, ef.ToString());
                this.Label1.Text = strVal;

                LogHelper.LogError(strVal);
            }
        }

        private string TdeToID(string listID)
        {
            LogHelper.LogInfo(string.Format(" TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TradeLogQuery  调用新接口 Apollo.Bow ：new TradeService().GetTradeModelById(),listid={0}", listID));
            string listid = string.Empty;
            try
            {
                var tradeModel = new TradeService().GetTradeModelById(listID);

                this.Label1.Text = "TdeToID===ListID:" + tradeModel.ListID + ",BankBackID:" + tradeModel.BankBackID + ",BankListID:" + tradeModel.BankListID + ",SPID:" + tradeModel.SPID + "---===---Old_TdeToID:" + new PickService().TdeToID(listID);
            }
            catch (Exception ef)
            {
                var strVal = string.Format(" TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TradeLogQuery  调用新接口 Apollo.Bow ：new TradeService().GetTradeModelById(),listid={0},new PickService().TdeToID,异常：{1}", listID, ef.ToString());
                this.Label1.Text = strVal;

                LogHelper.LogError(strVal);
            }
            return listid;
            
            // return new PickService().TdeToID(tdeid);
        }

        public void BindList(string listid) {

            LogHelper.LogInfo(string.Format(" TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TradeLogQuery  调用新接口 Apollo.Bow ：new TradeService().GetTradeDataById(),listid={0}", listid));

           
               DataSet ds = new TradeService().GetTradeDataById(listid);

                this.GridView1.DataSource = ds.Tables[0];
                this.GridView1.DataBind();
           
        }

        public void BindOldList(string listid,bool? services=false) {
            DataSet ds = new DataSet();

            if (services.Value)
            {  //绑定交易资料信息
                Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);


                DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());

                DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());

                int istr = 1;

                int imax = 2;

                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                ds = myService.GetPayList(listid, 4, beginTime, endTime, istr, imax);
            }
            else {
                ds = new TradeService().GetPayByListid(listid);  //绑定交易资料信息
            }
            this.GridView1.DataSource = ds.Tables[0];
            this.GridView1.DataBind();
        }

        private void GetDBConnStr(string strkey)
        {
            string dbstr = CommLib.DbConnectionString.Instance.GetConnectionString(strkey.Trim().ToUpper());

            LogHelper.LogInfo(" test.aspx  private void GetDBConnStr  strKey：" + strkey + ",dbstr:" + dbstr);

            string qq = "404968099";
            if (Request["qq"] != null)
            {
                qq = Request["qq"].ToString();
            }
            this.Label1.Text = dbstr;

            bindshimingrzData(qq);

            Response.Write(dbstr);

        }


        #region  企业C财付通帐号注销通知

        private void SendEmail(string actiontype)
        {
            try
            {
                CommMailSend.SendMsg("yukini@tencent.com", actiontype, "Fqqid=380885873", 2);
                LogHelper.LogInfo(" KFWebTest.aspx  SendEmail ok");
            }
            catch (Exception ep)
            {
                LogHelper.LogError(" KFWebTest.aspx  SendEmail ：" + ep);
            }
            finally
            {
                try
                {
                    CommMailSend.SendMsg("yukini@tencent.com", actiontype, "Fqqid=yukini@tencent.com", 1);
                    LogHelper.LogInfo(" KFWebTest.aspx  SendEmail ok");
                }
                catch (Exception ep2)
                {
                    LogHelper.LogError(" KFWebTest.aspx  SendEmail2 ：" + ep2);
                }
            }
        }


        public static string ConvertToFuid(string QQID)
        {
            try
            {
                //furion 20061115 email登录相关
                if (QQID == null || QQID.Trim().Length < 3)
                    return null;

                //start
                string qqid = QQID.Trim();


                string errMsg = "";
                string strSql = "uin=" + qqid;
                string struid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "fuid", out errMsg);

                if (struid == null)
                    return null;
                else
                    return struid;

            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("KF_Service PublicRes").Info("ConvertToFuid error:" + ex);
                return null;
            }
        }




        /// <summary>
        /// 获得数据库的时间 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string strNowTimeStander
        {
            get
            {
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB-V30"));
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZW"));
                try
                {
                    da.OpenConn();
                    return da.GetOneResult("select DATE_FORMAT(now(),'%Y-%m-%d %H:%i:%s')");
                }
                catch
                {
                    return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                finally
                {
                    da.Dispose();
                }
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            //var maxWorkThread = 0;
            //var maxThread = 0;
            //System.Threading.ThreadPool.GetMaxThreads(out maxWorkThread, out maxThread);


            if (!fileqqid.HasFile)
            {
                LogHelper.LogInfo(" KFWebTest.aspx  请上传文件");

                throw new ArgumentNullException("请上传文件");

            }

            if (System.IO.Path.GetExtension(fileqqid.FileName).ToLower() == ".txt")
            {
                string filePath = Server.MapPath("~/") + "PLFile//";
                string path = filePath + System.IO.Path.GetFileName(fileqqid.FileName);

                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                fileqqid.PostedFile.SaveAs(path);
                LogHelper.LogInfo(" KFWebTest.aspx  保存文件路径  path=" + path);

                var sourcefileContent = System.IO.File.ReadAllLines(path);

                #region old code
                //Dictionary<int,int> indexList = new Dictionary<int,int>();

                //int maxRows = 1000;
                //if (sourcefileContent != null && sourcefileContent.Length > maxRows)
                //{
                //    int plusNum = sourcefileContent.Length / maxRows;
                //    int plusNum2 = sourcefileContent.Length % maxRows;
                //    int baseAddNum = maxRows;

                //    for (int i = 0; i < plusNum; i++)
                //    {
                //        indexList.Add(baseAddNum - maxRows, baseAddNum);

                //        baseAddNum += maxRows;

                //        if (plusNum2 > 0 && baseAddNum >= sourcefileContent.Length)
                //        {
                //            indexList.Add(indexList[baseAddNum - 2*maxRows], sourcefileContent.Length);
                //        }
                //    }
                //}
                //else
                //{
                //    indexList.Add(0, sourcefileContent.Length);
                //}

                //foreach (var qqlist in indexList)
                //{
                //    System.Threading.Thread td = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(delegate(object qqidfile)
                //    {
                //        var currFileContent = (KeyValuePair<int, int>)qqidfile;
                //        for (int qqIndex = currFileContent.Key; qqIndex < currFileContent.Value; qqIndex++)
                //         {
                //             var qqid = sourcefileContent[qqIndex];
                //            //if (UpdateUserInfoAttr(qqid))
                //            //{
                //            //    LogHelper.LogInfo(string.Format(" KFWebTest.aspx  ----------Button1_Click--UpdateUserInfoAttr---修改成功--1------qqid={0}---------{1}--{2}-", qqid, currFileContent.Key, currFileContent.Value));
                //            //}
                //            //else
                //            //{
                //                LogHelper.LogInfo(string.Format(" KFWebTest.aspx  ----------Button1_Click--UpdateUserInfoAttr---修改失败--0------qqid={0}---------{1}--{2}-", qqid, currFileContent.Key, currFileContent.Value));
                //            //}

                //            System.Threading.Thread.Sleep(2);
                //        }
                //    }));

                //    td.Start(qqlist);
                //}

                #endregion


                if (sourcefileContent != null && sourcefileContent.Length > 0)
                {
                    System.Threading.Thread td = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(delegate
                      {
                          System.Threading.Tasks.Parallel.ForEach(sourcefileContent, delegate(string qqid)
                              {
                                  if (UpdateUserInfoAttr(qqid))
                                  {
                                      //LogHelper.LogInfo(string.Format(" KFWebTest.aspx  ----------Button1_Click--UpdateUserInfoAttr---修改成功--1------qqid={0}----------", qqid));
                                  }
                                  else
                                  {
                                      LogHelper.LogInfo(string.Format(" KFWebTest.aspx  ----------Button1_Click--UpdateUserInfoAttr---修改失败--0------qqid={0}----------", qqid));
                                  }

                              });
                      }));

                    td.Start();
                }

                Response.Write("KFWebTest.aspx  正在执行…………");
            }
            else
            {

                LogHelper.LogInfo(" KFWebTest.aspx  文件格式不合法。非txt");

                Response.Write("KFWebTest.aspx  文件格式不合法。非txt");
            }


            Response.End();
        }




        private bool UpdateUserInfoAttr(string qqid)
        {
            int accountType = 0;
            
            DataSet ds =null;
            try
            {
                ds = new AccountService().GetUserInfo(qqid, accountType, 1, 1);
            }
            catch (Exception ep) {
                //LogHelper.LogInfo(string.Format(" KFWebTest.aspx  ------------UpdateUserInfoAttr1-----0------qqid={0}-----{1}-", qqid, ep.Message));
                LogHelper.LogError(string.Format(" KFWebTest.aspx ------------UpdateUserInfoAttr2-----0------qqid={0}-----{1}-", qqid, ep.ToString()));
                LogHelper.LogInfo(string.Format("==========,{0},{1},{2},{3}", qqid, "", "", ""));
                return false;
            }

            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                LogHelper.LogError(" KFWebTest.aspx new AccountService().GetUserInfo  qqid=" + qqid + "未获取到数据");

                return false;
            }

            string atttype = "108";
            string oldatttype = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fatt_id"]);
            string fcre_type = ds.Tables[0].Rows[0]["Fcre_type"].ToString();

            //获得用户帐户信息 
            string userType = "";
            string userType_str = "";
            string Msg = "";
            bool exeSign = false;

            try
            {
                exeSign = new AccountService().GetUserType(qqid, accountType, out userType, out userType_str, out Msg);
            }
            catch(Exception ef)
            {
                LogHelper.LogInfo(" KFWebTest.aspx new AccountService().GetUserType   qqid=" + qqid + "获取帐号类型失败," + ef);
                userType = string.Empty;
            }

            if (exeSign == false)
            {
                userType = string.Empty;
                //LogHelper.LogInfo(" KFWebTest.aspx  userType=" + userType + "未获取到数据");
            }

            //string oldfmemo = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fmemo"]);
            //string fmemo = "企业C账户整改";

            int attid = 0;
            var attValue = string.Empty;
            if (int.TryParse(oldatttype, out attid))
            {
                attValue = Transfer.convertProAttType(attid);
            }
            string fcompany_name = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fcompany_name"]);
            LogHelper.LogInfo(string.Format("==========,{0},{1},{2},{3}", qqid, attValue, userType, fcompany_name));

            return true;

            //if (modifyAttType(qqid, atttype, oldatttype))
            //{
            //  return upgradeLog(qqid, fcre_type, userType, oldatttype, atttype, oldfmemo, fmemo);
            //}
            //else {
            //    return false;
            //}
        }

        /// <summary>
        /// 修改用户属性类型
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="atttype"></param>
        /// <param name="oldattid"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        private bool modifyAttType(string qqid, string atttype, string oldattid)
        {
            string Msg = string.Empty;

            try
            {
                //furion 20061116 email登录修改。
                string strID = ConvertToFuid(qqid);  //先转换成fuid
                string strSql = "uid=" + strID;
                strSql += "&attid=" + atttype;
                strSql += "&modify_time=" + CommQuery.ICEEncode(strNowTimeStander);

                Msg = string.Format("qqid={0},   atttype={1},    oldattid={2},", qqid, atttype, oldattid);

                if (CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERATT, out Msg) < 0)
                {
                    Msg += ",修改用户属性类型失败！";
                    LogHelper.LogInfo(" KFWebTest.aspx  " + Msg);
                    return false;
                }
                else
                {
                    Msg += ",修改用户属性类型成功！";
                    LogHelper.LogInfo(" KFWebTest.aspx  " + Msg);
                    return true;
                }
            }
            catch (Exception e)
            {
                Msg += ",修改用户属性类型异常！[" + e.ToString().Replace("'", "’") + "]";
                LogHelper.LogError(" KFWebTest.aspx  " + Msg);
                return false;
            }
        }


        private bool upgradeLog(string qqid, string fcre_type, string userType, string oldatttype, string atttype, string oldfmemo, string fmemo)
        {
            string Msg = string.Empty;
            try
            {
                bool operSucc = new AccountService().AddChangeUserInfoLog(qqid,
                    fcre_type, fcre_type,
                    userType, userType,
                    atttype, oldatttype,
                    fmemo, oldfmemo,
                    Session["uid"].ToString());

                Msg = "qqid=" + qqid + ",fcre_type=" + fcre_type + ",userType=" + userType + ",  oldatttype= " + oldatttype + ", atttype= " + atttype + " , oldfmemo=" + oldfmemo + ",fmemo= " + fmemo;
                if (operSucc)
                {
                    Msg += ",修改用户属性类型成功！";
                }
                else
                {

                    Msg += ",修改用户属性类型失败！";
                }
                LogHelper.LogInfo(" KFWebTest.aspx  " + Msg);
                return operSucc;
            }
            catch (Exception e)
            {

                Msg += ",修改用户属性类型异常！[" + e.ToString().Replace("'", "’") + "]";
                LogHelper.LogError(" KFWebTest.aspx  " + Msg);
                return false;
            }

        }

        #endregion


        private void CheckDBConn(string strkey)
        {
            string dbstr = CommLib.DbConnectionString.Instance.GetConnectionString(strkey.Trim().ToUpper());

            LogHelper.LogInfo(" test.aspx  private void GetDBConnStr  strKey：" + strkey + ",dbstr:" + dbstr);
            using (MySqlAccess da = new MySqlAccess(dbstr))
            {
                da.OpenConn();

                string sql_findUID_2 = "select * from c2c_db_74.t_card_bind_relation_5 where Fcard_id='6230582000010046574' or Fcard_id='762271dfd841546d1a271a6ee8763dbb' ";

                DataSet ds = da.dsGetTableByRange(sql_findUID_2, 0, 10);

                if (ds != null && ds.Tables.Count > 0)
                {
                    this.GridView1.DataSource = ds.Tables[0].DefaultView;
                    this.GridView1.DataBind();
                }
                else
                {
                    LogHelper.LogInfo(" KFWebTest.aspx  CheckDBConn 返回空数据。");

                    Response.Write(" KFWebTest.aspx  CheckDBConn 返回空数据。");
                    Response.End();
                }
            }
        }


        private void bindshimingrzData(string qq)
        {
            string fuin = classLibrary.setConfig.replaceMStr(qq);

            int max = 20;
            int start = 1;

            DataSet ds = new AuthenInfoService().GetUserClassQueryListForThis(fuin, start, max);

            if (ds != null && ds.Tables.Count > 0)
            {
                this.GridView1.DataSource = ds.Tables[0].DefaultView;
                this.GridView1.DataBind();
            }
            else
            {
                throw new LogicException("bindshimingrzData没有找到记录！");
            }

        }


        private void WeChatInfo(string wechatName)
        {
            string retInfo = string.Empty;

            string tempopenid = WeChatHelper.GetAAOpenIdFromWeChatName(wechatName);
            retInfo += tempopenid + "_=_" + WeChatHelper.GetAcctIdFromAAOpenId(tempopenid);

            tempopenid = WeChatHelper.GetHBOpenIdFromWeChatName(wechatName);
            retInfo += "_=_" + tempopenid;
            retInfo += "_=_" + WeChatHelper.GetAcctIdFromOpenId(tempopenid);

            retInfo += "_=_" + WeChatHelper.GetXYKHKOpenIdFromWeChatName(wechatName);

            tempopenid = WeChatHelper.GetFCXGOpenIdFromWeChatName(wechatName, Request.UserHostAddress);
            retInfo += "_=_" + tempopenid + "_=_Request IP:" + Request.UserHostAddress;

            LogHelper.LogInfo(" test.aspx  private void WeChatInfo  retInfo：" + retInfo);

            Response.Write(retInfo);
            Response.End();
        }

    }
}