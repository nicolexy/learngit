<%@ Page language="c#" Codebehind="login.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.login" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>login</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); BODY { BACKGROUND-IMAGE: url(IMAGES/Page/bg01.gif) }
		</style>
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
	</HEAD>
	<body>
        <h4 style="text-align:center;">加载中……</h4>
		<form id="Form1" method="post" runat="server" style="display:none;">
			<DIV align="center"><STRONG><FONT face="黑体" size="5"></FONT></STRONG>&nbsp;</DIV>
			<br>
			<br>
			<br>
			<DIV align="center"><STRONG><FONT face="黑体" size="5"><asp:label id="Label3_Info" runat="server">Label</asp:label></FONT></STRONG></DIV>
			<p><FONT face="宋体"></FONT>&nbsp;</p>
			<table width="51%" height="101" border="0" align="center" cellpadding="1" cellspacing="0">
				<tr>
					<td style="WIDTH: 190px; HEIGHT: 39px">
						<P align="center">&nbsp;<FONT face="宋体">用户名：</FONT></P>
					</td>
					<td style="HEIGHT: 39px">
						<asp:textbox id="TextBox_uid" runat="server" Width="152px"></asp:textbox></td>
				</tr>
				<tr>
					<td style="WIDTH: 190px; HEIGHT: 33px">
						<P align="center">&nbsp;<FONT face="宋体">密&nbsp; 码：</FONT></P>
					</td>
					<td style="HEIGHT: 33px"><asp:textbox id="TextBox2_pwd" runat="server" TextMode="Password" Width="152px" Height="27px"></asp:textbox></td>
				</tr>
				<tr>
					<td colspan="2" style="HEIGHT: 60px">
						<P align="center">&nbsp;<asp:button id="Button1" runat="server" Width="56px" Text="登 陆 " onclick="Button1_Click"></asp:button>
							<asp:button id="Button2" runat="server" Width="57px" Text="取 消" onclick="Button2_Click"></asp:button></P>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
