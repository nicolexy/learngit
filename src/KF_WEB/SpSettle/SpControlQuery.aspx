<%@ Page language="c#" Codebehind="SpControlQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SpSettle.SpControlQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>batPayReturn</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 2%; POSITION: absolute; TOP: 2%" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">�̻�Ȩ����Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:label id="Label2" runat="server">����Ա���룺</asp:label><asp:label id="labUid" runat="server" Width="64px"></asp:label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD>
						<TABLE style="WIDTH: 1080px; HEIGHT: 38px" cellSpacing="1" cellPadding="1" width="1080"
							border="1">
							<TR>
								<TD style="WIDTH: 489px">�̻���:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:textbox id="txtSpid" runat="server" Width="360px"></asp:textbox></TD>
								<TD><FONT face="����">&nbsp;<asp:button id="Button_qry" runat="server" Text="��ѯ" onclick="Button_qry_Click"></asp:button></FONT></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR bgColor="#b5c7de" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="Label8" runat="server">��ϸ��Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								</SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<TABLE id="Table1" style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0"
							runat="server">
							<TBODY>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label10" runat="server">�̻��ţ�</asp:label></P>
									</TD>
									<TD style="WIDTH: 275px" align="center" width="275"><P align="left"><asp:label id="lbSpid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">���Ʒ�ʽ��</asp:label></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="lbControll" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label14" runat="server">���ƹ���</asp:label></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="lbRule" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label12" runat="server">���������</asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="lbArgs" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label5" runat="server">����Ա��</asp:label></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="lbOperator" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label15" runat="server">����˵����</asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="lbExplain" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label3" runat="server">����ʱ�䣺</asp:label></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="lbCreateTime" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label6" runat="server">�޸�ʱ�䣺</asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="lbModifyTime" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>

							</TBODY>
						</TABLE>
					</TD>
				</TR>
                <TR bgColor="#b5c7de" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="Label4" runat="server">�̻�Ȩ����Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								</SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<TABLE id="Table3" style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0"
							runat="server">
							<TBODY>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label7" runat="server">֧����</asp:label></P>
									</TD>
									<TD style="WIDTH: 275px" align="center" width="275">
                                        <P align="left"><asp:label id="lb_c1" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label11" runat="server">���ˣ�</asp:label></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="lb_c2" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label9" runat="server">���˻��ˣ�</asp:label></P>
									</TD>
									<TD style="WIDTH: 275px" align="center" width="275">
                                        <P align="left"><asp:label id="lb_c3" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label16" runat="server">�˿</asp:label></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="lb_c4" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
                                <TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label18" runat="server">ί�д��ۣ�</asp:label></P>
									</TD>
									<TD style="WIDTH: 275px" align="center" width="275">
                                        <P align="left"><asp:label id="lb_c5" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label20" runat="server">�渶�˿</asp:label></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="lb_c6" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
                                <TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label22" runat="server">����ⶳ��</asp:label></P>
									</TD>
									<TD style="WIDTH: 275px" align="center" width="275">
                                        <P align="left"><asp:label id="lb_c7" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label24" runat="server">���ˣ�</asp:label></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="lb_c8" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
                                <TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label26" runat="server">����֧����</asp:label></P>
									</TD>
									<TD style="WIDTH: 275px" align="center" width="275">
                                        <P align="left"><asp:label id="lb_c9" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label28" runat="server">&nbsp;</asp:label></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="Label29" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>

							</TBODY>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
