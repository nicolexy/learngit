<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page Language="c#" CodeBehind="QueryLCTActivity.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Activity.QueryLCTActivity" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ComplainBussinessInput</title>
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
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellspacing="1" cellpadding="1" width="920"
            border="1">
            <tr>
                <td style="WIDTH: 100%" bgcolor="#e4e5f7" colspan="4"><font face="����"><font color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;���ͨ���ѯ</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </font>����Ա����: </FONT><span class="style3"><asp:Label ID="Label1" runat="server" ForeColor="Red" Width="73px"></asp:Label></span></td>
            </tr>
            <tr>

                <td align="right">
                    <asp:Label ID="Label5" runat="server">��ʼ���ڣ�</asp:Label></td>
                <td>
                    <%--<asp:TextBox ID="TextBoxBeginDate" runat="server"></asp:TextBox><asp:ImageButton ID="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:ImageButton>--%>
                    <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                    <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                     </td>
                <td align="right">
                    <asp:Label ID="Label4" runat="server">�������ڣ�</asp:Label></td>
                <td>
                    <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                    <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                    <%--<asp:TextBox ID="TextBoxEndDate" runat="server"></asp:TextBox><asp:ImageButton ID="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:ImageButton>--%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server">�Ƹ�ͨ�˺ţ�</asp:Label></td>
                <td>
                    <asp:TextBox ID="txtCftNo" Style="WIDTH: 300px;" runat="server"></asp:TextBox></td>
                <td align="right">
                    <asp:Label ID="Label2" runat="server">����ƣ�</asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlActId" runat="server">
                        <asp:ListItem Value="lct" Selected="True">���ͨ�</asp:ListItem>
                        <asp:ListItem Value="userfbsyk" Selected="false">�û��������濨</asp:ListItem>
                    </asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="�� ѯ" OnClick="btnQuery_Click"></asp:Button>
            </tr>
        </table>
        <table id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 154px; HEIGHT: 35%"
            cellspacing="1" cellpadding="1" width="920px" border="1" runat="server">
            <tr>
                <td valign="top" colspan="16">
                    <asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                        BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FPayUin" HeaderText="΢��֧���˺�"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FSPID" HeaderText="�̻���"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FTransId" HeaderText="�ʸ񵥺�"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FMoneyStr" HeaderText="�깺���"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FExpiredTime" HeaderText="�ʸ����ʱ��"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FStateStr" HeaderText="�û�״̬"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FUserTypeStr" HeaderText="�û�����"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FPrizeTypeStr" HeaderText="���еȼ�"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FPrizeMoneyStr" HeaderText="���н�Ԫ��"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FPrizeTime" HeaderText="�齱ʱ��"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FPrizeModifyTime" HeaderText="����ʱ��"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FStandby2" HeaderText="��ע"></asp:BoundColumn>
                            <asp:ButtonColumn Text="����" HeaderText="����" CommandName="detail"></asp:ButtonColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
            <tr>
                <td valign="top" colspan="16">
                    <asp:DataGrid ID="DataGrid2" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                        BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                        <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FUin" HeaderText="�û�iD"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FType" HeaderText="ʹ������"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FRate" HeaderText="��������"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FProfitDays" HeaderText="��������"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FStatus" HeaderText="������ʹ��״̬"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FDealTime" HeaderText="������ʹ��ʱ��"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FExpiredTime" HeaderText="����������ʱ��"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FProfitStartDay" HeaderText="���������ʼ����"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FProfitEndDay" HeaderText="��������������"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FProfitInfo" HeaderText="�����������Ļ����б�"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
            <tr height="25">
                <td colspan="16">
                    <webdiyer:AspNetPager ID="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
                        PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
                        SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]">
                    </webdiyer:AspNetPager>
                </td>
            </tr>
            <tr bgcolor="#e4e5f7">
                <td align="left" colspan="16" style="height: 20px;">
                    <font><b>��ϸ��Ϣ</b></font>
                </td>
            </tr>
            <tr bgcolor="#e4e5f7">
                <td style="height: 25px;">���</td>
                <td style="height: 25px;">�����</td>
                <td style="height: 25px;">�깺����</td>
                <td>����״̬</td>
                <td>���κ�</td>
                <td>�깺����</td>
                <td>��ȯʱ��</td>
                <td>��һ����������</td>
                <td>��Ʒ����ʱ��</td>
                <td>��ƷʧЧʱ��</td>
                <td>������ˮ</td>
                <td>���ͨopenid</td>
                <td>������Ϣ</td>
                <td>������</td>
                <td>�����</td>
                <td>��Ʒ����</td>
                <td>��Ʒ����</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lb_ActId" runat="server"></asp:Label></td>
                 <td>
                    <asp:Label ID="lb_ActName" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_TransId" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_State" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_BatchId" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_Spname" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_SendTicketTime" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_StartDate" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_CreateTime" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_ExpireTime" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_GivePosId" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_Openid" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_ErrorInfo" runat="server"></asp:Label></td>
                <td>
                    <asp:Label ID="lb_ChannelId" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lb_FActType" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lb_FPrizeType" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lb_FPrizeName" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
