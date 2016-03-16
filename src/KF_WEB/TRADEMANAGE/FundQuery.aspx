<%@ Page language="c#" Codebehind="FundQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.FundQuery" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FundQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style5 { COLOR: #000000 }
	.style6 { COLOR: #ff0000 }
		</style>
		<script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 2.51%; POSITION: absolute; TOP: 168px; HEIGHT: 0.93%"
				cellSpacing="0" cellPadding="0" width="95%" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="LightGray" BorderWidth="1px" BackColor="White"
							CellPadding="3" AutoGenerateColumns="False" PageSize="50" EnableViewState="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#FFFFFF"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="Black" BackColor="Silver"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fbank_list" HeaderText="给银行订单号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Faid" HeaderText="帐号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FListID" HeaderText="收款单ID单号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Faname" HeaderText="姓名"></asp:BoundColumn>
								<asp:BoundColumn DataField="FbankName" HeaderText="银行类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpay_front_time" HeaderText="充值时间"></asp:BoundColumn>
								<asp:BoundColumn DataField="FNewnum" HeaderText="充值金额"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateName" HeaderText="充值状态"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="详细内容">
									<ItemTemplate>
										<a href = 'FUndQuery_Detail.aspx?tdeid=<%# DataBinder.Eval(Container, "DataItem.FTde_ID")%>&begintime=<%=begintime %>&endtime=<%=endtime %>&listid=<%# DataBinder.Eval(Container, "DataItem.Flistid")%>&fpay_front_time=<%# DataBinder.Eval(Container, "DataItem.Fpay_front_time")%>&Fbank_list=<%# DataBinder.Eval(Container, "DataItem.Fbank_list")%>&Fbank_type=<%# DataBinder.Eval(Container, "DataItem.Fbank_type")%>&FHistoryFlag=<%# DataBinder.Eval(Container, "DataItem.FHistoryFlag")%>'>
											详细内容</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="FlistId" HeaderText="FlistId"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<tr>
					<td><asp:label id="Label9" runat="server">查询总记录数：</asp:label>
						<asp:label id="labCountNum" runat="server" Width="100"></asp:label>
						<asp:label id="Label10" runat="server">查询总金额：</asp:label>
						<asp:label id="labAmount" runat="server" Width="100"></asp:label></td>
				</tr>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" PageSize="50" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" CustomInfoTextAlign="Center"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
			<TABLE id="Table1" style="Z-INDEX: 103; LEFT: 2.25%; POSITION: absolute; TOP: 1.52%; HEIGHT: 106px"
				cellSpacing="0" cellPadding="0" width="95%" border="1">
				<TR>
					<TD style="WIDTH: 100%" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7" colSpan="5">
						<DIV align="center">
							<TABLE id="Table3" height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<TR>
									<TD width="79%"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;
												<asp:Label id="Label8" runat="server" Width="192px">Label</asp:Label></FONT></FONT></TD>
									<TD width="21%"><FONT face="宋体">&nbsp;</FONT>操作员代码: <SPAN class="style3">
											<asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
								</TR>
							</TABLE>
							<SPAN class="style3"></SPAN>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 91px" align="right">
						<asp:label id="Label2" runat="server">开始日期</asp:label></TD>
					<TD style="WIDTH: 290px">
						<asp:textbox id="TextBoxBeginDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',maxDate:'#F{$dp.$D(\'TextBoxEndDate\')}'})" Width="160px" CssClass="Wdate" BorderStyle="Groove"></asp:textbox>
					</TD>
					<TD align="right">
						<asp:label id="Label3" runat="server">结束日期</asp:label></TD>
					<TD>
						<asp:textbox id="TextBoxEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',minDate:'#F{$dp.$D(\'TextBoxBeginDate\')}'})" Width="160px" CssClass="Wdate" BorderStyle="Groove"></asp:textbox>
					</TD>
				</TR>
				<%--<TR>
					<TD style="WIDTH: 91px; HEIGHT: 25px" align="right">
						<asp:label id="Label5" runat="server">查询状态</asp:label></TD>
					<TD style="WIDTH: 290px; HEIGHT: 25px">
						<asp:dropdownlist id="ddlStateType" runat="server" Width="152px" AutoPostBack="True">
							<asp:ListItem Value="0" Selected="True">所有状态</asp:ListItem>
							<asp:ListItem Value="1">付款成功</asp:ListItem>
							<asp:ListItem Value="2">付款失败</asp:ListItem>
							<asp:ListItem Value="3">等待付款</asp:ListItem>
							<asp:ListItem Value="4">付款中</asp:ListItem>
							<asp:ListItem Value="5">作废</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD style="HEIGHT: 25px" align="right">
						<asp:label id="Label6" runat="server">金额限度</asp:label></TD>
					<TD style="HEIGHT: 25px">
						<asp:textbox id="tbFNum" runat="server" Width="88px" BorderStyle="Groove">0.00</asp:textbox><FONT face="宋体">-
							<asp:textbox id="txbNumMax" runat="server" Width="88px" BorderStyle="Groove">20000000.00</asp:textbox>元</FONT>
						<asp:regularexpressionvalidator id="Regularexpressionvalidator7" runat="server" ValidationExpression="^[0-9/.]+"
							ToolTip="**.**" ErrorMessage="RegularExpressionValidator" ControlToValidate="tbFNum" Display="Dynamic">请输入正确金额</asp:regularexpressionvalidator>
						<asp:regularexpressionvalidator id="Regularexpressionvalidator1" runat="server" ValidationExpression="^[0-9/.]+"
							ToolTip="**.**" ErrorMessage="RegularExpressionValidator" ControlToValidate="txbNumMax" Display="Dynamic">请输入正确金额</asp:regularexpressionvalidator></TD>
				</TR>--%>
				<TR>
					<TD style="WIDTH: 91px" align="right">
						<asp:dropdownlist id="dpLst" runat="server" AutoPostBack="True" onselectedindexchanged="dpLst_SelectedIndexChanged">
							<asp:ListItem Value="qq">按帐号</asp:ListItem>
							<asp:ListItem Value="czd">充值单号</asp:ListItem>
							<asp:ListItem Value="toBank" Selected="True">给银行的订单号</asp:ListItem>
							<asp:ListItem Value="BankBack">银行返回订单号</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD style="WIDTH: 290px">
						<asp:textbox id="tbQQID" runat="server" Width="165px" BorderStyle="Groove"></asp:textbox>
						<asp:requiredfieldvalidator id="rfvNullCheck" runat="server" ErrorMessage="不能为空" ControlToValidate="tbQQID"
							Display="Dynamic" Enabled="False"></asp:requiredfieldvalidator>
						<asp:regularexpressionvalidator id="revNumOnly" runat="server" ValidationExpression="^[0-9 ]{10,30}" ErrorMessage="非法充值单号"
							ControlToValidate="tbQQID" Display="Dynamic" Enabled="False"></asp:regularexpressionvalidator></TD>
					<%--<TD align="right"><FONT face="宋体">
							<asp:label id="Label4" runat="server">排序类型</asp:label></FONT></TD>
					<TD align="left">
						<asp:dropdownlist id="ddlSortType" runat="server" Width="152px" AutoPostBack="True">
							<asp:ListItem Value="1">时间小到大</asp:ListItem>
							<asp:ListItem Value="2">时间大到小</asp:ListItem>
							<asp:ListItem Value="3">金额小到大</asp:ListItem>
							<asp:ListItem Value="4">金额大到小</asp:ListItem>
						</asp:dropdownlist></TD>--%>
                    <TD align="center" colspan="2"><FONT face="宋体">
							<asp:CheckBox id="CheckBox1" runat="server" Text="历史记录"></asp:CheckBox>
							<asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></FONT></TD>
				</TR>
				<TR>
				<%--	<TD style="WIDTH: 91px" align="right">
						<asp:Label id="Label7" runat="server">充值银行</asp:Label></TD>
					<TD style="WIDTH: 290px">
						<asp:DropDownList id="ddlBankType" runat="server"></asp:DropDownList></TD>--%>
					
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
