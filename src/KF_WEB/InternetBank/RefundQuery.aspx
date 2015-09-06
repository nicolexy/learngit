<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page Language="c#" CodeBehind="RefundQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.InternetBank.RefundQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ComplainBussinessInput</title>
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
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellspacing="1" cellpadding="1" width="1020" border="1">
            <tr>
                <td style="WIDTH: 100%" bgcolor="#e4e5f7" colspan="8"><font face="宋体"><font color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;退款登记</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </font>操作员代码: </FONT><span class="style3"><asp:Label ID="Label1" runat="server" ForeColor="Red" Width="73px"></asp:Label></span></td>
            </tr>
            <tr>
                <td align="right"><asp:Label ID="Label2" runat="server">订单编码：</asp:Label></td>
                <td><asp:TextBox ID="listId" runat="server"></asp:TextBox></td>
                <td align="right"><asp:Label ID="Label3" runat="server">财付通订单号：</asp:Label></td>
                <td><asp:TextBox ID="cftOrderId" Width="230px" runat="server"></asp:TextBox></td>
                <td align="right">商户号：</td>
                <td><asp:DropDownList ID="ddl_refund_id" runat="server"></asp:DropDownList></td>
                <td align="right">登记人：</td>
                <td><asp:TextBox ID="tbx_submit_user" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label4" runat="server">开始日期：</asp:Label></td>
                <td><asp:TextBox ID="TextBoxBeginDate" onClick="WdatePicker({maxDate:'#F{$dp.$D(\'TextBoxEndDate\')}'})" Width="160px" CssClass="Wdate" runat="server"></asp:TextBox></td>
                <td align="right"><asp:Label ID="Label5" runat="server">结束日期：</asp:Label></td>
                <td colspan="5"><asp:TextBox ID="TextBoxEndDate" onClick="WdatePicker({minDate:'#F{$dp.$D(\'TextBoxBeginDate\')}'})" Width="160px" CssClass="Wdate" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label6" runat="server">退款类型：</asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlRefundType" runat="server" Width="152px">
                        <asp:ListItem Value="0" Selected="True">全部</asp:ListItem>
                        <asp:ListItem Value="10">投诉退款</asp:ListItem>
                        <asp:ListItem Value="11">发货失败</asp:ListItem>

                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Label ID="Label7" runat="server">提交退款：</asp:Label></td>
                <td colspan="5">
                    <asp:DropDownList ID="ddlRefundStatus" runat="server" Width="152px">
                        <asp:ListItem Value="0" Selected="True">全部</asp:ListItem>
                        <asp:ListItem Value="1">已提交</asp:ListItem>
                        <asp:ListItem Value="2">未提交</asp:ListItem>
                        <asp:ListItem Value="3">失效</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td align="right">交易状态：</td>
                <td>
                    <asp:DropDownList ID="ddlTradeState" runat="server"></asp:DropDownList></td>
                <td align="right">&nbsp;</td>
                <td colspan="5">
                    <asp:Button ID="Button1" runat="server" Width="80px" Text="查 询" OnClick="Button1_Click"></asp:Button>&nbsp;
                   <asp:Button ID="btnNew" runat="server" Width="80px" Text="新 增" OnClick="btnNew_Click"></asp:Button>&nbsp;
                   <asp:Button ID="Button2" runat="server" Width="80px" Text="导出excel" OnClick="Export_Click"></asp:Button>&nbsp;
                   <asp:Button ID="btnUpload" runat="server" Width="80px" Text="导入excel" OnClick="btnUpload_Click"></asp:Button>
                    <asp:Button ID="btnSubRefund" runat="server" Visible="true"  Width="80px" Text="提交账务退款" OnClick="btnRefundEmail_Click" OnClientClick="return confirm('确定要将退款数据发送邮件给财务吗？')"></asp:Button>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label10" runat="server">文件：</asp:Label>
                </td>
                <td align="left" colspan="7">
                    <asp:FileUpload ID="File1" runat="server" Width="355px" />
                    &nbsp;&nbsp;
                    <a href="/Template/Excel/RefundTemplate.xls" target="_blank">下载模版</a>
                    <%--<asp:HyperLink ID="DownloadTemplate" runat="server" NavigateUrl="/uploadfile/20150416/CSOMS/Template/RefundTemplate.xls">下载模版</asp:HyperLink>--%>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="8">
                    <asp:Label ID="Label8" runat="server">统计数据：</asp:Label>&nbsp;&nbsp;&nbsp;
                       <asp:Label ID="Label9" runat="server">0</asp:Label>
                </td>
            </tr>
        </table>
        <table id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 224px; HEIGHT: 50%"
            cellspacing="1" cellpadding="1" width="808" border="1" runat="server">
            <tr>
                <td valign="top">
                    <asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                        BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" OnItemDataBound="DataGrid1_ItemDataBound">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="Fid" HeaderText="ID" Visible="false"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Forder_id" HeaderText="财付通订单号"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fcoding" HeaderText="订单编码"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Ftrade_state_str" HeaderText="交易状态"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Famount_str" HeaderText="交易金额"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Frefund_amountStr" HeaderText="退款金额"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fbuy_acc" HeaderText="买家账号"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Ftrade_desc" HeaderText="交易说明"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fsubmit_user" HeaderText="登记人"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Frecycle_user" HeaderText="物品回收人"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Frefund_type_str" HeaderText="退款类型"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fsam_no" HeaderText="SAM工单号"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fsubmit_refund_str" HeaderText="提交退款"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbChange" runat="server" CommandName="CHANGE">编辑</asp:LinkButton>
                                    <asp:LinkButton ID="lbDel" runat="server" CommandName="DEL">删除</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
            <tr height="25">
                <td>
                    <webdiyer:AspNetPager ID="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
                        PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
                        SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
                    </webdiyer:AspNetPager>
                </td>
            </tr>
        </table>
        <table id="Table3" visible="false" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 224px; HEIGHT: 50%" width="808" border="1" runat="server">
            <tr>
                <td>总数：<asp:Label ID="lbTotal" runat="server">0</asp:Label>&nbsp;&nbsp;成功数：<asp:Label ID="lbSucc" runat="server">0</asp:Label>&nbsp;&nbsp;失败数：<asp:Label ID="lbFail" runat="server">0</asp:Label></td>
            </tr>
            <tr>
                <td>错误信息：<asp:Label ID="lbError" runat="server"></asp:Label></td>
            </tr>
        </table>
    </form>
</body>
</html>
