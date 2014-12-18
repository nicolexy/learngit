<%@ Page language="c#" Codebehind="TranLogIncomeSum.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TranLogIncomeSum" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TranLogIncomeSum</title>
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

				if(returnValue != null)
				    Form1.txtBeginDate.value=returnValue;
			}
			function openModeEnd()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.txtEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');

				if(returnValue != null)
				    Form1.txtEndDate.value=returnValue;
			}

		</script>
	</HEAD>
	<body id="bodyid" runat="server">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="820" border="1">
				<tr>
					<td align="center"><asp:label id="Label9" runat="server" CssClass="title_info">交易汇总数据</asp:label></td>
				</tr>
				<TR>
					<td><asp:label id="lblBeginDate" runat="server">开始日期</asp:label><asp:textbox id="txtBeginDate" runat="server"></asp:textbox><asp:imagebutton id="btnBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>&nbsp;
						<asp:label id="lblEndDate" runat="server">结束日期</asp:label><asp:textbox id="txtEndDate" runat="server"></asp:textbox><asp:imagebutton id="btnEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton><STRONG></STRONG>&nbsp;&nbsp;渠道
						<asp:dropdownlist id="DropDownListChannel" runat="server" AutoPostBack="True"></asp:dropdownlist>&nbsp;&nbsp;商户号
						<asp:TextBox id="TextBoxSpid" runat="server"></asp:TextBox>&nbsp;
						<asp:Button ID="btnSearch" Runat="server" Text="查询" onclick="btnSearch_Click"></asp:Button></td>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 820px; POSITION: absolute; TOP: 100px; HEIGHT: 500px">
				<table cellSpacing="0" cellPadding="0" border="0">
					<TR>
						<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="820px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" PageSize="15" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1"
								BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="True">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Faccountdate" HeaderText="日期">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FSpid" HeaderText="商户">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FChannelNo" HeaderText="渠道">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FProductType" HeaderText="产品">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FTransactionCount" HeaderText="明细汇&lt;br&gt;总笔数">
										<HeaderStyle Width="60px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FTransactionAmount" HeaderText="明细汇&lt;br&gt;总金额">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FAddCountA" HeaderText="Q币数">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FOutNumber" HeaderText="导入&lt;br&gt;总笔数">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FOutAmount" HeaderText="导入&lt;br&gt;总金额">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FRecordStatus" HeaderText="状态">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FAgentSPId" HeaderText="平台商">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
						</TD>
					</TR>
				</table>
			</div>
		</form>
	</body>
</HTML>
