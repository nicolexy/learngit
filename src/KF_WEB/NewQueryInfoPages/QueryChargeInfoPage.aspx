<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryChargeInfoPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryChargeInfoPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryChargeInfoPage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="1000">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;查询<asp:label id="lb_pageTitle" Runat="server">投资人资金流水信息</asp:label></FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<tr>
					<TD rowSpan="4" align="right"><asp:radiobutton id="rtbSpid" Runat="server" Checked="True" GroupName="rtnChoose" Text="详细查询"></asp:radiobutton></TD>
					<td><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT><span>查询类型</span>
						<asp:dropdownlist id="dd_queryType" Runat="server">
							<asp:ListItem Value="0" Selected="True">所有资金流水</asp:ListItem>
							<asp:ListItem Value="99" Selected="False">所有充值提现信息</asp:ListItem>
							<asp:ListItem Value="11" Selected="False">投资人充值</asp:ListItem>
							<asp:ListItem Value="14" Selected="False">投资人提现</asp:ListItem>
						</asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
						<asp:label runat="server" id="Label1">QQ帐号&nbsp;&nbsp;&nbsp;</asp:label><asp:textbox id="tbx_1" Runat="server" Width="250"></asp:textbox></td>
					<td><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
						<asp:label runat="server" id="Label2">交易单号&nbsp;&nbsp;&nbsp;</asp:label><asp:textbox id="tbx_3" Runat="server" Width="250"></asp:textbox></td>
				</tr>
				<tr>
					<TD><FONT face="宋体">&nbsp;&nbsp;&nbsp; </FONT>
						<asp:label runat="server" id="Label3">&nbsp;&nbsp;查询开始日期&nbsp;&nbsp;&nbsp;</asp:label>
                        <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker()" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					<td><FONT face="宋体">&nbsp;&nbsp;&nbsp; </FONT>
						<asp:label runat="server" id="Label4">&nbsp;&nbsp;查询结束日期&nbsp;&nbsp;&nbsp;</asp:label>                        
                        <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker()" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</td>
					</TD></tr>
				<tr>
					<TD colSpan="4" align="center"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button></TD>
				</tr>
			</TABLE>
			<table border="0" cellSpacing="0" cellPadding="0" width="1000">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="1000px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fbkid" HeaderText="流水ID号">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Flistid" HeaderText="交易单ID号">
									<HeaderStyle Width="200px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fspid" HeaderText="机构代码名称">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fqqid" HeaderText="用户ID">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FtypeName" HeaderText="借贷类型">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FsubjectName" HeaderText="科目">
									<HeaderStyle Width="50px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Ffromid" HeaderText="对方的ID">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="交易时间">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fpaynum" HeaderText="金额">
									<HeaderStyle Width="50px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbalance" HeaderText="帐户余额">
									<HeaderStyle Width="50px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fexplain" HeaderText="交易信息">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:ButtonColumn ButtonType="LinkButton" HeaderText="详细信息" Text="详细" CommandName="detail">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:ButtonColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
			<table border="0" cellSpacing="0" cellPadding="0" width="900" style="DISPLAY:none">
				<TR>
					<TD vAlign="top"><asp:datagrid id="Datagrid1" runat="server" Width="900px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Ftrue_name" HeaderText="用户名称">
									<HeaderStyle Width="200px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Ffrom_name" HeaderText="对方的名称">
									<HeaderStyle Width="200px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fbalance" HeaderText="帐户余额">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fpaynum" HeaderText="金额">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_typeName" HeaderText="用户银行类型">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FcurtypeName" HeaderText="币种">
									<HeaderStyle Width="50px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fip" HeaderText="客户的IP地址">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fexplain" HeaderText="备注">
									<HeaderStyle Width="50px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="用户操作时间">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="Aspnetpager1" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
			<table cellSpacing="1" cellPadding="0" width="900" border="0" bgcolor="black">
				<tr>
					<td bgColor="#eeeeee" height="18" colspan="4"><span>&nbsp;详细信息列表：</span></td>
				</tr>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" width="229" bgColor="#eeeeee" height="19"><font face="宋体">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="宋体">交易单号:</FONT></font></TD>
					<TD style="HEIGHT: 19px" width="225" bgColor="#ffffff" height="19"><font face="宋体">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="宋体"> &nbsp;机构代码:</font></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></font></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="宋体">&nbsp;用户的ID:</font></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20" colspan="3"><font face="宋体">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></font></TD>
					<%--<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;用户名称:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="宋体">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>--%>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="宋体">&nbsp;借贷类型:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><font face="宋体">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;科目:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;对方的ID:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;对方的名称:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<%--<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;帐户余额:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>--%>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;金额:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19" colspan="3"><FONT face="宋体">&nbsp;<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;用户银行类型:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<FONT face="宋体"><asp:label id="lb_c11" runat="server"></asp:label></FONT></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18">&nbsp;币种代码:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><font face="宋体">&nbsp;客户的IP地址:</font></TD>
					<TD bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c13" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><font face="宋体">&nbsp;最后修改时间:</font></TD>
					<TD bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c14" runat="server"></asp:label></font></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><font face="宋体">&nbsp;备注/说明:</font></TD>
					<TD bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c15" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><font face="宋体">&nbsp;用户操作时间:</font></TD>
					<TD bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c16" runat="server"></asp:label></font></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
