<%@ Page language="c#" Codebehind="AppealQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.AppealQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AppealQuery</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
<script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:panel id="PanelList" Runat="server">
				<TABLE border="1" cellSpacing="1" cellPadding="1" width="1000">
					<TR>
						<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;投诉列表</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>操作员代码: </FONT><SPAN class="style3">
								<asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label2" runat="server">投诉编号</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealID" Runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label6" runat="server">交易订单号</asp:label></TD>
						<TD>
							<asp:textbox id="txtOrderNo" Runat="server" Width="200px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label7" runat="server">投诉类型</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlAppealType" Runat="server" Width="400px">
								<asp:ListItem Value="" Selected="True">全部</asp:ListItem>
								<asp:ListItem Value="1">成交不买</asp:ListItem>
								<asp:ListItem Value="2">收货不付款（确认）</asp:ListItem>
								<asp:ListItem Value="3">退款纠纷（卖家投诉买家）</asp:ListItem>
								<asp:ListItem Value="4">买家恶意评价</asp:ListItem>
								<asp:ListItem Value="5">成交不卖</asp:ListItem>
								<asp:ListItem Value="6">卖家拒绝使用财付通交易</asp:ListItem>
								<asp:ListItem Value="7">收款不发货</asp:ListItem>
								<asp:ListItem Value="8">商品与描述不符</asp:ListItem>
								<asp:ListItem Value="9">卖家恶意评价</asp:ListItem>
								<asp:ListItem Value="10">退款纠纷（买家投诉卖家）</asp:ListItem>
								<asp:ListItem Value="11">卖家要求买家先确认收货，卖家再发货</asp:ListItem>
							</asp:DropDownList></TD>
						<TD align="right">
							<asp:label id="Label3" runat="server">被投诉用户</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealedQQ" runat="server" Width="200px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label4" runat="server">投诉用户</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealQQ" runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label5" runat="server">是否审核</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlFcheck_state" Runat="server">
								<asp:ListItem Value="" Selected="True">全部</asp:ListItem>
								<asp:ListItem Value="1">未审核</asp:ListItem>
								<asp:ListItem Value="2">已提交审核</asp:ListItem>
								<asp:ListItem Value="3">审核不通过</asp:ListItem>
								<asp:ListItem Value="4">审核通过</asp:ListItem>
								<asp:ListItem Value="5">已退款</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label8" runat="server">投诉发生日期</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestDate" runat="server" Width="200px"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
							</TD>
						<TD align="right">
							<asp:label id="Label9" runat="server">到日期</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestDate1" runat="server" Width="200px" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label10" runat="server">投诉更新日期</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestUpdDate" runat="server" Width="200px" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						</TD>
						<TD align="right">
							<asp:label id="Label11" runat="server">到日期</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestUpdDate1" runat="server" Width="200px"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label12" runat="server">投诉状态</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlAppealState" Runat="server">
								<asp:ListItem Value="" Selected="True">全部</asp:ListItem>
								<asp:ListItem Value="1">未处理</asp:ListItem>
								<asp:ListItem Value="2">处理中</asp:ListItem>
								<asp:ListItem Value="3">已结束</asp:ListItem>
								<asp:ListItem Value="4">已撤销</asp:ListItem>
							</asp:DropDownList></TD>
						<TD align="right">
							<asp:label id="Label13" runat="server">有无投诉响应</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlResponseFlag" Runat="server">
								<asp:ListItem Value="" Selected="True">全部</asp:ListItem>
								<asp:ListItem Value="1">无</asp:ListItem>
								<asp:ListItem Value="2">有</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label14" runat="server">有无条款</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlRefundFlag" Runat="server">
								<asp:ListItem Value="" Selected="True">全部</asp:ListItem>
								<asp:ListItem Value="1">无</asp:ListItem>
								<asp:ListItem Value="2">有</asp:ListItem>
							</asp:DropDownList></TD>
						<TD align="right">
							<asp:Button id="btnQuery" Runat="server" Text="查 询" onclick="btnQuery_Click"></asp:Button></TD>
						<TD></TD>
					</TR>
				</TABLE>
				<TABLE border="0" cellSpacing="0" cellPadding="0" width="1000">
					<TR>
						<TD vAlign="top">
							<asp:datagrid id="DataGrid1" runat="server" Width="1000px" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
								PageSize="15" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Fappealid" HeaderText="投诉编号">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fappeal_time" HeaderText="投诉发生日期">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmodify_time" HeaderText="投诉更新日期">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fvs_qqid" HeaderText="被诉用户">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fqqid" HeaderText="投诉用户">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FstateStr" HeaderText="投诉状态">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fappeal_type" HeaderText="投诉类型">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcheck_stateStr" HeaderText="审核结果">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CLResult" HeaderText="处理结果">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="KFResult" HeaderText="客服处理">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="详细">
										<ItemTemplate>
											<a href='./AppealQuery.aspx?type=detail&Fappealid=<%# DataBinder.Eval(Container, "DataItem.Fappealid")%>'>
												详细</a>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
								SubmitButtonText="转到" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
								NumericButtonCount="5"></webdiyer:aspnetpager></TD>
					</TR>
				</TABLE>
			</asp:panel><asp:panel id="PanelMod" Runat="server">
				<TABLE border="1" cellSpacing="1" cellPadding="1" width="900">
					<TR>
						<TD colSpan="6">处理详细结果
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label15" Runat="server">投诉方号码</asp:Label></TD>
						<TD>
							<asp:Label id="lblFqqid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label16" Runat="server">投诉单编号</asp:Label></TD>
						<TD>
							<asp:Label id="lblFappealid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label17" Runat="server">用户处理</asp:Label></TD>
						<TD>
							<asp:Label id="lblFresponse_flag" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label18" Runat="server">被投诉号码</asp:Label></TD>
						<TD>
							<asp:Label id="lblFvs_qqid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label19" Runat="server">交易单编号</asp:Label></TD>
						<TD>
							<asp:Label id="lblFlistid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label20" Runat="server">处罚选择</asp:Label></TD>
						<TD>无</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label21" Runat="server">订单金额</asp:Label></TD>
						<TD>
							<asp:Label id="lblFtotal_fee" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label22" Runat="server">投诉方退款（买家）</asp:Label></TD>
						<TD>
							<asp:Label id="lblFpaybuy" Runat="server"></asp:Label>（精确到分）</TD>
						<TD align="right">
							<asp:Label id="Label23" Runat="server">订单状态</asp:Label></TD>
						<TD>
							<asp:Label id="lblFlist_state" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label24" Runat="server">含购物券</asp:Label></TD>
						<TD>
							<asp:Label id="lblFtoken_fee" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label25" Runat="server">被诉方退款（卖家）</asp:Label></TD>
						<TD>
							<asp:Label id="lblFpaysale" Runat="server"></asp:Label>（精确到分）
						</TD>
						<TD align="right"></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD colSpan="6">投诉内容
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label27" Runat="server">投诉类型</asp:Label></TD>
						<TD>
							<asp:Label id="lblFappeal_type" Runat="server" Visible=False></asp:Label>
							<asp:Label id="lblFappeal_typeStr" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label28" Runat="server">开始时间</asp:Label></TD>
						<TD>
							<asp:Label id="lblFappeal_time" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label29" Runat="server">结束时间</asp:Label></TD>
						<TD>
							<asp:Label id="lblFend_time" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD>
							<asp:Label id="Label30" Runat="server">投诉内容</asp:Label></TD>
						<TD colSpan="5">
							<asp:Label id="lblFappeal_con" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD>
							<asp:Label id="Label31" Runat="server">处理意见</asp:Label></TD>
						<TD colSpan="5">
							<asp:Label id="lblFmemo" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD colSpan="6">
							<asp:datagrid id="Datagrid2" runat="server" Width="1000px" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
								PageSize="100" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Fownerid" HeaderText="帐号">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FtypeStr" HeaderText="类型">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmsg" HeaderText="投诉留言">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcreate_time" HeaderText="留言时间">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmodify_time" HeaderText="更新时间">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fattach1" HeaderText="图片1">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmemo" HeaderText="备注">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager1" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
								SubmitButtonText="转到" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
								NumericButtonCount="5"></webdiyer:aspnetpager></TD>
					</TR>
					<TR>
						<TD colSpan="6">处理意见（支持400字）
						</TD>
					</TR>
					<TR>
						<TD colSpan="6">
							<asp:TextBox id="txtMess" Runat="server" Width="500px" Height="200px"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD>
							<asp:Button id="Button1" Runat="server" Text="要求双方解释" onclick="Button1_Click"></asp:Button></TD>
						<TD>
							<asp:Button id="Button2" Runat="server" Text="要求被诉方解释" onclick="Button2_Click"></asp:Button></TD>
						<TD>
							<asp:Button id="Button3" Runat="server" Text="要求投诉方解释" onclick="Button3_Click"></asp:Button></TD>
						<TD colSpan="3">
							<asp:Label id="Label26" Runat="server">是否涉及退款处理</asp:Label>：
							<asp:RadioButton id="IsFfund_flag" Runat="server" Text="涉及" GroupName="Ffund_flag"></asp:RadioButton>
							<asp:RadioButton id="NoFfund_flag" Runat="server" Text="不涉及" GroupName="Ffund_flag"></asp:RadioButton><BR>
							<asp:Button id="Button4" Runat="server" Text="提交审核" Visible=False onclick="Button4_Click"></asp:Button></TD>
					</TR>
					<TR>
						<TD>
							<asp:Button id="Button5" Runat="server" Text="审核通过" onclick="Button5_Click"></asp:Button></TD>
						<TD>
							<asp:Button id="Button6" Runat="server" Text="审核未通过" onclick="Button6_Click"></asp:Button></TD>
					</TR>
				</TABLE>
			</asp:panel></form>
	</body>
</HTML>
