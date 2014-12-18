<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CashOutFreezeQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.CashOutFreezeQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>CashOutFreeanQuery</title>
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
        <table id="Table1" style="Z-INDEX: 103; POSITION: absolute; TOP: 5%; LEFT: 5%; vertical-align: middle" cellspacing="1" cellpadding="1" width="90%" border="1">
            <tr style="background-color: #e4e5f7;">
                <td style="WIDTH: 60%">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">
                    &nbsp;
                    <asp:Label ID="Label5" ForeColor="Red" Text=" 提现冻结资金查询" runat="server"></asp:Label>
                </td>
                <td style="WIDTH: 40%">
                    <asp:Label ID="Label6" runat="server" Text="操作员代码: "></asp:Label>
                    <asp:Label ID="lblFuid" runat="server" ForeColor="Red" Width="73px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 60%;">
                    <asp:Label ID="Label3" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;内部ID:&nbsp;&nbsp;&nbsp;</asp:Label>
                    <asp:TextBox ID="txtFuid" runat="server" BorderStyle="Groove"></asp:TextBox>
                </td>
                <td style="width: 40%; text-align: center">
                    <asp:Button ID="btnQuery" Width="80px" runat="server" Text=" 查  询 " OnClick="btnQuery_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; height: 30px;" colspan="2">
                    <asp:Label ID="Label2" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;提现单号:</asp:Label>
                    <asp:Label ID="lblCashOutBillNo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
