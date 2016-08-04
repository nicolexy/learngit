<%@ Page language="c#" Codebehind="UpdateMerchantCre.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.UpdateMerchantCre" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>UpdateMerchantCre</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
			</FONT>
			<br>
			<table height="108" cellSpacing="1" cellPadding="0" width="860" align="center" bgColor="#666666"
				border="0">
				<tr background="../IMAGES/Page/bg_bl.gif">
					<td style="HEIGHT: 16px" vAlign="middle" colSpan="4" height="16">
						<table height="25" cellSpacing="0" cellPadding="1" width="859" border="0">
							<tr>
								<td style="HEIGHT: 20px" width="80%" background="../IMAGES/Page/bg_bl.gif"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										修改商户身份证号</font>
								</td>
								<td style="HEIGHT: 20px" width="20%" background="../IMAGES/Page/bg_bl.gif">操作员代码: <span class="style5">
										<asp:label id="Label1" runat="server">Label</asp:label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="WIDTH: 204px; HEIGHT: 25px" width="204" bgColor="#eeeeee">
						<div align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 输入商户号：</div>
					</td>
					<td>&nbsp;
						<asp:textbox id="TX_SPID" runat="server" Width="180px" BorderWidth="1px"></asp:textbox><FONT face="宋体" color="red">&nbsp;*</FONT>
						<asp:requiredfieldvalidator id="rfvqq" runat="server" Width="94px" Display="Dynamic" ControlToValidate="TX_SPID"
							ErrorMessage="不能为空">请输入帐户号码</asp:requiredfieldvalidator>&nbsp;&nbsp;
					</td>
					<td>
						<P align="center"><FONT face="宋体">&nbsp;&nbsp;</FONT></P>
					</td>
				</tr>
				<TR bgColor="#ffffff">
					<TD style="HEIGHT: 18px" bgColor="#eeeeee"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							输入原证件号：</FONT></TD>
					<TD style="HEIGHT: 18px" colSpan="2">&nbsp;
						<asp:textbox id="txtOldCreid" runat="server" Width="180px" BorderWidth="1px"></asp:textbox><FONT face="宋体" color="red">&nbsp;*</FONT>
						<asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" Display="Dynamic" ControlToValidate="txtOldCreid"
							ErrorMessage="RequiredFieldValidator">不能为空</asp:requiredfieldvalidator></TD>
				</TR>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 输入新的证件号：</FONT></td>
					<td colSpan="2">&nbsp;
						<asp:textbox id="txbNewCreid" runat="server" Width="180px" BorderWidth="1px"></asp:textbox><FONT face="宋体" color="red">&nbsp;*</FONT>
						<asp:requiredfieldvalidator id="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="txbNewCreid"
							ErrorMessage="RequiredFieldValidator">不能为空</asp:requiredfieldvalidator></td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="HEIGHT: 23px" bgColor="#eeeeee">&nbsp;<FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp; 
							上传商户身份证截图：</FONT></td>
					<td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="宋体"> </FONT><INPUT id="File1" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="File1"
							runat="server">&nbsp;<SPAN class="style5">*
							<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" Display="Dynamic" ControlToValidate="File1"
								ErrorMessage="请上传图片"></asp:requiredfieldvalidator>&nbsp; </SPAN>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="HEIGHT: 23px" bgColor="#eeeeee">&nbsp;<FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp; 
							上传商户营业执照截图：</FONT></td>
					<td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="宋体"> </FONT><INPUT id="File2" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="File1"
							runat="server">&nbsp;<SPAN class="style5">*
							<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" Display="Dynamic" ControlToValidate="File1"
								ErrorMessage="请上传图片"></asp:requiredfieldvalidator>&nbsp; </SPAN>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 修改证件原因：</FONT></td>
					<td colSpan="2"><FONT face="宋体">&nbsp;</FONT>
						<asp:textbox id="txReason" runat="server" Width="338px" BorderWidth="1px" Height="69px"></asp:textbox>&nbsp;<span class="style5">*
							<asp:requiredfieldvalidator id="rfvReason" runat="server" Display="Dynamic" ControlToValidate="txReason" ErrorMessage="不能为空"></asp:requiredfieldvalidator></span></td>
				</tr>
			</table>
			<P align="center"><FONT face="宋体">&nbsp;&nbsp;&nbsp; </FONT>
				<asp:button id="Button_Update" runat="server" Width="130px" Height="30px" BorderStyle="Groove"
					Text="提交" onclick="Button_Update_Click"></asp:button><FONT face="宋体">&nbsp; </FONT>
			</P>
		</form>
		<DIV align="center"><FONT face="宋体"></FONT>&nbsp;</DIV>
	</body>
</HTML>
