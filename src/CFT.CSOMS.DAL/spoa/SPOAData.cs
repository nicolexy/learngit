using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CFT.CSOMS.DAL.SPOA
{
    public class SPOAData
    {

        public DataSet GetShouFuYiList(string qqid) 
        {
            if (string.IsNullOrEmpty(qqid)) 
            {
                throw new ArgumentNullException("参数不能为空！");
            }

            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            
            return cli.GetShouFuYiList(qqid);
        }

        public DataSet GetAgencyBusinessList(string Fqqid, string Fdomain, int offset, int count) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetAgencyBusinessList(Fqqid, Fdomain, offset, count);
        }
        public DataSet GetAgencyBusinessInfo(string Fid)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetAgencyBusinessInfo(Fid);
        }
        public DataSet GetAgencyBusinessInfoList(string Fspid)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetAgencyBusinessInfoList(Fspid);
        }

        public DataSet  QueryAgencyBySpid(string Fspid)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.QueryAgencyBySpid(Fspid);
        }
        public DataSet QueryAgencyInfoById(string Fid)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.QueryAgencyInfoById(Fid);
        }
        public DataSet GetHisBusinessList(string Fspid)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetHisBusinessList(Fspid);
        }

        #region 自助商户领单
        public DataSet GetSelfTypeList()
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetSelfTypeList();
        }
        public DataSet GetSelfKFList()
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetSelfKFList();
        }
        public int GetSelfQueryListCount(string SPID, int? DraftFlag, string CompanyName, int? Flag, string WWWAdress, string Appid, DateTime? ApplyTimeStart, DateTime? ApplyTimeEnd, string BankUserName, string KFCheckUser, string SuggestUser, string MerType)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetSelfQueryListCount(SPID,DraftFlag,CompanyName,Flag,WWWAdress,Appid,ApplyTimeStart,ApplyTimeEnd,BankUserName,KFCheckUser,SuggestUser,MerType);
            
        }
        public DataSet GetSelfQueryList(string SPID, int? DraftFlag, string CompanyName, int? Flag,string WWWAdress, string Appid,
            DateTime? ApplyTimeStart, DateTime? ApplyTimeEnd, string BankUserName, string KFCheckUser, string SuggestUser, string MerType, int topCount, int notInCount)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetSelfQueryList(SPID, DraftFlag, CompanyName,Flag,WWWAdress,  Appid, ApplyTimeStart,  ApplyTimeEnd,  BankUserName,  KFCheckUser,  SuggestUser,  MerType,  topCount,  notInCount);
        }
        public DataSet GetSelfQueryInfo(string ApplyCpInfoID)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetSelfQueryInfo(ApplyCpInfoID);
        }

        public void CheckTicket(string ApplyCpInfoID, string UserID) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.CheckTicket(ApplyCpInfoID, UserID);
        }

        public void ApproveTicket(string ApplyCpInfoID, string UserID, int Type, string Reason)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.ApproveTicket(ApplyCpInfoID, UserID, Type, Reason);
        }
        public DataSet GetApplyCpInfoXByID(string ApplyCpInfoID) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetApplyCpInfoXByID(ApplyCpInfoID);
        }
        #endregion

        public string GetTradeType(string spid) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetTradeType(spid);
        }

        public string GetWWWAddress(string spid) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetWWWAddress(spid);
        }

        public string GetBDName(string spid)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetBDName(spid);
        }

        public DataSet QueryBussFreezeList(string spid, string type, string state) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.QueryBussFreezeList(spid, type, state);
        }

        public void BusinessIdentityCardNum(string Fspid, string OldIdentityCardNum, string NewIdentityCardNum, string IDImage, string ElseImage, string UserName, string Reason) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.BusinessIdentityCardNum(Fspid, OldIdentityCardNum, NewIdentityCardNum, IDImage, ElseImage, UserName, Reason);
        }

        public void ModifyPayBusinessInfo(string TableFlag, string KeyID, string ContactUser, string ContactPhone, string ContactMobile, string ContactQQ, string ContactEmail, string CompanyAddress, string Postalcode) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.ModifyPayBusinessInfo(TableFlag, KeyID, ContactUser, ContactPhone, ContactMobile, ContactQQ, ContactEmail, CompanyAddress, Postalcode);
        }

        public void SubmitBusinessInfo(string UserName, string Fspid, string OldFspName, string NewFspName, string OldEmail, string NewEmail, string OldAddress, string NewAddress, string ApplyResult, string[] FileInfos) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.SubmitBusinessInfo(UserName, Fspid, OldFspName, NewFspName, OldEmail, NewEmail, OldAddress, NewAddress, ApplyResult, FileInfos);
        }

        public DataSet GetMspAmendTaskByID(string TaskId) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetMspAmendTaskByID(TaskId);
        }
        public void UpdateMspAmendTaskByTaskid(string TaskId, int AmendState, string UserId) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.UpdateMspAmendTaskByTaskid(TaskId, AmendState, UserId);
        }
        public void UpdateSpidDomainApplyByTaskid(string TaskId, string Reason) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.UpdateSpidDomainApplyByTaskid(TaskId, Reason);
        }
        public void UpdateMspAmendInfoByTaskid(string TaskId, string Reason) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.UpdateMspAmendInfoByTaskid(TaskId, Reason);
        }

        public DataSet GetAllValueAddedTax(string Spid, string CompanyName, int topCount, int notInCount)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetAllValueAddedTax(Spid, CompanyName, topCount, notInCount);
        }
        public DataSet GetOneValueAddedTax(string Spid) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetOneValueAddedTax(Spid);
        }
        public void ValueAddedTaxModify(string Spid, int Flag) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.ValueAddedTaxModify(Spid, Flag);
        }

        public DataSet GetApplyValueAddedTax(string Spid, string Flags, int topCount, int notInCount)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetApplyValueAddedTax(Spid,Flags,topCount,notInCount);
        }
        public DataSet GetValueAddedTaxDetail(string taskid) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetValueAddedTaxDetail(taskid);
        }
        public void ValueAddedTaxApprove(string taskid, string Memo, string imgTaxCert, string imgBizLicenseCert, string imgAuthorizationCert, string UserName) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.ValueAddedTaxApprove(taskid, Memo, imgTaxCert, imgBizLicenseCert, imgAuthorizationCert, UserName);
        }

        public void ValueAddedTaxCancel(string taskid, string spid, string Memo, string UserName)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            cli.ValueAddedTaxCancel(taskid, spid, Memo, UserName);
        }
        public DataSet GetSpidDomainQueryListCount(string filter)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            DataTable dt = new DataTable();
            dt.Columns.Add("Count");
            DataRow dr = dt.NewRow();
            //GetSpidDomainQueryListCount接口已经废弃
            dr["Count"] = 0;//cli.GetSpidDomainQueryListCount(filter);
            dt.Rows.Add(dr);
            return new DataSet() { Tables = { dt } };
        }

        public DataSet GetSpidDomainQueryList(string Spid, string CompanyName, DateTime? ApplyTimeStart, DateTime? ApplyTimeEnd, int? AmendState, string submitType, int topCount, int notInCount)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetSpidDomainQueryList(Spid,CompanyName,ApplyTimeStart,ApplyTimeEnd,AmendState,submitType,topCount, notInCount);
        }

        public DataSet QueryAmendMspInfo(string spid, string type, string caccounts)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.QueryAmendMspInfo(spid, type, caccounts);
        }

        public DataSet GetSpInfo(string SPID, string ApplyCpInfoID, string CompanyName, string WWWAdress, string WebName, string AppID, int topCount, int notInCount)

        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetSpInfo(SPID, ApplyCpInfoID, CompanyName, WWWAdress, WebName, AppID, topCount, notInCount);

        }

        public DataSet GetBDSpidList()
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetBDSpidList();
        }

        public DataSet GetCheckInfo(string spid, int checktype)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.GetCheckInfo(spid, checktype);
        }

        public bool ReSendEmail_ToSP(out string resultstr,string spid, string applyUser)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.ReSendEmail_ToSP(out resultstr,spid, applyUser);
        }

        public bool ReSendCertificate(out string resultstr, string spid, string applyUser)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();

            return cli.ReSendCertificate(out resultstr, spid, applyUser);
        }


        public DataSet QueryExpiredCertificate(string begindate, string enddate, string spid, int topCount, int notInCount)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.QueryExpiredCertificate(begindate, enddate, spid, topCount, notInCount);
        }

        public DataSet GetSelfQuerySPType() 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.GetSelfQuerySPType();
        }

        public string ClosePay(string fspid, string opuser, string freson) 
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.ClosePay(fspid, opuser, freson);
        }

        public string FreezeSpid(string fspid, string opuser, string freson)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.FreezeSpid(fspid, opuser, freson);
        }

        public string LostOfSpid(string fspid, string opuser, string freson)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.LostOfSpid(fspid, opuser, freson);
        }

        public string CloseAgency(string fspid, string opuser, string freson)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.CloseAgency(fspid, opuser, freson);
        }

        public string CloseRefund(string fspid, string opuser, string freson)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.CloseRefund(fspid, opuser, freson);
        }

        public string OpenRefund(string fspid, string opuser, string freson)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.OpenRefund(fspid, opuser, freson);
        }

        public string RestoreOfSpid(string fspid, string opuser, string freson)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.RestoreOfSpid(fspid, opuser, freson);
        }
        public string BusinessLogout(string fspid, string opuser, string freson)
        {
            SPOAServiceRef.GeneralSPOAServiceClient cli = new SPOAServiceRef.GeneralSPOAServiceClient();
            return cli.BusinessLogoutApply(fspid, opuser, freson);
        }
    }
}
