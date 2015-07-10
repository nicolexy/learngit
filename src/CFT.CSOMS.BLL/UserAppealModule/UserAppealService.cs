using System;

using System.Data;
using System.Configuration;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.BLL.UserAppealModule
{
    using CFT.CSOMS.DAL.UserAppealModule;
    using CFT.CSOMS.DAL.Infrastructure;
    public class UserAppealService
    {
        public DataSet QueryApealListNewDB(string startTime, string endTime, int state, int type)
        {
            return new UserAppealData().QueryApealListNewDB(startTime, endTime, state, type);
        }

        /// <summary>
        /// 申诉未完成状态结单批量处理
        /// 包含：
        /// 特殊找密60天以前未补充资料的，自动结单为：20 结单（未补填资料）
        /// 普通、微信解冻：该账户不处于冻结状态(10 已补充资料、2 待补充资料、0 未处理) 自动结单为：21 结单（无冻结日志）
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="proType">处理类型：1：解冻（包含普通解冻、微信解冻），2：特殊找密</param>
        public void BatchFinishAppeal(string startTime, string endTime,string proType)
        {
            try
            {
                DataSet ds;
                #region 查询符合结单的申诉记录
                try
                {
                    if (proType == "2")
                    {
                        ds = QueryApealListNewDB(startTime, endTime, 11, 11);//特殊找密
                    }
                    else if (proType == "1")
                    {
                        DataSet normalDS = QueryApealListNewDB(startTime, endTime, 0, 8);//普通解冻 未处理
                        DataSet wxFreDS = QueryApealListNewDB(startTime, endTime, 0, 19);//微信解冻 未处理
                        ds = PublicRes.ToOneDataset(normalDS, wxFreDS);//两个申诉类型结果合并到一个库中

                        normalDS = QueryApealListNewDB(startTime, endTime, 2, 8);//普通解冻 已补充资料
                        ds = PublicRes.ToOneDataset(ds, normalDS);
                        wxFreDS = QueryApealListNewDB(startTime, endTime, 2, 19);//微信解冻 已补充资料
                        ds = PublicRes.ToOneDataset(ds, wxFreDS);

                        normalDS = QueryApealListNewDB(startTime, endTime, 10, 8);//普通解冻 待补充资料
                        ds = PublicRes.ToOneDataset(ds, normalDS);
                        wxFreDS = QueryApealListNewDB(startTime, endTime, 10, 19);//微信解冻 待补充资料
                        ds = PublicRes.ToOneDataset(ds, wxFreDS);


                        //筛选出无冻结日志申诉记录
                        ScreenNoFreezeLogApeal(ds);
                    }
                    else
                        throw new Exception("类型不正确");
                }
                catch (Exception ex)
                {
                    throw new Exception("查询冻结申诉记录异常" + ex);
                }
                #endregion

                #region 结单，修改申诉状态
                int Total = 0;
                int Success = 0;
                int Fail = 0;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Total = ds.Tables[0].Rows.Count;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            UserAppealData userAppealData = new UserAppealData();
                            if (userAppealData.FinishAppealNewDB(dr["fid"].ToString(), "系统批量结单", "system"))
                                Success++;
                            else
                                Fail++;

                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                #endregion

                LogHelper.LogInfo("特殊申诉未完成状态批量结单处理,总笔数：" + Total + " ,成功笔数：" + Success + " ,失败笔数：" + Fail);

            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("批量申诉未完成状态结单处理异常：" + ex);
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 筛选出无冻结日志申诉记录
        /// </summary>
        /// <param name="ds">解冻申诉记录</param>
        public void ScreenNoFreezeLogApeal(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        int type = int.Parse(dr["FType"].ToString());
                        if (type == 8 || type == 19)
                        {
                            //查询冻结日志信息
                            DataSet ds2 = new UserAppealData().QueryFreezeListLog(dr["Fuin"].ToString(), 1);
                            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                            {
                                dr.Delete();
                            }
                        }
                        else
                            dr.Delete();

                    }
                    catch (Exception ex)
                    {
                        dr.Delete();
                        throw new Exception("Fuin=" + dr.ToString() + " 查询冻结日志信息异常：" + ex);
                    }

                }

                ds.AcceptChanges();
            }
        }
       
        //自助申诉详细页面判断是否实名认证 API接口
        public bool GetUserAuthenState(string uin, string userBankID, int bankType)
        {
            try
            {
                DataSet ds = new UserAppealData().GetUserAuthenState(uin, userBankID, bankType);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count != 1)
                {
                    return false;//是否实名认证
                }
                else
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    if (row["queryType"].ToString() == "2")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取是否实名认证异常："+ex.Message);
            }
        }

        //根据财付通帐号uin查询用户基本信息
        public DataTable GetAppealUserInfo(string qqid)
        {
            try
            {
                DataSet ds =new UserAppealData().GetAppealUserInfo(qqid);
                if (ds != null && ds.Tables.Count > 0)
                {
                    ds.Tables[0].Columns.Add("FBalanceStr", typeof(String));//总金额
                    ds.Tables[0].Columns.Add("FconStr", typeof(String));//冻结金额
                    ds.Tables[0].Columns.Add("Fcre_type_str", typeof(String));//证件类型
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string creid = ds.Tables[0].Rows[0]["Fcreid"].ToString();
                        ds.Tables[0].Rows[0]["Fcre_type_str"] =PublicRes.GetCreType(ds.Tables[0].Rows[0]["Fcre_type"].ToString());
                    }
                    COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "FBalance", "FBalanceStr");
                    COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "Fcon", "FconStr");
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("查询用户基本信息异常：" + ex.Message);
            }
        }

        //自助申诉记录列表查询
        public DataTable GetUserAppealList(string fuin, string begindate, string enddate, int fstate, int ftype, string QQType, string dotype, int offset, int limit, int SortType)
        {
            DataSet ds =new DataSet();
            if (ftype == 1 || ftype == 5 || ftype == 6 || ftype == 99)//三种类型在原来基础上进行分库分表
            {
                DataSet dsNew = new UserAppealData().GetCFTUserAppealListNew(fuin, begindate, enddate, fstate, ftype, QQType, dotype, SortType);
                if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsNew.Tables[0];
                    dt = PublicRes.GetPagedTable(dt, offset, limit);//分页处理
                    ds.Tables.Add(dt.Copy());
                }
            }
            else
            {
                ds = new UserAppealData().GetCFTUserAppealList(fuin, begindate, enddate, fstate, ftype, QQType, dotype, offset, limit, SortType);
            }

            ds = new UserAppealData().GetCFTUserAppealListFunction(ds);//处理内部信息,不排序

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
                return null;
        }

        //查询自助申诉记录详细信息
        public DataTable GetUserAppealDetail(string fid, string db, string tb)
        {
            try
            {
                DataSet ds = new DataSet();

                #region 查询结果
                if (string.IsNullOrEmpty(db.Trim()) || string.IsNullOrEmpty(tb.Trim()))
                {
                    #region
                    ds = new UserAppealData().GetCFTUserAppealDetail(int.Parse(fid));

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ds.Tables[0].Columns.Add("Image1", typeof(String));//正面
                        ds.Tables[0].Columns.Add("Image2", typeof(String));//反面

                        string imagestr = dr["cre_image"].ToString().Trim();
                        string url = ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

                        if (!url.EndsWith("/"))
                            url += "/";

                        // 目前财付盾解绑需要两张图片
                        if (dr["FType"].ToString() == "0" || dr["FType"].ToString() == "9" || dr["FType"].ToString() == "10")
                        {
                            if (imagestr.IndexOf("|") > 0)
                            {
                                string[] imgUrls = imagestr.Split('|');
                               dr["Image1"] = url + imgUrls[0];
                               dr["Image2"] = url + imgUrls[1];
                            }
                            else
                            {
                                dr["Image1"] = url + imagestr;
                            }
                        }
                        else
                        {
                            dr["Image1"] = url + imagestr;

                            //更换关联手机时再加入一个图片
                            string imagestr2 = dr["cre_image2"].ToString().Trim();
                            dr["Image2"] = url + imagestr2;
                        }

                    }
                    #endregion
                }
                else if (!string.IsNullOrEmpty(db.Trim()) && !string.IsNullOrEmpty(tb.Trim()))//三种类型的分库表
                {
                    #region
                    ds = new UserAppealData().GetCFTUserAppealDetailByDBTB(fid, db, tb);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ds.Tables[0].Columns.Add("Image1", typeof(String));//正面
                        ds.Tables[0].Columns.Add("Image2", typeof(String));//反面

                        string imagestr = dr["cre_image"].ToString().Trim();
                        string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();

                        if (!url.EndsWith("/"))
                            url += "/";

                        string urlCGI = System.Configuration.ConfigurationManager.AppSettings["GetAppealImageCgi"].Trim();//拉图片cgi
                        //20131111 lxl 加三种分库表类型可能两张图片
                        // 目前财付盾解绑需要两张图片
                        if (dr["FType"].ToString() == "0" || dr["FType"].ToString() == "9" || dr["FType"].ToString() == "10" || dr["FType"].ToString() == "1" || dr["FType"].ToString() == "5" || dr["FType"].ToString() == "6")
                        {
                            if (imagestr.IndexOf("|") > 0)
                            {
                                string[] imgUrls = imagestr.Split('|');
                                //新库表的身份证图片通过cgi来获取
                                dr["Image1"] = urlCGI + imgUrls[0];
                                dr["Image2"] = urlCGI + imgUrls[1];

                            }
                            else
                            {
                                dr["Image1"] = urlCGI + imagestr;
                            }
                        }
                        else
                        {
                            dr["Image1"] = urlCGI + imagestr;

                            //更换关联手机时再加入一个图片
                            string imagestr2 = dr["cre_image2"].ToString().Trim();  //是否是5类型？
                            dr["Image2"] = urlCGI + imagestr2;
                        }
                    }
                    #endregion
                }
                #endregion

                //处理结果，页面逻辑
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ds.Tables[0].Columns.Add("cre_type_str", typeof(String));//证件类型
                    ds.Tables[0].Columns.Add("clear_pps_str", typeof(String));//申诉类型为找回密码时，是否清空密保资料标记
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dr["cre_type_str"] = PublicRes.GetCreType(dr["cre_type"].ToString());

                        if (dr["clear_pps"].ToString() == "1" && dr["FType"].ToString() == "1")
                        {
                            dr["clear_pps_str"] = "清除";
                        }
                        else if (dr["FType"].ToString() == "1" && dr["clear_pps"].ToString() != "1")
                        {
                            dr["clear_pps_str"] = "不清除";
                        }
                        else
                        {
                            dr["clear_pps_str"] = "";
                        }
                    
                    }
                  
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("查询自助申诉记录详细信息异常：" + ex.Message);
            }
        }

        public bool DelAppeal(string fid, string Fcomment, string user, string userIP, string appeal_db, string appeal_tb)
        {
            if (string.IsNullOrEmpty(fid))
            {
                throw new ArgumentNullException("fid");
            }

            return new UserAppealData().DelAppeal(fid, Fcomment, user, userIP, appeal_db, appeal_tb);
        }

        public bool CannelAppeal(string fid, string reason, string OtherReason, string Fcomment, string user, string userIP, string appeal_db, string appeal_tb)
        {
            if (string.IsNullOrEmpty(fid))
            {
                throw new ArgumentNullException("fid");
            }
            if (!string.IsNullOrEmpty(appeal_db) && !string.IsNullOrEmpty(appeal_tb))
            {
                return new UserAppealData().CancelAppealDBTB(fid, appeal_db, appeal_tb, reason, OtherReason, Fcomment, user, userIP);
            }
            else
            {
                return new UserAppealData().CannelAppeal(fid, reason, OtherReason, Fcomment, user, userIP);
            }
        }

        public bool ConfirmAppeal(string fid, string Fcomment, string user, string userIP, string appeal_db, string appeal_tb)
        {
            if (string.IsNullOrEmpty(fid))
            {
                throw new ArgumentNullException("fid");
            }
            if (!string.IsNullOrEmpty(appeal_db) && !string.IsNullOrEmpty(appeal_tb))
            {
                return new UserAppealData().ConfirmAppealDBTB(fid, appeal_db, appeal_tb, Fcomment, user, userIP);
            }
            else
            {
                return new UserAppealData().ConfirmAppeal(fid, Fcomment, user, userIP);
            }
        }

        public bool UpdateSepcialApealLog(string ffreezeListID, int handleType, string handleUser, string handleResult, string userDesc)
        {
            return new UserAppealData().UpdateSepcialApealLog(ffreezeListID, handleType, handleUser, handleResult, userDesc);
        }

        public bool CannelAppealSpecial(string fid, string handleResult,string Fcomment, string userDesc, string user, string userIP, string appeal_db, string appeal_tb)
        {
            if (string.IsNullOrEmpty(appeal_db) || string.IsNullOrEmpty(appeal_tb))
            {
                GetTableName(fid, out appeal_db, out appeal_tb);
            }
            UserAppealService userAppealService = new UserAppealService();
            bool succes = userAppealService.CannelAppeal(fid, handleResult, "", Fcomment, user, userIP, appeal_db, appeal_tb);
            if (succes)
            {
                //11 为申诉类型
                bool writeLog = userAppealService.UpdateSepcialApealLog(fid, 2, user, handleResult, userDesc);
                if (writeLog)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool DelAppealSpecial(string fid, string handleResult, string Fcomment, string userDesc, string user, string userIP, string appeal_db, string appeal_tb)
        {
            if (string.IsNullOrEmpty(appeal_db) || string.IsNullOrEmpty(appeal_tb))
            {
                GetTableName(fid, out appeal_db, out appeal_tb);
            }
            UserAppealService userAppealService = new UserAppealService();
            bool succes = userAppealService.DelAppeal(fid, Fcomment, user, userIP, appeal_db, appeal_tb);
            if (succes)
            {
                //11 为申诉类型
                bool writeLog = userAppealService.UpdateSepcialApealLog(fid, 7, user, handleResult, userDesc);
                if (writeLog)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool ConfirmAppealSpecial(string fid, string handleResult, string Fcomment, string userDesc, string user, string userIP, string appeal_db, string appeal_tb)
        {
            if (string.IsNullOrEmpty(appeal_db) || string.IsNullOrEmpty(appeal_tb))
            {
                GetTableName(fid, out appeal_db, out appeal_tb);
            }
            UserAppealService userAppealService = new UserAppealService();
            bool succes = userAppealService.ConfirmAppeal(fid, Fcomment, user, userIP, appeal_db, appeal_tb);
            if (succes)
            {
                //11 为申诉类型
                bool writeLog = userAppealService.UpdateSepcialApealLog(fid, 1, user, handleResult, userDesc);
                if (writeLog)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private void GetTableName(string fid, out string appeal_db, out string appeal_tb)
        {
            string yearStr = fid.Substring(0, 4);
            string monthStr = fid.Substring(4, 2);
            int year, month;
            if (int.TryParse(yearStr, out year) && int.TryParse(monthStr, out month))
            {
                appeal_db = "db_appeal_" + yearStr;
                appeal_tb = "t_tenpay_appeal_trans_" + monthStr;
            }
            else
            {
                throw new Exception("fid输入格式不正确");
            }
        }

        /// <summary>
        /// 创建风控冻结处理日志NEW
        /// </summary>
        public bool CreateFreezeDiary(string ffreezeListID, int handleType, string handleUser, string handleResult
        , string memo, string uin, string userPhone, string submitDate, int bt, string userDesc, string zdyBt1, string zdyBt2, string zdyBt3
        , string zdyBt4, string zdyCont1, string zdyCont2, string zdyCont3, string zdyCont4)
        {
            return new UserAppealData().CreateFreezeDiary(ffreezeListID, handleType, handleUser, handleResult, memo, uin, userPhone, submitDate, bt, userDesc,
                zdyBt1, zdyBt2, zdyBt3, zdyBt4, zdyCont1, zdyCont2, zdyCont3, zdyCont4);
        }

        //选择补充资料的掩码计算
        public int GetIntBT(string creid, string creid2, string bank, string fund1, string fund2, string fund3, string fund4, string fund5, string fund6,
            string fund7, string user_desc)
        {
            Int32 bt = 0;
            try
            {
                if (creid == " 1")
                {
                    //身份证正面
                    bt = bt | 0x00000001;
                    bt = bt | 0x00000002;
                }
                if (creid2 == "1")
                {
                    //身份证反面
                    bt = bt | 0x00000004;
                    bt = bt | 0x00000008;
                }
                if (bank == " 1")
                {
                    //银行卡
                    bt = bt | 0x00000800;
                    bt = bt | 0x00001000;
                }

                //资金来源
                if (fund1 == " 1" || fund2 == " 1" || fund3 == " 1" || fund4 == " 1" || fund5 == " 1" || fund6 == " 1" || fund7 == " 1")
                {
                    bt = bt | 0x00010000;
                    if (fund1 == " 1")
                    { bt = bt | 0x00020000; }

                    if (fund2 == " 1")
                    { bt = bt | 0x00040000; }

                    if (fund3 == " 1")
                    { bt = bt | 0x00080000; }

                    if (fund4 == " 1")
                    { bt = bt | 0x00100000; }

                    if (fund5 == " 1")
                    { bt = bt | 0x00200000; }

                    if (fund6 == " 1")
                    { bt = bt | 0x00400000; }

                    if (fund7 == " 1")
                    { bt = bt | 0x00800000; }
                }

                //用户描述不为空
                if (string.IsNullOrEmpty(user_desc))
                {
                    bt = bt | 0x08000000;
                }
            }
            catch
            {
                throw new Exception("转码失败！");
            }

            return bt;
        }

        public DataSet GetFreezeDiary(string fid, int type)
        {
            DataSet ds = new UserAppealData().GetFreezeDiary("", fid, "", "", "", "", "", "", 1, 20);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                return null;
            }

            ds.Tables[0].Columns.Add("handleResult", typeof(string));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (type == 11)
                {
                    //特殊找回支付密码
                    dr["handleResult"] = dr["FCreateDate"].ToString() + "  " + dr["FHandleUser"].ToString()
                      + " 执行了 " + ConvertToAppealSpecialString(dr["FHandleType"].ToString()) + " 操作，用户描述为：" + dr["FMemo"].ToString() + "；客服处理结果为：" + dr["FHandleResult"].ToString();
                }
                else
                {
                    //普通解冻或微信解冻
                    dr["handleResult"] = dr["FCreateDate"].ToString() + "  " + dr["FHandleUser"].ToString()
                  + " 执行了 " + ConvertToAppealString(dr["FHandleType"].ToString()) + " 操作，用户描述为：" + dr["FMemo"].ToString() + "；客服处理结果为：" + dr["FHandleResult"].ToString();
                }
            }

            return ds;
        }

        public DataSet GetSpecialAppealList(string uin, string BeginDate, string EndDate, int ftype, int iStatue,
           string szListID, string szFreezeUser, string szFreezeReason, int iPageStart, int iPageMax, string orderType)
        {
            DataSet ds = new UserAppealData().GetSpecialAppealList(uin, BeginDate, EndDate, ftype, iStatue, szListID, szFreezeUser, szFreezeReason, iPageStart, iPageMax,
                orderType);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("handleStateName", typeof(string));

                DataRow dr = ds.Tables[0].Rows[0];

                if (ftype == 8 || ftype == 19)//冻结类型状态处理
                {
                    string stateName = "{0}";
                    if (dr["isFreezeListHas"].ToString() == "0")
                    {
                        stateName = "该账户不处于冻结状态 ({0})";
                    }

                    switch (dr["Fstate"].ToString())
                    {
                        case "0":
                            {
                                stateName = string.Format(stateName, "未处理"); ; break;
                            }
                        case "1":
                            {
                                stateName = string.Format(stateName, "结单(已解冻)"); break;
                            }
                        case "2":
                            {
                                stateName = string.Format(stateName, "待补充资料"); break;
                            }
                        case "7":
                            {
                                stateName = string.Format(stateName, "已作废"); break;
                            }
                        case "8":
                            {
                                stateName = string.Format(stateName, "挂起"); break;
                            }
                        case "10":
                            {
                                stateName = string.Format(stateName, "已补充资料"); break;
                            }
                        default:
                            {
                                stateName = string.Format(stateName, "未知" + dr["Fstate"].ToString()); break;
                            }
                    }
                    dr["handleStateName"] = stateName;
                }
                else if (ftype == 11)//特殊找回支付密码状态
                {
                    switch (dr["Fstate"].ToString())
                    {
                        case "0":
                            {
                                dr["handleStateName"] = "未处理"; break;
                            }
                        case "1":
                            {
                                dr["handleStateName"] = "申诉成功"; break;
                            }
                        case "2":
                            {
                                dr["handleStateName"] = "申诉失败"; break;
                            }
                        case "7":
                            {
                                dr["handleStateName"] = "已删除"; break;
                            }
                        case "11":
                            {
                                dr["handleStateName"] = "待补充资料"; break;
                            }
                        case "12":
                            {
                                dr["handleStateName"] = "已补充资料"; break;
                            }
                        default:
                            {
                                dr["handleStateName"] = "未知" + dr["Fstate"].ToString(); break;
                            }
                    }
                }
            }
            return ds;

        }

        public DataSet GetSpecialAppealDetail(string fid)
        {
            string db, tb;
            GetTableName(fid, out db, out tb);  //验证输入的fid合法性
            DataSet ds = new UserAppealData().GetSpecialAppealDetail(fid);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                try
                {
                    string img_cgi = ConfigurationManager.AppSettings["GetAppealImageCgi"].ToString();

                    ds.Tables[0].Columns.Add("ClearPPS", typeof(string));
                    ds.Tables[0].Columns.Add("FCreImg1Str", typeof(string));//身份证正面
                    ds.Tables[0].Columns.Add("FCreImg2Str", typeof(string));//身份证反面
                    ds.Tables[0].Columns.Add("FOtherImage1Str", typeof(string));//银行卡照片
                    ds.Tables[0].Columns.Add("FProveBanlanceImageStr", typeof(string));//资金来源截图
                    ds.Tables[0].Columns.Add("FOtherImage2Str", typeof(string));//补充其他证件照片
                    ds.Tables[0].Columns.Add("FOtherImage3Str", typeof(string));//补充的手持身份证半身照
                    ds.Tables[0].Columns.Add("FOtherImage4Str", typeof(string));//补充户籍证明照片
                    ds.Tables[0].Columns.Add("FOtherImage5Str", typeof(string));//补充资料截图

                    ds.Tables[0].Columns.Add("Fsup_desc1Str", typeof(string));//自定义标题1
                    ds.Tables[0].Columns.Add("Fsup_desc2Str", typeof(string));//自定义标题2
                    ds.Tables[0].Columns.Add("Fsup_desc3Str", typeof(string));//自定义标题3
                    ds.Tables[0].Columns.Add("Fsup_desc4Str", typeof(string));//自定义标题4
                    ds.Tables[0].Columns.Add("Fsup_tips1Str", typeof(string));//自定义内容1
                    ds.Tables[0].Columns.Add("Fsup_tips2Str", typeof(string));//自定义内容2
                    ds.Tables[0].Columns.Add("Fsup_tips3Str", typeof(string));//自定义内容3
                    ds.Tables[0].Columns.Add("Fsup_tips4Str", typeof(string));//自定义内容4

                    DataRow dr = ds.Tables[0].Rows[0];
                    int ftype = int.Parse(dr["Ftype"].ToString());

                    if (dr["FClearPps"].ToString() == "1" && dr["FType"].ToString() == "11")//是否清空密保资料
                    {
                        dr["ClearPPS"] = "清除";
                    }
                    else if (dr["FType"].ToString() == "11" && dr["FClearPps"].ToString() != "1")
                    {
                        dr["ClearPPS"] = "不清除";
                    }
                    else
                    {
                        dr["ClearPPS"] = "";
                    }

                    #region 图片

                    if (!(dr["FCreImg1"] is DBNull))
                    {
                        if (dr["FCreImg1"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                dr["FCreImg1Str"] = dr["FCreImg1"].ToString();  //身份证正面
                            }
                            else
                            {
                                dr["FCreImg1Str"] = img_cgi + dr["FCreImg1"].ToString();
                            }
                        }
                    }
                    if (!(dr["FCreImg2"] is DBNull))
                    {
                        if (dr["FCreImg2"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                dr["FCreImg2Str"] = dr["FCreImg2"].ToString();  //身份证反面
                            }
                            else
                            {
                                dr["FCreImg2Str"] = img_cgi + dr["FCreImg2"].ToString();
                            }
                        }
                    }
                    if (!(dr["FOtherImage1"] is DBNull))
                    {
                        if (dr["FOtherImage1"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                dr["FOtherImage1Str"] = dr["FOtherImage1"].ToString();  //银行卡照片
                            }
                            else
                            {
                                dr["FOtherImage1Str"] = img_cgi + dr["FOtherImage1"].ToString();
                            }
                        }
                    }
                    if (!(dr["FProveBanlanceImage"] is DBNull))
                    {
                        if (dr["FProveBanlanceImage"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                dr["FProveBanlanceImageStr"] = dr["FProveBanlanceImage"].ToString();  //资金来源截图
                            }
                            else
                            {
                                dr["FProveBanlanceImageStr"] = img_cgi + dr["FProveBanlanceImage"].ToString();
                            }
                        }
                    }

                    if (!(dr["FOtherImage2"] is DBNull))
                    {
                        if (dr["FOtherImage2"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                dr["FOtherImage2Str"] = dr["FOtherImage2"].ToString();  //补充其他证件照片
                            }
                            else
                            {
                                dr["FOtherImage2Str"] = img_cgi + dr["FOtherImage2"].ToString();
                            }
                        }
                    }
                    if (!(dr["FOtherImage3"] is DBNull))
                    {
                        if (dr["FOtherImage3"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                dr["FOtherImage3Str"] = dr["FOtherImage3"].ToString();  //补充的手持身份证半身照
                            }
                            else
                            {
                                dr["FOtherImage3Str"] = img_cgi + dr["FOtherImage3"].ToString();
                            }
                        }
                    }
                    if (!(dr["FOtherImage4"] is DBNull))
                    {
                        if (dr["FOtherImage4"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                dr["FOtherImage4Str"] = dr["FOtherImage4"].ToString();  //补充户籍证明照片
                            }
                            else
                            {
                                dr["FOtherImage4Str"] = img_cgi + dr["FOtherImage4"].ToString();
                            }
                        }
                    }
                    if (!(dr["FOtherImage5"] is DBNull))
                    {
                        if (dr["FOtherImage5"].ToString() != "")
                        {
                            if (ftype == 19)
                            {
                                dr["FOtherImage5Str"] = dr["FOtherImage5"].ToString();  //补充资料截图
                            }
                            else
                            {
                                dr["FOtherImage5Str"] = img_cgi + dr["FOtherImage5"].ToString();
                            }
                        }
                    }

                    #endregion

                    #region  自定义标题、内容

                    if (!(dr["Fsup_desc1"] is DBNull))
                    {
                        dr["Fsup_desc1Str"] = dr["Fsup_desc1"].ToString();//自定义标题1
                    }
                    if (!(dr["Fsup_desc2"] is DBNull))
                    {
                        dr["Fsup_desc2Str"] = dr["Fsup_desc2"].ToString();
                    }
                    if (!(dr["Fsup_desc3"] is DBNull))
                    {
                        dr["Fsup_desc3Str"] = dr["Fsup_desc3"].ToString();
                    }
                    if (!(dr["Fsup_desc4"] is DBNull))
                    {
                        dr["Fsup_desc4Str"] = dr["Fsup_desc4"].ToString();
                    }

                    if (!(dr["Fsup_tips1"] is DBNull))
                    {
                        dr["Fsup_tips1Str"] = dr["Fsup_tips1"].ToString();//自定义内容1
                    }
                    if (!(dr["Fsup_tips2"] is DBNull))
                    {
                        dr["Fsup_tips2Str"] = dr["Fsup_tips2"].ToString();
                    }
                    if (!(dr["Fsup_tips3"] is DBNull))
                    {
                        dr["Fsup_tips3Str"] = dr["Fsup_tips3"].ToString();
                    }
                    if (!(dr["Fsup_tips4"] is DBNull))
                    {
                        dr["Fsup_tips4Str"] = dr["Fsup_tips4"].ToString();
                    }

                    #endregion

                    return ds;
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("GetSpecialAppealDetail: 特殊申诉详情查询失败： " + ex.Message);
                    return null;
                }
            }
        }

        private string ConvertToAppealSpecialString(string type)
        {
            switch (type)
            {
                case "1":
                    return "通过";
                case "2":
                    return "拒绝";
                case "7":
                    return "删除";
                case "11":
                    return "补充资料";
                default:
                    return "未知操作" + type;
            }
        }

        private string ConvertToAppealString(string type)
        {
            switch (type)
            {
                case "1":
                    return "结单（已解冻）";
                case "2":
                    return "补充资料";
                case "7":
                    return "作废";
                case "8":
                    return "挂起";
                case "100":
                    return "补充处理结果";
                default:
                    return "未知操作" + type;
            }
        }
    
    }
}
