<%@ Page language="c#" Codebehind="ChildrenOrderFromQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ChildrenOrderFromQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ChildrenOrderFromQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css );
.style2 {
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
			<table cellSpacing="1" cellPadding="1" width="900" align="center" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="4"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;���ʻ�������ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 83px" align="right"><asp:label id="Label2" runat="server">������</asp:label></TD>
					<td style="WIDTH: 350px" align="left"><asp:textbox id="tbFlistid" runat="server" Width="320px"></asp:textbox></td>
					<TD style="WIDTH: 83px" align="right"><asp:label id="Label3" runat="server">�ʻ�����</asp:label></TD>
					<td style="WIDTH: 350px" align="left"><asp:dropdownlist id="ddlFcurtype" runat="server" Width="150px">
							<asp:ListItem Value="1">���ʻ�</asp:ListItem>
							<asp:ListItem Value="2">����</asp:ListItem> 
							<asp:ListItem Value="80">��Ϸ</asp:ListItem>
							<asp:ListItem Value="81">��������</asp:ListItem>
							<asp:ListItem Value="82">ֱͨ��</asp:ListItem>
						</asp:dropdownlist></td>
				</TR>
				<tr>
					<TD align="center" colSpan="4"><asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button></TD>
				</tr>
				<TR>
					<td align="right"><asp:label id="Label24" Runat="server">����״̬</asp:label></td>
					<td align="left"><asp:label id="lblFstate" Runat="server"></asp:label></td>
					<td align="right"><asp:label id="Label19" Runat="server">����޸�ʱ��</asp:label></td>
					<td align="left"><asp:label id="lblFmodify_time" Runat="server"></asp:label></td>
				</TR>
				<tr>
					<td align="right"><asp:label id="Label4" Runat="server">����˵��</asp:label></td>
					<td align="left" colSpan="3"><asp:textbox id="tbFmemo" Width="800px" Runat="server" TextMode="MultiLine"></asp:textbox></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
