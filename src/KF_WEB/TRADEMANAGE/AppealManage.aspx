<%@ Page language="c#" Codebehind="AppealManage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.AppealManage" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AppealManage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
			function openModeBegin()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.txtRequestDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null) Form1.txtRequestDate.value=returnValue;
			}
			
			function openModeEnd()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.txtRequestDate1.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null) Form1.txtRequestDate1.value=returnValue;
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:panel id="PanelList" Runat="server">
				<TABLE border="1" cellSpacing="1" cellPadding="1" width="1000">
					<TR>
						<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;投诉列表</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</FONT>操作员代码: </FONT><SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label2" runat="server">订单号</asp:label></TD>
						<TD>
							<asp:textbox id="txtOrderNo" Runat="server" Width="200px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label3" runat="server">被诉用户</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealedQQ" runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label4" runat="server">投诉用户</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealQQ" runat="server" Width="200px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label8" runat="server">投诉开始日期</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestDate" runat="server" Width="200px"></asp:textbox>
							<asp:imagebutton id="BeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
						<TD align="right">
							<asp:label id="Label9" runat="server">到日期</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestDate1" runat="server" Width="200px"></asp:textbox>
							<asp:imagebutton id="EndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<asp:Button id="btnQuery" Runat="server" Text="查 询"></asp:Button></TD>
						<TD></TD>
					</TR>
				</TABLE>
				<TABLE border="0" cellSpacing="0" cellPadding="0" width="1000">
					<TR>
						<TD vAlign="top">
							<asp:datagrid id="DataGrid1" runat="server" Width="1000px" ItemStyle-HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="15" AutoGenerateColumns="False"
								GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
								BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Flistid" HeaderText="订单号">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fqqid" HeaderText="投诉方QQ">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fvs_qqid" HeaderText="被投诉方QQ">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fappeal_time" HeaderText="投诉日期">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn HeaderText="实付金额">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FstateStr" HeaderText="订单状态">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CLResult" HeaderText="处理状态">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="KFResult" HeaderText="交易处理">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="详细">
										<ItemTemplate>
											<a href='./AppealManage.aspx?type=detail&Fappealid=<%# DataBinder.Eval(Container, "DataItem.Fappealid")%>'>
												详细</a>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" NumericButtonCount="5" PagingButtonSpacing="0"
								ShowInputBox="always" CssClass="mypager" OnPageChanged="ChangePage" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"
								AlwaysShow="True"></webdiyer:aspnetpager></TD>
					</TR>
				</TABLE>
			</asp:panel><asp:panel id="PanelMod" Runat="server">
				<TABLE border="1" cellSpacing="1" cellPadding="1" width="900">
					<TR>
						<TD colSpan="6">订单管理
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label15" Runat="server">订单号</asp:Label></TD>
						<TD>
							<asp:Label id="lblFqqid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label16" Runat="server">投诉单编号</asp:Label></TD>
						<TD>
							<asp:Label id="lblFappealid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label17" Runat="server">买家退款金额</asp:Label></TD>
						<TD>
							<asp:Label id="lblFresponse_flag" Runat="server"></asp:Label>单位（元）</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label18" Runat="server">卖家结算金额</asp:Label></TD>
						<TD>
							<asp:Label id="lblFvs_qqid" Runat="server"></asp:Label>单位（元）</TD>
						<TD align="right">
							<asp:Label id="Label19" Runat="server">实付金额</asp:Label></TD>
						<TD>
							<asp:Label id="lblFlistid" Runat="server"></asp:Label>单位（元）</TD>
						<TD align="right">
							<asp:Label id="Label20" Runat="server">交易状态</asp:Label></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label21" Runat="server">处理提示</asp:Label></TD>
						<TD colSpan="5">
							<asp:Label id="lblFtotal_fee" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label24" Runat="server">备注（必填）</asp:Label></TD>
						<TD colSpan="2">
							<asp:TextBox id="txtMess" Runat="server" Width="500px" Height="200px"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label5" Runat="server">财付通用户名（必填）</asp:Label></TD>
						<TD>
							<asp:TextBox id="Textbox1" Runat="server"></asp:TextBox></TD>
						<TD align="right">
							<asp:Label id="Label6" Runat="server">财付通密码（必填）</asp:Label></TD>
						<TD>
							<asp:TextBox id="Textbox2" Runat="server"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD>
							<asp:Button id="Button1" Runat="server" Text="投诉详情"></asp:Button></TD>
						<TD>
							<asp:Button id="Button2" Runat="server" Text="申请调账"></asp:Button></TD>
					</TR>
				</TABLE>
			</asp:panel></form>
	</body>
</HTML>
