<%@ Page language="c#" Codebehind="QueryQQ.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.QueryQQ" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryQQ</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
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
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT>
			<br>
			<table cellSpacing="1" cellPadding="0" width="95%" align="center" bgColor="#666666" border="0">
				<tr bgColor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
					<td style="HEIGHT: 16px" vAlign="middle" colSpan="2" height="16">
						<table height="8" cellSpacing="0" cellPadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
							border="0">
							<tr background="../IMAGES/Page/bg_bl.gif">
								<td style="HEIGHT: 24px" width="80%" height="24"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;QQ号码 
										&nbsp; </font><span class="style6"><FONT color="red">查询</FONT></span>
								</td>
								<td style="HEIGHT: 24px" width="20%"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td><FONT face="宋体">
							<TABLE id="Table1" height="45" cellSpacing="0" cellPadding="1" width="100%" align="center"
								border="0">
								<TBODY>
									<TR bgColor="lightgrey">
										<TD style="HEIGHT: 24px"><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT></TD>
										<TD style="HEIGHT: 24px" colSpan="2"><FONT face="宋体">&nbsp;&nbsp;</FONT></TD>
										<TD style="HEIGHT: 24px"><FONT face="宋体">&nbsp;&nbsp; </FONT>
										</TD>
									</TR>
									<TR bgColor="#eeeeee">
										<TD><FONT face="宋体"></FONT></TD>
										<TD colSpan="2"><FONT face="宋体">&nbsp;&nbsp;查询：&nbsp;<asp:textbox id="txbPara" runat="server" Width="122px"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												条&nbsp; 件：
												<asp:dropdownlist id="ddlCondition" runat="server" Width="131px">
													<asp:ListItem Value="FInternalID" Selected="True">按内部ID</asp:ListItem>
													<asp:ListItem Value="FCardID">按银行卡号</asp:ListItem>
													<asp:ListItem Value="FID">按身份证号</asp:ListItem>
													<asp:ListItem Value="FName">按姓名</asp:ListItem>
												</asp:dropdownlist></FONT></TD>
										<TD><FONT face="宋体"></FONT></TD>
									</TR>
								</TBODY>
							</TABLE>
						</FONT>
					</td>
					<td width="15%">
						<DIV align="center">&nbsp;
							<asp:button id="btQuery" runat="server" Width="108px" BorderStyle="Groove" Text="查 询" Height="27px" onclick="btQuery_Click"></asp:button></DIV>
					</td>
				</tr>
			</table>
			<br>
			<TABLE id="Table2" height="0" cellSpacing="0" cellPadding="0" width="95%" align="center"
				border="0">
				<TR>
					<TD bgColor="#666666">
						<TABLE id="Table2" cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
							<TR bgColor="#e4e5f7">
								<TD style="HEIGHT: 21px" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7" colSpan="2"
									height="21">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td style="WIDTH: 754px" width="754"><FONT color="#ff0000"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp; 
													QQ号码 明细</FONT>
											</td>
											<td style="WIDTH: 125px" width="125">
												<div align="center">自定义分页大小：</div>
											</td>
											<td width="15%">
												<P align="left">&nbsp;
													<asp:dropdownlist id="ddlPageSize" runat="server" Width="88px">
														<asp:ListItem Value="20">每页20条</asp:ListItem>
														<asp:ListItem Value="30">每页30条</asp:ListItem>
														<asp:ListItem Value="50">每页50条</asp:ListItem>
													</asp:dropdownlist></P>
											</td>
										</tr>
									</table>
								</TD>
							</TR>
							<TR>
								<TD bgColor="#ffffff" height="12"><FONT face="宋体">
										<DIV align="left"><asp:datagrid id="dgInfo" runat="server" Width="100%" AutoGenerateColumns="False">
												<AlternatingItemStyle BackColor="#EEEEEE"></AlternatingItemStyle>
												<HeaderStyle Font-Bold="True" BackColor="ActiveBorder"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="Fqqid" HeaderText="QQ号码"></asp:BoundColumn>
												</Columns>
											</asp:datagrid></DIV>
									</FONT>
									<webdiyer:aspnetpager id="AspNetPager1" runat="server" NumericButtonCount="10" PagingButtonSpacing="0"
										ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"
										ShowCustomInfoSection="left" AlwaysShow="True"></webdiyer:aspnetpager></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
		<DIV align="right"><FONT face="宋体"></FONT></DIV>
	</body>
</HTML>
