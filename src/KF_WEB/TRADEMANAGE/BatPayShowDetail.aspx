<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="BatPayShowDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BatPayShowDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BatPayShowDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 3%; WIDTH: 94%; POSITION: absolute; TOP: 5%; HEIGHT: 80%"
				cellSpacing="1" cellPadding="1" width="648" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colspan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><asp:label id="lbTitle" runat="server">数据明细</asp:label><FONT face="宋体"></FONT></SPAN></FONT></STRONG></TD>
				</TR>
				<TR>
					<TD colspan="2">
						<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="1">
							<TR>
								<TD>
									<asp:Label id="Label2" runat="server">当前状态</asp:Label></TD>
								<TD>
									<asp:DropDownList id="ddlState" runat="server">
										<asp:ListItem Value="9" Selected="True">所有状态</asp:ListItem>
										<asp:ListItem Value="0">已生成</asp:ListItem>
										<asp:ListItem Value="1">已提交付款</asp:ListItem>
										<asp:ListItem Value="2">付款成功</asp:ListItem>
										<asp:ListItem Value="3">付款失败</asp:ListItem>
										<asp:ListItem Value="5">成功无退票</asp:ListItem>
										<asp:ListItem Value="6">退票中</asp:ListItem>
										<asp:ListItem Value="7">已退票</asp:ListItem>
									</asp:DropDownList></TD>
								<TD>
									<asp:Label id="Label3" runat="server">用户姓名</asp:Label></TD>
								<TD>
									<asp:TextBox id="tbUserName" runat="server" BorderColor="Gray" BorderWidth="1px"></asp:TextBox></TD>
							</TR>
							<TR>
								<TD>
									<asp:Label id="Label4" runat="server">银行帐号</asp:Label></TD>
								<TD>
									<asp:TextBox id="tbBankAcc" runat="server"></asp:TextBox></TD>
								<TD>
									<asp:Label id="Label5" runat="server">付款金额</asp:Label></TD>
								<TD>
									<asp:TextBox id="tbCount" runat="server"></asp:TextBox></TD>
							</TR>
							<TR>
								<TD>
									<asp:Label id="Label1" runat="server">出款银行</asp:Label></TD>
								<TD>
									<asp:DropDownList id="ddlBankType" runat="server"></asp:DropDownList></TD>
								<TD colspan="2" align="center">
									<asp:Button id="Button2" runat="server" Text="查询记录" onclick="Button2_Click"></asp:Button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" colspan="2">
						<asp:DataGrid id="DataGrid1" runat="server" Width="100%" Height="122px" AutoGenerateColumns="False"
							BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3"
							GridLines="Horizontal">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="FSequence" HeaderText="汇总顺序号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTruename" HeaderText="真实姓名"></asp:BoundColumn>
								<asp:BoundColumn DataField="FBankAccNo" HeaderText="银行帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Famt1" HeaderText="付款金额"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStatusName" HeaderText="当前状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="FRTFlagName" HeaderText="退票"></asp:BoundColumn>
								<asp:BoundColumn DataField="FPayBankTypeName" HeaderText="出款银行"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTde_ID" HeaderText="付款表ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="FQQID" HeaderText="用户帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fnum1" HeaderText="金额">
									<ItemStyle BackColor="#66FFCC"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FStatusName1" HeaderText="状态">
									<ItemStyle BackColor="#66FFCC"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:DataGrid></TD>
				</TR>
				<TR>
					<TD colspan="2">
						<webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" PageSize="15"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
