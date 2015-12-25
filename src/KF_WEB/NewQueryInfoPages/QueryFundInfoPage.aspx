<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryFundInfoPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryFundInfoPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryFundInfoPage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="1100">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;查询<asp:label id="lb_pageTitle" Runat="server">基金易信息</asp:label></FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD rowSpan="3" align="right"><asp:label Runat="server" id="Label1">查询条件：</asp:label></TD>
					<td><asp:label id="lb_listID" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;交易单号：</asp:label><asp:textbox id="tbx_1" Runat="server" Width="350px"></asp:textbox></td>
				</TR>
				<tr>
					<td><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT> <label>财付通帐号：&nbsp;</label><asp:textbox id="tbx_2" Runat="server" Width="350px"></asp:textbox><label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;查询类型&nbsp;&nbsp;&nbsp;</label><asp:dropdownlist id="dd_type" Runat="server">
							<asp:ListItem Value="0" Selected="True">所有类型</asp:ListItem>
							<asp:ListItem Value="1">申购</asp:ListItem>
							<asp:ListItem Value="2">认购</asp:ListItem>
							<asp:ListItem Value="3">定投</asp:ListItem>
							<asp:ListItem Value="4">赎回</asp:ListItem>
							<asp:ListItem Value="5">撤销</asp:ListItem>
							<asp:ListItem Value="6">分红</asp:ListItem>
							<asp:ListItem Value="7">认申购失败</asp:ListItem>
							<asp:ListItem Value="8">比例确认退款</asp:ListItem>
						</asp:dropdownlist></td>
				</tr>
				<tr>
					<TD><FONT face="宋体">&nbsp;&nbsp;&nbsp; </FONT>
                        <label>&nbsp;&nbsp;查询开始时间：</label>
                        <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker()" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                        <label>&nbsp;&nbsp;查询结束时间：</label>
                        <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker()" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</TD>
				</tr>
				<tr>
					<TD colSpan="4" align="center"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button></TD>
				</tr>
			</TABLE>
			<table border="0" cellSpacing="0" cellPadding="0" width="1100">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
							HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
							GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Flistid" HeaderText="财付通订单号">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fspid" HeaderText="商户号">
									<HeaderStyle Width="150px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fpur_type" HeaderText="基金交易类型">
									<HeaderStyle Width="110px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Ftotal_fee" HeaderText="总金额">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcard_no" HeaderText="基金购买扣款的银行卡号">
									<HeaderStyle Width="200px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fstate" HeaderText="状态">
									<HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="FcurTypeName" HeaderText="业务类型">
									<HeaderStyle Width="150px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:ButtonColumn HeaderText="详细信息" ButtonType="LinkButton" Text="详细">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:ButtonColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" NumericButtonCount="5" PagingButtonSpacing="0"
							ShowInputBox="always" CssClass="mypager" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
			<table border="0" cellSpacing="1" cellPadding="0" width="1100" bgColor="black">
				<tr>
					<td bgColor="#eeeeee" height="18" colspan="4"><span>&nbsp;详细信息列表：</span></td>
				</tr>
				<TR>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18" width="195"><font face="宋体">&nbsp;&nbsp;财付通订单号:</font></TD>
					<TD bgColor="#ffffff" height="19" width="236"><font face="宋体">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19" width="229"><FONT style="BACKGROUND-COLOR: #eeeeee" face="宋体">&nbsp;&nbsp;商户号:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19" width="225"><font face="宋体">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></font></TD>
				</TR>
				<tr>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18" width="195"><font face="宋体">&nbsp;&nbsp;财付通帐号:</font></TD>
					<TD bgColor="#ffffff" height="19" width="236"><font face="宋体">&nbsp;<asp:label id="lb_c19" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="宋体">&nbsp;&nbsp;商户订单号:</font></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></font></TD>
				</tr>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="宋体">&nbsp;&nbsp;基金交易账户对应ID:</font></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;&nbsp;基金名称:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="宋体">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="宋体">&nbsp;&nbsp;基金代码:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><font face="宋体">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;&nbsp;基金交易类型:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;&nbsp;总金额:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;&nbsp;银行卡号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;&nbsp;状态:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;&nbsp;物理状态:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;&nbsp;基金交易日期:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<FONT face="宋体"><asp:label id="lb_c12" runat="server"></asp:label></FONT></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18">&nbsp;&nbsp;&nbsp;基金净值:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT style="BACKGROUND-COLOR: #eeeeee" face="宋体">&nbsp;&nbsp;基金净值日期:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;&nbsp;基金类型:</FONT></TD>
					<TD bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><font face="宋体">&nbsp;&nbsp;关联交易单号:</font></TD>
					<TD bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c16" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><font face="宋体">&nbsp;&nbsp;创建时间:</font></TD>
					<TD bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c17" runat="server"></asp:label></font></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><font face="宋体">&nbsp;&nbsp;最后修改时间:</font></TD>
					<TD bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c18" runat="server"></asp:label></font></TD>
					<td bgcolor="#ffffff"></td>
					<td bgcolor="#ffffff"></td>
				</TR>
			</table>
		</form>
	</body>
</HTML>
