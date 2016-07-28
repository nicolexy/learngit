<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClearCreidNew.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ClearCreidNew" %>

<!DOCTYPE HTML PUBLIC="-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ClearCreidNew</title>
    <meta content="Microsoft Visual Studio .NET 7.1 " name="generator" />
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <style type="text/css">
        @import url(../Styles/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %>);

        UNKNOWN {
            color:#000000;
        }
        .style3 {
            color:#ff0000;
        }
        BODY {
            background-image:url(../Images/Page/bg01.gif);
        }

    </style>
</head>
<body ms_positioning="GridLayout">
    <form id="form1" method="post" runat="server">
        
        <table style="left:5%; position:relative; top:5%;" cellspacing="1" cellpadding="1" width="90%" border="1">
            <tr style="background-color:#e4e5f7;">
                <td style="width:60%;">
                    <img src="../Images/Page/post.gif"/ height="16" width="20" /> &nbsp;
                    <asp:label ID="lblTitle" ForeColor="Red" Text="证件清理查询日志" runat="server"></asp:label>
                </td>
                <td style="width:40%;">
                    <asp:Label ID="lblOperator1" Text="操作员代码：" runat="server"></asp:Label>
                    <asp:Label ID="lblCode1" ForeColor="Red" Text="Operator" runat="server" Width="73px"></asp:Label>
                </td>
            </tr>

            <tr>
                <td align="left">
                    <asp:Label ID="lblCreid" runat="server" Text="身份证号码："></asp:Label>
                    <asp:TextBox ID="txtCreid" runat="server" Width="250px"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="btnQuery" Text="查询" runat="server" Width="80px" OnClick="btnQuery_Click" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" BackColor="White"
                         CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="false" Width="100%">
                        <FooterStyle  ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle  Font-Bold="true" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="true" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>

                        <Columns>
                            <asp:BoundColumn DataField="FCreid" HeaderText="身份证号码"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FCreate_time" HeaderText="操作时间"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FUser_type" HeaderText="用户属性"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FUid" HeaderText="操作人"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>

        <table style="left:5%; position:relative; top:10%;" cellspacing="1" cellpadding="1" width="90%" border="1">
            <tr style="background-color:#e4e5f7;">
                <td style="width:60%;">
                    <img src="../Images/Page/post.gif" height="16" width="20" />&nbsp;
                    <asp:Label ID="lblTitle2" Text="证件号码清理" runat="server"></asp:Label>
                </td>
                <td style="width:40%;">
                    <asp:Label ID="lblOperator2" Text="操作员代码：" runat="server"></asp:Label>
                    <asp:Label ID="lblCode2" Text="Operator" runat="server" ForeColor="Red" Width="73px"></asp:Label>
                </td>
            </tr>

            <tr>
                <td align="left">
                    <asp:Label ID="lblCreid2" runat="server" Text="身份证号码："></asp:Label>
                    <asp:TextBox ID="txtCreid2" Style="width:250px;" runat="server"></asp:TextBox>&nbsp;&nbsp;
                    <asp:RadioButton ID="rbtPT" Text="普通用户" GroupName="type" runat="server" Checked="true" ToolTip="普通用户"></asp:RadioButton>
                    <asp:RadioButton ID="rbtWX" Text="微信用户" GroupName="type" runat="server" ToolTip="微信用户"></asp:RadioButton>
                </td>
                <td align="center">
                    <asp:Button ID="btnClear" Text="清理" runat="server" Width="80px" OnClick="btnClear_Click" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
