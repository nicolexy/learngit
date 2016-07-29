<%@ Page language="c#" Codebehind="SettleRule.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SettleRule" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SettQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body id="bodyid" runat="server">
		<form id="Form1" method="post" runat="server">
			<TABLE cellSpacing="1" cellPadding="1" width="820" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;收支分离查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right" width="100">商户</TD>
					<TD align="left" width="420"><asp:textbox id="TextBoxSpid" runat="server"></asp:textbox></TD>
					<td align="left" width="300"><asp:button id="btnSearch" Text="查 询" Runat="server" Width="80px" onclick="btnSearch_Click"></asp:button></td>
				</TR>
				<tr>
					<td></td>
				</tr>
			</TABLE>
			<table cellSpacing="0" cellPadding="0" width="820" border="0">
				<tr>
					<TD vAlign="top" align="center"><asp:datagrid id="DataGrid1" runat="server" Width="820" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
							HorizontalAlign="Center" PageSize="15" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
							BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="True">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fspid" HeaderText="商户号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ffee_qqid" HeaderText="手续费支出帐户"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always"
							CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"
							AlwaysShow="True"></webdiyer:aspnetpager>
					</TD>
				</tr>
			</table>
		</form>
	</body>
</HTML>
