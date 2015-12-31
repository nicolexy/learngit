<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomsPushingQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.CustomsPushingQuery" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

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

</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table cellspacing="1" cellpadding="1" width="1100px"
            border="1">
            <tr>
                <td style="border: 0" bgcolor="#e4e5f7" colspan="2"><font face="宋体"><font color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20" />&nbsp;海关推单查询</font>
                </td>
                <td style="text-align: right; border: 0" bgcolor="#e4e5f7">操作员代码: <span class="style3">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Width="73px"></asp:Label></span>

                </td>
            </tr>
        </table>
        <br />
        <table cellspacing="1" cellpadding="1" width="1100px"
            border="1">
            <tr>
                <td style="text-align: right">商户号：</td>
                <td>
                    <asp:TextBox runat="server" ID="txt_partner"></asp:TextBox>
                </td>
                <td style="text-align: right">支付单号：</td>
                <td>
                    <asp:TextBox runat="server" ID="txt_transaction_id"></asp:TextBox></td>

            </tr>
            <tr>
                <td style="text-align: right">商户订单号：</td>
                <td>
                    <asp:TextBox runat="server" ID="txt_out_trade_no"></asp:TextBox></td>
                <td style="text-align: right">子商户订单号：</td>
                <td>
                    <asp:TextBox runat="server" ID="txt_sub_order_no"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align: right">子支付单号：</td>
                <td>
                    <asp:TextBox runat="server" ID="txt_sub_order_id"></asp:TextBox></td>
                <td style="text-align: right"></td>
                <td>
                    <asp:Button runat="server" ID="btnQuery" Text="查询" OnClick="btnQuery_Click" /></td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right">&nbsp;</td>
            </tr>
        </table>
        <br />
        <table runat="server" id="tbDetail" visible="false" border="1" cellspacing="0" cellpadding="0" width="1100px">
            <tr>
                <td bgcolor="#e4e5f7" colspan="5" class="auto-style1"><font color="red">
                    <img src="../images/page/post.gif" width="20" height="16">详情</font>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <div>
                        商户号：
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txt_partner1" runat="server"></asp:TextBox>
                </td>
                <td style="text-align: right">商户订单号：</td>
                <td>
                    <asp:TextBox ID="txt_out_trade_no1" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align: right">支付单号：</td>
                <td>
                    <asp:TextBox ID="txt_transaction_id1" runat="server"></asp:TextBox></td>
                <td style="text-align: right">申报单笔数：</td>
                <td>
                    <asp:TextBox ID="txt_count" runat="server"></asp:TextBox></td>
            </tr>

        </table>
        <br />
        <%--  "mch_customs_no,customs,state,modify_time,business_type,explanation"--%>
        <asp:GridView ID="DataGrid1" runat="server" Width="1100px" itemstyle-horizontalalign="center"
            HeaderStyle-HorizontalAlign="center" PageSize="5" AutoGenerateColumns="false"
            GridLines="horizontal" CellPadding="1" BackColor="white" BorderWidth="1px" BorderStyle="none" BorderColor="#e7e7ff">
            <FooterStyle ForeColor="#4a3c8c" BackColor="#b5c7de"></FooterStyle>
            <HeaderStyle Font-Bold="true" HorizontalAlign="center" ForeColor="#f7f7f7" BackColor="#4a3c8c"></HeaderStyle>
            <RowStyle HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="number" HeaderText="序号">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="modify_time_str" HeaderText="日期">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="sub_order_id" HeaderText="支付子订单号">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="sub_order_no" HeaderText="商户子订单号">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="customs_str" HeaderText="推送海关编码">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                  <asp:BoundField DataField="mch_customs_no" HeaderText="商户海关备案号">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                  <asp:BoundField DataField="state_str" HeaderText="推送状态">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="explanation" HeaderText="失败原因">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                 <asp:BoundField DataField="order_fee_str" HeaderText="子订单金额">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="fee_type" HeaderText="币种">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="product_fee_str" HeaderText="商品金额">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                 <asp:BoundField DataField="duty_str" HeaderText="关税">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
                 <asp:BoundField DataField="transport_fee_str" HeaderText="运费">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>

