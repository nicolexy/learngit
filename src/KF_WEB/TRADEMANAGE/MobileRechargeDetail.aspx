<%@ Page language="c#" Codebehind="MobileRechargeDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MobileRechargeDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MobileRechargeDetail</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="VBScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form style="FONT-FAMILY: ����" id="Form1" method="post" runat="server">
			<table style="POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="800">
				<TBODY>
					<tr style="BACKGROUND-COLOR: #e4e5f7">
						<td colSpan="4"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;�û��ֻ���ֵ��¼����</FONT>
						</td>
					</tr>
					<tr>
						<td>����</td>
						<td>����</td>
						<td>ʵ�ʳ�ֵ��Ԫ��</td>
						<td>��ֵ����Ԫ��</td>
					</tr>
					<tr>
						<td><asp:Label Runat="server" ID="lbl_pannel"></asp:Label></td>
						<td><asp:Label Runat="server" ID="lbl_order"></asp:Label></td>
						<td><asp:Label Runat="server" ID="lbl_realPrice"></asp:Label></td>
						<td><asp:Label Runat="server" ID="lbl_denomination"></asp:Label></td>
					</tr>
					<tr>
						<td>������</td>
						<td>����ǰʱ��</td>
						<td>�̻�����ʱ��</td>
						<td>����޸�ʱ��</td>
					</tr>
					<tr>
						<td><asp:Label Runat="server" ID="lbl_provider"></asp:Label></td>
						<td><asp:Label Runat="server" ID="lbl_prepay"></asp:Label></td>
						<td><asp:Label Runat="server" ID="lbl_return"></asp:Label></td>
						<td><asp:Label Runat="server" ID="lbl_modify"></asp:Label></td>
					</tr>
					<tr>
					<td colspan="4">
					<a href = "MobileRechargeQuery.aspx">����</a>
					</td>
					</tr>
				</TBODY>
			</table>
		</form>
	</body>
</HTML>
