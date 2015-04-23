<%@ Page language="c#" Codebehind="QueryAuthenStateInfoPageOld.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryAuthenStateInfoPageOld" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryAuthenStateInfoPageOld</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table id="table1" border="1" cellSpacing="1" cellPadding="1" width="1100" runat="server">
				<tr>
					<td width="50%"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><label class="style3">实名认证状态查询</label></td>
					<td><label class="style3">操作员ID：</label><asp:label id="lb_operatorID" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td colSpan="2"><label style="WIDTH:90px;HEIGHT:20px;FONT-SIZE:14px">&nbsp;&nbsp;证件类型：</label>
						<asp:DropDownList Runat="server" ID="ddl_creType">
							<asp:ListItem Value="身份证" Selected="True"></asp:ListItem>
						</asp:DropDownList>
						<label style="WIDTH:70px;HEIGHT:20px;FONT-SIZE:14px">&nbsp;&nbsp;证件号：</label>
						<asp:textbox id="tbx_creid" Width="250px" Runat="server"></asp:textbox>
						<asp:button style="POSITION: relative; LEFT: 20px" id="btn_submit_acc" Runat="server" Text="提交查询内容" onclick="btn_submit_acc_Click"></asp:button>
						<input style="POSITION: relative; LEFT: 40px" id="btn_clear1" onclick="" value="重置" type="reset">
					</td>
				</tr>
			</table>
			<hr>
			<br>
			<br>
			<div runat="server" id="div_detail">
				<span><label style="FONT-SIZE:15px">证件号：</label><asp:Label Runat="server" ID="lb_queryAcc" Font-Size="15px">证件号</asp:Label></span>
				<table id="table2" border="2" cellSpacing="1" cellPadding="1" width="1100" runat="server">
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">账户ID：</label></td>
						<td><asp:Label Runat="server" ID="lb_c1" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">身份证证件状态：</label></td>
						<td><asp:Label Runat="server" ID="lb_c2" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">银行卡认证状态：</label></td>
						<td><asp:Label Runat="server" ID="lb_c3" Font-Size="14"></asp:Label></td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
