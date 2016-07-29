<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>

<%@ Page Language="c#" CodeBehind="B2CReturnQuery.aspx.cs" AutoEventWireup="True"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.B2CReturnQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>TradeLogList</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
        .style2
        {
            color: #000000;
        }
        .style3
        {
            color: #ff0000;
        }
        BODY
        {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
        TD
        {
            font-size: 9pt;
        }
        .style4
        {
            color: #ff0000;
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="Table1" style="z-index: 101; position: absolute; top: 5%; left: 5%" cellspacing="1"
        cellpadding="1" width="85%" border="1">
        <tr>
            <td bgcolor="#e4e5f7" colspan="3">
                <font face="����" color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;�̻��˿��ѯ</font> </FONT>
            </td>
            <td align="right" bgcolor="#e4e5f7">
                <font face="����">����Ա����: <span class="style3">
                    <asp:Label ID="Label_uid" runat="server" Width="73px"></asp:Label></span></font>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label Style="z-index: 0" ID="Label4" runat="server">���׵���</asp:Label>
            </td>
            <td>
                <asp:TextBox Style="z-index: 0" ID="tbTransID" runat="server"></asp:TextBox><asp:Label
                    ID="Label12" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td align="right">
                <asp:Label Style="z-index: 0" ID="Label10" runat="server">�˿��</asp:Label>
            </td>
            <td>
                <asp:TextBox Style="z-index: 0" ID="tbDrawID" runat="server"></asp:TextBox><asp:Label
                    Style="z-index: 0" ID="Label13" runat="server" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server">��ʼ����</asp:Label>
            </td>
            <td>
                <font face="����">
                    <asp:TextBox ID="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:TextBox></font>
            </td>
            <td align="right">
                <asp:Label ID="Label3" runat="server">��������</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label Style="z-index: 0" ID="Label11" runat="server">�̻���</asp:Label>
            </td>
            <td>
                <asp:TextBox Style="z-index: 0" ID="tbSPID" runat="server"></asp:TextBox><asp:Label
                    Style="z-index: 0" ID="Label15" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td align="right">
                <asp:Label Style="z-index: 0" ID="Label7" runat="server">����ʺ�</asp:Label>
            </td>
            <td>
                <asp:TextBox Style="z-index: 0" ID="tbBuyerID" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label Style="z-index: 0" ID="Label5" runat="server">�˿�����</asp:Label>
            </td>
            <td>
                <asp:DropDownList Style="z-index: 0" ID="ddlrefund_type" runat="server" Width="152px">
                    <asp:ListItem Value="99" Selected="True">��������</asp:ListItem>
                    <asp:ListItem Value="1">B2C�˿�</asp:ListItem>
                    <asp:ListItem Value="2">��Ҫ����</asp:ListItem>
                    <asp:ListItem Value="3">����ֱ���˿�</asp:ListItem>
                    <asp:ListItem Value="4">�����˿�</asp:ListItem>
                    <asp:ListItem Value="5">���ÿ��˿�</asp:ListItem>
                    <asp:ListItem Value="6">�����˿�</asp:ListItem>
                    <asp:ListItem Value="9">��ֵ���˿�</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="right">
                <asp:Label Style="z-index: 0" ID="Label6" runat="server">ҵ��״̬</asp:Label>
            </td>
            <td>
                <asp:DropDownList Style="z-index: 0" ID="ddlStatus" runat="server" Width="152px">
                    <asp:ListItem Value="99" Selected="True">����״̬</asp:ListItem>
                    <asp:ListItem Value="1">������</asp:ListItem>
                    <asp:ListItem Value="2">������</asp:ListItem>
                    <asp:ListItem Value="3">����ʧ��</asp:ListItem>
                    <asp:ListItem Value="4">�˿�ɹ�</asp:ListItem>
                    <asp:ListItem Value="5">�˿�ʧ��</asp:ListItem>
                    <asp:ListItem Value="6">��������</asp:ListItem>
                    <asp:ListItem Value="7">ת�����</asp:ListItem>
                    <asp:ListItem Value="8">�ݲ�����</asp:ListItem>
                    <asp:ListItem Value="9">�˿�������</asp:ListItem>
                    <asp:ListItem Value="10">ת������ɹ�</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label8" runat="server">��������</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlBankType" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
            <td align="right">
                <asp:Label ID="Label9" runat="server">����״̬</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlSumType" runat="server" Width="152px">
                    <asp:ListItem Value="99">����״̬</asp:ListItem>
                    <asp:ListItem Value="0">��δ����</asp:ListItem>
                    <asp:ListItem Value="1">�Ѿ�����</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label16" runat="server">�������</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlTableType" runat="server" Width="152px">
                    <asp:ListItem Value="1">�̻��˿����뵥</asp:ListItem>
                    <asp:ListItem Value="2">�����˿����뵥</asp:ListItem>
                    <asp:ListItem Value="3">���ձ�</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="right">
                <asp:Button ID="Button1" runat="server" Width="80px" Text="�� ѯ" OnClick="Button2_Click">
                </asp:Button>
            </td>
        </tr>
    </table>
    <table id="Table2" style="z-index: 102; position: absolute; width: 85%; top: 38%;
        left: 5.02%" cellspacing="1" cellpadding="1" border="1" runat="server">
        <tr>
            <td valign="top">
                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" PageSize="50" BorderColor="#E7E7FF"
                    BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Horizontal"
                    AutoGenerateColumns="False">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="FSpID" HeaderText="SPID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftransaction_id" HeaderText="���׵�"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fbuyid" HeaderText="����ʺ�"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Frp_feeName" HeaderText="����ҽ��"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Frb_feeName" HeaderText="�����ҷ���"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Frefund_typeName" HeaderText="�˿�����"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FstatusName" HeaderText="ҵ��״̬"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftrue_name" HeaderText="�ʻ�����"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Ftransaction_id"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Fstatus"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="��ϸ">
                            <ItemTemplate>
                                <a href='<%# String.Format("B2cReturnQuery_Detail.aspx?tranid={0}&drawid={1}", DataBinder.Eval(Container, "DataItem.Ftransaction_id").ToString()
										, DataBinder.Eval(Container, "DataItem.Fdraw_id").ToString()) %>'>��ϸ </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="Frefund_type"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Fdraw_id" HeaderText="Fdraw_id"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr style="height:25">
            <td>
                <webdiyer:AspNetPager ID="pager" runat="server" AlwaysShow="True" NumericButtonCount="5"
                    ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    HorizontalAlign="right" OnPageChanged="ChangePage" SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]">
                </webdiyer:AspNetPager>
            </td>
        </tr>
        <tr>
            <td align="center">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
