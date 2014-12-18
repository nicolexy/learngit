<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="ShouFuYiQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.ShouFuYiQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AgencyBusinessQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css );
UNKNOWN {
	COLOR: #000000
}
.style3 {
	COLOR: #ff0000
}
BODY {
	BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif)
}
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="820" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;收付易查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">财付通帐号</asp:label></TD>
					<TD><asp:textbox id="txtQQ" runat="server"></asp:textbox></TD>
					<TD align="center" colSpan="2"><FONT face="宋体"><asp:button id="Button1" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 820px; POSITION: absolute; TOP: 120px; HEIGHT: 150px">
				<table cellSpacing="0" cellPadding="0" width="820" border="0">
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="820px" AllowPaging="True" BorderColor="#E7E7FF"
								BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
								PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="id"></asp:BoundColumn>
									<asp:BoundColumn DataField="CAccounts" HeaderText="商户号">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SPName" HeaderText="商户名称">
										<HeaderStyle Width="190px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyUser" HeaderText="申请人">
										<HeaderStyle Width="180px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTime" HeaderText="申请时间">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CheckUser" HeaderText="审核人">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="CheckTime" HeaderText="审核时间">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="CheckState" HeaderText="当前状态">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:ButtonColumn Text="详情" CommandName="Select">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:ButtonColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
				</table>
			</div>
			<div id="divInfo" style="LEFT: 5%; WIDTH: 820px; POSITION: absolute; TOP: 280px; HEIGHT: 300px"
				runat="server">
				<table cellSpacing="1" cellPadding="1" width="820" align="center" border="1">
					<tr>
						<TD align="left" width="150"><asp:label id="Label5" runat="server">需要开通大额收款的账号</asp:label></TD>
						<td align="left" width="250"><asp:label id="CAccounts" runat="server"></asp:label></td>
						<TD align="left" width="150"><asp:label id="Label6" runat="server">所属BD</asp:label></TD>
						<td align="left" width="250"><asp:label id="Contacter" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label7" runat="server">商户名称</asp:label></TD>
						<td align="left"><asp:label id="SPName" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label9" runat="server">商户地址</asp:label></TD>
						<td align="left"><asp:label id="Address" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label8" runat="server">所属行业</asp:label></TD>
						<td align="left"><asp:label id="SPTradeType" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label11" runat="server">域名</asp:label></TD>
						<td align="left"><asp:label id="SPDomain" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label10" runat="server">联系人</asp:label></TD>
						<td align="left"><asp:label id="Image" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label13" runat="server">联系电话</asp:label></TD>
						<td align="left"><asp:label id="Tel" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label12" runat="server">联系QQ</asp:label></TD>
						<td align="left"><asp:label id="QQ" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label15" runat="server">联系邮箱</asp:label></TD>
						<td align="left"><asp:label id="Email" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td align="left"><asp:label id="Label14" runat="server">C账号名称</asp:label></td>
						<td align="left"><asp:label id="XSE" Runat="server"></asp:label></td>
						<td align="left"><asp:label id="Label4" runat="server">渠道</asp:label></td>
						<td align="left"><asp:label id="RS" Runat="server"></asp:label></td>
					</tr>
                    <tr>
						<td align="left"><asp:label id="Label19" runat="server">结算模式</asp:label></td>
						<td align="left" colspan="3"><asp:label id="JSMode" Runat="server"></asp:label></td>
					</tr>
					<tr>
						<td align="left"><asp:label id="Label16" runat="server">付款单笔限额</asp:label></td>
						<td align="left" colspan="3"><asp:label id="JSLimit" Runat="server"></asp:label></td>
					</tr>
					<tr>
						<td align="left"><asp:label id="Label17" runat="server">提现金额</asp:label></td>
						<td align="left" colspan="3"><asp:label id="zj" Runat="server"></asp:label></td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
