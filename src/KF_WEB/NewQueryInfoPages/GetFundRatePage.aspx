<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page Language="c#" CodeBehind="GetFundRatePage.aspx.cs" AutoEventWireup="True"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.GetFundRatePage" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>GetFundRatePage</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
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
    <script language="javascript">
        function openModeBegin() {
            var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.tbx_beginDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
            if (returnValue != null) Form1.tbx_beginDate.value = returnValue;
        }
        function openModeEnd() {
            var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.tbx_endDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
            if (returnValue != null) Form1.tbx_endDate.value = returnValue;
        }
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table border="1" cellspacing="1" cellpadding="1" width="1100">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">��ѯ�û�����������</font>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lb_QQ" runat="server">���룺</asp:Label><asp:TextBox ID="TextBox1_InputQQ"
                    runat="server" Width="350px"></asp:TextBox>  &nbsp;  &nbsp;
                    <input id="WeChatId" name="IDType" runat="server" type="radio" checked/><label for="WeChatId">΢���ʺ�</label>
                            <input id="WeChatQQ" name="IDType" runat="server" type="radio" /><label for="WeChatQQ">΢�Ű�QQ</label>
                            <input id="WeChatMobile" name="IDType" runat="server" type="radio" /><label for="WeChatMobile">΢�Ű��ֻ�</label>
                            <input id="WeChatEmail" name="IDType" runat="server" type="radio" /><label for="WeChatEmail">΢�Ű�����</label>
                            <input id="WeChatUid" name="IDType" runat="server" type="radio" /><label for="WeChatUid">΢���ڲ�ID</label>
                            <input id="WeChatCft" name="IDType" runat="server" type="radio" /><label for="WeChatCft">΢�ŲƸ�ͨ�˺�Or��Q�˺�</label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnQuery" runat="server" Width="80px" Text="�� ѯ" OnClick="btnQuery_Click">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table border="1" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td align="left">
                            �û�������
                            <asp:Label ID="lblName" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            ����״̬��
                            <asp:Label ID="lblAccountStatus" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            ���ֻ���
                            <asp:Label ID="lblCell" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            ����ʱ�䣺
                            <asp:Label ID="lblCreateTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            ��ȫ�����ͣ�
                            <asp:Label ID="lblSafeBankCardType" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            ��ȫ��β�ţ�
                            <asp:Label ID="lblSafeBankCardNoTail" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            �ۼ����棺
                            <asp:Label ID="lblTotalProfit" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            �����ݶ
                            <asp:Label ID="lblBalance" runat="server"></asp:Label>
                        </td>
                     </tr>
                    <tr>
                        <td align="left">
                            ���ͨ��
                            <asp:Label ID="lbLCTBalance" runat="server"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <asp:Button ID="btnBalanceQuery" runat="server" Width="250px" Text="�ʽ���ˮ��ѯ" OnClick="btnBalanceQuery_Click">
                            </asp:Button>
                        </td>
                     </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
         <table id="tableLCTBalanceRoll" visible="false" border="1" cellspacing="0" cellpadding="0" width="1100" runat="server" >
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">���ͨ�����ˮ</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgLCTBalanceRollList" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                         <asp:BoundColumn DataField="Flistid" HeaderText="������">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Facc_time" HeaderText="����ʱ��">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FtypeStr" HeaderText="��������">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="FInOrOUT" HeaderText="��\��">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="Fchannel_idStr" HeaderText="������">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="FstateStr" HeaderText="����״̬">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftotal_feeStr" HeaderText="���">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fmemo" HeaderText="��ע">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                    </Columns>
                <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="BalanceRollPager"  runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="BalanceRollPager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
          </br>
    <table border="1" cellspacing="0" cellpadding="0" width="1100">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">�û��������˻�</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgUserFundSummary" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                       <asp:BoundColumn DataField="Fspid" HeaderText="Fspid" Visible="false">
                         </asp:BoundColumn>
                       <asp:BoundColumn DataField="Fcurtype" HeaderText="Fcurtype" Visible="false">
                       </asp:BoundColumn>
                         <asp:BoundColumn DataField="fund_code" HeaderText="�������" Visible="false">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="close_flag" Visible="false" >
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="fundName" HeaderText="��������">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="close_flagText" HeaderText="��ձ�־">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="transfer_flagText" HeaderText="ת������">
                            <HeaderStyle Width="180px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="buy_validText" HeaderText="��������">
                            <HeaderStyle Width="180px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="profitText" HeaderText="�ۼ�����">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="balanceText" HeaderText="���">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="conText" HeaderText="������">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:ButtonColumn Text="����" HeaderText="����" CommandName="detail"></asp:ButtonColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </br>
     <table border="1" cellspacing="1" cellpadding="1" width="1100">
        <tr>
            <td>
             <label>
                    ��ѯ��ʼʱ�䣺</label><asp:TextBox ID="tbx_beginDate" runat="server"></asp:TextBox><asp:ImageButton
                        ID="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif">
                    </asp:ImageButton><label>&nbsp;&nbsp;��ѯ����ʱ�䣺</label><asp:TextBox ID="tbx_endDate"
                        runat="server"></asp:TextBox><asp:ImageButton ID="ButtonEndDate" runat="server" CausesValidation="False"
                            ImageUrl="../Images/Public/edit.gif"></asp:ImageButton>
                  <span id="queryDiv"  runat="server">
                    <label>
                    ���룺</label><asp:DropDownList ID="ddlDirection" runat="server">
                        <asp:ListItem Selected="True" Value="0">ȫ��</asp:ListItem>
                        <asp:ListItem Value="1">����</asp:ListItem>
                        <asp:ListItem Value="2">ȡ��</asp:ListItem>
                    </asp:DropDownList>
                    <label>
                    ��ע��</label><asp:DropDownList ID="ddlMemo" runat="server">
                        <asp:ListItem Selected="True" Value="">ȫ��</asp:ListItem>
                        <asp:ListItem Value="�����깺">�����깺</asp:ListItem>
                        <asp:ListItem Value="��������">��������</asp:ListItem>
                        <asp:ListItem Value="�����˻�����">����</asp:ListItem>
                    </asp:DropDownList>
                    </span>
                       <asp:Button ID="btnQueryDetail" runat="server" Width="80px" Text="�� ѯ" OnClick="btnQueryDetail_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <br />
    <table id="tableQueryResult"  border="1" cellspacing="0" cellpadding="0" width="1100" runat="server" >
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">��ѯ�û�������������ϸ</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="DataGrid_QueryResult" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fday" HeaderText="����ʱ��">
                            <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fspname" HeaderText="����˾����">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fpur_typeName" HeaderText="��Ŀ">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fprofit_per_ten_thousand" HeaderText="�������">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="F7day_profit_rate_str" HeaderText="�����껯������">
                            <HeaderStyle Width="110px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fvalid_money_str" HeaderText="���汾���">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fprofit_str" HeaderText="������">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="pager" runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="pager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
    <br />
    <table id="tableBankRollList" border="1" cellspacing="0" cellpadding="0" width="1100" runat="server">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">��ѯ�û��ʽ���ˮ���</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgBankRollList" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF" OnItemDataBound="dgBankRollList_ItemDataBound">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��">
                            <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Flistid" HeaderText="������">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FtypeText" HeaderText="��ȡ">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FpaynumText" HeaderText="���">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FbalanceText" HeaderText="�˻����">
                            <HeaderStyle Width="110px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FconStr" HeaderText="�������">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FmemoText" HeaderText="��ע">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fspid" HeaderText="�̻���">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="����">
							<ItemTemplate>
								<asp:LinkButton id="UnCloseFundApplyButton" href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank Visible="false" runat="server" Text="�ͷ�ǿ��"></asp:LinkButton>
							</ItemTemplate>
					   </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="bankRollListPager" runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="bankRollListPager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
     <br />
    <table  id="tableBankRollListNotChildren" border="1" cellspacing="0" cellpadding="0" width="1100" runat="server">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">��ѯ�û�������ˮ���</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgBankRollListNotChildren" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Flistid" HeaderText="���׵���">
                            <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <%--<asp:BoundColumn DataField="Fsub_trans_id_str" HeaderText="������">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>--%>
                         <asp:BoundColumn DataField="Ffetchid" HeaderText="���ֵ���">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FtypeText" HeaderText="��ȡ">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftotal_fee_str" HeaderText="���">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Floading_type_str" HeaderText="��ط�ʽ">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fstate_str" HeaderText="״̬">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="Fcard_no" HeaderText="���п�β��">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="Fbank_type_str" HeaderText="��������">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="bankRollListNotChildrenPager" runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="bankRollListNotChildrenPager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
      <br />
    <table  id="tableCloseFundRoll" border="1" cellspacing="0" cellpadding="0" width="1100" runat="server">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">��ѯ������ϸ</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DataGrid ID="dgCloseFundRoll" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF" OnItemDataBound="dgCloseFundRoll_ItemDataBound">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fseqno" HeaderText="���">
                            <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftrade_id" HeaderText="���׵���">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FDate" HeaderText="�ں�">
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fstart_total_fee_str" HeaderText="�ܽ�����">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fcurrent_total_fee_str" HeaderText="��ǰ�ܽ��">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fend_tail_fee_str" HeaderText="ɨβ���">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftrans_date" HeaderText="��������">
                            <HeaderStyle Width="80px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fstart_date" HeaderText="��Ϣ��">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fend_date" HeaderText="������">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fstate_str" HeaderText="��״̬">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fuser_end_type_str" HeaderText="���ڲ���">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fpay_type_str" HeaderText="֧������">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Fchannel_id_str" HeaderText="������Ϣ">
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:BoundColumn>
                       <%-- <asp:TemplateColumn HeaderText="����">
							<ItemTemplate>
								<a href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank>�ͷ�ǿ��</a>
							</ItemTemplate>
						</asp:TemplateColumn>--%>
                         <asp:TemplateColumn HeaderText="����">
                         <HeaderStyle Width="200px"></HeaderStyle>
							<ItemTemplate>
								<asp:LinkButton id="CloseFundApplyButton" href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank Visible="false" runat="server" Text="�ͷ�ǿ��"></asp:LinkButton>
							</ItemTemplate>
					   </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="CloseFundRollPager" runat="server" HorizontalAlign="right"
                    NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager"
                    SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                    OnPageChanged="CloseFundRollPager_PageChanged">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
