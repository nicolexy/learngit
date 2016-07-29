<%@ Page language="c#" Codebehind="RecoverQQ.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.RecoverQQ" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>UserAccountQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style5 { COLOR: #ff0000 }
		</style>
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT>
			<br>
			<table height="108" cellSpacing="1" cellPadding="0" width="860" align="center" bgColor="#666666"
				border="0">
				<tr background="../IMAGES/Page/bg_bl.gif">
					<td style="HEIGHT: 21px" vAlign="middle" colSpan="4" height="21">
						<table height="25" cellSpacing="0" cellPadding="1" width="859" border="0">
							<tr>
								<td width="80%" background="../IMAGES/Page/bg_bl.gif"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										财付通帐号恢复</font>
									<div align="right"><FONT face="宋体"></FONT></div>
								</td>
								<td width="20%" background="../IMAGES/Page/bg_bl.gif">操作员代码: <span class="style5">
										<asp:label id="Label1" runat="server">Label</asp:label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="WIDTH: 204px; HEIGHT: 25px" width="204" bgColor="#eeeeee">
						<div align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 输入帐户号码：</div>
					</td>
					<td><FONT face="宋体" color="#ff0000">&nbsp;</FONT>
						<asp:textbox id="TX_QQID" runat="server" BorderWidth="1px" Width="180px"></asp:textbox><FONT face="宋体" color="red">&nbsp;*
							<asp:requiredfieldvalidator id="rfvqq" runat="server" Width="98px" ErrorMessage="不能为空" ControlToValidate="TX_QQID"
								Display="Dynamic">请输入帐户号码</asp:requiredfieldvalidator>&nbsp;&nbsp;</FONT>
					</td>
					<td>
						<P align="center"><FONT face="宋体">&nbsp;&nbsp;</P>
						</FONT></td>
				</tr>
				<TR bgColor="#ffffff">
					<TD style="HEIGHT: 21px" bgColor="#eeeeee">
						<div align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 再次输入帐户号码：
						</div>
					</TD>
					<td style="HEIGHT: 21px" colSpan="2">&nbsp;
						<asp:textbox id="TX_QQID_Confirm" runat="server" BorderWidth="1px" Width="180px"></asp:textbox>&nbsp;
						<span class="style5">*
							<asp:requiredfieldvalidator id="rfvqq_confirm" runat="server" ErrorMessage="不能为空" ControlToValidate="TX_QQID_Confirm"
								Display="Dynamic"></asp:requiredfieldvalidator>&nbsp;
							<asp:comparevalidator id="CompareV_QQ" runat="server" ErrorMessage="两次QQ号码输入要一致" ControlToValidate="TX_QQID_Confirm"
								Display="Dynamic" ControlToCompare="TX_QQID">两次QQ号码输入要一致</asp:comparevalidator>&nbsp;</span></td>
				</TR>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 恢复到的内部ID：</FONT></td>
					<td colSpan="2">&nbsp;
						<asp:textbox id="txQQUid" runat="server" BorderWidth="1px" Width="180px"></asp:textbox>&nbsp;
						<span class="style5">*
							<asp:requiredfieldvalidator id="rfvMail" runat="server" ErrorMessage="不能为空" ControlToValidate="txQQUid" Display="Dynamic"></asp:requiredfieldvalidator>&nbsp;</span></td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="WIDTH: 250px; HEIGHT: 25px" width="250" bgColor="#eeeeee">
						<div align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							设置登陆密码(email或手机帐号)：</div>
					</td>
					<td><FONT face="宋体" color="#ff0000">&nbsp;</FONT>
						<asp:textbox id="TX_PSW" runat="server" BorderWidth="1px" Width="180px"></asp:textbox><FONT face="宋体" color="red">&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
					</td>
					<td>
						<P align="center"><FONT face="宋体">&nbsp;&nbsp;</P>
						</FONT></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 恢复帐号原因：</FONT></td>
					<td colSpan="2"><FONT face="宋体">&nbsp;</FONT>
						<asp:textbox id="txReason" runat="server" BorderWidth="1px" Width="338px" Height="69px"></asp:textbox>&nbsp;<span class="style5">*
							<asp:requiredfieldvalidator id="rfvReason" runat="server" ErrorMessage="不能为空" ControlToValidate="txReason" Display="Dynamic"></asp:requiredfieldvalidator></span></td>
				</tr>
			</table>
			<P align="center"><FONT face="宋体">&nbsp;&nbsp;&nbsp; </FONT>
				<asp:button id="Button_Update" runat="server" Width="133px" Text="生成申请单" Height="30px" BorderStyle="Groove"></asp:button><FONT face="宋体">&nbsp;
				</FONT>
			</P>
		</form>
		<DIV align="center"><FONT face="宋体"></FONT>&nbsp;</DIV>
	</body>
</HTML>
