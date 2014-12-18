<%@ Page language="c#" Codebehind="KFDiaryManagePage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.KFDiaryManagePage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>KFDiaryManagePage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:Button id="btn_StartGrapDiary" Text="开启捕捉日志" runat="server" onclick="btn_StartGrapDiary_Click" />
			<asp:Button ID="btn_SendGrapedDiary" Text="发送捕捉日志" runat="server" onclick="btn_SendGrapedDiary_Click" />
			<asp:Button ID="btn_StopGrapDiary" Text="停止捕捉日志" runat="server" onclick="btn_StopGrapDiary_Click" />
		</form>
	</body>
</HTML>
