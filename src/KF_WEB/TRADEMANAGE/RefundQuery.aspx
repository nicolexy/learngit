<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="RefundQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.RefundQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundMain</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="����">
				<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 3%; WIDTH: 94%; POSITION: absolute; TOP: 5%; HEIGHT: 90%"
					cellSpacing="1" cellPadding="1" width="648" border="1">
					<TR bgColor="#eeeeee">
						<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><asp:label id="lbTitle" runat="server">������ϸ</asp:label><FONT face="����"></FONT></SPAN></FONT></STRONG></TD>
					</TR>
					<TR>
						<TD colSpan="2">
							<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="1">
								<TR>
									<TD><asp:label id="Label2" runat="server">ѡ������</asp:label></TD>
									<TD>
                                        <asp:textbox id="TextBoxBeginDate" runat="server" Width="100px" BorderStyle="Groove"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
                                        </TD>
									<TD><asp:label id="Label1" runat="server">�˿�����</asp:label></TD>
									<TD><asp:dropdownlist id="ddlBankType" runat="server"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD><asp:label id="Label3" runat="server">������Դ</asp:label></TD>
									<TD><asp:dropdownlist id="ddlFromType" runat="server">
											<asp:ListItem Value="9" Selected="True">������Դ</asp:ListItem>
											<asp:ListItem Value="1">�̻��˵�</asp:ListItem>
											<asp:ListItem Value="2">���ʽ���˵�</asp:ListItem>
											<asp:ListItem Value="3">�˹�¼���˵�</asp:ListItem>
										</asp:dropdownlist></TD>
									<TD><asp:label id="Label4" runat="server">�˿ʽ</asp:label></TD>
									<TD><asp:dropdownlist id="ddlRefundType" runat="server">
											<asp:ListItem Value="9" Selected="True">���з�ʽ</asp:ListItem>
											<asp:ListItem Value="1">�����˵�</asp:ListItem>
											<asp:ListItem Value="2">�ӿ��˵�</asp:ListItem>
											<asp:ListItem Value="3">�˹���Ȩ</asp:ListItem>
											<asp:ListItem Value="4">ת���˵�</asp:ListItem>
											<asp:ListItem Value="5">ת�����</asp:ListItem>
											<asp:ListItem Value="6">�����˿�</asp:ListItem>
										</asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD><asp:label id="Label5" runat="server">�˿�״̬</asp:label></TD>
									<TD><asp:dropdownlist id="ddlRefundState" runat="server">
											<asp:ListItem Value="9" Selected="True">����״̬</asp:ListItem>
											<asp:ListItem Value="0">��ʼ״̬</asp:ListItem>
											<asp:ListItem Value="1">�˵�������</asp:ListItem>
											<asp:ListItem Value="2">�˵��ɹ�</asp:ListItem>
											<asp:ListItem Value="3">�˵�ʧ��</asp:ListItem>
											<asp:ListItem Value="4">�˵�״̬δ��</asp:ListItem>
											<asp:ListItem Value="5">�ֹ��˵���</asp:ListItem>
											<asp:ListItem Value="6">�����ֹ��˵�</asp:ListItem>
											<asp:ListItem Value="7">����ת�����</asp:ListItem>
										</asp:dropdownlist></TD>
									<TD><asp:label id="Label6" runat="server">�ص�״̬</asp:label></TD>
									<TD><asp:dropdownlist id="ddlReturnState" runat="server">
											<asp:ListItem Value="9" Selected="True">����״̬</asp:ListItem>
											<asp:ListItem Value="1">�ص�ǰ</asp:ListItem>
											<asp:ListItem Value="2">�ص���</asp:ListItem>
										</asp:dropdownlist></TD>
								</TR>
                                <TR>
									<TD><asp:label id="Label8" runat="server">ѡ�񳡴�</asp:label></TD>
									<TD><asp:dropdownlist id="ddlFbatchid" runat="server">
											<asp:ListItem Value="0" Selected="True">ѡ�񳡴�</asp:ListItem>
											<asp:ListItem Value="R">1</asp:ListItem>
											<asp:ListItem Value="T">2</asp:ListItem>
											<asp:ListItem Value="U">3</asp:ListItem>
											<asp:ListItem Value="W">4</asp:ListItem>
											<asp:ListItem Value="Y">5</asp:ListItem>
										</asp:dropdownlist></TD>
									<TD><asp:label id="Label9" runat="server">�����ж�����</asp:label></TD>
                                    <TD><asp:TextBox id="tbFbank_listid" runat="server"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD><asp:label id="Label7" runat="server">�˿ID</asp:label></TD>
									<TD><asp:textbox id="tbListID" runat="server"></asp:textbox></TD>
									<TD align="center" colSpan="2"><asp:button id="btnQuery" runat="server" Text="��ѯ��¼" onclick="btnQuery_Click"></asp:button></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD vAlign="top" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BorderStyle="None" Height="122px" AutoGenerateColumns="False"
								BackColor="White" CellPadding="3" GridLines="Horizontal" BorderColor="#E7E7FF" BorderWidth="1px">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FPaylistid" HeaderText="�˿"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_listid" HeaderText="������"></asp:BoundColumn>
									<asp:BoundColumn DataField="FreturnamtName" HeaderText="�˵����"></asp:BoundColumn>
									<asp:BoundColumn DataField="FamtName" HeaderText="�������"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fpay_front_time" HeaderText="֧��ʱ��"></asp:BoundColumn>
									<asp:BoundColumn DataField="FstateName" HeaderText="�˵�״̬"></asp:BoundColumn>
									<asp:BoundColumn DataField="FreturnStateName" HeaderText="�ص�״̬"></asp:BoundColumn>
									<asp:BoundColumn DataField="FrefundPathName" HeaderText="�˵�;��"></asp:BoundColumn>
									<asp:BoundColumn DataField="FRefundID" HeaderText="�˵�ID"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD colSpan="2"><webdiyer:aspnetpager id="pager" runat="server" PageSize="15" AlwaysShow="True" NumericButtonCount="5"
								ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
								OnPageChanged="ChangePage" SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
					</TR>
					<TR>
						<TD colSpan="2">
							<asp:Label id="labErrMsg" runat="server" ForeColor="Red"></asp:Label>
						</TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
