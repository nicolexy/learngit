<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="FeeQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.FeeQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FeeQuery</title>
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
				<TR height="24" bgColor="#eeeeee">
					<TD><FONT face="宋体" color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">批次总信息</FONT></TD>
					<TD align="right" bgColor="#e4e5f7"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<TABLE id="Table2" border="1" cellSpacing="1" cellPadding="1" width="100%">
							<TR>
								<TD><asp:label id="Label5" runat="server">当前状态</asp:label></TD>
								<TD style="WIDTH: 340px"><FONT face="宋体"><asp:dropdownlist id="ddlState" runat="server" Width="104px">
											<asp:ListItem Value="9" Selected="True">所有状态</asp:ListItem>
											<asp:ListItem Value="0">初始状态</asp:ListItem>
											<asp:ListItem Value="1">付款成功</asp:ListItem>
											<asp:ListItem Value="2">退款成功</asp:ListItem>
										</asp:dropdownlist></FONT></TD>
								<TD><asp:label id="Label4" runat="server">交易类型</asp:label></TD>
								<TD><asp:dropdownlist id="ddlFtype" runat="server" Width="104px">
										<asp:ListItem Value="9" Selected="True">所有方式</asp:ListItem>
										<asp:ListItem Value="water">水费</asp:ListItem>
										<asp:ListItem Value="elec">电费</asp:ListItem>
										<asp:ListItem Value="gas">气费</asp:ListItem>
										<asp:ListItem Value="tel">电话费</asp:ListItem>
										<asp:ListItem Value="outtime">超时退款</asp:ListItem>
									</asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD><asp:label id="Label6" runat="server">款项情况</asp:label></TD>
								<TD style="WIDTH: 340px"><asp:dropdownlist id="ddlPaytypeState" runat="server" Width="104px">
										<asp:ListItem Value="9" Selected="True">所有状态</asp:ListItem>
										<asp:ListItem Value="0">退款</asp:ListItem>
										<asp:ListItem Value="1">打款</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD><asp:label id="Label2" runat="server">交易单</asp:label></TD>
								<TD><asp:textbox id="tblistID" runat="server" Width="200px"></asp:textbox><asp:button id="btnQuery" runat="server" Text="查询记录" onclick="btnQuery_Click"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR height="24" bgColor="#eeeeee">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG src="../IMAGES/Page/post.gif" width="15" height="16">
								<asp:label id="Label15" runat="server">缴费详细信息</asp:label><FONT face="宋体"></FONT></SPAN></FONT></STRONG></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" Width="100%" Height="70px" AutoGenerateColumns="False"
							BackColor="White" CellPadding="3" GridLines="Horizontal" BorderColor="#E7E7FF" BorderWidth="1px" BorderStyle="None">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:HyperLinkColumn DataNavigateUrlField="flistid" DataNavigateUrlFormatString="../TradeManage/TradeLogQuery.aspx?id={0}"
									DataTextField="flistid" HeaderText="交易单"></asp:HyperLinkColumn>
								<asp:BoundColumn Visible="False" DataField="Fbatchid" HeaderText="批次号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="StatusStr" HeaderText="交易单状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fnumname" HeaderText="交易金额"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="FPaynumname" HeaderText="付款金额"></asp:BoundColumn>
								<asp:BoundColumn DataField="FrefundNumname" HeaderText="退款金额"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftypename" HeaderText="缴费类型"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Frate" HeaderText="费率(万分之)"></asp:BoundColumn>
								<asp:BoundColumn DataField="FcreateTypename" HeaderText="创建类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fstatename" HeaderText="当前状态"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="FPaytypename" HeaderText="缴费结果"></asp:BoundColumn>
								<asp:TemplateColumn Visible="False" HeaderText="调整">
									<ItemTemplate>
										<asp:CheckBox id="CheckBox1" runat="server" Text="选择"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FCreateTime" HeaderText="对账日期"></asp:BoundColumn>
                                <asp:BoundColumn DataField="UserId" HeaderText="缴费账号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="account" HeaderText="QQ"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD colSpan="2"></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<TABLE id="Table3" border="0" cellSpacing="0" cellPadding="0" width="100%">
							<TR>
								<TD width="20%" align="center"></TD>
								<TD width="20%" align="center"></TD>
								<TD width="20%" align="center"></TD>
								<TD width="20%" align="center"></TD>
								<TD width="20%" align="center"></TD>
							</TR>
						</TABLE>
						<asp:label id="labErrMsg" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
			</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
