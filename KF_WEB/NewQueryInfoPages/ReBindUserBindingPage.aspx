<%@ Page language="c#" Codebehind="ReBindUserBindingPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.ReBindUserBindingPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ReBindUserBindingPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%">
				<tr>
					<td><label>重新绑定基金易帐号和证件号码</label></td>
					<asp:Label Runat="server" ID="lb_operatorID" CssClass=""></asp:Label>
				</tr>
				<tr>
					<td colspan="3"><label>查询条件</label></td>
					<td>
						<label>证件号码：</label>
						<asp:TextBox Runat="server" ID="tbx_cerNum"></asp:TextBox>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
