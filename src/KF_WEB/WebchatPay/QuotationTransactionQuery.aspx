<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuotationTransactionQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.QuotationTransactionQuery" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Register Src="../Control/UserNameControl.ascx" TagName="UserNameControl" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
    <title>跨境汇款</title>
    <style type="text/css">
        @import url( ../STYLES/common.css );
        @import url( ../STYLES/ossstyle.css );
        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
        .auto-style1 {
            text-align: right;
            font-weight: bold;
            width: 304px;
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>

</head>
<body>
    <form id="formMain" runat="server">
        <div class="container">
            <div class="style_title">
                理财通报价交易查询
            </div>
            <table cellspacing="1" cellpadding="1">
                <caption>
                    查询条件
                </caption>
                <tr>
                    <td class="tb_query_title" style="width: 120px">账 号：
                    </td>
                    <td>
                        <uc1:UserNameControl ID="UserNameControl1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <%-- <td class="tb_query_title">基 金：
                    </td>
                    <td>
                      <asp:DropDownList ID="ddl_fund" runat="server" Width="173px">
                          <asp:ListItem Text="中信证券" Value="9000001"></asp:ListItem>
                      </asp:DropDownList>
                   </td>   
                       <td class="tb_query_title">状 态：
                    </td>
                    <td>
                      <asp:DropDownList  ID="ddl_state" runat="server" Width="173px">
                          <asp:ListItem Text="全部" Value=""></asp:ListItem>
                          <asp:ListItem Text="待执行" Value="1"></asp:ListItem>
                          <asp:ListItem Text="发起到期赎回" Value="2"></asp:ListItem>
                          <asp:ListItem Text="到期赎回成功" Value="3"></asp:ListItem>
                      </asp:DropDownList>
                    </td>--%>
                    <td class="tb_query_title">收益截止日：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_profit_end_date" runat="server" onclick="WdatePicker()" onFocus="WdatePicker({disabledDays:[0,6]})" class="Wdate" Width="173px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <asp:Button ID="btnSerach" runat="server" Text="查 询" OnClick="btnSerach_Click" />

                    </td>
                </tr>
            </table>
            <br />
            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" Caption="查询结果"
                CssClass="datagrid" GridLines="None" CellSpacing="1" CellPadding="1" OnItemCommand="DataGrid1_ItemCommand">
                <HeaderStyle CssClass="th" />
                <Columns>
                    <asp:BoundColumn DataField="Fid" HeaderText="" Visible="false" />
                    <%--  <asp:BoundColumn DataField="Fissue_name" HeaderText="报价编号名称" />--%>
                    <asp:BoundColumn DataField="Fund_name" HeaderText="基金名称" />
                    <asp:BoundColumn DataField="Ftrans_date" HeaderText="交易日" />
                    <asp:BoundColumn DataField="Fvalue_date" HeaderText="起息日" />
                    <asp:BoundColumn DataField="Fdue_date" HeaderText="到期日" />
                    <asp:BoundColumn DataField="Fprofit_recon_date" HeaderText="收益截止日" />
                    <asp:BoundColumn DataField="Ffetch_arrive_date" HeaderText="资金到期日" />
                    <asp:BoundColumn DataField="Fredem_type_str" HeaderText="赎回方式" />
                    <asp:BoundColumn DataField="Ftotal_fee" HeaderText="已确认金额" />
                    <asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间" />
                    <asp:BoundColumn DataField="Fstate_str" HeaderText="状态" />
                    <asp:ButtonColumn Text="查看" CommandName="detail" />
                </Columns>
            </asp:DataGrid>
            <webdiyer:AspNetPager ID="pager1" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="1100px"
                PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
                SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
            </webdiyer:AspNetPager>
            <asp:Panel runat="server" ID="panDetail" Visible="false">
                <table cellspacing="1" cellpadding="1">
                    <caption>
                        详情
                    </caption>
                    <tr>
                        <td class="auto-style1">账号：</td>
                        <td>
                            <asp:Label runat="server" ID="txt_QQID"></asp:Label>
                        </td>
                        <td class="tb_query_title">状态：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fstate_str"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="auto-style1">报价编号：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fissue"></asp:Label>
                        </td>
                        <td class="tb_query_title"><%--当前申购金额：--%>是否真实收益：</td>
                        <td><%--<asp:Label runat="server" ID="lbl_Fbuy_total"> </asp:Label>--%>
                            <asp:Label ID="lbl_Fprofit_type" runat="server"> </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">基金名称：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fund_name"></asp:Label>
                        </td>
                        <td class="tb_query_title">交易日：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Ftrans_date"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="auto-style1">赎回方式：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fredem_type_str"></asp:Label>
                        </td>
                        <td class="tb_query_title">起息日：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fvalue_date"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="auto-style1">到期年化收益率：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fprofit_rate_str"></asp:Label>
                        </td>
                        <td class="tb_query_title">期限：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fduration"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="auto-style1"><%--提前终止年化收益率：--%>已确认金额：</td>
                        <td><%--<asp:Label runat="server" ID="lbl_Fterminate_profit_rate"></asp:Label>--%>
                            <asp:Label ID="lbl_Ftotal_fee" runat="server"></asp:Label>
                        </td>
                        <td class="tb_query_title">到期日：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fdue_date"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="auto-style1">昨日收益：</td>
                        <td>
                            <asp:Label ID="lbl_Flast_profit" runat="server"> </asp:Label>
                        </td>
                        <td class="tb_query_title">收益截止日：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fprofit_recon_date"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="auto-style1"><%--昨日收益：--%>本期收益：</td>
                        <td><%--<asp:Label runat="server" ID="lbl_Flast_profit"></asp:Label>--%>
                            <asp:Label ID="lbl_Ftotal_profit" runat="server"> </asp:Label>
                        </td>
                        <td class="tb_query_title">资金到账日：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Ffetch_arrive_date"> </asp:Label></td>
                    </tr>
                    <tr>
                        <%-- <td class="auto-style1">是否真实收益：</td>
                     <td> <asp:Label runat="server" ID="lbl_Fprofit_type"></asp:Label> </td> --%>
                        <td class="tb_query_title">创建时间：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fcreate_time"> </asp:Label></td>
                        <td class="tb_query_title">修改时间：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Fmodify_time"> </asp:Label></td>
                    </tr>

                </table>
            </asp:Panel>
        </div>
        <br />
    </form>
</body>
</html>
