<%@ Page language="c#" Codebehind="RefundErrorMain.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundErrorMain" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundErrorMain</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body background="../IMAGES/Page/bg01.gif" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="����">
				<TABLE style="Z-INDEX: 101; POSITION: absolute; WIDTH: 94%; HEIGHT: 80%; TOP: 5%; LEFT: 5%"
					id="Table3" border="1" cellSpacing="1" borderColor="#666666" cellPadding="1" width="383"
					align="center" height="127">
					<TR bgColor="#eeeeee">
						<TD style="HEIGHT: 4px" height="4" colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG src="../IMAGES/Page/post.gif" width="15" height="16"><STRONG>&nbsp;
										<asp:label id="lbTitle" runat="server">lbTitle</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="Label1" runat="server" ForeColor="ControlText">����Ա���룺</asp:label><SPAN class="style3"><asp:label id="Label_uid" runat="server">Label</asp:label></SPAN></STRONG></SPAN></FONT></TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 126px" vAlign="top" width="100%" colSpan="2" align="center"><FONT face="����">
								<TABLE id="Table1" border="1" cellSpacing="1" cellPadding="1" width="99%">
									<TR>
										<TD colSpan="3"><asp:datagrid id="DataGrid1" runat="server" EnableViewState="False" Width="100%" BorderStyle="None"
												BorderWidth="1px" BorderColor="#E7E7FF" BackColor="White" CellPadding="3" AutoGenerateColumns="False"
												GridLines="Horizontal">
												<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
												<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
												<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
												<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
												<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
												<Columns>
													<asp:HyperLinkColumn DataNavigateUrlField="Furl" DataNavigateUrlFormatString="RefundErrorReturn.Aspx?{0}"
														DataTextField="FBatchDay" HeaderText="����">
														<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
														<ItemStyle Font-Underline="True" HorizontalAlign="Center" ForeColor="Blue" VerticalAlign="Middle"></ItemStyle>
													</asp:HyperLinkColumn>
													<asp:BoundColumn DataField="FBankTypeName" HeaderText="����"></asp:BoundColumn>
													<asp:HyperLinkColumn DataNavigateUrlField="Furl2" DataNavigateUrlFormatString="RefundErrorHandle.Aspx?{0}"
														DataTextField="FPayCount" HeaderText="�ܱ���">
														<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														<ItemStyle Font-Underline="True" HorizontalAlign="Right" ForeColor="Blue"></ItemStyle>
													</asp:HyperLinkColumn>
													<asp:BoundColumn DataField="FPaySum1" HeaderText="�ܽ��" DataFormatString="{0:N}">
														<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FrefundPath" HeaderText="�˿�;��"></asp:BoundColumn>
													<asp:BoundColumn DataField="FStatusName" HeaderText="��ǰ״̬"></asp:BoundColumn>
													<asp:BoundColumn DataField="FMsg" HeaderText="������ʾ"></asp:BoundColumn>
													<asp:BoundColumn DataField="FProposer" HeaderText="������"></asp:BoundColumn>
													<asp:BoundColumn DataField="FBatchid" HeaderText="���κ�"></asp:BoundColumn>
													<asp:BoundColumn DataField="FApproveDate" HeaderText="ִ��ʱ��"></asp:BoundColumn>
												</Columns>
												<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
											</asp:datagrid></TD>
									</TR>
								</TABLE>
							</FONT>
						</TD>
					</TR>
					<tr>
						<td style="HEIGHT: 48px" height="25" colSpan="2" align="center"><FONT face="����"></FONT></td>
					</tr>
					<TR>
						<TD height="25" align="center"><asp:label id="Label2" runat="server">��ʼ����</asp:label>
                            <asp:textbox id="TextBoxBeginDate" runat="server" Width="100px" BorderWidth="1px" BorderColor="Gray"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>&nbsp;&nbsp;
							<asp:label id="Label3" runat="server">��������</asp:label>
                            <asp:textbox id="TextBoxEndDate" runat="server" Width="100px"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>&nbsp;&nbsp;
							<asp:label id="Label4" runat="server">����״̬</asp:label><asp:dropdownlist id="ddlState" runat="server" Width="115px">
								<asp:ListItem Value="9999">����״̬</asp:ListItem>
								<asp:ListItem Value="1">�����ٴ������˿�</asp:ListItem>
								<asp:ListItem Value="4">�����˿�����ͨ��</asp:ListItem>
								<asp:ListItem Value="96">�����˿���</asp:ListItem>
								<asp:ListItem Value="10">�����˿����ص����</asp:ListItem>
								<asp:ListItem Value="11">��������˹���Ȩ����</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="2">���븶���˿�</asp:ListItem>
								<asp:ListItem Value="5">�����˿�����ͨ��</asp:ListItem>
								<asp:ListItem Value="7">�����˿��˿���</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="3">�����˹���Ȩ�˿�</asp:ListItem>
								<asp:ListItem Value="6">��Ȩ�˿�����ͨ��</asp:ListItem>
								<asp:ListItem Value="9">��Ȩ�˿���</asp:ListItem>
								<asp:ListItem Value="97">��Ȩ���������˿���</asp:ListItem>
								<asp:ListItem Value="98">��Ȩ��������</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="12">�˿�ֱ�ӵ���Ϊ�ɹ�������</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="13">����ת�����������</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="99">���δ������</asp:ListItem>
							</asp:dropdownlist>&nbsp;&nbsp;
							<asp:label id="Label6" runat="server">��������</asp:label><asp:dropdownlist id="ddlBankType" runat="server" Width="104px"></asp:dropdownlist>&nbsp;&nbsp;
							<asp:label id="Label7" runat="server">;��</asp:label><asp:dropdownlist id="ddlRefundPath" runat="server">
								<asp:ListItem Value="9999">����״̬</asp:ListItem>
								<asp:ListItem Value="3E">�˹���Ȩ</asp:ListItem>
								<asp:ListItem Value="6E">�����˿�</asp:ListItem>
								<asp:ListItem Value="1E">�ٴ��˿�</asp:ListItem>
								<asp:ListItem Value="7E">ת�����</asp:ListItem>
								<asp:ListItem Value="9E">ֱ�ӳɹ�</asp:ListItem>
							</asp:dropdownlist>&nbsp;&nbsp;
							<asp:label style="Z-INDEX: 0" id="Label5" runat="server">������</asp:label><asp:textbox style="Z-INDEX: 0" id="txtProposer" runat="server" Width="80px"></asp:textbox></TD>
						<TD>
							<asp:button id="btnQuery" runat="server" Width="93px" Text="ȡ������״̬" onclick="btnQuery_Click"></asp:button></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
