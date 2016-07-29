<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>

<%@ Page Language="c#" CodeBehind="MobileRechargeQueryNew.aspx.cs" AutoEventWireup="True"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MobileRechargeQueryNew" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>TradeLogList</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
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
        TD
        {
            font-size: 9pt;
        }
        .style4
        {
            color: #ff0000;
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="Table1" style="z-index: 101; position: absolute; top: 5%; left: 5%" cellspacing="1"
        cellpadding="1" width="85%" border="1">
        <tr>
            <td bgcolor="#e4e5f7" colspan="5">
                <font face="宋体" color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;商户退款查询</font> </FONT>
            </td>
            <td align="right" bgcolor="#e4e5f7">
                <font face="宋体">操作员代码: <span class="style3">
                    <asp:Label ID="Label_uid" runat="server" Width="73px"></asp:Label></span></font>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label Style="z-index: 0" ID="Label4" runat="server">买家帐号</asp:Label>
            </td>
            <td>
                <asp:TextBox Style="z-index: 0" ID="txtUserQQ" runat="server"></asp:TextBox>
            </td>
            <td align="right">
                <asp:Label ID="Label2" runat="server">开始日期</asp:Label>
            </td>
            <td>
                <font face="宋体">
                    <asp:TextBox ID="txtBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:TextBox></font>
            </td>
            <td align="right">
                <asp:Label ID="Label3" runat="server">结束日期</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label Style="z-index: 0" ID="Label10" runat="server">支付订单号</asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox Style="z-index: 0" ID="txtOrderId" runat="server" Width="457px"></asp:TextBox>
            </td>
            <td colspan="2">
                <asp:Button ID="btnSearch" runat="server" Width="80px" Text="查 询" OnClick="Button2_Click">
                </asp:Button></td>
        </tr>
         <tr>
            <td colspan="6" align="center">
                <asp:DataGrid ID="dataGridMobileRecharge" runat="server" Width="100%" PageSize="20" BorderColor="#E7E7FF"
                    BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Horizontal"
                    AutoGenerateColumns="False">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="buyerUin" HeaderText="买家QQ"></asp:BoundColumn>

                         <asp:TemplateColumn HeaderText="面值">
									<ItemTemplate>
                                        <%# MoneyTransfer.FenToYuan(DataBinder.Eval(Container, "DataItem.faceValue").ToString()) %>
									</ItemTemplate>
								</asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="销卡金额">
									<ItemTemplate>
                                        <%# MoneyTransfer.FenToYuan(DataBinder.Eval(Container, "DataItem.payFee").ToString()) %>
									</ItemTemplate>
								</asp:TemplateColumn>

                        <asp:BoundColumn DataField="recoverTime" HeaderText="销卡时间"></asp:BoundColumn>
                        <asp:BoundColumn DataField="cardNo" HeaderText="充值卡卡号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="state" HeaderText="订单状态"></asp:BoundColumn>
                        <asp:BoundColumn DataField="dealId" HeaderText="业务订单ID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="msg" HeaderText="销卡结果描述"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr style="height:25px;">
            <td align="right"  colspan="6" >
                <webdiyer:AspNetPager ID="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" PageSize="20"
                    ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    HorizontalAlign="right" OnPageChanged="ChangePage" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
                </webdiyer:AspNetPager>
            </td>
        </tr>
        <tr>
            <td align="center">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
