<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DFBatchQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.DFBatchQuery1" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryDKListInfoPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="1300">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;代付批次信息查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<tr>
					<td colspan="2"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT> <span>商户号：</span><asp:TextBox Runat="server" ID="tbx_spid"></asp:TextBox>
						<span>商户批次号：</span><asp:TextBox Runat="server" ID="tbx_spBatchID"></asp:TextBox>
						<asp:Label id="Label2" runat="server">批次状态：</asp:Label>
						<asp:DropDownList id="ddl_state" runat="server">
							<asp:ListItem Value="0" Selected="True">全部状态</asp:ListItem>
							<asp:ListItem Value="1">初始状态</asp:ListItem>
							<asp:ListItem Value="2">待审批</asp:ListItem>
							<asp:ListItem Value="3">可付款</asp:ListItem>
							<asp:ListItem Value="4">拒绝付款</asp:ListItem>
							<asp:ListItem Value="5">执行完成</asp:ListItem>
							<asp:ListItem Value="6">受理完成</asp:ListItem>
							<asp:ListItem Value="7">已取消付款</asp:ListItem>
						</asp:DropDownList>
						<span>查询时间段：</span>
                        <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
						<span>到：</span>
                        <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</td>
				</tr>
				<tr>
					<td colSpan="5" align="center"><asp:button id="btn_serach" Width="80px" Text="查询" Runat="server" onclick="btn_serach_Click"></asp:button></td>
				</tr>
			</TABLE>
			<table border="0" cellSpacing="0" cellPadding="0" width="1300">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="1300px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<Columns>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="发起时间"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="结束时间"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fubatch_id" HeaderText="商户批次号"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="spidUrl" DataTextField="Fspid" HeaderText="商户号"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Ftotal_paynumName" HeaderText="实付总金额"></asp:BoundColumn>								
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="totalBatchUrl" DataTextField="Ffact_num"
									HeaderText="实付总笔数"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Fsucpay_amountName" HeaderText="成功金额"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="successBatchUrl" DataTextField="Fsucpay_num"
									HeaderText="成功笔数"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Ffailpay_amountName" HeaderText="失败金额"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="failedBatchUrl" DataTextField="Ffailpay_num"
									HeaderText="失败笔数"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="FHandling_amountName" HeaderText="处理中金额"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="handlingBatchUrl" DataTextField="FHandling_num"
									HeaderText="处理中笔数"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="FstateName" HeaderText="批次状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fresult_info" HeaderText="失败原因"></asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
			<div>
				<p>
					<label>交易成功总笔数：</label><asp:Label Runat="server" ID="lb_successNum">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>交易成功总额：</label><asp:Label Runat="server" ID="lb_successAllMoney">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>交易失败总笔数：</label><asp:Label Runat="server" ID="lb_failNum">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>交易失败总额：</label><asp:Label Runat="server" ID="lb_failAllMoney">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>处理中的总笔数：</label><asp:Label Runat="server" ID="lb_handlingNum">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>处理中的总额：</label><asp:Label Runat="server" ID="lb_handlingMoney">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_outExcel" Width="80px" runat="server" Text="导出结果" OnClick="btn_outExcel_Click" Visible="false"></asp:Button>
				</p>
			</div>
			<table border="0" cellSpacing="1" cellPadding="0" width="1200" bgColor="black" style="DISPLAY:none">
				<tr>
					<td bgColor="#eeeeee" height="18" colSpan="4"><span>&nbsp;详细信息列表：</span></td>
				</tr>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19" width="229"><font face="宋体">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="宋体">基金交易账户对应ID：</FONT></font></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19" width="225"><font face="宋体">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="宋体">&nbsp;用户的CFT内部ID:</font></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></font></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="宋体">&nbsp;投资人真实姓名:</font></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;开户有效标志:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="宋体">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="宋体">&nbsp;证件类型:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><font face="宋体">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;证件号码:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;物理状态:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;创建时间:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;最后修改时间:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;<asp:label id="Label1" runat="server">&nbsp;</asp:label></FONT></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
