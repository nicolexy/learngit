<%@ Page language="c#" Codebehind="SynRecordQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SynRecordQuery" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SynRecordQuery</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		    .auto-style1
            {
                width: 83px;
                height: 25px;
            }
            .auto-style2
            {
                height: 25px;
            }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
						if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
					}
		</script>
		<script language="javascript">
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxEndDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="Z-INDEX: 101; POSITION: absolute;  LEFT: 5%" id="Table1" border="1"
				cellSpacing="1" cellPadding="1" width="85%" >
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT color="red" face="宋体"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
							&nbsp;&nbsp;&nbsp;同步记录查询</FONT> </FONT></TD>
					<TD bgColor="#e4e5f7" align="right"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">开始日期</asp:label></TD>
					<TD><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
					<TD align="right"><asp:label id="Label3" runat="server">结束日期</asp:label></TD>
					<TD><asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
				</TR>
				<TR>
					<TD align="right" class="auto-style1"><asp:label id="Label5" runat="server">交易单</asp:label></TD>
					<TD class="auto-style2"><asp:textbox id="tbTransactionID" runat="server"></asp:textbox><asp:dropdownlist id="ddlPayType" runat="server" Width="63px" Visible="False">
							<asp:ListItem Value="9" Selected="True">所有类型</asp:ListItem>
							<asp:ListItem Value="1">C2C</asp:ListItem>
							<asp:ListItem Value="2">B2C</asp:ListItem>
							<asp:ListItem Value="3">未定义(充值)</asp:ListItem>
							<asp:ListItem Value="4">快速支付</asp:ListItem>
							<asp:ListItem Value="5">收款转账(我要收款)</asp:ListItem>
							<asp:ListItem Value="6">支付转账(按钮付款)</asp:ListItem>
						</asp:dropdownlist>
						<asp:TextBox id="tbBargainor" runat="server" Width="40px" Visible="False"></asp:TextBox>
						<asp:TextBox id="tbSPBillno" runat="server" Width="24px" Visible="False"></asp:TextBox></TD>
					<TD align="right" class="auto-style2"><asp:label id="Label6" runat="server">支付状态</asp:label></TD>
					<TD class="auto-style2"><asp:dropdownlist id="ddlPay_Status" runat="server">
							<asp:ListItem Value="9" Selected="True">所有状态</asp:ListItem>
							<asp:ListItem Value="1">支付前</asp:ListItem>
							<asp:ListItem Value="2">支付成功</asp:ListItem>
						</asp:dropdownlist><asp:dropdownlist id="ddlSynStatus" runat="server">
							<asp:ListItem Value="9" Selected="True">所有状态</asp:ListItem>
							<asp:ListItem Value="1">未同步</asp:ListItem>
							<asp:ListItem Value="2">同步失败</asp:ListItem>
							<asp:ListItem Value="3">同步成功</asp:ListItem>
						</asp:dropdownlist><asp:dropdownlist id="ddlSynType" runat="server" Visible="False">
							<asp:ListItem Value="9" Selected="True">所有类型</asp:ListItem>
							<asp:ListItem Value="0">未定义</asp:ListItem>
							<asp:ListItem Value="1">支付同步</asp:ListItem>
							<asp:ListItem Value="2">确认收货同步</asp:ListItem>
							<asp:ListItem Value="3">退款同步</asp:ListItem>
						</asp:dropdownlist>
						<asp:TextBox id="tbPurchaser" runat="server" Width="37px" Visible="False"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD align="right">
						<asp:label id="Label14" runat="server">商户编号</asp:label></TD>
					<TD>
						<asp:TextBox id="tbSPID" runat="server"></asp:TextBox></TD>
					<TD align="right"><asp:label id="Label13" runat="server">同步结果</asp:label></TD>
					<TD>
						<asp:dropdownlist id="ddlSynResult" runat="server">
							<asp:ListItem Value="9" Selected="True">所有类型</asp:ListItem>
							<asp:ListItem Value="0">同步成功</asp:ListItem>
							<asp:ListItem Value="1">未定义</asp:ListItem>
							<asp:ListItem Value="2">连接错误</asp:ListItem>
							<asp:ListItem Value="3">apache访问错误</asp:ListItem>
							<asp:ListItem Value="4">返回结果没有正确标示</asp:ListItem>
							<asp:ListItem Value="5">url 解析格式错误</asp:ListItem>
							<asp:ListItem Value="6">域名解释失败</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD align="center" colspan="3">
						<asp:CheckBox id="CheckBox1" runat="server" Text="带历史库" Visible="False"></asp:CheckBox><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button>
                        <asp:Button id="btnBatchSyn" runat="server" Width="115px" Text="同步选中的数据" OnClick="btnBatchSyn_Click"></asp:Button>
                    </td>
                     <td><asp:Button id="selbtnID" runat="server" Text ="全选" Width="80px" OnClick="btnSelectItem_Click"></asp:Button>
					</TD>
				</TR>
			</TABLE>

			<TABLE id="Table2" style="Z-INDEX: 102; POSITION: absolute; WIDTH: 85%; TOP: 30%; LEFT: 5.02%"
				cellSpacing="1" cellPadding="1" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Ftransaction_id" HeaderText="交易单"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpay_statusName" HeaderText="支付状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fsyn_statusName" HeaderText="同步状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fsyn_typeName" HeaderText="同步类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpay_typeName" HeaderText="支付类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fsp_id" HeaderText="商户编号"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="FCreat_TimeName"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="flag"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fsyn_resultName" HeaderText="同步结果"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="详细">
									<ItemTemplate>
										<a href= '<%# "SynRecordQuery_Detail.aspx?tranid="
	+DataBinder.Eval(Container, "DataItem.Ftransaction_id") 
	+ "&createtime=" + DataBinder.Eval(Container, "DataItem.FCreat_TimeName").ToString().Substring(0,10)
	+ "&flag="+ DataBinder.Eval(Container, "DataItem.flag") %>' >详细</a>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="选中同步">
									<ItemTemplate>
										<asp:CheckBox id="CheckBox2" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="Ftransaction_id"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
