<%@ Page language="c#" Codebehind="IconManagement.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.VIPAccount.IconManagement" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>IconManagement</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="VBScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server" style="FONT-FAMILY:����">
			<table style="Z-INDEX: 100; POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1"
				cellSpacing="1" cellPadding="1" width="400">
				<tr style="BACKGROUND-COLOR: #e4e5f7">
					<td colspan="3">
						<FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;ͼ�����</FONT>
					</td>
				</tr>
				<tr>
					<td width="50">
						<label style="PADDING-LEFT:10px">�˺�:</label>
					</td>
					<td>
						<asp:textbox id="tbx_acc" Width="200px" Runat="server"></asp:textbox>
					</td>
					<td>
						<asp:Button Text="��ѯ" Runat="server" ID="btn_query" Width="100px" onclick="btnSearch_Click"></asp:Button>
					</td>
				</tr>
                <tr>
					<td width="100" colspan="3">
						<label style="PADDING-LEFT:10px"></label>
					</td>
				</tr>
                <tr>
					<td width="100">
						<label style="PADDING-LEFT:10px">��Ա�ȼ�:</label>
					</td>
					<td colspan="2">
                        <asp:label id="tbmemberGrade" runat="server"></asp:label>
					</td>
				</tr>
                <tr>
					<td width="100">
						<label style="PADDING-LEFT:10px">ͼ��ȼ�:</label>
					</td>
					<td colspan="2">
                        <asp:label id="tbiconGrade" runat="server"></asp:label>
					</td>
				</tr>
				<tr>
					<td colspan="3" style="PADDING-LEFT:20px">
                      <asp:Button Text="ˢ��ͼ��" Runat="server" ID="Button_Flash" Width="100px" onclick="lit_Click"></asp:Button>
                      <asp:Button Text="Ϩ��ͼ��" Runat="server" ID="Button_extinguish" Width="100px" onclick="extinguish_Click"></asp:Button>
                    </td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
