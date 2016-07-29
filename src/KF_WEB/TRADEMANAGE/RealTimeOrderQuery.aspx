<%@ Page language="c#" Codebehind="RealTimeOrderQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.RealTimeOrderQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RealTimeOrderQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 2.2%; POSITION: absolute" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;&nbsp;订单实时查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						</FONT>操作员代码: <SPAN class="style3">
							<asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 111px" align="right"><asp:label id="Label2" runat="server">开始日期</asp:label></TD>
					<TD style="WIDTH: 291px"><asp:textbox id="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">结束日期</asp:label></TD>
					<TD><asp:textbox id="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 111px" align="right"><asp:label id="Label5" runat="server">处理状态</asp:label></TD>
					<TD style="WIDTH: 291px"><asp:dropdownlist id="ddlStateType" runat="server" Width="152px">
							<asp:ListItem Value="9">所有状态</asp:ListItem>
							<asp:ListItem Value="0">初始状态</asp:ListItem>
							<asp:ListItem Value="1">无需处理</asp:ListItem>
							<asp:ListItem Value="2" Selected="True">需要调帐</asp:ListItem>
							<asp:ListItem Value="3">调帐失败</asp:ListItem>
							<asp:ListItem Value="4">原交易已成功</asp:ListItem>
							<asp:ListItem Value="5">调帐已成功</asp:ListItem>
							<asp:ListItem Value="6">订单匹配失败</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD align="right"><asp:label id="Label6" runat="server">金额限度</asp:label></TD>
					<TD><asp:textbox id="tbFNum" runat="server">0</asp:textbox><asp:regularexpressionvalidator id="Regularexpressionvalidator7" runat="server" Display="Dynamic" ControlToValidate="tbFNum"
							ErrorMessage="RegularExpressionValidator" ToolTip="**.**" ValidationExpression="^[0-9/.]+">请输入正确金额</asp:regularexpressionvalidator></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 111px" align="right" colSpan="1" rowSpan="1"><asp:label id="Label7" runat="server">订单号</asp:label></TD>
					<TD style="WIDTH: 291px"><asp:textbox id="tbQQID" runat="server" Width="165px"></asp:textbox></TD>
					<TD align="right"><FONT face="宋体"><asp:label id="Label4" runat="server">排序类型</asp:label></FONT></TD>
					<TD align="left"><asp:dropdownlist id="ddlSortType" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">不排序</asp:ListItem>
							<asp:ListItem Value="1">时间小到大</asp:ListItem>
							<asp:ListItem Value="2">时间大到小</asp:ListItem>
							<asp:ListItem Value="3">金额小到大</asp:ListItem>
							<asp:ListItem Value="4">金额大到小</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 111px" align="right" colSpan="1" rowSpan="1">
						<asp:Label id="Label8" runat="server">银行类型</asp:Label></TD>
					<TD style="WIDTH: 291px">
						<asp:DropDownList id="ddlBankType" runat="server"></asp:DropDownList></TD>
					<TD align="center" colspan="2"><FONT face="宋体"><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 2.2%; POSITION: absolute; TOP: 22%" cellSpacing="1"
				cellPadding="1" width="90%" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" PageSize="50" AutoGenerateColumns="False"
							GridLines="Horizontal" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Freq_orderno" HeaderText="订单号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fans_orderno" HeaderText="银行订单"></asp:BoundColumn>
								<asp:BoundColumn DataField="FBankName" HeaderText="银行类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="订单创建时间"></asp:BoundColumn>
								<asp:BoundColumn DataField="FNewAmount" HeaderText="充值金额"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStatusName" HeaderText="订单状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSignName" HeaderText="处理状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Flistid" HeaderText="交易单ID"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" PageSize="50" AlwaysShow="True" NumericButtonCount="5"
							ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
							OnPageChanged="ChangePage" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
