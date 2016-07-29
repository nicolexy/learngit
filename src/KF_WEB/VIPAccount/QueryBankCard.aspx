<%@ Page language="c#" Codebehind="QueryBankCard.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.VIPAccount.QueryBankCard" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryBankCard</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="VBScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
UNKNOWN {
	COLOR: #000000
}
.style3 {
	COLOR: #ff0000
}
BODY {
	BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif)
}
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script language="javascript" type="text/javascript">
		function CheckCardAccount()
		{
			var account = document.getElementById("tbx_acc").value
			var newAcc=/^\d+$/;
			if(newAcc.test(account) == false)
			{
				alert("QQ财付通账号请输入数字");
				return false;
			}
			var card = document.getElementById("tbx_card").value;
			if(card == "") return true;
			var newPar=/^\d{4}$/;
			if(newPar.test(card) == false)
			{
				alert("卡号输入不正确");
				return false;
			}
			return true;
		}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form style="FONT-FAMILY: 宋体" id="Form1" method="post" runat="server">
			<table style="POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="800">
				<TBODY>
					<tr style="BACKGROUND-COLOR: #e4e5f7">
						<td colSpan="5"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;银行卡查询</FONT>
						</td>
					</tr>
					<tr>
						<td width="100"><label>QQ财付通账号:</label>
						</td>
						<td><asp:textbox id="tbx_acc" Runat="server" Width="200px"></asp:textbox></td>
						<td width="80"><label>银行卡后四位:</label>
						</td>
						<td><asp:textbox id="tbx_card" Runat="server" Width="200px" MaxLength="4"></asp:textbox></td>
						<td><asp:button id="btn_query" Runat="server" Width="100px" Text="查询"></asp:button></td>
					</tr>
					<tr>
						<td colSpan="5"><asp:datagrid id="DataGrid_CardBind" runat="server" Width="100%" ItemStyle-HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
								GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Fname" HeaderText="姓名" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="banktypeName" HeaderText="银行类型" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_id" HeaderText="卡号后四位">
										<HeaderStyle Width="200px" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FstateName" HeaderText="状态">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="time" HeaderText="修改时间">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid></td>
					</tr>
					<tr>
					</tr>
					<tr>
						<td colSpan="5"><asp:datagrid id="DataGrid_Order" runat="server" Width="100%" ItemStyle-HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
								GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Ftrans_id" HeaderText="单据ID" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fpay_value" HeaderText="消费积分">
										<HeaderStyle Width="120px" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_id" HeaderText="银行卡尾号">
										<HeaderStyle Width="120px" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmod_time" HeaderText="交易时间">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fdesc_string" HeaderText="备注">
										<HeaderStyle Width="240px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="payStateName" HeaderText="订单状态">
										<HeaderStyle Width="240px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid></td>
					</tr>
				</TBODY>
			</table>
		</form>
	</body>
</HTML>
