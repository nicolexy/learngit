<%@ Page Language="c#" CodeBehind="BankRefereNoQuery.aspx.cs" AutoEventWireup="True"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BankRefereNoQuery" %>

<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>银行参考号查询</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );

        .style2 {
            color: #000000;
        }

        .style3 {
            color: #ff0000;
        }

        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
    </style>
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div style="width: 820px; margin: 10px 60px;">
            <table id="Table1" style="width: 100%" cellspacing="1" cellpadding="1" border="1">
                <tr>
                    <td bgcolor="#e4e5f7" colspan="4">
                        <font face="宋体" color="red">
                            <img height="16" src="../IMAGES/Page/post.gif" width="20">
                            &nbsp;&nbsp;银行参考号查询</font>
                    </td>
                    <td align="right" bgcolor="#e4e5f7">
                        <font face="宋体">操作员代码: <span class="style3">
                            <asp:Label ID="Label1" runat="server" Width="73px"></asp:Label></span></font>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        银行参考号：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_BankRefereNo" runat="server" Width="250px"></asp:TextBox>
                    </td>
                    <td colspan="3"><span>银行名称：</span>
                        <asp:DropDownList ID="DropOldBankType" runat="server" Width="200px" BorderWidth="2px" BorderStyle="Ridge"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">日期： 
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxDate" runat="server" Width="160px" onClick="WdatePicker()" CssClass="Wdate"></asp:TextBox>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="Label10" runat="server">选择批量查询文件：</asp:Label>&nbsp;<asp:FileUpload ID="File1" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td align="left" colspan="2"></td>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnQuery" runat="server" Width="100px" Text="批量查询" OnClick="btnBatchQuery"></asp:Button>&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" Width="80px" Text="查 询" OnClick="btnSearch_Click"></asp:Button>
                    </td>
                </tr>
            </table>

            <div id="showbox" runat="server">
                <br />
                <div>
                    <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False"
                        GridLines="Horizontal" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
                        BorderColor="#E7E7FF">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <Columns>
                            <%--<asp:BoundColumn DataField="fpay_acc" HeaderText="银行卡号"></asp:BoundColumn>--%>
                            <asp:BoundColumn DataField="fbank_order" HeaderText="银行订单号"></asp:BoundColumn>
                            <%--<asp:BoundColumn DataField="FamtStr" HeaderText="金额"></asp:BoundColumn>--%>
                            <asp:BoundColumn DataField="Fbiz_type_str" HeaderText="支付状态"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </div>
                <div>
                    <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonTextFormatString="[{0}]"
                        SubmitButtonText="转到" OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager"
                        ShowInputBox="always" PagingButtonSpacing="0" ShowCustomInfoSection="left" NumericButtonCount="5"
                        AlwaysShow="True">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
