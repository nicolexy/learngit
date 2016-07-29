<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="FCRollQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCRollQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FCRollQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
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
			<TABLE style="Z-INDEX: 101; LEFT: 5%; position:relative; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;外币账户流水查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">商户编号：</asp:label></TD>
                    <TD><asp:textbox id="txtspid" style="WIDTH: 180px;" runat="server"></asp:textbox><Font color="red">*</Font></TD>
                     <TD align="right"><asp:label id="Label4" runat="server">账户类型：</asp:label></TD>
                    <TD><asp:dropdownlist id="acc_type" runat="server" Width="152px">
							<asp:ListItem Value="1">交易账户</asp:ListItem>
							<asp:ListItem Value="2">现金账户</asp:ListItem>
                             <asp:ListItem Value="3">固定保证金账户</asp:ListItem>
                            <asp:ListItem Value="4">循环保证金账户</asp:ListItem>
                            <asp:ListItem Value="5">冻结账户</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label5" runat="server">查询时间：</asp:label></TD>
                    <TD colspan="3">
			            <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                        到
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                    </TD>
				</TR>
				<TR>
                    <TD align="center" colspan="4"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
            <div id="div1" style="Z-INDEX: 103; LEFT: 5.02%; position:relative;top:50px;WIDTH: 85%; ">
            <TABLE id="Table1" style="Z-INDEX: 104; WIDTH: 85%; "
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGridCurType" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Facno" HeaderText="acno" Visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcur_type" HeaderText="币种" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcur_type_str" HeaderText="币种"></asp:BoundColumn>
                                <asp:ButtonColumn Text="查询" HeaderText="操作" CommandName="queryRoll"></asp:ButtonColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <asp:button id="ButtonExport" Visible=false runat="server" Width="150px" Text="下载数据Excel" onclick="Export_Click"></asp:button>
			<TABLE id="Table2" style="Z-INDEX: 105; WIDTH: 85%; "
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
								<asp:BoundColumn DataField="Ftrade_time" HeaderText="交易时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcoding" HeaderText="商户订单号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Facc_type_str" HeaderText="账户类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsubject_str" HeaderText="交易类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="isIn" HeaderText="收入"></asp:BoundColumn>
                                <asp:BoundColumn DataField="isOut" HeaderText="支出"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbalance_str" HeaderText="余额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmemo" HeaderText="交易信息"></asp:BoundColumn>
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
            </div>
		</form>
	</body>
</HTML>
