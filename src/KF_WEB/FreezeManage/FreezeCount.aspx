<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="FreezeCount.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FreezeManage.FreezeCount" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FreezeCount</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		    .table_line {
		    display:inline-block;
            }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_beginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.tbx_beginDate.value=returnValue;
					}
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_endDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.tbx_endDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table border="1" cellSpacing="1" cellPadding="1" width="1100">
				<TR>
					<TD style="WIDTH: 719px; HEIGHT: 20px" bgColor="#e4e5f7" colSpan="2"><FONT color="red" face="宋体"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><asp:label id="lb_pageTitle" Runat="server">风控解冻审核工单统计</asp:label></FONT></TD>
					<td style="HEIGHT: 20px"></FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></td>
				</TR>
				<tr style="DISPLAY:none">
					<td width="80" rowspan="5"></td>
					<td style="WIDTH: 635px" colspan="2"><label style="WIDTH: 80px; HEIGHT: 20px; VERTICAL-ALIGN: middle">冻结原因：</label><asp:textbox id="tbx_freezeReason" Width="240px" Runat="server"></asp:textbox></td>
				</tr>
				<tr>
					<td style="WIDTH: 635px" colspan="2"><label style="WIDTH: 80px; HEIGHT: 20px; VERTICAL-ALIGN: middle">结单人员：</label><asp:textbox id="tbx_freezeHandleUserID" Width="240px" Runat="server"></asp:textbox></td>
				</tr>
				<tr>
					<td style="WIDTH: 635px" colspan="2">状态：
                        <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal" CssClass="table_line" >
                            <%--<asp:ListItem Value="0">未处理</asp:ListItem>--%>
                            <asp:ListItem Value="8">挂起</asp:ListItem>
                            <asp:ListItem Value="1">通过</asp:ListItem>
                            <asp:ListItem Value="2">拒绝</asp:ListItem>
                            <asp:ListItem Value="7">删除</asp:ListItem>
                            <asp:ListItem Value="11">补充资料</asp:ListItem>
                            <asp:ListItem Value="100">补充处理结果</asp:ListItem>
                        </asp:CheckBoxList>
                       
						<%--	<asp:CheckBox Runat="server" ID="cbx_unHandle" Text="未处理" /></span> <span style="MARGIN:0px 0px 0px 30px">
							<asp:CheckBox Runat="server" ID="cbx_hangUP" Text="挂起" /></span> <span style="MARGIN:0px 0px 0px 30px">
							<asp:CheckBox Runat="server" ID="cbx_fin1" Text="结单（已解冻）" /></span> <span style="MARGIN:0px 0px 0px 30px">
							<asp:CheckBox Runat="server" ID="cbx_fin2" Text="结单（未解冻）" /></span> <span style="MARGIN:0px 0px 0px 30px">
							<asp:CheckBox Runat="server" ID="cbx_del" Text="作废单" />--%>
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 635px">
						<asp:DropDownList Runat="server" ID="ddl_timeType">
							<asp:ListItem Value="进单时间" Selected="True" />
							<asp:ListItem Value="最后修改时间" />
						</asp:DropDownList>
						&nbsp;&nbsp;&nbsp;从&nbsp;&nbsp;&nbsp;
						<asp:TextBox runat="server" ID="tbx_beginDate"></asp:TextBox><asp:imagebutton id="btnBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton>
						&nbsp;&nbsp;&nbsp;到&nbsp;&nbsp;&nbsp;
						<asp:TextBox Runat="server" ID="tbx_endDate"></asp:TextBox><asp:imagebutton id="btnEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton>
						<label>&nbsp;&nbsp;&nbsp;(日期跨度为：1个月)</label>
					</td>
				</tr>
				<tr>
					<td align="center" style="WIDTH: 635px" colspan="2"><asp:Button Runat="server" Text="查 询" ID="btn_query" Width="80" onclick="btn_query_Click"></asp:Button></td>
				</tr>
			</table>
			<br>
			<br>
			<table border="0" cellSpacing="0" cellPadding="0" width="1100">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="1100px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
							Font-Size="13px">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="showTitle" HeaderText="冻结原因/结单人员">
									<HeaderStyle Width="250px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="unHandle_Num" HeaderText="未处理">
									<HeaderStyle Width="110px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="handUp_Num" HeaderText="挂起">
									<HeaderStyle Width="120px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fin1_Num" HeaderText="通过">
									<HeaderStyle Width="160px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fin2_Num" HeaderText="拒绝">
									<HeaderStyle Width="160px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AddRecordNum" HeaderText="补充处理结果">
									<HeaderStyle Width="160px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Discard_Num" HeaderText="删除">
									<HeaderStyle Width="75px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="adddatanumsum" HeaderText="补充资料">
									<HeaderStyle Width="75px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="TotalNum" HeaderText="总计">
									<HeaderStyle Width="75px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
