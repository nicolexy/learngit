<%@ Page language="c#" Codebehind="RealtimeOrder.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.RealtimeOrder" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RealtimeOrder</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style5 { COLOR: #000000 }
	.style6 { COLOR: #ff0000 }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 103; LEFT: 2.25%; POSITION: absolute; TOP: 1.52%; HEIGHT: 106px"
				cellSpacing="0" cellPadding="0" width="95%" border="1">
				<TR>
					<TD style="WIDTH: 100%" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7" colSpan="5">
						<DIV align="center">
							<TABLE id="Table3" height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<TR>
									<TD width="79%"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
												&nbsp;订单实时调帐</FONT></FONT></TD>
									<TD width="21%"><FONT face="宋体">&nbsp;</FONT>操作员代码: <SPAN class="style3">
											<asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
								</TR>
							</TABLE>
							<SPAN class="style3"></SPAN>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 91px" align="right"><asp:label id="Label7" runat="server">充值银行</asp:label></TD>
					<TD style="WIDTH: 290px"><FONT face="宋体">&nbsp;<asp:dropdownlist id="ddlBankType" runat="server"></asp:dropdownlist>
						</FONT>
					</TD>
					<TD align="right"><asp:label id="Label3" runat="server">订单号</asp:label></TD>
					<TD><asp:textbox id="tbOrder" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="labError" runat="server" ForeColor="Red"></asp:label></TD>
					<TD align="center" colSpan="2"><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 103; LEFT: 2.25%; POSITION: absolute; TOP: 25%" cellSpacing="1"
				cellPadding="1" width="95%" border="0" runat="server">
				<TBODY>
					<tr borderColor="#999999" bgColor="#999999">
						<td colSpan="4"></td>
					</tr>
					<TR>
						<TD align="center" width="20%">
							<P align="right"><asp:label id="Label10" runat="server">收款单ID：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD align="center" width="30%">
							<P align="left"><asp:label id="labFListID" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
						<TD align="center" width="20%">
							<P align="right"><asp:label id="Label4" runat="server">交易金额：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD align="center" width="30%">
							<P align="left"><asp:label id="labFNum" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
					</TR>
					<tr borderColor="#999999" bgColor="#999999">
						<td colSpan="4"></td>
					</tr>
					<TR>
						<TD style="HEIGHT: 17px" align="center">
							<P align="right"><asp:label id="Label14" runat="server">付款状态：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD style="HEIGHT: 17px" align="center">
							<P align="left"><asp:label id="labFState" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
						<TD style="HEIGHT: 17px" align="center">
							<P align="right"><asp:label id="Label12" runat="server">交易标记：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD style="HEIGHT: 17px" align="center">
							<P align="left"><asp:label id="labFSign" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
					</TR>
					<tr borderColor="#999999" bgColor="#999999">
						<td colSpan="4"></td>
					</tr>
					<TR>
						<TD align="center">
							<P align="right"><asp:label id="Label19" runat="server">银行订单号：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD align="center">
							<P align="left"><asp:label id="labFBank_List" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
						<TD align="center">
							<P align="right"><asp:label id="Label21" runat="server">银行返回订单号：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD align="center">
							<P align="left"><asp:label id="labFBank_Acc" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
					</TR>
					<tr borderColor="#999999" bgColor="#999999">
						<td colSpan="4"><FONT face="宋体"></FONT></td>
					</tr>
					<TR>
						<TD style="HEIGHT: 18px" align="center">
							<P align="right"><asp:label id="Label5" runat="server">银行类型：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD style="HEIGHT: 18px" align="center">
							<P align="left"><asp:label id="labFBank_Type" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
						<TD style="HEIGHT: 18px" align="center">
							<P align="right"><asp:label id="Label15" runat="server">充值人帐号：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD style="HEIGHT: 18px" align="center">
							<P align="left"><asp:label id="labFaid" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
					</TR>
					<tr borderColor="#999999" bgColor="#999999">
						<td colSpan="4"><FONT face="宋体"></FONT></td>
					</tr>
					<TR>
						<TD align="center">
							<P align="right"><asp:label id="Label6" runat="server">充值人姓名：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD align="center">
							<P align="left"><asp:label id="labFaname" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
						<TD align="center">
							<P align="right"><FONT face="宋体"></FONT><asp:label id="Label16" runat="server">付款前时间：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD align="center">
							<P align="left"><asp:label id="labFpay_front_time" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
					</TR>
					<tr borderColor="#999999" bgColor="#999999">
						<td colSpan="4"></td>
					</tr>
					<TR>
						<TD style="HEIGHT: 16px" align="center">
							<P align="right"><asp:label id="Label23" runat="server">银行返回时间：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD style="HEIGHT: 16px" align="center">
							<P align="left"><asp:label id="labFbank_time" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
						<TD style="HEIGHT: 16px" align="center">
							<P align="right"><asp:label id="Label25" runat="server">最后修改时间：</asp:label><FONT face="宋体">:</FONT></P>
						</TD>
						<TD style="HEIGHT: 16px" align="center">
							<P align="left"><asp:label id="labFmodify_time" runat="server" ForeColor="Blue"></asp:label></P>
						</TD>
					</TR>
					<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
						<td colSpan="4"></td>
					</tr>
					<TR style="HEIGHT: 46px">
						<TD vAlign="middle" align="center" colSpan="4"><FONT face="宋体"></FONT></TD>
					</TR>
				</TBODY>
			</TABLE>
		</form>
	</body>
</HTML>
