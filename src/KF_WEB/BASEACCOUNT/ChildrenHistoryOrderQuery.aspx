<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="ChildrenHistoryOrderQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ChildrenHistoryOrderQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ComplainBussinessInput</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;��ʷ������ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				
                <TR>
                    <TD align="right"><asp:label id="Label4" runat="server">�Ƹ�ͨ�˺ţ�</asp:label></TD>
					<TD>
						<asp:textbox id="tbCft" runat="server"></asp:textbox>
                    </TD>
                    <TD align="right"><asp:label id="Label5" runat="server">�˻����ͣ�</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlType" runat="server" Width="152px">
							<asp:ListItem Value="80">��Ϸ</asp:ListItem>
                            <asp:ListItem Value="81">����</asp:ListItem>
                            <asp:ListItem Value="82">ֱͨ��</asp:ListItem>
						</asp:DropDownList>
                    </TD>
                </TR>
                <TR>
                    <TD align="right"><asp:label id="Label2" runat="server">��ʼ���ڣ�</asp:label></TD>
                    <TD>
                        <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                    </TD>
                    <TD align="right">
                        <asp:label id="Label3" runat="server">�������ڣ�</asp:label></TD>
                    <td>
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                    </td>
				</TR>	
				<TR>
					<TD align="center" colspan="4">
                    <asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 154px; HEIGHT: 30%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
                <TR>
					<TD bgColor="#e4e5f7" height="18"><SPAN>&nbsp;�û��ʽ���ˮ</SPAN></TD>
				</TR>
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" EnableViewState="False">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="FListid" HeaderText="����ID��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Faction_type_str" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fuid" HeaderText="�ڲ��ʺ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fqqid" HeaderText="�ʻ�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcurtype_str" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftype_str" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsubject_str" HeaderText="���/��Ŀ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftrue_name" HeaderText="�û�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fpaynum_str" HeaderText="���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbalance_str" HeaderText="�ʻ����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffrom_uid" HeaderText="�Է��ڲ��ʺ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffromid" HeaderText="�Է��ʺ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffrom_name" HeaderText="�Է�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fspid" HeaderText="��������(������)"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fprove" HeaderText="����ƾ֤"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fapplyid" HeaderText="Ӧ��ϵͳ��ID"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fip" HeaderText="�ͻ���IP��ַ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmemo" HeaderText="��ע/˵��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_time_acc" HeaderText="����ʱ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_time" HeaderText="����޸�ʱ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fvs_qqid" HeaderText="���׶Է����ʺ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fexplain" HeaderText="���׵���ע"></asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
            
		</form>
	</body>
</HTML>
