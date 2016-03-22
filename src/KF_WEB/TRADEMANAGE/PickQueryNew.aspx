<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="PickQueryNew.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.PickQueryNew" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PickQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 3%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;提现记录查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">开始日期</asp:label></TD>
					<TD><asp:textbox id="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">结束日期</asp:label></TD>
					<TD><asp:textbox id="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox></TD>
				</TR>
				<%--<TR>
					<TD align="right"><asp:label id="Label5" runat="server">查询状态</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlStateType" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">所有状态</asp:ListItem>
							<asp:ListItem Value="1">付款成功</asp:ListItem>
							<asp:ListItem Value="2">付款失败</asp:ListItem>
							<asp:ListItem Value="3">等待付款</asp:ListItem>
							<asp:ListItem Value="4">付款中</asp:ListItem>
							<asp:ListItem Value="5">作废</asp:ListItem>
						</asp:DropDownList></TD>
					<TD align="right"><asp:label id="Label6" runat="server">金额限度</asp:label></TD>
					<TD><asp:textbox id="tbFNum" runat="server">0</asp:textbox>
						<asp:regularexpressionvalidator id="Regularexpressionvalidator7" runat="server" ValidationExpression="^[0-9/.]+"
							ToolTip="**.**" ErrorMessage="RegularExpressionValidator" ControlToValidate="tbFNum" Display="Dynamic">请输入正确金额</asp:regularexpressionvalidator></TD>
				</TR>--%>
				<TR>
					<TD align="right" colSpan="1" rowSpan="1">
						<asp:DropDownList id="ddlIDType" runat="server">
							<asp:ListItem Value="0" Selected="True">帐号</asp:ListItem>
							<asp:ListItem Value="1">银行帐号</asp:ListItem>
							<asp:ListItem Value="2">提现单号</asp:ListItem>
						</asp:DropDownList></TD>
					<TD><asp:textbox id="tbQQID" runat="server"></asp:textbox></TD>
					<TD align="right"><FONT face="宋体">
							<%--<asp:Label id="Label7" runat="server">排序类型</asp:Label></FONT></TD>--%>
					<TD align="left">
						<%--<asp:DropDownList id="ddlSortType" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">不排序</asp:ListItem>
							<asp:ListItem Value="1">时间小到大</asp:ListItem>
							<asp:ListItem Value="2">时间大到小</asp:ListItem>
							<asp:ListItem Value="3">金额小到大</asp:ListItem>
							<asp:ListItem Value="4">金额大到小</asp:ListItem>
						</asp:DropDownList>--%></TD>
				</TR>
				<%--<TR>
					<TD align="right" colSpan="1"><asp:label id="Label8" runat="server">提现银行</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlBankType" runat="server"></asp:DropDownList></TD>
					<TD align="right" colSpan="1"><asp:label id="Label9" runat="server">提现类型</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlCashType" runat="server"></asp:DropDownList></TD>
				</TR>--%>
                <TR>
					<TD align="center" colspan="4"><FONT face="宋体"><asp:button id="Button1" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></FONT></TD>
				</TR>
                  <TR>
					<TD align="center" colspan="4">
                        <FONT face="宋体" color="red">
                             1、时间跨度只支持按自然月查询，不支持跨月查询<br />
                            2、通过银行卡查提现记录，只支持查2015年5月01日之后的数据，2015年5月1日之前的数据请用账号或提现单号查询。 
                        </FONT>

					</TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 210px; HEIGHT: 70%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" EnableViewState="False">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Faid" HeaderText="帐号ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="Facc_name" HeaderText="姓名"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_name" HeaderText="开户名称"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fnum_str" HeaderText="提现金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcharge_str" HeaderText="手续费"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fabank_type_str" HeaderText="提现银行"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_type_str" HeaderText="出款银行"></asp:BoundColumn>
								<asp:BoundColumn DataField="FaBankID" HeaderText="银行帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpay_front_time" HeaderText="提现时间"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateName" HeaderText="提现状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsign_str" HeaderText="退票"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmemo" HeaderText="退票原因"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="详细内容">
									<ItemTemplate>
										<a href = 'PickQuery_Detail.aspx?listid=<%# DataBinder.Eval(Container, "DataItem.FlistID")%>&begintime=<%=begintime %>&endtime=<%=endtime %>'>
											详细内容</a>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
