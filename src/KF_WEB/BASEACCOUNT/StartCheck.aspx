<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="StartCheck.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.StartCheck" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>StartCheck</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style5 { COLOR: #000000 }
	.style6 { COLOR: #ff0000 }
		</style>
	</HEAD>
	<body background="../IMAGES/Page/bg01.gif">
		<form id="Form1" method="post" runat="server">
			<FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT>
			<br>
			<table height="20" cellSpacing="1" cellPadding="0" width="95%" align="center" bgColor="#666666"
				border="0">
				<tr bgColor="#e4e5f7">
					<td vAlign="middle" height="20">
						<table height="90%" cellSpacing="0" cellPadding="1" width="100%" border="0">
							<tr>
								<td style="WIDTH: 746px" width="746" background="../IMAGES/Page/bg_bl.gif" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
									</font><span class="style5"><span class="style6">
											<asp:linkbutton id="lkUnchecked" runat="server" CausesValidation="False" ForeColor="Red">&#24453;&#23457;&#25209;&#30340;&#20219;&#21153;</asp:linkbutton></span>|&nbsp;
										<asp:linkbutton id="lkChecked" runat="server">&#24050;&#23457;&#25209;&#30340;&#20219;&#21153; </asp:linkbutton>|</span></td>
								<td width="370" background="../IMAGES/Page/bg_bl.gif">
									<DIV align="right">��������:<asp:dropdownlist id="dlCheckType" runat="server" AutoPostBack="True" Width="120px">
											<asp:ListItem Value="0">������������</asp:ListItem>
										</asp:dropdownlist>&nbsp;
										<asp:linkbutton id="lkShow" runat="server" ForeColor="Red" Visible="False">- ����</asp:linkbutton>&nbsp;&nbsp; 
										&nbsp;
									</DIV>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td>
						<DIV align="left"><asp:datagrid id=dgCheck runat="server" Width="100%" PageSize="12" AutoGenerateColumns="False" DataSource="<%# dtcheck %>" Height="0px">
								<ItemStyle Wrap="False"></ItemStyle>
								<HeaderStyle Wrap="False" BackColor="#EEEEEE"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FStartTime" HeaderText="����ʱ��">
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="ID��">
										<ItemTemplate>
											<asp:LinkButton id="linkStr" runat="server" OnCommand="detailPage" Visible=false CommandName = '<%# DataBinder.Eval(Container, "DataItem.FCheckType") %>' CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Fobjid") %>'>
												<%# DataBinder.Eval(Container, "DataItem.Fobjid") %>
											</asp:LinkButton>
											<asp:LinkButton id="Linkbutton2" runat="server" OnCommand="detailCheck" CommandName = '<%# DataBinder.Eval(Container, "DataItem.FCheckType") %>' CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FID") %>'>
												<%# DataBinder.Eval(Container, "DataItem.Fobjid") %>
											</asp:LinkButton>
											</asp:LinkButton>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:HyperLinkColumn Visible="False" DataNavigateUrlField="fobjid" DataNavigateUrlFormatString="../TradeManage/TradeLogQuery.aspx?id={0}"
										DataTextField="Fobjid" HeaderText="ID��"></asp:HyperLinkColumn>
									<asp:HyperLinkColumn DataTextField="FStartUser" HeaderText="�ύ��"></asp:HyperLinkColumn>
									<asp:HyperLinkColumn DataTextField="FTypeName" HeaderText="��������"></asp:HyperLinkColumn>
									<asp:BoundColumn DataField="FCheckMoney" HeaderText="���(Ԫ)">
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="�������">
										<ItemTemplate>
											<% if (sign == "checked") { %>
											<asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FCheckResult") %>'>
											</asp:Label>
											<% }%>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="FState" HeaderText="��ǰ״̬">
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="��ϸ">
										<ItemTemplate>
											<FONT face="����">
												<asp:LinkButton id="LinkButton1" runat="server" OnCommand="detailCheck" CommandName = '<%# DataBinder.Eval(Container, "DataItem.FCheckType") %>' CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FID") %>'>�쿴��ϸ</asp:LinkButton>
											</FONT>
										</ItemTemplate>
									</asp:TemplateColumn>
									<%--<asp:TemplateColumn Visible="False" HeaderText="ִ��״̬">
										<ItemTemplate>
											<%# covert(DataBinder.Eval(Container, "DataItem.Fstate").ToString())%>
											<%if (exedSign == "No") {%>
											<asp:LinkButton id="Linkbutton3" runat="server" OnCommand="executeTask" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FID") %>'>���ִ��</asp:LinkButton>
											<% } %>
										</ItemTemplate>
									</asp:TemplateColumn>--%>
								</Columns>
							</asp:datagrid></DIV>
						<asp:label id="lbTask" runat="server" ForeColor="DarkGray" Visible="False">Label</asp:label><webdiyer:aspnetpager id="pager" runat="server" Width="100%" Visible="True" PageSize="12" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="ת��" OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" ShowCustomInfoSection="left" NumericButtonCount="8"></webdiyer:aspnetpager></td>
				</tr>
			</table>
			<br>
			<table cellSpacing="1" cellPadding="0" width="95%" align="center" bgColor="#666666" border="0">
				<TBODY>
					<tr bgColor="#e4e5f7">
						<td vAlign="middle" height="20">
							<table height="90%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<tr>
									<td width="80%" background="../IMAGES/Page/bg_bl.gif" height="18">
										<P><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20"></font><span class="style5">������ϸ</span></P>
									</td>
									<td width="20%" background="../IMAGES/Page/bg_bl.gif">
										<div align="center"></div>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr bgColor="#ffffff">
						<td height="12"><iframe id=iframe0 name=iframe0 
      marginWidth=0 marginHeight=0 src="<%=iFramePath%>" frameBorder=0 
      width="100%" scrolling=auto height="<%=iFrameHeight%>" 
      > </iframe>
						</td>
					</tr>
				</TBODY>
			</table>
			<p>
				<TABLE id="Table1" cellSpacing="1" cellPadding="0" width="95%" align="center" bgColor="#666666"
					border="0">
					<TR bgColor="#e4e5f7">
						<TD vAlign="middle" height="20">
							<TABLE id="Table2" height="90%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<TR>
									<TD style="WIDTH: 746px" width="746" background="../IMAGES/Page/bg_bl.gif" height="18"><FONT color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										</FONT><span class="style5">������¼</span></TD>
									<TD width="370" background="../IMAGES/Page/bg_bl.gif">
										<DIV align="center"><FONT face="����"></FONT>&nbsp;</DIV>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR bgColor="#ffffff">
						<TD>
							<DIV align="left"><asp:datagrid id=dgCheckLog runat="server" Width="100%" AutoGenerateColumns="False" DataSource="<%# dtLog %>" Height="0px">
									<ItemStyle Wrap="False"></ItemStyle>
									<HeaderStyle Wrap="False" BackColor="#EEEEEE"></HeaderStyle>
									<Columns>
										<asp:BoundColumn DataField="FCheckID" HeaderText="����ID">
											<ItemStyle Wrap="False"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="FCheckTime" HeaderText="����ʱ��">
											<ItemStyle Wrap="False"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="FCheckUser" HeaderText="������">
											<ItemStyle Wrap="False"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="FCheckMemo" HeaderText="�������">
											<ItemStyle Wrap="False"></ItemStyle>
										</asp:BoundColumn>
										<asp:TemplateColumn HeaderText="�Ƿ�ͬ��">
											<HeaderStyle Wrap="False"></HeaderStyle>
											<ItemStyle Wrap="False"></ItemStyle>
											<ItemTemplate>
												<asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FCheckResult") %>'>
												</asp:Label>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:datagrid></DIV>
							<FONT face="����">&nbsp;</FONT>
							<asp:label id="Label2" runat="server" ForeColor="Red" Visible="False">Label</asp:label></TD>
					</TR>
				</TABLE>
				<br>
				<TABLE id="Table3" cellSpacing="1" cellPadding="0" width="95%" align="center" bgColor="#666666"
					border="0">
					<TR bgColor="#e4e5f7">
						<TD vAlign="middle" height="20">
							<TABLE id="Table4" height="90%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<TR>
									<TD width="80%" background="../IMAGES/Page/bg_bl.gif" height="18"><FONT color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										</FONT><SPAN class="style5">�ҷ��������</SPAN></TD>
									<TD width="20%" background="../IMAGES/Page/bg_bl.gif">
										<DIV align="center"></DIV>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR bgColor="#ffffff">
						<TD height="12">
							<DIV align="center"><asp:label id="lbInfo" runat="server" ForeColor="DarkGreen" Width="99%" Height="29px" Font-Size="Medium"></asp:label></DIV>
						</TD>
					</TR>
				</TABLE>
			</p>
			<br>
		</form>
	</body>
</HTML>
