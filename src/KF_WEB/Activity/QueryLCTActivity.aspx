<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page Language="c#" CodeBehind="QueryLCTActivity.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Activity.QueryLCTActivity" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ComplainBussinessInput</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        UNKNOWN {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellspacing="1" cellpadding="1" width="920"
            border="1">
            <tr>
                <td style="WIDTH: 100%" bgcolor="#e4e5f7" colspan="4"><font face="宋体"><font color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;理财通活动查询</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </font>操作员代码: </FONT><span class="style3"><asp:Label ID="Label1" runat="server" ForeColor="Red" Width="73px"></asp:Label></span></td>
            </tr>
            <tr>

                <td align="right">
                    <asp:Label ID="Label5" runat="server">开始日期：</asp:Label></td>
                <td>
                    <%--<asp:TextBox ID="TextBoxBeginDate" runat="server"></asp:TextBox><asp:ImageButton ID="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:ImageButton>--%>
                    <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                    <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                     </td>
                <td align="right">
                    <asp:Label ID="Label4" runat="server">结束日期：</asp:Label></td>
                <td>
                    <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                    <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                    <%--<asp:TextBox ID="TextBoxEndDate" runat="server"></asp:TextBox><asp:ImageButton ID="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:ImageButton>--%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server">财付通账号：</asp:Label></td>
                <td>
                    <asp:TextBox ID="txtCftNo" Style="WIDTH: 300px;" runat="server"></asp:TextBox></td>
                <td align="right">
                    <asp:Label ID="Label2" runat="server">活动名称：</asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlActId" runat="server">
                        <asp:ListItem Value="lct" Selected="True">理财通活动</asp:ListItem>
                        <asp:ListItem Value="userfbsyk" Selected="false">用户翻倍收益卡</asp:ListItem>
                    </asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click"></asp:Button>
            </tr>
        </table>
        <table id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 154px; HEIGHT: 35%"
            cellspacing="1" cellpadding="1" width="920px" border="1" runat="server">
            <tr>
                <td valign="top" colspan="16">
                    <asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                        BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FPayUin" HeaderText="微信支付账号"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FSPID" HeaderText="商户号"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FTransId" HeaderText="资格单号"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FMoneyStr" HeaderText="申购金额"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FExpiredTime" HeaderText="资格过期时间"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FStateStr" HeaderText="用户状态"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FUserTypeStr" HeaderText="用户类型"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FPrizeTypeStr" HeaderText="抽中等级"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FPrizeMoneyStr" HeaderText="抽中金额（元）"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FPrizeTime" HeaderText="抽奖时间"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FPrizeModifyTime" HeaderText="更新时间"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FStandby2" HeaderText="备注"></asp:BoundColumn>
                            <asp:ButtonColumn Text="详情" HeaderText="操作" CommandName="detail"></asp:ButtonColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
            <tr>
                <td valign="top" colspan="16">
                    <asp:DataGrid ID="DataGrid2" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                        BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FUin" HeaderText="用户iD"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FType" HeaderText="使用限制"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FRate" HeaderText="翻倍倍率"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FProfitDays" HeaderText="翻倍天数"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FStatus" HeaderText="翻倍卡使用状态"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FDealTime" HeaderText="翻倍卡使用时间"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FExpiredTime" HeaderText="翻倍卡到期时间"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FProfitStartDay" HeaderText="收益计算起始日期"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FProfitEndDay" HeaderText="收益计算结束日期"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FProfitInfo" HeaderText="参与计算收益的基金列表"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
            <tr height="25">
                <td colspan="16">
                    <webdiyer:AspNetPager ID="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
                        PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
                        SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
                    </webdiyer:AspNetPager>
                </td>
            </tr>
            <tr bgcolor="#e4e5f7">
                <td align="left" colspan="16" style="height: 20px;">
                    <font><b>详细信息</b></font>
                </td>
            </tr>
            <tr bgcolor="#e4e5f7">
                <td style="height: 25px;">活动号</td>
                <td style="height: 25px;">活动名称</td>
                <td style="height: 25px;">申购单号</td>
                <td>赠送状态</td>
                <td>批次号</td>
                <td>申购基金</td>
                <td>发券时间</td>
                <td>第一个收益日期</td>
                <td>奖品创建时间</td>
                <td>奖品失效时间</td>
                <td>赠送流水</td>
                <td>理财通openid</td>
                <td>错误信息</td>
                <td>渠道号</td>
                <td>活动分类</td>
                <td>奖品类型</td>
                <td>奖品名称</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lb_ActId" runat="server"></asp:Label></td>
                 <td>
                    <asp:Label ID="lb_ActName" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_TransId" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_State" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_BatchId" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_Spname" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_SendTicketTime" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_StartDate" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_CreateTime" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_ExpireTime" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_GivePosId" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_Openid" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_ErrorInfo" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_ChannelId" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lb_FActType" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lb_FPrizeType" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lb_FPrizeName" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
