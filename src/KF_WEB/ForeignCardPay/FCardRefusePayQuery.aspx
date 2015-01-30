<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="FCardRefusePayQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCardPay.FCardRefusePayQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FCardRefusePayQuery</title>
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
		            function openModeEnd() {
		                var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.TextBoxEndDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
		                if (returnValue != null) Form1.TextBoxEndDate.value = returnValue;
		            }
		</script>
        <script>
            function CheckEmail() {
                var txtEmail = document.getElementById("txtEmail");

                if (txtEmail.value.replace(/^\s*/, "").replace(/\s*$/, "").length == 0) {
                    txtEmail.focus();
                    txtEmail.select();
                    alert("邮箱不允许为空!");
                    return false;
                }
            }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;外卡拒付查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">商户编号：</asp:label></TD>
                    <TD><asp:textbox id="txtspid" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label4" runat="server">商家订单号：</asp:label></TD>
                    <TD><asp:textbox id="txtCoding" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label3" runat="server">财付通订单号：</asp:label></TD>
                    <TD><asp:textbox id="txtspListID" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
                     <TD align="right"><asp:label id="Label6" runat="server">拒付状态：</asp:label></TD>
                    <TD><asp:dropdownlist id="check_state" runat="server" Width="152px">
							<asp:ListItem Value="" Selected="True" >所有状态</asp:ListItem>
							<asp:ListItem Value="1">查单</asp:ListItem>
							<asp:ListItem Value="2">拒付</asp:ListItem>
                            <asp:ListItem Value="3">预仲裁</asp:ListItem>
                            <asp:ListItem Value="4">仲裁</asp:ListItem>
                            <asp:ListItem Value="5">拒付退款</asp:ListItem>
                            <asp:ListItem Value="6">逾期解冻</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
                <TR>
                <TD align="right"><asp:label id="Label7" runat="server">商户处理状态：</asp:label></TD>
                    <TD><asp:dropdownlist id="sp_process_state" runat="server" Width="152px">
							<asp:ListItem Value="" Selected="True" >所有状态</asp:ListItem>
							<asp:ListItem Value="1">未处理</asp:ListItem>
							<asp:ListItem Value="2">已申诉</asp:ListItem>
                            <asp:ListItem Value="3">同意拒付</asp:ListItem>
                            <asp:ListItem Value="4">无需处理</asp:ListItem>
                            <asp:ListItem Value="5">商户逾期转退款</asp:ListItem>
						</asp:dropdownlist></TD>
                    <TD align="right"><asp:label id="Label5" runat="server">查单日期：</asp:label></TD>
                    <TD><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    到<asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    <Font color="red">*</Font></TD>
				</TR>
				<TR>
                    <TD align="center" colspan="4"><Font color="red">*必填</Font>&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 184px; HEIGHT: 35%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fspid" HeaderText="商户编号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcoding" HeaderText="商家订单号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Flistid" HeaderText="财付通订单号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FModify_time" HeaderText="查单时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_req_refund_fee_str" HeaderText="查单金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_currency_freeze_fee_str" HeaderText="冻结金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_currency_refund_fee_str" HeaderText="拒付退款金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcheck_state_str" HeaderText="拒付状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsp_process_state_str" HeaderText="商户处理状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Frisk_process_state_str" HeaderText="风控处理状态"></asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" 
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
