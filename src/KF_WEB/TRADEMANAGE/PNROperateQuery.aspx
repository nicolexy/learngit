<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="PNROperateQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.PNROperateQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PNROperateQuery</title>
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
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="820" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="6"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;PNR操作查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">PNR：</asp:label></TD>
					<TD><asp:textbox id="txtPNR" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">开始时间：</asp:label></TD>
					<TD><asp:textbox id="txtstartTime" runat="server" Width="200"></asp:textbox></TD>
                    <TD align="right"><asp:label id="Label4" runat="server">结束时间：</asp:label></TD>
					<TD><asp:textbox id="txtendTime" runat="server" Width="200"></asp:textbox></TD>
				</TR>
				<TR>
					
                    <TD align="right"><asp:label id="Label21" runat="server">航空公司：</asp:label></TD>
					<TD><asp:dropdownlist id="ddlAgent" runat="server" Width="152px">
							<%--<asp:ListItem Value="99">不排序</asp:ListItem>
							<asp:ListItem Value="0">时间小到大</asp:ListItem>
							<asp:ListItem Value="1">时间大到小</asp:ListItem>--%>
						</asp:dropdownlist></TD>
                    <TD align="right"><asp:label id="Label24" runat="server">状态：</asp:label></TD>
					<TD><asp:dropdownlist id="ddlStatus" runat="server" Width="152px">
							<%--<asp:ListItem Value="99">不排序</asp:ListItem>
							<asp:ListItem Value="0">时间小到大</asp:ListItem>
							<asp:ListItem Value="1">时间大到小</asp:ListItem>--%>
						</asp:dropdownlist></TD>
                    <TD align="center" colspan="2"><FONT face="宋体"><asp:button id="btnSearch" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH:900px; POSITION: absolute; TOP: 170px;">
				<table cellSpacing="0" cellPadding="0" width="100%" border="0">
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF"  Width="100%">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Fpnr" HeaderText="PNR">
										<HeaderStyle Width="200px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="startTime" HeaderText="开始时间">
									    <HeaderStyle Width="130px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="endTime" HeaderText="结束时间">
										<HeaderStyle Width="130px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fstatus_str" HeaderText="状态">
										<HeaderStyle Width="130px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fagent_str" HeaderText="航空公司">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FairUserID" HeaderText="航司账号">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fplatformid" HeaderText="平台">
										<HeaderStyle Width="170px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fip" HeaderText="IP">
										<HeaderStyle Width="170px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fcmd" HeaderText="命令字">
										<HeaderStyle Width="170px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
                    <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" PageSize="10"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				   </TR>
				</table>
			</div>
		</form>
	</body>
</HTML>
