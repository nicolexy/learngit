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
            <asp:TemplateColumn HeaderText="���׵�ID��">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Flistid") %>'
                        NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Flistid", "../TradeManage/OrderDetail.aspx?listid=") + DataBinder.Eval(Container, "DataItem.Flistid") %>'
                        Target="_parent">

                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="����">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# setConfig.convertPayType(DataBinder.Eval(Container, "DataItem.Ftrade_type").ToString()) %>'
                        NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Flistid", "../TradeManage/TradeLogQuery.aspx?id=") + DataBinder.Eval(Container, "DataItem.Flistid") %>'
                        Target="_parent" ID="Hyperlink1">

                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn Visible="False" DataField="Fcoding" HeaderText="��������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbank_listid" HeaderText="�����еĶ�����">
                <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="Fbank_backid" HeaderText="���з��صĽ��ף���������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="���׵�״̬">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.convertTradeState(DataBinder.Eval(Container, "DataItem.Flstate").ToString()) %>'
                        ID="Label1">

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="���׵�״̬">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fstate").ToString()=="100"?"":setConfig.convertTradeListState(DataBinder.Eval(Container, "DataItem.Fstate").ToString()) %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn Visible="False" DataField="Fpay_type" HeaderText="֧������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbuy_uid" HeaderText="����ڲ��ʺ�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbuyid" HeaderText="����ʺ�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbuy_name" HeaderText="�������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="��ҿ���������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.convertbankType(DataBinder.Eval(Container, "DataItem.Fbuy_bank_type").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn Visible="False" DataField="Fbuy_bankid" HeaderText="��ҵ������ʺ�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="Fsale_uid" HeaderText="�����ڲ��ʺ�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fsaleid" HeaderText="�����ʺ�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fsale_name" HeaderText="��������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="���ִ���">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.convertMoney_type(DataBinder.Eval(Container, "DataItem.Fcurtype").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="��Ʒ�ļ۸�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fprice").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="��������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fcarriage").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Ӧ֧�����ܼ۸�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaynum").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="ʵ��֧������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Ffact").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="����������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fprocedure").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Fservice" HeaderText="�������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="����ҽ��">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaybuy").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="�����ҽ��">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaysale").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="�ֽ�֧�����">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fcash").ToString()) %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Fspid" HeaderText="�����������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fmodify_time" HeaderText="����޸�ʱ�䣨���أ�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fcreate_time_c2c" HeaderText="��������ʱ�䣨C2C��">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fcreate_time" HeaderText="��������ʱ�䣨����ʱ�䣩">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fip" HeaderText="����޸Ľ��׵���IP">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="����˵��">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#  DataBinder.Eval(Container, "DataItem.Fmemo").ToString() %>'>

                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Fexplain" HeaderText="��ע/��̨��Ա������¼">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fbargain_time" HeaderText="��Ҹ���ʱ�䣨bank��">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fpay_time" HeaderText="��Ҹ���ʱ�䣨����ʱ�䣩">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Freceive_time_c2c" HeaderText="���Դ�������ʱ�䣨C2C��">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Freceive_time" HeaderText="��������ʱ�䣨���أ�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fsale_bank_type" HeaderText="���ҿ�����������">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Fsale_bankid" HeaderText="���������ʺ�">
                <HeaderStyle Wrap="False"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:BoundColumn>
        </Columns>
    </asp:DataGrid>
    <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonCount="3" PagingButtonSpacing="0"
        ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
        SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]" PageSize="8" AlwaysShow="True" />
    </form>
</body>
</html>
