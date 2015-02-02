<%@ Page language="c#" Codebehind="UserClassQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.UserClassQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FundQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1" cellPadding="1"
				width="85%" border="1">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT face="宋体" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;实名认证处理查询</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<tr>
					<td style="WIDTH: 83px" align="right">
						<asp:Label id="Label2" runat="server" Text="用户帐号"></asp:Label>
					<td>
						<asp:TextBox ID="txtQQ" Runat="server"></asp:TextBox>
					</td>
					<TD align="center" colSpan="2"><FONT face="宋体"><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></FONT></TD>
				</tr>
			</TABLE>
			<TABLE id="Table2" style="POSITION: absolute; WIDTH: 85%; HEIGHT: 70%; TOP: 150px; LEFT: 5%"
				cellSpacing="1" cellPadding="1" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" EnableViewState="False" AutoGenerateColumns="False"
							GridLines="Horizontal" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE" Font-Size="12px"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Result" HeaderText="处理结果"></asp:BoundColumn>
								<asp:BoundColumn DataField="FPickUser" HeaderText="处理人"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpicktime" HeaderText="申请时间" DataFormatString="{0:D}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FPickTime" HeaderText="处理时间" DataFormatString="{0:D}"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmemo" HeaderText="审核信息"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="详细内容">
									<ItemTemplate>
										<a href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank>详细内容</a>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
