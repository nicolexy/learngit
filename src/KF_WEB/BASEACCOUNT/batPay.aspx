<%@ Page language="c#" Codebehind="batPay.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.batPay"  %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>batPay</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body background="../IMAGES/Page/bg01.gif" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table3" style="Z-INDEX: 101; LEFT: 5%; WIDTH: 80%; POSITION: absolute; TOP: 5%; HEIGHT: 80%"
				borderColor="#666666" height="127" cellSpacing="1" cellPadding="1" width="383" align="center"
				border="1">
				<TR>
					<TD bgColor="#e4e5f7" style="WIDTH: 100%;HEIGHT: 4px" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;汇总付款数据</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 250px" vAlign="top" align="center" width="100%" colSpan="2" height="0"><FONT face="宋体">
							<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="1">
								<TR>
									<TD colSpan="3"><asp:datagrid id="DataGrid1" runat="server" GridLines="Horizontal" AutoGenerateColumns="False"
											CellPadding="3" BackColor="White" BorderColor="#E7E7FF" BorderWidth="1px" BorderStyle="None" Width="100%"
											EnableViewState="False">
											<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
											<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
											<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
											<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
											<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="FDate" HeaderText="日期"></asp:BoundColumn>
												<asp:BoundColumn DataField="FBankID" HeaderText="银行"></asp:BoundColumn>
												<asp:BoundColumn DataField="FStatusName" HeaderText="当前状态"></asp:BoundColumn>
												<asp:HyperLinkColumn DataTextField="Detail" DataNavigateUrlField="FUrl" DataNavigateUrlFormatString="batpayDetail.Aspx?{0}"
													HeaderText="详细">
													<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle Font-Underline="True" HorizontalAlign="Center" ForeColor="Blue" VerticalAlign="Middle"></ItemStyle>
												</asp:HyperLinkColumn>
											</Columns>
											<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
							</TABLE>
						</FONT>
					</TD>
				</TR>
				<TR>
					<TD align="center" height="25" rowSpan="1"><asp:label id="Label2" runat="server">选择日期</asp:label><asp:textbox id="TextBoxBeginDate" runat="server" BorderColor="Gray" BorderWidth="1px"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
					<td><asp:button id="Button1" runat="server" Text="取得最新状态"></asp:button></td>
				</TR>
			</TABLE>
			</FONT></form>
	</body>
</HTML>
