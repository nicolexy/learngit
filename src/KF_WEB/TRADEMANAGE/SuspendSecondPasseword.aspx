<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="SuspendSecondPasseword.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SuspendSecondPasseword" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SuspendSecondPasseword</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="Z-INDEX: 101; POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1"
				cellSpacing="1" cellPadding="1" width="85%">
				<TR>
					<TD bgColor="#e4e5f7"><FONT color="red" face="宋体"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
							&nbsp;&nbsp;二次登录密码撤销</FONT></TD>
					<TD bgColor="#e4e5f7" align="right"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="center"><asp:label id="Label5" runat="server">QQ号</asp:label><asp:textbox id="txtQQ" Runat="server"></asp:textbox></TD>
					<td><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button></td>
				</TR>
			</TABLE>
			<TABLE style="Z-INDEX: 101; POSITION: absolute; TOP: 15%; LEFT: 5%" id="Table2" border="1"
				cellSpacing="1" cellPadding="1" width="85%" runat="server">
				<TR>
					<TD align="center"><asp:label id="lblQQ" runat="server"></asp:label><asp:label id="lblResult" runat="server"></asp:label></TD>
					<td><asp:button id="btnSuspend" runat="server" Width="80px" Text="撤销二次密码" Visible="False" onclick="btnSuspend_Click"></asp:button></td>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
