<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="AirTicketsOrderQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TravelPlatform.AirTicketsOrderQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AirTicketsOrderQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
        <script language="javascript">
        	    function openModeBegin() {
        	        var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.TextBoxBeginDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
        	        if (returnValue != null) Form1.TextBoxBeginDate.value = returnValue;
        	    }
		</script>
		<script language="javascript">
		    function openModeEnd() {
		        var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.TextBoxEndDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
		        if (returnValue != null) Form1.TextBoxEndDate.value = returnValue;
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="820" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="2"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;机票订单查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">按订单号、票号查询：</asp:label></TD>
					<TD align="left">
                       &nbsp; &nbsp; <asp:label id="Label3" runat="server">票源订单号：</asp:label>
                        <asp:textbox id="TextSppreno" runat="server"></asp:textbox>
                        <asp:label id="Label24" runat="server">票号：</asp:label>
                        <asp:textbox id="TextTicketno" runat="server"></asp:textbox>
                        <asp:label id="Label25" runat="server">财付通交易单号：</asp:label>
					    <asp:textbox id="TextTransaction_id" runat="server"></asp:textbox>
                   </TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label4" runat="server">按乘机人信息查询：</asp:label></TD>
					<TD align="left">
                       &nbsp; &nbsp; <asp:label id="Label21" runat="server">姓名：</asp:label>
                        <asp:textbox id="TextPassenger_name" runat="server"></asp:textbox>
                        <asp:label id="Label26" runat="server">证件号码：</asp:label>
                        <asp:textbox id="TextCert_id" runat="server"></asp:textbox>
                   </TD>
				</TR>
                <TR>
					<TD align="right"><asp:label id="Label27" runat="server">按联系人信息查询：</asp:label></TD>
					<TD align="left">
                        &nbsp; &nbsp;<asp:label id="Label28" runat="server">姓名：</asp:label>
                        <asp:textbox id="TextName" runat="server"></asp:textbox>
                        <asp:label id="Label29" runat="server">手机号码：</asp:label>
                        <asp:textbox id="TextMobile" runat="server"></asp:textbox>
                   </TD>
				</TR>
                <TR>
					<TD align="right"><asp:label id="Label30" runat="server">按财付通账号查询：</asp:label></TD>
					<TD align="left">
                       &nbsp; &nbsp; <asp:textbox id="TextUin" runat="server"></asp:textbox>
                   </TD>
				</TR>
                 <TR>
					<TD align="right"><asp:label id="Label31" runat="server">保单号查询：</asp:label></TD>
					<TD align="left">
                        &nbsp; &nbsp;<asp:textbox id="TextInsur_no" runat="server"></asp:textbox>
                   </TD>
				</TR>
                 <TR>
					<TD align="right"><asp:label id="Label32" runat="server">订购日期：</asp:label></TD>
					<TD align="left">
                        &nbsp; &nbsp;<asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton>
                       到
                       <asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton>
                       &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:label id="Label33" runat="server">订单状态：</asp:label>
                       <asp:dropdownlist id="ddlState" runat="server" Width="152px">
							<asp:ListItem Value="all" Selected="True">全部订单</asp:ListItem>
							<asp:ListItem Value="payed">已支付</asp:ListItem>
							<asp:ListItem Value="unpay">已占座未支付</asp:ListItem>
							<asp:ListItem Value="invalid">作废状态</asp:ListItem>
							<asp:ListItem Value="pay_noticket">已支付未出票</asp:ListItem>
							<asp:ListItem Value="pay_ticket">已支付已出票</asp:ListItem>
							<asp:ListItem Value="refund_ticket">退票</asp:ListItem>
						</asp:dropdownlist>
                   </TD>
				</TR>
                <TR>
                   <TD colSpan="2" align="center"><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 10px; OVERFLOW: auto; WIDTH:95%; POSITION: absolute; TOP: 300px;">
				<table cellSpacing="0" cellPadding="0" width="100%" border="1">
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="listid" HeaderText="订单编号">
										<HeaderStyle Width="200px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="uin" HeaderText="财付通账号">
									    <HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="sppreno" HeaderText="票源订单号">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="transaction_id" HeaderText="财付通交易单号">
										<HeaderStyle Width="170px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="book_time" HeaderText="订购时间">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="pay_time" HeaderText="支付时间">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="from" HeaderText="航段">
										<HeaderStyle Width="50px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="flight_no" HeaderText="航班号">
									    <HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="cabin" HeaderText="舱位">
										<HeaderStyle Width="50px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="dtime" HeaderText="起飞时间">
									    <HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="total_money_str" HeaderText="金额合计">
										<HeaderStyle Width="200px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="strSp_code" HeaderText="供应商">
									    <HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="strTrade_state" HeaderText="订单状态">
									    <HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
								<%--	<asp:ButtonColumn Text="查看" CommandName="Select">
										<HeaderStyle Width="30px"></HeaderStyle>
									</asp:ButtonColumn>--%>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
                    <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
							 HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
				</table>
			</div>
		</form>
	</body>
</HTML>
