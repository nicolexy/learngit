<%@ Page language="c#" Codebehind="FreezeReason.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.FreezeReason" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FreezeReason</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
.style2 {
	FONT-WEIGHT: bold; COLOR: #ff0000
}
BODY {
	BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif)
}
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT>
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 30%; WIDTH: 336px; POSITION: absolute; TOP: 15%; HEIGHT: 335px"
				cellSpacing="1" cellPadding="1" width="336" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;
									<asp:label id="Label1_state" runat="server" Width="88px" ForeColor="Red">Label</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:label id="Label2" runat="server">����Ա���룺</asp:label><asp:label id="labUid" runat="server" Width="64px"></asp:label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<TABLE id="Table1" style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0"
							runat="server">
							<TR borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"></TD>
							</TR>
							<TR>
								<TD align="center">
									<P align="right"><asp:label id="Label10" runat="server">���׵��ţ�</asp:label><FONT face="����">:</FONT></P>
								</TD>
								<TD align="center" width="70%" colSpan="3">
									<P align="left"><asp:label id="Label_listID" runat="server" ForeColor="Red">0001</asp:label></P>
								</TD>
							</TR>
							<TR borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 17px" align="center">
									<P align="right">
										<asp:label id="Label14" runat="server">�û�������</asp:label><FONT face="����">:</FONT></P>
								</TD>
								<TD style="HEIGHT: 17px" align="center" colSpan="3">
									<P align="left">
										<asp:TextBox id="tbUserName" runat="server" Width="192px"></asp:TextBox>
										<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator"
											ControlToValidate="tbUserName">�������û���</asp:RequiredFieldValidator></P>
								</TD>
							</TR>
							<TR borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"><FONT face="����"></FONT></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right">
										<asp:label id="Label5" runat="server">��ϵ��ʽ��</asp:label><FONT face="����">:</FONT></P>
								</TD>
								<TD style="HEIGHT: 18px" align="center" colSpan="3">
									<P align="left">
										<asp:TextBox id="tbContact" runat="server" Width="192px"></asp:TextBox>
										<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="RequiredFieldValidator"
											ControlToValidate="tbContact">��������ϵ��ʽ</asp:RequiredFieldValidator></P>
								</TD>
							</TR>
                            <TR>
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right"><asp:label id="Label1" runat="server">����������</asp:label><FONT face="����">:</FONT></P>
								</TD>
								<TD style="HEIGHT: 18px" align="center" colSpan="3">
                                <P align="left">
                                  <asp:dropdownlist id="ddlFreezeChannel" runat="server">
							         <asp:ListItem Value="1" Selected="True">��ض���</asp:ListItem>
                                     <asp:ListItem Value="2">���Ķ���</asp:ListItem>
                                     <asp:ListItem Value="3">�û�����</asp:ListItem>
                                     <asp:ListItem Value="4">�̻�����</asp:ListItem>
                                     <asp:ListItem Value="5">BG�ӿڶ���</asp:ListItem>
                                     <asp:ListItem Value="6">���ӿ��ɽ��׶���</asp:ListItem>
                                  </asp:dropdownlist>
                                </P>
								</TD>
							</TR>
							<TR style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right">
										<asp:label id="labReason" runat="server">��ϵ��ʽ��</asp:label><FONT face="����">:</FONT></P>
								</TD>
								<TD style="HEIGHT: 18px" align="center" colSpan="3">
									<P align="left">
										<asp:TextBox id="tbMemo" runat="server" Width="208px" Height="92px" TextMode="MultiLine"></asp:TextBox></P>
								</TD>
							</TR>
							<TR style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"></TD>
							</TR>
						</TABLE>
						<asp:button id="BT_F_Or_Not" runat="server" Text=" " Width="87px" onclick="BT_F_Or_Not_Click"></asp:button>
						<asp:Button id="Button1_back" runat="server" ForeColor="Red" Width="118px" Text=" ��������" Height="25px" onclick="Button1_back_Click"></asp:Button></TD>
				</TR>
			</TABLE>
			<br>
			<br>
		</form>
	</body>
</HTML>
