<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCXGMoneyAndFlow.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCXGMoneyAndFlow" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>订单查询</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        .style2 {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }

        .container {
            margin: 20px;
            width: 1200px;
        }

            .container table {
                width: 100%;
                background-color: #808080;
            }

            .container caption {
                text-align: left;
                background: #b0c3d1;
                padding: 4px;
            }

            .container td, .container th {
                background: #fff;
                height: 20px;
                padding: 2px 4px;
            }

            .container .tab_info th {
                font-weight: lighter;
                width: 120px;
                text-align: right;
            }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div class="container">
            <table class="tab_input" cellpadding="1" cellspacing="1">
                <tr>
                    <td width="50%">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" />
                        <span class="style3">账户资金和流水查询</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">输入：
                        <asp:TextBox ID="txt_input" runat="server" />
                        <asp:RadioButton ID="checkWeChatId" runat="server" GroupName="chekedType" Text="WeChat ID" />
                        <asp:RadioButton ID="checkUin" runat="server" GroupName="chekedType" Text="钱包账户" />
                        <asp:RadioButton ID="checkUId" runat="server" GroupName="chekedType" Text="内部ID" Checked="true" />
                         &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <label>开始时间</label>
                        <asp:TextBox ID="txtStime" runat="server" class="Wdate" onclick="WdatePicker()"></asp:TextBox>
                         <label>结束时间</label>
                        <asp:TextBox ID="txtEtime" runat="server" class="Wdate" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- <span>开始时间：<input type="text" runat="server" id="txt_startDate" onclick="WdatePicker()" /><img onclick="txt_startDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" alt="选择日期" /></span>
                      <span>结束时间：<input type="text" runat="server" id="txt_endDate" onclick="WdatePicker()" /><img onclick="txt_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" alt="选择日期" /></span>  --%>
                    </td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="查询" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>
            <div>
                <br />
                <asp:LinkButton ID="btn_trade" runat="server" OnClick="SwitchHandler">交易单</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="btn_refund" runat="server" OnClick="SwitchHandler">退款单</asp:LinkButton>
                 &nbsp;
                <asp:LinkButton ID="btn_BankrollList" runat="server" OnClick="SwitchHandler">资金流水</asp:LinkButton>
                 &nbsp;
                <asp:LinkButton ID="btn_Fetch" runat="server" OnClick="SwitchHandler">提现</asp:LinkButton>
                <hr />
            </div>
            <asp:DataGrid ID="dg_trade" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="交易单">
                <HeaderStyle Font-Bold="True" Height="25px" />
                <Columns>
                    <asp:BoundColumn HeaderText="交易时间" DataField="acc_time" />
                    <asp:BoundColumn HeaderText="商户编号" DataField="spid" />
                    <asp:BoundColumn HeaderText="商户订单号" DataField="coding" />
                    <asp:BoundColumn HeaderText="MD订单号" DataField="listid" />
                    <asp:BoundColumn HeaderText="商户名称" DataField="sp_name" />
                    <asp:BoundColumn HeaderText="交易金额" DataField="paynum_str" />
                    <asp:BoundColumn HeaderText="交易类型" DataField="trade_type_str" />
                    <%--<asp:BoundColumn HeaderText="交易单状态" DataField="list_state_str" />--%>
                    <asp:BoundColumn HeaderText="交易状态" DataField="trade_state_str" />
                    <%--<asp:BoundColumn HeaderText="用户MD账号" DataField="" />--%>
                    <%--<asp:BoundColumn HeaderText="卡类型" DataField="card_type" />--%>
                    <asp:BoundColumn HeaderText="币种" DataField="price_curtype_str" />
                    <asp:BoundColumn HeaderText="应支付金额" DataField="bank_paynum_str" />
                    <asp:BoundColumn HeaderText="实际支付金额" DataField="bank_paynum_str" />
                </Columns>
            </asp:DataGrid>
            <asp:DataGrid ID="dg_refund" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="退款单">
                <HeaderStyle Font-Bold="True" Height="25px" />
                <Columns>
                    <asp:BoundColumn HeaderText=" 交易时间" DataField="acc_time" />
                    <asp:BoundColumn HeaderText=" 商户号" DataField="spid" />
                    <asp:BoundColumn HeaderText=" 商户订单号" DataField="coding" />
                    <asp:BoundColumn HeaderText=" MD订单号" DataField="MDList" />
                    <asp:BoundColumn HeaderText=" 退款订单号" DataField="refund_list" />
                    <asp:BoundColumn HeaderText=" 退款时间" DataField="refund_time" />
                    <asp:BoundColumn HeaderText=" 交易币种" DataField="bank_curtype_str" />
                    <asp:BoundColumn HeaderText=" 实际退款金额" DataField="total_payout_str" />
                    <asp:BoundColumn HeaderText=" 退款状态" DataField="refund_state_str" />
                </Columns>
            </asp:DataGrid>
              <asp:DataGrid ID="dg_fetch" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="提现">
                <HeaderStyle Font-Bold="True" Height="25px" />
                      <Columns>
                          <asp:BoundColumn HeaderText=" 账户帐号" DataField="user_uin" />
                          <asp:BoundColumn HeaderText=" 提现单号" DataField="listid" />
                          <asp:BoundColumn HeaderText=" 业务类型" DataField="fetch_type" />
                          <asp:BoundColumn HeaderText=" 科目" DataField="subject" />
                          <asp:BoundColumn HeaderText=" 提现金额" DataField="num" />
                          <asp:BoundColumn HeaderText=" 手续费" DataField="charge" />
                          <asp:BoundColumn HeaderText=" 出款账户银行ID" DataField="bank_acno" />
                          <asp:BoundColumn HeaderText=" 提现时间" DataField="create_time" />
                          <asp:BoundColumn HeaderText=" 付款时间" DataField="pay_time" />
                          <asp:BoundColumn HeaderText=" 回导时间" DataField="acc_time" />
                          <asp:BoundColumn HeaderText=" 开户名称" DataField="acc_name" />
                          <asp:BoundColumn HeaderText=" 银行账号" DataField="card_bankid" />
                         <%-- <asp:BoundColumn HeaderText=" 开户银行" DataField="" />--%>
                          <asp:BoundColumn HeaderText=" 提现状态" DataField="fetch_state" />
                          <asp:BoundColumn HeaderText=" 修改时间" DataField="modify_time" />
                      <%--    <asp:BoundColumn HeaderText=" 失败原因" DataField="" />--%>
                          <asp:BoundColumn HeaderText=" 退票原因" DataField="memo" />
                          <%--<asp:BoundColumn HeaderText=" 备注/说明" DataField="memo" />--%>
                </Columns>
            </asp:DataGrid>
              <asp:DataGrid ID="dg_BankrollList" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="资金流水">
                <HeaderStyle Font-Bold="True" Height="25px" />
                      <Columns>
                         <%-- <asp:BoundColumn HeaderText="用户账户" DataField="uin" />--%>
                           <asp:BoundColumn HeaderText="流水ID号" DataField="bkid" />
                           <asp:BoundColumn HeaderText="订单号" DataField="listid" />
                           <asp:BoundColumn HeaderText="交易时间" DataField="trade_time" />
                           <asp:BoundColumn HeaderText="交易类型" DataField="type" />
                           <asp:BoundColumn HeaderText="类别/科目" DataField="subject" />
                           <asp:BoundColumn HeaderText="金额" DataField="paynum" />
                           <asp:BoundColumn HeaderText="账户余额" DataField="balance" />
                           <asp:BoundColumn HeaderText="冻结金额" DataField="connum" />
                          <asp:BoundColumn HeaderText="冻结余额" DataField="con" />
                          <asp:BoundColumn HeaderText="对方账号" DataField="vs_info" />
                          <asp:BoundColumn HeaderText="备注/说明" DataField="memo" />
                           <asp:BoundColumn HeaderText="修改时间" DataField="modify_time" />
                        
                </Columns>
            </asp:DataGrid>
            <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" PageSize="10" ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" OnPageChanged="pager_PageChanged" Visible="false"></webdiyer:AspNetPager>
                
        </div>
    </form>
</body>
</html>
