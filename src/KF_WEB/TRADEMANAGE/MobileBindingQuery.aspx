<%@ Page Language="c#" CodeBehind="MobileBindingQuery.aspx.cs" AutoEventWireup="True"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MobileBindingQuery" %>

<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>MobileBindQuery</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
        .style2
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
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="Table1" style="z-index: 101; position: absolute; top: 5%; left: 5%" cellspacing="1"
        cellpadding="1" width="85%" border="1">
        <tr>
            <td bgcolor="#e4e5f7">
                <font face="宋体" color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20" alt="" />
                    &nbsp;&nbsp;手机绑定查询</font>
            </td>
            <td align="right" bgcolor="#e4e5f7">
                <font face="宋体">操作员代码: <span class="style3">
                    <asp:Label ID="Label1" runat="server" Width="73px"></asp:Label></span></font>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="Label5" runat="server">财付通帐号</asp:Label><asp:TextBox ID="txbQQ" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click">
                </asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnUpdateBindInfo" runat="server" Text="更新绑定状态" OnClick="btnUpdateBindInfo_Click">
                </asp:Button>
            </td>
        </tr>
        
    </table>
    <table id="Table2" style="z-index: 102; position: absolute; width: 85%; height: 70%;
        top: 100px; left: 5.02%" cellspacing="1" cellpadding="1" width="808" border="1"
        runat="server">
        <tr>
            <td valign="top">
                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF" OnItemCommand="DataGrid1_ItemCommand">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fuid" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fqqid" HeaderText="财付通帐号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Femail" HeaderText="邮箱"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fmobile" HeaderText="手机号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MobileState" HeaderText="手机状态"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MsgState" HeaderText="短信提醒"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MobilePayState" HeaderText="手机支付"></asp:BoundColumn>
                        <asp:ButtonColumn DataTextField="Unbind" HeaderText="解绑" CommandName="Select"></asp:ButtonColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
