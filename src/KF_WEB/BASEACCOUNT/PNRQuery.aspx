<%@ Page language="c#" Codebehind="PNRQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.PNRQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PNRQuery</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css );
UNKNOWN {
	COLOR: #000000
}
.style3 {
	COLOR: #ff0000
}
BODY {
	BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif)
}
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="820">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;ǩԼ��ϵ��ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label7" runat="server">�Ƹ�ͨ�ʺ�</asp:label></TD>
					<TD><asp:textbox id="txtQQ" runat="server"></asp:textbox></TD>
					<TD colSpan="2" align="center"><asp:button id="Button2" runat="server" Width="80px" Text="�� ѯ" onclick="Button2_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<table id="Table1" border="0" cellSpacing="0" cellPadding="0" width="820" runat="server">
				<TR>
					<TD vAlign="top" align="center"><asp:checkboxlist id="cblSpid" Runat="server">
							<asp:ListItem Value="*">����</asp:ListItem>
							<asp:ListItem Value="1205733301">�й����ʺ��չɷ����޹�˾</asp:ListItem>
							<asp:ListItem Value="1201187901">�й��Ϸ����չɷ����޹�˾</asp:ListItem>
							<asp:ListItem Value="1204311001">���Ϻ��չɷ����޹�˾(B2B)</asp:ListItem>
							<asp:ListItem Value="1203652401">���Ϻ��չɷ����޹�˾(B2C)</asp:ListItem>
							<asp:ListItem Value="1204760501">�й��������չɷ����޹�˾</asp:ListItem>
							<asp:ListItem Value="1204996901">�Ĵ����չɷ����޹�˾(B2B)</asp:ListItem>
							<asp:ListItem Value="1204997001">�Ĵ����չɷ����޹�˾(B2C)</asp:ListItem>
							<asp:ListItem Value="1203292301">���ں����������ι�˾</asp:ListItem>
							<asp:ListItem Value="1203998801">ɽ�����չɷ����޹�˾</asp:ListItem>
							<asp:ListItem Value="1202405701">���ﺽ�����޹�˾</asp:ListItem>
							<asp:ListItem Value="1203891701">���ź������޹�˾</asp:ListItem>
							<asp:ListItem Value="1204986801">���������������ι�˾</asp:ListItem>
							<asp:ListItem Value="1204706901">��¹�������޹�˾</asp:ListItem>
						</asp:checkboxlist></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
