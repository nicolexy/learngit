<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="AgencyBusinessQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.AgencyBusinessQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AgencyBusinessQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
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
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;中介商户查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">用户帐号</asp:label></TD>
					<TD><asp:textbox id="txtQQ" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">网址</asp:label></TD>
					<TD><asp:textbox id="txtNetAddress" runat="server"></asp:textbox></TD>
				</TR>
                
				<TR>
					<TD align="center" colSpan="4"><FONT face="宋体"><asp:button id="btnSearch" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 820px; POSITION: absolute; TOP: 150px; HEIGHT: 30%">
				<table cellSpacing="0" cellPadding="0" width="820" border="0">
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="820px" AllowPaging="True" BorderColor="#E7E7FF"
								BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
								HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="Fid"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Sflag"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fqqid" HeaderText="帐号">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                    
									<asp:BoundColumn DataField="Fdomain" HeaderText="域名">
										<HeaderStyle Width="190px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Femail" HeaderText="邮箱">
										<HeaderStyle Width="180px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DictName" HeaderText="状态">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:ButtonColumn Text="查看" CommandName="Select">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:ButtonColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
                    <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
				</table>
			</div>
			<div id="divInfo" style="LEFT: 5%; WIDTH: 820px; POSITION: absolute; TOP: 280px; HEIGHT: 50%"
				runat="server">
				<table cellSpacing="1" cellPadding="1" width="820" align="center" border="1">
					<tr>
						<TD align="left" width="150"><asp:label id="Label5" runat="server">序列号</asp:label></TD>
						<td align="left" width="250"><asp:label id="lblNo" runat="server"></asp:label></td>
						<TD align="left" width="150"><asp:label id="Label6" runat="server">帐号</asp:label></TD>
						<td align="left" width="250"><asp:label id="lblQQ" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label7" runat="server">域名</asp:label></TD>
						<td align="left"><asp:label id="lblURL" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label9" runat="server">电话</asp:label></TD>
						<td align="left"><asp:label id="lblPhone" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label8" runat="server">手机</asp:label></TD>
						<td align="left"><asp:label id="lblMobile" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label11" runat="server">邮箱</asp:label></TD>
						<td align="left"><asp:label id="lblEmail" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label10" runat="server">行业类型</asp:label></TD>
						<td align="left"><asp:label id="lblTradeType" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label13" runat="server">姓名</asp:label></TD>
						<td align="left"><asp:label id="lblName" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label12" runat="server">地址</asp:label></TD>
						<td align="left"><asp:label id="lblAddress" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label15" runat="server">邮编</asp:label></TD>
						<td align="left"><asp:label id="lblAddressNo" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td align="left"><asp:label id="Label14" runat="server">帐户备注</asp:label></td>
						<td align="left"><asp:label id="lblRemark" Runat="server"></asp:label></td>
						<td align="left"><asp:label id="Label4" runat="server">创建时间</asp:label></td>
						<td align="left"><asp:label id="lblCreateTime" Runat="server"></asp:label></td>
					</tr>
					<tr>
						<td align="left"><asp:label id="Label16" runat="server">修改时间</asp:label></td>
						<td align="left"><asp:label id="lblModifyTime" Runat="server"></asp:label></td>
						<td align="left"><asp:label id="Label18" runat="server">审核人ID</asp:label></td>
						<td align="left"><asp:label id="lblApprovePersonID" Runat="server"></asp:label></td>
					</tr>
					<tr>
						<td align="left"><asp:label id="Label17" runat="server">审核人</asp:label></td>
						<td align="left"><asp:label id="lblApprovePerson" Runat="server"></asp:label></td>
						<td align="left"><asp:label id="Label20" runat="server">审核时间</asp:label></td>
						<td align="left"><asp:label id="lblApproveTime" Runat="server"></asp:label></td>
					</tr>
					<tr>
						<td align="left"><asp:label id="Label19" runat="server">状态</asp:label></td>
						<td align="left"><asp:label id="lblType" Runat="server"></asp:label></td>
						<td align="left"><asp:label id="Label22" runat="server">推荐人</asp:label></td>
						<td align="left"><asp:label id="lblCommendatory" Runat="server"></asp:label></td>
					</tr>
					<tr>
						<td align="left"><asp:label id="Label21" Runat="server">操作备注</asp:label></td>
						<td align="left" colSpan="3"><asp:label id="lblOperateRemark" Runat="server"></asp:label></td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
