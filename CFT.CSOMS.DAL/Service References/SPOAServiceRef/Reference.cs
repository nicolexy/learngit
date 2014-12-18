﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18063
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFT.CSOMS.DAL.SPOAServiceRef {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SPOAServiceRef.IGeneralSPOAService")]
    public interface IGeneralSPOAService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/OpenAgency", ReplyAction="http://tempuri.org/IGeneralSPOAService/OpenAgencyResponse")]
        string OpenAgency(string fspid, string opuser, string freson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/RestoreOfSpid", ReplyAction="http://tempuri.org/IGeneralSPOAService/RestoreOfSpidResponse")]
        string RestoreOfSpid(string fspid, string opuser, string freson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetCheckInfo", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetCheckInfoResponse")]
        System.Data.DataSet GetCheckInfo(string ApplyCpInfoID, int checktype);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ReSendEmail_ToSP", ReplyAction="http://tempuri.org/IGeneralSPOAService/ReSendEmail_ToSPResponse")]
        bool ReSendEmail_ToSP(out string resultstr, string SPID, string ApplyUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ReSendCertificate", ReplyAction="http://tempuri.org/IGeneralSPOAService/ReSendCertificateResponse")]
        bool ReSendCertificate(out string resultstr, string SPID, string ApplyUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/QueryExpiredCertificate", ReplyAction="http://tempuri.org/IGeneralSPOAService/QueryExpiredCertificateResponse")]
        System.Data.DataSet QueryExpiredCertificate(string beginDt, string endDt, string SPID, int topCount, int notInCount);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetBDSpidList", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetBDSpidListResponse")]
        System.Data.DataSet GetBDSpidList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/UpdateMspAmendTaskByTaskid", ReplyAction="http://tempuri.org/IGeneralSPOAService/UpdateMspAmendTaskByTaskidResponse")]
        void UpdateMspAmendTaskByTaskid(string Taskid, int AmendState, string CheckUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/UpdateMspAmendInfoByTaskid", ReplyAction="http://tempuri.org/IGeneralSPOAService/UpdateMspAmendInfoByTaskidResponse")]
        void UpdateMspAmendInfoByTaskid(string Taskid, string DisagreeResult);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/UpdateSpidDomainApplyByTaskid", ReplyAction="http://tempuri.org/IGeneralSPOAService/UpdateSpidDomainApplyByTaskidResponse")]
        void UpdateSpidDomainApplyByTaskid(string Taskid, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/BusinessIdentityCardNum", ReplyAction="http://tempuri.org/IGeneralSPOAService/BusinessIdentityCardNumResponse")]
        void BusinessIdentityCardNum(string Fspid, string OldIdentityCardNum, string NewIdentityCardNum, string IDImage, string ElseImage, string UserName, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ValueAddedTaxModify", ReplyAction="http://tempuri.org/IGeneralSPOAService/ValueAddedTaxModifyResponse")]
        void ValueAddedTaxModify(string Spid, int Flag);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ApplyRefund", ReplyAction="http://tempuri.org/IGeneralSPOAService/ApplyRefundResponse")]
        void ApplyRefund(string Fspid, string UserName, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ApproveSpidCompanyName", ReplyAction="http://tempuri.org/IGeneralSPOAService/ApproveSpidCompanyNameResponse")]
        void ApproveSpidCompanyName(string Taskid, string UserID, bool Result, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ApproveTicket", ReplyAction="http://tempuri.org/IGeneralSPOAService/ApproveTicketResponse")]
        void ApproveTicket(string ApplyCpInfoID, string UserID, int Type, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/BusinessFreeze", ReplyAction="http://tempuri.org/IGeneralSPOAService/BusinessFreezeResponse")]
        void BusinessFreeze(string Fspid, string UserName, bool IsFreeze, bool IsFreezePay, bool IsAccLoss, bool IsCloseAgent, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/BusinessFreezeSPOA", ReplyAction="http://tempuri.org/IGeneralSPOAService/BusinessFreezeSPOAResponse")]
        void BusinessFreezeSPOA(string Fspid, bool IsFreeze, bool IsFreezePay, bool IsAccLoss, bool IsCloseAgent, string UserName, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/BusinessResume", ReplyAction="http://tempuri.org/IGeneralSPOAService/BusinessResumeResponse")]
        void BusinessResume(string Fspid, string UserName, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/CheckTicket", ReplyAction="http://tempuri.org/IGeneralSPOAService/CheckTicketResponse")]
        void CheckTicket(string ApplyCpInfoID, string UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ModifyPayBusinessInfo", ReplyAction="http://tempuri.org/IGeneralSPOAService/ModifyPayBusinessInfoResponse")]
        void ModifyPayBusinessInfo(string TableFlag, string KeyID, string ContactUser, string ContactPhone, string ContactMobile, string ContactQQ, string ContactEmail, string CompanyAddress, string Postalcode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ShutRefund", ReplyAction="http://tempuri.org/IGeneralSPOAService/ShutRefundResponse")]
        void ShutRefund(string Fspid, string UserName, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/SubmitBusinessInfo", ReplyAction="http://tempuri.org/IGeneralSPOAService/SubmitBusinessInfoResponse")]
        void SubmitBusinessInfo(string UserName, string Fspid, string OldFspName, string NewFspName, string OldEmail, string NewEmail, string OldAddress, string NewAddress, string ApplyResult, string[] FileInfos);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ValueAddedTaxApprove", ReplyAction="http://tempuri.org/IGeneralSPOAService/ValueAddedTaxApproveResponse")]
        void ValueAddedTaxApprove(string taskid, string Memo, string imgTaxCert, string imgBizLicenseCert, string imgAuthorizationCert, string UserName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/LostOfSpid", ReplyAction="http://tempuri.org/IGeneralSPOAService/LostOfSpidResponse")]
        string LostOfSpid(string fspid, string opuser, string freson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/FreezeSpid", ReplyAction="http://tempuri.org/IGeneralSPOAService/FreezeSpidResponse")]
        string FreezeSpid(string fspid, string opuser, string freson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/ClosePay", ReplyAction="http://tempuri.org/IGeneralSPOAService/ClosePayResponse")]
        string ClosePay(string fspid, string opuser, string freson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/OpenPay", ReplyAction="http://tempuri.org/IGeneralSPOAService/OpenPayResponse")]
        string OpenPay(string fspid, string opuser, string freson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/CloseRefund", ReplyAction="http://tempuri.org/IGeneralSPOAService/CloseRefundResponse")]
        string CloseRefund(string fspid, string opuser, string freson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/OpenRefund", ReplyAction="http://tempuri.org/IGeneralSPOAService/OpenRefundResponse")]
        string OpenRefund(string fspid, string opuser, string freson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/CloseAgency", ReplyAction="http://tempuri.org/IGeneralSPOAService/CloseAgencyResponse")]
        string CloseAgency(string fspid, string opuser, string freson);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetAgencyBusinessList", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetAgencyBusinessListResponse")]
        System.Data.DataSet GetAgencyBusinessList(string qqid, string domain, int notInCount, int topCount);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetAgencyBusinessInfo", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetAgencyBusinessInfoResponse")]
        System.Data.DataSet GetAgencyBusinessInfo(string Fid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/QueryAgencyBySpid", ReplyAction="http://tempuri.org/IGeneralSPOAService/QueryAgencyBySpidResponse")]
        System.Data.DataSet QueryAgencyBySpid(string spid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/QueryAgencyInfoById", ReplyAction="http://tempuri.org/IGeneralSPOAService/QueryAgencyInfoByIdResponse")]
        System.Data.DataSet QueryAgencyInfoById(string fid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetShouFuYiList", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetShouFuYiListResponse")]
        System.Data.DataSet GetShouFuYiList(string qq);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetAgencyBusinessInfoList", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetAgencyBusinessInfoListResponse")]
        System.Data.DataSet GetAgencyBusinessInfoList(string Fspid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetHisBusinessList", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetHisBusinessListResponse")]
        System.Data.DataSet GetHisBusinessList(string spid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetSelfTypeList", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetSelfTypeListResponse")]
        System.Data.DataSet GetSelfTypeList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetSelfKFList", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetSelfKFListResponse")]
        System.Data.DataSet GetSelfKFList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetSelfQueryListCount", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetSelfQueryListCountResponse")]
        int GetSelfQueryListCount(string filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetSelfQueryList", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetSelfQueryListResponse")]
        System.Data.DataSet GetSelfQueryList(string filter, int topCount, int notInCount);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetSelfQuerySPType", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetSelfQuerySPTypeResponse")]
        System.Data.DataSet GetSelfQuerySPType();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetSelfQueryInfo", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetSelfQueryInfoResponse")]
        System.Data.DataSet GetSelfQueryInfo(string applyCpInfoID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetSpidDomainQueryList", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetSpidDomainQueryListResponse")]
        System.Data.DataSet GetSpidDomainQueryList(string filter, string submitType, int topCount, int notInCount);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetSpidDomainQueryListCount", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetSpidDomainQueryListCountResponse")]
        int GetSpidDomainQueryListCount(string filter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetOneValueAddedTax", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetOneValueAddedTaxResponse")]
        System.Data.DataSet GetOneValueAddedTax(string spid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetAllValueAddedTax", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetAllValueAddedTaxResponse")]
        System.Data.DataSet GetAllValueAddedTax(string filter, int topCount, int notInCount);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetValueAddedTaxDetail", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetValueAddedTaxDetailResponse")]
        System.Data.DataSet GetValueAddedTaxDetail(string taskid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetTradeType", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetTradeTypeResponse")]
        string GetTradeType(string spid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetWWWAddress", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetWWWAddressResponse")]
        string GetWWWAddress(string spid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetBDName", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetBDNameResponse")]
        string GetBDName(string spid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetApplyValueAddedTax", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetApplyValueAddedTaxResponse")]
        System.Data.DataSet GetApplyValueAddedTax(string filter, int topCount, int notInCount);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/QueryBussFreezeList", ReplyAction="http://tempuri.org/IGeneralSPOAService/QueryBussFreezeListResponse")]
        System.Data.DataSet QueryBussFreezeList(string spid, string type, string state);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetAllValueAddedTax1", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetAllValueAddedTax1Response")]
        string GetAllValueAddedTax1();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetApplyCpInfoXByID", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetApplyCpInfoXByIDResponse")]
        System.Data.DataSet GetApplyCpInfoXByID(string ApplyCpInfoID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetMspAmendTaskByID", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetMspAmendTaskByIDResponse")]
        System.Data.DataSet GetMspAmendTaskByID(string TaskId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/GetSpInfo", ReplyAction="http://tempuri.org/IGeneralSPOAService/GetSpInfoResponse")]
        System.Data.DataSet GetSpInfo(string filter, int topCount, int notInCount);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeneralSPOAService/QueryAmendMspInfo", ReplyAction="http://tempuri.org/IGeneralSPOAService/QueryAmendMspInfoResponse")]
        System.Data.DataSet QueryAmendMspInfo(string spid, string type, string caccounts);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGeneralSPOAServiceChannel : CFT.CSOMS.DAL.SPOAServiceRef.IGeneralSPOAService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GeneralSPOAServiceClient : System.ServiceModel.ClientBase<CFT.CSOMS.DAL.SPOAServiceRef.IGeneralSPOAService>, CFT.CSOMS.DAL.SPOAServiceRef.IGeneralSPOAService {
        
        public GeneralSPOAServiceClient() {
        }
        
        public GeneralSPOAServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GeneralSPOAServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GeneralSPOAServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GeneralSPOAServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string OpenAgency(string fspid, string opuser, string freson) {
            return base.Channel.OpenAgency(fspid, opuser, freson);
        }
        
        public string RestoreOfSpid(string fspid, string opuser, string freson) {
            return base.Channel.RestoreOfSpid(fspid, opuser, freson);
        }
        
        public System.Data.DataSet GetCheckInfo(string ApplyCpInfoID, int checktype) {
            return base.Channel.GetCheckInfo(ApplyCpInfoID, checktype);
        }
        
        public bool ReSendEmail_ToSP(out string resultstr, string SPID, string ApplyUser) {
            return base.Channel.ReSendEmail_ToSP(out resultstr, SPID, ApplyUser);
        }
        
        public bool ReSendCertificate(out string resultstr, string SPID, string ApplyUser) {
            return base.Channel.ReSendCertificate(out resultstr, SPID, ApplyUser);
        }
        
        public System.Data.DataSet QueryExpiredCertificate(string beginDt, string endDt, string SPID, int topCount, int notInCount) {
            return base.Channel.QueryExpiredCertificate(beginDt, endDt, SPID, topCount, notInCount);
        }
        
        public System.Data.DataSet GetBDSpidList() {
            return base.Channel.GetBDSpidList();
        }
        
        public void UpdateMspAmendTaskByTaskid(string Taskid, int AmendState, string CheckUser) {
            base.Channel.UpdateMspAmendTaskByTaskid(Taskid, AmendState, CheckUser);
        }
        
        public void UpdateMspAmendInfoByTaskid(string Taskid, string DisagreeResult) {
            base.Channel.UpdateMspAmendInfoByTaskid(Taskid, DisagreeResult);
        }
        
        public void UpdateSpidDomainApplyByTaskid(string Taskid, string Reason) {
            base.Channel.UpdateSpidDomainApplyByTaskid(Taskid, Reason);
        }
        
        public void BusinessIdentityCardNum(string Fspid, string OldIdentityCardNum, string NewIdentityCardNum, string IDImage, string ElseImage, string UserName, string Reason) {
            base.Channel.BusinessIdentityCardNum(Fspid, OldIdentityCardNum, NewIdentityCardNum, IDImage, ElseImage, UserName, Reason);
        }
        
        public void ValueAddedTaxModify(string Spid, int Flag) {
            base.Channel.ValueAddedTaxModify(Spid, Flag);
        }
        
        public void ApplyRefund(string Fspid, string UserName, string Reason) {
            base.Channel.ApplyRefund(Fspid, UserName, Reason);
        }
        
        public void ApproveSpidCompanyName(string Taskid, string UserID, bool Result, string Reason) {
            base.Channel.ApproveSpidCompanyName(Taskid, UserID, Result, Reason);
        }
        
        public void ApproveTicket(string ApplyCpInfoID, string UserID, int Type, string Reason) {
            base.Channel.ApproveTicket(ApplyCpInfoID, UserID, Type, Reason);
        }
        
        public void BusinessFreeze(string Fspid, string UserName, bool IsFreeze, bool IsFreezePay, bool IsAccLoss, bool IsCloseAgent, string Reason) {
            base.Channel.BusinessFreeze(Fspid, UserName, IsFreeze, IsFreezePay, IsAccLoss, IsCloseAgent, Reason);
        }
        
        public void BusinessFreezeSPOA(string Fspid, bool IsFreeze, bool IsFreezePay, bool IsAccLoss, bool IsCloseAgent, string UserName, string Reason) {
            base.Channel.BusinessFreezeSPOA(Fspid, IsFreeze, IsFreezePay, IsAccLoss, IsCloseAgent, UserName, Reason);
        }
        
        public void BusinessResume(string Fspid, string UserName, string Reason) {
            base.Channel.BusinessResume(Fspid, UserName, Reason);
        }
        
        public void CheckTicket(string ApplyCpInfoID, string UserID) {
            base.Channel.CheckTicket(ApplyCpInfoID, UserID);
        }
        
        public void ModifyPayBusinessInfo(string TableFlag, string KeyID, string ContactUser, string ContactPhone, string ContactMobile, string ContactQQ, string ContactEmail, string CompanyAddress, string Postalcode) {
            base.Channel.ModifyPayBusinessInfo(TableFlag, KeyID, ContactUser, ContactPhone, ContactMobile, ContactQQ, ContactEmail, CompanyAddress, Postalcode);
        }
        
        public void ShutRefund(string Fspid, string UserName, string Reason) {
            base.Channel.ShutRefund(Fspid, UserName, Reason);
        }
        
        public void SubmitBusinessInfo(string UserName, string Fspid, string OldFspName, string NewFspName, string OldEmail, string NewEmail, string OldAddress, string NewAddress, string ApplyResult, string[] FileInfos) {
            base.Channel.SubmitBusinessInfo(UserName, Fspid, OldFspName, NewFspName, OldEmail, NewEmail, OldAddress, NewAddress, ApplyResult, FileInfos);
        }
        
        public void ValueAddedTaxApprove(string taskid, string Memo, string imgTaxCert, string imgBizLicenseCert, string imgAuthorizationCert, string UserName) {
            base.Channel.ValueAddedTaxApprove(taskid, Memo, imgTaxCert, imgBizLicenseCert, imgAuthorizationCert, UserName);
        }
        
        public string LostOfSpid(string fspid, string opuser, string freson) {
            return base.Channel.LostOfSpid(fspid, opuser, freson);
        }
        
        public string FreezeSpid(string fspid, string opuser, string freson) {
            return base.Channel.FreezeSpid(fspid, opuser, freson);
        }
        
        public string ClosePay(string fspid, string opuser, string freson) {
            return base.Channel.ClosePay(fspid, opuser, freson);
        }
        
        public string OpenPay(string fspid, string opuser, string freson) {
            return base.Channel.OpenPay(fspid, opuser, freson);
        }
        
        public string CloseRefund(string fspid, string opuser, string freson) {
            return base.Channel.CloseRefund(fspid, opuser, freson);
        }
        
        public string OpenRefund(string fspid, string opuser, string freson) {
            return base.Channel.OpenRefund(fspid, opuser, freson);
        }
        
        public string CloseAgency(string fspid, string opuser, string freson) {
            return base.Channel.CloseAgency(fspid, opuser, freson);
        }
        
        public System.Data.DataSet GetAgencyBusinessList(string qqid, string domain, int notInCount, int topCount) {
            return base.Channel.GetAgencyBusinessList(qqid, domain, notInCount, topCount);
        }
        
        public System.Data.DataSet GetAgencyBusinessInfo(string Fid) {
            return base.Channel.GetAgencyBusinessInfo(Fid);
        }
        
        public System.Data.DataSet QueryAgencyBySpid(string spid) {
            return base.Channel.QueryAgencyBySpid(spid);
        }
        
        public System.Data.DataSet QueryAgencyInfoById(string fid) {
            return base.Channel.QueryAgencyInfoById(fid);
        }
        
        public System.Data.DataSet GetShouFuYiList(string qq) {
            return base.Channel.GetShouFuYiList(qq);
        }
        
        public System.Data.DataSet GetAgencyBusinessInfoList(string Fspid) {
            return base.Channel.GetAgencyBusinessInfoList(Fspid);
        }
        
        public System.Data.DataSet GetHisBusinessList(string spid) {
            return base.Channel.GetHisBusinessList(spid);
        }
        
        public System.Data.DataSet GetSelfTypeList() {
            return base.Channel.GetSelfTypeList();
        }
        
        public System.Data.DataSet GetSelfKFList() {
            return base.Channel.GetSelfKFList();
        }
        
        public int GetSelfQueryListCount(string filter) {
            return base.Channel.GetSelfQueryListCount(filter);
        }
        
        public System.Data.DataSet GetSelfQueryList(string filter, int topCount, int notInCount) {
            return base.Channel.GetSelfQueryList(filter, topCount, notInCount);
        }
        
        public System.Data.DataSet GetSelfQuerySPType() {
            return base.Channel.GetSelfQuerySPType();
        }
        
        public System.Data.DataSet GetSelfQueryInfo(string applyCpInfoID) {
            return base.Channel.GetSelfQueryInfo(applyCpInfoID);
        }
        
        public System.Data.DataSet GetSpidDomainQueryList(string filter, string submitType, int topCount, int notInCount) {
            return base.Channel.GetSpidDomainQueryList(filter, submitType, topCount, notInCount);
        }
        
        public int GetSpidDomainQueryListCount(string filter) {
            return base.Channel.GetSpidDomainQueryListCount(filter);
        }
        
        public System.Data.DataSet GetOneValueAddedTax(string spid) {
            return base.Channel.GetOneValueAddedTax(spid);
        }
        
        public System.Data.DataSet GetAllValueAddedTax(string filter, int topCount, int notInCount) {
            return base.Channel.GetAllValueAddedTax(filter, topCount, notInCount);
        }
        
        public System.Data.DataSet GetValueAddedTaxDetail(string taskid) {
            return base.Channel.GetValueAddedTaxDetail(taskid);
        }
        
        public string GetTradeType(string spid) {
            return base.Channel.GetTradeType(spid);
        }
        
        public string GetWWWAddress(string spid) {
            return base.Channel.GetWWWAddress(spid);
        }
        
        public string GetBDName(string spid) {
            return base.Channel.GetBDName(spid);
        }
        
        public System.Data.DataSet GetApplyValueAddedTax(string filter, int topCount, int notInCount) {
            return base.Channel.GetApplyValueAddedTax(filter, topCount, notInCount);
        }
        
        public System.Data.DataSet QueryBussFreezeList(string spid, string type, string state) {
            return base.Channel.QueryBussFreezeList(spid, type, state);
        }
        
        public string GetAllValueAddedTax1() {
            return base.Channel.GetAllValueAddedTax1();
        }
        
        public System.Data.DataSet GetApplyCpInfoXByID(string ApplyCpInfoID) {
            return base.Channel.GetApplyCpInfoXByID(ApplyCpInfoID);
        }
        
        public System.Data.DataSet GetMspAmendTaskByID(string TaskId) {
            return base.Channel.GetMspAmendTaskByID(TaskId);
        }
        
        public System.Data.DataSet GetSpInfo(string filter, int topCount, int notInCount) {
            return base.Channel.GetSpInfo(filter, topCount, notInCount);
        }
        
        public System.Data.DataSet QueryAmendMspInfo(string spid, string type, string caccounts) {
            return base.Channel.QueryAmendMspInfo(spid, type, caccounts);
        }
    }
}
