<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page Language="c#" CodeBehind="FCATransferQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCATransferQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>FCATransferQuery</title>
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
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
    <script>
        function CheckEmail() {
            var txtEmail = document.getElementById("txtEmail");
            if (txtEmail != null) {
                if (txtEmail.value.replace(/^\s*/, "").replace(/\s*$/, "").length == 0) {
                    txtEmail.focus();
                    txtEmail.select();
                    alert("���䲻����Ϊ��!");
                    return false;
                }
            }
        }
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellspacing="1" cellpadding="1" width="820"
            border="1">
            <tr>
                <td style="WIDTH: 100%" bgcolor="#e4e5f7" colspan="5"><font face="����"><font color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�̻������ѯ</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </font>����Ա����: </FONT><span class="style3"><asp:Label ID="Label1" runat="server" ForeColor="Red" Width="73px"></asp:Label></span></td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label2" runat="server">�̻���ţ�</asp:Label></td>
                <td>
                    <asp:TextBox ID="txtspid" Style="WIDTH: 180px;" runat="server"></asp:TextBox><font color="red">*</font></td>
                <td align="right">
                    <asp:Label ID="Label5" runat="server">��ѯʱ�䣺</asp:Label></td>
                <td>
                    <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                    <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                    ��
                    <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                    <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                    <font color="red">*</font></td>
                <td align="center">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="�� ѯ" OnClick="btnQuery_Click"></asp:Button>
            </tr>
        </table>
        <table id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 184px; HEIGHT: 35%"
            cellspacing="1" cellpadding="1" width="808" border="1" runat="server">
            <tr>
                <td valign="top">
                    <asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                        BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="Fspid" HeaderText="�̻����"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fdraw_date" HeaderText="��������"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fcur_type_str" HeaderText="����"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fsettle_amount_str" HeaderText="���㾻���A"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Fcycins_unfreeze_amout_str" HeaderText="ѭ���ͷ�B"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Ffixins_freeze_amount_str" HeaderText="���ɹ̱�C"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Frefuse_unfreeze_amount_str" HeaderText="�ܸ��ⶳD"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Frefuse_freeze_amount_str" HeaderText="�ܸ�����E"></asp:BoundColumn>
                            <asp:BoundColumn DataField="huakuan_str" HeaderText="������M=A+B-C+D-E"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
            <tr height="25">
                <td>
                    <webdiyer:AspNetPager ID="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
                        PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
                        SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]">
                    </webdiyer:AspNetPager>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
