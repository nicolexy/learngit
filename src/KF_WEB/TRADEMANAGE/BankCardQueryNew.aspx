<%@ Page Language="c#" CodeBehind="BankCardQueryNew.aspx.cs" AutoEventWireup="True"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BankCardQueryNew" %>

<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>BankCardQueryNew</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
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
        .auto-style1
        {
            width: 173px;
        }
        .auto-style2
        {
            height: 41px;
        }
        .auto-style3
        {
            width: 173px;
            height: 41px;
        }
    </style>
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="Table1" style="z-index: 101; left: 5%; position: absolute; top: 5%" cellspacing="1"
        cellpadding="1" width="820px" border="1">
        <tr>
            <td bgcolor="#e4e5f7" colspan="4">
                <font face="����" color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">
                    &nbsp;&nbsp;���п���ѯ</font>
            </td>
            <td align="right" bgcolor="#e4e5f7">
                <font face="����">����Ա����: <span class="style3">
                    <asp:Label ID="Label1" runat="server" Width="73px"></asp:Label></span></font>
            </td>
        </tr>
        <tr>
            <td align="right" nowrap>
                <asp:Label ID="Label5" runat="server">���п���</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtBankCardID" runat="server" Width="250px"></asp:TextBox>
            </td>
            <td align="right" class="auto-style1">
                <asp:Label ID="Label3" runat="server">ҵ������</asp:Label>
            </td>
               <TD><asp:dropdownlist id="ddlBizType" runat="server" Width="152px">
							<asp:ListItem Value="10100">֧��</asp:ListItem>
							<asp:ListItem Value="10200">����</asp:ListItem>
							<asp:ListItem Value="10300">�˿�</asp:ListItem>
                           <asp:ListItem Value="10400">ATM��ֵ</asp:ListItem>
                           <asp:ListItem Value="10500">session����</asp:ListItem>
						</asp:dropdownlist>
               </TD>
   
        </tr>

                <tr>
            <td align="right" nowrap class="auto-style2">
                <asp:Label ID="Label6" runat="server">��������</asp:Label>
            </td>
               <TD class="auto-style2">
                       <asp:DropDownList ID="DropOldBankType" runat="server" Width = "200px"  borderwidth = "2px" BorderStyle = "Ridge" >
                                                                                   
                        </asp:DropDownList>
               </TD>
                    
            <td  colspan="2" align="right">
               ���ڣ�<asp:TextBox ID="TextBoxDate" runat="server" Width="160px" onClick="WdatePicker()"  CssClass="Wdate"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td align="left" colspan="2"><asp:label id="Label10" runat="server">ѡ��������ѯ�ļ���</asp:label>&nbsp;<asp:FileUpload id="File1" runat="server" /></td>
            <td align="center" colspan="3">
                <asp:Button ID="btnQuery" runat="server" Width="100px" Text="������ѯ" OnClick="btnBatchQuery"></asp:Button>&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" Width="80px" Text="�� ѯ" OnClick="btnSearch_Click">
                </asp:Button>
            </td>
        </tr>
    </table>
    <table id="Table2" style="z-index: 102; left: 5.02%; width: 85%; position: absolute;
        top: 175px; height: 70%" cellspacing="1" cellpadding="1" width="808" border="1"
        runat="server">
        <tr>
            <td valign="top">
                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="fpay_acc" HeaderText="���п���"></asp:BoundColumn>
                        <asp:HyperLinkColumn DataNavigateUrlField="Furl"  DataTextField="fbank_order" HeaderText="���ж�����">
														<ItemStyle Font-Underline="True" ForeColor="Blue"></ItemStyle>
						</asp:HyperLinkColumn>
                       <%-- <asp:BoundColumn DataField="fbank_order" HeaderText="���ж�����"></asp:BoundColumn>--%>
                        <asp:BoundColumn DataField="FamtStr" HeaderText="���"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fbiz_type_str" HeaderText="ҵ��״̬"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr style="height: 25">
            <td>
                <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonTextFormatString="[{0}]"
                    SubmitButtonText="ת��" OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager"
                    ShowInputBox="always" PagingButtonSpacing="0" ShowCustomInfoSection="left" NumericButtonCount="5"
                    AlwaysShow="True">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
