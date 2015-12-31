<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="UserAppeal.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.UserAppeal" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>UserAppeal</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" id="Table1" border="1"
				cellSpacing="1" cellPadding="1" width="85%">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT color="red" face="宋体"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
							&nbsp;&nbsp;申诉后台处理</FONT> </FONT></TD>
					<TD bgColor="#e4e5f7" align="right"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">开始日期</asp:label></TD>
					<TD>
                        <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</TD>
					<TD align="right"><asp:label id="Label3" runat="server">结束日期</asp:label></TD>
					<TD>
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 83px" align="right"><asp:label id="Label5" runat="server">帐号</asp:label></TD>
					<TD><asp:textbox id="tbFuin" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label6" runat="server">申诉类型</asp:label></TD>
					<TD><asp:dropdownlist id="ddlType" runat="server" Width="152px" AutoPostBack="True" onselectedindexchanged="ddlType_SelectedIndexChanged">
							<asp:ListItem Value="99" Selected="True">所有自助申诉类型</asp:ListItem>
							<asp:ListItem Value="0">财付盾解绑</asp:ListItem>
							<asp:ListItem Value="1">找回密码</asp:ListItem>
							<asp:ListItem Value="2">修改姓名</asp:ListItem>
							<asp:ListItem Value="3">修改公司名</asp:ListItem>
							<asp:ListItem Value="4">注销帐号</asp:ListItem>
							<asp:ListItem Value="5">完整注册用户更换关联手机</asp:ListItem>
							<asp:ListItem Value="6">简化注册用户更换绑定手机</asp:ListItem>
							<asp:ListItem Value="7">更换证件号</asp:ListItem>
							<asp:ListItem Value="9">手机令牌</asp:ListItem>
                            <asp:ListItem Value="10">第三方令牌</asp:ListItem>
							<asp:ListItem Value="20">实名认证</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<%--<TD align="right"><asp:label id="Label8" runat="server">帐号类型</asp:label></TD>
					<TD><asp:dropdownlist id="ddlQQType" runat="server" Width="152px">
							<asp:ListItem Value="">所有类型</asp:ListItem>
							<asp:ListItem Value="0">非会员</asp:ListItem>
							<asp:ListItem Value="1">普通会员</asp:ListItem>
							<asp:ListItem Value="2">VIP会员</asp:ListItem>
						</asp:dropdownlist></TD>--%>
					<td align="right">
						<asp:label id="Label9" runat="server">处理类别</asp:label></td>
					<td>
						<asp:dropdownlist id="DDL_DoType" runat="server" Width="152px" AutoPostBack="True">
							<asp:ListItem Value="9">所有</asp:ListItem>
							<asp:ListItem Value="0" Selected="True">人工审批</asp:ListItem>
							<asp:ListItem Value="1">高分单自动通过</asp:ListItem>
						</asp:dropdownlist></td>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label4" runat="server">申诉状态</asp:label></TD>
					<TD><!--没有真正的逻辑状态，只能根据其它字段判断--><asp:dropdownlist id="ddlState" runat="server" Width="152px">
							<asp:ListItem Value="99">所有状态</asp:ListItem>
							<asp:ListItem Value="0" Selected="True">未处理</asp:ListItem>
							<asp:ListItem Value="1">申诉成功</asp:ListItem>
							<asp:ListItem Value="2">申诉失败</asp:ListItem>
							<asp:ListItem Value="3">大额待复核</asp:ListItem>
							<asp:ListItem Value="4">直接转后台</asp:ListItem>
							<asp:ListItem Value="5">异常转后台</asp:ListItem>
							<asp:ListItem Value="6">发邮件失败</asp:ListItem>
							<asp:ListItem Value="7">已删除</asp:ListItem>
							<asp:ListItem Value="8">已锁定状态</asp:ListItem>
							<asp:ListItem Value="9">短信撤销状态</asp:ListItem>
							<asp:ListItem Value="10">直接申诉成功</asp:ListItem>
						</asp:dropdownlist><asp:dropdownlist id="ddlStateUserClass" runat="server" Width="152px">
							<asp:ListItem Value="99" Selected="True">全部</asp:ListItem>
							<asp:ListItem Value="0">未处理</asp:ListItem>
							<asp:ListItem Value="1">已锁定状态</asp:ListItem>
							<asp:ListItem Value="2">认证成功</asp:ListItem>
							<asp:ListItem Value="3">认证失败</asp:ListItem>
						</asp:dropdownlist><asp:label id="lblTotal" runat="server"></asp:label></TD>
					</TD>
					<TD align="right"><asp:label id="Label7" runat="server">批处理工单数</asp:label></TD>
					<td><asp:textbox id="txtCount" runat="server">50</asp:textbox>(最大值为:50)
						<asp:button id="btnGet" runat="server" Width="80px" Text="批量领单" onclick="btnGet_Click"></asp:button></td>
				</TR>
				<tr>
                  <TD align="right"><asp:label id="Label10" runat="server">排序类型</asp:label></TD>
					<TD><asp:dropdownlist id="ddlSortType" runat="server" Width="152px">
							<asp:ListItem Value="99">不排序</asp:ListItem>
							<asp:ListItem Value="0">时间小到大</asp:ListItem>
							<asp:ListItem Value="1">时间大到小</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD colSpan="2" align="center"><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
				</tr>
			</TABLE>
			<TABLE style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 205px; HEIGHT: 60%"
				id="Table2" border="1" cellSpacing="1" cellPadding="1" width="808" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" OnItemDataBound="DataGrid1_ItemDataBound">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="FUin" HeaderText="用户帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Femail" HeaderText="Email"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTypeName" HeaderText="申诉类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateName" HeaderText="申诉状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSubmitTime" HeaderText="申请时间" DataFormatString="{0:F}"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpicktime" HeaderText="审核时间" DataFormatString="{0:F}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCheckInfo" HeaderText="审核信息"></asp:BoundColumn>
                                <asp:BoundColumn DataField="balance" Visible="false" HeaderText="金额"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="详细内容">
									<ItemTemplate>
										<%--<a href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank>详细内容</a>--%>
                                        <asp:LinkButton id="queryButton" Visible="false" runat="server" CommandName="query"
                                            href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank 
                                             Text="详细内容"></asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
							 HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
