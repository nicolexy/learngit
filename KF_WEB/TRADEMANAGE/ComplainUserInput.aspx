<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="ComplainUserInput.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.ComplainUserInput" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ComplainBussinessInput</title>
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
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxEndDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;用户投诉通知</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">商户号码：</asp:label></TD>
                    <TD><asp:textbox id="bussId" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">财付通订单号：</asp:label></TD>
                    <TD><asp:textbox id="cftOrderId" Width="230px" runat="server"></asp:textbox></TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label4" runat="server">开始日期：</asp:label></TD>
					<TD><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
					<TD align="right"><asp:label id="Label5" runat="server">结束日期：</asp:label></TD>
					<TD><asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
                </TR>
				<TR>
                    <TD align="right"><asp:label id="Label6" runat="server">投诉类型：</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlComplainType" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">全部</asp:ListItem>
							<asp:ListItem Value="1">买家要求补发货</asp:ListItem>
							<asp:ListItem Value="2">买家申请退款</asp:ListItem>
							<asp:ListItem Value="3">买家对商品质量不满意</asp:ListItem>
							<asp:ListItem Value="4">交易纠纷</asp:ListItem>
						</asp:DropDownList>
                    </TD>
                    <TD align="right"><asp:label id="Label7" runat="server">状态：</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlComplainStatus" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">全部</asp:ListItem>
							<asp:ListItem Value="1">已通知商户</asp:ListItem>
							<asp:ListItem Value="2">已催办商户</asp:ListItem>
							<asp:ListItem Value="3">商户已答复结果</asp:ListItem>
							<asp:ListItem Value="4">结单</asp:ListItem>
						</asp:DropDownList>
                    </TD>
                </TR>
				
				<TR>
					<TD align="center" colspan="4">
                    <asp:button id="Button1" runat="server" Width="80px" Text="查 询" onclick="Button1_Click"></asp:button>&nbsp;
                   <asp:button id="btnNew" runat="server" Width="80px" Text="新 增" onclick="btnNew_Click"></asp:button></TD>
				</TR>
                <tr>
                    <td align="left" colspan="4">
                       <asp:label id="Label8" runat="server">统计数据：</asp:label>&nbsp;
                       <asp:label id="Label9" runat="server">0</asp:label>
                    </td>
                </tr>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 194px; HEIGHT: 70%"
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
								<asp:BoundColumn DataField="Fbuss_id" HeaderText="商户号码"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbuss_name" HeaderText="商户名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Forder_id" HeaderText="财付通订单号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbuss_order_id" HeaderText="商户订单编号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Forder_fee_str" HeaderText="订单金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcomp_type_str" HeaderText="投诉类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fstatus_str" HeaderText="投诉状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcontact" HeaderText="用户联系方式"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmemo" HeaderText="备注"></asp:BoundColumn>

								<asp:BoundColumn DataField="Fnotice_time" HeaderText="通知时间"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fremind_time" HeaderText="催办时间"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="操作">
									<ItemTemplate>
										<a href = 'ComplainUserDetail.aspx?listid=<%# DataBinder.Eval(Container, "DataItem.Flistid")%>&qbussid=<%=qbussid %>&begindate=<%=qbegindate %>&enddate=<%=qenddate %>&orderid=<%=qorderid %>&comptype=<%=qcomptype %>&status=<%=qstatus %>&qpage=<%=qpage %>'>
											编辑</a>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="催办">
									<ItemTemplate>
										<a href= 'ComplainUserDetail.aspx?listid=<%# DataBinder.Eval(Container, "DataItem.Flistid")%>&bussid=<%# DataBinder.Eval(Container, "DataItem.Fbuss_id")%>&flag=1&qbussid=<%=qbussid %>&begindate=<%=qbegindate %>&enddate=<%=qenddate %>&orderid=<%=qorderid %>&comptype=<%=qcomptype %>&status=<%=qstatus %>&qpage=<%=qpage %>'>
											催办</a>
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
