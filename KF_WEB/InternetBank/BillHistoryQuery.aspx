<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillHistoryQuery.aspx.cs"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.InternetBank.BillHistoryQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>历史账单查询</title>
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
</head>
<body>
    <form id="form1" runat="server">
    <table style="z-index: 100; position: absolute; top: 5%; left: 5%" id="Table1" border="1"
        cellspacing="1" cellpadding="1" width="900px">
        <tr style="background-color: #e4e5f7">
            <td colspan="3">
                <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" /><label style="color: Red;
                    padding-left: 30px">历史账单查询(此处提供2012年1月到上月的账单查询)</label>
            </td>
        </tr>
        <tr>
            <td>
                <label style="padding-left: 10px;">
                    28位财付通订单号:</label>
            </td>
            <td style="text-align: center">
                <asp:TextBox ID="txtOrder" Width="400px" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: center">
                <asp:Button Text="查询" runat="server" ID="btn_OrderQuery" Width="100px" OnClick="btn_OrderQuery_Click">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label style="padding-left: 10px;">
                    收货QQ号码:</label>
                <asp:TextBox ID="txtQQ" Width="200px" runat="server"></asp:TextBox>
                <label style="padding-left: 100px;">
                    年月(YYYYMM):</label>
                <asp:TextBox ID="txtYearMonth" Width="200px" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: center">
                <asp:Button Text="查询" runat="server" ID="btn_QQQuery" Width="100px" OnClick="btn_QQQuery_Click">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="3">
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
                        <asp:BoundColumn DataField="Fserial_no" HeaderText="网银订单号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fpay_channel" HeaderText="银行"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fservice_code" HeaderText="产品"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fpay_amt" HeaderText="应付金额:元"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fbank_amt" HeaderText="扣费金额:元"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fpay_info" HeaderText="支付号码"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fuin" HeaderText="收货号码"></asp:BoundColumn>
                        <asp:BoundColumn DataField="state" HeaderText="流水状态"></asp:BoundColumn>
                        <asp:BoundColumn DataField="billState" HeaderText="对账状态"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fcreate_time" HeaderText="交易时间"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fcktime" HeaderText="对账时间"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fchg_time" HeaderText="补发时间"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fuser_ip" HeaderText="用户IP"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fcomment" HeaderText="备注"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
