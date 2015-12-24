<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.InternetBank.BillQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>当月账单查询</title>
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
    <script src="../SCRIPTS/Local.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <table style="z-index: 100; position: absolute; top: 3%; left: 5%" id="Table1" border="1"
        cellspacing="1" cellpadding="1" width="1170px">
        <tr style="background-color: #e4e5f7">
            <td colspan="4">
                <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" /><label style="color: Red;
                    padding-left: 30px">当月账单查询</label>
            </td>
        </tr>
        <tr>
            <td>
                <label style="padding-left: 10px;">
                    账号:</label>
                <asp:TextBox ID="txtQQ" Width="200px" runat="server"></asp:TextBox>
            </td>
            <td>
                <label style="padding-left: 10px">
                    订单号:</label>
                <asp:TextBox ID="txtOrder" Width="200px" runat="server"></asp:TextBox>
            </td>
            <td>
                <label style="padding-left: 10px">
                    付款帐号:</label>
                <asp:TextBox ID="txtPayAccount" Width="200px" runat="server"></asp:TextBox>
            </td>
            <td>
                <label style="padding-left: 10px">
                    银行流水号:</label>
                <asp:TextBox ID="txtBankTrunoverID" Width="200px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <label>
                    开始日期:</label>
                	<input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker()" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
            </td>
            <td>
                <label>
                    结束日期:</label>
                <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker()" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
            </td>
            <td colspan="2" style="text-align: center">
                <asp:Button Text="查询" runat="server" ID="btn_query" Width="100px" 
                    onclick="btn_query_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:DataGrid ID="DataGrid_QueryResult" runat="server" Width="1170px" BorderColor="#E7E7FF"
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
                        <asp:BoundColumn DataField="Fpay_channel" HeaderText="银行"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fservice_code" HeaderText="业务"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fuin" HeaderText="QQ号码"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fpay_amt" HeaderText="应付金额:元"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fbank_amt" HeaderText="扣费金额:元"></asp:BoundColumn>
                        <asp:BoundColumn DataField="state" HeaderText="流水状态"></asp:BoundColumn>
                        <asp:BoundColumn DataField="billState" HeaderText="账单状态"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fserial_no" HeaderText="订单号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fbank_water" HeaderText="银行流水号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fcreate_time" HeaderText="交易时间"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fchg_time" HeaderText="补发时间"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fuser_ip" HeaderText="用户IP"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fportal_water" HeaderText="portal流水"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fpay_info" HeaderText="扣费号码"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fcomment" HeaderText="备注"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
