<%@ Page language="c#" Codebehind="DKAdjust.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.DKAdjust" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FreezeReason</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 20%; POSITION: absolute; TOP: 15%; HEIGHT: 335px"
				cellSpacing="1" cellPadding="1" width="60%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;
									<asp:label id="Label1_state" runat="server" ForeColor="Red">��������״̬</asp:label></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<TABLE id="Table1" style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0"
							runat="server">
							<TR borderColor="#999999" bgColor="#999999">
								<TD align="center" colSpan="4"><asp:label id="labError" runat="server" ForeColor="Red"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right"><asp:label id="labInputfile" runat="server" Visible="False">�����ļ��ϴ���</asp:label></P>
								</TD>
								<TD style="HEIGHT: 18px" align="center" colSpan="3"><P align="left"><INPUT id="uploadFile" onpropertychange="startRequest('../GetFileCount.aspx',uploadFile)"
											type="file" size="44" name="uploadFile" runat="server">
										<asp:button id="btnFile" runat="server" Visible="False" Text="�ϴ��ļ�" Width="87px" onclick="btnFile_Click"></asp:button></P>
								</TD>
							</TR>
							<TR>
								<TD align="center">
									<P align="right"><asp:label id="lab11" runat="server">�������Σ�</asp:label></P>
								</TD>
								<TD align="center" width="70%" colSpan="3">
									<P align="left"><asp:label id="lab21" runat="server" ForeColor="Red"></asp:label></P>
								</TD>
							</TR>
							<TR>
								<TD align="center">
									<P align="right"><asp:label id="lab12" runat="server">���������</asp:label></P>
								</TD>
								<TD align="center" width="70%" colSpan="3">
									<P align="left"><asp:label id="lab22" runat="server" ForeColor="Red"></asp:label></P>
								</TD>
							</TR>
							<TR>
								<TD align="center">
									<P align="right"><asp:label id="lab13" runat="server">�����</asp:label></P>
								</TD>
								<TD align="center" width="70%" colSpan="3">
									<P align="left"><asp:label id="lab23" runat="server" ForeColor="Red"></asp:label></P>
								</TD>
							</TR>
							<TR borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="100%" BorderColor="#E7E7FF" BorderStyle="None"
										BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
										PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
										<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
										<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
										<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
										<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
										<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
										<Columns>
											<asp:BoundColumn Visible="False" DataField="Fcep_id" HeaderText="���׵���">
												<HeaderStyle HorizontalAlign="Center" Width="250px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Fbankacc_no" HeaderText="���п���"></asp:BoundColumn>
											<asp:BoundColumn DataField="Funame" HeaderText="�û���"></asp:BoundColumn>
											<asp:BoundColumn DataField="FpaynumName" HeaderText="�������"></asp:BoundColumn>
										</Columns>
										<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 17px" align="center">
									<P align="right"><asp:label id="Label14" runat="server">�������ͣ�</asp:label></P>
								</TD>
								<TD style="HEIGHT: 17px" align="center" colSpan="3"><P align="left"><asp:radiobuttonlist id="RadioButtonList1" runat="server" Width="100%" Enabled="False" RepeatDirection="Horizontal">
											<asp:ListItem Value="FAIL" Selected="True">����Ϊʧ��</asp:ListItem>
											<asp:ListItem Value="SUCCESS">����Ϊ�ɹ�</asp:ListItem>
										</asp:radiobuttonlist></P>
								</TD>
							</TR>
							<TR borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"><FONT face="����"></FONT></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right"><asp:label id="Label5" runat="server">����ԭ��</asp:label></P>
								</TD>
								<TD style="HEIGHT: 18px" align="center" colSpan="3"><P align="left"><asp:textbox id="tbReason" runat="server" Width="100%"></asp:textbox></P>
								</TD>
							</TR>
							<TR style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right"><asp:label id="labReason" runat="server">������</asp:label></P>
								</TD>
								<TD style="HEIGHT: 18px" align="center" colSpan="3"><P align="left"><INPUT id="uploadImg" onpropertychange="startRequest('../GetFileCount.aspx',uploadFile)"
											type="file" size="44" name="uploadImg" runat="server">
										<asp:hyperlink id="HyperLink1" runat="server" Visible="False">�鿴����</asp:hyperlink></P>
								</TD>
							</TR>
							<TR style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"></TD>
							</TR>
						</TABLE>
						<asp:button id="bt_ok" runat="server" Text="ȷ���ύ" Width="87px" onclick="bt_ok_Click"></asp:button><asp:button id="btn_cancel" runat="server" Text="ȡ��" Width="87px" Height="25px" onclick="btn_cancel_Click"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
