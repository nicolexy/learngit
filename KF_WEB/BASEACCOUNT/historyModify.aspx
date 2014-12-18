<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="historyModify.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.historyModify" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>historyModify</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
					}
		</script>
		<script language="javascript">
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxEndDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<br>
			<table border="0" cellSpacing="1" cellPadding="0" width="95%" bgColor="#666666" align="center">
				<tr bgColor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
					<td style="HEIGHT: 16px" height="16" vAlign="middle" colSpan="2">
						<table border="0" cellSpacing="0" cellPadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
							height="8">
							<tr background="../IMAGES/Page/bg_bl.gif">
								<td style="HEIGHT: 24px" height="24" width="80%"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
										用户修改信息 </font><span class="style6"><FONT color="red">查询</FONT></span>
								</td>
								<td style="HEIGHT: 24px" width="20%"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td><FONT face="宋体">
							<TABLE id="Table1" border="0" cellSpacing="0" cellPadding="1" width="100%" align="center"
								height="45">
								<TBODY>
									<TR bgColor="lightgrey">
										<TD style="HEIGHT: 24px"><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT></TD>
										<TD style="HEIGHT: 24px" colSpan="2"><FONT face="宋体">&nbsp; 起始日期
												<asp:textbox id="TextBoxBeginDate" runat="server" Width="122px">2005-05-01</asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton>&nbsp;&nbsp;&nbsp;&nbsp; 
												结束日期
												<asp:textbox id="TextBoxEndDate" runat="server" Width="122px"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></FONT></TD>
										<TD style="HEIGHT: 24px"><FONT face="宋体">&nbsp;&nbsp; </FONT>
										</TD>
									</TR>
									<TR bgColor="#eeeeee">
										<TD><FONT face="宋体"></FONT></TD>
										<TD colSpan="2"><FONT face="宋体">&nbsp; 精确查询
												<asp:textbox id="txbCustom" runat="server" Width="122px"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												条&nbsp; 件：
												<asp:dropdownlist id="ddlCondition" runat="server" Width="131px" AutoPostBack="True" onselectedindexchanged="ddlCondition_SelectedIndexChanged">
													<asp:ListItem Value="fqqid" Selected="True">按用户帐号</asp:ListItem>
													<asp:ListItem Value="FbankNO">按银行卡号码</asp:ListItem>
													<asp:ListItem Value="FName">按修改前姓名</asp:ListItem>
													<asp:ListItem Value="FNameNew">按修改后姓名</asp:ListItem>
													<asp:ListItem Value="FfetchMail">按取回邮箱名</asp:ListItem>
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
			<TABLE id="Table1" border="0" cellSpacing="0" cellPadding="0" width="95%" align="center"
				height="0">
				<TR>
					<TD bgColor="#666666">
						<TABLE id="Table2" border="0" cellSpacing="1" cellPadding="0" width="100%" align="center">
							<TR bgColor="#e4e5f7">
								<TD style="HEIGHT: 21px" bgColor="#e4e5f7" height="21" background="../IMAGES/Page/bg_bl.gif"
									colSpan="2">
									<table border="0" cellSpacing="0" cellPadding="0" width="100%">
										<tr>
											<td style="WIDTH: 754px" width="754"><FONT color="#ff0000"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp; 
													历史信息修改明细</FONT>
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
												<HeaderStyle Font-Bold="True" Wrap="False" BackColor="ActiveBorder"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="Fqqid" HeaderText="用户帐号">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FName" HeaderText="修改前姓名">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FNameNew" HeaderText="修改后姓名">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FcleanMibao" HeaderText="清密保">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FfetchMail" HeaderText="取回邮件地址">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="Freason" HeaderText="修改原因">
														<HeaderStyle Wrap="False"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FcommitTime" HeaderText="提交时间">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
												</Columns>
											</asp:datagrid></DIV>
									</FONT>
									<webdiyer:aspnetpager id="AspNetPager1" runat="server" AlwaysShow="True" ShowCustomInfoSection="left"
										NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" HorizontalAlign="right" CssClass="mypager"
										ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="10"></webdiyer:aspnetpager></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			<FONT face="宋体"></FONT>
		</form>
	</body>
</HTML>
