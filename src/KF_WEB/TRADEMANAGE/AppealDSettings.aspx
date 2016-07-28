<%@ Page language="c#" Codebehind="AppealDSettings.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.AppealDSettings" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundMain</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="900" border="1" align="center">
				<TR bgColor="#eeeeee">
					<TD colSpan="5"><FONT color="#ff0000"><IMG src="../IMAGES/Page/post.gif" width="15"><asp:label id="lbTitle" runat="server">提现规则查询</asp:label></FONT>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:label id="Label1" runat="server" ForeColor="ControlText">操作员代码：</asp:label><FONT color="#ff0000"><asp:label id="Label_uid" runat="server">Label</asp:label></FONT>
					</TD>
				</TR>
				<tr>
					<TD><FONT face="宋体">商户号 </FONT>
						<asp:textbox id="tb_spid" runat="server" Width="96px"></asp:textbox></TD>
					<TD><FONT face="宋体">用户帐号
							<asp:textbox id="tb_user" runat="server" Width="96px"></asp:textbox></FONT></TD>
					<TD><FONT face="宋体">规则类型</FONT>
						<asp:dropdownlist id="ddl_pritype" runat="server" Width="179px">
							<asp:ListItem Value="-1" Selected="True">不限</asp:ListItem>
							<asp:ListItem Value="1">结算金额全额转指定对方账户</asp:ListItem>
							<asp:ListItem Value="2">结算金额全额转银行帐户</asp:ListItem>
							<asp:ListItem Value="3">结算金额全额留存本帐户</asp:ListItem>
							<asp:ListItem Value="4">帐户余额全额转银行帐户</asp:ListItem>
							<asp:ListItem Value="5">帐户余额全额转指定对方帐户</asp:ListItem>
							<asp:ListItem Value="6">帐户余额留存指定金额，其余转对方帐户</asp:ListItem>
							<asp:ListItem Value="7">帐户余额留存指定金额，其余转银行帐户</asp:ListItem>
							<asp:ListItem Value="8">部分转对方帐户，使对方至指定金额，其余转银行帐户</asp:ListItem>
							<asp:ListItem Value="9">转制定金额至对方帐户</asp:ListItem>
							<asp:ListItem Value="10">T+0提现</asp:ListItem>
						</asp:dropdownlist></FONT></TD>
					<TD>
						<P><FONT face="宋体">状态
								<asp:dropdownlist id="ddl_state" runat="server" Width="96px">
									<asp:ListItem Value="-1">不限</asp:ListItem>
									<asp:ListItem Value="1">商户申请</asp:ListItem>
									<asp:ListItem Value="2">商户复核</asp:ListItem>
									<asp:ListItem Value="3">财付通审批通过</asp:ListItem>
									<asp:ListItem Value="4">拒绝</asp:ListItem>
									<asp:ListItem Value="5">作废</asp:ListItem>
								</asp:dropdownlist></FONT></P>
					</TD>
					<TD><FONT face="宋体">&nbsp;<asp:button id="btn_query" runat="server" Width="58px" BackColor="InactiveCaptionText" Text="查询" onclick="btn_query_Click"></asp:button></FONT></TD>
				</tr>
				<TR>
					<TD colSpan="5"><FONT face="宋体"><asp:label id="lb_msg" runat="server" Width="720px" ForeColor="Red"></asp:label>&nbsp;&nbsp;</FONT></TD>
				</TR>
				<tr>
					<td colSpan="5"><asp:datagrid id="DataGrid1" runat="server" Width="100%" GridLines="Horizontal" AutoGenerateColumns="False"
							CellPadding="3" BackColor="White" BorderStyle="None" BorderColor="#E7E7FF" BorderWidth="1px" Height="122px">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fno" HeaderText="序号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSpid" HeaderText="商户号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSpidName" HeaderText="商户名"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fuser" HeaderText="用户帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FuserName" HeaderText="用户名"></asp:BoundColumn>
								<asp:BoundColumn DataField="FPriTypeS" HeaderText="规则类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="FAmountSetS" HeaderText="指定金额(分)"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserIn" HeaderText="转帐帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserNameIn" HeaderText="转帐帐号名"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateS" HeaderText="状态"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td colSpan="5"><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" PageSize="15"></webdiyer:aspnetpager></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
