<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCXGOrderQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCXGOrderQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>订单查询</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http：//schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        .style2 {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }

        .container {
            margin: 20px;
            width: 1200px;
        }

            .container table {
                width: 100%;
                background-color: #808080;
            }

            .container caption {
                text-align: left;
                background: #b0c3d1;
                padding: 4px;
            }

            .container td, .container th {
                background: #fff;
                height: 20px;
                padding: 2px 4px;
            }

            .container .tab_info th {
                font-weight: lighter;
                width: 120px;
                text-align: right;
            }

        .tab_dg tr:first-child td {
            background-color: #4A3C8C;
            font-weight: bold;
            line-height: 20px;
            color: white;
            text-align: center;
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div class="container">
            <table class="tab_input" cellpadding="1" cellspacing="1">
                <tr>
                    <td width="50%">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" />
                        <span class="style3">订单查询</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>商户编号：
                    <asp:TextBox ID="txt_spid" runat="server" /></td>
                    <td>商户订单号：
                    <asp:TextBox ID="txt_sp_billno" runat="server" /></td>
                </tr>
                <tr>
                    <td>银行卡号：  
                    <asp:TextBox ID="txt_card_no" runat="server" />
                    </td>
                    <td>查询日期：
                     <input type="text" runat="server" id="txt_cardQuery_date" onclick="WdatePicker()" />
                        <img onclick="txt_startDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" alt="选择日期" /></td>
                </tr>
                <tr>
                    <td>MD订单号：
                    <asp:TextBox ID="txt_mdlistid" runat="server" /></td>
                    <td>
                        <asp:Button ID="Button2" runat="server" Text="查询" OnClick="Button1_Click" />
                        <%--交易状态
                      <select>
                          <option>所有</option>
                          <option value="1">等待买家支付</option>
                          <option value="2">支付成功</option>
                          <option value="7">转入退款</option>
                          <option value="99">交易关闭</option>
                      </select>--%>
                    </td>
                </tr>

            </table>
            <br />
            <asp:DataGrid ID="dg_OrderInfo" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" OnSelectedIndexChanged="dg_OrderInfo_Select">
                <Columns>
                    <asp:BoundColumn HeaderText="商户编号" DataField="spid" />
                    <asp:BoundColumn HeaderText="商户订单号" DataField="coding" />
                    <asp:BoundColumn HeaderText="MD订单号" DataField="listid" />
                    <asp:BoundColumn HeaderText="交易时间" DataField="acc_time" />
                    <asp:BoundColumn HeaderText="交易金额" DataField="paynum_str" />
                    <asp:BoundColumn HeaderText="交易类型" DataField="trade_type_str" />
                    <asp:BoundColumn HeaderText="交易状态" DataField="trade_state_str" />
                    <asp:BoundColumn HeaderText="是否退款" DataField="IsRefund" />
                    <asp:BoundColumn HeaderText="是否拒付" DataField="IsRefuse" />
                    <asp:ButtonColumn HeaderText="详情" Text="查看" CommandName="Select" />
                </Columns>
            </asp:DataGrid>
            <br />
            <table class="tab_info" cellpadding="1" cellspacing="1" runat="server" visible="false" id="tab_order_info">
                <caption>
                    订单详情
                </caption>
                <tr>
                    <th>商户编号：</th>
                    <td>
                        <asp:Label ID="lb_spid" runat="server" />
                    </td>
                    <th>商户名称：</th>
                    <td>
                        <asp:Label ID="lb_sp_name" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>MD订单号：</th>
                    <td>
                        <asp:Label ID="lb_listid" runat="server" />
                    </td>
                    <th>商户订单号：</th>
                    <td>
                        <asp:Label ID="lb_coding" runat="server" />
                    </td>
                </tr>
                <tr>
                    <%-- <th>银行授权号：</th>
                    <td>
                        <asp:Label ID="lb_1" runat="server" />
                    </td>--%>
                    <th>银行订单号：</th>
                    <td>
                        <asp:Label ID="lb_rec_banklist" runat="server" />
                    </td>
                    <th>交易卡种：</th>
                    <td>
                        <asp:Label ID="lb_card_curtype_str" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>交易时间：</th>
                    <td>
                        <asp:Label ID="lb_acc_time" runat="server" />
                    </td>
                    <%--       <th>卡号：</th>
                    <td>
                        <asp:Label ID="lb_6" runat="server" />
                    </td>--%>
                    <th>交易金额：</th>
                    <td>
                        <asp:Label ID="lb_paynum_str" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>买家账号：</th>
                    <td>
                        <asp:Label ID="lb_buy_uid" runat="server" />
                    </td>
                    <th>卖家账号：</th>
                    <td>
                        <asp:Label ID="lb_sp_uid" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>交易状态：</th>
                    <td>
                        <asp:Label ID="lb_trade_state_str" runat="server" />
                    </td>
                    <th>退款时间：</th>
                    <td>
                        <asp:Label ID="lb_refund_time_acc" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>退款状态：</th>
                    <td>
                        <asp:Label ID="lb_refund_state_str" runat="server" />
                    </td>
                    <th>退款金额：</th>
                    <td>
                        <asp:Label ID="lb_refund_paynum_str" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>拒付状态：</th>
                    <td>
                        <asp:Label ID="lb_appeal_sign_str" runat="server" />
                    </td>
                    <th>拒付退款金额：</th>
                    <td>
                        <asp:Label ID="lb_14" runat="server" />
                    </td>
                </tr>
                <%--<tr>
                    <th>商户处理状态：</th>
                    <td></td>
                    <th></th>
                    <td></td>
                </tr>--%>
            </table>
            <table class="tab_info" cellpadding="1" cellspacing="1" runat="server" id="tab_CardInfo" visible="false">
                <caption>
                    持卡人信息
                </caption>
                <tr>
                    <th>持卡人姓名：</th>
                    <td></td>
                    <th>邮箱：</th>
                    <td></td>
                </tr>
                <tr>
                    <th>电话：</th>
                    <td></td>
                    <th>IP来源：</th>
                    <td></td>
                </tr>
                <tr>
                    <th>地址：</th>
                    <td></td>
                    <th>邮编 ：</th>
                    <td></td>
                </tr>
                <tr>
                    <th>交易金额：</th>
                    <td></td>
                    <th>交易卡种：</th>
                    <td></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
