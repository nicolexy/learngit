<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TencentCreditQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.TencentCreditQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>腾讯信用查询</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
        <table id="table1" border="1" cellspacing="1" cellpadding="1" width="900" runat="server">
            <tr>
                <td width="50%">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" /><label class="style3">腾讯信用查询</label></td>
                <td>
                    <label class="style3">操作员ID：</label><asp:Label ID="lb_operatorID" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:Label ID="Label1" runat="server" Width="80" Font-Size="15px">QQ号：</asp:Label>
                    <asp:TextBox ID="tbx_uin" Width="250px" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查询" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr align="center">
                <td>信用等级</td>
                <td>账户排名</td>
            </tr>
            <tr align="center">
                <td>
                    <span runat="server" id="credit_star"></span>
                </td>
                <td>
                    <div runat="server" id="credit_rank"></div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
