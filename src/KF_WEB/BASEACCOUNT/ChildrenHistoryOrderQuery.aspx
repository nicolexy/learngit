<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="ChildrenHistoryOrderQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ChildrenHistoryOrderQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ComplainBussinessInput</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;历史订单查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				
                <TR>
                    <TD align="right"><asp:label id="Label4" runat="server">财付通账号：</asp:label></TD>
					<TD>
						<asp:textbox id="tbCft" runat="server"></asp:textbox>
                    </TD>
                    <TD align="right"><asp:label id="Label5" runat="server">账户类型：</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlType" runat="server" Width="152px">
							<asp:ListItem Value="80">游戏</asp:ListItem>
                            <asp:ListItem Value="81">积分</asp:ListItem>
                            <asp:ListItem Value="82">直通车</asp:ListItem>
						</asp:DropDownList>
                    </TD>
                </TR>
                <TR>
                    <TD align="right"><asp:label id="Label2" runat="server">开始日期：</asp:label></TD>
                    <TD>
                        <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                    </TD>
                    <TD align="right">
                        <asp:label id="Label3" runat="server">结束日期：</asp:label></TD>
                    <td>
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                    </td>
				</TR>	
				<TR>
					<TD align="center" colspan="4">
                    <asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 154px; HEIGHT: 30%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
                <TR>
					<TD bgColor="#e4e5f7" height="18"><SPAN>&nbsp;用户资金流水</SPAN></TD>
				</TR>
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" EnableViewState="False">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="FListid" HeaderText="单据ID号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Faction_type_str" HeaderText="动作类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fuid" HeaderText="内部帐号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fqqid" HeaderText="帐户号码"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcurtype_str" HeaderText="币种类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftype_str" HeaderText="交易类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsubject_str" HeaderText="类别/科目"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftrue_name" HeaderText="用户名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fpaynum_str" HeaderText="金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbalance_str" HeaderText="帐户余额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffrom_uid" HeaderText="对方内部帐号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffromid" HeaderText="对方帐号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffrom_name" HeaderText="对方名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fspid" HeaderText="机构代码(发起者)"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fprove" HeaderText="记帐凭证"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fapplyid" HeaderText="应用系统的ID"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fip" HeaderText="客户的IP地址"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmemo" HeaderText="备注/说明"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_time_acc" HeaderText="帐务时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fvs_qqid" HeaderText="交易对方的帐号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fexplain" HeaderText="交易单备注"></asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
            
		</form>
	</body>
</HTML>
