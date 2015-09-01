<%@ Register TagPrefix="uc1" TagName="AccountManage" Src="AccountManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BaseAccount" Src="BaseAccountControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AccountOperate" Src="AccountOperateControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TradeManage" Src="TradeManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RiskConManage" Src="RiskConManage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="OverseasPay" Src="OverseasPay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FreezeManage" Src="FreezeManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AccountLedgerManage" Src="AccountLedgerManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AccountOperaManage" Src="AccountOperaManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BankBillManage" Src="BankBillManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FastPay" Src="FastPayControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FundAccountManage" Src="FundAccountManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MediumTradeManage" Src="MediumTradeManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LifeFeeDetailManage" Src="LifeFeeDetailControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MicroPay" Src="MicroPayControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NameAuthened" Src="NameAuthenedControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SelfHelpAppealManage" Src="SelfHelpAppealManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="VIPAccountManage" Src="VIPAccountManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SpecialManageControl" Src="SpecialManageControl.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="LeftControl.ascx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Control.LeftControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="DKManageControl" Src="DKManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DFManageControl" Src="DFManageControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InternetBank" Src="InternetBank.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ActivityCooperation" Src="ActivityCooperation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TokenCoin" Src="TokenCoin.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SysManage" Src="SysManage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CreditPayControl" Src="CreditPayControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WebchatPayControl" Src="WebchatPayControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TravelPlatform" Src="TravelPlatform.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ForeignCurrencyPay" Src="ForeignCurrencyPay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ForeignCurrencyAccount" Src="ForeignCurrencyAccount.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ForeignCardPay" Src="ForeignCardPay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PNRQuery" Src="PNRQueryControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HandQBusiness" Src="HandQBusiness.ascx" %>

<script language="javascript" type="text/javascript" src="scripts/local.js"></script>
<script language="javascript" type="text/javascript">
    function showHidenMenu(o) {
        var _this = document.getElementById(o);
        _this.style.display = _this.style.display == "none" ? "" : "none";
        var sbiling = _this.parentElement.children || _this.parentElement.childNodes;
       
        for (var i = 0; i < sbiling.length; i++) {
            var cur = sbiling[i];
            if (cur.nodeType == 1 && cur.nodeName.toLowerCase() == "tr" && cur.id != "" && cur.id != o)
            {
                cur.style.display = "none";
            }           
        }
    }
</script>
<style type="text/css">
BODY { BACKGROUND-IMAGE: url(./IMAGES/Page/bg01.gif) }
</style>
<table width="118" border="0" cellspacing="0" cellpadding="0" class="Left_tree" style="WIDTH: 118px; HEIGHT: 490px" id="tree">
	<tr>
		<td width="130" valign="top" class="Left_table_wh">
            <table width="100%">
                <tr style="cursor:hand" onclick="javascript:showHidenMenu('basicInfo')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20">&nbsp;&nbsp;&nbsp;&nbsp;<strong>基础信息管理</strong></td>
                </tr>
                <tr id="basicInfo" style="display:none">
                    <td >
                        <uc1:BaseAccount id="baseAccount1" runat="server"></uc1:BaseAccount> <!--账户管理-->
                         <uc1:AccountOperate id="accountOperate1" runat="server"></uc1:AccountOperate> <!--账户操作-->
			            <uc1:VIPAccountManage id="VIPAccountManage" runat="server"></uc1:VIPAccountManage><!--会员帐号-->
			            <uc1:SelfHelpAppealManage id="SelfHelpAppealManage1" runat="server"></uc1:SelfHelpAppealManage><!--自助申诉-->
			            <uc1:NameAuthened id="NameAuthened1" runat="server"></uc1:NameAuthened><!--实名认证-->
			            <uc1:TokenCoin ID="TokenCoin1" runat="server" /><!--财付券管理-->
			            <uc1:ActivityCooperation ID="ActivityCooperation1" runat="server" /><!--活动合作-->
			            <uc1:RiskConManage id="RiskConManage1" runat="server"></uc1:RiskConManage><!--风控管理-->
			            <uc1:FundAccountManage id="FundAccountManage1" runat="server"></uc1:FundAccountManage><!--基金账户-->
                  </td>
                </tr>
                <tr style="cursor:hand" onclick="javascript:showHidenMenu('payManagement')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20">&nbsp;&nbsp;&nbsp;&nbsp;<strong>支付管理</strong></td>
                </tr>
                <tr id="payManagement" style="display:none">
                  <td >
                        <uc1:WebchatPayControl ID="WebchatPayControl1" runat="server" /><!--微信支付-->
                        <uc1:FastPay id="FastPay1" runat="server"></uc1:FastPay><!--快捷支付-->
                        <uc1:CreditPayControl ID="CreditPayControl1" runat="server" /><!--信用支付-->
                        <uc1:OverseasPay id="OverseasPay1" runat="server"></uc1:OverseasPay><!--国际支付-->
                        <uc1:MicroPay id="MicroPay1" runat="server"></uc1:MicroPay><!--微支付--> 
                        <uc1:ForeignCurrencyPay ID="ForeignCurrencyPay1" runat="server" /><!--外币支付-->
                        <uc1:ForeignCardPay ID="ForeignCardPay1" runat="server" /><!--外卡支付-->
                        <uc1:HandQBusiness ID="HandQBusiness" runat="server" /><!--手Q业务-->
                  </td>
                </tr>
                <tr style="cursor:hand" onclick="javascript:showHidenMenu('tradeManagement')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20">&nbsp;&nbsp;&nbsp;&nbsp;<strong>交易管理</strong></td>  
                </tr>
                <tr id="tradeManagement" style="display:none">
                  <td >
                        <uc1:TradeManage id="tradeManage1" runat="server"></uc1:TradeManage> <!--交易查询-->
			            <uc1:LifeFeeDetailManage id="LifeFeeDetailManage1" runat="Server"></uc1:LifeFeeDetailManage><!--生活缴费-->
                        <uc1:InternetBank ID="InternetBank" runat="server" /><!--网银查询-->
                  </td>
                </tr>
                <tr style="cursor:hand" onclick="javascript:showHidenMenu('bussInfo')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20">&nbsp;&nbsp;&nbsp;&nbsp;<strong>商户信息管理</strong></td>
                </tr>
                <tr id="bussInfo" style="display:none">
                  <td >
                        <uc1:AccountManage id="accountManage1" runat="server"></uc1:AccountManage><!--商户管理-->
                        <uc1:PNRQuery id="PNRQuery1" runat="server"></uc1:PNRQuery><!--PNR查询-->
			            <uc1:AccountLedgerManage id="AccountLedgerManage1" runat="server"></uc1:AccountLedgerManage><!--商户分账-->
			            <uc1:AccountOperaManage id="AccountOperaManage1" runat="server"></uc1:AccountOperaManage><!--商户操作-->
                        <uc1:DKManageControl id="DKManageControl1" runat="server"></uc1:DKManageControl><!--代扣查询-->
                        <uc1:DFManageControl id="DFManageControl1" runat="server"></uc1:DFManageControl><!--代付查询-->
                        <uc1:TravelPlatform ID="TravelPlatform1" runat="server" /><!--旅行平台-->
                        <uc1:ForeignCurrencyAccount ID="ForeignCurrencyAccount1" runat="server" /><!--外币商户管理-->
                  </td>
                </tr>
                <tr style="cursor:hand" onclick="javascript:showHidenMenu('sysManagement')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20">&nbsp;&nbsp;&nbsp;&nbsp;<strong>系统管理</strong></td>
                </tr>
                <tr id="sysManagement" style="display:none">
                  <td>
                        <uc1:BankBillManage id="BankBillManage1" runat="server"></uc1:BankBillManage><!--银行账单-->
                        <uc1:SysManage ID="SysManage1" runat="server" /><!--公告管理-->
                        <uc1:FreezeManage id="FreezeManage1" runat="server"></uc1:FreezeManage><!--资料审核-->
			            <uc1:SpecialManageControl id="SpecialManageControl1" runat="Server"></uc1:SpecialManageControl><!--日志管理-->
                  </td>
                </tr>
            </table>
			
		</td>
		<td width="10" valign="top" class="Left_table_wh"><img src="../Images/Page/bg_head3.gif" alt="" width="10" height="490" /></td>
	</tr>
</table>
