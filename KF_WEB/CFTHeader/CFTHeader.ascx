<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CFTHeader.ascx.cs" Inherits="TENCENT.OSS.C2C.Finance.Portal.CFTHeader.CFTHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<style type="text/css">
.cftheadername { FONT-WEIGHT: bold; FONT-SIZE: 14pt; FILTER: shadow(color=#CCCCCC,direction=120,enabled=1,strength=5) glow(color=#FFFFFF,enabled=1,strength=1); COLOR: navy; TEXT-ALIGN: left }
.cftheadertitle { FONT-WEIGHT: bold; FONT-SIZE: 9pt; FILTER: shadow(color=#FFFFFF,direction=120,enabled=1,strength=2) glow(color=#FFFFFF,enabled=1,strength=1); COLOR: #006699; TEXT-DECORATION: none }
.cftheaderlink { FONT-SIZE: 9pt; FILTER: shadow(color=#FFFFFF,direction=120,enabled=1,strength=1) glow(color=#FFFFFF,enabled=1,strength=1); COLOR: navy; TEXT-DECORATION: none }
.cftheaderdiv { FONT-SIZE: 9pt; COLOR: #dddddd; TEXT-DECORATION: none }
.cftheadertext { FONT-SIZE: 9pt; COLOR: #000000; TEXT-DECORATION: none }
</style>
<table width="100%" height="50" border="0" cellpadding="0" cellspacing="0" background="bg01.gif"
	id="cftheader" runat="server">
	<tr>
		<td width="125">
			<asp:HyperLink id="LinkPortal" runat="server" ImageUrl="logo.gif" Target="_blank" NavigateUrl="http://portal.cf.com"></asp:HyperLink><a href="http://edwardzheng-pc/test"></a></td>
		<td width="250" class="cftheadername" valign="middle">&nbsp;
			<asp:Label id="LabelName" runat="server"></asp:Label>
		</td>
		<td width="35" valign="middle">
			<asp:Label id="LabelVersion" runat="server" ForeColor="Gray" Font-Size="9pt" Font-Bold="True"
				Font-Names="Tahoma,宋体"></asp:Label></td>
		<td align="right"><table width="100%" border="0" cellspacing="5" cellpadding="0">
				<tr>
					<td align="right" class="cftheadertext">欢迎您，
						<asp:Label id="LabelUser" runat="server" ForeColor="Red"></asp:Label><font color="#ff0000">&nbsp;
						</font>&nbsp;
						<asp:LinkButton id="LinkLogout" runat="server" ForeColor="Red" onclick="LinkLogout_Click">退出系统</asp:LinkButton>&nbsp;</td>
				</tr>
				<tr>
					<td align="right" class="cftheadertext"><table height="20" border="0" cellpadding="0" cellspacing="2" bordercolordark="#999999"
							bordercolorlight="white">
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
