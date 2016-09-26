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
<%@ Register TagPrefix="uc1" TagName="HKWalletPay" Src="HKWalletPay.ascx" %>
<%@ Register Src="~/Control/FundControl.ascx" TagPrefix="uc1" TagName="FundControl" %>
<%@ Register Src="~/Control/MenuControl.ascx" TagPrefix="uc1" TagName="MenuControl" %>



<script language="javascript" type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>
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
    $(document).ready(function () {
        //$('table tr[name=menu_one]').click(function () {
        //    if ($(this).next().css('display') == 'none') {
        //        $('table tr[name=menu_one]').each(function (i) {
        //            $(this).next().hide();
        //        });
        //        $(this).next().show();
        //    } else {
        //        $(this).next().hide();
        //    }
        //});
        //$('span tr[name=menu_flag]').click(function () {
        //    if ($(this).next().css('display') == 'none') {
        //        $(this).parents('table tr[name=menu_two]').find('tr[name=menu_flag]').each(function (i) {
        //            $(this).next().hide();
        //        });
        //        $(this).next().show();
        //    } else {
        //        $(this).next().hide();
        //    }
        //});
        $('a[target=WorkArea]').click(function () {
            $('a[target=WorkArea]').each(function (i) {
                $(this).css('color','');
            });
            $(this).css('color','red');
        });
    });
</script>
<style type="text/css">
BODY { BACKGROUND-IMAGE: url(./IMAGES/Page/bg01.gif) }
</style>
<table width="118" border="0" cellspacing="0" cellpadding="0" class="Left_tree" style="WIDTH: 118px; HEIGHT: 490px" id="tree">
	<tr>
		<td width="130" valign="top" class="Left_table_wh">
            <table width="100%">
                <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('basicInfo')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>������Ϣ����</strong></td>
                </tr>
                <tr id="basicInfo" name="menu_two" style="display:none">
                    <td >
                        <uc1:BaseAccount id="baseAccount1" runat="server" Visible="false"></uc1:BaseAccount> <!--�˻�����-->
                         <uc1:AccountOperate id="accountOperate1" runat="server" Visible="false"></uc1:AccountOperate> <!--�˻�����-->
			      
			            <uc1:SelfHelpAppealManage id="SelfHelpAppealManage1" runat="server" Visible="false"></uc1:SelfHelpAppealManage><!--��������-->
			            <uc1:NameAuthened id="NameAuthened1" runat="server" Visible="false"></uc1:NameAuthened><!--ʵ����֤-->
			            <%--<uc1:TokenCoin ID="TokenCoin1" runat="server" /><!--�Ƹ�ȯ����-->--%>
			            <%--<uc1:ActivityCooperation ID="ActivityCooperation1" runat="server" /><!--�����-->--%>
			            <uc1:RiskConManage id="RiskConManage1" runat="server" Visible="false"></uc1:RiskConManage><!--��ع���-->
			            <uc1:FundAccountManage id="FundAccountManage1" runat="server" Visible="false"></uc1:FundAccountManage><!--�����˻�-->
                        <uc1:WebchatPayControl ID="WebchatPayControl1" runat="server"  Visible="false"/><!--΢��֧��-->
                  </td>
                </tr>

                 <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('tradeManagement')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>���׹���</strong></td>  
                </tr>
                <tr id="tradeManagement" name="menu_two" style="display:none">
                  <td >
                        <uc1:TradeManage id="tradeManage1" runat="server" Visible="false"></uc1:TradeManage> <!--���ײ�ѯ-->
			            <uc1:LifeFeeDetailManage id="LifeFeeDetailManage1" runat="Server" Visible="false"></uc1:LifeFeeDetailManage><!--����ɷ�-->
                        <%--<uc1:InternetBank ID="InternetBank" runat="server"  Visible="false"/><!--������ѯ-->--%>
                        <uc1:BankBillManage id="BankBillManage1" runat="server" Visible="false"></uc1:BankBillManage><!--�����˵�-->
                  </td>
                </tr>

                <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('fundManagement')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>���ͨ</strong></td>
                </tr>
                 <tr id="fundManagement" name="menu_two" style="display:none">
                  <td>
                         <uc1:FundControl runat="server" ID="FundControl1"  Visible="false"/><!--���ͨ--> 
                  </td>
                </tr>

                    <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('HandQ')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>��Q֧��</strong></td>
                </tr>
                 <tr id="HandQ" name="menu_two" style="display:none">
                  <td>
                          <uc1:HandQBusiness ID="HandQBusiness" runat="server"  Visible="false"/><!--��Qҵ��-->
                  </td>
                </tr>

                <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('CreditCardPayment')">
                 <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>���ÿ�����</strong></td>
                </tr>
                 <tr id="CreditCardPayment" name="menu_two" style="display:none">
                  <td>
                        <uc1:MenuControl runat="server" ID="CreditPayMenuControl"  Visible="false"/><!--���ÿ�����-->
                  </td>
                </tr>

                
                    <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('FastPay')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>���֧��</strong></td>
                </tr>
                 <tr id="FastPay" name="menu_two" style="display:none">
                  <td>
                         <uc1:MenuControl runat="server" ID="FastPayMenuControl"  Visible="false"/> <!--���֧��-->
                  </td>
                </tr>

                   <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('Tencentbusiness')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>����˾ҵ��</strong></td>
                </tr>
                 <tr id="Tencentbusiness" name="menu_two" style="display:none">
                  <td>
                        <uc1:MenuControl runat="server" ID="TencentbusinessMenuControl"  Visible="false"/> <!--����˾ҵ��-->
                  </td>
                </tr>

                <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('payManagement')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>֧������</strong></td>
                </tr>
                <tr id="payManagement" name="menu_two" style="display:none">
                  <td >
                     <%--   <uc1:WebchatPayControl ID="WebchatPayControl1" runat="server" /><!--΢��֧��-->--%>
                      <%--  <uc1:FastPay id="FastPay1" runat="server"></uc1:FastPay><!--���֧��-->--%>
                      <%--  <uc1:FundControl runat="server" ID="FundControl1" /><!--���ͨ-->--%>
                        <uc1:CreditPayControl ID="CreditPayControl1" runat="server"  Visible="false"/><!--����֧��-->
                        <uc1:OverseasPay id="OverseasPay1" runat="server" Visible="false"></uc1:OverseasPay><!--����֧��-->
                        <uc1:MicroPay id="MicroPay1" runat="server" Visible="false"></uc1:MicroPay><!--΢֧��--> 
                        <uc1:ForeignCurrencyPay ID="ForeignCurrencyPay1" runat="server"  Visible="false"/><!--���֧��-->
                        <uc1:ForeignCardPay ID="ForeignCardPay1" runat="server"  Visible="false"/><!--�⿨֧��-->
                      <%--  <uc1:HandQBusiness ID="HandQBusiness" runat="server" /><!--��Qҵ��-->--%>
                        <uc1:HKWalletPay runat="server" ID="HKWalletPay"  Visible="false"/> <!--HKǮ��֧��-->
                  </td>
                </tr>
               
                <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('bussInfo')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>�̻���Ϣ����</strong></td>
                </tr>
                <tr id="bussInfo" name="menu_two" style="display:none">
                  <td >
                        <uc1:AccountManage id="accountManage1" runat="server" Visible="false"></uc1:AccountManage><!--�̻�����-->
                        <uc1:PNRQuery id="PNRQuery1" runat="server" Visible="false"></uc1:PNRQuery><!--PNR��ѯ-->
			            <uc1:AccountLedgerManage id="AccountLedgerManage1" runat="server" Visible="false"></uc1:AccountLedgerManage><!--�̻�����-->
			            <uc1:AccountOperaManage id="AccountOperaManage1" runat="server" Visible="false"></uc1:AccountOperaManage><!--�̻�����-->
                        <uc1:DKManageControl id="DKManageControl1" runat="server" Visible="false"></uc1:DKManageControl><!--���۲�ѯ-->
                        <uc1:DFManageControl id="DFManageControl1" runat="server" Visible="false"></uc1:DFManageControl><!--������ѯ-->
                        <uc1:TravelPlatform ID="TravelPlatform1" runat="server"  Visible="false"/><!--����ƽ̨-->
                        <uc1:ForeignCurrencyAccount ID="ForeignCurrencyAccount1" runat="server"  Visible="false"/><!--����̻�����-->
                  </td>
                </tr>
                <tr style="cursor:pointer;" name="menu_one" onclick="javascript:showHidenMenu('sysManagement')">
                  <td style="background-image:url(images/page/menu_bk.gif); height:20px;">&nbsp;&nbsp;&nbsp;&nbsp;<strong>ϵͳ����</strong></td>
                </tr>
                <tr id="sysManagement" name="menu_two" style="display:none">
                  <td>
                      <%--  <uc1:BankBillManage id="BankBillManage1" runat="server"></uc1:BankBillManage><!--�����˵�-->--%>
                        <uc1:SysManage ID="SysManage1" runat="server"  Visible="false"/><!--�������-->
                        <uc1:FreezeManage id="FreezeManage1" runat="server" Visible="false"></uc1:FreezeManage><!--�������-->
			            <uc1:SpecialManageControl id="SpecialManageControl1" runat="Server" Visible="false"></uc1:SpecialManageControl><!--��־����-->
                        <uc1:MenuControl runat="server" ID="BGBatchMenuControl"  Visible="false"/> <!--BG��������-->
                  </td>
                </tr>
                 
            </table>
			
		</td>
		<td width="10" valign="top" class="Left_table_wh"><img src="../Images/Page/bg_head3.gif" alt="" width="10" height="490" /></td>
	</tr>
</table>
