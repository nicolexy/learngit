<%@ Page Language="c#" CodeBehind="PropertyTurnover.aspx.cs" AutoEventWireup="True"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.VIPAccount.PropertyTurnover" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>PropertyTurnover</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="VBScript">
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
    <script src="../SCRIPTS/Local.js"></script>
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
    <form id="Form1" method="post" runat="server" style="font-family: 宋体">
    <table style="position: absolute; top: 5%; left: 5%" id="Table1" border="1" cellspacing="1"
        cellpadding="1" width="800">
        <tbody>
            <tr style="background-color: #e4e5f7">
                <td colspan="4">
                    <font color="red">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;财付值交易流水查询</font>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <label>
                        开始日期:</label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbx_beginDate" Width="120"></asp:TextBox>
                    <asp:ImageButton ID="btnBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif"
                        CausesValidation="False"></asp:ImageButton>
                </td>
                <td width="100">
                    <label>
                        结束日期:</label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbx_endDate" Width="120"></asp:TextBox>
                    <asp:ImageButton ID="btnEndDate" runat="server" ImageUrl="../Images/Public/edit.gif"
                        CausesValidation="False"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td width="80">
                    <label>
                        账号:</label>
                </td>
                <td>
                    <asp:TextBox ID="tbx_acc" Width="250px" runat="server"></asp:TextBox>
                </td>
                <td width="80">
                    <label>
                        订单号:</label>
                </td>
                <td>
                    <asp:TextBox ID="tbx_order" Width="250px" runat="server" Style="z-index: 0"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button Text="补发" runat="server" ID="btn_resend" Width="100px" Style="display: none">
                    </asp:Button>
                </td>
                <td colspan="4" align="center">
                    <asp:Button Text="查询" runat="server" ID="btn_query" Width="100px"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataGrid ID="DataGrid_QueryResult" runat="server" Width="800px" BorderColor="#E7E7FF"
                        BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal"
                        AutoGenerateColumns="False" PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                        </HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FOrig_req" HeaderText="订单号" FooterStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="200px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FIncrease" HeaderText="数量">
                                <HeaderStyle Width="200px" HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FSvc_id" HeaderText="加分原因">
                                <HeaderStyle Width="200px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FCommit_time" HeaderText="加分时间">
                                <HeaderStyle Width="200px"></HeaderStyle>
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <label>
                        财付值总计:</label><asp:Label runat="server" ID="totalValue">0</asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
