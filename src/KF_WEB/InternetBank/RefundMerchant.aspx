<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundMerchant.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.InternetBank.RefundMerchant" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>退款商户录入</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }
        .tb_s{margin:10px 0;}
    </style>
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body ms_positioning="GridLayout">
    <form id="form1" method="post" runat="server">
        <div style="margin-left:5%;">
            <table class="tb_s" cellspacing="1" cellpadding="1" width="820" border="1">
                <tr>
                    <td style="WIDTH: 100%" bgcolor="#e4e5f7" colspan="6">
                        <div style="float:left;">
                            <img height="16" src="../IMAGES/Page/post.gif" width="20">
                            <span style="color:red;margin-left:20px;">退款商户录入</span>
                        </div>
                        <div style="float:right;">
                            <span>操作员代码: </span>
                            <asp:Label ID="lab_operator" runat="server" ForeColor="Red" Width="73px"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right">商户号码</td>
                    <td><asp:TextBox ID="tbx_query_refund" runat="server"></asp:TextBox></td>
                    <td align="right">开始日期</td>
                    <td><asp:TextBox ID="tbx_beginDate" onClick="WdatePicker({maxDate:'#F{$dp.$D(\'tbx_endDate\')}'})" Width="160px" CssClass="Wdate" runat="server"></asp:TextBox></td>
                    <td align="right">结束日期</td>
                    <td><asp:TextBox ID="tbx_endDate" onClick="WdatePicker({minDate:'#F{$dp.$D(\'tbx_beginDate\')}'})" Width="160px" CssClass="Wdate" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align:center;">
                        <asp:Button ID="btn_query" runat="server" Width="80px" Text="查 询" OnClick="btn_query_Click"></asp:Button>
                        <span style="margin:0 10px;"></span>
                        <asp:Button ID="btn_add" runat="server" Width="80px" Text="新 增" OnClick="btn_add_Click"></asp:Button>
                    </td>
                </tr>
            </table>
        
            <table class="tb_s"  id="Table2" cellspacing="1" cellpadding="1" border="1" width="850px" runat="server">
                <tr>
                    <td valign="top">
                        <asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                            BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" OnItemDataBound="DataGrid1_ItemDataBound" OnItemCommand="DataGrid1_ItemCommand">
                            <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                            <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                            <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                            <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                            <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="Fid" HeaderText="ID" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Frefund_id" HeaderText="商户号码"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Frefund_name" HeaderText="商户名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcreate_time" HeaderText="录入时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_time" HeaderText="修改时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcreate_by" HeaderText="创建人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_by" HeaderText="修改人"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbChange" runat="server" CommandName="CHANGE">修改</asp:LinkButton>
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
                            SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" OnPageChanged="ChangePage" PageSize="10">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>

            <table class="tb_s"  id="table_action" visible="false" cellspacing="1" cellpadding="1" width="820" border="1" runat="server">
            <tr>
                <td style="WIDTH: 100%" bgcolor="#e4e5f7" colspan="2">
                    <div style="float:left;">
                        <img height="16" src="../IMAGES/Page/post.gif" width="20">
                        <asp:Label ID="lab_action_title" runat="server" ForeColor="Red" Text="新增商户"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="right">商户号码</td>
                <td><asp:TextBox ID="tbx_refund_number" Width="300" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">商户名称</td>
                <td><asp:TextBox ID="tbx_refund_name" Width="300" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right;">
                    <asp:Button ID="btn_submit" runat="server" Width="80px" Text="提 交" OnClick="btn_submit_Click"></asp:Button>
                    <span style="margin:0 10px;"></span>
                    <asp:Button ID="btn_back" runat="server" Width="80px" Text="返 回" OnClick="btn_back_Click"></asp:Button>
                </td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>
