<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="SuspendRefundment.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.SuspendRefundment" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SuspendRefundment</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script language="javascript">					
			function select_deselectAll (chkVal, idVal) 
			{
				var frm = document.forms[0];
				for (i=0; i<frm.length; i++) 
				{
					if (idVal.indexOf ('CheckAll') != -1)
					{
						if(frm.elements[i].id.indexOf('CheckBox') != -1)
						{
							if(chkVal == true) 
							{
								frm.elements[i].checked = true;
							} 
							else 
							{
								frm.elements[i].checked = false;
							}
						}
					} 
					else if (idVal.indexOf('DeleteThis') != -1) 
					{
						if(frm.elements[i].checked == false) 
						{
							frm.elements[1].checked = false;
						}
					}
				}
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1"
				cellPadding="1" width="85%" border="1">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT face="宋体" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;商户快速交易退款撤销操作(提起申请后数据就不会汇总到退款数据中)</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label4" runat="server">交易单ID</asp:label></TD>
					<TD style="WIDTH: 325px"><asp:textbox id="tbTransID" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right">
						<asp:Label id="Label11" runat="server">输入商户</asp:Label></TD>
					<TD><asp:textbox id="tbSPID" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">开始日期</asp:label></TD>
					<TD style="WIDTH: 325px"><FONT face="宋体">
                        <asp:textbox id="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
                        </FONT></TD>
					<TD align="right"><asp:label id="Label3" runat="server">结束日期</asp:label></TD>
					<TD>
                        <asp:textbox id="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
					</TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label8" runat="server">银行类型</asp:label></TD>
					<TD style="WIDTH: 325px">
						<asp:DropDownList id="ddlBankType" runat="server" Width="152px"></asp:DropDownList></TD>
					<TD align="right"><asp:label id="Label6" runat="server">业务状态</asp:label></TD>
					<TD><asp:dropdownlist id="ddlStatus" runat="server" Width="152px">
							<asp:ListItem Value="99">所有状态</asp:ListItem>
							<asp:ListItem Value="1">待审批</asp:ListItem>
							<asp:ListItem Value="2">审批中</asp:ListItem>
							<asp:ListItem Value="3">审批失败</asp:ListItem>
							<asp:ListItem Value="4">退款成功</asp:ListItem>
							<asp:ListItem Value="5">退款失败</asp:ListItem>
							<asp:ListItem Value="6">资料重填</asp:ListItem>
							<asp:ListItem Value="7">转入代发</asp:ListItem>
							<asp:ListItem Value="8">暂不处理</asp:ListItem>
							<asp:ListItem Value="9">退款流程中</asp:ListItem>
							<asp:ListItem Value="10">转入代发成功</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label10" runat="server">提现单ID</asp:label></TD>
					<TD style="WIDTH: 325px"><asp:textbox id="tbDrawID" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label7" runat="server">买家帐号</asp:label></TD>
					<TD><asp:textbox id="tbBuyerID" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label5" runat="server">汇总状态</asp:label></TD>
					<TD style="WIDTH: 325px">
						<asp:DropDownList id="ddlSumType" runat="server" Width="152px">
							<asp:ListItem Value="99" Selected="True">所有状态</asp:ListItem>
							<asp:ListItem Value="0">尚未汇总</asp:ListItem>
							<asp:ListItem Value="1">已经汇总</asp:ListItem>
							<asp:ListItem Value="2">暂停退款汇总</asp:ListItem>
						</asp:DropDownList></TD>
					<td align="right">
						<asp:label style="Z-INDEX: 0" id="Label9" runat="server">退款类型</asp:label></td>
					<TD>
						<asp:dropdownlist style="Z-INDEX: 0" id="ddlrefund_type" runat="server" Width="152px">
							<asp:ListItem Value="3">银行直接退款</asp:ListItem>
							<asp:ListItem Value="9">充值单退款</asp:ListItem>
						</asp:dropdownlist>
						<asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
						<asp:Button id="btnSuspend" runat="server" Text="申请撤销" onclick="btnSuspend_Click"></asp:Button></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; POSITION: absolute; WIDTH: 85%; TOP: 40%; LEFT: 5.02%"
				cellSpacing="1" cellPadding="1" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" PageSize="50">
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<Columns>
								<asp:HyperLinkColumn DataNavigateUrlField="FSpID" DataNavigateUrlFormatString="../BaseAccount/ModifyMedi.aspx?fqqid={0}"
									DataTextField="FSpID" HeaderText="SPID"></asp:HyperLinkColumn>
								<asp:TemplateColumn HeaderText="交易单">
									<ItemTemplate>
										<a href='<%#  GetTransUrl(DataBinder.Eval(Container, "DataItem.Ftransaction_id").ToString()) %>' >
											<%# DataBinder.Eval(Container, "DataItem.Ftransaction_id") %>
										</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Fbuyid" HeaderText="买家帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Frp_feeName" HeaderText="退买家金额" DataFormatString="{0:N}"></asp:BoundColumn>
								<asp:BoundColumn DataField="Frb_feeName" HeaderText="退卖家费用"></asp:BoundColumn>
								<asp:BoundColumn DataField="Frefund_typeName" HeaderText="退款类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstatusName" HeaderText="业务状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftrue_name" HeaderText="帐户名称"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Ftransaction_id"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Fstatus"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="选择">
									<HeaderTemplate>
										<INPUT id="CheckAll" onclick="return select_deselectAll(this.checked,this.id)" type="checkbox"><LABEL>选择</LABEL>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:CheckBox id="CheckBox1" runat="server" Text="选择" Visible="False"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="详细">
									<ItemTemplate>
										<a href='<%# String.Format("../TradeManage/B2cReturnQuery_Detail.aspx?tranid={0}&drawid={1}", DataBinder.Eval(Container, "DataItem.Ftransaction_id").ToString()
										, DataBinder.Eval(Container, "DataItem.Fdraw_id").ToString()) %>' >详细 </a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="Frefund_type"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Fdraw_id" HeaderText="Fdraw_id"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="fstandby1" HeaderText="fstandby1"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
				<TR>
					<TD align="center">
						<asp:Label id="Label12" runat="server"></asp:Label>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
