<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryWeiXinMircoPay.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryWeiXinMircoPay" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>境外微信小额支付查询</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );

        body, div, ul {
            margin: 0;
            padding: 0;
        }

        .style3 {
            color: #ff0000;
        }

        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }

        .tdfull {
            _height: expression(this.offsetParent.offsetHeight+"px");
        }

        .search_ul li {
            list-style: none;
            float: left;
            margin: 4px 20px;
        }

        .clear {
            clear: both;
        }

        #dgList td {
            text-align: center;
        }

        .info_tab {
            border-collapse: collapse;
            border: none;
        }

            .info_tab tbody td {
                padding: 2px 20px;
                border-bottom: 1px solid #000;
            }

                .info_tab tbody td:first-child {
                    text-align: right;
                }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table cellspacing="1" cellpadding="0" align="center" bgcolor="#666666" border="0" width="95%">
            <tr bgcolor="#e4e5f7" style="background-image: url(/IMAGES/Page/bg_bl.gif)">
                <td valign="middle" colspan="2">
                    <table style="height: 90%; width: 100%" cellspacing="0" cellpadding="1" border="0" class="tdfull">
                        <tr>
                            <td width="80%" style="background-image: url(/IMAGES/Page/bg_bl.gif)" height="18">
                                <font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16" src="../IMAGES/Page/post.gif" width="20" alt="" />境外微信小额支付查询</font>
                            </td>
                            <td width="20%" style="background-image: url(/IMAGES/Page/bg_bl.gif)" nowrap="nowrap">操作员代码: 
                                <span class="style3">
                                    <asp:Label ID="Label_uid" runat="server" Width="200px"></asp:Label>
                                </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr bgcolor="#ffffff">
                <td colspan="2">
                    <div>
                        <div>
                            <ul class="search_ul">
                                <li><span>商户号：</span><asp:TextBox ID="txt_spid" runat="server"></asp:TextBox></li>
                                <li><span>商户订单号：</span><asp:TextBox ID="txt_out_trade_no" runat="server"></asp:TextBox></li>
                                <li><span>微信订单号：</span><asp:TextBox ID="txt_listid" runat="server"></asp:TextBox></li>
                                <li><span>财付通订单号：</span><asp:TextBox ID="txt_cftlistid" runat="server"></asp:TextBox></li>
                            </ul>
                        </div>
                        <div class="clear"></div>
                        <div>
                            <ul class="search_ul">
                                <li><span>买家姓名：</span><asp:TextBox ID="txt_name" runat="server"></asp:TextBox></li>
                                <li><span>买家手机号：</span><asp:TextBox ID="txt_mobile" runat="server"></asp:TextBox></li>
                            </ul>
                        </div>
                        <div style="float: right; margin-right: 50px;">
                            <asp:Button ID="Button1" runat="server" Text="查询" Width="100" Height="25" OnClick="Button1_Click" />
                        </div>
                        <div class="clear"></div>
                        <div>
                            <ul class="search_ul">
                                <li><span>商户号：</span>
                                    <span>日期从</span>
                                    <asp:TextBox ID="txt_fromtime" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:TextBox>
                                    <span style="margin: auto 10px">到</span>
                                    <asp:TextBox ID="txt_totime" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:TextBox>
                                </li>
                                <li><span>交易状态：</span>
                                    <asp:DropDownList ID="DropDownListTrade_State" runat="server"></asp:DropDownList></li>
                            </ul>
                        </div>
                        <div class="clear"></div>
                    </div>
                </td>
            </tr>
            <tr bgcolor="#ffffff">
                <td colspan="2">
                    <div>
                        <asp:DataGrid ID="dgList" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
                            CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
                            <AlternatingItemStyle BackColor="#F7F7F7" />
                            <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                            <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Height="30px" />
                            <ItemStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" Mode="NumericPages" />
                            <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                            <Columns>
                                <asp:ButtonColumn CommandName="Select" DataTextField="listid" HeaderText="微信订单号"></asp:ButtonColumn>
                                <asp:BoundColumn HeaderText="财付通订单号" DataField="cftlistid"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="商户号" DataField="spid"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="商户订单号" DataField="out_trade_no"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="创建日期" DataField="create_time"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="买家账号" DataField="buyid"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="买家姓名" DataField="name"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="手机号码" DataField="mobile"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="外币类型" DataField="currency_type"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="交易金额(外币)" DataField="total_fee_fc"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="交易金额(人民币)" DataField="total_fee_rmb"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="交易状态" DataField="trade_state"></asp:BoundColumn>
                                <%--<asp:BoundColumn HeaderText="交易说明" DataField="detail"></asp:BoundColumn>--%>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                    <div>
                        <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:AspNetPager>
                    </div>
                </td>
            </tr>
        </table>
    </form>
    <div id="dialog" style="position: absolute; top: 0; background-image: url(../IMAGES/Page/bg01.gif); width: 100%;" runat="server">
        <div style="margin: 20px auto; width: 95%;">
            <table style="width: 100%" class="info_tab">
                <thead>
                    <tr>
                        <th colspan="4">外币附加信息</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="width: 30%">外币类型：</td>
                        <td style="width: 400px;"><span id="currency_type" runat="server"></span></td>
                        <td style="width: 120px;">交易汇率：</td>
                        <td><span id="trans_rate" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td>交易汇率时间：</td>
                        <td><span id="rate_time" runat="server"></span></td>
                        <td>传入的币种类型：</td>
                        <td><span id="input_fc_rmb" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td>交易金额(外币)：</td>
                        <td><span id="total_fee_fc" runat="server"></span></td>
                        <td>交易金额(人民币)：</td>
                        <td><span id="total_fee_rmb" runat="server"></span></td>
                    </tr>
                    <%-- <tr>
                        <td>支付接入渠道：</td>
                        <td><span id="Span7" runat="server"></span></td>
                        <td>身份信息来源渠道：</td>
                        <td><span id="Span8" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td>身份信息核对标识：</td>
                        <td><span id="Span9" runat="server"></span></td>
                        <td>客户额度是否已累加：</td>
                        <td><span id="Span10" runat="server"></span></td>
                    </tr>--%>
                    <tr>
                        <td>购汇姓名：</td>
                        <td><span id="name" runat="server"></span></td>
                        <td>购汇身份证信息：</td>
                        <td><span id="cre_num" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td>手机号码：</td>
                        <td><span id="mobile" runat="server"></span></td>
                        <td>买家账户号码(QQ号码)：</td>
                        <td><span id="buyid" runat="server"></span></td>
                    </tr>
    <%--                <tr>
                        <td>下属商户名称：</td>
                        <td><span id="Span15" runat="server"></span></td>
                        <td>下属商户编码：</td>
                        <td><span id="Span16" runat="server"></span></td>
                    </tr>--%>
                </tbody>
            </table>
            <iframe id="iframe_tradelog" runat="server" width="100%" height="620px" scrolling="no"></iframe>
            <span runat="server" style="color:red" id="ErrorReason"></span>
            <div style="text-align: center; margin-top: 20px;">
                <input type="button" value="关 闭" onclick="dialog.style.visibility = 'hidden'" style="width: 50px; height: 25px; cursor: pointer;" />
            </div>
        </div>
    </div>
</body>
</html>
