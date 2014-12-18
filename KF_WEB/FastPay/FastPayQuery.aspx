<%@ Page language="c#" Codebehind="FastPayQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FastPay.FastPayQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FastPayQuery</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js">
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
			<TABLE id="Table1" style="POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1" cellPadding="1"
				width="800" border="1">
				<TR>
					<TD style="WIDTH: 800px" bgColor="#e4e5f7" colSpan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;一点通业务</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></TD>
				</TR>
				<asp:panel id="PanelList" runat="server">
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label2" runat="server">财付通账号</asp:label></TD>
						<TD width="250">
							<asp:textbox id="txtQQ" runat="server"></asp:textbox></TD>
						<TD width="150" align="right">
							<asp:label id="Label3" runat="server">内部ID</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_uid" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label17" runat="server">银行类型</asp:label></TD>
						<TD width="250">
							<asp:DropDownList id="ddl_BankType" runat="server">
								<asp:ListItem Value="" Selected="True">全部</asp:ListItem>
								<asp:ListItem Value="2001">招行一点通</asp:ListItem>
								<asp:ListItem Value="2002">工行一点通</asp:ListItem>
								<asp:ListItem Value="3001">兴业信用卡</asp:ListItem>
								<asp:ListItem Value="3002">中行信用卡</asp:ListItem>
							</asp:DropDownList></TD>
						<TD width="150" align="right">
							<asp:label id="Label18" runat="server">银行卡号</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_bankID" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label19" runat="server">证件类型</asp:label></TD>
						<TD width="250">
							<asp:DropDownList id="ddl_creType" runat="server">
								<asp:ListItem Value="" Selected="True">全部</asp:ListItem>
								<asp:ListItem Value="1">身份证</asp:ListItem>
								<asp:ListItem Value="2">护照</asp:ListItem>
								<asp:ListItem Value="3">军官证</asp:ListItem>
							</asp:DropDownList></TD>
						<TD width="150" align="right">
							<asp:label id="Label20" runat="server">证件号</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_creID" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label21" runat="server">协议号</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_serNum" runat="server"></asp:textbox></TD>
						<TD width="150" align="right">
							<asp:label id="Label22" runat="server">手机号码</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_phone" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label23" runat="server">起始日期</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_beginDate" runat="server"></asp:textbox>
							<asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
						<TD width="150" align="right">
							<asp:label id="Label24" runat="server">结束日期</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_endDate" runat="server"></asp:textbox>
							<asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center"><FONT face="宋体">
								<asp:button id="btnSearch" runat="server" Width="80px" Text="查 询"></asp:button></FONT></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<asp:datagrid id="dgList" runat="server" Width="100%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1"
								BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Findex" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fbind_serialno" HeaderText="绑定序列号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fuin" HeaderText="财付通账号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_typeStr" HeaderText="银行类型">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fprotocol_no" HeaderText="协议编号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_statusStr" HeaderText="银行绑定状态">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcard_tail" HeaderText="银行卡后四位">
										<HeaderStyle Wrap="False"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Ftruename" HeaderText="银行卡账户名">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="详细">
										<ItemTemplate>
											<a href='./BankCardUnbind.aspx?type=edit&Findex=<%# DataBinder.Eval(Container, "DataItem.Findex")%>&Fuid=<%# DataBinder.Eval(Container, "DataItem.Fuid")%>'>
												详细</a>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:datagrid>
							<WEBDIYER:ASPNETPAGER id="pager" runat="server" HorizontalAlign="right" NumericButtonCount="10" PagingButtonSpacing="0"
								ShowInputBox="always" CssClass="mypager" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"
								ShowCustomInfoSection="left" AlwaysShow="True"></WEBDIYER:ASPNETPAGER></WEBDIYER:ASPNETPAGER></TD>
					</TR>
				</asp:panel>
				<asp:panel id="PanelMod" runat="server" HorizontalAlign="Center" Visible="False">
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label6" runat="server">财付通账号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFuin" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label8" runat="server">银行类型</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_type" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label4" runat="server">绑定序列号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_serialno" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label5" runat="server">协议编号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFprotocol_no" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label7" runat="server">银行绑定状态</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_status" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label10" runat="server">银行卡后四位</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFcard_tail" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label9" runat="server">银行卡账户名</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFtruename" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label12" runat="server">关联类型</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_type" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label14" runat="server">有效标识</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_flag" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label15" runat="server">关联的银行卡号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_id" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label11" runat="server">关联状态</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_status" runat="server"></asp:label></TD>
						<TD>
							<asp:Label id="lblFindex" Visible="False" Runat="server"></asp:Label></TD>
						<TD>
							<asp:Label id="lblFuid" Visible="False" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label25" runat="server">证件类型</asp:label></TD>
						<TD width="250">
							<asp:label id="lblcreType" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label16" runat="server">证件号码</asp:label></TD>
						<TD width="250">
							<asp:label id="lblCreID" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label26" runat="server">手机号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblPhone" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label28" runat="server">内部ID</asp:label></TD>
						<TD width="250">
							<asp:label id="lblUid" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label13" runat="server">备注</asp:label></TD>
						<TD colSpan="3">
							<asp:TextBox id="txtFmemo" Width="100%" Runat="server" TextMode="MultiLine"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<asp:Button id="btnUnbind" Text="解除绑定" Runat="server" Enabled="False"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Button id="btnCancel" Text=" 取  消 " Runat="server"></asp:Button></TD>
					</TR>
				</asp:panel>
			</TABLE>
		</form>
	</body>
</HTML>
