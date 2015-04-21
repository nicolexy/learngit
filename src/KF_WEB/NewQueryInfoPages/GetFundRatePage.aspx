<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page Language="c#" CodeBehind="GetFundRatePage.aspx.cs" AutoEventWireup="True"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.GetFundRatePage" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>GetFundRatePage</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
        UNKNOWN
        {
            color: #000000;
        }
        .style3
        {
            color: #ff0000;
        }
        BODY
        {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
    </style>
    <script language="javascript">
        function openModeBegin() {
            var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.tbx_beginDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
            if (returnValue != null) Form1.tbx_beginDate.value = returnValue;
        }
        function openModeEnd() {
            var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.tbx_endDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
            if (returnValue != null) Form1.tbx_endDate.value = returnValue;
        }
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table border="1" cellspacing="1" cellpadding="1" width="1100">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">查询用户余额收益情况</font>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lb_QQ" runat="server">输入：</asp:Label><asp:TextBox ID="TextBox1_InputQQ"
                    runat="server" Width="350px"></asp:TextBox>  &nbsp;  &nbsp;
                    <input id="WeChatId" name="IDType" runat="server" type="radio" checked/><label for="WeChatId">微信帐号</label>
                            <input id="WeChatQQ" name="IDType" runat="server" type="radio" /><label for="WeChatQQ">微信绑定QQ</label>
                            <input id="WeChatMobile" name="IDType" runat="server" type="radio" /><label for="WeChatMobile">微信绑定手机</label>
                            <input id="WeChatEmail" name="IDType" runat="server" type="radio" /><label for="WeChatEmail">微信绑定邮箱</label>
                            <input id="WeChatUid" name="IDType" runat="server" type="radio" /><label for="WeChatUid">微信内部ID</label>
                            <input id="WeChatCft" name="IDType" runat="server" type="radio" /><label for="WeChatCft">微信财付通账号Or手Q账号</label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table border="1" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td align="left">
                            用户姓名：
                            <asp:Label ID="lblName" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            开户状态：
                            <asp:Label ID="lblAccountStatus" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            绑定手机：
                            <asp:Label ID="lblCell" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            开户时间：
                            <asp:Label ID="lblCreateTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            安全卡类型：
                            <asp:Label ID="lblSafeBankCardType" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            安全卡尾号：
                            <asp:Label ID="lblSafeBankCardNoTail" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            累计收益：
                            <asp:Label ID="lblTotalProfit" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            所购份额：
                            <asp:Label ID="lblBalance" runat="server"></asp:Label>
                        </td>
                     </tr>
                    <tr>
                        <td align="left">
                            理财通余额：
                            <asp:Label ID="lbLCTBalance" runat="server"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <asp:Button ID="btnBalanceQuery" runat="server" Width="250px" Text="资金流水查询" OnClick="btnBalanceQuery_Click">
                            </asp:Button>
                        </td>
                     </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
         <table id="tableLCTBalanceRoll" visible="false" border="1" cellspacing="0" cellpadding="0" width="1100" runat="server" >
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">理财通余额流水</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgLCTBalanceRollList" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                         <asp:BoundColumn DataField="Flistid" HeaderText="订单号">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="Fcreate_time" HeaderText="交易时间">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Facc_time" HeaderText="对账时间">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FtypeStr" HeaderText="交易类型">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="FInOrOUT" HeaderText="出\入">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="Fchannel_idStr" HeaderText="渠道号">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="FstateStr" HeaderText="交易状态">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftotal_feeStr" HeaderText="金额">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fmemo" HeaderText="备注">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                    </Columns>
                <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="BalanceRollPager"  runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="BalanceRollPager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
          </br>
    <table border="1" cellspacing="0" cellpadding="0" width="1100">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">用户各基金账户</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgUserFundSummary" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                       <asp:BoundColumn DataField="Fspid" HeaderText="Fspid" Visible="false">
                         </asp:BoundColumn>
                       <asp:BoundColumn DataField="Fcurtype" HeaderText="Fcurtype" Visible="false">
                       </asp:BoundColumn>
                         <asp:BoundColumn DataField="fund_code" HeaderText="基金代码" Visible="false">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="close_flag" Visible="false" >
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="fundName" HeaderText="基金名称">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="close_flagText" HeaderText="封闭标志">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="transfer_flagText" HeaderText="转换交易">
                            <HeaderStyle Width="180px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="buy_validText" HeaderText="购买类型">
                            <HeaderStyle Width="180px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="profitText" HeaderText="累计收益">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="balanceText" HeaderText="余额">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="conText" HeaderText="冻结金额">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:ButtonColumn Text="详情" HeaderText="操作" CommandName="detail"></asp:ButtonColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </br>
     <table border="1" cellspacing="1" cellpadding="1" width="1100">
        <tr>
            <td>
             <label>
                    查询开始时间：</label><asp:TextBox ID="tbx_beginDate" runat="server"></asp:TextBox><asp:ImageButton
                        ID="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif">
                    </asp:ImageButton><label>&nbsp;&nbsp;查询结束时间：</label><asp:TextBox ID="tbx_endDate"
                        runat="server"></asp:TextBox><asp:ImageButton ID="ButtonEndDate" runat="server" CausesValidation="False"
                            ImageUrl="../Images/Public/edit.gif"></asp:ImageButton>
                  <span id="queryDiv"  runat="server">
                    <label>
                    出入：</label><asp:DropDownList ID="ddlDirection" runat="server">
                        <asp:ListItem Selected="True" Value="0">全部</asp:ListItem>
                        <asp:ListItem Value="1">存入</asp:ListItem>
                        <asp:ListItem Value="2">取出</asp:ListItem>
                    </asp:DropDownList>
                    <label>
                    备注：</label><asp:DropDownList ID="ddlMemo" runat="server">
                        <asp:ListItem Selected="True" Value="">全部</asp:ListItem>
                        <asp:ListItem Value="基金申购">基金申购</asp:ListItem>
                        <asp:ListItem Value="基金收益">基金收益</asp:ListItem>
                        <asp:ListItem Value="余额宝子账户提现">提现</asp:ListItem>
                    </asp:DropDownList>
                    </span>
                       <asp:Button ID="btnQueryDetail" runat="server" Width="80px" Text="查 询" OnClick="btnQueryDetail_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <br />
    <table id="tableQueryResult"  border="1" cellspacing="0" cellpadding="0" width="1100" runat="server" >
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">查询用户余额收益情况明细</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="DataGrid_QueryResult" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fday" HeaderText="交易时间">
                            <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fspname" HeaderText="基金公司名称">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fpur_typeName" HeaderText="科目">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fprofit_per_ten_thousand" HeaderText="万份收益">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="F7day_profit_rate_str" HeaderText="七日年化收益率">
                            <HeaderStyle Width="110px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fvalid_money_str" HeaderText="收益本金额">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fprofit_str" HeaderText="收益金额">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="pager" runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="pager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
    <br />
    <table id="tableBankRollList" border="1" cellspacing="0" cellpadding="0" width="1100" runat="server">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">查询用户资金流水情况</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgBankRollList" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF" OnItemDataBound="dgBankRollList_ItemDataBound">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fcreate_time" HeaderText="交易时间">
                            <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Flistid" HeaderText="订单号">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FtypeText" HeaderText="存取">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FpaynumText" HeaderText="金额">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FbalanceText" HeaderText="账户余额">
                            <HeaderStyle Width="110px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FconStr" HeaderText="冻结余额">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FmemoText" HeaderText="备注">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fspid" HeaderText="商户号">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="操作">
							<ItemTemplate>
								<asp:LinkButton id="UnCloseFundApplyButton" href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank Visible="false" runat="server" Text="客服强赎"></asp:LinkButton>
							</ItemTemplate>
					   </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="bankRollListPager" runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="bankRollListPager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
     <br />
    <table  id="tableBankRollListNotChildren" border="1" cellspacing="0" cellpadding="0" width="1100" runat="server">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">查询用户交易流水情况</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgBankRollListNotChildren" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Flistid" HeaderText="交易单号">
                            <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <%--<asp:BoundColumn DataField="Fsub_trans_id_str" HeaderText="订单号">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>--%>
                         <asp:BoundColumn DataField="Ffetchid" HeaderText="提现单号">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FtypeText" HeaderText="存取">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftotal_fee_str" HeaderText="金额">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Floading_type_str" HeaderText="赎回方式">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fstate_str" HeaderText="状态">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="Fcard_no" HeaderText="银行卡尾号">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="Fbank_type_str" HeaderText="银行类型">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="bankRollListNotChildrenPager" runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="bankRollListNotChildrenPager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
      <br />
    <table  id="tableCloseFundRoll" border="1" cellspacing="0" cellpadding="0" width="1100" runat="server">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">查询交易明细</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgCloseFundRoll" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF" OnItemDataBound="dgCloseFundRoll_ItemDataBound">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fseqno" HeaderText="序号">
                            <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftrade_id" HeaderText="交易单号">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FDate" HeaderText="期号">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fstart_total_fee_str" HeaderText="总金额（本金）">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fcurrent_total_fee_str" HeaderText="当前总金额">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fend_tail_fee_str" HeaderText="扫尾金额">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftrans_date" HeaderText="基金交易日">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fstart_date" HeaderText="起息日">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fend_date" HeaderText="到期日">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fstate_str" HeaderText="绑定状态">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fuser_end_type_str" HeaderText="到期策略">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fpay_type_str" HeaderText="支付类型">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fchannel_id_str" HeaderText="渠道信息">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                       <%-- <asp:TemplateColumn HeaderText="操作">
							<ItemTemplate>
								<a href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank>客服强赎</a>
							</ItemTemplate>
						</asp:TemplateColumn>--%>
                         <asp:TemplateColumn HeaderText="操作">
                         <HeaderStyle Width="200px"></HeaderStyle>
							<ItemTemplate>
								<asp:LinkButton id="CloseFundApplyButton" href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank Visible="false" runat="server" Text="客服强赎"></asp:LinkButton>
							</ItemTemplate>
					   </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="CloseFundRollPager" runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="CloseFundRollPager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
