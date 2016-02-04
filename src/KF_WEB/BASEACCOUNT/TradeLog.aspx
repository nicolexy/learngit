<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>

<%@ Page Language="c#" CodeBehind="TradeLog.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.TradeLog" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>TradeLog</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
    </style>
    <script language="javascript" type="text/javascript">
        window.onload = function () {
            var curIfm = window.parent.document.getElementById('iframe0');
            curIfm.style.height = document.body.scrollHeight + 20;
        }
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" DataSource="<%# DS_TradeLog %>">
        <HeaderStyle BackColor="#EEEEEE"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="交易单ID号">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Flistid") %>'
                        NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Flistid", "../TradeManage/OrderDetail.aspx?listid=") + DataBinder.Eval(Container, "DataItem.Flistid") %>'
                        Target="_parent">

                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="类型">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# setConfig.convertPayType(DataBinder.Eval(Container, "DataItem.Ftrade_type").ToString()) %>'
                        NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Flistid", "../TradeManage/TradeLogQuery.aspx?id=") + DataBinder.Eval(Container, "DataItem.Flistid") %>'
                        Target="_parent" ID="Hyperlink1">

                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn Visible="False" DataField="Fcoding" HeaderText="订单编码">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbank_listid" HeaderText="给银行的订单号">
                <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="Fbank_backid" HeaderText="银行返回的交易（定单）号">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="交易单状态">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.convertTradeState(DataBinder.Eval(Container, "DataItem.Flstate").ToString()) %>'
                        ID="Label1">

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="交易的状态">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fstate").ToString()=="100"?"":setConfig.convertTradeListState(DataBinder.Eval(Container, "DataItem.Fstate").ToString()) %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn Visible="False" DataField="Fpay_type" HeaderText="支付类型">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbuy_uid" HeaderText="买家内部帐号">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbuyid" HeaderText="买家帐号">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbuy_name" HeaderText="付款方名称">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="买家开户行类型">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.convertbankType(DataBinder.Eval(Container, "DataItem.Fbuy_bank_type").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn Visible="False" DataField="Fbuy_bankid" HeaderText="买家的银行帐号">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="Fsale_uid" HeaderText="卖家内部帐号">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fsaleid" HeaderText="卖家帐号">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fsale_name" HeaderText="卖家名称">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="币种代码">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.convertMoney_type(DataBinder.Eval(Container, "DataItem.Fcurtype").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="产品的价格">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fprice").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="物流费用">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fcarriage").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="应支付的总价格">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaynum").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="实际支付费用">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Ffact").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="交易手续费">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fprocedure").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Fservice" HeaderText="服务费率">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="退买家金额">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaybuy").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="退卖家金额">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaysale").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="现金支付金额">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fcash").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Fspid" HeaderText="发起机构代码">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间（本地）">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fcreate_time_c2c" HeaderText="订单创建时间（C2C）">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fcreate_time" HeaderText="订单创建时间（本地时间）">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fip" HeaderText="最后修改交易单的IP">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="交易说明">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#  DataBinder.Eval(Container, "DataItem.Fmemo").ToString() %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Fexplain" HeaderText="备注/后台人员操作记录">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbargain_time" HeaderText="买家付款时间（bank）">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fpay_time" HeaderText="买家付款时间（本地时间）">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Freceive_time_c2c" HeaderText="可以打款给卖家时间（C2C）">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Freceive_time" HeaderText="打款给卖家时间（本地）">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fsale_bank_type" HeaderText="卖家开户银行类型">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fsale_bankid" HeaderText="卖家银行帐号">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
        </Columns>
    </asp:DataGrid>
    <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonCount="3" PagingButtonSpacing="0"
        ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
        SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" PageSize="8" AlwaysShow="True" />
    </form>
</body>
</html>
