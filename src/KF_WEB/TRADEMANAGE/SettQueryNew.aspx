<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="SettQueryNew.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SettQueryNew" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SettQueryNew</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script language="javascript">
			function openModeBegin()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.txtBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null) Form1.txtBeginDate.value=returnValue;
			}
			
			function openModeEnd()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.txtEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null)
				{
				    Form1.txtEndDate.value=returnValue;
				}
			}
		</script>
	</HEAD>
	<body id="bodyid" runat="server">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;结算查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<tr>
					<TD align="right"><asp:label id="Label2" runat="server">开始日期</asp:label></TD>
					<TD><asp:textbox id="txtBeginDate" runat="server"></asp:textbox><asp:imagebutton id="btnBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
					<TD align="right"><asp:label id="Label5" runat="server">结束日期</asp:label></TD>
					<TD><asp:textbox id="txtEndDate" runat="server"></asp:textbox><asp:imagebutton id="btnEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
				</tr>
				<TR>
					<TD align="right"><asp:label id="Label3" runat="server">商户号</asp:label></TD>
					<TD><asp:textbox id="txtSpid" runat="server"></asp:textbox></TD>
					<td align="center" colSpan="2"><asp:button id="btnSearch" Width="100px" Runat="server" Text="查询" onclick="btnSearch_Click"></asp:button></td>
				</TR>
				<tr>
					<TD bgColor="#e4e5f7" colSpan="4"><FONT color="red">&nbsp;&nbsp;未结算查询(与起止日期无关)</FONT>
					</TD>
				</tr>
				<tr>
					<td colSpan="4"><asp:label id="Label4" runat="server">未结算合计</asp:label></td>
				</tr>
				<tr>
					<td align="right"><asp:label id="Label6" runat="server">交易笔数:</asp:label></td>
					<td><asp:label id="lblBalanceCount" Runat="server"></asp:label></td>
					<td align="right"><asp:label id="Label7" runat="server">交易额:</asp:label></td>
					<td><asp:label id="lblBalance" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td colSpan="4"><asp:label id="Label8" runat="server">今日未结算统计</asp:label></td>
				</tr>
				<tr>
					<td align="right"><asp:label id="Label9" runat="server">成功交易笔数:</asp:label></td>
					<td align="left"><asp:label id="lblBalanceSuccessCount" Runat="server"></asp:label></td>
					<td align="right"><asp:label id="Label10" runat="server">成功交易额:</asp:label></td>
					<td align="left"><asp:label id="lblBalanceSuccess" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td align="right"><asp:label id="Label11" runat="server">退款笔数:</asp:label></td>
					<td><asp:label id="lblRefundCount" Runat="server"></asp:label></td>
					<td align="right"><asp:label id="Label12" runat="server">退款金额:</asp:label></td>
					<td><asp:label id="lblRefund" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td colSpan="4"><asp:label id="Label13" runat="server">往日未结算统计</asp:label></td>
				</tr>
				<tr>
					<td align="right"><asp:label id="Label14" runat="server">交易笔数:</asp:label></td>
					<td><asp:label id="lblHisBalanceCount" Runat="server"></asp:label></td>
					<td align="right"><asp:label id="Label15" runat="server">交易额:</asp:label></td>
					<td align="left"><asp:label id="lblHisBalance" Runat="server"></asp:label></td>
				</tr>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 820px; POSITION: absolute; TOP: 290px; HEIGHT: 180px">
				<table cellSpacing="0" cellPadding="0" width="820" border="0">
					<tr>
						<TD vAlign="top" align="center" colSpan="4"><asp:datagrid id="UnbalancedgList" runat="server" Width="900px" PageSize="5" AllowPaging="True"
								BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
								HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FCalculateNo" HeaderText="序号" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn HeaderText="序号">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FChannelNo" HeaderText="渠道">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FFeeItem" HeaderText="项目">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FCurrentDate" HeaderText="交易开始日期">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FNextDate" HeaderText="交易结束日期">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FTransactionCount" HeaderText="结算笔数">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FTransactionAmount" HeaderText="结算金额">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FProductType" HeaderText="交易类别">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FAddCountA" HeaderText="交易数量">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
				</table>
			</div>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 820px; POSITION: absolute; TOP: 470px; HEIGHT: 150px">
				<table cellSpacing="0" cellPadding="0" width="820" border="0">
					<TR>
						<TD bgColor="#e4e5f7" colSpan="4"><FONT color="red">&nbsp;&nbsp;已结算查询(与起止日期相关)</FONT>
						</TD>
					</TR>
					<tr>
						<TD vAlign="top" align="center" colSpan="4"><asp:datagrid id="dgList" runat="server" Width="820px" PageSize="5" AllowPaging="True" BorderColor="#E7E7FF"
								BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False" HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FNo" HeaderText="序号" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn HeaderText="序号">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FCreateTime" HeaderText="划账日期">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Transfer" HeaderText="划账金额">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Balance" HeaderText="结算金额">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Poundage" HeaderText="手续费金额">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:ButtonColumn DataTextField="FPriTypeStr" HeaderText="结算详情" CommandName="Select">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:ButtonColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
				</table>
			</div>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 820px; POSITION: absolute; TOP: 610px; HEIGHT: 180px">
				<table id="tableDetail" cellSpacing="0" cellPadding="0" width="820" border="0" runat="server">
					<TR>
						<TD bgColor="#e4e5f7" colSpan="4"><FONT color="red">&nbsp;&nbsp;已结算详情</FONT>
						</TD>
					</TR>
					<tr>
						<TD vAlign="top" align="center" colSpan="4"><asp:datagrid id="dgListDetail" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="True" PageSize="5">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FCalculateNo" HeaderText="序号" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn HeaderText="序号">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FChannelNo" HeaderText="渠道">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FFeeItem" HeaderText="项目">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FCurrentDate" HeaderText="交易开始日期">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FNextDate" HeaderText="交易结束日期">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FTransactionCount" HeaderText="结算笔数">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FTransactionAmount" HeaderText="结算金额">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FFeeStandard" HeaderText="费率">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FPerMolecule" HeaderText="分子" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn DataField="FPerDenominator" HeaderText="分母" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn DataField="FDueAmount" HeaderText="付款金额">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FProductType" HeaderText="交易类别">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FAddCountA" HeaderText="交易数量">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
