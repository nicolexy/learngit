<%@ Page language="c#" Codebehind="changeUserName_2.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.changeUserName_2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>changeUserName_2</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style5 { COLOR: #ff0000 }
		</style>
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT>
			<br>
			<table height="108" cellSpacing="1" cellPadding="0" width="860" align="center" bgColor="#666666"
				border="0">
				<tr background="../IMAGES/Page/bg_bl.gif">
					<td style="HEIGHT: 16px" vAlign="middle" colSpan="4" height="16">
						<table height="25" cellSpacing="0" cellPadding="1" width="859" border="0">
							<tr>
								<td style="HEIGHT: 20px" width="80%" background="../IMAGES/Page/bg_bl.gif"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										�����޸�</font>
								</td>
								<td style="HEIGHT: 20px" width="20%" background="../IMAGES/Page/bg_bl.gif">����Ա����: <span class="style5">
										<asp:label id="Label1" runat="server">Label</asp:label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="WIDTH: 204px; HEIGHT: 25px" width="204" bgColor="#eeeeee">
						<div align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; �����ʺţ�</div>
					</td>
					<td>&nbsp;
						<asp:textbox id="TX_QQID" runat="server" Width="180px" BorderWidth="1px"></asp:textbox><FONT face="����" color="red">&nbsp;*</FONT>
						<asp:requiredfieldvalidator id="rfvqq" runat="server" Width="94px" Display="Dynamic" ControlToValidate="TX_QQID"
							ErrorMessage="����Ϊ��">�������ʻ�����</asp:requiredfieldvalidator>&nbsp;&nbsp;
					</td>
					<td>
						<P align="center"><FONT face="����">&nbsp;&nbsp;</FONT></P>
					</td>
				</tr>
				<TR bgColor="#ffffff">
					<TD style="HEIGHT: 18px" bgColor="#eeeeee"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							����ԭ������</FONT></TD>
					<TD style="HEIGHT: 18px" colSpan="2">&nbsp;
						<asp:textbox id="txtOldName" runat="server" Width="180px" BorderWidth="1px"></asp:textbox><FONT face="����" color="red">&nbsp;*</FONT>
						<asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" Display="Dynamic" ControlToValidate="txtOldName"
							ErrorMessage="RequiredFieldValidator">����Ϊ��</asp:requiredfieldvalidator></TD>
				</TR>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; �����µ�������</FONT></td>
					<td colSpan="2">&nbsp;
						<asp:textbox id="txbNewName" runat="server" Width="180px" BorderWidth="1px"></asp:textbox><FONT face="����" color="red">&nbsp;*</FONT>
						<asp:requiredfieldvalidator id="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="txbNewName"
							ErrorMessage="RequiredFieldValidator">����Ϊ��</asp:requiredfieldvalidator></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; �����µ�������</FONT></td>
					<td colSpan="2">&nbsp;
						<asp:textbox id="txbNewNameConfirm" runat="server" Width="180px" BorderWidth="1px"></asp:textbox><FONT face="����" color="red">&nbsp;*</FONT>
						<asp:requiredfieldvalidator id="RequiredFieldValidator6" runat="server" Display="Dynamic" ControlToValidate="txbNewNameConfirm"
							ErrorMessage="RequiredFieldValidator">����Ϊ��</asp:requiredfieldvalidator>&nbsp;
						<asp:comparevalidator id="cpvNewName" runat="server" Display="Dynamic" ControlToValidate="txbNewNameConfirm"
							ErrorMessage="CompareValidator" ControlToCompare="txbNewName">�����µ�����Ҫһ��</asp:comparevalidator></td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="HEIGHT: 20px" bgColor="#eeeeee"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							�����޸������ַ��</FONT></td>
					<td style="HEIGHT: 20px" colSpan="2">&nbsp;
						<asp:textbox id="txMail" runat="server" Width="180px" BorderWidth="1px"></asp:textbox>&nbsp;
						<span class="style5">*
							<asp:requiredfieldvalidator id="rfvMail" runat="server" Display="Dynamic" ControlToValidate="txMail" ErrorMessage="����Ϊ��"></asp:requiredfieldvalidator>&nbsp;
							<asp:regularexpressionvalidator id="RegularExpressionValidator3" runat="server" Display="Dynamic" ControlToValidate="txMail"
								ErrorMessage="�ʼ���ʽ����ȷ" ValidationExpression="^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)(,(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*))*$">�ʼ���ʽ����ȷ</asp:regularexpressionvalidator></span></td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="HEIGHT: 21px" bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						�ٴ����������ַ��</td>
					<td style="HEIGHT: 21px" colSpan="2">&nbsp;
						<asp:textbox id="txMailConfirm" runat="server" Width="180px" BorderWidth="1px"></asp:textbox>&nbsp;
						<span class="style5">*
							<asp:requiredfieldvalidator id="rfvMail_confirm" runat="server" Display="Dynamic" ControlToValidate="txMailConfirm"
								ErrorMessage="����Ϊ��"></asp:requiredfieldvalidator>&nbsp;
							<asp:comparevalidator id="CompareV_Mail" runat="server" Display="Dynamic" ControlToValidate="txMailConfirm"
								ErrorMessage="�����ʼ�����Ҫһ��" ControlToCompare="txMail">�����ʼ�����Ҫһ��</asp:comparevalidator>&nbsp;
							<asp:regularexpressionvalidator id="RegularExpressionValidator4" runat="server" Display="Dynamic" ControlToValidate="txMailConfirm"
								ErrorMessage="�ʼ���ʽ����ȷ" ValidationExpression="^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)(,(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*))*$">�ʼ���ʽ����ȷ</asp:regularexpressionvalidator></span></td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="HEIGHT: 23px" bgColor="#eeeeee">&nbsp;<FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; 
							�ϴ��û��ʻ����Ͻ�ͼ��</FONT></td>
					<td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="����"> </FONT><INPUT id="File1" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="File1"
							runat="server">&nbsp;<SPAN class="style5">*
							<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" Display="Dynamic" ControlToValidate="File1"
								ErrorMessage="���ϴ�ͼƬ"></asp:requiredfieldvalidator>&nbsp; </SPAN>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; �޸�����ԭ��</FONT></td>
					<td colSpan="2"><FONT face="����">&nbsp;</FONT>
						<asp:textbox id="txReason" runat="server" Width="338px" BorderWidth="1px" Height="69px"></asp:textbox>&nbsp;<span class="style5">*
							<asp:requiredfieldvalidator id="rfvReason" runat="server" Display="Dynamic" ControlToValidate="txReason" ErrorMessage="����Ϊ��"></asp:requiredfieldvalidator></span></td>
				</tr>
			</table>
			<P align="center"><FONT face="����">&nbsp;&nbsp;&nbsp; </FONT>
				<asp:button id="Button_Update" runat="server" Width="130px" Height="30px" BorderStyle="Groove"
					Text="�������뵥" onclick="Button_Update_Click"></asp:button><FONT face="����">&nbsp; </FONT>
			</P>
		</form>
		<DIV align="center"><FONT face="����"></FONT>&nbsp;</DIV>
	</body>
</HTML>
