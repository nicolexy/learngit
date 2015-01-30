<%@ Page language="c#" Codebehind="fetchName.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.fetchName" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>fetchName</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); BODY { BACKGROUND-IMAGE: none }
	.style2 { FONT-FAMILY: "黑体"; FONT-SIZE: 20px; FONT-WEIGHT: bold }
	.style3 { FONT-SIZE: 14px; FONT-WEIGHT: bold }
	.style4 { FONT-FAMILY: "黑体"; FONT-SIZE: 16px; FONT-WEIGHT: bold }
	.style5 { FONT-FAMILY: "黑体"; FONT-SIZE: 14px; FONT-WEIGHT: bold }
	.style6 { FONT-SIZE: 16px; FONT-WEIGHT: bold }
		</style>
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="1" width="75%" align="center" border="0">
				<tr>
					<td width="7%" height="64" rowSpan="2" style="HEIGHT: 64px">&nbsp;</td>
					<td width="44%" height="23">
						<p class="style4">NO.
							<asp:label id="lbFetchNo" runat="server"></asp:label></p>
					</td>
					<td width="44%">
						<div align="right"><strong><asp:hyperlink id="hyLKPrint" runat="server" NavigateUrl="fetchName.aspx" Target="_blank">生成打印版</asp:hyperlink><asp:linkbutton id="lkView" runat="server" Visible="False">[预览]</asp:linkbutton><asp:linkbutton id="lkPrint" runat="server" Visible="False" onclick="lkPrint_Click">[打印]</asp:linkbutton></strong></div>
					</td>
					<td width="5%" rowSpan="2" style="HEIGHT: 64px">&nbsp;</td>
				</tr>
				<tr>
					<td colSpan="2" height="41" style="HEIGHT: 41px">
						<div align="center">
							<p class="style2">用户修改姓名申请表</p>
						</div>
					</td>
				</tr>
				<tr>
					<td height="341">&nbsp;
					</td>
					<td vAlign="top" colSpan="2">
						<DIV align="center">
							<table height="384" cellSpacing="0" cellPadding="0" width="607" align="center" border="0">
								<tr>
									<td bgColor="#000000">
										<table height="900" cellSpacing="1" cellPadding="0" width="634" align="center" bgColor="#000000"
											border="0">
											<tr bgColor="#ffffff">
												<td style="HEIGHT: 26px">
													<div align="center"><strong><FONT size="2">用户原姓名</FONT></strong></div>
												</td>
												<td style="HEIGHT: 26px">
													<div align="center"><strong><asp:label id="lbOldName" runat="server" Width="100%" Height="13px">Label</asp:label></strong></div>
												</td>
												<td style="HEIGHT: 26px">
													<div align="center"><strong><FONT size="2">申请时间</FONT></strong></div>
												</td>
												<td style="HEIGHT: 26px">
													<div align="center"><strong><asp:label id="lbTime" runat="server">2005-09-14 10:17</asp:label></strong></div>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td style="HEIGHT: 24px" height="24">
													<div align="center"><strong><FONT size="2">修改后姓名</FONT></strong></div>
												</td>
												<td style="HEIGHT: 24px">
													<div align="center"><strong><asp:label id="lbNewName" runat="server" Width="100%" Height="13px">Label</asp:label></strong></div>
												</td>
												<td style="HEIGHT: 24px">
													<P align="center"><STRONG><FONT size="2">帐号</FONT></STRONG></P>
												</td>
												<td style="HEIGHT: 24px"><strong>
														<DIV align="center"><STRONG><asp:label id="lbQQ" runat="server">22000254</asp:label></STRONG></DIV>
													</strong>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td style="HEIGHT: 29px" height="29">
													<div align="center"><strong><FONT size="2">取回邮箱</FONT></strong></div>
												</td>
												<td style="HEIGHT: 29px" colSpan="3">
													<P align="left"><FONT face="宋体">&nbsp;</FONT><STRONG>&nbsp;
															<asp:label id="lbMail" runat="server" Width="100%">rayguo@tencent.com</asp:label></STRONG></P>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td style="HEIGHT: 31px" height="31">
													<div align="center"><strong>修改原因</strong></div>
												</td>
												<td style="HEIGHT: 31px" colSpan="3">&nbsp;<FONT face="宋体"> </FONT>
													<asp:label id="lbReason" runat="server" Width="100%">密码忘记了~ 没有办法！ 请帮忙修改！~</asp:label></td>
											</tr>
											<tr bgColor="#cccccc">
												<td colSpan="4" height="25">
													<div class="style5" align="left">用户个人信息及帐户信息</div>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td colSpan="4">
													<div align="center"><asp:image id="imageUInfo" runat="server" Width="850px" Height="250px"></asp:image></div>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td colSpan="4">
													<div align="center"><asp:image id="imgAccInfo" runat="server" Width="850px" Height="210px"></asp:image></div>
												</td>
											</tr>
											<tr bgColor="#ffffff" style="DISPLAY:none">
												<td bgColor="#cccccc" colSpan="4" height="25"><span class="style5">用户身份证及银行卡信息</span></td>
											</tr>
											<tr bgColor="#ffffff" style="DISPLAY:none">
												<td colSpan="2" height="19">
													<div align="center">原身份证复印件</div>
												</td>
												<td colSpan="2">
													<div align="center">新身份证复印件</div>
												</td>
											</tr>
											<tr bgColor="#ffffff" style="DISPLAY:none">
												<td colSpan="2" height="187">
													<div align="center"><asp:image id="imgOldCardID" runat="server" Width="300px" Height="190px"></asp:image></div>
												</td>
												<td colSpan="2">
													<div align="center"><FONT face="宋体"><asp:image id="imgNewCardID" runat="server" Width="300px" Height="190px"></asp:image></FONT></div>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td height="53">
													<div align="center">修改操作人</div>
												</td>
												<td height="53">
													<div class="style6" align="center"><asp:label id="lbHandleID" runat="server">zoezhang</asp:label></div>
												</td>
												<td height="53">
													<div align="center">审批人</div>
												</td>
												<td height="53">
													<div class="style3" align="center"><asp:label id="lbCheckUid" runat="server"></asp:label></div>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td style="HEIGHT: 29px" height="29">
													<div align="center">提请日期</div>
												</td>
												<td style="HEIGHT: 29px" height="29">
													<div class="style3" align="center"><asp:label id="lbCommitTime" runat="server">Label</asp:label></div>
												</td>
												<td style="HEIGHT: 29px" height="29">
													<div align="center">审批日期</div>
												</td>
												<td style="HEIGHT: 29px" height="29">
													<div class="style3" align="center"><asp:label id="lbCheckDate" runat="server"></asp:label></div>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td height="49">
													<p align="center">审批意见</p>
												</td>
												<td colSpan="3" height="49"><FONT face="宋体">
														<P align="left">&nbsp;<asp:label id="lbCheckInfo" runat="server"></asp:label></P>
													</FONT>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</DIV>
					</td>
					<td>&nbsp;</td>
				</tr>
			</table>
			<P align="center"><FONT face="宋体">
					<% if (Request.QueryString["print"] == null) {%>
					<% if (Request.QueryString["posi"] == null) { %>
					<asp:button id="btCommit" runat="server" Width="190px" Height="37px" Text="确认无误，提交审批！" BorderStyle="Groove" onclick="btCommit_Click"></asp:button></FONT></P>
			<% } else {%>
			<DIV align="center"><A href="javascript:history.go(-1)"><IMG src="../Images/Page/back.gif"></A></DIV>
			<%  }%>
			<%}%>
		</form>
	</body>
</HTML>
