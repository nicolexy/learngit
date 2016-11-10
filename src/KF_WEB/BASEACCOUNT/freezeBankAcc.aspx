<%@ Page language="c#" Codebehind="freezeBankAcc.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.freezeBankAcc" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FreezeReason</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #ff0000; FONT-WEIGHT: bold }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table2" style="Z-INDEX: 101; POSITION: absolute; WIDTH: 436px; HEIGHT: 355px; TOP: 18%; LEFT: 30%"
				cellSpacing="1" cellPadding="1" width="336" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;
									<asp:label id="Label1_state" runat="server" Width="88px" ForeColor="Red">Label</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:label id="Label2" runat="server">操作员代码：</asp:label><asp:label id="labUid" runat="server" Width="64px"></asp:label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><FONT face="宋体" color="gray">如果是商户的QQ号，则无法冻结，请联系系统管理员。</FONT><br>
						<TABLE id="Table1" style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0"
							runat="server">
							<TR borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"><FONT face="宋体"></FONT></TD>
							</TR>
							<TR>
								<TD align="center">
									<P align="right"><asp:label id="Label10" runat="server">用户帐号：</asp:label><FONT face="宋体">:</FONT></P>
								</TD>
								<TD align="center" width="70%" colSpan="3">
									<P align="left"><asp:label id="Label_listID" runat="server" ForeColor="Red">0001</asp:label></P>
								</TD>
							</TR>
							<TR borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"><FONT face="宋体"></FONT></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 17px" align="center">
									<P align="right"><asp:label id="Label14" runat="server">用户姓名：</asp:label><FONT face="宋体">:</FONT></P>
								</TD>
								<TD style="HEIGHT: 17px" align="center" colSpan="3"><P align="left"><asp:textbox id="tbUserName" runat="server" Width="192px"></asp:textbox></P>
								</TD>
							</TR>
							<TR borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"><FONT face="宋体"></FONT></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right"><asp:label id="Label5" runat="server">联系方式：</asp:label><FONT face="宋体">:</FONT></P>
								</TD>
								<TD style="HEIGHT: 18px" align="center" colSpan="3"><P align="left"><asp:textbox id="tbContact" runat="server" Width="192px"></asp:textbox></P>
								</TD>
							</TR>
                            <TR>
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right"><asp:label id="Label1" runat="server">冻结渠道：</asp:label><FONT face="宋体">:</FONT></P>
								</TD>
								<TD style="HEIGHT: 18px" align="left" colSpan="3">
                                    <asp:dropdownlist id="ddlFreezeChannel" runat="server">
							         <%--   <asp:ListItem Value="1" Selected="True">风控冻结</asp:ListItem>
                                        <asp:ListItem Value="2">拍拍冻结</asp:ListItem>
                                        <asp:ListItem Value="3">用户冻结</asp:ListItem>
                                        <asp:ListItem Value="4">商户冻结</asp:ListItem>
                                        <asp:ListItem Value="5">BG接口冻结</asp:ListItem>
                                        <asp:ListItem Value="6">涉嫌可疑交易冻结</asp:ListItem>--%>
                                    </asp:dropdownlist>
								</TD>
							</TR>
							<TR borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"><FONT face="宋体"></FONT></TD>
							</TR>
							<!--  暂时不开放“添加到期冻结日期”  -->
							<TR style="DISPLAY:none">
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right"><asp:label id="Label3" runat="server">冻结到期日期：</asp:label><FONT face="宋体">:</FONT></P>
								</TD>
								<TD style="HEIGHT: 18px" align="left" valign="middle" colSpan="3">
									<div>
										<asp:CheckBox Runat="server" ID="cbx_showEndDate" Text="添加冻结到期日期"></asp:CheckBox>
									</div>
									<div id="div_endDate" runat="server">
										<asp:textbox id="tbx_FreezeEndDate" runat="server" Width="192px" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
									</div>
								</TD>
							</TR>
							<TR style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"><FONT face="宋体"></FONT></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 236px; HEIGHT: 18px" align="center">
									<P align="right"><asp:label id="labReason" runat="server">冻结原因：</asp:label><FONT face="宋体">:</FONT></P>
								</TD>
								<TD style="HEIGHT: 18px" align="center" colSpan="3"><P align="left"><asp:textbox id="tbMemo" runat="server" Width="208px" Height="92px" TextMode="MultiLine"></asp:textbox></P>
								</TD>
							</TR>
							<TR style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
								<TD colSpan="4"><FONT face="宋体"></FONT></TD>
							</TR>
						</TABLE>
						<asp:button id="BT_F_Or_Not" runat="server" Width="87px" Text="冻结账户" onclick="BT_F_Or_Not_Click"></asp:button><asp:button id="Button1_back" runat="server" Width="118px" ForeColor="Red" Height="25px" Text=" 请点击返回"
							Visible="False" CausesValidation="False" onclick="Button1_back_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<br>
			<br>
		</form>
	</body>
</HTML>
