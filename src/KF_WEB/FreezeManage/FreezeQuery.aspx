<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page Language="c#" CodeBehind="FreezeQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FreezeManage.FreezeVerify" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>FreezeVerify</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
        UNKNOWN
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
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table border="1" cellspacing="1" cellpadding="1" width="1200">
        <tr>
            <td style="width: 443px; height: 20px" bgcolor="#e4e5f7" colspan="2">
                <font color="red" face="����">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16" alt=""><asp:Label ID="lb_pageTitle"
                        runat="server">��ؽⶳ���</asp:Label></font>
            </td>
            <td style="height: 20px">
                <FONT>����Ա����: </FONT><span class="style3"><asp:Label ID="lb_operatorID" runat="server"
                    Width="73px" ForeColor="Red"></asp:Label></span>
            </td>
        </tr>
        <tr>
            <td>
                <label style="vertical-align: middle; width: 80px; height: 20px">
                    �Ƹ�ͨ���룺</label><asp:TextBox ID="tbx_payAccount" runat="server" Width="140px"></asp:TextBox>
            </td>
            <td>
                <label style="vertical-align: middle; width: 80px; height: 20px">
                <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
            </td>
            <td>
                <label style="vertical-align: middle; width: 80px; height: 20px">
                 <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
            </td>
            <td>
                <label style="vertical-align: middle; width: 80px; height: 20px">
                    ����״̬��</label>
                <asp:DropDownList ID="ddl_orderState" runat="server">
                    <asp:ListItem Value="0">δ����</asp:ListItem>
                    <asp:ListItem Value="1">�ᵥ���ѽⶳ��</asp:ListItem>
                    <asp:ListItem Value="2">�ᵥ��δ�ⶳ��</asp:ListItem>
                    <asp:ListItem Value="7">������</asp:ListItem>
                    <asp:ListItem Value="8">����</asp:ListItem>
                    <asp:ListItem Value="99" Selected="True">����</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <label style="vertical-align: middle; width: 80px; height: 20px">
                    �����ţ�</label><asp:TextBox ID="tbx_listNo" runat="server" Width="140px"></asp:TextBox>
            </td>
            <td>
                <label style="vertical-align: middle; width: 80px; height: 20px">
                    �ᵥ��Ա��</label><asp:TextBox ID="tbx_people" runat="server" Width="140px"></asp:TextBox>
            </td>
            <td>
                <label style="vertical-align: middle; width: 80px; height: 20px">
                    ����ԭ��</label><asp:TextBox ID="tbx_reason" runat="server" Width="140px"></asp:TextBox>
            </td>
            <td colspan="2">
                <label style="vertical-align: middle">
                    ��¼������</label><asp:Label ID="lb_count" runat="server" Text="0" />
            </td>
        </tr>
        <tr>
            <td>
                <label style="vertical-align: middle; width: 80px; height: 20px">
                    ����ʽ��</label>
                <asp:DropDownList runat="Server" ID="ddl_queryOrderType">
                    <asp:ListItem Value="1" Selected="True">�ύʱ����������</asp:ListItem>
                    <asp:ListItem Value="2">�ύʱ���������</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td colspan="4" align="center">
                <asp:Button ID="btn_query" runat="server" Text="�� ѯ" Width="120px" onclick="btn_query_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellspacing="0" cellpadding="0" width="1200" align="left">
        <tr>
            <td valign="top" align="left">
                <asp:DataGrid ID="DataGrid_QueryResult" runat="server" Width="1200px" BorderColor="#E7E7FF"
                    BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal"
                    AutoGenerateColumns="False" PageSize="5" HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" Font-Size="13px">
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn DataField="FID" HeaderText="������">
                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fuin" HeaderText="�Ƹ�ͨ����">
                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FreezeReason" HeaderText="����ԭ��">
                            <HeaderStyle HorizontalAlign="Center" Width="260px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FsubmitTime" HeaderText="�ύʱ��">
                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="handleStateName" HeaderText="����״̬">
                            <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="handleUserName" HeaderText="�ᵥ��Ա">
                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:HyperLinkColumn Text="����" Target="_blank" DataNavigateUrlField="OpUrl" HeaderText="����">
                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                        </asp:HyperLinkColumn>
                        <asp:HyperLinkColumn Text="��־" Target="_blank" DataNavigateUrlField="DiaryUrl" HeaderText="��־">
                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn Visible="False" DataField="Fuincolor"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td>
                <webdiyer:AspNetPager ID="pager" runat="server" HorizontalAlign="right" AlwaysShow="True"
                    NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��" CssClass="mypager"
                    ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
