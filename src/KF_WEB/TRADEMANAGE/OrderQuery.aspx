<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="OrderQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.OrderQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FundQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style5 { COLOR: #000000 }
	.style6 { COLOR: #ff0000 }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table2" style="Z-INDEX: 102; POSITION: absolute; HEIGHT: 0.93%; TOP: 155px; LEFT: 2.25%"
				cellSpacing="0" cellPadding="0" width="95%" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" EnableViewState="False" PageSize="15"
							AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderColor="LightGray">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="Black" BackColor="Silver"></HeaderStyle>
							<Columns>
								<asp:HyperLinkColumn DataNavigateUrlField="FlistidUrl" DataNavigateUrlFormatString="OrderDetail.aspx?{0}"
									DataTextField="FListID" HeaderText="订单号"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="FBank_listid" HeaderText="给银行订单号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FBuyID" HeaderText="买家帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSaleid" HeaderText="卖家帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTradeStateName" HeaderText="交易状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftrade_typeName" HeaderText="交易类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="FpaynumName" HeaderText="实际支付费用"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCreate_time" HeaderText="创建时间"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" PageSize="15" CustomInfoTextAlign="Center" AlwaysShow="True"
							NumericButtonCount="5" ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always"
							CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
			<TABLE id="Table1" style="Z-INDEX: 103; POSITION: absolute; HEIGHT: 106px; TOP: 1.52%; LEFT: 2.25%"
				cellSpacing="0" cellPadding="0" width="95%" border="1">
				<TR>
					<TD style="WIDTH: 100%" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7" colSpan="5">
						<DIV align="center">
							<TABLE id="Table3" height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<TR>
									<TD width="79%"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;
												<asp:label id="Label7" runat="server" Width="73px" ForeColor="Red">订单查询</asp:label></FONT></FONT></TD>
									<TD width="21%"><FONT face="宋体">&nbsp;</FONT>操作员代码: <SPAN class="style3">
											<asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
								</TR>
							</TABLE>
							<SPAN class="style3"></SPAN>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 91px" align="right">
						<asp:label id="Label2" runat="server">开始日期</asp:label></TD>
					<TD style="WIDTH: 290px">
						<asp:textbox id="TextBoxBeginDate" runat="server" Width="100px" BorderStyle="Groove"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						<FONT face="宋体">&nbsp;
						</FONT>
					</TD>
					<TD align="right">
						<asp:label id="Label3" runat="server">结束日期</asp:label></TD>
					<TD>
						<asp:textbox id="TextBoxEndDate" runat="server" Width="100px" BorderStyle="Groove"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 91px; HEIGHT: 25px" align="right">
						<asp:label id="Label5" runat="server">买家帐号</asp:label></TD>
					<TD style="WIDTH: 290px; HEIGHT: 25px">
						<asp:TextBox id="tbBuyQQ" runat="server"></asp:TextBox></TD>
					<TD style="HEIGHT: 25px" align="right">
						<asp:Label id="Label6" runat="server">卖家帐号</asp:Label></TD>
					<TD style="HEIGHT: 25px"><FONT face="宋体">
							<asp:TextBox id="tbSaleQQ" runat="server"></asp:TextBox></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 91px; HEIGHT: 25px" align="right">
						<asp:label id="Label8" runat="server">买家帐号内部ID</asp:label></TD>
					<TD style="WIDTH: 290px; HEIGHT: 25px">
						<asp:TextBox id="tbBuyQQInnerID" runat="server"></asp:TextBox></TD>
					<TD style="HEIGHT: 25px" align="right">
						<asp:Label id="Label9" runat="server">卖家帐号内部ID</asp:Label></TD>
					<TD style="HEIGHT: 25px"><FONT face="宋体">
							<asp:TextBox id="tbSaleQQInnerID" runat="server"></asp:TextBox></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 91px" align="right">
						<asp:dropdownlist id="dpLst" runat="server" AutoPostBack="True">
							<asp:ListItem Value="FlistID" Selected="True">订单号</asp:ListItem>
							<asp:ListItem Value="FSpid">机构代码</asp:ListItem>
							<asp:ListItem Value="FBank_listID">给银行的订单号</asp:ListItem>
							<asp:ListItem Value="FCoding">商户订单编码</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD style="WIDTH: 290px">
						<asp:textbox id="tbListID" runat="server" Width="165px" BorderStyle="Groove"></asp:textbox></TD>
					<TD align="right"><FONT face="宋体">
							<asp:label id="Label4" runat="server">交易状态</asp:label></FONT></TD>
					<TD align="left">
						<asp:dropdownlist id="ddlStateType" runat="server" Width="152px" AutoPostBack="True">
							<asp:ListItem Value="99" Selected="True">所有状态</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD colspan="4" align="center">
						<asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
