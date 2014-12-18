<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="RefundErrorHandle.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundErrorHandle" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundErrorHandle</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
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
			<TABLE id="Table1" style="Z-INDEX: 101; POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1"
				cellPadding="1" width="85%" border="1" runat="server">
				<TR>
					<TD bgColor="#e4e5f7"><FONT face="宋体" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;退单异常数据查询</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="1" runat="server">
							<TR>
								<TD align="right"><asp:label id="Label2" runat="server">挂失败开始日期</asp:label></TD>
								<TD><FONT face="宋体"><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></FONT></TD>
								<TD align="right"><asp:label id="Label3" runat="server">挂失败结束日期</asp:label></TD>
								<TD><asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 30px" align="right"><asp:label id="Label9" runat="server">处理状态</asp:label></TD>
								<TD style="HEIGHT: 30px"><asp:dropdownlist id="ddlhandle_type" runat="server" Width="152px" AutoPostBack="True">
										<asp:ListItem Value="99" Selected="True">所有类型</asp:ListItem>
										<asp:ListItem Value="1">待处理</asp:ListItem>
										<asp:ListItem Value="2">处理中</asp:ListItem>
										<asp:ListItem Value="4">申请调状态</asp:ListItem>
										<asp:ListItem Value="3">处理完成</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD style="HEIGHT: 30px" align="right"><asp:label id="Label7" runat="server">异常原因</asp:label></TD>
								<TD style="HEIGHT: 30px"><asp:dropdownlist id="ddlerror_type" runat="server" Width="152px">
										<asp:ListItem Value="99">所有状态</asp:ListItem>
										<asp:ListItem Value="1">网银退单失败</asp:ListItem>
										<asp:ListItem Value="2">手工调失败</asp:ListItem>
										<asp:ListItem Value="3">退款校验置失败</asp:ListItem>
									</asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD align="right"><asp:label id="Label5" runat="server">数据来源</asp:label></TD>
								<TD><asp:dropdownlist id="ddlrefund_type" runat="server" Width="152px">
										<asp:ListItem Value="99" Selected="True">所有类型</asp:ListItem>
										<asp:ListItem Value="1">商户退单</asp:ListItem>
										<asp:ListItem Value="2">对帐结果退单</asp:ListItem>
										<asp:ListItem Value="3">人工录入退单</asp:ListItem>
										<asp:ListItem Value="4">对帐异常退单</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD align="right"><asp:label id="Label6" runat="server">退款银行</asp:label></TD>
								<TD><asp:dropdownlist id="ddlrefund_bank" runat="server" Width="152px"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD align="right"><asp:dropdownlist id="ddlorder_type" runat="server" Width="105px">
										<asp:ListItem Value="1">给银行订单号</asp:ListItem>
										<asp:ListItem Value="2">充值单号</asp:ListItem>
										<asp:ListItem Value="3">退款单号</asp:ListItem>
										<asp:ListItem Value="4">商户号</asp:ListItem>
										<asp:ListItem Value="5">退款批次号</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD><asp:textbox id="tbrefund_order" runat="server"></asp:textbox></TD>
								<TD align="right"><asp:label id="Label8" runat="server">退款途径</asp:label></TD>
								<TD><asp:dropdownlist id="ddlrefund_path" runat="server" Width="152px">
										<asp:ListItem Value="99" Selected="True">所有方式</asp:ListItem>
										<asp:ListItem Value="1">网银退单</asp:ListItem>
										<asp:ListItem Value="2">接口退单</asp:ListItem>
										<asp:ListItem Value="3">人工授权</asp:ListItem>
										<asp:ListItem Value="4">转帐退单</asp:ListItem>
										<asp:ListItem Value="5">转入代发</asp:ListItem>
										<asp:ListItem Value="6">付款退款</asp:ListItem>
									</asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD align="right"><asp:label id="Label10" runat="server">退单状态</asp:label></TD>
								<TD><asp:dropdownlist id="ddlState" runat="server" Width="152px">
										<asp:ListItem Value="99" Selected="True">所有状态</asp:ListItem>
										<asp:ListItem Value="0">初始状态</asp:ListItem>
										<asp:ListItem Value="1">退单流程中</asp:ListItem>
										<asp:ListItem Value="2">退单成功</asp:ListItem>
										<asp:ListItem Value="3">退单失败</asp:ListItem>
										<asp:ListItem Value="8">挂异常处理中</asp:ListItem>
										<asp:ListItem Value="6">申请手工处理</asp:ListItem>
										<asp:ListItem Value="5">手工退单中</asp:ListItem>
										<asp:ListItem Value="7">申请转入代发</asp:ListItem>
										<asp:ListItem Value="4">退单状态未定</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD align="right"><asp:label id="Label4" runat="server">每页显示笔数</asp:label><asp:dropdownlist id="ddlChangePageSize" runat="server">
										<asp:ListItem Value="50">默认50</asp:ListItem>
										<asp:ListItem Value="100">100</asp:ListItem>
										<asp:ListItem Value="500">500</asp:ListItem>
										<asp:ListItem Value="1000">1000</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD><asp:button id="btnQuery" runat="server" Text="查询记录" onclick="btnQuery_Click"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="1" runat="server">
							<TR>
								<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
										CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" PageSize="50">
										<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
										<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
										<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
										<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
										<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
										<Columns>
											<asp:BoundColumn Visible="False" DataField="FOldID" HeaderText="退单ID"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="FPaylistid" HeaderText="充值单号"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fbank_listid" HeaderText="银行订单号"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fbank_typeName" HeaderText="退款银行"></asp:BoundColumn>
											<asp:BoundColumn DataField="FBatch_date" HeaderText="原退款日期"></asp:BoundColumn>
											<asp:BoundColumn DataField="FreturnamtName" HeaderText="退单金额" DataFormatString="{0:N}"></asp:BoundColumn>
											<asp:BoundColumn DataField="FamtName" HeaderText="订单金额" DataFormatString="{0:N}"></asp:BoundColumn>
											<asp:BoundColumn DataField="FstateName" HeaderText="退单状态"></asp:BoundColumn>
											<asp:BoundColumn DataField="FreturnStateName" HeaderText="回导状态"></asp:BoundColumn>
											<asp:BoundColumn DataField="FrefundPathName" HeaderText="退单途径"></asp:BoundColumn>
											<asp:BoundColumn DataField="FHandleTypeName" HeaderText="处理状态"></asp:BoundColumn>
											<asp:BoundColumn DataField="FHandleBatchId" HeaderText="批次ID"></asp:BoundColumn>
											<asp:BoundColumn DataField="FAuthorizeFlagName" HeaderText="授权退款信息"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="详细">
												<ItemTemplate>
													<A href='<%# String.Format("RefundErrorQuery_Detail.aspx?oldid={0}", DataBinder.Eval(Container, "DataItem.FOldID").ToString()) %>'>
														详细 </A>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR height="25">
								<TD><webdiyer:aspnetpager id="pager" runat="server" PageSize="50" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
										OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
										ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr>
					<td colSpan="2"><asp:label id="labErrMsg" runat="server" ForeColor="Red"></asp:label></td>
				</tr>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
