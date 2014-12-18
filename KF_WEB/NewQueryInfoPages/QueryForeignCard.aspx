<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryForeignCard.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryForeignCard" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryForeignCard</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
					}
		</script>
		<script language="javascript">
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxEndDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form style="FONT-FAMILY: 宋体" id="Form1" method="post" runat="server">
			<table border="0" cellSpacing="1" cellPadding="0" width="95%" align="center">
				<tr style="BACKGROUND-IMAGE: url(../Images/Page/bg_bl.gif)" bgColor="#e4e5f7">
					<td style="HEIGHT: 24px" colSpan="3"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><font color="#ff0000">
							外卡订单交易查询 </font>
					</td>
				</tr>
				<tr bgColor="lightgrey">
					<td>&nbsp;起始日期
						<asp:textbox id="TextBoxBeginDate" runat="server" Width="122px"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton>&nbsp;&nbsp;&nbsp;&nbsp;
					</td>
					<td>&nbsp;结束日期
						<asp:textbox id="TextBoxEndDate" runat="server" Width="122px"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></td>
					<td>&nbsp;支付情况<asp:dropdownlist id="DropDownState" Runat="server">
							<asp:ListItem Value="all" Selected="True">所有</asp:ListItem>
							<asp:ListItem Value="unpay">未支付</asp:ListItem>
							<asp:ListItem Value="payed">支付成功</asp:ListItem>
							<asp:ListItem Value="refund">退款</asp:ListItem>
							<asp:ListItem Value="waiting">待处理</asp:ListItem>
						</asp:dropdownlist>
					</td>
				</tr>
				<tr bgColor="lightgrey">
					<td>&nbsp;商户号&nbsp;<asp:textbox id="txbMerchant" runat="server" Width="150px"></asp:textbox>
					</td>
					<td>&nbsp;交易流水订单号<asp:textbox id="txbOrder" runat="server" Width="150px"></asp:textbox>
					</td>
					<td>&nbsp;交易金额<asp:textbox id="txbMoney" runat="server" Width="150px"></asp:textbox>
					</td>
				</tr>
				<tr bgColor="lightgrey">
					<td>&nbsp;银行订单号<asp:textbox id="txtBankOrder" runat="server" Width="150px"></asp:textbox>
					</td>
					<td style="PADDING-RIGHT: 150px" colSpan="2" align="right"><asp:button id="btQuery" runat="server" Width="108px" Height="27px" Text="查 询" BorderStyle="Groove"></asp:button></td>
				</tr>
				<tr>
					<td colSpan="3"><asp:datagrid id="dgInfo" runat="server" Width="100%" BorderStyle="None" AutoGenerateColumns="False"
							HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PageSize="5"
							GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Ftransaction_id" HeaderText="流水订单号">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcoding" HeaderText="商户订单号">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fspid" HeaderText="商户号">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="TradeState" HeaderText="支付情况">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fdesc" HeaderText="支付信息">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_currency_fee" HeaderText="订单总额">
									<HeaderStyle Wrap="False"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="BankType" HeaderText="支付银行">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fpay_time" HeaderText="支付时间">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fsp_currency_refund_fee" HeaderText="退款金额">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Frefund_time" HeaderText="退款时间">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_listid" HeaderText="银行订单号">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
