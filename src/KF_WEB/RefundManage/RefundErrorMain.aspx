<%@ Page language="c#" Codebehind="RefundErrorMain.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundErrorMain" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundErrorMain</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body background="../IMAGES/Page/bg01.gif" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体">
				<TABLE style="Z-INDEX: 101; POSITION: absolute; WIDTH: 94%; HEIGHT: 80%; TOP: 5%; LEFT: 5%"
					id="Table3" border="1" cellSpacing="1" borderColor="#666666" cellPadding="1" width="383"
					align="center" height="127">
					<TR bgColor="#eeeeee">
						<TD style="HEIGHT: 4px" height="4" colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG src="../IMAGES/Page/post.gif" width="15" height="16"><STRONG>&nbsp;
										<asp:label id="lbTitle" runat="server">lbTitle</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="Label1" runat="server" ForeColor="ControlText">操作员代码：</asp:label><SPAN class="style3"><asp:label id="Label_uid" runat="server">Label</asp:label></SPAN></STRONG></SPAN></FONT></TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 126px" vAlign="top" width="100%" colSpan="2" align="center"><FONT face="宋体">
								<TABLE id="Table1" border="1" cellSpacing="1" cellPadding="1" width="99%">
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
													<asp:HyperLinkColumn DataNavigateUrlField="Furl" DataNavigateUrlFormatString="RefundErrorReturn.Aspx?{0}"
														DataTextField="FBatchDay" HeaderText="日期">
														<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
														<ItemStyle Font-Underline="True" HorizontalAlign="Center" ForeColor="Blue" VerticalAlign="Middle"></ItemStyle>
													</asp:HyperLinkColumn>
													<asp:BoundColumn DataField="FBankTypeName" HeaderText="银行"></asp:BoundColumn>
													<asp:HyperLinkColumn DataNavigateUrlField="Furl2" DataNavigateUrlFormatString="RefundErrorHandle.Aspx?{0}"
														DataTextField="FPayCount" HeaderText="总笔数">
														<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														<ItemStyle Font-Underline="True" HorizontalAlign="Right" ForeColor="Blue"></ItemStyle>
													</asp:HyperLinkColumn>
													<asp:BoundColumn DataField="FPaySum1" HeaderText="总金额" DataFormatString="{0:N}">
														<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FrefundPath" HeaderText="退款途径"></asp:BoundColumn>
													<asp:BoundColumn DataField="FStatusName" HeaderText="当前状态"></asp:BoundColumn>
													<asp:BoundColumn DataField="FMsg" HeaderText="操作提示"></asp:BoundColumn>
													<asp:BoundColumn DataField="FProposer" HeaderText="发起人"></asp:BoundColumn>
													<asp:BoundColumn DataField="FBatchid" HeaderText="批次号"></asp:BoundColumn>
													<asp:BoundColumn DataField="FApproveDate" HeaderText="执行时间"></asp:BoundColumn>
												</Columns>
												<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
											</asp:datagrid></TD>
									</TR>
								</TABLE>
							</FONT>
						</TD>
					</TR>
					<tr>
						<td style="HEIGHT: 48px" height="25" colSpan="2" align="center"><FONT face="宋体"></FONT></td>
					</tr>
					<TR>
						<TD height="25" align="center"><asp:label id="Label2" runat="server">开始日期</asp:label>
                            <asp:textbox id="TextBoxBeginDate" runat="server" Width="100px" BorderWidth="1px" BorderColor="Gray"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>&nbsp;&nbsp;
							<asp:label id="Label3" runat="server">结束日期</asp:label>
                            <asp:textbox id="TextBoxEndDate" runat="server" Width="100px"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>&nbsp;&nbsp;
							<asp:label id="Label4" runat="server">操作状态</asp:label><asp:dropdownlist id="ddlState" runat="server" Width="115px">
								<asp:ListItem Value="9999">所有状态</asp:ListItem>
								<asp:ListItem Value="1">申请再次网银退款</asp:ListItem>
								<asp:ListItem Value="4">网银退款审批通过</asp:ListItem>
								<asp:ListItem Value="96">网银退款中</asp:ListItem>
								<asp:ListItem Value="10">网银退款结果回导完成</asp:ListItem>
								<asp:ListItem Value="11">出纳完成人工授权操作</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="2">申请付款退款</asp:ListItem>
								<asp:ListItem Value="5">付款退款审批通过</asp:ListItem>
								<asp:ListItem Value="7">付款退款退款中</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="3">申请人工授权退款</asp:ListItem>
								<asp:ListItem Value="6">授权退款审批通过</asp:ListItem>
								<asp:ListItem Value="9">授权退款中</asp:ListItem>
								<asp:ListItem Value="97">授权书新流程退款中</asp:ListItem>
								<asp:ListItem Value="98">授权书生成中</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="12">退款直接调整为成功审批中</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="13">申请转入代发处理中</asp:ListItem>
								<asp:ListItem Value="9999">--------------</asp:ListItem>
								<asp:ListItem Value="99">批次处理完成</asp:ListItem>
							</asp:dropdownlist>&nbsp;&nbsp;
							<asp:label id="Label6" runat="server">银行类型</asp:label><asp:dropdownlist id="ddlBankType" runat="server" Width="104px"></asp:dropdownlist>&nbsp;&nbsp;
							<asp:label id="Label7" runat="server">途径</asp:label><asp:dropdownlist id="ddlRefundPath" runat="server">
								<asp:ListItem Value="9999">所有状态</asp:ListItem>
								<asp:ListItem Value="3E">人工授权</asp:ListItem>
								<asp:ListItem Value="6E">付款退款</asp:ListItem>
								<asp:ListItem Value="1E">再次退款</asp:ListItem>
								<asp:ListItem Value="7E">转入代发</asp:ListItem>
								<asp:ListItem Value="9E">直接成功</asp:ListItem>
							</asp:dropdownlist>&nbsp;&nbsp;
							<asp:label style="Z-INDEX: 0" id="Label5" runat="server">发起人</asp:label><asp:textbox style="Z-INDEX: 0" id="txtProposer" runat="server" Width="80px"></asp:textbox></TD>
						<TD>
							<asp:button id="btnQuery" runat="server" Width="93px" Text="取得最新状态" onclick="btnQuery_Click"></asp:button></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
