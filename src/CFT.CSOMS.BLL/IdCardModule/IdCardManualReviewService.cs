using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CFT.Apollo.Logging;
using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.IdCardModule;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.IdCardModule
{
    /// <summary>
    /// 身份证影印件客服人工审核
    /// </summary>
    public class IdCardManualReviewService
    {
        /// <summary>
        /// 客服领取需要人工审核的单据
        /// </summary>
        /// <param name="uin">用户帐号</param>
        /// <returns></returns>
        public bool ReceiveNeedReviewIdCardData(string uid, string uin, int reviewCount, string beginDate, string endDate, out string message)
        {
            bool receiveResult = false;
            message = string.Empty;
            try
            {
                IdCardManualReview idCardManualReviewDAL = new IdCardManualReview();
                List<string> yearMonths = GetYearMonthList(DateTime.Parse(beginDate).ToString("yyyy-MM"), DateTime.Parse(endDate).ToString("yyyy-MM"), 5);
                if (!string.IsNullOrEmpty(uin))
                {
                    //通过帐号分配任务
                    receiveResult = idCardManualReviewDAL.ReceiveNeedReviewIdCardData(uid, uin, yearMonths, out message);
                }
                else
                {
                    //批量分配任务
                    receiveResult = idCardManualReviewDAL.ReceiveNeedReviewIdCardData2(uid, reviewCount, yearMonths, out message);
                }

            }
            catch (Exception ex)
            {
                receiveResult = false;
            }
            return receiveResult;
        }


        /// <summary>
        /// 客服领取需要人工审核的单据
        /// </summary>
        /// <param name="startDate">查询开始日期</param>
        /// <param name="endDate">查询结束日期</param>
        /// <param name="receiveCount">批处理数量</param>
        /// <returns></returns>
        public DataTable LoadReview(string uid, string uin, int reviewStatus, int reviewResult, string beginDate, string endDate, bool isHaveRightForSeeDetail,string modifyBeginDate,string modifyEndDate,string foperator,int fmemo,int pageSize, int pageNumber, string order, ref int total)
        {
            DataTable dt = new DataTable();
            try
            {
                List<string> yearMonths = GetYearMonthList(DateTime.Parse(beginDate).ToString("yyyy-MM"), DateTime.Parse(endDate).ToString("yyyy-MM"), 5);
                IdCardManualReview idCardManualReviewDAL = new IdCardManualReview();
                dt = idCardManualReviewDAL.LoadReview(uid, uin, reviewStatus, reviewResult, yearMonths, beginDate, endDate, isHaveRightForSeeDetail, modifyBeginDate, modifyEndDate, foperator, fmemo,pageSize, pageNumber, order, ref  total);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public DataTable LoadReviewForExport(string uid, string uin, int reviewStatus, int reviewResult, string beginDate, string endDate,string modifyBeginDate,string modifyEndDate,string foperator,int fmemo,  string order)
        {
            DataTable dt = new DataTable();
            try
            {
                List<string> yearMonths = GetYearMonthList(DateTime.Parse(beginDate).ToString("yyyy-MM"), DateTime.Parse(endDate).ToString("yyyy-MM"), 5);
                IdCardManualReview idCardManualReviewDAL = new IdCardManualReview();
                dt = idCardManualReviewDAL.LoadReviewForExport(uid, uin, reviewStatus, reviewResult, yearMonths, beginDate, endDate, modifyBeginDate, modifyEndDate, foperator, fmemo, order);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }


        /// <summary>
        /// 加载审核信息
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fserial_numbe"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable LoadReview(int fid, string fserial_numbe, string tableName)
        {
            DataTable dt = new DataTable();
            try
            {
                IdCardManualReview idCardManualReviewDAL = new IdCardManualReview();
                dt = idCardManualReviewDAL.LoadReview(fid, fserial_numbe, tableName);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public bool Update(string fserial_numbe, int fid, int fresult, string memo, string tableName, string foperator,int agreeRemark, out string message)
        {
            bool receiveResult = false;
            message = string.Empty;
            try
            {
                IdCardManualReview idCardManualReviewDAL = new IdCardManualReview();
                receiveResult = idCardManualReviewDAL.Update(fserial_numbe, fid, fresult, memo, tableName, foperator, agreeRemark, out  message);

            }
            catch (Exception ex)
            {
                receiveResult = false;
                message = "保存失败";
            }
            return receiveResult;
        }

        public bool UpdateFstate(string fserial_numbe, int fid, int fstate, string tableName)
        {
            bool receiveResult = false;
            try
            {
                IdCardManualReview idCardManualReviewDAL = new IdCardManualReview();
                receiveResult = idCardManualReviewDAL.UpdateFstate(fserial_numbe, fid, fstate, tableName);
            }
            catch (Exception ex)
            {
                receiveResult = false;
            }
            return receiveResult;
        }

        /// <summary>
        /// 银行查补单状态查询
        /// </summary>
        /// <param name="bank_type"></param>
        /// <param name="bill_no"></param>
        /// <param name="transaction_id"></param>
        /// <returns>返回格式如下:result=0&res_info=ok&amount=10&bank_billno=201608040000125722&bank_query_source=3&bank_query_time=2016-08-04 12:36:51&bill_no=201608040000125722&pay_result=2&sync_state=15&transaction_id=1216402401321608040000000000</returns>
        public bool Review(string uin, string seq_no, string credit_spid, string front_image, string back_image, int audit_result, string audit_error_des, string audit_operator, string audit_time, out string msg)
        {
            //LogHelper.LogInfo(string.Format("IdCardManualReviewService.Review,uin={0},uid={1},seq_no={2},credit_spid={3},front_image={4},back_image={5},audit_result={6},audit_error_des={7},audit_operator={8},audit_time={9}", uin, uid, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time));            
            msg = "";
            bool result = false;
            try
            {
                #region 接口签名
                //接口签名md5($uin$seq_no$credit_spid$front_image$back_image$audit_result$audit_error_des$audit_operator$audit_time$key)开发联调环境key: 1234
                StringBuilder sb_sign = new StringBuilder();
                sb_sign.Append(uin.ToString());
                if (!string.IsNullOrEmpty(seq_no))
                {
                    sb_sign.Append(seq_no.ToString());//seq_no
                }
                if (!string.IsNullOrEmpty(credit_spid))
                {
                    sb_sign.Append(credit_spid.ToString());//credit_spid
                }
                if (!string.IsNullOrEmpty(front_image))
                {
                    sb_sign.Append(front_image.ToString());//front_image
                }
                if (!string.IsNullOrEmpty(back_image))
                {
                    sb_sign.Append(back_image.ToString());//back_image
                }
                if (audit_result > 0)
                {
                    sb_sign.Append(audit_result.ToString());//audit_result
                }
                if (!string.IsNullOrEmpty(audit_error_des))
                {
                    sb_sign.Append(audit_error_des.ToString());//audit_error_des
                }
                if (!string.IsNullOrEmpty(audit_operator))
                {
                    sb_sign.Append(audit_operator.ToString());//audit_operator
                }
                if (!string.IsNullOrEmpty(audit_time))
                {
                    sb_sign.Append(DateTime.Parse(audit_time).ToString("yyyy-MM-dd HH:mm:ss"));//audit_time
                }
                sb_sign.Append("e1674ed8b2d4e12b99a06cd48368369d");//key //开发联调环境key: 12345
                LogHelper.LogInfo("IdCardManualReviewService.Review,sb_sign:" + sb_sign.ToString());
                string sb_sign_ToUTF8 = ToUTF8(sb_sign.ToString());
                string sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb_sign_ToUTF8, "MD5").ToLower();// GetReviewSign(dic); //"edf3ea3fd7d7610188acb1a7fc1433f8";
                LogHelper.LogInfo("IdCardManualReviewService.Review,sign:" + sign);
                #endregion

                #region 业务签名
                StringBuilder sb_reqString = new StringBuilder();
                sb_reqString.AppendFormat("uin={0}", uin.ToString());
                sb_reqString.AppendFormat("&seq_no={0}", seq_no);
                sb_reqString.AppendFormat("&credit_spid={0}", credit_spid);
                sb_reqString.AppendFormat("&front_image={0}", front_image);
                sb_reqString.AppendFormat("&back_image={0}", back_image);
                sb_reqString.AppendFormat("&audit_result={0}", audit_result);
                sb_reqString.AppendFormat("&audit_error_des={0}", audit_error_des);
                sb_reqString.AppendFormat("&audit_operator={0}", audit_operator);
                sb_reqString.AppendFormat("&audit_time={0}", DateTime.Parse(audit_time).ToString("yyyy-MM-dd HH:mm:ss"));
                sb_reqString.AppendFormat("&sign={0}", sign);

                byte[] bytes = Encoding.Default.GetBytes(sb_reqString.ToString());
                string reqTextStr = Convert.ToBase64String(bytes);
                //LogHelper.LogInfo("IdCardManualReviewService.Review,reqTextStr:" + reqTextStr);
                //sb_cgiString.Append("&ReqText=dWluPTIwMTMxMTA3OTAyNDEzOUB3eC50ZW5wYXkuY29tJnVpZD0yOTk3MDg1MTUmc2VxX25vPTExNDcwOTkyNDkzMDAwMDAwMDEmY3JlZGl0X3NwaWQ9MTAwMDAwMDMmZnJvbnRfaW1hZ2U9MjAxNjA0MjUxNDI3NTkxNzAyMzExJmJhY2tfaW1hZ2U9MjAxNjA0MjUxNDI3NTkxNzAyMzEyJmF1ZGl0X3Jlc3VsdD0xJmF1ZGl0X2Vycm9yX2Rlcz1pbWFnZSBub3QgY2xlYXImYXVkaXRfb3BlcmF0b3I9aGVpZGl6aGFuZyZhdWRpdF90aW1lPTIwMTYtMDgtMTEgMTA6MDA6MDAmc2lnbj1lZGYzZWEzZmQ3ZDc2MTAxODhhY2IxYTdmYzE0MzNmOA==");
                #endregion

                #region //平台签名：参数URL串+平台商户key的MD5值（参数URL串要求：按参数名ASCII顺序，为空的参数不参与签名）
                //调用实名系统的银行接口 申请的客服商户号：1000077701，对应的密钥：c0df1a64cb1431fb34f400db1d0ac3b4
                Dictionary<string, string> dic_Sign = new Dictionary<string, string>();
                dic_Sign.Add("From", "2");
                dic_Sign.Add("GateType", "2");
                dic_Sign.Add("OutPutType", "2");
                dic_Sign.Add("PlatSpid", "1000077701");
                string platTimeStamp = CommQuery.GetTimeStamp();
                dic_Sign.Add("PlatTimeStamp", platTimeStamp);

                if (!string.IsNullOrEmpty(reqTextStr))
                {
                    dic_Sign.Add("ReqText", reqTextStr);
                }
                dic_Sign.Add("SeqNo", seq_no);

                if (!string.IsNullOrEmpty(uin))
                {
                    dic_Sign.Add("Uin", uin);
                }

                dic_Sign.Add("Ver", "1.0");
                dic_Sign.Add("key", "c0df1a64cb1431fb34f400db1d0ac3b4");//开发环境为:123456  ；e1674ed8b2d4e12b99a06cd48368369d
                string ptSign = GetPingTaiSign(dic_Sign); //"edf3ea3fd7d7610188acb1a7fc1433f8";   
                #endregion

                #region 拼接接口https://10.123.7.25/finance_pay/kf_auth_ocr_audit.fcgi?后的请求串
                //LogHelper.LogInfo("IdCardManualReviewService.Review,ptSign:" + ptSign);
                //标准格式
                //https://10.123.7.25/finance_pay/kf_auth_ocr_audit.fcgi?
                //From=2
                //&GateType=2
                //&OutPutType=2
                //&PlatSpid=1000077701  
                //&PlatTimeStamp=1471002616
                //&ReqText=dWluPTIwMTMxMTA3OTAyNDEzOUB3eC50ZW5wYXkuY29tJnVpZD0yOTk3MDg1MTUmc2VxX25vPTExNDcwOTkyNDkzMDAwMDAwMDEmY3JlZGl0X3NwaWQ9MTAwMDAwMDMmZnJvbnRfaW1hZ2U9MjAxNjA0MjUxNDI3NTkxNzAyMzExJmJhY2tfaW1hZ2U9MjAxNjA0MjUxNDI3NTkxNzAyMzEyJmF1ZGl0X3Jlc3VsdD0xJmF1ZGl0X2Vycm9yX2Rlcz1pbWFnZSBub3QgY2xlYXImYXVkaXRfb3BlcmF0b3I9aGVpZGl6aGFuZyZhdWRpdF90aW1lPTIwMTYtMDgtMTEgMTA6MDA6MDAmc2lnbj1lZGYzZWEzZmQ3ZDc2MTAxODhhY2IxYTdmYzE0MzNmOA==
                //&SeqNo=1471002616&Uin=201311079024139@wx.tenpay.com
                //&Ver=1.0
                //&Sign=db4182c43ce4ba347209cbae7b9a2c08
                StringBuilder sb_cgiString = new StringBuilder();
                sb_cgiString.Append("From=2");
                sb_cgiString.Append("&GateType=2");
                sb_cgiString.Append("&OutPutType=2");
                sb_cgiString.Append("&PlatSpid=1000077701");
                sb_cgiString.Append("&PlatTimeStamp=" + platTimeStamp + "");
                sb_cgiString.Append("&ReqText=" + reqTextStr + "");
                sb_cgiString.Append("&SeqNo=" + seq_no + "");
                sb_cgiString.Append("&Uin=" + uin.ToString() + "");
                sb_cgiString.Append("&Ver=1.0");
                sb_cgiString.Append("&Sign=" + ptSign + "");
                //LogHelper.LogInfo("IdCardManualReviewService.Review,sb_cgiString:" + sb_cgiString.ToString());
                #endregion

                #region 调用OCR客服审核接口 并对接口返回值进行处理
                IdCardManualReview idCardManualReviewDAL = new IdCardManualReview();                
                
                //调用平台接口返回结果 
                //{"PlatCode":"0","PlatMsg":"Request Accepted","RetText":"eyJyZXN1bHQiOiIxOTQ5MDIwMDA0IiwicmVzX2luZm8iOiJbMTk0OTAyMDAwNF3mgqjnmoTmk43kvZzlt7Lmj5DkuqTvvIzor7fnoa7orqTmmK/lkKblt7LnlJ/mlYjjgIIifQ==","SeqNo":"1471002616","Sign":"2AD13DC30C226F717C7A04F071FDDF0D"}                
                //当PlatCode=0时表示调用金融网关接口成功，业务逻辑返回包含在RetText中,将RetText用Base64解码得到业务逻辑返回结果
                //当PlatCode>0时表示调用金融网关接口失败，PlatMsg为调用失败的原因
                
                string reviewResult = string.Empty;
                bool isReviewSuccess = idCardManualReviewDAL.Review(sb_cgiString.ToString(), out reviewResult);
                LogHelper.LogInfo("IdCardManualReviewService.Review,reviewResult:" + reviewResult);
                if (isReviewSuccess)
                {
                    //平台调用接口返回结果                
                    var reviewResultJson = Newtonsoft.Json.JsonConvert.DeserializeObject(reviewResult) as Newtonsoft.Json.Linq.JObject;
                    if (reviewResultJson != null && reviewResultJson.Count > 0)
                    {
                        string platCode = reviewResultJson["PlatCode"].ToString();
                        if (platCode.Equals("0"))
                        {                            
                            //调用接口返回结果
                            string retText = reviewResultJson["RetText"].ToString();
                            //将RetText用Base64解码得到业务逻辑返回结果
                            //返回格式:{"result":"0","res_info":"ok"}                
                            //当result=0时表示业务处理成功
                            //当result!=0时表示业务处理失败,res_info为失败原因
                            string retTextDecodeBase64 = DecodeBase64(retText);

                            var retTextDecodeBase64Json = Newtonsoft.Json.JsonConvert.DeserializeObject(retTextDecodeBase64) as Newtonsoft.Json.Linq.JObject;
                            if (retTextDecodeBase64Json != null && retTextDecodeBase64Json.Count > 0)
                            {
                                string retTextResult = retTextDecodeBase64Json["result"].ToString();
                                result = retTextResult.Equals("0") ? true : false;
                                if (retTextResult.Equals("0"))
                                {
                                    msg = "实名认证系统更新成功。";
                                }
                                else if (retTextResult.Equals("99224024"))
                                {
                                    //这种是用户多次重传照片，征信那边对应好几张照片，跟实名系统那边存的有不一致，会发个版本优化下，减少这种情况，这种也先不审核
                                    msg = "实名认证系统更新失败:" + " OCR信息错误。";
                                }
                                else if (retTextResult.Equals("99224025"))
                                {
                                    //这种一般是征信接口超时了，征信那边成功了，实名系统那边没有数据，直接放弃不需要审核
                                    msg = "实名认证系统更新失败:" + " 数据库中无OCR记录。";
                                }
                                else if (retTextResult.Equals("99225006 "))
                                {
                                    //这种完全不需要处理，用户已经ocr成功了
                                    msg = "重入错误，用户OCR已成功。";
                                    result = true;
                                }
                                else
                                {
                                    msg = "实名认证系统更新失败:" + retTextDecodeBase64Json["res_info"].ToString();
                                }                                
                                LogHelper.LogInfo("IdCardManualReviewService.Review,retTextResult:" + retTextDecodeBase64Json.ToString());
                            }
                        }
                        else
                        {
                            result = false;
                            msg ="调用金融网关fcgi接口失败:"+ reviewResultJson["PlatMsg"].ToString();
                        }
                    }
                }
                else
                {
                    result = false;
                    msg = reviewResult;
                }
                #endregion
            }
            catch (Exception err)
            {
                LogHelper.LogInfo("IdCardManualReviewService.Review:" + err.Message);
                result = false;
                msg = "审核出错：" + err.Message.ToString();                
            }
            return result;
        }

        public bool ReviewByRelay(string uin, string seq_no, string credit_spid, string front_image, string back_image, int audit_result, string audit_error_des, string audit_operator, string audit_time, out string msg)
        {
            //LogHelper.LogInfo(string.Format("IdCardManualReviewService.Review,uin={0},uid={1},seq_no={2},credit_spid={3},front_image={4},back_image={5},audit_result={6},audit_error_des={7},audit_operator={8},audit_time={9}", uin, uid, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time));            
            msg = "";
            bool result = false;
            try
            {                           
                #region 接口签名
                //接口签名md5($uin$seq_no$credit_spid$front_image$back_image$audit_result$audit_error_des$audit_operator$audit_time$key)开发联调环境key: 1234
                StringBuilder sb_sign = new StringBuilder();
                sb_sign.Append(uin.ToString());
                if (!string.IsNullOrEmpty(seq_no))
                {
                    sb_sign.Append(seq_no.ToString());//seq_no
                }
                if (!string.IsNullOrEmpty(credit_spid))
                {
                    sb_sign.Append(credit_spid.ToString());//credit_spid
                }
                if (!string.IsNullOrEmpty(front_image))
                {
                    sb_sign.Append(front_image.ToString());//front_image
                }
                if (!string.IsNullOrEmpty(back_image))
                {
                    sb_sign.Append(back_image.ToString());//back_image
                }
                if (audit_result > 0)
                {
                    sb_sign.Append(audit_result.ToString());//audit_result
                }
                if (!string.IsNullOrEmpty(audit_error_des))
                {
                    sb_sign.Append(audit_error_des.ToString());//audit_error_des
                }
                if (!string.IsNullOrEmpty(audit_operator))
                {
                    sb_sign.Append(audit_operator.ToString());//audit_operator
                }
                if (!string.IsNullOrEmpty(audit_time))
                {
                    sb_sign.Append(DateTime.Parse(audit_time).ToString("yyyy-MM-dd HH:mm:ss"));//audit_time
                }                
                sb_sign.Append("e1674ed8b2d4e12b99a06cd48368369d");//key //开发联调环境key: 12345
                LogHelper.LogInfo("IdCardManualReviewService.Review,sb_sign:" + sb_sign.ToString());
                string sb_sign_ToUTF8 = ToUTF8(sb_sign.ToString());
                string sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb_sign_ToUTF8, "MD5").ToLower();// GetReviewSign(dic); //"edf3ea3fd7d7610188acb1a7fc1433f8";
                LogHelper.LogInfo("IdCardManualReviewService.Review,sign:" + sign);
                #endregion

                #region 业务签名
                StringBuilder sb_reqString = new StringBuilder();
                sb_reqString.AppendFormat("uin={0}", uin.ToString());
                sb_reqString.AppendFormat("&seq_no={0}", seq_no);
                sb_reqString.AppendFormat("&credit_spid={0}", credit_spid);
                sb_reqString.AppendFormat("&front_image={0}", front_image);
                sb_reqString.AppendFormat("&back_image={0}", back_image);
                sb_reqString.AppendFormat("&audit_result={0}", audit_result);
                sb_reqString.AppendFormat("&audit_error_des={0}", audit_error_des);
                sb_reqString.AppendFormat("&audit_operator={0}", audit_operator);
                sb_reqString.AppendFormat("&audit_time={0}", DateTime.Parse(audit_time).ToString("yyyy-MM-dd HH:mm:ss"));
                sb_reqString.AppendFormat("&sign={0}", sign);

                byte[] bytes = Encoding.Default.GetBytes(sb_reqString.ToString());
                string reqTextStr = Convert.ToBase64String(bytes);
                //LogHelper.LogInfo("IdCardManualReviewService.Review,reqTextStr:" + reqTextStr);
                //sb_cgiString.Append("&ReqText=dWluPTIwMTMxMTA3OTAyNDEzOUB3eC50ZW5wYXkuY29tJnVpZD0yOTk3MDg1MTUmc2VxX25vPTExNDcwOTkyNDkzMDAwMDAwMDEmY3JlZGl0X3NwaWQ9MTAwMDAwMDMmZnJvbnRfaW1hZ2U9MjAxNjA0MjUxNDI3NTkxNzAyMzExJmJhY2tfaW1hZ2U9MjAxNjA0MjUxNDI3NTkxNzAyMzEyJmF1ZGl0X3Jlc3VsdD0xJmF1ZGl0X2Vycm9yX2Rlcz1pbWFnZSBub3QgY2xlYXImYXVkaXRfb3BlcmF0b3I9aGVpZGl6aGFuZyZhdWRpdF90aW1lPTIwMTYtMDgtMTEgMTA6MDA6MDAmc2lnbj1lZGYzZWEzZmQ3ZDc2MTAxODhhY2IxYTdmYzE0MzNmOA==");
                #endregion

                #region //平台签名：参数URL串+平台商户key的MD5值（参数URL串要求：按参数名ASCII顺序，为空的参数不参与签名）
                //调用实名系统的银行接口 申请的客服商户号：1000077701，对应的密钥：c0df1a64cb1431fb34f400db1d0ac3b4
                Dictionary<string, string> dic_Sign = new Dictionary<string, string>();
                dic_Sign.Add("From", "2");
                dic_Sign.Add("GateType", "2");
                dic_Sign.Add("OutPutType", "2");
                dic_Sign.Add("PlatSpid", "1000077701");
                string platTimeStamp = CommQuery.GetTimeStamp();
                dic_Sign.Add("PlatTimeStamp", platTimeStamp);//"1471002616"

                if (!string.IsNullOrEmpty(reqTextStr))
                {
                    dic_Sign.Add("ReqText", reqTextStr);
                }
                dic_Sign.Add("SeqNo", seq_no);//"1471002616"

                if (!string.IsNullOrEmpty(uin))
                {
                    dic_Sign.Add("Uin", uin);
                }

                dic_Sign.Add("Ver", "1.0");
                dic_Sign.Add("key", "c0df1a64cb1431fb34f400db1d0ac3b4");//开发环境为:123456  ；e1674ed8b2d4e12b99a06cd48368369d
                string ptSign = GetPingTaiSign(dic_Sign); //"edf3ea3fd7d7610188acb1a7fc1433f8";   
                #endregion

                #region 拼接接口https://10.123.7.25/finance_pay/kf_auth_ocr_audit.fcgi?后的请求串
                //LogHelper.LogInfo("IdCardManualReviewService.Review,ptSign:" + ptSign);
                //标准格式
                //https://10.123.7.25/finance_pay/kf_auth_ocr_audit.fcgi?
                //From=2
                //&GateType=2
                //&OutPutType=2
                //&PlatSpid=1000077701  
                //&PlatTimeStamp=1471002616
                //&ReqText=dWluPTIwMTMxMTA3OTAyNDEzOUB3eC50ZW5wYXkuY29tJnVpZD0yOTk3MDg1MTUmc2VxX25vPTExNDcwOTkyNDkzMDAwMDAwMDEmY3JlZGl0X3NwaWQ9MTAwMDAwMDMmZnJvbnRfaW1hZ2U9MjAxNjA0MjUxNDI3NTkxNzAyMzExJmJhY2tfaW1hZ2U9MjAxNjA0MjUxNDI3NTkxNzAyMzEyJmF1ZGl0X3Jlc3VsdD0xJmF1ZGl0X2Vycm9yX2Rlcz1pbWFnZSBub3QgY2xlYXImYXVkaXRfb3BlcmF0b3I9aGVpZGl6aGFuZyZhdWRpdF90aW1lPTIwMTYtMDgtMTEgMTA6MDA6MDAmc2lnbj1lZGYzZWEzZmQ3ZDc2MTAxODhhY2IxYTdmYzE0MzNmOA==
                //&SeqNo=1471002616&Uin=201311079024139@wx.tenpay.com
                //&Ver=1.0
                //&Sign=db4182c43ce4ba347209cbae7b9a2c08
                StringBuilder sb_cgiString = new StringBuilder();
                sb_cgiString.Append("From=2");
                sb_cgiString.Append("&GateType=2");
                sb_cgiString.Append("&OutPutType=2");
                sb_cgiString.Append("&PlatSpid=1000077701");
                sb_cgiString.Append("&PlatTimeStamp=" + platTimeStamp + "");
                sb_cgiString.Append("&ReqText=" + reqTextStr + "");
                sb_cgiString.Append("&SeqNo=" + seq_no + "");
                sb_cgiString.Append("&Uin=" + uin.ToString() + "");
                sb_cgiString.Append("&Ver=1.0");
                sb_cgiString.Append("&Sign=" + ptSign + "");
                //LogHelper.LogInfo("IdCardManualReviewService.Review,sb_cgiString:" + sb_cgiString.ToString());
                #endregion

                #region 调用OCR客服审核接口 并对接口返回值进行处理
                IdCardManualReview idCardManualReviewDAL = new IdCardManualReview();
                //调用平台接口返回结果 
                //{"PlatCode":"0","PlatMsg":"Request Accepted","RetText":"eyJyZXN1bHQiOiIxOTQ5MDIwMDA0IiwicmVzX2luZm8iOiJbMTk0OTAyMDAwNF3mgqjnmoTmk43kvZzlt7Lmj5DkuqTvvIzor7fnoa7orqTmmK/lkKblt7LnlJ/mlYjjgIIifQ==","SeqNo":"1471002616","Sign":"2AD13DC30C226F717C7A04F071FDDF0D"}                
                //当PlatCode=0时表示调用金融网关接口成功，业务逻辑返回包含在RetText中,将RetText用Base64解码得到业务逻辑返回结果
                //当PlatCode>0时表示调用金融网关接口失败，PlatMsg为调用失败的原因
                
                string reviewResult = string.Empty;
                reviewResult = idCardManualReviewDAL.ReviewByRelay(sb_cgiString.ToString());
                LogHelper.LogInfo("IdCardManualReviewService.Review,reviewResult:" + reviewResult);
                if (!string.IsNullOrEmpty(reviewResult))
                {
                    //平台调用接口返回结果                
                    var reviewResultJson = Newtonsoft.Json.JsonConvert.DeserializeObject(reviewResult) as Newtonsoft.Json.Linq.JObject;
                    if (reviewResultJson != null && reviewResultJson.Count > 0)
                    {
                        string platCode = reviewResultJson["PlatCode"].ToString();
                        if (platCode.Equals("0"))
                        {
                            //调用接口返回结果
                            string retText = reviewResultJson["RetText"].ToString();
                            //将RetText用Base64解码得到业务逻辑返回结果
                            //返回格式:{"result":"0","res_info":"ok"}                
                            //当result=0时表示业务处理成功
                            //当result!=0时表示业务处理失败,res_info为失败原因
                            string retTextDecodeBase64 = DecodeBase64(retText);

                            var retTextDecodeBase64Json = Newtonsoft.Json.JsonConvert.DeserializeObject(retTextDecodeBase64) as Newtonsoft.Json.Linq.JObject;
                            if (retTextDecodeBase64Json != null && retTextDecodeBase64Json.Count > 0)
                            {
                                string retTextResult = retTextDecodeBase64Json["result"].ToString();
                                result = retTextResult.Equals("0") ? true : false;                                
                                if (retTextResult.Equals("0"))
                                {
                                    msg = "实名认证系统更新成功。";
                                }
                                else if (retTextResult.Equals("99224024"))
                                {
                                    //这种是用户多次重传照片，征信那边对应好几张照片，跟实名系统那边存的有不一致，会发个版本优化下，减少这种情况，这种也先不审核
                                    msg = "实名认证系统更新失败:" + " OCR信息错误。";
                                }
                                else if (retTextResult.Equals("99224025"))
                                {
                                    //这种一般是征信接口超时了，征信那边成功了，实名系统那边没有数据，直接放弃不需要审核
                                    msg = "实名认证系统更新失败:" + " 数据库中无OCR记录。";
                                }
                                else if (retTextResult.Equals("99225006 "))
                                {
                                    //这种完全不需要处理，用户已经ocr成功了
                                    msg = "重入错误，用户OCR已成功。";
                                    result = true;
                                }
                                else
                                {
                                    msg = "实名认证系统更新失败:" + retTextDecodeBase64Json["res_info"].ToString();
                                } 
                                LogHelper.LogInfo("IdCardManualReviewService.Review,retTextResult:" + retTextDecodeBase64Json.ToString());
                            }
                        }
                        else
                        {
                            result = false;
                            msg = "调用金融网关fcgi接口失败:" + reviewResultJson["PlatMsg"].ToString();
                        }
                    }                    
                }
                else
                {
                    result = false;
                    msg = "调用金融网关fcgi接口失败" ;
                }                
                #endregion
            }
            catch (Exception err)
            {
                LogHelper.LogInfo("IdCardManualReviewService.Review:" + err.Message);
                result = false;
                msg = "审核出错：" + err.Message.ToString();
            }
            return result;
        }
      

        public string GetReviewSign(Dictionary<string, string> dic)
        {
            string sign = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in dic)
                {
                    sb.Append(item.Value);
                }
                string result = ToUTF8(sb.ToString());
                sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(result, "MD5").ToLower();
            }
            catch (Exception ex)
            {
                sign = string.Empty;
            }
            return sign;
        }

        /// <summary>
        /// 平台签名
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public string GetPingTaiSign(Dictionary<string, string> dic)
        {
            string sign = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in dic)
                {
                    sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
                }
                string result = sb.ToString().TrimEnd('&');
                string result3 = ToUTF8(result);
                sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(result3, "MD5").ToLower();

            }
            catch (Exception ex)
            {
                sign = string.Empty;
            }
            return sign;
        }

        public static string ToUTF8(string myString)
        {
            byte[] bytes = Encoding.Default.GetBytes(myString);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        public List<string> GetYearMonthList(string yearMonthFrom, string yearMonthTo, int type)
        {
            List<string> list = new List<string>();
            DateTime time = DateTime.Parse(yearMonthFrom + "-01");
            DateTime time2 = DateTime.Parse(yearMonthTo + "-01");
            DateTime time3 = new DateTime();
            if (type == 1)
            {
                for (time3 = time; time3 <= time2; time3 = time3.AddMonths(1))
                {
                    list.Add(time3.ToString("yyyy年MM月"));
                }
                return list;
            }
            if (type == 2)
            {
                for (time3 = time; time3 <= time2; time3 = time3.AddMonths(1))
                {
                    list.Add(time3.ToString("yyyy年M月"));
                }
                return list;
            }
            if (type == 3)
            {
                for (time3 = time; time3 <= time2; time3 = time3.AddMonths(1))
                {
                    list.Add(time3.ToString("yyyy-MM"));
                }
                return list;
            }
            if (type == 4)
            {
                for (time3 = time; time3 <= time2; time3 = time3.AddMonths(1))
                {
                    list.Add(time3.ToString("yyyy-M"));
                }
            }
            if (type == 5)
            {
                for (time3 = time; time3 <= time2; time3 = time3.AddMonths(1))
                {
                    list.Add(time3.ToString("yyyyMM"));
                }
            }
            return list;
        }
    }
}

