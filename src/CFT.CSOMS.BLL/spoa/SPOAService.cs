using System;
using System.Collections.Generic;
using System.Linq;
using CFT.CSOMS.DAL.SPOA;
using System.Text;
using System.Data;

namespace CFT.CSOMS.BLL.SPOA
{
    public class SPOAService
    {
        /// <summary>
        /// 收付易查询
        /// </summary>
        /// <param name="qqid">财付通账号</param>
        /// <returns></returns>
        public DataSet GetShouFuYiList(string qqid)
        {
            DataSet ds = null;

            try
            {
                if (string.IsNullOrEmpty(qqid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }

                ds = new SPOAData().GetShouFuYiList(qqid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }

        /// <summary>
        /// 通过账号或域名查询--中介商户查询
        /// </summary>
        /// <param name="Fqqid">财付通账号</param>
        /// <param name="Fdomain">网址</param>
        /// <returns></returns>
        public DataSet GetAgencyBusinessList(string Fqqid, string Fdomain, int offset, int count)
        {
            DataSet ds = null;

            try
            {
                ds = new SPOAData().GetAgencyBusinessList(Fqqid, Fdomain, offset, count);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        /// <summary>
        /// 根据商户号查中介商户资料--商户资料修改
        /// </summary>
        /// <param name="Fspid">商户号</param>
        /// <returns></returns>
        public DataSet GetAgencyBusinessInfoList(string Fspid)
        {
            DataSet ds = null;

            try
            {
                if (string.IsNullOrEmpty(Fspid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }

                ds = new SPOAData().GetAgencyBusinessInfoList(Fspid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        /// <summary>
        /// 中介商户查询单笔--中介商户查询
        /// </summary>
        /// <param name="Fspid">商户号</param>
        /// <returns></returns>
        public DataSet GetAgencyBusinessInfo(string Fid)
        {
            DataSet ds = null;

            try
            {
                if (string.IsNullOrEmpty(Fid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }

                ds = new SPOAData().GetAgencyBusinessInfo(Fid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        public DataSet QueryAgencyBySpid(string spid)
        {
            DataSet ds = null;

            try
            {
                if (string.IsNullOrEmpty(spid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }

                ds = new SPOAData().QueryAgencyBySpid(spid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        /// <summary>
        /// 通过ID查中介商户详情函数
        /// </summary>
        /// <param name="Fid"></param>
        /// <returns></returns>
        public DataSet QueryAgencyInfoById(string Fid) 
        {
            DataSet ds = null;

            try
            {
                if (string.IsNullOrEmpty(Fid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }

                ds = new SPOAData().QueryAgencyInfoById(Fid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        /// <summary>
        /// 查询商户历史修改记录--商户资料修改
        /// </summary>
        /// <param name="Fspid">商户号</param>
        /// <returns></returns>
        public DataSet GetHisBusinessList(string Fspid)
        {
            DataSet ds = null;

            try
            {
                if (string.IsNullOrEmpty(Fspid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }

                ds = new SPOAData().GetHisBusinessList(Fspid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        /// <summary>
        /// 查询商户历史修改记录--联系人手机资料修改
        /// </summary>
        /// <param name="Fspid">商户号</param>
        /// <returns></returns>
        public DataSet QueryApplyListBySpid(string Fspid)
        {
            DataSet ds = null;

            try
            {
                if (string.IsNullOrEmpty(Fspid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }

                ds = new SPOAData().QueryApplyListBySpid(Fspid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        #region 自助商户领单

        /// <summary>
        /// 商户接入状态查询--自助商户领单
        /// </summary>
        /// <returns></returns>
        public DataSet GetSelfTypeList()
        {
            DataSet ds = null;

            try
            {
                ds = new SPOAData().GetSelfTypeList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }
        /// <summary>
        /// 所属客服查询--自助商户领单
        /// </summary>
        /// <returns></returns>
        public DataSet GetSelfKFList()
        {
            DataSet ds = null;

            try
            {
                ds = new SPOAData().GetSelfKFList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }
        /// <summary>
        /// 自助和BD商户列表记录数--自助商户领单
        /// <param name="filter">where查询条件</param>
        /// </summary>
        /// <returns></returns>
        public int GetSelfQueryListCount(string SPID, int? DraftFlag, string CompanyName, int? Flag, string WWWAdress, string Appid, DateTime? ApplyTimeStart, DateTime? ApplyTimeEnd, string BankUserName, string KFCheckUser, string SuggestUser, string MerType)
        {
            
            try
            {
                return new SPOAData().GetSelfQueryListCount(SPID, DraftFlag, CompanyName, Flag, WWWAdress, Appid, ApplyTimeStart, ApplyTimeEnd, BankUserName, KFCheckUser, SuggestUser, MerType);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        /// <summary>
        /// 自助和BD商户列表记录数--自助商户领单
        /// <param name="filter">where查询条件</param>
        /// <param name="TopCount"></param>
        /// <param name="NotInCount"></param>
        /// </summary>
        /// <returns></returns>
        public DataSet GetSelfQueryList(string SPID, int? DraftFlag, string CompanyName, int? Flag, string WWWAdress, string Appid,
            DateTime? ApplyTimeStart, DateTime? ApplyTimeEnd, string BankUserName, string KFCheckUser, string SuggestUser, string MerType, int topCount, int notInCount)
        {
            DataSet ds = null;

            try
            {
                ds = new SPOAData().GetSelfQueryList(SPID, DraftFlag, CompanyName, Flag, WWWAdress, Appid, ApplyTimeStart, ApplyTimeEnd, BankUserName, KFCheckUser, SuggestUser, MerType, topCount, notInCount);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }
        /// <summary>
        /// 自助和BD商户列表查询--自助商户领单
        /// <param name="ApplyCpInfoID">唯一ID</param>
        /// </summary>
        /// <returns></returns>
        public DataSet GetSelfQueryInfo(string ApplyCpInfoID)
        {
            DataSet ds = null;

            try
            {
                if (string.IsNullOrEmpty(ApplyCpInfoID))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }

                ds = new SPOAData().GetSelfQueryInfo(ApplyCpInfoID);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }
        /// <summary>
        /// 自助领单--自助商户领单
        /// <param name="ApplyCpInfoID">唯一ID</param>
        /// <param name="UserID">操作员</param>
        /// </summary>
        /// <returns></returns>
        public void CheckTicket(string ApplyCpInfoID, string UserID)
        {
            try
            {
                if (string.IsNullOrEmpty(ApplyCpInfoID))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }
                if (string.IsNullOrEmpty(UserID))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }
                new SPOAData().CheckTicket(ApplyCpInfoID, UserID);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 自助和BD商户列表查询--自助商户领单
        /// <param name="ApplyCpInfoID">唯一ID</param>
        /// </summary>
        /// <returns></returns>
        public DataSet GetApplyCpInfoXByID(string ApplyCpInfoID)
        {
            DataSet ds = null;

            try
            {
                if (string.IsNullOrEmpty(ApplyCpInfoID))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }

                ds = new SPOAData().GetApplyCpInfoXByID(ApplyCpInfoID);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }

        /// <summary>
        /// 自助领单--自助商户领单
        /// <param name="ApplyCpInfoID">唯一ID</param>
        /// <param name="UserID">操作员</param>
        /// </summary>
        /// <returns></returns>
        public void ApproveTicket(string ApplyCpInfoID, string UserID, int Type, string Reason)
        {
            try
            {
                if (string.IsNullOrEmpty(ApplyCpInfoID))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }
                if (string.IsNullOrEmpty(UserID))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }
                new SPOAData().ApproveTicket(ApplyCpInfoID, UserID, Type, Reason);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        public string GetTradeType(string spid)
        {
            try
            {
                if (string.IsNullOrEmpty(spid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }
                return new SPOAData().GetTradeType(spid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string GetWWWAddress(string spid)
        {
            try
            {
                if (string.IsNullOrEmpty(spid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }
                return new SPOAData().GetWWWAddress(spid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string GetBDName(string spid)
        {
            try
            {
                if (string.IsNullOrEmpty(spid))
                {
                    throw new ArgumentNullException("参数不能为空！");
                }
                return new SPOAData().GetBDName(spid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet QueryBussFreezeList(string spid, string type, string state)
        {
            if (string.IsNullOrEmpty(spid))
            {
                throw new ArgumentNullException("参数不能为空！");
            }
            return new SPOAData().QueryBussFreezeList(spid, type, state);
        }

        public void BusinessIdentityCardNum(string Fspid, string OldIdentityCardNum, string NewIdentityCardNum, string IDImage, string ElseImage, string UserName, string Reason)
        {
            if (string.IsNullOrEmpty(Fspid))
            {
                throw new ArgumentNullException("参数不能为空！");
            }
            new SPOAData().BusinessIdentityCardNum(Fspid, OldIdentityCardNum, NewIdentityCardNum, IDImage, ElseImage, UserName, Reason);
        }

        public void ModifyPayBusinessInfo(string TableFlag, string KeyID, string ContactUser, string ContactPhone, string ContactMobile, string ContactQQ, string ContactEmail, string CompanyAddress, string Postalcode)
        {
            new SPOAData().ModifyPayBusinessInfo(TableFlag, KeyID, ContactUser, ContactPhone, ContactMobile, ContactQQ, ContactEmail, CompanyAddress, Postalcode);
        }

        public void SubmitBusinessInfo(string UserName, string Fspid, string OldFspName, string NewFspName, string OldEmail, string NewEmail, string OldAddress, string NewAddress, string ApplyResult, string[] FileInfos) 
        {
            new SPOAData().SubmitBusinessInfo(UserName, Fspid, OldFspName, NewFspName, OldEmail, NewEmail, OldAddress, NewAddress, ApplyResult, FileInfos);
        }
        public string SpidMobileApply(string ApplyUser,string spid, string newMobile, string file_idCard, string file_MobileFile, string memo)
        {
            return new SPOAData().SpidMobileApply(ApplyUser, spid, newMobile, file_idCard, file_MobileFile, memo);
        }
        public DataSet GetMspAmendTaskByID(string TaskId)
        {
            return new SPOAData().GetMspAmendTaskByID(TaskId);
        }
        public void UpdateMspAmendTaskByTaskid(string TaskId, int AmendState, string UserId)
        {
            new SPOAData().UpdateMspAmendTaskByTaskid(TaskId, AmendState, UserId);
        }
        public void UpdateSpidDomainApplyByTaskid(string TaskId, string Reason)
        {
            new SPOAData().UpdateSpidDomainApplyByTaskid(TaskId, Reason);
        }
        public void UpdateMspAmendInfoByTaskid(string TaskId, string Reason)
        {
            new SPOAData().UpdateMspAmendInfoByTaskid(TaskId, Reason);
        }

        public DataSet GetAllValueAddedTax(string Spid, string CompanyName, int topCount, int notInCount)
        {
            return new SPOAData().GetAllValueAddedTax(Spid, CompanyName, topCount, notInCount);
        }
        public DataSet GetOneValueAddedTax(string Spid)
        {
            return new SPOAData().GetOneValueAddedTax(Spid);
        }
        public void ValueAddedTaxModify(string Spid, int Flag)
        {
            new SPOAData().ValueAddedTaxModify(Spid, Flag);
        }

        public DataSet GetApplyValueAddedTax(string Spid, string Flags, int topCount, int notInCount)
        {
            return new SPOAData().GetApplyValueAddedTax(Spid, Flags, topCount, notInCount);
        }
        public DataSet GetValueAddedTaxDetail(string taskid)
        {
            if (string.IsNullOrEmpty(taskid))
            {
                throw new ArgumentNullException("参数不能为空！");
            }
            return new SPOAData().GetValueAddedTaxDetail(taskid);
        }
        public void ValueAddedTaxApprove(string taskid, string Memo, string imgTaxCert, string imgBizLicenseCert, string imgAuthorizationCert, string UserName)
        {
            new SPOAData().ValueAddedTaxApprove(taskid, Memo, imgTaxCert, imgBizLicenseCert, imgAuthorizationCert, UserName);
        }

        public void ValueAddedTaxCancel(string taskid, string spid, string Memo, string UserName)
        {
            new SPOAData().ValueAddedTaxCancel(taskid, spid, Memo, UserName);
        }

        public DataSet GetSpidDomainQueryListCount(string filter)
        {
            return new SPOAData().GetSpidDomainQueryListCount(filter);
        }
        public DataSet GetSpidDomainQueryList(string Spid, string CompanyName, DateTime? ApplyTimeStart, DateTime? ApplyTimeEnd, int? AmendState, string submitType, int topCount, int notInCount)
        {
            return new SPOAData().GetSpidDomainQueryList(Spid, CompanyName, ApplyTimeStart, ApplyTimeEnd, AmendState, submitType, topCount, notInCount);
        }


        /// <summary>
        /// 商户信息修改
        /// </summary>
        /// <param name="spid">商户号</param>
        /// <param name="type">修改类型</param>
        /// <param name="caccounts">C账号</param>
        /// <returns></returns>
        public DataSet QueryAmendMspInfo(string spid, string type, string caccounts)
        {
            DataSet ds = null;

            try
            {
                ds = new SPOAData().QueryAmendMspInfo(spid, type, caccounts);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }

        /// <summary>
        /// 直付商户查询函数
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="limit">总数</param>
        /// <param name="offset">起始数</param>
        /// <returns></returns>
        public DataSet GetSpInfo(string SPID, string ApplyCpInfoID, string CompanyName, string WWWAdress, string WebName, string AppID, int topCount, int notInCount)

        {
            DataSet ds = null;

            try
            {
                ds = new SPOAData().GetSpInfo(SPID, ApplyCpInfoID, CompanyName, WWWAdress, WebName, AppID, topCount, notInCount);

            }
            catch (Exception e)
            {
                throw new Exception("直付商户查询异常：" + e.Message);
            }
            return ds;
        }

        public DataSet GetBDSpidList()
        {
            DataSet ds = null;

            try
            {
                ds = new SPOAData().GetBDSpidList();
            }
            catch (Exception e)
            {
                throw new Exception("合作形式列表查询异常：" + e.Message);
            }
            return ds;
        }

        public DataSet GetCheckInfo(string spid, int checktype)
        {
            DataSet ds = null;

            try
            {
                ds = new SPOAData().GetCheckInfo(spid, checktype);
            }
            catch (Exception e)
            {
                throw new Exception("审批信息查询异常：" + e.Message);
            }
            return ds;
        }

        /// <summary>
        /// 重发邮件通知
        /// </summary>
        /// <param name="resultstr"></param>
        /// <param name="spid">商户号</param>
        /// <param name="applyUser">登录人的全名</param>
        /// <returns></returns>
        public bool ReSendEmail_ToSP(out string resultstr, string spid, string applyUser)
        {
            resultstr = "";
             try
            {
                return new SPOAData().ReSendEmail_ToSP(out resultstr, spid, applyUser);
            }
             catch (Exception e)
             {
                 throw new Exception("重发邮件通知异常：resultstr:" + resultstr+"  "+e.Message);
             }
        }

        /// <summary>
        /// 重发证书
        /// </summary>
        /// <param name="resultstr"></param>
        /// <param name="spid">商户号</param>
        /// <param name="applyUser">登录人的全名</param>
        /// <returns></returns>
        public bool ReSendCertificate(out string resultstr, string spid, string applyUser)
        {
            resultstr = "";
            try
            {
                return new SPOAData().ReSendCertificate(out resultstr, spid, applyUser);
            }
            catch (Exception e)
            {
                throw new Exception(" 重发证书异常：resultstr:" + resultstr + "  " + e.Message);
            }
        }
        /// <summary>
        /// 重发秘钥
        /// </summary>
        /// <param name="spid"></param>
        /// <returns></returns>
        public string SendSPMerkey(string spid)
        {
            try
            {
                return new SPOAData().SendSPMerkey(spid);
            }
            catch (Exception e)
            {
                throw new Exception("重发证书异常：" + e.ToString());
            }
        }

        /// <summary>
        /// 商户证书到期查询
        /// </summary>
        /// <param name="begindate">证书到期开始时间</param>
        /// <param name="enddate">证书到期结束时间</param>
        /// <param name="spid">商户号</param>
        /// <returns></returns>
        public DataSet QueryExpiredCertificate(string begindate, string enddate, string spid, int topCount, int notInCount)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = new SPOAData().QueryExpiredCertificate(begindate, enddate, spid, topCount, notInCount);
            }
            catch (Exception e)
            {
                throw new Exception("商户证书到期查询异常：" + e.Message);
            }
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetSelfQuerySPType() 
        {
            DataSet ds = new DataSet();
            try
            {
                ds = new SPOAData().GetSelfQuerySPType();
            }
            catch (Exception e)
            {
                
            }
            return ds;
        }
        /// <summary>
        /// 关闭支付
        /// </summary>
        /// <param name="fspid">商户号</param>
        /// <param name="opuser">操作人</param>
        /// <param name="freson">关闭原因</param>
        /// <returns>返回结果[0为成功执行]</returns>
        public string ClosePay(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().ClosePay(fspid, opuser, freson);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 冻结商户结算合同
        /// </summary>
        /// <param name="fspid"></param>
        /// <param name="opuser"></param>
        /// <param name="freson"></param>
        /// <returns></returns>
        public string FreezeSpid(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().FreezeSpid(fspid, opuser, freson);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 商户挂失
        /// </summary>
        /// <param name="fspid">商户号</param>
        /// <param name="opuser">操作人</param>
        /// <param name="freson">挂失原因</param>
        /// <returns>返回结果[0为成功执行]</returns>
        public string LostOfSpid(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().LostOfSpid(fspid, opuser, freson);
            }
            catch (Exception e) 
            {
                throw e;
            }
        }

        /// <summary>
        /// 关闭中介
        /// </summary>
        /// <param name="fspid">商户号</param>
        /// <param name="opuser">操作人</param>
        /// <param name="freson">关闭原因</param>
        /// <returns>返回结果[0为成功执行]</returns>
        public string CloseAgency(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().CloseAgency(fspid, opuser, freson);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 关闭退款
        /// </summary>
        /// <param name="fspid">商户号</param>
        /// <param name="opuser">操作人</param>
        /// <param name="freson">关闭原因</param>
        /// <returns>返回结果[0为成功执行]</returns
        public string CloseRefund(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().CloseRefund(fspid, opuser, freson);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 开通退款
        /// </summary>
        /// <param name="fspid">商户号</param>
        /// <param name="opuser">操作人</param>
        /// <param name="freson">开通原因</param>
        /// <returns>返回结果[0为成功执行]</returns>
        public string OpenRefund(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().OpenRefund(fspid, opuser, freson);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 恢复[冻结|挂失]
        /// </summary>
        /// <param name="fspid">商户号</param>
        /// <param name="opuser">操作人</param>
        /// <param name="freson">恢复原因</param>
        /// <returns>返回结果[0为成功执行]</returns>
        public string RestoreOfSpid(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().RestoreOfSpid(fspid, opuser, freson);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 商户注销申请
        /// </summary>
        /// <param name="fspid">商户号</param>
        /// <param name="opuser">操作人</param>
        /// <param name="freson">恢复原因</param>
        /// <returns>返回结果[0为成功执行]</returns>
        public string BusinessLogout(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().BusinessLogout(fspid, opuser, freson);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 开通付款
        /// </summary>
        /// <param name="fspid"></param>
        /// <param name="opuser"></param>
        /// <param name="freson"></param>
        /// <returns></returns>
        public string OpenPay(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().OpenPay(fspid, opuser, freson);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string OpenAgency(string fspid, string opuser, string freson)
        {
            try
            {
                return new SPOAData().OpenAgency(fspid, opuser, freson);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }

}
       
   
