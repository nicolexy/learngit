<%@ Page language="c#" Codebehind="ChangeQQOld.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.C2C.KF.KF_Web.BaseAccount.ChangeQQOld" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ChangeQQOld</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style4 { COLOR: #ff0000 }
	.style5 { COLOR: #ff0000; FONT-WEIGHT: bold }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; POSITION: absolute; WIDTH: 74%; TOP: 4%; LEFT: 13%"
				cellSpacing="1" cellPadding="1" border="1">
				<TR bgColor="#eeeeee">
					<TD colSpan="4" height="7">
						<P align="left">&nbsp;<FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;修改帐号</STRONG></SPAN></FONT><FONT color="#ff0000">&nbsp;&nbsp;&nbsp;</FONT>
						</P>
					</TD>
				</TR>
				<TR>
					<td><asp:label id="Label2" runat="server">旧帐号:</asp:label></td>
					<TD style="HEIGHT: 26px"><asp:textbox id="OldQQ" runat="server"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="请输入帐号" ControlToValidate="OldQQ"></asp:requiredfieldvalidator></TD>
					<TD style="HEIGHT: 26px"><asp:label id="Label4" runat="server">新帐号</asp:label></TD>
					<TD><asp:textbox id="NewQQ" runat="server"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ErrorMessage="请输入帐号" ControlToValidate="NewQQ"></asp:requiredfieldvalidator></TD>
				</TR>
				<tr>
					<td>
						<asp:Label id="Label3" runat="server">修改原因</asp:Label></td>
					<td colspan="3">
						<asp:TextBox id="tbMemo" runat="server" Width="506px"></asp:TextBox></td>
				</tr>
				<tr>
					<td colspan="4" align="center"><asp:button id="btnChangeQQ" runat="server" Width="94px" Text="修改" onclick="btnChangeQQ_Click"></asp:button></td>
				</tr>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; POSITION: absolute; WIDTH: 74%; TOP: 27%; LEFT: 13%"
				cellSpacing="1" cellPadding="1" border="1">
				<TR bgColor="#eeeeee">
					<TD colSpan="4" height="7">
						<P align="left">&nbsp;<FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;查询修改帐号历史</STRONG></SPAN>&nbsp;&nbsp;&nbsp;</FONT>
						</P>
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 26px"><asp:label id="Label5" runat="server">修改帐号</asp:label></TD>
					<TD style="HEIGHT: 26px"><asp:textbox id="tbqueryQQ" runat="server"></asp:textbox></TD>
					<TD style="HEIGHT: 26px"><asp:label id="Label7" runat="server">操作人员</asp:label></TD>
					<TD><asp:textbox id="tbUserID" runat="server"></asp:textbox></TD>
				</TR>
				<tr>
					<td align="center" colSpan="4"><asp:button id="btnQuery" runat="server" Width="94px" Text="查询" CausesValidation="False" onclick="btnQuery_Click"></asp:button></td>
				</tr>
			</TABLE>
			<TABLE id="Table3" style="Z-INDEX: 103; POSITION: absolute; WIDTH: 74%; TOP: 50%; LEFT: 13%"
				cellSpacing="1" cellPadding="1" border="1" runat="server">
				<TR>
					<TD>
						<asp:DataGrid id="DataGrid1" runat="server" Width="100%" BorderColor="#999999" BorderWidth="1px"
							BorderStyle="None" BackColor="White" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False"
							EnableViewState="False">
							<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#DCDCDC"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="FOldQQ" HeaderText="旧帐号">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FNewQQ" HeaderText="新帐号">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FUserID" HeaderText="操作人员">
									<HeaderStyle Width="14%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FActionTime" HeaderText="操作时间">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FMemo" HeaderText="备注"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
						</asp:DataGrid></TD>
				</TR>
				<TR>
					<TD>
						<webdiyer:aspnetpager id="AspNetPager1" runat="server" NumericButtonCount="10" PagingButtonSpacing="0"
							ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"
							ShowCustomInfoSection="left" AlwaysShow="True"></webdiyer:aspnetpager></FONT></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
