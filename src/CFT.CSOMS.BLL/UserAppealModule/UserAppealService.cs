using System;

using System.Data;
using System.Configuration;
namespace CFT.CSOMS.BLL.UserAppealModule
{
    using CFT.CSOMS.DAL.UserAppealModule;
    using CFT.CSOMS.DAL.Infrastructure;
    public class UserAppealService
    {
       
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

        public bool CannelAppealSpecial(string fid, string Fcomment, string userDesc, string user, string userIP, string appeal_db, string appeal_tb)
        {
            UserAppealService userAppealService = new UserAppealService();
            bool succes = userAppealService.CannelAppeal(fid, Fcomment, "", "", user, userIP, appeal_db, appeal_tb);
            if (succes)
            {
                //11 为申诉类型
                bool writeLog = userAppealService.UpdateSepcialApealLog(fid, 2, user, Fcomment, userDesc);
                if (writeLog)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool DelAppealSpecial(string fid, string Fcomment, string userDesc, string user, string userIP, string appeal_db, string appeal_tb)
        {
            UserAppealService userAppealService = new UserAppealService();
            bool succes = userAppealService.DelAppeal(fid, Fcomment, user, userIP, appeal_db, appeal_tb);
            if (succes)
            {
                //11 为申诉类型
                bool writeLog = userAppealService.UpdateSepcialApealLog(fid, 7, user, Fcomment, userDesc);
                if (writeLog)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool ConfirmAppealSpecial(string fid, string Fcomment, string userDesc, string user, string userIP, string appeal_db, string appeal_tb)
        {
            UserAppealService userAppealService = new UserAppealService();
            bool succes = userAppealService.ConfirmAppeal(fid, Fcomment, user, userIP, appeal_db, appeal_tb);
            if (succes)
            {
                //11 为申诉类型
                bool writeLog = userAppealService.UpdateSepcialApealLog(fid, 1, user, Fcomment, userDesc);
                if (writeLog)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    
    }
}
