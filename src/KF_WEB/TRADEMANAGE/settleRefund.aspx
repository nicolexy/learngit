<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="settleRefund.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.settleRefund.settleRefund" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SeparateListQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE cellSpacing="1" cellPadding="1" width="900" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;分账退款查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:radiobutton id="rtnList" Runat="server" Text="根据订单号查询" GroupName="rtnChoose" Width="123px"></asp:radiobutton></TD>
					<td colSpan="3"><asp:label id="Label7" runat="server">财付通订单号</asp:label><asp:textbox id="txtFlistid" Width="400px" Runat="server"></asp:textbox></td>
				</TR>
				<tr>
					<TD align="right"><asp:radiobutton id="rtnRefefundList" Runat="server" Text="根据退款申请单查询" GroupName="rtnChoose"></asp:radiobutton></TD>
					<td colSpan="3"><asp:label id="Label2" runat="server">退款申请单号</asp:label><asp:TextBox id="txtRefefundList" runat="server" Width="400px"></asp:TextBox></td>
				</tr>
				<tr>
					<TD align="center" colSpan="4"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button></TD>
				</tr>
			</TABLE>
			<br>
			<br>
			<table cellSpacing="0" cellPadding="0" width="900" border="0">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="900" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
							HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1"
							BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="listid" HeaderText="财付通订单号">
									<HeaderStyle Width="250px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="refund_id" HeaderText="退款申请单号">
									<HeaderStyle Width="250px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="modify_time" HeaderText="最后修改时间">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="rp_fee" HeaderText="退款金额">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="status" HeaderText="状态">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="详细">
									<ItemTemplate>
										<a href= '<%# "SettleRefundDetail.aspx?refund_id="	+DataBinder.Eval(Container, "DataItem.refund_id") + "&listid=" + DataBinder.Eval(Container, "DataItem.listid")%>' >详细</a>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always"
							CssClass="mypager" HorizontalAlign="right" OnPageChanged="pager_PageChanged" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"
							AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
