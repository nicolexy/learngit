<%@ Page Language="c#" CodeBehind="ClearCreid.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ClearCreid" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ClearCreid</title>
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

</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table style="LEFT: 5%; POSITION: relative; TOP: 5%;" cellspacing="1" cellpadding="1" width="90%" border="1">
            <tr style="background-color: #e4e5f7;">
                <td style="WIDTH: 60%">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">
                    &nbsp;
                    <asp:Label ID="Label5" ForeColor="Red" Text=" 证件清理查询日志" runat="server"></asp:Label>
                </td>
                <td style="WIDTH: 40%">
                    <asp:Label ID="Label6" runat="server" Text="操作员代码: "></asp:Label>
                    <asp:Label ID="Label7" runat="server" ForeColor="Red" Width="73px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label8" runat="server">身份证号码：</asp:Label>
                    <asp:TextBox ID="txtCardId" Style="WIDTH: 250px;" runat="server"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                        BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FCreid" HeaderText="身份证号码"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FCreate_time" HeaderText="操作时间"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FUser_type" HeaderText="用户属性"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FUid" HeaderText="操作人"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>

        <table style="LEFT: 5%; POSITION: relative; TOP: 10%;" cellspacing="1" cellpadding="1" width="90%"
            border="1">
            <tr style="background-color: #e4e5f7;">
                <td style="WIDTH: 60%">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">
                    &nbsp;
                    <asp:Label ID="Label1" ForeColor="Red" Text=" 证件号码清理" runat="server"></asp:Label>
                </td>
                <td style="WIDTH: 40%">
                    <asp:Label ID="Label3" runat="server" Text="操作员代码: "></asp:Label>
                    <asp:Label ID="Label4" runat="server" ForeColor="Red" Width="73px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server">身份证号码：</asp:Label>
                    <asp:TextBox ID="creid" Style="WIDTH: 250px;" runat="server"></asp:TextBox>&nbsp;&nbsp;
                      <asp:RadioButton ID="rbpt" runat="server" Text="普通用户" Checked="True" GroupName="id" ToolTip="普通用户"></asp:RadioButton>
                    <asp:RadioButton ID="rbwx" runat="server" Text="微信用户" GroupName="id" ToolTip="微信用户"></asp:RadioButton>
                </td>
                <td align="center">
                    <asp:Button ID="btnClear" runat="server" Width="80px" Text="清 理" OnClick="btnClear_Click"></asp:Button>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
