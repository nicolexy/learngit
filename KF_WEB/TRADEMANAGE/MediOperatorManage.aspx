<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="MediOperatorManage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MediOperatorManage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MediOperatorManage</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
				cellPadding="1" width="85%" border="1">
				<TR>
					<TD align="right"><asp:label id="lblSPID" runat="server">商户号</asp:label></TD>
					<TD><asp:textbox id="tbSPID" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label5" runat="server">登录帐户</asp:label></TD>
					<TD>
						<asp:TextBox id="tbAccount" runat="server"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD align="center" colspan="4"><FONT face="宋体"><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<TABLE style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 18%; HEIGHT: 392px"
				cellSpacing="1" cellPadding="1" border="1" runat="server">
				<TR>
					<TD vAlign="top" align="center"><asp:datagrid id="DataGrid1" runat="server" Width="100%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
							HorizontalAlign="Center" PageSize="15" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
							BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="True">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Ftruename" HeaderText="姓名"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fdept_name" HeaderText="部门"></asp:BoundColumn>
                                
								<asp:BoundColumn DataField="Fqqid" HeaderText="帐户"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstateName" HeaderText="状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fop_typeName" HeaderText="用户类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="Flstatename" HeaderText="物理状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fspid" HeaderText="所属商户"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="权限">
									<ItemTemplate>
										<asp:HyperLink runat="server" Text="查看" Font-Underline=True ID="LinkSpid" style="CURSOR: hand"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
