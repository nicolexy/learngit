<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="BankBillNoQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BankBillNoQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>BankBillNoQuery</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="VBScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script language="javascript">
					function openModeBegin()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_beginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.tbx_beginDate.value=returnValue;
					}
		</script>
</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form style="FONT-FAMILY: 宋体" id="Form1" method="post" runat="server">
			<table style="POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="800">
				<TBODY>
					<tr style="BACKGROUND-COLOR: #e4e5f7">
						<td colSpan="4"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">银行订单号查询</FONT>
						</td>
					</tr>
					<tr>
						<td width="80"><label>银行订单编码:</label>
						</td>
						<td><asp:textbox id="tbx_acc" Width="250px" Runat="server"></asp:textbox></td>
                        <td width="100"><label>日期:</label>
						</td>
						<td><asp:textbox id="tbx_beginDate" Width="120" Runat="server"></asp:textbox><asp:imagebutton id="btnBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></td>
					</tr>
                    <tr>
						<td width="80"><label>银行类型:</label>
						</td>
						<td><asp:textbox id="tbx_bankType" Width="250px" Runat="server"></asp:textbox></td>
                        <TD align="right"><asp:label id="Label8" runat="server">业务类型</asp:label></TD>
					    <TD><asp:dropdownlist id="ddlBiuType" runat="server" Width="152px">
							<asp:ListItem Value="10100">支付</asp:ListItem>
							<asp:ListItem Value="10200">提现</asp:ListItem>
							<asp:ListItem Value="10300">退款</asp:ListItem>
							<asp:ListItem Value="10400">ATM充值</asp:ListItem>
                            <asp:ListItem Value="10500">session索引</asp:ListItem>
						</asp:dropdownlist></TD>
					</tr>
					<tr>
						<td colSpan="4" align="center"><asp:button id="btn_query" Width="100px" Runat="server" Text="查询" onclick="btn_query_Click"></asp:button></td>
					</tr>
					<tr>
						<td colSpan="4"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="800px" ItemStyle-HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
								GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="fbill_no" HeaderText="账号" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></td>
					</tr>
					<tr height="25">
						<td colspan="4"><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" ShowCustomInfoSection="left"
								NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always"
								PagingButtonSpacing="0" NumericButtonCount="10"></webdiyer:aspnetpager>
						</td>
					</tr>
				</TBODY>
			</table>
		</form>
	</body>
</HTML>
