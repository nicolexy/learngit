<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page Language="c#" CodeBehind="FreezeNewQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FreezeManage.FreezeNewQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>FreezeVerify</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        UNKNOWN {
            color: #000000;
        }

        .style3 {
            color: #ff0000;
        }

        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }

        .wd140 {
            min-width: 140px;
        }
        .wd260 {
            width:260px;
            max-width:300px;
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table border="1" cellspacing="1" cellpadding="1" width="1200">
            <tr>
                <td style="width: 443px; height: 20px" bgcolor="#e4e5f7" colspan="2">
                    <font color="red" face="宋体">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16" alt=""><asp:Label ID="lb_pageTitle"
                            runat="server">特殊申诉处理</asp:Label></font>
                </td>
                <td style="height: 20px">
                    <font>操作员代码: </font><span class="style3">
                        <asp:Label ID="lb_operatorID" runat="server"
                            Width="73px" ForeColor="Red"></asp:Label></span>
                </td>
            </tr>
            <tr>
                <td>
                    <label style="vertical-align: middle; width: 80px; height: 20px">
                        请输入：</label><asp:TextBox ID="tbx_payAccount" runat="server" Width="140px"></asp:TextBox>
                </td>
                <td colspan="3">
                    <input id="WeChatId" name="IDType" runat="server" type="radio" checked /><label for="WeChatId">微信帐号</label>
                    <input id="WeChatQQ" name="IDType" runat="server" type="radio" /><label for="WeChatQQ">微信绑定QQ</label>
                    <input id="WeChatMobile" name="IDType" runat="server" type="radio" /><label for="WeChatMobile">微信绑定手机</label>
                    <input id="WeChatEmail" name="IDType" runat="server" type="radio" /><label for="WeChatEmail">微信绑定邮箱</label>
                    <input id="WeChatUid" name="IDType" runat="server" type="radio" /><label for="WeChatUid">微信内部ID</label>
                    <input id="WeChatCft" name="IDType" runat="server" type="radio" /><label for="WeChatCft">微信财付通账号</label>
                </td>
            </tr>
            <tr>
                <td>
                    <label style="vertical-align: middle; width: 80px; height: 20px">
                        开始时间：</label>
                        <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                </td>
                <td>
                    <label style="vertical-align: middle; width: 80px; height: 20px">
                        结束时间：</label>
                        <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                </td>
                <td colspan="2">

                    <label style="vertical-align: middle; width: 80px; height: 20px">
                        申诉类型：</label>
                    <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                        <asp:ListItem Value="8">普通解冻</asp:ListItem>
                        <asp:ListItem Value="19">微信解冻</asp:ListItem>
                        <asp:ListItem Value="11">特殊找回支付密码</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                <label style="vertical-align: middle; width: 80px; height: 20px">
                    订单状态：</label>
                    <asp:DropDownList ID="ddl_orderState" runat="server">
                        <asp:ListItem Value="0" Selected="True">未处理</asp:ListItem>
                        <asp:ListItem Value="1">结单（已解冻）</asp:ListItem>
                        <asp:ListItem Value="2">待补充资料</asp:ListItem>
                        <asp:ListItem Value="7">已作废</asp:ListItem>
                        <asp:ListItem Value="8">挂起</asp:ListItem>
                        <asp:ListItem Value="10">已补充资料</asp:ListItem>
                        <asp:ListItem Value="21">结单（无冻结日志）</asp:ListItem>
                        <asp:ListItem Value="22">结单（账户不处理冻结状态）</asp:ListItem>
                        <asp:ListItem Value="99">所有</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddl_orderStateSpecial" runat="server" Visible="false">
                        <asp:ListItem Value="99">所有</asp:ListItem>
                        <asp:ListItem Value="0" Selected="True">未处理</asp:ListItem>
                        <asp:ListItem Value="1">申诉成功</asp:ListItem>
                        <asp:ListItem Value="2">申诉失败</asp:ListItem>
                        <%--<asp:ListItem Value="3">大额待复核</asp:ListItem>
							<asp:ListItem Value="4">直接转后台</asp:ListItem>
							<asp:ListItem Value="5">异常转后台</asp:ListItem>
							<asp:ListItem Value="6">发邮件失败</asp:ListItem>--%>
                        <asp:ListItem Value="7">已删除</asp:ListItem>
                        <%--<asp:ListItem Value="8">已锁定状态</asp:ListItem>
							<asp:ListItem Value="9">短信撤销状态</asp:ListItem>
							<asp:ListItem Value="10">直接申诉成功</asp:ListItem>--%>
                        <asp:ListItem Value="11">待补充资料</asp:ListItem>
                        <asp:ListItem Value="12">已补充资料</asp:ListItem>
                        <asp:ListItem Value="20">结单（未补填资料）</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label style="vertical-align: middle; width: 80px; height: 20px">
                        工单号：</label><asp:TextBox ID="tbx_listNo" runat="server" Width="140px"></asp:TextBox>
                </td>
                <td>
                    <label style="vertical-align: middle; width: 80px; height: 20px">
                        结单人员：</label><asp:TextBox ID="tbx_people" runat="server" Width="140px"></asp:TextBox>
                </td>
                <td>
                    <label style="vertical-align: middle; width: 80px; height: 20px">
                        冻结原因：</label><asp:TextBox ID="tbx_reason" runat="server" Width="140px"></asp:TextBox>
             <%--       <asp:Label runat="server" ID="channel_title">冻结渠道：</asp:Label>
                    <asp:DropDownList ID="ddl_channel" runat="server">
                        <asp:ListItem Value="0">所有</asp:ListItem>
                        <asp:ListItem Value="1">风控冻结</asp:ListItem>
                        <asp:ListItem Value="2">拍拍冻结</asp:ListItem>
                        <asp:ListItem Value="3">用户冻结</asp:ListItem>
                        <asp:ListItem Value="4">商户冻结</asp:ListItem>
                        <asp:ListItem Value="5">BG接口冻结</asp:ListItem>
                        <asp:ListItem Value="6">涉嫌可以交易冻结</asp:ListItem>
                    </asp:DropDownList>--%>
                </td>
                <td colspan="2">
                    <label style="vertical-align: middle">
                        记录条数：</label><asp:Label ID="lb_count" runat="server" Text="0" />
                </td>
            </tr>
            <tr>
                <td>
                    <label style="vertical-align: middle; width: 80px; height: 20px">
                        排序方式：</label>
                    <asp:DropDownList runat="Server" ID="ddl_queryOrderType">
                        <asp:ListItem Value="1" Selected="True">提交时间最早优先</asp:ListItem>
                        <asp:ListItem Value="2">提交时间最迟优先</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="3" align="center">
                    <asp:Button ID="btn_query" runat="server" Text="查 询" Width="120px" OnClick="btn_query_Click"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellspacing="0" cellpadding="0" width="1200" align="left">
            <tr>
                <td valign="top" align="left">
                    <asp:DataGrid ID="DataGrid_QueryResult" runat="server" Width="1200px" BorderColor="#E7E7FF"
                        BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal"
                        AutoGenerateColumns="False" PageSize="5" HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center" Font-Size="13px">
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FID" HeaderText="工单号">
                                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Fuin" HeaderText="账号">
                                <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FreezeReason" HeaderText="冻结原因">
                                <HeaderStyle HorizontalAlign="Center" CssClass="wd260"></HeaderStyle>
                            </asp:BoundColumn>
                          <%--  <asp:BoundColumn DataField="channel_str" HeaderText="冻结渠道">
                                <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                            </asp:BoundColumn>--%>
                            <asp:BoundColumn DataField="FsubmitTime" HeaderText="提交时间">
                                <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Fsuptime" HeaderText="补充资料时间">
                                <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="handleStateName" HeaderText="订单状态">
                                <HeaderStyle CssClass="wd140" HorizontalAlign="Center" Width="140px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="handleUserName" HeaderText="结单人员">
                                <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:HyperLinkColumn Text="处理" Target="_blank" DataNavigateUrlField="OpUrl" HeaderText="操作">
                                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn Text="日志" Target="_blank" DataNavigateUrlField="DiaryUrl" HeaderText="日志">
                                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                            </asp:HyperLinkColumn>
                            <asp:BoundColumn Visible="False" DataField="Fuincolor"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <webdiyer:AspNetPager ID="pager" runat="server" HorizontalAlign="right" AlwaysShow="True"
                        NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" CssClass="mypager"
                        ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5">
                    </webdiyer:AspNetPager>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
