<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="RefundQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.RefundQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundMain</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体">
				<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 3%; WIDTH: 94%; POSITION: absolute; TOP: 5%; HEIGHT: 90%"
					cellSpacing="1" cellPadding="1" width="648" border="1">
					<TR bgColor="#eeeeee">
						<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><asp:label id="lbTitle" runat="server">数据明细</asp:label><FONT face="宋体"></FONT></SPAN></FONT></STRONG></TD>
					</TR>
					<TR>
						<TD colSpan="2">
							<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="1">
								<TR>
									<TD><asp:label id="Label2" runat="server">选择日期</asp:label></TD>
									<TD>
                                        <asp:textbox id="TextBoxBeginDate" runat="server" Width="100px" BorderStyle="Groove"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
                                        </TD>
									<TD><asp:label id="Label1" runat="server">退款银行</asp:label></TD>
									<TD><asp:dropdownlist id="ddlBankType" runat="server"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD><asp:label id="Label3" runat="server">数据来源</asp:label></TD>
									<TD><asp:dropdownlist id="ddlFromType" runat="server">
											<asp:ListItem Value="9" Selected="True">所有来源</asp:ListItem>
											<asp:ListItem Value="1">商户退单</asp:ListItem>
											<asp:ListItem Value="2">对帐结果退单</asp:ListItem>
											<asp:ListItem Value="3">人工录入退单</asp:ListItem>
										</asp:dropdownlist></TD>
									<TD><asp:label id="Label4" runat="server">退款方式</asp:label></TD>
									<TD><asp:dropdownlist id="ddlRefundType" runat="server">
											<asp:ListItem Value="9" Selected="True">所有方式</asp:ListItem>
											<asp:ListItem Value="1">网银退单</asp:ListItem>
											<asp:ListItem Value="2">接口退单</asp:ListItem>
											<asp:ListItem Value="3">人工授权</asp:ListItem>
											<asp:ListItem Value="4">转帐退单</asp:ListItem>
											<asp:ListItem Value="5">转入代发</asp:ListItem>
											<asp:ListItem Value="6">付款退款</asp:ListItem>
										</asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD><asp:label id="Label5" runat="server">退款状态</asp:label></TD>
									<TD><asp:dropdownlist id="ddlRefundState" runat="server">
											<asp:ListItem Value="9" Selected="True">所有状态</asp:ListItem>
											<asp:ListItem Value="0">初始状态</asp:ListItem>
											<asp:ListItem Value="1">退单流程中</asp:ListItem>
											<asp:ListItem Value="2">退单成功</asp:ListItem>
											<asp:ListItem Value="3">退单失败</asp:ListItem>
											<asp:ListItem Value="4">退单状态未定</asp:ListItem>
											<asp:ListItem Value="5">手工退单中</asp:ListItem>
											<asp:ListItem Value="6">申请手工退单</asp:ListItem>
											<asp:ListItem Value="7">申请转入代发</asp:ListItem>
										</asp:dropdownlist></TD>
									<TD><asp:label id="Label6" runat="server">回导状态</asp:label></TD>
									<TD><asp:dropdownlist id="ddlReturnState" runat="server">
											<asp:ListItem Value="9" Selected="True">所有状态</asp:ListItem>
											<asp:ListItem Value="1">回导前</asp:ListItem>
											<asp:ListItem Value="2">回导后</asp:ListItem>
										</asp:dropdownlist></TD>
								</TR>
                                <TR>
									<TD><asp:label id="Label8" runat="server">选择场次</asp:label></TD>
									<TD><asp:dropdownlist id="ddlFbatchid" runat="server">
											<asp:ListItem Value="0" Selected="True">选择场次</asp:ListItem>
											<asp:ListItem Value="R">1</asp:ListItem>
											<asp:ListItem Value="T">2</asp:ListItem>
											<asp:ListItem Value="U">3</asp:ListItem>
											<asp:ListItem Value="W">4</asp:ListItem>
											<asp:ListItem Value="Y">5</asp:ListItem>
										</asp:dropdownlist></TD>
									<TD><asp:label id="Label9" runat="server">给银行订单号</asp:label></TD>
                                    <TD><asp:TextBox id="tbFbank_listid" runat="server"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD><asp:label id="Label7" runat="server">退款单ID</asp:label></TD>
									<TD><asp:textbox id="tbListID" runat="server"></asp:textbox></TD>
									<TD align="center" colSpan="2"><asp:button id="btnQuery" runat="server" Text="查询记录" onclick="btnQuery_Click"></asp:button></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD vAlign="top" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BorderStyle="None" Height="122px" AutoGenerateColumns="False"
								BackColor="White" CellPadding="3" GridLines="Horizontal" BorderColor="#E7E7FF" BorderWidth="1px">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FPaylistid" HeaderText="退款单"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_listid" HeaderText="订单号"></asp:BoundColumn>
									<asp:BoundColumn DataField="FreturnamtName" HeaderText="退单金额"></asp:BoundColumn>
									<asp:BoundColumn DataField="FamtName" HeaderText="订单金额"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fpay_front_time" HeaderText="支付时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="FstateName" HeaderText="退单状态"></asp:BoundColumn>
									<asp:BoundColumn DataField="FreturnStateName" HeaderText="回导状态"></asp:BoundColumn>
									<asp:BoundColumn DataField="FrefundPathName" HeaderText="退单途径"></asp:BoundColumn>
									<asp:BoundColumn DataField="FRefundID" HeaderText="退单ID"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD colSpan="2"><webdiyer:aspnetpager id="pager" runat="server" PageSize="15" AlwaysShow="True" NumericButtonCount="5"
								ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
								OnPageChanged="ChangePage" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
					</TR>
					<TR>
						<TD colSpan="2">
							<asp:Label id="labErrMsg" runat="server" ForeColor="Red"></asp:Label>
						</TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
