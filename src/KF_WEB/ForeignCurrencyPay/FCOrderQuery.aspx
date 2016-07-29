<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="FCOrderQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCOrderQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FCOrderQuery</title>
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
			<TABLE style="Z-INDEX: 101;LEFT: 5%; position:relative;top:10px" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;订单查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">商户编号：</asp:label></TD>
                    <TD><asp:textbox id="txtspid" style="WIDTH: 180px;" runat="server"></asp:textbox><Font color="red">*</Font></TD>
					<TD align="right"><asp:label id="Label4" runat="server">商家订单号：</asp:label></TD>
                    <TD><asp:textbox id="txtCoding" style="WIDTH: 180px;" runat="server"></asp:textbox><Font color="red">*</Font></TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label3" runat="server">财付通订单号：</asp:label></TD>
                    <TD><asp:textbox id="txtspListID" style="WIDTH: 180px;" runat="server"></asp:textbox><Font color="red">*</Font></TD>
                     <TD align="right"><asp:label id="Label6" runat="server">交易状态：</asp:label></TD>
                    <TD><asp:dropdownlist id="ddl_state" runat="server" Width="152px">
							<asp:ListItem Value="" Selected="True" >所有状态</asp:ListItem>
							<asp:ListItem Value="1">未支付</asp:ListItem>
							<asp:ListItem Value="2">支付成功</asp:ListItem>
                            <asp:ListItem Value="7">转入退款</asp:ListItem>
                            <asp:ListItem Value="99">交易关闭</asp:ListItem>
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
                        <Font color="red">*</Font></TD>
				</TR>
				<TR>
                    <TD align="center" colspan="4"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
            </br>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
            <asp:button id="ButtonExport" Visible=false  runat="server" Width="150px" Text="下载数据Excel" onclick="Export_Click"></asp:button>
			<TABLE id="Table2" style="Z-INDEX: 102; position:relative;top:20px;LEFT: 5.02%; WIDTH: 85%;  HEIGHT: 35%"
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
                                <asp:BoundColumn DataField="Fcreate_time" HeaderText="交易时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fpaynum_str" HeaderText="交易金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftrade_state_str" HeaderText="交易状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FTK_str" HeaderText="是否退款"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FZF_str" HeaderText="是否拒付"></asp:BoundColumn>
								<asp:ButtonColumn Text="详情" HeaderText="详细" CommandName="detail"></asp:ButtonColumn>
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
            <TABLE id="tableOrder" runat="server" cellSpacing="1" cellPadding="0" style="Z-INDEX: 103; position:relative;top:20px;LEFT: 5.02%; WIDTH: 85%; "  bgColor="black" border="0">
                <TR>
					<TD bgColor="#eeeeee" colSpan="4" height="18"><Font color="red"><SPAN>&nbsp;订单信息：</SPAN></Font></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户编号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商家订单号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;财付通订单号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;银行订单号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;银行授权号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;交易时间:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;交易金额:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;交易卡种:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;卡号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;交易状态:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;退款状态:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;退款金额:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;拒付状态:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
				</TR>
                 <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户处理状态:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;拒付退款金额:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
                
            </TABLE>
            <TABLE id="tableUser" runat="server" cellSpacing="1" cellPadding="0" style="Z-INDEX: 104; position:relative;top:20px;LEFT: 5.02%; WIDTH: 85%; "  bgColor="black" border="0">
                <TR>
					<TD bgColor="#eeeeee" colSpan="4" height="18"><Font color="red"><SPAN>&nbsp;持卡人信息：</SPAN></Font></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;持卡人姓名:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="bb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;邮箱:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="bb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;电话:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="bb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;IP来源:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="bb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;地址:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="bb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;邮编:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="bb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="bb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="bb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
            </TABLE>
		</form>
	</body>
</HTML>
