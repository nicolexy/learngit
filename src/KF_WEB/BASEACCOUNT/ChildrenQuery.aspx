<%@ Page language="c#" Codebehind="ChildrenQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ChildrenQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ChildrenQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="1" cellPadding="1" width="900" align="center" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="6"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;���ʺŲ�ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 83px" align="right"><asp:label id="Label5" runat="server">�ʺ�</asp:label></TD>
					<td style="WIDTH: 300px" align="left"><asp:textbox id="tbQQ" runat="server"></asp:textbox></td>
					<TD style="WIDTH: 200px" align="left"><asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button></TD>
					<td style="WIDTH: 200px"></td>
					<td style="WIDTH: 200px"></td>
					<td style="WIDTH: 200px"></td>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="6">
						<table cellSpacing="1" cellPadding="1" width="900" align="center" border="1">
							<tr>
								<td><asp:label id="Label24" Runat="server">�ʺ�</asp:label></td>
								<td><asp:label id="Label4" Runat="server">�ʻ�����</asp:label></td>
								<td><asp:label id="Label19" Runat="server">�ʻ����</asp:label></td>
								<td><asp:label id="Label20" Runat="server">�ʻ�״̬</asp:label></td>
								<td><asp:label id="Label2" Runat="server">��ϸ</asp:label></td>
								<td><asp:label id="Label3" Runat="server">�ʻ�����</asp:label></td>
							</tr>
							<tr>
								<td><asp:label id="lblCommonQQ" Runat="server"></asp:label></td>
								<td><asp:label id="Label21" Runat="server">��ͨ�ʻ�</asp:label></td>
								<td><asp:label id="lblCommonMoney" Runat="server"></asp:label></td>
								<td><asp:label id="lblCommonType" Runat="server"></asp:label></td>
								<td></td>
								<td></td>
							</tr>
							<tr>
								<td><asp:label id="lblGameQQ" Runat="server"></asp:label></td>
								<td><asp:label id="Label25" Runat="server">��Ϸ�ʻ�</asp:label></td>
								<td><asp:label id="lblGameMoney" Runat="server"></asp:label></td>
								<td><asp:label id="lblGameType" Runat="server"></asp:label></td>
								<td><asp:button id="btnGame" Text="��ϸ" Runat="server" Enabled="False" onclick="btnGame_Click"></asp:button></td>
								<td><asp:button id="btnGameState" Width="60px" Runat="server" Enabled="False" onclick="btnGameState_Click"></asp:button></td>
							</tr>
							<tr>
								<td><asp:label id="lblFLQQ" Runat="server"></asp:label></td>
								<td><asp:label id="Label23" Runat="server">�����ʻ�</asp:label></td>
								<td><asp:label id="lblFLMoney" Runat="server"></asp:label></td>
								<td><asp:label id="lblFLType" Runat="server"></asp:label></td>
								<td><asp:button id="btnFL" Text="��ϸ" Runat="server" Enabled="False" onclick="btnFL_Click"></asp:button></td>
								<td><asp:button id="btnFLState" Width="60px" Runat="server" Enabled="False" onclick="btnFLState_Click"></asp:button></td>
							</tr>
							<tr>
								<td><asp:label id="lblZTCQQ" Runat="server"></asp:label></td>
								<td><asp:label id="Label28" Runat="server">ֱͨ���ʻ�</asp:label></td>
								<td><asp:label id="lblZTCMoney" Runat="server"></asp:label></td>
								<td><asp:label id="lblZTCType" Runat="server"></asp:label></td>
								<td><asp:button id="btnZTC" Text="��ϸ" Runat="server" Enabled="False" onclick="btnZTC_Click"></asp:button></td>
								<td><asp:button id="btnZTCState" Width="60px" Runat="server" Enabled="False" onclick="btnZTCState_Click"></asp:button></td>
							</tr>
						</table>
					</TD>
				</TR>
			</table>
			<asp:panel id="PanelInfo" Runat="server">
				<TABLE height="35" cellSpacing="0" cellPadding="0" width="900" align="center" border="0">
					<TR>
						<TD bgColor="#666666">
							<TABLE height="35" cellSpacing="1" cellPadding="0" width="900" align="center" border="0">
								<TR bgColor="#e4e5f7">
									<TD background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7" colSpan="3" height="20">&nbsp;�û��ʽ���ˮ</TD>
								</TR>
								<TR>
									<TD bgColor="#ffffff" colSpan="3" height="12"><IFRAME id=iframe0 
            name=iframe0 marginWidth=0 marginHeight=0 src="<%=iFramePath%>" 
            frameBorder=0 width="100%" scrolling=auto 
        height="250"></IFRAME>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</asp:panel></form>
	</body>
</HTML>
