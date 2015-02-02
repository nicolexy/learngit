<%@ Page language="c#" Codebehind="QueryAuthenInfoPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryAuthenInfoPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryAuthenInfoPage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
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
					<td width="50%"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><label class="style3">ʵ����֤״̬��ѯ</label></td>
					<td><label class="style3">����ԱID��</label><asp:label id="lb_operatorID" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td colSpan="2"><asp:label id="Label1" runat="server" Width="80" Font-Size="15px">�ʺţ�</asp:label><asp:textbox id="tbx_acc" Width="250px" Runat="server"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td colSpan="2">
						<label style="WIDTH:75px">�������ͣ�</label>
						<asp:DropDownList Runat="server" ID="ddl_bankType">
							<asp:ListItem value="��������" Selected="True"></asp:ListItem>
							<asp:ListItem value="ũҵ����"></asp:ListItem>
							<asp:ListItem value="��������"></asp:ListItem>
							<asp:ListItem value="��������"></asp:ListItem>
							<asp:ListItem value="�й�����"></asp:ListItem>
							<asp:ListItem value="��ͨ����"></asp:ListItem>
							<asp:ListItem value="�ַ�����"></asp:ListItem>
							<asp:ListItem value="��������"></asp:ListItem>
							<asp:ListItem value="�չ����"></asp:ListItem>
							<asp:ListItem value="ƽ������"></asp:ListItem>
							<asp:ListItem value="��������"></asp:ListItem>
							<asp:ListItem value="�㷢����"></asp:ListItem>
						</asp:DropDownList>
						<asp:label id="Label2" runat="server" Width="80" Font-Size="15px"> ���п��ţ�</asp:label><asp:textbox id="tbx_bacc" Width="250px" Runat="server"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td align="center" colspan="2">
						<span style="MARGIN:0px 50px 0px 0px">
							<asp:Button Runat="server" ID="btn_query" Text="�� ѯ" Width="80" onclick="btn_query_Click"></asp:Button></span>
						<input type="reset" value="�� ��" style="WIDTH:80px">
					</td>
				</tr>
			</table>
			<hr>
			<br>
			<br>
			<div runat="server" id="div_detail">
				<span>
					<asp:Label id="lb_userStatue" runat="server" style="DISPLAY:none;FONT-SIZE:15px">������ĸ���</asp:Label></span><br>
				<span><label style="FONT-SIZE:15px">�ʺţ�</label><asp:Label Runat="server" ID="lb_queryAcc" Font-Size="15px">�ʺ�</asp:Label></span>
				<br>
				<span><label style="FONT-SIZE:15px">ʵ����֤����Ϣ��</label></span>
				<table id="table2" border="2" cellSpacing="1" cellPadding="1" width="1100" runat="server">
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">��֤��ʽ��</label></td>
						<td><asp:Label Runat="server" ID="lb_c1" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">��֤״̬(���д��״̬)��</label></td>
						<td><asp:Label Runat="server" ID="lb_c2" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">�����֤״̬��</label></td>
						<td><asp:Label Runat="server" ID="Label3" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">֤�����ͣ�</label></td>
						<td><asp:Label Runat="server" ID="lb_c3" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">֤���ţ�</label></td>
						<td><asp:Label Runat="server" ID="lb_c4" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">�������ͣ�</label></td>
						<td><asp:Label Runat="server" ID="lb_c5" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">���п��ţ�</label></td>
						<td><asp:Label Runat="server" ID="lb_c6" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">��һ��ͨ����֤���ʺţ�</label></td>
						<td><asp:Label Runat="server" ID="lb_c7" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">����ȨID��</label></td>
						<td><asp:Label Runat="server" ID="Label4" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">���ȷ�ϴ�����</label></td>
						<td><asp:Label Runat="server" ID="lb_c10" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">֤���޸Ĵ�����</label></td>
						<td><asp:Label Runat="server" ID="lb_c11" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">���п��޸Ĵ�����</label></td>
						<td><asp:Label Runat="server" ID="lb_c12" Font-Size="14"></asp:Label></td>
					</tr>
				</table>
				<br>
				<span><label style="FONT-SIZE:15px">ͨ��ʵ����֤��Ϣ��</label></span>
				<table id="Table3" border="2" cellSpacing="1" cellPadding="1" width="1100" runat="server">
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">��֤��ʽ��</label></td>
						<td><asp:Label Runat="server" ID="lb_c15" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">֤�����ͣ�</label></td>
						<td><asp:Label Runat="server" ID="lb_c16" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">���֤���룺</label></td>
						<td><asp:Label Runat="server" ID="lb_c17" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">��֤ͨ��ʱ�䣺</label></td>
						<td><asp:Label Runat="server" ID="lb_c18" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">����޸�ʱ�䣺</label></td>
						<td><asp:Label Runat="server" ID="lb_c19" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">֤���Ŷ�Ӧ��������</label></td>
						<td><asp:Label Runat="server" ID="lb_c20" Font-Size="14"></asp:Label></td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
