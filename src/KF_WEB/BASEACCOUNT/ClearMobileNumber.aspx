<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClearMobileNumber.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ClearMobileNumber" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>手机号码清理</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );

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
        <table style="left: 5%; POSITION: relative; TOP: 5%; width: 1000px;" cellspacing="1" cellpadding="1" width="90%" border="1">
            <tr style="background-color: #e4e5f7;">
                <td style="WIDTH: 60%">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20" alt="" />
                    &nbsp;
                    <asp:Label ID="Label5" ForeColor="Red" Text=" 手机号码清零" runat="server"></asp:Label>
                </td>
                <td style="WIDTH: 40%">
                    <asp:Label ID="Label6" runat="server" Text="操作员代码: "></asp:Label>
                    <asp:Label ID="Label7" runat="server" ForeColor="Red" Width="73px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <span>手机号码：</span>
                    <asp:TextBox ID="txt_mobile" Style="WIDTH: 250px;" runat="server"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click"></asp:Button>
                </td>
            </tr>
            <tr style="background-color: #e4e5f7;">
                <td colspan="2">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20" alt="" />
                    &nbsp;
                    <asp:Label ID="Label2" ForeColor="Red" Text=" 清零日记" runat="server"></asp:Label>
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
                            <asp:BoundColumn DataField="FMobile" HeaderText="手机号码"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FCreate_time" HeaderText="操作时间"></asp:BoundColumn>
                            <%--<asp:BoundColumn DataField="FUser_type" HeaderText="用户属性"></asp:BoundColumn>--%>
                            <asp:BoundColumn DataField="Fsubmit_user" HeaderText="操作人"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <br /> <br />
                </td>
            </tr>
            <tr style="background-color: #e4e5f7;">
                <td style="WIDTH: 60%">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20" alt="" />
                    &nbsp;
                    <asp:Label ID="Label1" ForeColor="Red" Text=" 手机号码清零" runat="server"></asp:Label>
                </td>
                <td style="WIDTH: 40%">操作
                </td>
            </tr>
            <tr>
                <td align="left">
                    <span>当前绑定次数：</span>
                    <asp:Label ID="BindCount" Style="WIDTH: 250px;" runat="server" ForeColor="Red"></asp:Label>
                </td>
                <td align="center">
                    <asp:Button ID="Button1" runat="server" Width="80px" Text="清 零" OnClick="Button1_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
