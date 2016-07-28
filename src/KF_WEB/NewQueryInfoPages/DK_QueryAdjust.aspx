<%@ Page language="c#" Codebehind="DK_QueryAdjust.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.DK_QueryAdjust" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryForeignCard</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form style="FONT-FAMILY: 宋体" id="Form1" method="post" runat="server">
			<table border="0" cellSpacing="1" cellPadding="0" width="95%" align="center">
				<tr style="BACKGROUND-IMAGE: url(../Images/Page/bg_bl.gif)" bgColor="#e4e5f7">
					<td style="HEIGHT: 24px" colSpan="3"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><font color="#ff0000">
							代扣调账信息查询 </font>
					</td>
				</tr>
				<tr bgColor="lightgrey">
					<td>&nbsp;起始日期
					    <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</td>
					<td>&nbsp;结束日期
						<input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</td>
					<td>&nbsp;银行渠道<asp:dropdownlist id="ddlbanktype" Runat="server">
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
					<td>&nbsp;商家订单号<asp:textbox id="txbOrder" runat="server" Width="150px"></asp:textbox>
					</td>
					<td>&nbsp;财付通订单号<asp:textbox id="txbMoney" runat="server" Width="150px"></asp:textbox>
					</td>
				</tr>
				<tr bgColor="lightgrey">
					<td>&nbsp;银行卡号<asp:textbox id="txtBankOrder" runat="server" Width="150px"></asp:textbox>
					</td>
					<td>&nbsp;用户名<asp:textbox id="Textbox1" runat="server" Width="150px"></asp:textbox>
					</td>
					<td style="PADDING-RIGHT: 150px" align="right"><asp:button id="btQuery" runat="server" Width="108px" Height="27px" Text="查 询" BorderStyle="Groove" onclick="btQuery_Click"></asp:button></td>
				</tr>
				<tr>
					<td colSpan="3"><asp:datagrid id="dgInfo" runat="server" Width="100%" BorderStyle="None" AutoGenerateColumns="False"
							HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PageSize="100"
							GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderColor="#E7E7FF">
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<Columns>
								<asp:BoundColumn DataField="Fcoding" HeaderText="商户订单号">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_list" HeaderText="财付通订单号">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fspid" HeaderText="商户号">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_typename" HeaderText="银行渠道">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fbankacc_no" HeaderText="银行卡号">
									<HeaderStyle Wrap="False"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Funame" HeaderText="用户名">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fpaynumname" HeaderText="订单金额">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fendstatename" HeaderText="调整类型">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcheckstatename" HeaderText="处理状态">
									<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
