<%@ Page language="c#" Codebehind="BatPayQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BatPayQuery" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BatPayQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
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
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="900"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="3"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;汇总付款数据</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 126px" vAlign="top" align="center" width="100%" colSpan="3" height="0"><FONT face="宋体">
							<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="99%" border="1">
								<TR>
									<TD colSpan="3"><asp:datagrid id="DataGrid1" runat="server" EnableViewState="False" Width="100%" BorderStyle="None"
											BorderWidth="1px" BorderColor="#E7E7FF" BackColor="White" CellPadding="3" AutoGenerateColumns="False"
											GridLines="Horizontal">
											<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
											<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
											<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
											<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
											<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
											<Columns>
											<asp:HyperLinkColumn DataNavigateUrlField="FUrl" DataNavigateUrlFormatString="BatPayShowDetail.Aspx?{0}"  Text="详细"
													HeaderText="总笔数">
													<ItemStyle Font-Underline="True" ForeColor="Blue"></ItemStyle>
												</asp:HyperLinkColumn>
												<asp:BoundColumn DataField="FDate" HeaderText="日期"></asp:BoundColumn>
												<asp:BoundColumn DataField="FBatchOrder" HeaderText="批次"></asp:BoundColumn>
												<asp:BoundColumn DataField="FBankID" HeaderText="银行"></asp:BoundColumn>
												<asp:BoundColumn DataField="FStatusName" HeaderText="当前状态"></asp:BoundColumn>
												<asp:BoundColumn DataField="FMsg" HeaderText="操作提示"></asp:BoundColumn>
											</Columns>
											<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
							</TABLE>
						</FONT>
					</TD>
				</TR>
				<TR>
					<TD align="center" height="25" rowSpan="1"><asp:label id="Label2" runat="server">选择日期</asp:label><asp:textbox id="TextBoxBeginDate" runat="server" BorderWidth="1px" BorderColor="Gray"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
					<TD>
						<asp:Label id="Label3" runat="server">选择批次</asp:Label>
						<asp:TextBox id="txBatchOrder" runat="server" Width="111px">1</asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txBatchOrder"></asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="数字" ControlToValidate="txBatchOrder"
							ValidationExpression="\d{1,2}"></asp:RegularExpressionValidator>
						<asp:RangeValidator id="RangeValidator1" runat="server" ErrorMessage="1->77" ControlToValidate="txBatchOrder"
							MaximumValue="77" MinimumValue="1" Type="Integer"></asp:RangeValidator></TD>
					<td><asp:button id="Button1" runat="server" Text="取得最新状态"></asp:button></td>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
