<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryUserControledFinPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.QueryUserControledFinPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryUserControledFinPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
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
			<table id="table1" border="1" cellSpacing="1" cellPadding="1" width="900" runat="server">
				<tr>
					<td width="50%"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><label class="style3">用户受控资金信息查询</label></td>
					<td><label class="style3">操作员ID：</label><asp:label id="lb_operatorID" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td colSpan="2"><asp:label id="Label1" runat="server" Width="80" Font-Size="15px">帐号：</asp:label><asp:textbox id="tbx_acc" Width="250px" Runat="server"></asp:textbox>
					</td>
				</tr>
				<%--<tr>
					<td colspan="3">
						<div style="MARGIN:0px 50px 0px 0px">
							<label>查询起始时间：</label>
							<asp:TextBox Runat="server" ID="tbx_beginDate" Width="120"></asp:TextBox>
							<asp:imagebutton id="btnBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
							<label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label> <label>查询结束时间：</label>
							<asp:TextBox Runat="server" ID="tbx_endDate" Width="120"></asp:TextBox>
							<asp:imagebutton id="btnEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
						</div>
					</td>
				</tr>--%>
				<tr>
					<td align="center" colspan="2">
						<span style="MARGIN:0px 50px 0px 0px">
							<asp:Button Runat="server" ID="btn_query" Text="查 询" Width="80" onclick="btn_query_Click"></asp:Button>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button Runat="server" ID="btn_removeAll" Text="解绑所有子账户余额" Width="150" onclick="btn_removeAll_Click"></asp:Button>
						</span>
					</td>
				</tr>
			</table>
			<table border="0" cellSpacing="0" cellPadding="0" width="900">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="900px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            OnItemDataBound="DataGrid1_ItemDataBound">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fcur_typeName" HeaderText="资金来源的子账户类型" FooterStyle-HorizontalAlign="Center">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FbalanceStr" HeaderText="受控金额">
									<HeaderStyle Width="100px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FlstateName" HeaderText="物理状态">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="FtypeText" HeaderText="类型">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
                                 <asp:BoundColumn DataField="cur_type" HeaderText="类型编码">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
                               <asp:TemplateColumn>
									    <ItemTemplate>
										    <asp:Button id="removeButton" Visible="false" runat="server" CommandName="remove" Text="解除"></asp:Button>
									    </ItemTemplate>
								   </asp:TemplateColumn>
                                 <asp:BoundColumn DataField="uid" HeaderText="uid" Visible="false">
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="balance" HeaderText="受控金额 分" Visible="false">
								</asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
                       <%-- <webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager>--%>

					</TD>
				</TR>
			</table>
			<hr>
			<br>
			<br>
			<div runat="server" id="div_detail" style="DISPLAY:none">
				<span><label style="FONT-SIZE:15px">帐号：</label><asp:Label Runat="server" ID="lb_queryAcc" Font-Size="15px">帐号</asp:Label></span>
				<table id="table2" border="2" cellSpacing="1" cellPadding="1" width="1100" runat="server">
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">认证方式：</label></td>
						<td><asp:Label Runat="server" ID="lb_c1" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">认证状态(银行打款状态)：</label></td>
						<td><asp:Label Runat="server" ID="lb_c2" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">身份认证状态：</label></td>
						<td><asp:Label Runat="server" ID="Label3" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">证件类型：</label></td>
						<td><asp:Label Runat="server" ID="lb_c3" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">证件号：</label></td>
						<td><asp:Label Runat="server" ID="lb_c4" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">银行类型：</label></td>
						<td><asp:Label Runat="server" ID="lb_c5" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">银行卡号：</label></td>
						<td><asp:Label Runat="server" ID="lb_c6" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">第一次通过认证的帐号：</label></td>
						<td><asp:Label Runat="server" ID="lb_c7" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">主授权ID：</label></td>
						<td><asp:Label Runat="server" ID="Label4" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">金额确认次数：</label></td>
						<td><asp:Label Runat="server" ID="lb_c10" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">证件修改次数：</label></td>
						<td><asp:Label Runat="server" ID="lb_c11" Font-Size="14"></asp:Label></td>
					</tr>
					<tr>
						<td width="25%"><label style="FONT-SIZE:14px">银行卡修改次数：</label></td>
						<td><asp:Label Runat="server" ID="lb_c12" Font-Size="14"></asp:Label></td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
