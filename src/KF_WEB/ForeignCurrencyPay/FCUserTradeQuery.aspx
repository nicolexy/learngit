<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCUserTradeQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCUserTradeQuery" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title></title>
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

        /*#tab_info {
            margin: 5px 10px;
        }

            #tab_info th {
                background-color: white;
                width: 100px;
            }

            #tab_info td {
                padding: 5px 20px;
            }*/
    </style>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table id="table1" border="1" cellspacing="1" cellpadding="0" runat="server">
            <tr>
                <td width="50%">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" /><label class="style3">外币用户交易查询</label></td>
                <td>
                    <label class="style3">操作员ID：</label><asp:Label ID="lb_operatorID" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <span>用户WeChat Id:</span>
                    <asp:TextBox ID="WeChatId" Width="250px" runat="server"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="Button1" runat="server" Text="查询" OnClick="Button1_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="wrap_info" runat="server" visible="false">
                        <div style="padding: 10px;">
                            <div id="tab_btn" runat="server">
                                <asp:LinkButton ID="btn_tradeBill" runat="server" OnClick="btn_Click">交易单</asp:LinkButton>
                                | 
                        <asp:LinkButton ID="btn_refund" runat="server" OnClick="btn_Click">退款单</asp:LinkButton>
                                | 
                        <asp:LinkButton ID="a_fundStream" runat="server" OnClick="a_fundStream_Click">资金流水</asp:LinkButton>
                                | 
                        <asp:LinkButton ID="btn_bindCardRecord" runat="server" OnClick="btn_Click">绑卡记录</asp:LinkButton>
                                <span style="margin-left: 200px;">
                                    <asp:LinkButton ID="btn_createCVS" runat="server" OnClick="btn_createCVS_Click">生成CSV</asp:LinkButton>
                                </span>
                            </div>
                            <hr />
                            <div id="fundStreamTermInfo" runat="server" visible="false">
                                <select runat="server" id="FAccType">
                                    <option value="1">普通账号</option>
                                    <option value="2">微信红包账号</option>
                                </select>
                                <select runat="server" id="FCurType">
                                    <option value="344">HKD</option>
                                    <option value="156">CUY</option>
                                    <option value="392">JPY</option>
                                    <option value="840">USD</option>
                                </select>
                                <asp:LinkButton ID="btn_fundStream" runat="server" OnClick="btn_Click">查找</asp:LinkButton>
                            </div>
                        </div>
                        <div>

                            <asp:DataGrid ID="dg_tradeBill" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
                                CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
                                <AlternatingItemStyle BackColor="#F7F7F7" />
                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Height="30px" />
                                <ItemStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" Mode="NumericPages" />
                                <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <Columns>
                                    <asp:BoundColumn DataField="acc_time" HeaderText="交易时间" />
                                    <asp:BoundColumn DataField="spid" HeaderText="商户编号" />
                                    <asp:BoundColumn DataField="coding" HeaderText="商户订单号" />
                                    <asp:BoundColumn DataField="listid" HeaderText="MD订单号 / 财付通订单号" />
                                    <asp:BoundColumn DataField="sp_name" HeaderText="商户名称" />
                                    <asp:BoundColumn DataField="price_str" HeaderText="交易金额" />
                                    <asp:BoundColumn DataField="trade_type_str" HeaderText="交易类型" />
                                    <asp:BoundColumn DataField="list_state_str" HeaderText="交易单状态" />
                                    <asp:BoundColumn DataField="trade_state_str" HeaderText="交易状态" />
                                    <asp:BoundColumn DataField="buy_uid" HeaderText="用户内部账号" />
                                    <asp:BoundColumn DataField="sub_business_type" HeaderText="卡类型" />
                                    <%--<asp:BoundColumn DataField="" HeaderText="发卡行"/>--%>
                                    <asp:BoundColumn DataField="price_curtype_str" HeaderText="币种" />
                                    <asp:BoundColumn DataField="bank_paynum_str" HeaderText="应支付金额" />
                                    <asp:BoundColumn DataField="bank_paynum_str" HeaderText="实际支付金额" />
                                    <%--<asp:BoundColumn DataField="" HeaderText="交易手续费"/>--%>
                                </Columns>
                            </asp:DataGrid>

                            <asp:DataGrid ID="dg_refundBill" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
                                CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
                                <AlternatingItemStyle BackColor="#F7F7F7" />
                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Height="30px" />
                                <ItemStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" Mode="NumericPages" />
                                <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <Columns>
                                    <asp:BoundColumn DataField="acc_time" HeaderText="交易时间" />
                                    <asp:BoundColumn DataField="sp_billno" HeaderText="商户订单号" />
                                    <asp:BoundColumn DataField="coding" HeaderText="MD订单号" />
                                    <asp:BoundColumn DataField="draw_id" HeaderText="退款订单号" />
                                    <asp:BoundColumn DataField="sp_refund_time" HeaderText="退款时间" />
                                    <asp:BoundColumn DataField="cur_type_str" HeaderText="交易币种" />
                                    <asp:BoundColumn DataField="sp_refund_num_str" HeaderText="退款金额" />
                                    <asp:BoundColumn DataField="sp_refund_cash_str" HeaderText="实际退款金额" />
                                    <asp:BoundColumn DataField="refund_state_str" HeaderText="退款状态" />
                                </Columns>
                            </asp:DataGrid>

                            <asp:DataGrid ID="dg_bindCardRecord" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
                                CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
                                <AlternatingItemStyle BackColor="#F7F7F7" />
                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Height="30px" />
                                <ItemStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" Mode="NumericPages" />
                                <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <Columns>
                                    <asp:BoundColumn DataField="bind_time" HeaderText="绑卡时间" />
                                    <asp:BoundColumn DataField="card_tail" HeaderText="卡号" />
                                    <%--<asp:BoundColumn DataField="id" HeaderText="发行卡" />--%>
                                    <asp:BoundColumn DataField="card_type_str" HeaderText="卡标" />
                                    <asp:BoundColumn DataField="card_owner_name" HeaderText="持卡人姓名" />
                                    <asp:BoundColumn DataField="mobile" HeaderText="电话" />
                                    <asp:BoundColumn DataField="bill_email" HeaderText="邮箱" />
                                    <asp:BoundColumn DataField="bill_area" HeaderText="国家/地区" />
                                    <asp:BoundColumn DataField="bill_address" HeaderText="城市" />
                                    <asp:BoundColumn DataField="bill_address" HeaderText="街道" />
                                    <%--<asp:BoundColumn DataField="zip_code" HeaderText="邮编" />--%>
                                    <%--<asp:BoundColumn DataField="" HeaderText="绑定IP来源" />--%>
                                    <%--<asp:BoundColumn DataField="spid" HeaderText="商户号" />--%>
                                </Columns>
                            </asp:DataGrid>


                            <asp:DataGrid ID="dg_BankRollFlow" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
                                CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
                                <AlternatingItemStyle BackColor="#F7F7F7" />
                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Height="30px" />
                                <ItemStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" Mode="NumericPages" />
                                <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <Columns>
                                    <asp:BoundColumn DataField="listid" HeaderText="MD订单号" />
                                    <%--<asp:BoundColumn DataField="" HeaderText="类别/科目" />--%>
                                    <asp:BoundColumn DataField="uid" HeaderText="用户内部账号" />
                                    <asp:BoundColumn DataField="curtype_str" HeaderText="币种" />
                                    <asp:BoundColumn DataField="paynum_str" HeaderText="金额" />
                                    <asp:BoundColumn DataField="type_str" HeaderText="资金流向" />
                                    <%--<asp:BoundColumn DataField="uid" HeaderText="用户内部账号" />--%>
                                    <asp:BoundColumn DataField="balance_str" HeaderText="账户余额" />
                                    <%--<asp:BoundColumn DataField="" HeaderText="对方内部账号" />--%>
                                </Columns>
                            </asp:DataGrid>
                            <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" OnPageChanged="pager_PageChanged"></webdiyer:AspNetPager>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
