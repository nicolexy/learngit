<%@ Page language="c#" Codebehind="logOnUser.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.C2C.KF.KF_Web.BaseAccount.logOnUser" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>logOnUser</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	.style3 { COLOR: #000000 }
	.style4 { COLOR: #ff0000 }
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
			<table cellSpacing="1" cellPadding="0" width="90%" align="center" bgColor="#666666" border="0">
				<tr bgColor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
					<td vAlign="middle" colSpan="2" height="20">
						<table height="90%" cellSpacing="0" cellPadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
							border="0">
							<tr background="../IMAGES/Page/bg_bl.gif">
								<td width="80%" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										用户销户</font>
									<div align="right"></div>
								</td>
								<td width="20%"><asp:Label ID="Label1" Runat="server"></asp:Label></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td>
						<div align="center"></div>
						<div align="left">
							<table height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<tr>
									<td width="19%">&nbsp;</td>
									<td width="78%"><FONT face="宋体">输入销户帐号:&nbsp;<asp:textbox id="TextBox1_QQID" runat="server" BorderStyle="Solid" BorderWidth="1px"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="RequiredFieldValidator"
												Width="117px" ControlToValidate="TextBox1_QQID">请输入帐号</asp:requiredfieldvalidator></FONT></td>
									<TD width="3%">&nbsp;</TD>
								</tr>
								<TR>
									<TD width="19%"></TD>
									<TD width="78%"><FONT face="宋体">再次确认帐号:
											<asp:textbox id="txbConfirmQ" runat="server" BorderStyle="Solid" BorderWidth="1px"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" Display="Dynamic" ErrorMessage="RequiredFieldValidator"
												Width="77px" ControlToValidate="txbConfirmQ">请输入帐号</asp:requiredfieldvalidator><asp:comparevalidator id="CompareValidator1" runat="server" ErrorMessage="CompareValidator" ControlToValidate="txbConfirmQ"
												ControlToCompare="TextBox1_QQID">两次输入号码不一致</asp:comparevalidator></FONT></TD>
									<TD width="3%"></TD>
								</TR>
								<TR>
									<TD width="19%"></TD>
									<TD width="78%"><FONT face="宋体">输入销户原因:
											<asp:textbox id="txtReason" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="286px"
												Height="40px" TextMode="MultiLine"></asp:textbox>&nbsp;
											<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" Display="Dynamic" ErrorMessage="RequiredFieldValidator"
												Width="63px" ControlToValidate="txtReason">请输入原因</asp:requiredfieldvalidator></FONT></TD>
									<TD width="3%"></TD>
								</TR>
                                <TR>
									<TD width="19%"></TD>
									<TD width="78%"><FONT face="宋体">系统自动销户是否通知用户:<asp:CheckBox ID="EmailCheckBox" runat="server" 
                                            oncheckedchanged="CheckBox1_CheckedChanged" />是</FONT></TD>
									<TD width="3%"></TD>
								</TR>
                                <TR>
									<TD width="19%"></TD>
									<TD width="78%"><FONT face="宋体">输入用户邮箱地址:
											<asp:textbox id="txtEmail" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="135px">
                                            </asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT></TD>
									<TD width="3%"></TD>
								</TR>
							</table>
						</div>
						<div align="left"></div>
						<div align="center"><FONT face="宋体"></FONT></div>
					</td>
					<td width="25%">
						<div align="center">&nbsp;
							<asp:button id="btLogOn" runat="server" Width="122px" Height="31px" Text="销户申请" onclick="btLogOn_Click"></asp:button></div>
					</td>
				</tr>
			</table>
			<P align="center">
				<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="90%" align="center" border="0"
					runat="server" Visible="true">
					<TR>
						<TD bgColor="#666666">
							<TABLE id="Table2" cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
								<TR bgColor="#e4e5f7">
									<TD background="../IMAGES/Page/bg_bl.gif" height="20"><FONT color="#ff0000"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;销户历史明细(最新10条)
										</FONT>
									</TD>
								</TR>
								<TR>
									<TD bgColor="#ffffff" height="12"><FONT face="宋体"></FONT><FONT face="宋体">
											<asp:datagrid id="dgInfo" runat="server" Width="100%" AutoGenerateColumns="False">
												<HeaderStyle BackColor="#EEEEEE"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="Fid" HeaderText="ID"></asp:BoundColumn>
													<asp:BoundColumn DataField="Fqqid" HeaderText="帐号"></asp:BoundColumn>
													<asp:BoundColumn DataField="Fquid" HeaderText="内部帐号"></asp:BoundColumn>
													<asp:BoundColumn DataField="Freason" HeaderText="销户原因"></asp:BoundColumn>
													<asp:BoundColumn DataField="handid" HeaderText="执行人"></asp:BoundColumn>
													<asp:BoundColumn DataField="handip" HeaderText="执行人IP"></asp:BoundColumn>
													<asp:BoundColumn DataField="FlastModifyTime" HeaderText="最后修改时间"></asp:BoundColumn>
												</Columns>
											</asp:datagrid></FONT></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</P>
			<P align="center"><asp:linkbutton id="lkHistoryQuery" runat="server" CausesValidation="False" onclick="lkHistoryQuery_Click">高级查询</asp:linkbutton></P>
			<P align="center">
				<TABLE id="TABLE3" cellSpacing="0" cellPadding="0" width="90%" align="center" border="0"
					runat="server" Visible="false">
					<TR>
						<TD bgColor="#666666">
							<TABLE style="HEIGHT: 119px" cellSpacing="1" cellPadding="0" width="100%" align="center"
								border="0">
								<TR bgColor="#e4e5f7">
									<TD background="../IMAGES/Page/bg_bl.gif" colSpan="6" height="20">
										<P align="left"><FONT color="#ff0000"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;销户查询&nbsp;&nbsp;</FONT></P>
									</TD>
								</TR>
								<TR>
									<TD style="WIDTH: 128px; HEIGHT: 33px" bgColor="#ffffff" height="33">
										<P align="center"><FONT face="宋体">开始日期</FONT></P>
									</TD>
									<TD style="WIDTH: 299px; HEIGHT: 33px" bgColor="#ffffff" height="33"><FONT face="宋体">
											<asp:textbox id="TextBoxBeginDate" runat="server" BorderStyle="Groove">2006-01-01</asp:textbox>
											<asp:ImageButton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:ImageButton></FONT>
									</TD>
									<TD style="WIDTH: 139px; HEIGHT: 33px" bgColor="#ffffff" height="33"><FONT face="宋体">结束日期</FONT></TD>
									<TD colspan="2" style="HEIGHT: 33px" bgColor="#ffffff" height="33"><FONT face="宋体">
											<asp:textbox id="TextBoxEndDate" runat="server" BorderStyle="Groove"></asp:textbox>
											<asp:ImageButton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:ImageButton></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 128px; HEIGHT: 1px" bgColor="#ffffff" height="1">
										<P align="center"><FONT face="宋体">按销户帐号</FONT></P>
									</TD>
									<TD style="WIDTH: 299px; HEIGHT: 1px" bgColor="#ffffff" height="1"><FONT face="宋体"><asp:textbox id="TxbQueryQQ" runat="server" BorderStyle="Groove"></asp:textbox></FONT></TD>
									<TD style="WIDTH: 139px; HEIGHT: 1px" bgColor="#ffffff" height="1"><FONT face="宋体">按操作员</FONT></TD>
									<TD style="HEIGHT: 1px" bgColor="#ffffff" height="1"><FONT face="宋体"><asp:textbox id="txbHandID" runat="server" BorderStyle="Groove"></asp:textbox></FONT></TD>
                                    <td bgColor="#ffffff">微信绑定QQ<asp:textbox id="tbWxQQ" runat="server" BorderStyle="Groove"></asp:textbox>&nbsp;&nbsp;微信绑定邮箱<asp:textbox id="tbWxEmail" runat="server" BorderStyle="Groove"></asp:textbox>&nbsp;&nbsp;微信绑定手机<asp:textbox id="tbWxPhone" runat="server" BorderStyle="Groove"></asp:textbox></td>
								</TR>
								<TR>
									<TD bgColor="#ffffff" colSpan="5" height="12"><FONT face="宋体">
											<P align="center"><asp:button id="btQuery" runat="server" Width="108px" Text="查 询" CausesValidation="False" onclick="btQuery_Click"></asp:button></P>
										</FONT>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
		</form>
		</P>
	</body>
</HTML>
